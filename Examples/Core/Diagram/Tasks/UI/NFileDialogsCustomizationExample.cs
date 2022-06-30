using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.DrawingCommands;
using Nevron.Nov.Diagram.Formats;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NFileDialogsCustomizationExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NFileDialogsCustomizationExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NFileDialogsCustomizationExample()
		{
			NFileDialogsCustomizationExampleSchema = NSchema.Create(typeof(NFileDialogsCustomizationExample), NExampleBaseSchema);
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

			// Replace the "Open" and "Save As" command actions with the custom ones
			m_DrawingView.Commander.ReplaceCommandAction(new CustomOpenCommandAction());
			m_DrawingView.Commander.ReplaceCommandAction(new CustomSaveAsCommandAction());

			return drawingViewWithRibbon;
		}
		protected override NWidget CreateExampleControls()
		{
			return null;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to customize the NOV Diagram Open and Save As file dialogs.</p>";
		}

		private void InitDiagram(NDrawingDocument drawingDocument)
		{
			NDrawing drawing = drawingDocument.Content;
			NPage activePage = drawing.ActivePage;

			drawing.ScreenVisibility.ShowGrid = false;
			drawing.ScreenVisibility.ShowPorts = false;

			NBasicShapeFactory basisShapes = new NBasicShapeFactory();
			NFlowchartShapeFactory flowChartingShapes = new NFlowchartShapeFactory();
			NConnectorShapeFactory connectorShapes = new NConnectorShapeFactory();

			NShape nonPrintableShape = basisShapes.CreateShape(ENBasicShape.Rectangle);
			nonPrintableShape.Text = "Non printable shape";
			nonPrintableShape.AllowPrint = false;
			nonPrintableShape.Geometry.Fill = new NColorFill(NColor.Tomato);
			nonPrintableShape.SetBounds(50, 50, 150, 50);
			activePage.Items.Add(nonPrintableShape);

			NShape isLifeGood = flowChartingShapes.CreateShape(ENFlowchartingShape.Decision);
			isLifeGood.Text = "Is Life Good?";
			isLifeGood.SetBounds(300, 50, 150, 100);
			isLifeGood.Geometry.Fill = new NColorFill(NColor.LightSkyBlue);
			activePage.Items.Add(isLifeGood);

			NShape goodShape = flowChartingShapes.CreateShape(ENFlowchartingShape.Termination);
			goodShape.Text = "Good";
			goodShape.SetBounds(200, 200, 100, 100);
			goodShape.Geometry.Fill = new NColorFill(NColor.Gold);
			activePage.Items.Add(goodShape);

			NShape changeSomething = flowChartingShapes.CreateShape(ENFlowchartingShape.Process);
			changeSomething.Text = "Change Something";
			changeSomething.Geometry.Fill = new NColorFill(NColor.Thistle);
			changeSomething.SetBounds(450, 200, 100, 100);
			activePage.Items.Add(changeSomething);

			NShape yesConnector = connectorShapes.CreateShape(ENConnectorShape.RoutableConnector);
			yesConnector.Text = "Yes";
			yesConnector.GlueBeginToPort(isLifeGood.GetPortByName("Left"));
			yesConnector.GlueEndToPort(goodShape.GetPortByName("Top"));
			activePage.Items.Add(yesConnector);

			NShape noConnector = connectorShapes.CreateShape(ENConnectorShape.RoutableConnector);
			noConnector.Text = "No";
			noConnector.GlueBeginToPort(isLifeGood.GetPortByName("Right"));
			noConnector.GlueEndToPort(changeSomething.GetPortByName("Top"));
			activePage.Items.Add(noConnector);

			NShape gobackConnector = connectorShapes.CreateShape(ENConnectorShape.RoutableConnector);
			gobackConnector.GlueBeginToPort(changeSomething.GetPortByName("Right"));
			gobackConnector.GlueEndToPort(isLifeGood.GetPortByName("Top"));
			activePage.Items.Add(gobackConnector);
		}

		#endregion

		#region Fields

		private NDrawingView m_DrawingView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NFileDialogsCustomizationExample.
		/// </summary>
		public static readonly NSchema NFileDialogsCustomizationExampleSchema;

		#endregion

		#region Nested Types

		private class CustomOpenCommandAction : NOpenCommandAction
		{
			public CustomOpenCommandAction()
			{
			}
			static CustomOpenCommandAction()
			{
				CustomOpenCommandActionSchema = NSchema.Create(typeof(CustomOpenCommandAction), NOpenCommandActionSchema);
			}

			/// <summary>
			/// Executes the command action.
			/// </summary>
			/// <param name="target"></param>
			/// <param name="parameter"></param>
			public override void Execute(NNode target, object parameter)
			{
				if (IsEnabled(target) == false)
					return;

				// Get the drawing view
				NDrawingView drawingView = GetDrawingView(target);

				NDrawingFormatRegistry registry = new NDrawingFormatRegistry ();
				registry.DocumentFormats = new NDrawingFormat[] {
					NDrawingFormat.Visio,
					NDrawingFormat.VectorAutoCadDxf
				};

				drawingView.OpenFile(NDrawingFormat.NevronBinary, registry, true, true);
			}

			public static readonly NSchema CustomOpenCommandActionSchema;
		}

		private class CustomSaveAsCommandAction : NSaveAsCommandAction
		{
			public CustomSaveAsCommandAction()
			{
			}
			static CustomSaveAsCommandAction()
			{
				CustomSaveAsCommandActionSchema = NSchema.Create(typeof(CustomSaveAsCommandAction), NSaveAsCommandActionSchema);
			}

			// <summary>
			/// Executes the command action.
			/// </summary>
			/// <param name="target"></param>
			/// <param name="parameter"></param>
			public override void Execute(NNode target, object parameter)
			{
				if (IsEnabled(target) == false)
					return;

				// Get the drawing view
				NDrawingView drawingView = (NDrawingView)GetDrawingView(target);

				NDrawingFormatRegistry registry = new NDrawingFormatRegistry();
				registry.DocumentFormats = new NDrawingFormat[] {
					NDrawingFormat.Visio,
					NDrawingFormat.VectorAutoCadDxf
				};

				drawingView.SaveAs(NDrawingFormat.NevronBinary, registry, true);
			}

			public static readonly NSchema CustomSaveAsCommandActionSchema;
		}

		#endregion
	}
}