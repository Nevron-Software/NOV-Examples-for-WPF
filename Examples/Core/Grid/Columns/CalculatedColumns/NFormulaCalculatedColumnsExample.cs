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
    public class NFormulaCalculatedColumnsExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NFormulaCalculatedColumnsExample()
        {
        }
        /// <summary>
        /// Static constructor.
        /// </summary>
        static NFormulaCalculatedColumnsExample()
        {
            NFormulaCalculatedColumnsExampleSchema = NSchema.Create(typeof(NFormulaCalculatedColumnsExample), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            m_GridView = new NTableGridView();
            NTableGrid grid = m_GridView.Grid;

            // bind the grid to the data source
            grid.DataSource = NDummyDataSource.CreatePersonsOrdersDataSource();

            // add a formula column
            m_TotalColumn = new NFormulaCalculatedColumn();
            m_TotalColumn.Title = "Total";
            string fx = grid.CreateFormulaFieldName("Price") + "*" + grid.CreateFormulaFieldName("Quantity");
            m_TotalColumn.Formula = fx;
            m_TotalColumn.Format.BackgroundFill = new NColorFill(NColor.SeaShell); 
            grid.Columns.Add(m_TotalColumn);

            return m_GridView;
        }
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();

            NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_TotalColumn).CreatePropertyEditors(m_TotalColumn,
                NFormulaCalculatedColumn.FormulaProperty);

            for (int i = 0; i < editors.Count; i++)
            {
                stack.Add(editors[i]);
            }

            return stack;
        }
        protected override string GetExampleDescription()
        {
            return @"
<p>
    Demonstrates formula calculated columns.
</p>
<p>
    Formula calculated columns are columns whose row values are not obtained from the data source, 
    but are dynamically calculated via a user-specified formula, that can reference data source field values.
    Formula calculated columns are represented by the <b>NFormulaCalculatedColumn</b> class.
</p>
<p>
    In the example the <b>Total</b> column is a formula calculated column that is calculated via the {<b>Price</b>*<b>Quantity</b>} formula.
</p>";
        }

        #endregion

        #region Fields

        NTableGridView m_GridView;
        NFormulaCalculatedColumn m_TotalColumn;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NFormulaCalculatedColumnsExample.
        /// </summary>
        public static readonly NSchema NFormulaCalculatedColumnsExampleSchema;

        #endregion
    }
}