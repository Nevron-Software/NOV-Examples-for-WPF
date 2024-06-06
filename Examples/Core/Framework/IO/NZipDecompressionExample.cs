using System;
using System.IO;

using Nevron.Nov.Compression;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.IO;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Framework
{
	public class NZipDecompressionExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NZipDecompressionExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NZipDecompressionExample()
		{
			NZipDecompressionExampleSchema = NSchema.Create(typeof(NZipDecompressionExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_HeaderLabel = new NLabel("File: CSharph.zip");

			m_TreeView = new NTreeView();
			m_TreeView.MinWidth = 300;

			NPairBox pairBox = new NPairBox(m_HeaderLabel, m_TreeView, ENPairBoxRelation.Box1AboveBox2);
			pairBox.HorizontalPlacement = ENHorizontalPlacement.Left;
			pairBox.Spacing = NDesign.VerticalSpacing;

			Stream sourceCodeStream = NResources.RBIN_SourceCode_CSharp_zip.Stream;
			DecompressZip(sourceCodeStream);

			return pairBox;
		}
		protected override NWidget CreateExampleControls()
		{
			NButton openFileButton = new NButton("Open ZIP Archive");
			openFileButton.Click += OnOpenFileButtonClick;
			openFileButton.VerticalPlacement = ENVerticalPlacement.Top;

			return openFileButton;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to work with ZIP archives. By default the example extracts the file names
	of the source code files of all NOV examples. Using the <b>Open ZIP Archive</b> button you can see the
	contents of another ZIP file.
</p>
";
		}

		#endregion

		#region Implementation

		private void DecompressZip(Stream stream)
		{
			m_TreeView.SelectedItem = null;
			m_TreeView.Items.Clear();

			ZipDecompressor decompressor = new ZipDecompressor(m_TreeView);
			NCompression.DecompressZip(stream, decompressor);
		}

		#endregion

		#region Event Handlers

		private void OnOpenFileButtonClick(NEventArgs arg)
		{
			NOpenFileDialog openFileDialog = new NOpenFileDialog();
			openFileDialog.FileTypes = new NFileDialogFileType[] {
				new NFileDialogFileType("ZIP Archives", "zip")
			};

			openFileDialog.Closed += OnOpenFileDialogClosed;
			openFileDialog.RequestShow();
		}
		private void OnOpenFileDialogClosed(NOpenFileDialogResult arg)
		{
			if (arg.Result != ENCommonDialogResult.OK || arg.Files.Length != 1)
				return;

			NFile file = arg.Files[0];
			file.OpenReadAsync().Then(
				delegate (Stream stream)
				{
					using(stream)
					{
						DecompressZip(stream);
					}

					m_HeaderLabel.Text = "File: " + file.Path;
				},
				delegate (Exception ex)
				{
					NMessageBox.ShowError(ex.Message, "Error");
				}
			);
		}

		#endregion

		#region Fields

		private NLabel m_HeaderLabel;
		private NTreeView m_TreeView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NZipDecompressionExample.
		/// </summary>
		public static readonly NSchema NZipDecompressionExampleSchema;

		#endregion

		#region Nested Types

		private class ZipDecompressor : INZipDecompressor
		{
			public ZipDecompressor(NTreeView treeView)
			{
				m_TreeView = treeView;
			}

			public bool Filter(NZipItem item)
			{
				return true;
			}
			public void OnItemDecompressed(NZipItem item)
			{
				string[] partNames = item.Name.Split(PathDelimitersCharArray, StringSplitOptions.RemoveEmptyEntries);
				
				// Add the folders to the tree view
				NTreeViewItemCollection items = m_TreeView.Items;
				for (int i = 0, partNameCount = partNames.Length - 1; i < partNameCount; i++)
				{
					string partName = partNames[i];
					NTreeViewItem treeViewItem = GetItemByName(items, partName);
					if (treeViewItem == null)
					{
						// An item with the current entry name does not exist, so create it
						treeViewItem = AddFolder(items, partName);
					}

					items = treeViewItem.Items;
				}

				// Add the file
				AddFile(items, partNames[partNames.Length - 1]);
			}

			private static NTreeViewItem GetItemByName(NTreeViewItemCollection items, string name)
			{
				for (int i = 0, count = items.Count; i < count; i++)
				{
					NTreeViewItem item = items[i];
					string itemName = (string)item.Tag;
					if (itemName == name)
						return item;
				}

				return null;
			}
			private static NTreeViewItem CreateItem(NImage image, string name)
			{
				NPairBox pairBox = new NPairBox(image, name);
				pairBox.Box2.VerticalPlacement = ENVerticalPlacement.Center;
				pairBox.Spacing = NDesign.HorizontalSpacing;

				NTreeViewItem item = new NTreeViewItem(pairBox);
				item.Tag = name;
				return item;
			}
			private static NTreeViewItem AddFolder(NTreeViewItemCollection items, string name)
			{
				// Find the place for the folder item
				int i;
				for (i = items.Count - 1; i >= 0; i--)
				{
					if (items[i].Items.Count > 0)
					{
						// This is not a leaf node, which means we have reached the last folder in the given list of items
						break;
					}
				}

				// Insert the folder item
				NTreeViewItem item = CreateItem(NResources.Image__16x16_Folders_png, name);
				items.Insert(i + 1, item);
				return item;
			}
			private static NTreeViewItem AddFile(NTreeViewItemCollection items, string name)
			{
				NTreeViewItem item = CreateItem(NResources.Image__16x16_Contacts_png, name);
				items.Add(item);
				return item;
			}

			private NTreeView m_TreeView;
			private static readonly char[] PathDelimitersCharArray = new char[] { '\\', '/' };
		}

		#endregion
	}
}