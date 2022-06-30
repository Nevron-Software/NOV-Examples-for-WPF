using Nevron.Nov.Dom;
using Nevron.Nov.Grid;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Grid
{
    public class NMultilevelSortingExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NMultilevelSortingExample()
        {
        }
        /// <summary>
        /// Static constructor.
        /// </summary>
        static NMultilevelSortingExample()
        {
            NMultilevelSortingExampleSchema = NSchema.Create(typeof(NMultilevelSortingExample), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            // create a view and get its grid
            NTableGridView view = new NTableGridView();
            NTableGrid grid = view.Grid;

            // bind the grid to the data source
            grid.DataSource = NDummyDataSource.CreateCompanySalesDataSource();

            // create a sorting rule that sorts by the company column first
            NColumn companyColumn = grid.Columns.GetColumnByFieldName("Company");
            grid.SortingRules.Add(new NSortingRule(companyColumn, ENSortingDirection.Ascending));

            // create a sorting rule that sorts by the sales column next
            NColumn salesColumn = grid.Columns.GetColumnByFieldName("Sales");
            grid.SortingRules.Add(new NSortingRule(salesColumn, ENSortingDirection.Ascending));

            return view;
        }
        protected override NWidget CreateExampleControls()
        {
            return null;
        }
        protected override string GetExampleDescription()
        {
            return @"
<p>
    Demonstrates multilevel grid sorting by columns.
</p>
<p>
    In this example we have sorted in <b>Ascending</b> order first by the <b>Company</b> field and then by the <b>Sales</b> field.
</p>
";
        }

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NColumnSortingExample.
        /// </summary>
        public static readonly NSchema NMultilevelSortingExampleSchema;

        #endregion
    }
}