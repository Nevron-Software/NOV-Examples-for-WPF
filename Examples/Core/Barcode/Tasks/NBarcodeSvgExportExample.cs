using System;
using System.IO;

using Nevron.Nov.Barcode;
using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Formats.Svg;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Barcode
{
	public class NBarcodeSvgExportExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NBarcodeSvgExportExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NBarcodeSvgExportExample()
		{
			NBarcodeSvgExportExampleSchema = NSchema.Create(typeof(NBarcodeSvgExportExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_Barcode = new NMatrixBarcode(ENMatrixBarcodeSymbology.QrCode, "Nevron Software" +
				Environment.NewLine + Environment.NewLine + "https://www.nevron.com");
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

			NButton exportToSvgButton = new NButton("Export to SVG...");
			exportToSvgButton.Click += OnExportToSvgButtonClick;
			stack.Add(exportToSvgButton);

			return new NUniSizeBoxGroup(stack);
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to export barcodes to SVG. Enter some text in the text box on the right
	and click the <b>Export to SVG...</b> button.
</p>
";
		}

		#endregion

		#region Event Handlers

		private void OnExportToSvgButtonClick(NEventArgs arg)
		{
			NSaveFileDialog exportToSvgDialog = new NSaveFileDialog();
			exportToSvgDialog.FileTypes = new NFileDialogFileType[] {
				new NFileDialogFileType("SVG Files", "svg") };
			exportToSvgDialog.Closed += OnExportToSvgDialogClosed;
			exportToSvgDialog.RequestShow();
		}
		private void OnExportToSvgDialogClosed(NSaveFileDialogResult arg)
		{
			if (arg.Result != ENCommonDialogResult.OK)
				return;

			// Generate an SVG document from the barcode
			NContinuousMediaDocument<NBarcode> svgExporter = new NContinuousMediaDocument<NBarcode>(m_Barcode);
			NSvgDocument svgDocument = svgExporter.CreateSvg(m_Barcode, 0);

			// Save the SVG document to a file
			arg.File.CreateAsync().Then(
				delegate (Stream stream)
				{
					using (stream)
					{
						svgDocument.SaveToStream(stream);
					}
				}
			);
		}

		#endregion

		#region Fields

		private NBarcode m_Barcode;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NBarcodeSvgExportExample.
		/// </summary>
		public static readonly NSchema NBarcodeSvgExportExampleSchema;

		#endregion
	}
}