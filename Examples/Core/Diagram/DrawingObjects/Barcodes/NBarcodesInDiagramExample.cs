using Nevron.Nov.Barcode;
using Nevron.Nov.Diagram;
using Nevron.Nov.Dom;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NBarcodesInDiagramExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NBarcodesInDiagramExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NBarcodesInDiagramExample()
		{
			NBarcodesInDiagramExampleSchema = NSchema.Create(typeof(NBarcodesInDiagramExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create a simple drawing
			NDrawingViewWithRibbon drawingViewWithRibbon = new NDrawingViewWithRibbon();
			m_DrawingView = drawingViewWithRibbon.View;

			m_DrawingView.Document.HistoryService.Pause();
			try
			{
				InitDiagram(m_DrawingView.Document);
			}
			finally
			{
				m_DrawingView.Document.HistoryService.Resume();
			}

			return drawingViewWithRibbon;
		}
		protected override NWidget CreateExampleControls()
		{
			return null;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>Demonstrates how to create and host barcodes in diagram shapes.</p>";
		}

		private void InitDiagram(NDrawingDocument drawingDocument)
		{
			NPage activePage = drawingDocument.Content.ActivePage;

			// Create a barcode widget
			NMatrixBarcode barcode = new NMatrixBarcode(ENMatrixBarcodeSymbology.QrCode, "https://www.nevron.com");			

			// Create a shape and place the barcode widget in it
			NShape shape = new NShape();
			shape.SetBounds(100, 100, 100, 100);
			shape.Widget = barcode;
			activePage.Items.Add(shape);
		}

		#endregion

		#region Fields

		private NDrawingView m_DrawingView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NBarcodesInDiagramExample.
		/// </summary>
		public static readonly NSchema NBarcodesInDiagramExampleSchema;

		#endregion
	}
}