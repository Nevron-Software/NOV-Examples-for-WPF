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
    public class NColumnProtectionExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NColumnProtectionExample()
        {
        }
        /// <summary>
        /// Static constructor.
        /// </summary>
        static NColumnProtectionExample()
        {
            NColumnProtectionExampleSchema = NSchema.Create(typeof(NColumnProtectionExample), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            m_GridView = new NTableGridView();
            m_GridView.Grid.DataSource = NDummyDataSource.CreateProductsDataSource();
            m_GridView.Grid.AllowSortColumns = true;
            return m_GridView;
        }
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();

            // create the columns combo box
            {
                stack.Add(new NLabel("Select Column:"));
                m_ColumnsComboBox = new NComboBox();
                stack.Add(m_ColumnsComboBox);
                for (int i = 0; i < m_GridView.Grid.Columns.Count; i++)
                {
                    NColumn column = m_GridView.Grid.Columns[i];
                    NComboBoxItem item = new NComboBoxItem(column.Title);
                    item.Tag = column;
                    m_ColumnsComboBox.Items.Add(item);
                }

                m_ColumnsComboBox.SelectedIndexChanged += OnColumnsComboBoxSelectedIndexChanged;

                // create the columns 
                m_ColumnPropertiesHolder = new NContentHolder();
                stack.Add(new NGroupBox("Selected Column Properties", m_ColumnPropertiesHolder));
            }

            return stack;
        }
        protected override string GetExampleDescription()
        {
            return @"
<p>
    Demonstrates the column properties that affect the operations, which the user can perform with the column (e.g column protection).
<p>";
        }

        #endregion

        #region Event Handlers

        void OnColumnsComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            NComboBoxItem item = m_ColumnsComboBox.SelectedItem;
            if (item == null)
            {
                m_ColumnPropertiesHolder.Content = null;
                return;
            }

            NColumn column = (NColumn)item.Tag;

            NStackPanel columnPropertiesStack = new NStackPanel();
            m_ColumnPropertiesHolder.Content = new NUniSizeBoxGroup(columnPropertiesStack);

            NDesigner designer = NDesigner.GetDesigner(column);
            NList<NPropertyEditor> editors = designer.CreatePropertyEditors(column,
                NColumn.AllowFilterProperty,
                NColumn.AllowSortProperty,
                NColumn.AllowGroupProperty,
                NColumn.AllowFormatProperty,
                NColumn.AllowEditProperty,
                NColumn.AllowReorderProperty,
                NColumn.AllowResizeProperty);

            for (int i = 0; i < editors.Count; i++)
            {
                columnPropertiesStack.Add(editors[i]);
            }
        }

        #endregion

        #region Fields

        NTableGridView m_GridView;
        NComboBox m_ColumnsComboBox;
        NContentHolder m_ColumnPropertiesHolder;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NColumnProtectionExample.
        /// </summary>
        public static readonly NSchema NColumnProtectionExampleSchema;

        #endregion
    }
}