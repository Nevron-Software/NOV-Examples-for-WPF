using System.IO;

using Nevron.Nov.Compression;
using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Framework
{
    public class NDxfDecoderExample : NExampleBase
	{
		#region Constructors

		public NDxfDecoderExample()
		{
		}
		static NDxfDecoderExample()
		{
			NDxfDecoderExampleSchema = NSchema.Create(typeof(NDxfDecoderExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Decompress an AutoCAD DXF file from a ZIP resource
            NZipDecompressor zipDecompressor = new NZipDecompressor();
            using (Stream stream = new MemoryStream(NResources.RBIN_DXF_FloorPlan_zip.Data))
            {
                NCompression.DecompressZip(stream, zipDecompressor);
            }

			// Create a DXF image from the decompressed stream
			NImage dxfImage = NImage.FromStream(zipDecompressor.Items[0].Stream);

			// Show the image in an image box
            m_ImageBox = new NImageBox(dxfImage);
            m_ImageBox.HorizontalPlacement = ENHorizontalPlacement.Fit;
            m_ImageBox.VerticalPlacement = ENVerticalPlacement.Fit;
            m_ImageBox.SetBorder(1, NColor.Red);

            return m_ImageBox;
		}
        protected override NWidget CreateExampleControls()
        {
            NList<NPropertyEditor> propertyEditors = NDesigner.GetDesigner(m_ImageBox).CreatePropertyEditors(m_ImageBox,
                NImageBox.HorizontalPlacementProperty,
                NImageBox.VerticalPlacementProperty);

            NStackPanel stack = new NStackPanel();
            for (int i = 0; i < propertyEditors.Count; i++)
            {
                stack.Add(propertyEditors[i]);
            }

            return new NUniSizeBoxGroup(stack);
        }
        protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates NOV's AutoCAD Drawing Interchange (DXF) image decoding capabilities.
</p>
";
		}

		#endregion

		#region Fields

		private NImageBox m_ImageBox;

        #endregion

        #region Schema

        public static readonly NSchema NDxfDecoderExampleSchema;

		#endregion
	}
}