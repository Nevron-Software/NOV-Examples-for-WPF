using System;
using Nevron.Nov.Barcode;
using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Barcode
{
	public class NPdf417BarcodeExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NPdf417BarcodeExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NPdf417BarcodeExample()
		{
			NPdf417BarcodeExampleSchema = NSchema.Create(typeof(NPdf417BarcodeExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_Barcode = new NMatrixBarcode();
			m_Barcode.Symbology = ENMatrixBarcodeSymbology.Pdf417;
			m_Barcode.Text = "Nevron Software";
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
	This example demonstrates how to create PDF 417 2D barcodes. Use the controls on the right to change
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
		/// Schema associated with NPdf417BarcodeExample.
		/// </summary>
		public static readonly NSchema NPdf417BarcodeExampleSchema;

		#endregion
	}
}