using Nevron.Nov.Dom;
using Nevron.Nov.Grid;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Grid
{
    public class NFormulaGroupingExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NFormulaGroupingExample()
        {
        }
        /// <summary>
        /// Static constructor.
        /// </summary>
        static NFormulaGroupingExample()
        {
            NFormulaGroupingExampleSchema = NSchema.Create(typeof(NFormulaGroupingExample), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            NTableGridView gridView = new NTableGridView();
            NTableGrid grid = gridView.Grid;

            // bind the grid to the data source
            grid.DataSource = NDummyDataSource.CreateCompanySalesDataSource();

            // create a grouping rule that groups by the company field value first
            // note that in order to indicate the grouping in the grouping panel, the rule must reference the respective column
            NColumn companyColumn = grid.Columns.GetColumnByFieldName("Company");
            string fx1 = grid.CreateFormulaFieldName("Company");
            NFormulaRowValue fxRowValue = new NFormulaRowValue(fx1);
            NGroupingRule groupingRule1 = new NGroupingRule(companyColumn, fxRowValue, ENSortingDirection.Ascending);
            grid.GroupingRules.Add(groupingRule1);

            // create a grouping rule that groups by sales larger than 1000 next
            // note that in order to indicate the grouping in the grouping panel, the rule must reference the respective column
            string fx2 = grid.CreateFormulaFieldName("Sales") + ">1000";
            NColumn salesColumn = grid.Columns.GetColumnByFieldName("Sales");
            NGroupingRule groupingRule2 = NGroupingRule.FromFormula(salesColumn, fx2);

            groupingRule2.CreateGroupRowCellsDelegate += delegate(NGroupingRuleCreateGroupRowCellsArgs arg)
            {
                bool groupValue = (bool)((NVariant)arg.GroupRow.GroupValue);
                string text =  groupValue? "Sales greater than 1000" : "Sales less than or equal to 1000";
                return new NGroupRowCell[] { new NGroupRowCell(text) };
            };
            grid.GroupingRules.Add(groupingRule2);
            
            // alter some view preferences
            grid.AllowSortColumns = true;
            grid.AlternatingRows = true;
            grid.RowHeaders.Visible = true;

            return gridView;
        }

        protected override NWidget CreateExampleControls()
        {
            return null;
        }
        protected override string GetExampleDescription()
        {
            return @"
<p>
    Demonstrates formula row values in combination with grouping rules.
</p>
<p>
    In this example we have created two grouping rules, one that groups by the <b>Company</b> field and another one that groups by the <b>Sales</b> larger than 1000.
    Both conditions have been expressed with instances of the <b>NFormulaRowValue</b>. 
</p>
<p>
    Furthermore the example creates custom group row cells that display the <b>""Sales greater than 1000""</b> or <b>""Sales less than or equal to 1000""</b> for the second grouping rule rows.
</p>
";
        }

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NFormulaGroupingExample.
        /// </summary>
        public static readonly NSchema NFormulaGroupingExampleSchema;

        #endregion
    }
}