using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Framework
{
	public class NIcoDecoderExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NIcoDecoderExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NIcoDecoderExample()
		{
			NIcoDecoderExampleSchema = NSchema.Create(typeof(NIcoDecoderExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_HeaderLabel = new NLabel();
			m_HeaderLabel.Margins = new NMargins(0, NDesign.VerticalSpacing, 0, 0);
			NStylePropertyEx.SetRelativeFontSize(m_HeaderLabel, ENRelativeFontSize.Large);

			m_TablePanel = new NTableFlowPanel();
			m_TablePanel.Direction = ENHVDirection.LeftToRight;
			m_TablePanel.MaxOrdinal = 2;

			LoadIconImages(NResources.Image_Print_ico);

			NPairBox pairBox = new NPairBox(m_HeaderLabel, m_TablePanel, ENPairBoxRelation.Box1AboveBox2);
			return pairBox;
		}
		protected override NWidget CreateExampleControls()
		{
			NButton button = NButton.CreateImageAndText(Presentation.NResources.Image_File_Open_png, "Load Icon...");
			button.HorizontalPlacement = ENHorizontalPlacement.Left;
			button.VerticalPlacement = ENVerticalPlacement.Top;
			button.Click += OnLoadIconButtonClick;

			return button;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to load Windows icons (ICO files). Click the <b>Load Icon</b> button on the right to load an icon from file.
</p>";
		}

		#endregion

		#region Implementation

		private void LoadIconImages(NImage icon)
		{
			// Update the image name
			if (icon.ImageSource is NFileImageSource fileImageSource)
			{
				m_HeaderLabel.Text = fileImageSource.Uri.GetLocalPath();
			}
			else if(icon.ImageSource is NEmbeddedResourceImageSource embeddedResourceImageSource)
			{
				m_HeaderLabel.Text = embeddedResourceImageSource.Resource.ResourceName;
			}

			// Get the image source and all frame rasters stored in it
			NImageSource imageSource = icon.ImageSource;

			// Create an image box for each raster
			m_TablePanel.Clear();

			for (int i = 0; i < imageSource.FrameCount; i++)
			{
				NRaster raster = imageSource.CreateFrameRaster(i);

				NLabel label = new NLabel($"Image {i + 1} - {raster.Width} x {raster.Height}:");
				m_TablePanel.Add(label);

				NImageBox imageBox = new NImageBox(NImage.FromRaster(raster));
				imageBox.HorizontalPlacement = ENHorizontalPlacement.Center;
				m_TablePanel.Add(imageBox);
			}
		}

		#endregion

		#region Event Handlers

		private void OnLoadIconButtonClick(NEventArgs arg)
		{
			NOpenFileDialog openDialog = new NOpenFileDialog();
			openDialog.FileTypes = new NFileDialogFileType[] { new NFileDialogFileType(NImageFormat.Ico.Name, NImageFormat.Ico.FileExtensions) };
			openDialog.Closed += OnLoadIconDialogClosed;
			openDialog.RequestShow();
		}
		private void OnLoadIconDialogClosed(NOpenFileDialogResult arg)
		{
			if (arg.Result == ENCommonDialogResult.OK)
			{
				// Craete an image from file
				NImage image = NImage.FromFileEmbedded(arg.Files[0]);
				image.ImageSource.Changed += delegate(NImageSource imgSource)
				{
					// Image data loaded, so extract and show the icon images
					LoadIconImages(image);
				};

				// Call the CreateRaster method of the image source to force it load its data from disk
				image.ImageSource.CreateRaster();
			}
		}

		#endregion

		#region Fields

		private NLabel m_HeaderLabel;
		private NTableFlowPanel m_TablePanel;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NIcoDecoderExample.
		/// </summary>
		public static readonly NSchema NIcoDecoderExampleSchema;

		#endregion
	}
}