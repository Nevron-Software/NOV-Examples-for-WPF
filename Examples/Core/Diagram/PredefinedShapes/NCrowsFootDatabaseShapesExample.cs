using Nevron.Nov.Diagram;
using Nevron.Nov.Dom;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NCrowsFootDatabaseShapesExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NCrowsFootDatabaseShapesExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NCrowsFootDatabaseShapesExample()
		{
			NCrowsFootDatabaseShapesExampleSchema = NSchema.Create(typeof(NCrowsFootDatabaseShapesExample), NExampleBaseSchema);
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
			return @"
<p>
    This example demonstrates the Crows Foot Database shapes, which are created by the NDatabaseShapesFactory.
</p>
";
		}

		private void InitDiagram(NDrawingDocument drawingDocument)
		{
			throw new System.NotImplementedException();
		}

		#endregion

		#region Fields

		private NDrawingView m_DrawingView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NCrowsFootDatabaseShapesExample.
		/// </summary>
		public static readonly NSchema NCrowsFootDatabaseShapesExampleSchema;

		#endregion
	}
}