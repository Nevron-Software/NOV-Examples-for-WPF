using System;
using Nevron.Nov.Data;
using Nevron.Nov.Dom;
using Nevron.Nov.Grid;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Grid
{
    public class NGroupingSummaryRowsExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NGroupingSummaryRowsExample()
        {
        }
        /// <summary>
        /// Static constructor.
        /// </summary>
        static NGroupingSummaryRowsExample()
        {
            NGroupingSummaryRowsExampleSchema = NSchema.Create(typeof(NGroupingSummaryRowsExample), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            // create a view and get its grid
            NTableGridView view = new NTableGridView();
            NTableGrid grid = view.Grid;

            // customize the grid
            grid.AllowEdit = false;

            // bind to data source, but exclude the "PersonId" field from binding 
            grid.AutoCreateColumn += delegate(NAutoCreateColumnEventArgs arg)
            {
                if (arg.FieldInfo.Name == "PersonId")
                {
                    arg.DataColumn = null;
                }
            };
            grid.DataSource = NDummyDataSource.CreatePersonsOrdersDataSource();

            // create a calculated Total column
            NCustomCalculatedColumn<double> totalColumn = new NCustomCalculatedColumn<double>();
            totalColumn.Title = "Total";
            totalColumn.GetRowValueDelegate = delegate(NCustomCalculatedColumnGetRowValueArgs<double> args)
            {
                double price = Convert.ToDouble(args.DataSource.GetValue(args.RowIndex, "Price"));
                int quantity = Convert.ToInt32(args.DataSource.GetValue(args.RowIndex, "Quantity"));
                return (double)(price * quantity);
            };
            grid.Columns.Add(totalColumn);

            // create a grouping rule that groups by the Product Name column
            NGroupingRule groupingRule = new NGroupingRule(grid.Columns.GetColumnByFieldName("Product Name"));
            
            // create a footer summary row for the total total
            groupingRule.CreateFooterSummaryRowsDelegate = delegate(NGroupingRuleCreateSummaryRowsArgs args)
            {
                // get the recordset for the group
                NRecordset recordset = args.GroupRow.Recordset;

                // calculate the sum of totals
                double total = 0;
                for (int i = 0; i < recordset.Count; i++)
                {
                    total += Convert.ToDouble(totalColumn.GetRowValue(recordset[i])); 
                }

                // create the total summary row
                NSummaryRow totalRow = new NSummaryRow();
                totalRow.Cells = new NSummaryCellCollection();

                NSummaryCell cell = new NSummaryCell();
                cell.BeginXPosition.Mode = ENSpanCellBeginXPositionMode.AnchorToEndX;
                cell.EndXPosition.Mode = ENSpanCellEndXPositionMode.RowEndX;
                cell.Content = new NLabel("Grand Total: " + total.ToString("0.00"));
                totalRow.Cells.Add(cell);

                return new NSummaryRow[] { totalRow };
            };
            grid.GroupingRules.Add(groupingRule);

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
    Demonstrates groups' summary rows and calculated columns.
</p>
<p>
    In this example we have created the <b>Total</b> <b>NCustomCalculatedColumn</b> that shows the <b>Price</b> * <b>Quantity</b> calculation.
</p>
<p>
    Additionally the we have grouped the records by <b>Product Name</b> and created a summary row for each of the groups that displays the <b>Grand Total</b> of the totals contained in the group.
</p>
";
        }

        #endregion

        #region Fields

        

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NGroupingSummaryRowsExample.
        /// </summary>
        public static readonly NSchema NGroupingSummaryRowsExampleSchema;

        #endregion
    }
}