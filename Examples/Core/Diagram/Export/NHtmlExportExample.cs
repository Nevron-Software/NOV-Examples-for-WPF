using System;

using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NHtmlExportExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NHtmlExportExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NHtmlExportExample()
		{
			NHtmlExportExampleSchema = NSchema.Create(typeof(NHtmlExportExample), NExampleBaseSchema);
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
			NStackPanel stackPanel = new NStackPanel();

			NButton saveAsButton = new NButton("Save as Web Page...");
			saveAsButton.Click += OnSaveAsButtonClick;
			stackPanel.Add(saveAsButton);

			return stackPanel;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
    Demonstrates how to export a NOV Diagram drawing to a web page (HTML). The drawing is exported
	to SVG that gets inserted into an HTML web page. If the drawing has multiple pages, then some
	CSS and JavaScript are inserted in the HTML web page in order to create a tab navigation interface.
	Each drawing page is inserted into a tab page.
</p>
";
		}

		private void InitDiagram(NDrawingDocument drawingDocument)
		{
			NDrawing drawing = drawingDocument.Content;
			NPage activePage = drawing.ActivePage;

			drawing.ScreenVisibility.ShowGrid = false;
			drawing.ScreenVisibility.ShowPorts = false;

			CreateDiagram(activePage);

			NPage page = new NPage("Page-2");
			drawing.Pages.Add(page);
			CreateDiagram(page);
		}

		#endregion

		#region Implementation

		private void CreateDiagram(NPage page)
		{
			NBasicShapeFactory basisShapes = new NBasicShapeFactory();
			NFlowchartShapeFactory flowChartingShapes = new NFlowchartShapeFactory();
			NConnectorShapeFactory connectorShapes = new NConnectorShapeFactory();

			NShape titleShape = basisShapes.CreateShape(ENBasicShape.Rectangle);
			titleShape.Geometry.Fill = new NColorFill(NColor.LightGray);
			titleShape.Text = page.Title;
			titleShape.SetBounds(10, 10, page.Width - 20, 50);
			page.Items.Add(titleShape);

			NShape nonPrintableShape = basisShapes.CreateShape(ENBasicShape.Rectangle);
			nonPrintableShape.Text = "Non printable shape";
			nonPrintableShape.AllowPrint = false;
			nonPrintableShape.Geometry.Fill = new NColorFill(NColor.Tomato);
			nonPrintableShape.SetBounds(50, 150, 150, 50);
			page.Items.Add(nonPrintableShape);

			NShape isLifeGood = flowChartingShapes.CreateShape(ENFlowchartingShape.Decision);
			isLifeGood.Text = "Is Life Good?";
			isLifeGood.SetBounds(300, 150, 150, 100);
			isLifeGood.Geometry.Fill = new NColorFill(NColor.LightSkyBlue);
			page.Items.Add(isLifeGood);

			NShape goodShape = flowChartingShapes.CreateShape(ENFlowchartingShape.Termination);
			goodShape.Text = "Good";
			goodShape.SetBounds(200, 300, 100, 100);
			goodShape.Geometry.Fill = new NColorFill(NColor.Gold);
			page.Items.Add(goodShape);

			NShape changeSomething = flowChartingShapes.CreateShape(ENFlowchartingShape.Process);
			changeSomething.Text = "Change Something";
			changeSomething.Geometry.Fill = new NColorFill(NColor.Thistle);
			changeSomething.SetBounds(450, 300, 100, 100);
			page.Items.Add(changeSomething);

			NShape yesConnector = connectorShapes.CreateShape(ENConnectorShape.RoutableConnector);
			yesConnector.Text = "Yes";
			yesConnector.GlueBeginToPort(isLifeGood.GetPortByName("Left"));
			yesConnector.GlueEndToPort(goodShape.GetPortByName("Top"));
			page.Items.Add(yesConnector);

			NShape noConnector = connectorShapes.CreateShape(ENConnectorShape.RoutableConnector);
			noConnector.Text = "No";
			noConnector.GlueBeginToPort(isLifeGood.GetPortByName("Right"));
			noConnector.GlueEndToPort(changeSomething.GetPortByName("Top"));
			page.Items.Add(noConnector);

			NShape gobackConnector = connectorShapes.CreateShape(ENConnectorShape.RoutableConnector);
			gobackConnector.GlueBeginToPort(changeSomething.GetPortByName("Right"));
			gobackConnector.GlueEndToPort(isLifeGood.GetPortByName("Top"));
			page.Items.Add(gobackConnector);
		}

		#endregion

		#region Event Handlers

		private void OnSaveAsButtonClick(NEventArgs arg)
		{
			string fileName = m_DrawingView.Drawing.Information.FileName;
			if (String.IsNullOrEmpty(fileName) || !fileName.EndsWith("vsdx", StringComparison.OrdinalIgnoreCase))
			{
				// The document has not been saved, yet, so set a file name with HTML extension
				// to make the default Save As dialog show Web Page as file save as type
				m_DrawingView.Drawing.Information.FileName = "Document1.html";
			}

            m_DrawingView.SaveAs();
		}

		#endregion

		#region Fields

		private NDrawingView m_DrawingView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NHtmlExportExample.
		/// </summary>
		public static readonly NSchema NHtmlExportExampleSchema;

		#endregion
	}
}