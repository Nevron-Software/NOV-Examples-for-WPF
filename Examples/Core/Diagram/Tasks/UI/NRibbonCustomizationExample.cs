using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.DrawingCommands;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Diagram.UI;
using Nevron.Nov.Dom;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NRibbonCustomizationExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NRibbonCustomizationExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NRibbonCustomizationExample()
		{
			NRibbonCustomizationExampleSchema = NSchema.Create(typeof(NRibbonCustomizationExample), NExampleBaseSchema);
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

			// Create and customize a ribbon UI builder
			m_RibbonBuilder = new NDiagramRibbonBuilder();

			// Add the custom command action to the drawing view's commander
			m_DrawingView.Commander.Add(new CustomCommandAction());

			// Rename the "Home" ribbon tab page
			NRibbonTabPageBuilder homeTabBuilder = m_RibbonBuilder.TabPageBuilders[NDiagramRibbonBuilder.TabPageHomeName];
			homeTabBuilder.Name = "Start";

			// Rename the "Text" ribbon group of the "Home" tab page
			NRibbonGroupBuilder fontGroupBuilder = homeTabBuilder.RibbonGroupBuilders[NHomeTabPageBuilder.GroupTextName];
			fontGroupBuilder.Name = "Custom Name";

			// Remove the "Clipboard" ribbon group from the "Home" tab page
			homeTabBuilder.RibbonGroupBuilders.Remove(NHomeTabPageBuilder.GroupClipboardName);

			// Insert the custom ribbon group at the beginning of the home tab page
			homeTabBuilder.RibbonGroupBuilders.Insert(0, new CustomRibbonGroupBuilder());

			return m_RibbonBuilder.CreateUI(m_DrawingView);
		}
		protected override NWidget CreateExampleControls()
		{
			return null;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to customize the NOV diagram ribbon.</p>";
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
		private NDiagramRibbonBuilder m_RibbonBuilder;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NRibbonCustomizationExample.
		/// </summary>
		public static readonly NSchema NRibbonCustomizationExampleSchema;

		#endregion

		#region Constants

		public static readonly NCommand CustomCommand = NCommand.Create(typeof(Nevron.Nov.Examples.Diagram.NRibbonCustomizationExample),
			"CustomCommand", "Custom Command");

		#endregion

		#region Nested Types

		public class CustomRibbonGroupBuilder : NRibbonGroupBuilder
		{
			#region Constructors

			public CustomRibbonGroupBuilder()
				: base("Custom Group", NResources.Image_Ribbon_16x16_smiley_png)
			{
			}

			#endregion

			#region Protected Overrides

			protected override void AddRibbonGroupItems(NRibbonGroupItemCollection items)
			{
				// Add the "Copy" command
				items.Add(CreateRibbonButton(NResources.Image_Ribbon_32x32_clipboard_copy_png,
					Nevron.Nov.Presentation.NResources.Image_Edit_Copy_png, NDrawingView.CopyCommand));

				// Add the custom command
				items.Add(CreateRibbonButton(NResources.Image_Ribbon_32x32_smiley_png,
					NResources.Image_Ribbon_16x16_smiley_png, CustomCommand));
			}

			#endregion
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