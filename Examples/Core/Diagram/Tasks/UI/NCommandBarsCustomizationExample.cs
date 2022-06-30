using Nevron.Nov.Dom;
using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.DrawingCommands;
using Nevron.Nov.UI;
using Nevron.Nov.Diagram.Shapes;

namespace Nevron.Nov.Examples.Diagram
{
	public class NCommandBarsCustomizationExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NCommandBarsCustomizationExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NCommandBarsCustomizationExample()
		{
			NCommandBarsCustomizationExampleSchema = NSchema.Create(typeof(NCommandBarsCustomizationExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create a simple drawing
			m_DrawingView = new NDrawingView();

			m_DrawingView.Document.HistoryService.Pause();
			try
			{
				InitDiagram(m_DrawingView.Document);
			}
			finally
			{
				m_DrawingView.Document.HistoryService.Resume();
			}

			// Create and customize a command bar UI builder
			m_CommandBarBuilder = new NDiagramCommandBarBuilder();

			// Add the custom command action to the drawing view's commander
			m_DrawingView.Commander.Add(new CustomCommandAction());

			// Remove the "Edit" menu and insert a custom one
			m_CommandBarBuilder = new NDiagramCommandBarBuilder();
			m_CommandBarBuilder.MenuDropDownBuilders.Remove(NDiagramCommandBarBuilder.MenuEditName);
			m_CommandBarBuilder.MenuDropDownBuilders.Insert(1, new CustomMenuBuilder());

			// Remove the "Standard" toolbar and insert a custom one
			m_CommandBarBuilder.ToolBarBuilders.Remove(NDiagramCommandBarBuilder.ToolbarStandardName);
			m_CommandBarBuilder.ToolBarBuilders.Insert(0, new CustomToolBarBuilder());

			return m_CommandBarBuilder.CreateUI(m_DrawingView);
		}
		protected override NWidget CreateExampleControls()
		{
			return null;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>This example demonstrates how to customize the NOV diagram command bars (menus and toolbars).</p>
";
		}

		private void InitDiagram(NDrawingDocument drawingDocument)
		{
			NBasicShapeFactory factory = new NBasicShapeFactory();
			NShape shape = factory.CreateShape(ENBasicShape.Rectangle);
			shape.SetBounds(100, 100, 150, 100);
			drawingDocument.Content.ActivePage.Items.Add(shape);
		}

		#endregion

		#region Fields

		private NDrawingView m_DrawingView;
		private NDiagramCommandBarBuilder m_CommandBarBuilder;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NCommandBarsCustomizationExample.
		/// </summary>
		public static readonly NSchema NCommandBarsCustomizationExampleSchema;

		#endregion

		#region Constants

		public static readonly NCommand CustomCommand = NCommand.Create(typeof(Nevron.Nov.Examples.Diagram.NCommandBarsCustomizationExample),
			"CustomCommand", "Custom Command");

		#endregion

		#region Nested Types

		public class CustomMenuBuilder : NMenuDropDownBuilder
		{
			public CustomMenuBuilder()
				: base("Custom Menu")
			{
			}

			protected override void AddItems(NMenuItemCollection items)
			{
				// Add the "Copy" menu item
				items.Add(CreateMenuItem(Nevron.Nov.Presentation.NResources.Image_Edit_Copy_png, NDrawingView.CopyCommand));

				// Add the custom command menu item
				items.Add(CreateMenuItem(NResources.Image_Ribbon_16x16_smiley_png, CustomCommand));
			}
		}

		public class CustomToolBarBuilder : NToolBarBuilder
		{
			public CustomToolBarBuilder()
				: base("Custom Toolbar")
			{
			}

			protected override void AddItems(NCommandBarItemCollection items)
			{
				// Add the "Copy" button
				items.Add(CreateButton(Nevron.Nov.Presentation.NResources.Image_Edit_Copy_png, NDrawingView.CopyCommand));

				// Add the custom command button
				items.Add(CreateButton(NResources.Image_Ribbon_16x16_smiley_png, CustomCommand));
			}
		}

		public class CustomCommandAction : NDrawingCommandAction
		{
			#region Constructors

			/// <summary>
			/// Default constructor.
			/// </summary>
			public CustomCommandAction()
			{
			}

			/// <summary>
			/// Static constructor.
			/// </summary>
			static CustomCommandAction()
			{
				CustomCommandActionSchema = NSchema.Create(typeof(CustomCommandAction), NDrawingCommandAction.NDrawingCommandActionSchema);
			}

			#endregion

			#region Public Overrides

			/// <summary>
			/// Gets the command associated with this command action.
			/// </summary>
			/// <returns></returns>
			public override NCommand GetCommand()
			{
				return CustomCommand;
			}
			/// <summary>
			/// Executes the command action.
			/// </summary>
			/// <param name="target"></param>
			/// <param name="parameter"></param>
			public override void Execute(NNode target, object parameter)
			{
				NDrawingView drawingView = GetDrawingView(target);

				NMessageBox.Show("Drawing Custom Command executed!", "Custom Command", ENMessageBoxButtons.OK,
					ENMessageBoxIcon.Information);
			}

			#endregion

			#region Schema

			/// <summary>
			/// Schema associated with CustomCommandAction.
			/// </summary>
			public static readonly NSchema CustomCommandActionSchema;

			#endregion
		}

		#endregion
	}
}