using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Dom;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
    public class NRibbonAndCommandBarsExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NRibbonAndCommandBarsExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NRibbonAndCommandBarsExample()
		{
			NRibbonAndCommandBarsExampleSchema = NSchema.Create(typeof(NRibbonAndCommandBarsExample), NExampleBaseSchema);
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

			// Create and execute a ribbon UI builder
			m_RibbonBuilder = new NDiagramRibbonBuilder();
			return m_RibbonBuilder.CreateUI(m_DrawingView);
		}
		protected override NWidget CreateExampleControls()
		{
			// Switch UI button
			NButton switchUIButton = new NButton(SwitchToCommandBars);
			switchUIButton.VerticalPlacement = ENVerticalPlacement.Top;
			switchUIButton.Click += OnSwitchUIButtonClick;

			return switchUIButton;
		}
		protected override string GetExampleDescription()
		{
			return "<p>This example demonstrates how to switch the NOV Diagram commanding interface between ribbon and command bars.</p>";
		}

		private void InitDiagram(NDrawingDocument drawingDocument)
		{
			NBasicShapeFactory factory = new NBasicShapeFactory();
			NShape shape = factory.CreateShape(ENBasicShape.Rectangle);
			shape.SetBounds(100, 100, 150, 100);
			drawingDocument.Content.ActivePage.Items.Add(shape);
		}

		#endregion

		#region Implementation

		private void SetUI(NCommandUIHolder oldUiHolder, NWidget widget)
		{
			if (oldUiHolder.ParentNode is NTabPage)
			{
				((NTabPage)oldUiHolder.ParentNode).Content = widget;
			}
			else if (oldUiHolder.ParentNode is NPairBox)
			{
				((NPairBox)oldUiHolder.ParentNode).Box1 = widget;
			}
		}

		#endregion

		#region Event Handlers

		private void OnSwitchUIButtonClick(NEventArgs arg)
		{
			NButton switchUIButton = (NButton)arg.TargetNode;
			NLabel label = (NLabel)switchUIButton.Content;

			// Remove the drawing view from its parent
			NCommandUIHolder uiHolder = m_DrawingView.GetFirstAncestor<NCommandUIHolder>();
			m_DrawingView.ParentNode.RemoveChild(m_DrawingView);

			if (label.Text == SwitchToRibbon)
			{
				// We are in "Command Bars" mode, so switch to "Ribbon"
				label.Text = SwitchToCommandBars;

				// Create the ribbon
				SetUI(uiHolder, m_RibbonBuilder.CreateUI(m_DrawingView));
			}
			else
			{
				// We are in "Ribbon" mode, so switch to "Command Bars"
				label.Text = SwitchToRibbon;

				// Create the command bars
				if (m_CommandBarBuilder == null)
				{
					m_CommandBarBuilder = new NDiagramCommandBarBuilder();
				}

				SetUI(uiHolder, m_CommandBarBuilder.CreateUI(m_DrawingView));
			}
		}

		#endregion

		#region Fields

		private NDrawingView m_DrawingView;
		private NDiagramRibbonBuilder m_RibbonBuilder;
		private NDiagramCommandBarBuilder m_CommandBarBuilder;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NRibbonAndCommandBarsSwitchingExample.
		/// </summary>
		public static readonly NSchema NRibbonAndCommandBarsExampleSchema;

		#endregion

		#region Constants

		private const string SwitchToCommandBars = "Switch to Command Bars";
		private const string SwitchToRibbon = "Switch to Ribbon";

		#endregion
	}
}