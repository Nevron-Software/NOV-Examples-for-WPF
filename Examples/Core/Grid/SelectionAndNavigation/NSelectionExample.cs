using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Grid;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Grid
{
    public class NSelectionExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NSelectionExample()
        {
        }
        /// <summary>
        /// Static constructor.
        /// </summary>
        static NSelectionExample()
        {
            NSelectionExampleSchema = NSchema.Create(typeof(NSelectionExample), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            m_TableView = new NTableGridView();
            m_TableView.Grid.DataSource = NDummyDataSource.CreateCompanySalesDataSource();
            m_TableView.Grid.AllowEdit = true;
            return m_TableView;
        }
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();
            
            // create the row headers properties
            {
                NStackPanel selectionStack = new NStackPanel();

                NDesigner designer = NDesigner.GetDesigner(NGridSelection.NGridSelectionSchema);
                NList<NPropertyEditor> editors = designer.CreatePropertyEditors(
                    m_TableView.Grid.Selection,
                    NGridSelection.ModeProperty,
                    NGridSelection.AllowCurrentCellProperty,
                    NGridSelection.BeginEditCellOnClickProperty,
                    NGridSelection.BeginEditCellOnDoubleClickProperty,
                    NGridSelection.BeginEditCellOnBecomeCurrentProperty
                    );

                for (int i = 0; i < editors.Count; i++)
                {
                    selectionStack.Add(editors[i]);
                }

                NGroupBox selectionGroup = new NGroupBox("Selection Properties", selectionStack);
                stack.Add(new NUniSizeBoxGroup(selectionGroup));
            }

            return stack;
        }
        protected override string GetExampleDescription()
        {
            return @"
<p>
    Demonstrates the grid selection modes and various properties that affect the current cell and its editing behavior.
</p>
";
        }

        #endregion

        #region Fields

        NTableGridView m_TableView;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NSelectionExample.
        /// </summary>
        public static readonly NSchema NSelectionExampleSchema;

        #endregion
    }
}