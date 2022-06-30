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
    public class NTouchSupportExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NTouchSupportExample()
        {
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static NTouchSupportExample()
        {
            NTouchSupportExampleSchema = NSchema.Create(typeof(NTouchSupportExample), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Protected Overrides - Example

        protected override NWidget CreateExampleContent()
        {
            m_Canvas = new NCanvas();
            m_Canvas.PrePaint += OnCanvasPrePaint;

            // subscribe for touch events
            m_Canvas.TouchDown += OnCanvasTouchDown;
            m_Canvas.TouchMove += OnCanvasTouchMove;
            m_Canvas.TouchUp += OnCanvasTouchUp;

            // subscribe for mouse events
            m_Canvas.MouseDown += OnCanvasMouseDown;
            m_Canvas.MouseUp += OnCanvasMouseUp;
            m_Canvas.MouseMove += OnCanvasMouseMove;

            return m_Canvas;
        }
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();
            stack.FitMode = ENStackFitMode.Last;
            stack.FillMode = ENStackFillMode.Last;

            // capture touch check
            m_CaptureTouchCheck = new NCheckBox("Capture Touch");
            stack.Add(m_CaptureTouchCheck);

            // handle touch events check
            m_HandleTouchEventsCheck = new NCheckBox("Handle Touch Events");
            stack.Add(m_HandleTouchEventsCheck);

            // track move events
            m_LogMoveEventsCheck = new NCheckBox("Track Move Events");
            stack.Add(m_LogMoveEventsCheck);

            // Create clear canvas button
            NButton clearCanvasButton = new NButton("Clear Canvas");
            clearCanvasButton.Click += clearCanvas_Click;
            stack.Add(clearCanvasButton);

            // Create the events log
            m_EventsLog = new NExampleEventsLog();
            stack.Add(m_EventsLog);

            return stack;
        }

        void clearCanvas_Click(NEventArgs arg)
        {
            m_TouchPoints.Clear();
            m_Canvas.InvalidateDisplay();
        }
        protected override string GetExampleDescription()
        {
            return @"
<p>
	Demonstrates the core touch support in NOV. Use your fingers to draw on the canvas and explore the touch events that NOV sends to the application.
    Note that touch input is only available to touch enabled enviroments.
</p>
";
        }

        #endregion

        #region Canvas Event Handlers

        void OnCanvasPrePaint(NCanvasPaintEventArgs arg)
        {
            for (int i = 0; i < m_TouchPoints.Count; i++)
            {
                m_TouchPoints[i].Paint(arg.PaintVisitor);
            }
        }
        void OnCanvasTouchUp(NTouchActionEventArgs arg)
        {
            AddTouchPoint(arg);
            m_EventsLog.LogEvent("Touch Up");

            if (m_HandleTouchEventsCheck.Checked)
            {
                arg.Cancel = true;
            }
        }
        void OnCanvasTouchMove(NTouchActionEventArgs arg)
        {
            AddTouchPoint(arg);

            if (m_LogMoveEventsCheck.Checked)
            {
                m_EventsLog.LogEvent("Touch Move");
            }

            if (m_HandleTouchEventsCheck.Checked)
            {
                arg.Cancel = true;
            }
        }

        void OnCanvasTouchDown(NTouchActionEventArgs arg)
        {
            AddTouchPoint(arg);
            m_EventsLog.LogEvent("Touch Down");

            if (m_CaptureTouchCheck.Checked)
            {
                m_Canvas.CaptureTouch(arg.Device);
                m_EventsLog.LogEvent("Captured");
            }

            if (m_HandleTouchEventsCheck.Checked)
            {
                arg.Cancel = true;
            }
        }

        void OnCanvasMouseMove(NMouseEventArgs arg)
        {
            if (m_LogMoveEventsCheck.Checked)
            {
                m_EventsLog.LogEvent("Mouse Move");
            }
        }
        void OnCanvasMouseUp(NMouseButtonEventArgs arg)
        {
            m_EventsLog.LogEvent("Mouse Up");
        }
        void OnCanvasMouseDown(NMouseButtonEventArgs arg)
        {
            m_EventsLog.LogEvent("Mouse Down");
        }

        void AddTouchPoint(NTouchActionEventArgs arg)
        {
            m_TouchPoints.Add(new NTouchPoint(arg.TargetPosition, arg.ScreenSize, arg.DeviceState));
            m_Canvas.InvalidateDisplay();
        }

        #endregion

        #region Fields

        NCheckBox m_CaptureTouchCheck;
        NCheckBox m_HandleTouchEventsCheck;
        NCheckBox m_LogMoveEventsCheck;
        NCanvas m_Canvas;
        NExampleEventsLog m_EventsLog;
        NList<NTouchPoint> m_TouchPoints = new NList<NTouchPoint>();

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NTouchSupportExample.
        /// </summary>
        public static readonly NSchema NTouchSupportExampleSchema;

        #endregion

        #region Constants

        private const string EnglishLanguageName = "English (US)";
        private const string BulgarianLanguageName = "Bulgarian";
        private const string GermanLanguageName = "German";

        #endregion

        #region Nested Types - TouchPoint

        public class NTouchPoint
        {
            public NTouchPoint(NPoint point, NSize size, ENTouchDeviceState state)
            {
                State = state;
                Location = point;
                Size = size;
            }

            public void Paint(NPaintVisitor visitor)
            {
                NColor color;
                switch (State)
                {
                    case ENTouchDeviceState.Down:
                        color = NColor.Blue;
                        break;

                    case ENTouchDeviceState.Unknown:
                        color = NColor.Green;
                        break;

                    case ENTouchDeviceState.Up:
                        color = NColor.Red;
                        break;

                    default:
                        throw new Exception("New ENTouchDeviceState?");
                }

                NSize size = Size;
                if (size.Width == 0 || size.Height == 0)
                {
                    size = new NSize(5, 5);
                }

                visitor.SetStroke(new NStroke(color));
                visitor.PaintEllipse(NRectangle.FromCenterAndSize(Location, size));
            }

            NPoint Location;
            NSize Size;
            ENTouchDeviceState State;
        }

        #endregion
    }
}