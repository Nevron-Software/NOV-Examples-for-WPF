using Nevron.Nov.DataStructures;
using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.DataVisualizer;
using Nevron.Nov.Diagram.Layout;
using Nevron.Nov.Diagram.Themes;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NOrgChartImportExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NOrgChartImportExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NOrgChartImportExample()
		{
			NOrgChartImportExampleSchema = NSchema.Create(typeof(NOrgChartImportExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create a simple drawing
			NDrawingViewWithRibbon drawingViewWithRibbon = new NDrawingViewWithRibbon();
			m_DrawingView = drawingViewWithRibbon.View;

            // hide grid and ports
            m_DrawingView.Content.ScreenVisibility.ShowGrid = false;
            m_DrawingView.Content.ScreenVisibility.ShowPorts = false;

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
			return @"<p>Demonstrates how to create an organizational chart diagram (org chart) from an Excel spreadsheet.</p>";
		}

		private void InitDiagram(NDrawingDocument drawingDocument)
		{
			NPage page = drawingDocument.Content.ActivePage;

			// Create an org chart from an Excel spreadsheet
			NOrganizationChartVisualizer importer = new NOrganizationChartVisualizer();
			importer.Columns.Add("Id", "ID");
			importer.Columns.Add("Name", "Employee Name");
			importer.Columns.Add("ReportsTo", "Reports to");
			importer.Columns.Add("Position", "Position");
			importer.BuildDiagramFromStream(m_DrawingView, NResources.RBIN_XLSX_Organization_xlsx.Stream);

			// Add a title shape
			NShape titleShape = CreateTitleShape("Org Chart Import");
			page.Items.Add(titleShape);

			page.SizeToContent();
		}

		#endregion

		#region Implementation

		private NShape CreateTitleShape(string title)
		{
			NDrawingTheme theme = NDrawingTheme.MyDrawNature;

			NShape titleShape = new NShape();
			titleShape.SetBounds(0, -75, 350, 50);
			titleShape.Text = title;

			NTextBlock titleTextBlock = (NTextBlock)titleShape.TextBlock;
			titleTextBlock.ResizeMode = ENTextBlockResizeMode.ShapeSize;
			titleTextBlock.FontSize = 28;
			titleTextBlock.FontStyleBold = true;

			// Set theme-based colors to the title text, so that it changes when the user changes the theme
			NColor strokeColor = theme.ColorPalette.Variants[0][0];
			strokeColor.Tag = new NThemeVariantColorInfo(0);
			titleTextBlock.Stroke = new NStroke(strokeColor);

			NColor fillColor = theme.ColorPalette.Variants[0][4];
			fillColor.Tag = new NThemeVariantColorInfo(4);
			titleTextBlock.Fill = new NColorFill(fillColor);

			// Alternatively, you can also use fixed colors (uncomment the 2 lines below)
			//titleTextBlock.Stroke = new NStroke(NColor.DarkBlue);
			//titleTextBlock.Fill = new NColorFill(NColor.LightBlue);

			// Set an expression to center the title horizontally in the page
			titleShape.SetFx(NShape.PinXProperty, new NFormulaFx("$ParentSheet.X + $ParentSheet.Width / 2", true));

			return titleShape;
		}

		#endregion

		#region Fields

		private NDrawingView m_DrawingView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NOrgChartImportExample.
		/// </summary>
		public static readonly NSchema NOrgChartImportExampleSchema;

		#endregion
	}
}