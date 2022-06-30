using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NDfdShapesExample : NDiagramExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NDfdShapesExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NDfdShapesExample()
		{
			NDfdShapesExampleSchema = NSchema.Create(typeof(NDfdShapesExample), NDiagramExampleBase.NDiagramExampleBaseSchema);
		}

		#endregion

		#region Protected Overrides - Example

		protected override void InitDiagram()
		{
			base.InitDiagram();

			const double XStep = 150;
			const double YStep = 200;

			m_DrawingDocument.HistoryService.Pause();

			try
			{
				NDrawing drawing = m_DrawingDocument.Content;
				NPage activePage = drawing.ActivePage;

				// Hide grid and ports
				drawing.ScreenVisibility.ShowGrid = false;
				drawing.ScreenVisibility.ShowPorts = false;

				// Create all shapes
				NDfdShapeFactory factory = new NDfdShapeFactory();
				factory.DefaultSize = new NSize(80, 60);

				double x = 0;
				double y = 0;

				for (int i = 0; i < factory.ShapeCount; i++)
				{
					NShape shape = factory.CreateShape(i);
					shape.HorizontalPlacement = ENHorizontalPlacement.Center;
					shape.VerticalPlacement = ENVerticalPlacement.Center;
					string name = factory.GetShapeInfo(i).Name;
					shape.Tooltip = new NTooltip(name);
					shape.Text = factory.GetShapeInfo(i).Name;
					MoveTextBelowShape(shape);

					activePage.Items.Add(shape);

					if (shape.ShapeType == ENShapeType.Shape1D)
					{
						shape.SetBeginPoint(new NPoint(x, y));
						shape.SetEndPoint(new NPoint(x + shape.Width, y));
					}
					else
					{
						shape.SetBounds(x, y, shape.Width, shape.Height);
					}

					x += XStep;
					if (x > activePage.Width)
					{
						x = 0;
						y += YStep;
					}
				}

				// size page to content
                activePage.Layout.ContentPadding = new NMargins(50);
				activePage.SizeToContent();
			}
			finally
			{
				m_DrawingDocument.HistoryService.Resume();
			}
		}

		protected override string GetExampleDescription()
		{
			return @"
<p>
    This example demonstrates the data flow shapes, which are created by the NDfdShapeFactory.
</p>
";
		}

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NDfdShapesExample.
		/// </summary>
		public static readonly NSchema NDfdShapesExampleSchema;

		#endregion		
	}
}
