using Nevron.Nov.Diagram;
using Nevron.Nov.Dom;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NFishboneShapesExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NFishboneShapesExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NFishboneShapesExample()
		{
			NFishboneShapesExampleSchema = NSchema.Create(typeof(NFishboneShapesExample), NExampleBaseSchema);
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
    This example demonstrates the fishbone shapes, which are created by the NFishboneShapeFactory.
</p>
";
		}

		private void InitDiagram(NDrawingDocument drawingDocument)
		{
			m_DrawingView.LoadFromResource(NResources.RBIN_NDX_FishboneDiagram_ndx);
		}

		#endregion

		#region Fields

		private NDrawingView m_DrawingView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NFishboneShapesExample.
		/// </summary>
		public static readonly NSchema NFishboneShapesExampleSchema;

		#endregion
	}
}
