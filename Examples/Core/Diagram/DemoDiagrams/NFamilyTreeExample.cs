using Nevron.Nov.DataStructures;
using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Layout;
using Nevron.Nov.Diagram.Shapes;
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
			// Create a drawing view with a ribbon
			NDrawingViewWithRibbon drawingViewWithRibbon = new NDrawingViewWithRibbon();
			m_DrawingView = drawingViewWithRibbon.View;

            // hide grid and ports
            m_DrawingView.Content.ScreenVisibility.ShowGrid = false;
            m_DrawingView.Content.ScreenVisibility.ShowPorts = false;

            // Create a family tree diagram
            m_DrawingView.Document.HistoryService.Pause();
			InitDiagram(m_DrawingView.Document);

			return drawingViewWithRibbon;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();

			// Get the family tree drawing extension
			NFamilyTreeExtension familyTreeExtension = (NFamilyTreeExtension)m_DrawingView.Content.Extensions.FindByType(NFamilyTreeExtension.FamilyTreeExtensionType);

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

			// Set the family tree extension to the drawing to activate the "Family Tree" ribbon tab
			drawing.Extensions = new NDiagramExtensionCollection();
			drawing.Extensions.Add(new NFamilyTreeExtension());

            // Create a family tree diagram in the active page of the drawing
            CreateFamilyTree(drawing.ActivePage);
		}
		private void CreateFamilyTree(NPage page)
		{
			// Create the parents
			NShape fatherShape = NLibrary.FamilyTreeShapes.CreateShape(ENFamilyTreeShape.Male);
			fatherShape.SetShapePropertyValue("FirstName", "Abraham");
			fatherShape.SetShapePropertyValue("LastName", "Lincoln");
			fatherShape.SetShapePropertyValue("BirthDate", new NMaskedDateTime(1809, 02, 12));// NMaskedDateTime
			fatherShape.SetShapePropertyValue("DeathDate", new NMaskedDateTime(1865, 04, 15));
			page.Items.Add(fatherShape);

			NShape motherShape = NLibrary.FamilyTreeShapes.CreateShape(ENFamilyTreeShape.Female);
			motherShape.SetShapePropertyValue("FirstName", "Mary");
			motherShape.SetShapePropertyValue("LastName", "Todd");
			motherShape.SetShapePropertyValue("BirthDate", new NMaskedDateTime(1811));
			page.Items.Add(motherShape);

			// Create the children
			NShape childShape1 = NLibrary.FamilyTreeShapes.CreateShape(ENFamilyTreeShape.Male);
			childShape1.SetShapePropertyValue("FirstName", "Thomas");
			childShape1.SetShapePropertyValue("LastName", "Lincoln");
			childShape1.SetShapePropertyValue("BirthDate", new NMaskedDateTime(1853, 4, 4));
			childShape1.SetShapePropertyValue("DeathDate", new NMaskedDateTime(1871));
			page.Items.Add(childShape1);

			NShape childShape2 = NLibrary.FamilyTreeShapes.CreateShape(ENFamilyTreeShape.Male);
			childShape2.SetShapePropertyValue("FirstName", "Robert Todd");
			childShape2.SetShapePropertyValue("LastName", "Lincoln");
			childShape2.SetShapePropertyValue("BirthDate", new NMaskedDateTime(1843, 8, 1));
			childShape2.SetShapePropertyValue("DeathDate", new NMaskedDateTime(1926, 7, 26));
			page.Items.Add(childShape2);

			NShape childShape3 = NLibrary.FamilyTreeShapes.CreateShape(ENFamilyTreeShape.Male);
			childShape3.SetShapePropertyValue("FirstName", "William Wallace");
			childShape3.SetShapePropertyValue("LastName", "Lincoln");
			childShape3.SetShapePropertyValue("BirthDate", new NMaskedDateTime(1850, 12, 21));
			childShape3.SetShapePropertyValue("DeathDate", new NMaskedDateTime(1862, 2, 20));
			page.Items.Add(childShape3);

			NShape childShape4 = NLibrary.FamilyTreeShapes.CreateShape(ENFamilyTreeShape.Male);
			childShape4.SetShapePropertyValue("FirstName", "Edward Baker");
			childShape4.SetShapePropertyValue("LastName", "Lincoln");
			childShape4.SetShapePropertyValue("BirthDate", new NMaskedDateTime(1846, 3, 10));
			childShape4.SetShapePropertyValue("DeathDate", new NMaskedDateTime(1850, 2, 1));
			page.Items.Add(childShape4);

			// Create the relationship shape
			NShape relShape = NLibrary.FamilyTreeShapes.CreateShape(ENFamilyTreeShape.Relationship);
			relShape.SetShapePropertyValue("MarriageDate", new NMaskedDateTime(1842, 11, 4));
			page.Items.Add(relShape);

			page.Items.Add(CreateConnector(fatherShape, relShape));
			page.Items.Add(CreateConnector(motherShape, relShape));

			page.Items.Add(CreateConnector(relShape, childShape1));
			page.Items.Add(CreateConnector(relShape, childShape2));
			page.Items.Add(CreateConnector(relShape, childShape3));
			page.Items.Add(CreateConnector(relShape, childShape4));

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