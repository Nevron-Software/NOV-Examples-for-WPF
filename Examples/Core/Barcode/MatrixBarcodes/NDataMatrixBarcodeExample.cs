using System;
using Nevron.Nov.Barcode;
using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Barcode
{
	public class NDataMatrixBarcodeExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NDataMatrixBarcodeExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NDataMatrixBarcodeExample()
		{
			NDataMatrixBarcodeExampleSchema = NSchema.Create(typeof(NDataMatrixBarcodeExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_Barcode = new NMatrixBarcode();
			m_Barcode.Symbology = ENMatrixBarcodeSymbology.DataMatrix;
			m_Barcode.Text = "Nevron Software\r\n\r\nhttps://www.nevron.com";
			m_Barcode.SetBorder(1, NColor.Red);

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
	This example demonstrates how to create Data Matrix 2D barcodes. Use the controls on the right to change
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
		/// Schema associated with NDataMatrixBarcodeExample.
		/// </summary>
		public static readonly NSchema NDataMatrixBarcodeExampleSchema;

		#endregion
	}
}