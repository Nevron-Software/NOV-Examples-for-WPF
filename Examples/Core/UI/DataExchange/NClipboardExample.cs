using System.Globalization;
using System.IO;

using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.Text;
using Nevron.Nov.Text.Formats;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NClipboardExample : NExampleBase
	{
		#region Constructors

		public NClipboardExample()
		{
		}
		static NClipboardExample()
		{
			NClipboardExampleSchema = NSchema.Create(typeof(NClipboardExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NTab tab = new NTab();
			tab.HeadersPosition = ENTabHeadersPosition.Left;

			tab.TabPages.Add(new NTabPage("Clipboard Text", CreateTextDemo()));
			tab.TabPages.Add(new NTabPage("Clipboard RTF", CreateRTFDemo()));
			tab.TabPages.Add(new NTabPage("Clipboard Raster", CreateRasterDemo()));

			return tab;
		}
		protected override NWidget CreateExampleControls()
		{
			return null;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>The example demonstrates how to use Clipboard in NOV. Click the <b>Set Clipboard Text</b> button to set the text of the text box
to the clipboard and clear the text box. Click <b>Get Clipboard Text</b> to load the text from the clipboard to the text box. The same
goes for the rich text and the images examples.</p>";
		}

		#endregion

		#region Implementation - Text

		private NWidget CreateTextDemo()
		{
			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.Last;
			stack.FitMode = ENStackFitMode.Last;

			NButton setTextButton = new NButton("Set Clipboard Text");
			setTextButton.Click += new Function<NEventArgs>(OnSetTextButtonClick);

			NButton getTextButton = new NButton("Get Clipboard Text");
			getTextButton.Click += new Function<NEventArgs>(OnGetTextButtonClick);

			NPairBox pairBox = new NPairBox(setTextButton, getTextButton);
			pairBox.HorizontalPlacement = ENHorizontalPlacement.Left;
			pairBox.Spacing = NDesign.HorizontalSpacing;
			stack.Add(pairBox);

			m_TextBox = new NTextBox();
			m_TextBox.Text = "This is some text. You can edit it or enter more if you want.\n\n" + 
				"When ready click the \"Set Clipboard Text\" button to move it to the clipboard.";
			m_TextBox.Multiline = true;
			stack.Add(m_TextBox);

			return stack;
		}
		private void OnGetTextButtonClick(NEventArgs args)
		{
			NDataObject dataObject = NClipboard.GetDataObject();
			object data = dataObject.GetData(NDataFormat.TextFormatString);
			if (data != null)
			{
				m_TextBox.Text = (string)data;
			}
		}
		private void OnSetTextButtonClick(NEventArgs args)
		{
			NDataObject dataObject = new NDataObject();
			dataObject.SetData(NDataFormat.TextFormatString, m_TextBox.Text);
			NClipboard.SetDataObject(dataObject);

			m_TextBox.Text = "Text box content moved to clipboard.";
		}

		#endregion

		#region Implementation - Rich Text

		private NWidget CreateRTFDemo()
		{
			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.Last;
			stack.FitMode = ENStackFitMode.Last;

			NButton setButton = new NButton("Set Clipboard RTF");
			setButton.Click += new Function<NEventArgs>(OnSetRTFButtonClick);

			NButton getButton = new NButton("Get Clipboard RTF");
			getButton.Click += new Function<NEventArgs>(OnGetRTFButtonClick);

			NPairBox pairBox = new NPairBox(setButton, getButton);
			pairBox.HorizontalPlacement = ENHorizontalPlacement.Left;
			pairBox.Spacing = NDesign.HorizontalSpacing;
			stack.Add(pairBox);

			// Create a rich text view and some content
			m_RichText = new NRichTextView();
			m_RichText.PreferredSize = new NSize(400, 300);

			NSection section = new NSection();
			m_RichText.Content.Sections.Add(section);

			NTable table = new NTable(2, 2);
			table.AllowSpacingBetweenCells = false;

			section.Blocks.Add(table);
			for (int i = 0; i < 4; i++)
			{
				NTableCell cell = table.Rows[i / 2].Cells[i % 2];
				cell.Border = NBorder.CreateFilledBorder(NColor.Black);
				cell.BorderThickness = new NMargins(1);
				NParagraph paragraph = (NParagraph)cell.Blocks[0];
				paragraph.Inlines.Add(new NTextInline("Cell " + (i + 1).ToString(CultureInfo.InvariantCulture)));
			}

			stack.Add(m_RichText);
			return stack;
		}
		private void OnGetRTFButtonClick(NEventArgs args)
		{
			NDataObject dataObject = NClipboard.GetDataObject();
			byte[] data = dataObject.GetRTF();
			if (data != null)
			{
				m_RichText.LoadFromStream(new MemoryStream(data), NTextFormat.Rtf);
			}
		}
		private void OnSetRTFButtonClick(NEventArgs args)
		{
			NDataObject dataObject = new NDataObject();
			using (MemoryStream stream = new MemoryStream())
			{
				m_RichText.SaveToStream(stream, NTextFormat.Rtf);
				dataObject.SetData(NDataFormat.RTFFormat, stream.ToArray());
				NClipboard.SetDataObject(dataObject);
			}

			// Clear the rich text
			m_RichText.Content.Sections.Clear();

			NSection section = new NSection();
			m_RichText.Content.Sections.Add(section);
			section.Blocks.Add(new NParagraph("Rich text content moved to clipboard."));
		}

		#endregion

		#region Implementation - Images

		private NWidget CreateRasterDemo()
		{
			NStackPanel rasterStack = new NStackPanel();
			rasterStack.FillMode = ENStackFillMode.Last;
			rasterStack.FitMode = ENStackFitMode.Last;

			// create the controls that demonstrate how to place image content on the clipboard
			{
				NGroupBox setRastersGroupBox = new NGroupBox("Setting images on the clipboard");
				rasterStack.Add(setRastersGroupBox);

				NStackPanel setRastersStack = new NStackPanel();
				setRastersStack.Direction = ENHVDirection.LeftToRight;
				setRastersGroupBox.Content = setRastersStack;

				for (int i = 0; i < 3; i++)
				{
					NPairBox pair = new NPairBox();

					switch (i)
					{
						case 0:
							pair.Box1 = new NImageBox(NResources.Image__48x48_Book_png);
							break;
						case 1:
							pair.Box1 = new NImageBox(NResources.Image__48x48_Clock_png);
							break;
						case 2:
							pair.Box1 = new NImageBox(NResources.Image__48x48_Darts_png);
							break;
					}

					pair.Box2 = new NLabel("Set me on the clipboard");
					pair.Box2.HorizontalPlacement = ENHorizontalPlacement.Center;
					pair.BoxesRelation = ENPairBoxRelation.Box1AboveBox2;

					NButton setRasterButton = new NButton(pair);
					setRasterButton.Tag = i;
					setRasterButton.Click += new Function<NEventArgs>(OnSetRasterButtonClick);
					setRastersStack.Add(setRasterButton);
				}
			}

			// create the controls that demonstrate how to get image content from the clipboard
			{
				NGroupBox getRastersGroupBox = new NGroupBox("Getting images from the clipboard");
				rasterStack.Add(getRastersGroupBox);

				NStackPanel getRastersStack = new NStackPanel();
				getRastersStack.FillMode = ENStackFillMode.Last;
				getRastersStack.FitMode = ENStackFitMode.Last;
				getRastersGroupBox.Content = getRastersStack;

				m_ImageBox = new NImageBox();
				m_ImageBox.Margins = new NMargins(10);
				m_ImageBox.Border = NBorder.CreateFilledBorder(NColor.Black);
				m_ImageBox.BorderThickness = new NMargins(1);
				m_ImageBox.Visibility = ENVisibility.Hidden;

				NButton getRasterButton = new NButton("Get image from the clipboard");
				getRasterButton.HorizontalPlacement = ENHorizontalPlacement.Left;
				getRasterButton.Click += new Function<NEventArgs>(OnGetRasterButtonClick);
				getRastersStack.Add(getRasterButton);

				NScrollContent scrollContent = new NScrollContent();
				scrollContent.BackgroundFill = new NHatchFill(ENHatchStyle.LargeCheckerBoard, NColor.Gray, NColor.LightGray);
				scrollContent.Content = m_ImageBox;
				scrollContent.NoScrollHAlign = ENNoScrollHAlign.Left;
				scrollContent.NoScrollVAlign = ENNoScrollVAlign.Top;
				getRastersStack.Add(scrollContent);
			}

			return rasterStack;
		}
		private void OnSetRasterButtonClick(NEventArgs args)
		{
			// get a raster to place on the clipbar
			NRaster raster = null;
			switch ((int)args.TargetNode.Tag)
			{
				case 0:
					raster = NResources.Image__48x48_Book_png.ImageSource.CreateRaster();
					break;
				case 1:
					raster = NResources.Image__48x48_Clock_png.ImageSource.CreateRaster();
					break;
				case 2:
					raster = NResources.Image__48x48_Darts_png.ImageSource.CreateRaster();
					break;
			}

			// create a data object
			NDataObject dataObject = new NDataObject();
			dataObject.SetData(NDataFormat.RasterFormat, raster);

			// set it on the clipboard
			NClipboard.SetDataObject(dataObject);
		}
		private void OnGetRasterButtonClick(NEventArgs args)
		{
			// get a data object from the clipboard
			NDataObject dataObject = NClipboard.GetDataObject();

			// try get a raster from the data object
			object data = dataObject.GetData(NDataFormat.RasterFormat);
			if (data == null)
				return;

			// place it inside the image box
			NRaster raster = (NRaster)data;
			m_ImageBox.Image = new NImage(raster);
			m_ImageBox.Visibility = ENVisibility.Visible;
		}

		#endregion

		#region Fields

		private NTextBox m_TextBox;
		private NRichTextView m_RichText;
		private NImageBox m_ImageBox;

		#endregion

		#region Schema

		public static readonly NSchema NClipboardExampleSchema;

		#endregion
	}
}