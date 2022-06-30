using System;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Grid;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Grid
{
    public class NColumnAndFieldFilteringExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NColumnAndFieldFilteringExample()
        {
        }
        /// <summary>
        /// Static constructor.
        /// </summary>
        static NColumnAndFieldFilteringExample()
        {
            NColumnAndFieldFilteringExampleSchema = NSchema.Create(typeof(NColumnAndFieldFilteringExample), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            NTableGridView view = new NTableGridView();
            NTableGrid grid = view.Grid;

            // bind the grid to the data source, but exclude the "PersonId" column.
            grid.AutoCreateColumn += delegate(NAutoCreateColumnEventArgs arg)
            {
                if (arg.FieldInfo.Name == "PersonId")
                {
                    arg.DataColumn = null;
                }
            };
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

            // show only records where PersonId = "0"
            // NOTE: when the rule is not associated with a column, you must explicitly specify a row value for the row condition.
            NFilteringRule personIdFilterRule = new NFilteringRule();
            NOperatorRowCondition rowCondition = new NOperatorRowCondition(ENRowConditionOperator.Equals, "0");
            rowCondition.RowValue = new NFieldRowValue("PersonId");
            personIdFilterRule.RowCondition = rowCondition;
            grid.FilteringRules.Add(personIdFilterRule);

            // show only records for which the total column is larger than 150
            // NOTE: when the rule is associated with a column, by default the row condition operates on the column values.
            NFilteringRule companyFilterRule = new NFilteringRule();
            companyFilterRule.Column = totalColumn;
            companyFilterRule.RowCondition = new NOperatorRowCondition(ENRowConditionOperator.GreaterThan, "150");
            grid.FilteringRules.Add(companyFilterRule);
            
            // customize the grid
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
    Demonstrates filtering by field or column values as well as multiple filters.
</p>
<p>
    NOV Grid allows you to create filter rules that work on column and field provided row values. 
    This makes it possible to create filter rules that work on calculated columns (i.e. columns that do not have an associated field in the data source) 
    as well as create filter rules that work on data source fields, regardless of whether they are represented by columns in the grid.
</p>
<p>
    In this example we have create two filter rules.
</br>
    The first filter rule filters records for which the <b>PersonId</b> field is equal to 0. This filter rule is not associated with a column, 
    since the <b>PersonId</b> field is not represented by a column in the grid.
</br>
    The second filter rule filters records for which the <b>Total</b> calculated column is greater than 150. This filter rule is associated with a column,
    which is at the same time row value provider for this filter rule row condition. Note that the <b>Total</b> column does not have an associated field in the data source.
</p>";
        }

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NColumnAndFieldFilteringExample.
        /// </summary>
        public static readonly NSchema NColumnAndFieldFilteringExampleSchema;

        #endregion
    }
}