using Nevron.Nov.DataStructures;
using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Layout;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NFamilyTreeExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NFamilyTreeExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NFamilyTreeExample()
		{
			NFamilyTreeExampleSchema = NSchema.Create(typeof(NFamilyTreeExample), NExampleBaseSchema);
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
			NStackPanel stack = new NStackPanel();

			// Get the family tree drawing extension
			NFamilyTreeExtension familyTreeExtension = m_DrawingView.Content.Extensions.FindByType<NFamilyTreeExtension>();

			// Create property editors
			NList<NPropertyEditor> propertyEditors = NDesigner.GetDesigner(familyTreeExtension).CreatePropertyEditors(familyTreeExtension,
				NFamilyTreeExtension.DateFormatProperty,
				NFamilyTreeExtension.ShowPhotosProperty
			);

			for (int i = 0; i < propertyEditors.Count; i++)
			{
				stack.Add(propertyEditors[i]);
			}

			return new NUniSizeBoxGroup(stack);
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create and arrange Family Tree diagrams. Use the controls
	on the right to change the family tree settings. You can also click the ""Settings"" button
	in the ""Family Tree"" contextual ribbon tab.
</p>";
		}

		#endregion

		#region Implementation

		private void InitDiagram(NDrawingDocument drawingDocument)
		{
			// Get drawing and the active page
			NDrawing drawing = drawingDocument.Content;
			NPage page = drawing.ActivePage;

			// Set the family tree extension to the drawing to activate the "Family Tree" ribbon tab
			drawing.Extensions = new NDiagramExtensionCollection();
			drawing.Extensions.Add(new NFamilyTreeExtension());

			// Create 3 person shapes
			NPersonShape fatherShape = new NPersonShape(ENGender.Male, "Abraham", "Lincoln",
				new NDateTime(1809, 02, 12), new NDateTime(1865, 04, 15));
			page.Items.Add(fatherShape);

			NPersonShape motherShape = new NPersonShape(ENGender.Female, "Mary", "Todd",
				new NDateTime(1811), null);
			page.Items.Add(motherShape);

			NPersonShape childShape1 = new NPersonShape(ENGender.Male, "Thomas", "Lincoln",
				new NDateTime(1853, 4, 4), new NDateTime(1871));
			page.Items.Add(childShape1);

			NPersonShape childShape2 = new NPersonShape(ENGender.Male, "Robert Todd", "Lincoln",
				new NDateTime(1843, 8, 1), new NDateTime(1926, 7, 26));
			page.Items.Add(childShape2);

			NPersonShape childShape3 = new NPersonShape(ENGender.Male, "William Wallace", "Lincoln",
				new NDateTime(1850, 12, 21), new NDateTime(1862, 2, 20));
			page.Items.Add(childShape3);

			NPersonShape childShape4 = new NPersonShape(ENGender.Male, "Edward Baker", "Lincoln",
				new NDateTime(1846, 3, 10), new NDateTime(1850, 2, 1));
			page.Items.Add(childShape4);

			// Create a family shape
			NFamilyShape familyShape = new NFamilyShape();
			familyShape.Marriage = new NFamilyTreeEvent(new NDateTime(1842, 11, 4));
			page.Items.Add(familyShape);

			page.Items.Add(CreateConnector(fatherShape, familyShape));
			page.Items.Add(CreateConnector(motherShape, familyShape));

			page.Items.Add(CreateConnector(familyShape, childShape1));
			page.Items.Add(CreateConnector(familyShape, childShape2));
			page.Items.Add(CreateConnector(familyShape, childShape3));
			page.Items.Add(CreateConnector(familyShape, childShape4));

			// Arrange the family tree shapes
			NFamilyGraphLayout layout = new NFamilyGraphLayout();
			object[] shapes = page.GetShapes(false).ToArray<object>();
			layout.Arrange(shapes, new NDrawingLayoutContext(page));

			// Size the page to its content
			page.SizeToContent();
		}

		#endregion

		#region Fields

		private NDrawingView m_DrawingView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NFamilyTreeExample.
		/// </summary>
		public static readonly NSchema NFamilyTreeExampleSchema;

		#endregion

		#region Static Methods

		private static NRoutableConnector CreateConnector(NShape fromShape, NShape toShape)
		{
			NRoutableConnector connector = new NRoutableConnector();
			connector.GlueBeginToShape(fromShape);
			connector.GlueEndToShape(toShape);

			return connector;
		}

		#endregion
	}
}