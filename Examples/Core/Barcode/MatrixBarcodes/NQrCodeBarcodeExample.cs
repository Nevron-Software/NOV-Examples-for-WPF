using Nevron.Nov.Barcode;
using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Barcode
{
	public class NQrCodeBarcodeExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NQrCodeBarcodeExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NQrCodeBarcodeExample()
		{
			NQrCodeBarcodeExampleSchema = NSchema.Create(typeof(NQrCodeBarcodeExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_Barcode = new NMatrixBarcode();
			m_Barcode.Symbology = ENMatrixBarcodeSymbology.QrCode;
			m_Barcode.Text = "Nevron Software\r\n\r\nhttps://www.nevron.com";
			m_Barcode.Border = NBorder.CreateFilledBorder(NColor.Red);
			m_Barcode.BorderThickness = new NMargins(1);

			return m_Barcode;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();

			// Create the property editors
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_Barcode).CreatePropertyEditors(
				m_Barcode,
				NMatrixBarcode.HorizontalPlacementProperty,
				NMatrixBarcode.VerticalPlacementProperty,
				NMatrixBarcode.BackgroundFillProperty,
				NMatrixBarcode.TextFillProperty,
				NMatrixBarcode.ErrorCorrectionProperty,
				NMatrixBarcode.SizeModeProperty,
				NMatrixBarcode.ScaleProperty,
				NMatrixBarcode.TextProperty
			);

			for (int i = 0, count = editors.Count; i < count; i++)
			{
				stack.Add(editors[i]);
			}

			return new NUniSizeBoxGroup(stack);
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create QR Code 2D barcodes. Use the controls on the right to change
	the appearance of the barcode widget.
</p>
";
		}

		#endregion

		#region Fields

		private NMatrixBarcode m_Barcode;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NQrCodeBarcodeExample.
		/// </summary>
		public static readonly NSchema NQrCodeBarcodeExampleSchema;

		#endregion
	}
}