using System.Globalization;

using Nevron.Nov.Data;
using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Grid;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Framework
{
    public class NEmbeddedResourcesExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NEmbeddedResourcesExample()
        {
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static NEmbeddedResourcesExample()
        {
            NEmbeddedResourcesExampleSchema = NSchema.Create(typeof(NEmbeddedResourcesExample), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            m_TreeView = new NTreeView();
            m_TreeView.SelectedPathChanged += OnTreeViewSelectedPathChanged;

            m_ResourcesMap = new NMap<NTreeViewItem, NEmbeddedResourceContainer>();
            m_TreeView.Items.Add(CreateRootItem(Nevron.Nov.Presentation.NResources.Instance));
            m_TreeView.Items.Add(CreateRootItem(Nevron.Nov.Diagram.NResources.Instance));
            m_TreeView.Items.Add(CreateRootItem(Nevron.Nov.Text.NResources.Instance));
            m_TreeView.Items.Add(CreateRootItem(Nevron.Nov.Schedule.NResources.Instance));
            m_TreeView.Items.Add(CreateRootItem(Nevron.Nov.Grid.NResources.Instance));

            // Create a data table
            m_DataTable = new NMemoryDataTable();
            m_DataTable.AddField(new NFieldInfo("Image", typeof(NImage)));
            m_DataTable.AddField(new NFieldInfo("Name", typeof(string)));
            m_DataTable.AddField(new NFieldInfo("Size", typeof(string)));
            m_DataTable.AddField(new NFieldInfo("Action", typeof(string)));

            // Create a grid view
            m_GridView = new NTableGridView();
            m_GridView.GroupingPanel.Visibility = ENVisibility.Collapsed;
            m_GridView.ReadOnly = true;

            NTableGrid tableGrid = m_GridView.Grid;
            tableGrid.AlternatingRows = false;
            tableGrid.RowHeaders.Visible = false;
            tableGrid.AutoCreateColumn += OnGridAutoCreateColumn;
            tableGrid.DataSource = new NDataSource(m_DataTable);

            NSplitter splitter = new NSplitter(m_TreeView, m_GridView, ENSplitterSplitMode.OffsetFromNearSide, 200);
            return splitter;
        }
        protected override NWidget CreateExampleControls()
        {
            return null;
        }
        protected override string GetExampleDescription()
        {
            return @"
<p>
	Browser for all resources embedded in the NOV assemblies. Select a category from the tree view
	with resources and then click the <b>Copy Code</b> button in the grid next to the image resource
	you are interested in to copy the code for using it to the clipboard.
</p>
";
        }

        #endregion

        #region Implementation

        private NTreeViewItem CreateRootItem(NEmbeddedResourceContainer resourceContainer)
        {
            string nspace = resourceContainer.GetType().Namespace;
            string name = nspace.Substring("Nevron.Nov.".Length);

            NTreeViewItem rootItem = new NTreeViewItem(name);
            m_ResourcesMap.Set(rootItem, resourceContainer);
            rootItem.Expanded = true;

            string[] names = resourceContainer.GetResourceNames();
            for (int i = 0; i < names.Length; i++)
            {
                string[] tokens = names[i].Split('_');
                if (tokens[0] != "RIMG")
                    continue;

                // Navigate to the path of the current image resource in the tree view
                NTreeViewItem item = rootItem;
                for (int j = 1; j < tokens.Length - 2; j++)
                {
                    item = GetOrCreateItem(item.Items, tokens[j]);
                }

                // Add the image resource to the path
                NList<string> images = GetImageNames(item);
                if (images == null)
                {
                    images = new NList<string>();
                    item.Tag = images;
                }

                images.Add(names[i]);
            }

            return rootItem;
        }
        private NTreeViewItem GetOrCreateItem(NTreeViewItemCollection items, string name)
        {
            for (int i = 0, count = items.Count; i < count; i++)
            {
                NLabel label = (NLabel)items[i].Header.GetFirstDescendant(NLabel.NLabelSchema);
                if (label.Text == name)
                    return items[i];
            }

            NTreeViewItem item = new NTreeViewItem(NPairBox.Create(NResources.Image__16x16_Folders_png, name));
            items.Add(item);
            return item;
        }
        private NEmbeddedResourceContainer GetResourceContainer(NTreeViewItem item)
        {
            // Find the root item of the given item
            while (item.ParentItem != null)
            {
                item = item.ParentItem;
            }

            // Return the resource container represented by this root item
            return m_ResourcesMap[item];
        }
        private NList<string> GetImageNames(NTreeViewItem item)
        {
            return (NList<string>)item.Tag;
        }

        #endregion

        #region Event Handlers

        private void OnTreeViewSelectedPathChanged(NValueChangeEventArgs arg)
        {
            m_DataTable.RemoveAllRows();
            m_GridView.Grid.Update();

            NTreeView treeView = (NTreeView)arg.CurrentTargetNode;
            NTreeViewItem selectedItem = treeView.SelectedItem;
            if (selectedItem == null)
                return;

            // Get the resource container and the images for the selected item
            NEmbeddedResourceContainer resourceContainer = GetResourceContainer(selectedItem);
            NList<string> images = GetImageNames(selectedItem);

            // Populate the stack with the images in the selected resources folder
            string containerType = resourceContainer.GetType().FullName;
            for (int i = 0; i < images.Count; i++)
            {
                string resourceName = images[i];

                string imageName = resourceName.Replace("RIMG", "Image");
                NImage image = NImage.FromResource(resourceContainer.GetResource(resourceName));
                string imageSize = image.Width.ToString(
                    CultureInfo.InvariantCulture) + 
                    " x " + 
                    image.Height.ToString(CultureInfo.InvariantCulture);
                string code = containerType + "." + imageName;

                m_DataTable.AddRow(image, imageName, imageSize, code);
            }

        }
        private void OnGridAutoCreateColumn(NAutoCreateColumnEventArgs arg)
        {
            NDataColumn dataColumn = arg.DataColumn;
            if (dataColumn.FieldName == "Action")
            {
                dataColumn.Format = new NButtonColumnFormat();
            }
        }

        #endregion

        #region Fields

        private NTreeView m_TreeView;
        private NTableGridView m_GridView;
        private NMemoryDataTable m_DataTable;
        private NMap<NTreeViewItem, NEmbeddedResourceContainer> m_ResourcesMap;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NEmbeddedResourcesExample.
        /// </summary>
        public static readonly NSchema NEmbeddedResourcesExampleSchema;

        #endregion

        #region Nested Types

        private class NButtonColumnFormat : NColumnFormat
        {
            #region Constructors

            /// <summary>
            /// Default constructor.
            /// </summary>
            public NButtonColumnFormat()
            {
            }

            /// <summary>
            /// Static constructor.
            /// </summary>
            static NButtonColumnFormat()
            {
                NButtonColumnFormatSchema = NSchema.Create(typeof(NButtonColumnFormat), NColumnFormat.NColumnFormatSchema);
            }

            #endregion

            #region Overrides

            public override void FormatDefaultDataCell(NDataCell dataCell)
            {
            }
            protected override NWidget CreateValueDataCellView(NDataCell dataCell, object rowValue)
            {
                NButton button = new NButton("Copy Code");
                button.Tag = rowValue;
                button.Click += OnButtonClick;
                return button;
            }
            protected override ENHorizontalPlacement GetAutomaticHorizontalAlignment(NDataCell dataCell, object rowValue)
            {
                return ENHorizontalPlacement.Center;
            }

            #endregion

            #region Event Handlers

            private void OnButtonClick(NEventArgs arg)
            {
                NClipboard.SetText((string)arg.CurrentTargetNode.Tag);
            }

            #endregion

            #region Schema

            /// <summary>
            /// Schema associated with NButtonColumnFormat.
            /// </summary>
            public static readonly NSchema NButtonColumnFormatSchema;

            #endregion
        }

        #endregion
    }
}