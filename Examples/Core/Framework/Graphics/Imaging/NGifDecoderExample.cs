using Nevron.Nov.Dom;
using Nevron.Nov.UI;
using Nevron.Nov.Graphics;
using Nevron.Nov.DataStructures;
using Nevron.Nov.Layout;

namespace Nevron.Nov.Examples.Framework
{
	public class NGifDecoderExample : NExampleBase
	{
		#region Constructors

		public NGifDecoderExample()
		{
		}
		static NGifDecoderExample()
		{
			NGifDecoderExampleSchema = NSchema.Create(typeof(NGifDecoderExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NList<string> imageNames = NImageDecodingExampleHelper.GetImageNames("GifSuite", "gif");

			NTableFlowPanel table = new NTableFlowPanel();
			table.HorizontalPlacement = ENHorizontalPlacement.Left;
			table.VerticalPlacement = ENVerticalPlacement.Top;
			table.Padding = new NMargins(30);
			table.HorizontalSpacing = 30;
			table.VerticalSpacing = 30;
			table.MaxOrdinal = 2;

            int rowCount = imageNames.Count;
			for (int i = 0; i < rowCount; i++)
			{
				NLabel nameLabel = new NLabel(imageNames[i]);
				nameLabel.MaxWidth = 200;

				NEmbeddedResourceImageSource imgSrc = new NEmbeddedResourceImageSource(new NEmbeddedResourceRef(NResources.Instance, imageNames[i]));
				imgSrc.AnimateFrames = true;			

				NImageBox novImageBox = new NImageBox(new NImage(imgSrc));
				novImageBox.ImageMapping = new NAlignTextureMapping(ENHorizontalAlignment.Center, ENVerticalAlignment.Center);

				table.Add(nameLabel);
				table.Add(novImageBox);
			}

			// The table must be scrollable
			NScrollContent scroll = new NScrollContent();
			scroll.Content = table;
			return scroll;
		}
		protected override NWidget CreateExampleControls()
		{
			return null;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates NOV's GIF image decoding capabilities and the built-in support for animated GIFs.
</p>
";
		}

		#endregion

		#region Schema

		public static readonly NSchema NGifDecoderExampleSchema;

		#endregion
	}
}