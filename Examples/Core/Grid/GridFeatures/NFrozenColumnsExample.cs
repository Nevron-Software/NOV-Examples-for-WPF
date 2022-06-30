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
    public class NFrozenColumnsExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NFrozenColumnsExample()
        {
        }
        /// <summary>
        /// Static constructor.
        /// </summary>
        static NFrozenColumnsExample()
        {
            NFrozenColumnsExampleSchema = NSchema.Create(typeof(NFrozenColumnsExample), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            m_GridView = new NTableGridView();
            m_GridView.Grid.DataSource = NDummyDataSource.CreatePersonsOrdersDataSource();

            // create a total column that is pinned to the right
            // add an event calculated column of type Double
            NCustomCalculatedColumn<Double> totalColumn = new NCustomCalculatedColumn<Double>();
            totalColumn.Title = "Total";
            totalColumn.FreezeMode = ENColumnFreezeMode.Right;
            totalColumn.GetRowValueDelegate += delegate(NCustomCalculatedColumnGetRowValueArgs<double> arg)
            {
                // calculate a RowValue for the RowIndex
                double price = Convert.ToDouble(arg.DataSource.GetValue(arg.RowIndex, "Price"));
                double quantity = Convert.ToDouble(arg.DataSource.GetValue(arg.RowIndex, "Quantity"));
                return (double)(price * quantity);
            };
            totalColumn.Format.BackgroundFill = new NColorFill(NColor.SeaShell);
            m_GridView.Grid.Columns.Add(totalColumn);

            // freeze the pruduct name to the left
            NColumn productNameColumn = m_GridView.Grid.Columns.GetColumnByFieldName("Product Name");
            productNameColumn.Format.BackgroundFill = new NColorFill(NColor.SeaShell);
            productNameColumn.FreezeMode = ENColumnFreezeMode.Left;

            return m_GridView;
        }
        protected override NWidget CreateExampleControls()
        {
            NStackPanel pstack = new NStackPanel();
            return pstack;
        }
        protected override string GetExampleDescription()
        {
            return @"
<p>
    Demonstrates <b>Frozen Columns</b>.
</p>
<p>
    Columns can be frozen to the left or right side of the grid window area.
    In this example the <b>Total</b> column is frozen to the right side, while the <b>Product Name</b> column is frozen to the left side.
</p>
";
        }

        #endregion

        #region Fields

        NTableGridView m_GridView;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NFrozenColumnsExample.
        /// </summary>
        public static readonly NSchema NFrozenColumnsExampleSchema;

        #endregion
    }
}