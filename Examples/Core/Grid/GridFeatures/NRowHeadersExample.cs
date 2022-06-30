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
    public class NRowHeadersExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NRowHeadersExample()
        {
        }
        /// <summary>
        /// Static constructor.
        /// </summary>
        static NRowHeadersExample()
        {
            NRowHeadersExampleSchema = NSchema.Create(typeof(NRowHeadersExample), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            m_TableView = new NTableGridView();
            m_TableView.Grid.DataSource = NDummyDataSource.CreateCompanySalesDataSource();

            // show the row headers
            m_TableView.Grid.RowHeaders.Visible = true;
            return m_TableView;
        }
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();
            
            // create the row headers properties
            {
                NStackPanel rowHeadersStack = new NStackPanel();

                NDesigner designer = NDesigner.GetDesigner(NRowHeaderCollection.NRowHeaderCollectionSchema);
                NList<NPropertyEditor> editors = designer.CreatePropertyEditors(m_TableView.Grid.RowHeaders,
                    NRowHeaderCollection.VisibleProperty,
                    NRowHeaderCollection.ShowRowNumbersProperty,
                    NRowHeaderCollection.ShowRowSymbolProperty);

                for (int i = 0; i < editors.Count; i++)
                {
                    rowHeadersStack.Add(editors[i]);
                }

                NGroupBox rowHeadersGroup = new NGroupBox("Row Headers Properties", rowHeadersStack);
                stack.Add(new NUniSizeBoxGroup(rowHeadersGroup)); 
            }

            // create the grid properties
            {
                NStackPanel gridStack = new NStackPanel();

                NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_TableView.Grid).CreatePropertyEditors(m_TableView.Grid,
                    NGrid.FrozenRowsProperty,
                    NGrid.IntegralVScrollProperty);

                for (int i = 0; i < editors.Count; i++)
                {
                    gridStack.Add(editors[i]);
                }

                NGroupBox gridGroup = new NGroupBox("Grid Properties", gridStack);
                stack.Add(new NUniSizeBoxGroup(gridGroup));
            }

            return stack;
        }
        protected override string GetExampleDescription()
        {
            return @"
<p>
    Demonstrates the row headers.
</p>
<p>
    Row headers are small button-like elements that you can use to select rows. 
    Besides for selection, row headers indicate the row state (e.g. current or editing status) and can be configured to show the row ordinal in the data source.
</p>
";
        }

        #endregion

        #region Fields

        NTableGridView m_TableView;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NRowHeadersExample.
        /// </summary>
        public static readonly NSchema NRowHeadersExampleSchema;

        #endregion
    }
}