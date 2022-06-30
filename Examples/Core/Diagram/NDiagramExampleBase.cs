using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Expressions;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
    public abstract class NDiagramExampleBase : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NDiagramExampleBase()
        {
        }
        /// <summary>
        /// Static constructor.
        /// </summary>
        static NDiagramExampleBase()
        {
            NDiagramExampleBaseSchema = NSchema.Create(typeof(NDiagramExampleBase), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Protected Overrides - Example

        protected override NWidget CreateExampleContent()
        {
            // Create a simple drawing
            m_DrawingView = new NDrawingView();
            m_DrawingDocument = m_DrawingView.Document;

            m_DrawingView.Document.HistoryService.Pause();
            try
            {
                InitDiagram();
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
			NStackPanel stack = new NStackPanel();
			
			// Switch UI button
			NButton switchUIButton = new NButton(SwitchToCommandBars);
			switchUIButton.Click += OnSwitchUIButtonClick;
			stack.Add(switchUIButton);

			return stack;
		}

        #endregion

        #region Protected Overridable

		protected virtual void InitDiagram()
		{
			NShape shape = new NShape();

			// ... 

			// left
			NPort left = new NPort(0, 0.5d, true);
			left.SetDirection(ENBoxDirection.Left);
			shape.Ports.Add(left);

			// top
			NPort top = new NPort(0.5d, 0.0f, true);
			top.SetDirection(ENBoxDirection.Up);
			shape.Ports.Add(top);

			// right
			NPort right = new NPort(1.0d, 0.5d, true);
			right.SetDirection(ENBoxDirection.Right);
			shape.Ports.Add(right);

			// bottom
			NPort bottom = new NPort(0.5d, 1.0f, true);
			bottom.SetDirection(ENBoxDirection.Down);
			shape.Ports.Add(bottom);
		}

		protected virtual void MoveTextBelowShape(NShape shape)
		{
			NTextBlock textBlock = shape.GetTextBlock();
			textBlock.Padding = new NMargins(0, 5, 0, 0);
			textBlock.ResizeMode = ENTextBlockResizeMode.TextSize;
			textBlock.SetFx(NTextBlock.PinYProperty, new NShapeHeightFactorFx(1.0));
			textBlock.LocPinY = 0;
		}		

		#endregion

		#region Implementation

		private NShape CreateTrapezoidShape(double width, double height, double pinX, double pinY)
        {
            NShape shape = new NShape();
            shape.Init2DShape();

            shape.Width = width;
            shape.Height = height;

            shape.PinX = pinX;
            shape.PinY = pinY;

            // add controls
            NControl control = new NControl();
            control.SetFx(NControl.XProperty, new NShapeWidthFactorFx(0.3));
            control.Y = 0.0d;
            control.XBehavior = ENCoordinateBehavior.OffsetFromMax;
            control.YBehavior = ENCoordinateBehavior.Locked;
            control.Tooltip = "Modify strip width";
            shape.Controls.Add(control);

            // add a geometry
            NGeometry geometry1 = new NGeometry();
            {
                NMoveTo plotFigure = 
                geometry1.MoveTo("MIN(Controls.0.X,Width-Controls.0.X)", 0.0d);
                geometry1.LineTo("Width-Geometry.0.X", 0.0d);
                geometry1.LineTo(new NShapeWidthFactorFx(1), new NShapeHeightFactorFx(1));
                geometry1.LineTo(0.0d, "Height");
                geometry1.LineTo("Geometry.0.X", "Geometry.0.Y");
                plotFigure.CloseFigure = true;
            }
            shape.Geometry = geometry1;

            // add ports
            // top
            NPort port = new NPort();
            port.SetFx(NPort.XProperty, new NShapeWidthFactorFx(0.5));
            port.Y = 0.0d;
            port.GlueMode = ENPortGlueMode.Outward;
            port.DirX = 0.0d;
            port.DirY = -1;
            shape.Ports.Add(port);

            // right
            port = new NPort();
            port.SetFx(NPort.XProperty, new NShapeWidthFactorFx(1));
            port.SetFx(NPort.YProperty, new NShapeHeightFactorFx(0.5));
            port.GlueMode = ENPortGlueMode.InwardAndOutward;
            port.DirX = 1;
            port.DirY = 0.0d;
            shape.Ports.Add(port);

            // bottom
            port = new NPort();
            port.SetFx(NPort.XProperty, "Controls.0.X");
            port.SetFx(NPort.YProperty, new NShapeHeightFactorFx(1));
            port.DirX = 0.0d;
            port.DirY = 1;
            shape.Ports.Add(port);

            // left
            port = new NPort();
            port.X = 0.0d;
            port.SetFx(NPort.YProperty, new NShapeHeightFactorFx(0.5));
            port.DirX = -1;
            port.DirY = 0.0d;
            shape.Ports.Add(port);

            // shape.FillStyle = new NColorFillStyle(Color.Gray);
            shape.Geometry.Stroke = new NStroke(1, NColor.Black);
            shape.Geometry.Stroke.LineJoin = ENLineJoin.Miter;

            /*			NShadow shadow = new NShadow(NColor.Green, 50, 50);
                        shadow.ScalePinPoint = new NPoint(0.5, 0.5);
                        shadow.Scale = 1.0;
                        shadow.UseFillAndStrokeAlpha = false;
                        shadow.ApplyToFilling = true;
                        shadow.ApplyToOutline = true;
                        shape.Shadow = shadow;*/

            NStackPanel stack = new NStackPanel();

            NButton button = new NButton("Hello Joro");
            //button.Click += new Function<NEventArgs>(button_Click);
            stack.Add(button);

            NLabel label = new NLabel("Hello World");
            stack.Add(label);
            //shape.Widget = stack;
            //shape.Widget = new NLabel("Hello World");

            return shape;
        }
        private NShape CreateFlexiArrow2Shape(NPoint from, NPoint to)
        {
            NShape shape = new NShape();
            shape.Init1DShape(EN1DShapeXForm.Vector);

            shape.Height = 0;
            shape.BeginX = from.X;
            shape.BeginY = from.Y;
            shape.EndX = to.X;
            shape.EndY = to.Y;

            // add controls
            NControl controlPoint = new NControl();
            controlPoint.SetFx("X", "13.0956");
            controlPoint.SetFx("Y", "Height*0.75");
            controlPoint.Visible = true;
            controlPoint.XBehavior = ENCoordinateBehavior.OffsetFromMin;
            controlPoint.YBehavior = ENCoordinateBehavior.OffsetFromMid;
            controlPoint.Tooltip = "Modify arrowhead 1";
            shape.Controls.AddChild(controlPoint);

            controlPoint = new NControl();
            controlPoint.SetFx("X", "Width-40");
            controlPoint.SetFx("Y", "Height*1");
            controlPoint.Visible = true;
            controlPoint.XBehavior = ENCoordinateBehavior.OffsetFromMax;
            controlPoint.YBehavior = ENCoordinateBehavior.OffsetFromMid;
            controlPoint.Tooltip = "Modify arrowhead 2";
            shape.Controls.AddChild(controlPoint);

            controlPoint = new NControl();
            controlPoint.SetFx("X", "Width-20");
            controlPoint.SetFx("Y", "Height*1");
            controlPoint.Visible = true;
            controlPoint.XBehavior = ENCoordinateBehavior.OffsetFromMax;
            controlPoint.YBehavior = ENCoordinateBehavior.OffsetFromMid;
            controlPoint.Tooltip = "Modify arrowhead 3";
            shape.Controls.AddChild(controlPoint);

            // add a geometry
            NGeometry geometry = new NGeometry();
            NMoveTo plotFigure =
            geometry.MoveTo("Width*0", "Height*0.5");
            geometry.LineTo("Controls.0.X", "ABS(Controls.0.Y)");
            geometry.LineTo("Controls.1.X", "ABS(Controls.1.Y)");
            geometry.LineTo("Controls.2.X", "ABS(Controls.2.Y)");
            geometry.LineTo("Width", "Height*0.5");
            geometry.LineTo("Controls.2.X", "Height-Geometry.3.Y");
            geometry.LineTo("Controls.1.X", "Height-Geometry.2.Y");
            geometry.LineTo("Controls.0.X", "Height-Geometry.1.Y");
            geometry.LineTo("Geometry.0.X", "Geometry.0.Y");
            plotFigure.CloseFigure = true;
            shape.Geometry = geometry;

            return shape;
        }

        #endregion

		#region Event Handlers

		private void OnSwitchUIButtonClick(NEventArgs arg)
		{
			NButton switchUIButton = (NButton)arg.TargetNode;
			NLabel label = (NLabel)switchUIButton.Content;

			// Remove the rich text view from its parent
			m_DrawingView.ParentNode.RemoveChild(m_DrawingView);

			if (label.Text == SwitchToRibbon)
			{
				// We are in "Command Bars" mode, so switch to "Ribbon"
				label.Text = SwitchToCommandBars;

				// Create the ribbon
				m_ExampleTabPage.Content = m_RibbonBuilder.CreateUI(m_DrawingView);
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

				m_ExampleTabPage.Content = m_CommandBarBuilder.CreateUI(m_DrawingView);
			}
		}

		#endregion

		#region Fields

		protected NDrawingView m_DrawingView;
        protected NDrawingDocument m_DrawingDocument;
		protected NDiagramRibbonBuilder m_RibbonBuilder;
		protected NDiagramCommandBarBuilder m_CommandBarBuilder;

		#endregion

		#region Schema

		/// <summary>
        /// Schema associated with NDiagramExampleBase.
        /// </summary>
        public static readonly NSchema NDiagramExampleBaseSchema;

        #endregion

		#region Constants

		private const string SwitchToCommandBars = "Switch to Command Bars";
		private const string SwitchToRibbon = "Switch to Ribbon";

		#endregion
	}
}