using System;

using Nevron.Nov.Barcode;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Barcode
{
	public class NBarcodeImageExportExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NBarcodeImageExportExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NBarcodeImageExportExample()
		{
			NBarcodeImageExportExampleSchema = NSchema.Create(typeof(NBarcodeImageExportExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Protected Overrides

		protected override NWidget CreateExampleContent()
		{
			m_ImageBox = new NImageBox();
			m_ImageBox.HorizontalPlacement = ENHorizontalPlacement.Center;
			m_ImageBox.VerticalPlacement = ENVerticalPlacement.Center;
			return m_ImageBox;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();

			m_BarcodeTextBox = new NTextBox("Nevron Software" +
				Environment.NewLine + Environment.NewLine + "https://www.nevron.com");

			m_BarcodeTextBox.Multiline = true;
			m_BarcodeTextBox.AcceptsEnter = true;
			m_BarcodeTextBox.PreferredHeight = 100;
			m_BarcodeTextBox.TextChanged += OnBarcodeTextBoxTextChanged;
			stack.Add(m_BarcodeTextBox);

			m_GenerateImageButton = new NButton("Generate Image");
			m_GenerateImageButton.Click += OnGenerateImageButtonClick;
			stack.Add(m_GenerateImageButton);

			OnGenerateImageButtonClick(null);

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how export barcodes to raster images. Enter some text in the text box on the right
	and click the <b>Generate Image</b> button.
</p>
";
		}

		#endregion

		#region Event Handlers

		private void OnBarcodeTextBoxTextChanged(NValueChangeEventArgs arg)
		{
			string text = (string)arg.NewValue;
			m_GenerateImageButton.Enabled = text != null && text.Length > 0;
		}
		private void OnGenerateImageButtonClick(NEventArgs arg)
		{
			NMatrixBarcodePainter painter = new NMatrixBarcodePainter();
			painter.Symbology = ENMatrixBarcodeSymbology.QrCode;
			painter.Text = m_BarcodeTextBox.Text;
			NRaster qrRaster = painter.CreateRaster(100, 100, NRaster.DefaultResolution);
			NImage qrImage = new NImage(qrRaster);

			m_ImageBox.Image = qrImage;
		}

		#endregion

		#region Fields

		private NImageBox m_ImageBox;
		private NTextBox m_BarcodeTextBox;
		private NButton m_GenerateImageButton;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NBarcodeImageExportExample.
		/// </summary>
		public static readonly NSchema NBarcodeImageExportExampleSchema;

		#endregion
	}
}