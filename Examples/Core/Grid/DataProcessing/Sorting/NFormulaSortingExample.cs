using System;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Grid;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Grid
{
    public class NFormulaSortingExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NFormulaSortingExample()
        {
        }
        /// <summary>
        /// Static constructor.
        /// </summary>
        static NFormulaSortingExample()
        {
            NFormulaSortingExampleSchema = NSchema.Create(typeof(NFormulaSortingExample), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            // create a view and get its grid
            NTableGridView view = new NTableGridView();
            NTableGrid grid = view.Grid;

            // bind the grid to the data source
            grid.DataSource = NDummyDataSource.CreatePersonsOrdersDataSource();

            // add a custom calculated column of type Double that displays the Total (e.g. Price * Quantity)
            NCustomCalculatedColumn<double> totalColumn = new NCustomCalculatedColumn<double>();
            totalColumn.Title = "Total";
            totalColumn.GetRowValueDelegate += delegate(NCustomCalculatedColumnGetRowValueArgs<double> arg)
            {
                // calculate a RowValue for the RowIndex
                double price = Convert.ToDouble(arg.DataSource.GetValue(arg.RowIndex, "Price"));
                double quantity = Convert.ToDouble(arg.DataSource.GetValue(arg.RowIndex, "Quantity"));
                return (double)(price * quantity);
            };
            totalColumn.Format.BackgroundFill = new NColorFill(NColor.SeaShell);
            grid.Columns.Add(totalColumn);

            // create the sales sorting rule.
            // also subcribe for the create sorting rule event to recreate the rule when users 
            grid.SortingRules.Add(CreateTotalSortingRule(grid));
            totalColumn.CreateSortingRuleDelegate = delegate(NColumn theColumn)
            {
                return CreateTotalSortingRule(grid);
            };

            // alter some view preferences
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
            return @"
<p>
    Demonstrates expression sorting rules in combination with calculated columns.
</p>
<p>
    In this example we have created an <b>NCustomCalculatedColumn</b> that displays the <b>Price</b> * <b>Quantity</b> calculation (e.g. <b>Total</b>).
</p>
<p>
    We have also created a sorting rule that sorts by the <b>Total</b> calculated column in a different way - via a Formula Expression.
</p>
";
        }

        #endregion

        #region Implementation

        NSortingRule CreateTotalSortingRule(NTableGrid grid)
        {
            NColumn salesColumn = grid.Columns.GetColumnByFieldName("Sales");
            NColumn priceColumn = grid.Columns.GetColumnByFieldName("Price");

            // create a sorting rule that sorts by the Total value
            string quantityFieldName = grid.CreateFormulaFieldName("Quantity");
            string priceFieldName = grid.CreateFormulaFieldName("Price");

            NSortingRule sortingRule = NSortingRule.FromFormula(null, priceFieldName + "*" + quantityFieldName, ENSortingDirection.Ascending);
            sortingRule.Column = salesColumn;
            return sortingRule;
        }

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NFormulaSortingExample.
        /// </summary>
        public static readonly NSchema NFormulaSortingExampleSchema;

        #endregion
    }
}