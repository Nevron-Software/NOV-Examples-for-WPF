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
    public class NColumnsWidthExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NColumnsWidthExample()
        {
        }
        /// <summary>
        /// Static constructor.
        /// </summary>
        static NColumnsWidthExample()
        {
            NColumnsWidthExampleSchema = NSchema.Create(typeof(NColumnsWidthExample), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            m_TableView = new NTableGridView();
            m_TableView.Grid.DataSource = NDummyDataSource.CreatePersonsDataSource();
            return m_TableView;
        }
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();
            
            // create the columns combo box
            stack.Add(new NLabel("Select Column:"));
            
            m_ColumnsComboBox = new NComboBox();
            stack.Add(m_ColumnsComboBox);

            for (int i = 0; i < m_TableView.Grid.Columns.Count; i++)
            {
                NColumn column = m_TableView.Grid.Columns[i];

                NComboBoxItem item = new NComboBoxItem(column.Title);
                item.Tag = column;
                m_ColumnsComboBox.Items.Add(item);
            }

            m_ColumnsComboBox.SelectedIndexChanged += OnColumnsComboBoxSelectedIndexChanged;

            // create the columns 
            m_ColumnPropertiesHolder = new NContentHolder();
            stack.Add(new NGroupBox("Selected Column Properties", m_ColumnPropertiesHolder));

            return stack;
        }

        protected override string GetExampleDescription()
        {
            return @"
<p>
    Demonstrates the column properties that affect the column width.
</p>
<p>
    Select a column from the <b>Select Column</b> combo box and explore the properties that control the selected column width.
</p>
";
        }

        #endregion

        #region Event Handlers

        void OnColumnsComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            NComboBoxItem item = m_ColumnsComboBox.SelectedItem;
            if (item == null)
            {
                m_ColumnPropertiesHolder.Content = null;
            }
            else
            {
                NColumn column = (NColumn)item.Tag;

                NStackPanel columnPropertiesStack = new NStackPanel();
                m_ColumnPropertiesHolder.Content = new NUniSizeBoxGroup(columnPropertiesStack);

                NDesigner designer = NDesigner.GetDesigner(column);
                NList<NPropertyEditor> editors = designer.CreatePropertyEditors(column,
                    NColumn.WidthModeProperty,
                    NColumn.FixedWidthProperty,
                    NColumn.WidthPercentProperty );

                for (int i = 0; i < editors.Count; i++)
                {
                    columnPropertiesStack.Add(editors[i]);
                }
            }
        }

        #endregion

        #region Fields

        NTableGridView m_TableView;
        NComboBox m_ColumnsComboBox;
        NContentHolder m_ColumnPropertiesHolder;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NColumnsWidthExample.
        /// </summary>
        public static readonly NSchema NColumnsWidthExampleSchema;

        #endregion
    }
}