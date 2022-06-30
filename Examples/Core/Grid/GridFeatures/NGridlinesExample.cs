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
    public class NGridlinesExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NGridlinesExample()
        {
        }
        /// <summary>
        /// Static constructor.
        /// </summary>
        static NGridlinesExample()
        {
            NGridlinesExampleSchema = NSchema.Create(typeof(NGridlinesExample), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            m_GridView = new NTableGridView();
            m_GridView.Grid.DataSource = NDummyDataSource.CreateCompanySalesDataSource();
            return m_GridView;
        }
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();

            // default gridlines
            {
                NStackPanel pstack = new NStackPanel();
                pstack.VerticalSpacing = 2;

                NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_GridView.Grid).CreatePropertyEditors(
                    m_GridView.Grid,
                    NGrid.HorizontalGridlinesStrokeProperty,
                    NGrid.VerticalGridlinesStrokeProperty
                );

                for (int i = 0; i < editors.Count; i++)
                {
                    pstack.Add(editors[i]);
                }

                stack.Add(new NGroupBox("Grid Gridlines", new NUniSizeBoxGroup(pstack)));
            }

            // column gridlines
            {
                NStackPanel pstack = new NStackPanel();
                pstack.VerticalSpacing = 2;

                NList<NPropertyEditor> editors = NDesigner.GetDesigner(NColumnCollection.NColumnCollectionSchema).CreatePropertyEditors(
                    m_GridView.Grid.Columns,
                    NColumnCollection.VisibleProperty,
                    NColumnCollection.TopGridlineStrokeProperty,
                    NColumnCollection.BottomGridlineStrokeProperty,
                    NColumnCollection.VerticalGridlinesStrokeProperty
                );

                for (int i = 0; i < editors.Count; i++)
                {
                    pstack.Add(editors[i]);
                }

                stack.Add(new NGroupBox("Columns Properties", new NUniSizeBoxGroup(pstack)));
            }

            // row headers gridlines
            {
                NStackPanel pstack = new NStackPanel();
                pstack.VerticalSpacing = 2;

                NList<NPropertyEditor> editors = NDesigner.GetDesigner(NRowHeaderCollection.NRowHeaderCollectionSchema).CreatePropertyEditors(
                    m_GridView.Grid.RowHeaders,
                    NRowHeaderCollection.VisibleProperty, 
                    NRowHeaderCollection.LeftGridlineStrokeProperty,
                    NRowHeaderCollection.RightGridlineStrokeProperty,
                    NRowHeaderCollection.HorizontalGridlinesStrokeProperty
                );

                for (int i = 0; i < editors.Count; i++)
                {
                    pstack.Add(editors[i]);
                }

                stack.Add(new NGroupBox("Row Headers Properties", new NUniSizeBoxGroup(pstack)));
            }


            return stack;
        }
        protected override string GetExampleDescription()
        {
            return @"
<p>
    Demonstrates gridlines.
</p>
<p>
    NOV Grid for .NET features several types of gridlines that are displayed by the Grid cells, Columns and Row Headers.
</p>
<p>
    Use the controls on the right to change the appearance of the gridlines.
</p>
";
        }

        #endregion

        #region Fields

        NTableGridView m_GridView;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NGridlinesExample.
        /// </summary>
        public static readonly NSchema NGridlinesExampleSchema;

        #endregion
    }
}