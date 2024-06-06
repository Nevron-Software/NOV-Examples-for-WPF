using System.IO;

using Nevron.Nov.Compression;
using Nevron.Nov.Diagram;
using Nevron.Nov.Dom;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NAutoCadDxfImportExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NAutoCadDxfImportExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NAutoCadDxfImportExample()
		{
			NAutoCadDxfImportExampleSchema = NSchema.Create(typeof(NAutoCadDxfImportExample), NExampleBaseSchema);
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
			return @"<p>Demonstrates how to import an AutoCAD Drawing Interchange Drawing (DXF) to NOV Diagram.</p>";
		}

		private void InitDiagram(NDrawingDocument drawingDocument)
		{
			// Decompress the ZIP archive that contains the AutoCAD DXF Drawing
			NZipDecompressor zipDecompressor = new NZipDecompressor();
			using (Stream stream = new MemoryStream(NResources.RBIN_DXF_FloorPlan_zip.Data))
			{
				NCompression.DecompressZip(stream, zipDecompressor);
			}

			// Import the AutoCAD DXF Drawing
			m_DrawingView.LoadFromStreamAsync(zipDecompressor.Items[0].Stream);

			// Hide ports
			m_DrawingView.Content.ScreenVisibility.ShowPorts = false;
		}

		#endregion

		#region Fields

		private NDrawingView m_DrawingView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NAutoCadDxfImportExample.
		/// </summary>
		public static readonly NSchema NAutoCadDxfImportExampleSchema;

		#endregion
	}
}