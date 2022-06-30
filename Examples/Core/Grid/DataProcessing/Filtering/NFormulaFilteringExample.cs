using Nevron.Nov.Data;
using Nevron.Nov.Dom;
using Nevron.Nov.Grid;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Grid
{
    public class NFormulaFilteringExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NFormulaFilteringExample()
        {
        }
        /// <summary>
        /// Static constructor.
        /// </summary>
        static NFormulaFilteringExample()
        {
            NFormulaFilteringExampleSchema = NSchema.Create(typeof(NFormulaFilteringExample), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            // create a view and get its grid
            NTableGridView view = new NTableGridView();
            NTableGrid grid = view.Grid;
            
            // bind to sales data source
            NDataSource dataSource = NDummyDataSource.CreateCompanySalesDataSource();
            grid.DataSource = dataSource;

            // create an expression filter rule that matches records for which Company is equal to Leka
            string companyFxName = dataSource.CreateFormulaFieldName("Company");
            string expression1 = companyFxName + "==\"" + NDummyDataSource.RandomCompanyName() + "\"";
            grid.FilteringRules.Add(new NFilteringRule(new NFormulaRowCondition(expression1)));

            // create an expression filter rule that matches records for which Sales is larger than 1000
            string salesFxName = dataSource.CreateFormulaFieldName("Sales");
            string expression2 = salesFxName + ">1000";
            grid.FilteringRules.Add(new NFilteringRule(new NFormulaRowCondition(expression2)));
            
            grid.AllowSortColumns = true;
            grid.AlternatingRows = true;
            grid.RowHeaders.Visible = true;

            return view;
        }
        protected override NWidget CreateExampleControls()
        {
            return null;
        }
        protected override string GetExampleDescription()
        {
            return @"<p>
Demonstrates formula filtering as well as multiple filter conditions.
</p>
<p>
In this example we have used formula row conditions to specify the predicates that pass only records for a specific Company, for which the <b>Sales</b> are greater than 1000.
You can use formula filtering to define complex filter rules.
</p>
";
        }

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NFormulaFilteringExampleSchema.
        /// </summary>
        public static readonly NSchema NFormulaFilteringExampleSchema;

        #endregion
    }
}