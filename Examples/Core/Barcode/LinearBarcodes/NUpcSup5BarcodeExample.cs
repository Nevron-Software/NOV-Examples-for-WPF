using Nevron.Nov.Barcode;
using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Barcode
{
	public class NUpcSup5BarcodeExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NUpcSup5BarcodeExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NUpcSup5BarcodeExample()
		{
			NUpcSup5BarcodeExampleSchema = NSchema.Create(typeof(NUpcSup5BarcodeExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create a linear barcode widget
			m_Barcode = new NLinearBarcode();
			m_Barcode.Symbology = ENLinearBarcodeSymbology.UPC_Supplemental_5Digit;
			m_Barcode.Text = "12345";
			m_Barcode.HorizontalPlacement = ENHorizontalPlacement.Center;
			m_Barcode.VerticalPlacement = ENVerticalPlacement.Center;

			return m_Barcode;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();

			// Create the property editors
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_Barcode).CreatePropertyEditors(
				m_Barcode,
				NLinearBarcode.HorizontalPlacementProperty,
				NLinearBarcode.VerticalPlacementProperty,
				NLinearBarcode.BackgroundFillProperty,
				NLinearBarcode.TextFillProperty,
				NLinearBarcode.SizeModeProperty,
				NLinearBarcode.ScaleProperty
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
	This example demonstrates how to create a 5 digit supplement of an UPC barcode. Use the controls on
	the right to change the appearance of the barcode widget.
</p>
";
		}

		#endregion

		#region Fields

		private NLinearBarcode m_Barcode;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NUpcSup5BarcodeExample.
		/// </summary>
		public static readonly NSchema NUpcSup5BarcodeExampleSchema;

		#endregion
	}
}