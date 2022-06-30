using System;
using Nevron.Nov.Grid;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.DataStructures;
using Nevron.Nov.Data;
using Nevron.Nov.Dom;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Grid
{
    public class NCustomCalculatedColumnsExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NCustomCalculatedColumnsExample()
        {
        }
        /// <summary>
        /// Static constructor.
        /// </summary>
        static NCustomCalculatedColumnsExample()
        {
            NCustomCalculatedColumnsExampleSchema = NSchema.Create(typeof(NCustomCalculatedColumnsExample), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            m_GridView = new NTableGridView();
            NTableGrid grid = m_GridView.Grid;

            // bind the grid to the data source
            grid.DataSource = NDummyDataSource.CreatePersonsOrdersDataSource();

            // add an event calculated column of type Double
            NCustomCalculatedColumn<Double> totalColumn = new NCustomCalculatedColumn<Double>();
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

            return m_GridView;
        }
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();
            return stack;
        }
        protected override string GetExampleDescription()
        {
            return @"
<p>
    Demonstrates custom calculated columns.
</p>
<p>
    Custom calculated columns are represented by the <b>NCustomCalculatedColumn</b> class. 
    It exposes a <b>GetRowValueDelegate</b> delegate, which is called whenever the column must provide a value for a specific row.
    Thus it is up to the user to provide a row value for a specific row.
</p>
<p>
    In the example the <b>Total</b> column is a custom calculated column that is calculated as {<b>Price</b>*<b>Quantity</b>}.
</p>";
        }

        #endregion

        #region Fields

        NTableGridView m_GridView;
        

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NCustomCalculatedColumnsExample.
        /// </summary>
        public static readonly NSchema NCustomCalculatedColumnsExampleSchema;

        #endregion
    }
}