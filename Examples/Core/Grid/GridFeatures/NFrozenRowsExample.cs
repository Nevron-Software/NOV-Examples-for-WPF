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
    public class NFrozenRowsExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NFrozenRowsExample()
        {
        }
        /// <summary>
        /// Static constructor.
        /// </summary>
        static NFrozenRowsExample()
        {
            NFrozenRowsExampleSchema = NSchema.Create(typeof(NFrozenRowsExample), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            m_GridView = new NTableGridView();
            m_GridView.Grid.DataSource = NDummyDataSource.CreatePersonsOrdersDataSource();
            m_GridView.Grid.FrozenRows = 3;

            return m_GridView;
        }
        protected override NWidget CreateExampleControls()
        {
            NStackPanel pstack = new NStackPanel();
            pstack.VerticalSpacing = 2;

            NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_GridView.Grid).CreatePropertyEditors(
                m_GridView.Grid,
                NGrid.FrozenRowsProperty);

            for (int i = 0; i < editors.Count; i++)
            {
                pstack.Add(editors[i]);
            }



            return pstack;
        }
        protected override string GetExampleDescription()
        {
            return @"
<p>
    Demonstrates <b>Frozen Rows</b>.
</p>
<p>
    Frozen rows are controlled by the <b>FrozenRows</b> grid property. 
    It specifies the count of rows from the top of the grid, that are non-scrollable. 
    Frozen rows are thus appearing pinned to the column headers.
</p>
";
        }

        #endregion

        #region Fields

        NTableGridView m_GridView;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NFrozenRowsExample.
        /// </summary>
        public static readonly NSchema NFrozenRowsExampleSchema;

        #endregion
    }
}