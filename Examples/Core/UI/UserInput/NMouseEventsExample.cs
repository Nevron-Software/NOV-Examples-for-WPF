using System;
using System.IO;
using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Globalization;
using Nevron.Nov.Graphics;
using Nevron.Nov.IO;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
    public class NMouseEventsExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NMouseEventsExample()
        {
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static NMouseEventsExample()
        {
            NMouseEventsExampleSchema = NSchema.Create(typeof(NMouseEventsExample), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            m_Canvas = new NCanvas();
            m_Canvas.PrePaint += OnCanvasPrePaint;

            // subscribe for mouse events
            m_Canvas.MouseDown += OnCanvasMouseDown;
            m_Canvas.MouseUp += OnCanvasMouseUp;
            m_Canvas.MouseMove += OnCanvasMouseMove;
            m_Canvas.MouseWheel += OnCanvasMouseWheel;
            m_Canvas.MouseIn += OnCanvasMouseIn;
            m_Canvas.MouseOut += OnCanvasMouseOut;
            m_Canvas.GotMouseCapture += OnCanvasGotMouseCapture;
            m_Canvas.LostMouseCapture += OnCanvasLostMouseCapture;

            return m_Canvas;
        }
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();
            stack.FitMode = ENStackFitMode.Last;
            stack.FillMode = ENStackFillMode.Last;

            // track move events
            m_LogMoveEventsCheck = new NCheckBox("Track Move Events");
            stack.Add(m_LogMoveEventsCheck);

            // capture mouse on left mouse down
            m_CaptureMouseOnLeftMouseDown = new NCheckBox("Capture Mouse on Left MouseDown");
            stack.Add(m_CaptureMouseOnLeftMouseDown);

            // capture mouse on right mouse down
            m_CaptureMouseOnRightMouseDown = new NCheckBox("Capture Mouse on Right MouseDown");
            stack.Add(m_CaptureMouseOnRightMouseDown);

            // Create the events log
            m_EventsLog = new NExampleEventsLog();
            stack.Add(m_EventsLog);

            return stack;
        }
        protected override string GetExampleDescription()
        {
            return @"
<p>
	Demonstrates the mouse support in NOV. Click and move the mouse over the canvas to explore the mouse events that NOV sends to the application.
    Mouse support is available to most desktop enviroments.
</p>
";
        }

        #endregion

        #region Event Handlers

        void OnCanvasPrePaint(NCanvasPaintEventArgs arg)
        {
            NPaintVisitor visitor = arg.PaintVisitor;

            // paint background
            visitor.SetFill(NColor.Ivory);
            visitor.PaintRectangle(m_Canvas.GetContentEdge());

            // paint mouse
            if (NMouse.IsOver(m_Canvas))
            {
                // define some metrics first
                double buttonWidth = 10;
                double buttonHeight = 15;
                double buttonsOffset = 5;
                NSize pointerSize = new NSize(5, 5);
                NPoint buttonsCenter = new NPoint(m_MouseLocation.X, m_MouseLocation.Y + buttonsOffset + buttonHeight / 2);
                NRectangle buttonsFrame = NRectangle.FromCenterAndSize(buttonsCenter, buttonWidth * 3, buttonHeight);

                // paint left button, if down
                if (m_LeftMouseDown)
                {
                    NRectangle buttonRect = new NRectangle(buttonsFrame.Left, buttonsFrame.Top, buttonWidth, buttonHeight);
                    visitor.SetFill(NColor.Red);
                    visitor.PaintRectangle(buttonRect);
                }

                // paint middle button, if down
                if (m_MiddleMouseDown)
                {
                    NRectangle buttonRect = new NRectangle(buttonsFrame.Left + buttonWidth, buttonsFrame.Top, buttonWidth, buttonHeight);
                    visitor.SetFill(NColor.Green);
                    visitor.PaintRectangle(buttonRect);
                }

                // paint right button, if down
                if (m_RightMouseDown)
                {
                    NRectangle buttonRect = new NRectangle(buttonsFrame.Right - buttonWidth, buttonsFrame.Top, buttonWidth, buttonHeight);
                    visitor.SetFill(NColor.Blue);
                    visitor.PaintRectangle(buttonRect);
                }

                // paint mouse pointer and buttons frame
                visitor.ClearStyles();
                visitor.SetStroke(new NStroke(NColor.Black));

                visitor.PaintEllipse(NRectangle.FromCenterAndSize(m_MouseLocation, pointerSize));
                visitor.PaintRectangle(buttonsFrame);

                double leftSeparator = buttonsCenter.X - buttonWidth / 2;
                visitor.PaintLine(leftSeparator, buttonsFrame.Top, leftSeparator, buttonsFrame.Bottom);

                double rightSeparator = buttonsCenter.X + buttonWidth / 2;
                visitor.PaintLine(rightSeparator, buttonsFrame.Top, rightSeparator, buttonsFrame.Bottom);
            }

            // paint capture frame
            if (NMouse.IsCaptured(m_Canvas))
            {
                NRectangle outerRect = m_Canvas.GetContentEdge();
                NRectangle innerRect = outerRect;
                innerRect.Deflate(3);

                NGraphicsPath path = new NGraphicsPath();
                path.AddRectangle(outerRect);
                path.AddRectangle(innerRect);

                visitor.ClearStyles();
                visitor.SetFill(NColor.Red);
                visitor.PaintPath(path);
            }
        }

        void OnCanvasMouseMove(NMouseEventArgs arg)
        {
            // log event
            if (m_LogMoveEventsCheck.Checked)
            {
                m_EventsLog.LogEvent(String.Format("Mouse Move. Position X: {0}, Y; {1}", m_MouseLocation.X, m_MouseLocation.Y));
            }

            // update mouse location
            m_MouseLocation = arg.CurrentTargetPosition;
            m_Canvas.InvalidateDisplay();
        }
        void OnCanvasMouseUp(NMouseButtonEventArgs arg)
        {
            m_EventsLog.LogEvent("Mouse Up. Button: " + arg.Button);

            switch (arg.Button)
            {
                case ENMouseButtons.Left:
                    m_LeftMouseDown = false;
                    break;

                case ENMouseButtons.Right:
                    m_RightMouseDown = false;
                    break;

                case ENMouseButtons.Middle:
                    m_MiddleMouseDown = false;
                    break;

                default:
                    break;
            }

            m_Canvas.InvalidateDisplay();
        }
        void OnCanvasMouseDown(NMouseButtonEventArgs arg)
        {
            m_EventsLog.LogEvent("Mouse Down. Button: " + arg.Button);

            switch (arg.Button)
            {
                case ENMouseButtons.Left:
                    m_LeftMouseDown = true;

                    if (m_CaptureMouseOnLeftMouseDown.Checked)
                    {
                        m_Canvas.CaptureMouse();
                    }
                    break;

                case ENMouseButtons.Right:
                    m_RightMouseDown = true;

                    if (m_CaptureMouseOnRightMouseDown.Checked)
                    {
                        m_Canvas.CaptureMouse();
                    }
                    break;

                case ENMouseButtons.Middle:
                    m_MiddleMouseDown = true;
                    break;

                default:
                    break;
            }

            m_Canvas.InvalidateDisplay();
        }
        void OnCanvasMouseWheel(NMouseWheelEventArgs arg)
        {
            m_EventsLog.LogEvent("Mouse Wheel. Delta: " + arg.Delta);
        }

        void OnCanvasMouseOut(NMouseOverChangeEventArgs arg)
        {
            m_EventsLog.LogEvent("Mouse Out");
            m_Canvas.InvalidateDisplay();
        }
        void OnCanvasMouseIn(NMouseOverChangeEventArgs arg)
        {
            m_EventsLog.LogEvent("Mouse In");
            m_Canvas.InvalidateDisplay();
        }

        void OnCanvasGotMouseCapture(NMouseCaptureChangeEventArgs arg)
        {
            m_EventsLog.LogEvent("Got Mouse Capture");
            m_Canvas.InvalidateDisplay();
        }
        void OnCanvasLostMouseCapture(NMouseCaptureChangeEventArgs arg)
        {
            m_EventsLog.LogEvent("Lost Mouse Capture");
            m_Canvas.InvalidateDisplay();
        }

        #endregion

        #region Fields

        NCheckBox m_LogMoveEventsCheck;
        NCheckBox m_CaptureMouseOnLeftMouseDown;
        NCheckBox m_CaptureMouseOnRightMouseDown;
        NCanvas m_Canvas;
        NExampleEventsLog m_EventsLog;

        NPoint m_MouseLocation;
        bool m_LeftMouseDown;
        bool m_MiddleMouseDown;
        bool m_RightMouseDown;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NMouseEventsExample.
        /// </summary>
        public static readonly NSchema NMouseEventsExampleSchema;

        #endregion

        #region Constants

        private const string EnglishLanguageName = "English (US)";
        private const string BulgarianLanguageName = "Bulgarian";
        private const string GermanLanguageName = "German";

        #endregion
    }
}