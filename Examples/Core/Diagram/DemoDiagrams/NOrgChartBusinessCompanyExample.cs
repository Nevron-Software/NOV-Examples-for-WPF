using Nevron.Nov.DataStructures;
using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Layout;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Diagram.Themes;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NOrgChartBusinessCompanyExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NOrgChartBusinessCompanyExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NOrgChartBusinessCompanyExample()
		{
			NOrgChartBusinessCompanyExampleSchema = NSchema.Create(typeof(NOrgChartBusinessCompanyExample), NExampleBaseSchema);
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
			return @"<p>Demonstrates how to create an organizational chart diagram (org chart) of a business company using the following NOV Diagram features:</p>
<ul>
	<li>Theme-based colors for the title - change the page theme from the ""Design"" tab to see how the style of the title will change, too.</li>
	<li>Organizational shapes - used to represent the employees of the business company.</li>
	<li>Tip Over Tree Layout - used to arrange the org chart shapes in a compact way, placing leaf shapes in a column instead of in a row.</li>
</ul>
";
		}

		private void InitDiagram(NDrawingDocument drawingDocument)
		{
			NPage page = drawingDocument.Content.ActivePage;

			// Create a title shape
			NShape titleShape = CreateTitleShape("Business Company");
			page.Items.Add(titleShape);

			// Recursively create the org chart shapes
			CreateOrgShape(page, ExecutiveEmployee);

			// Arrange the shapes using a Layered Graph Layout
			NTipOverTreeLayout layout = new NTipOverTreeLayout();
			layout.LeafsPlacement = ENTipOverChildrenPlacement.ColRight;
			layout.RegionLayout.VerticalSpacing = 25; // Controls the spacing between the title and the diagram

			NList<NShape> shapes = page.GetShapes(false, NDiagramFilters.ShapeType2D);
			layout.Arrange(shapes.CastAll<object>(), new NDrawingLayoutContext(page));

			page.SizeToContent();
		}

		#endregion

		#region Implementation

		private NShape CreateTitleShape(string title)
		{
			NDrawingTheme theme = NDrawingTheme.MyDrawNature;

			NShape titleShape = new NShape();
			titleShape.SetBounds(0, 0, 500, 50);
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
		private NShape CreateOrgShape(NPage page, Employee employee)
		{
			NShape shape = NLibrary.OrganizationalChartShapes.CreateShape(employee.Position, OrgChartShapeSize.Width, OrgChartShapeSize.Height);
			shape.Text = employee.Name;
			shape.ImageBlock.Image = employee.Gender == ENGender.Male ?
				NResources.Image_SVG_Male_svg :
				NResources.Image_SVG_Female_svg;
			page.Items.Add(shape);

			if (employee.SubordinateEmployees != null)
			{
				for (int i = 0; i < employee.SubordinateEmployees.Length; i++)
				{
					NShape childShape = CreateOrgShape(page, employee.SubordinateEmployees[i]);
					ConnectShapes(shape, childShape);
				}
			}

			return shape;
		}
		private void ConnectShapes(NShape shape1, NShape shape2)
		{
			NRoutableConnector connector = new NRoutableConnector();
			shape1.OwnerPage.Items.Add(connector);
			connector.GlueBeginToShape(shape1);
			connector.GlueEndToShape(shape2);
		}

		#endregion

		#region Fields

		private NDrawingView m_DrawingView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NOrgChartBusinessCompanyExample.
		/// </summary>
		public static readonly NSchema NOrgChartBusinessCompanyExampleSchema;

		#endregion

		#region Constants - Data

		private Employee ExecutiveEmployee = new Employee(ENGender.Male, ENOrganizationalShape.Executive, "William Smith", 
			new Employee(ENGender.Male, ENOrganizationalShape.Manager, "Charlie Good",
				new Employee(ENGender.Male, ENOrganizationalShape.Assistant, "Peter Marshall"),
				new Employee(ENGender.Female, ENOrganizationalShape.Assistant, "Tracy Chapmann")),
			new Employee(ENGender.Male, ENOrganizationalShape.Manager, "Kevin Tylor",
				new Employee(ENGender.Female, ENOrganizationalShape.Assistant, "Jane Buckley"),
				new Employee(ENGender.Male, ENOrganizationalShape.Assistant, "Dave Zak")),
			new Employee(ENGender.Female, ENOrganizationalShape.Manager, "Patricia Holgate",
				new Employee(ENGender.Male, ENOrganizationalShape.Assistant, "Stephen Maule"),
				new Employee(ENGender.Male, ENOrganizationalShape.Assistant, "Steve Tucker"))
		);

		#endregion

		#region Constants

		private static readonly NSize OrgChartShapeSize = new NSize(120, 70);

		#endregion

		#region Nested Types

		private class Employee
		{
			public Employee(ENGender gender, ENOrganizationalShape position, string name, params Employee[] subordinateEmployees)
			{
				Gender = gender;
				Position = position;
				Name = name;
				SubordinateEmployees = subordinateEmployees;
			}

			public ENGender Gender;
			public ENOrganizationalShape Position;
			public string Name;
			public Employee[] SubordinateEmployees;
		}

		#endregion
	}
}