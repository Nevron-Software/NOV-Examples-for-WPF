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
    public class NKeyboardEventsExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NKeyboardEventsExample()
        {
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static NKeyboardEventsExample()
        {
            NKeyboardEventsExampleSchema = NSchema.Create(typeof(NKeyboardEventsExample), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            m_Canvas = new NCanvas();
            m_Canvas.PrePaint += OnCanvasPrePaint;

            // subscribe for keyboard events
            m_Canvas.KeyDown += OnCanvasKeyDown;
            m_Canvas.KeyUp += OnCanvasKeyUp;
            m_Canvas.InputChar += OnCanvasInputChar;
            m_Canvas.GotFocus += OnCanvasGotFocus;
            m_Canvas.LostFocus += OnCanvasLostFocus;

            // subscribe for mouse events
            m_Canvas.MouseDown += OnCanvasMouseDown;

            return m_Canvas;
        }

        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();
            stack.FitMode = ENStackFitMode.Last;
            stack.FillMode = ENStackFillMode.Last;

            // handle key up
            m_HandleKeyDown = new NCheckBox("Handle Key Down");
            stack.Add(m_HandleKeyDown);

            // handle key up
            m_HandleKeyUp = new NCheckBox("Handle Key Up");
            stack.Add(m_HandleKeyUp);

            // handle key up
            m_HandleInputChar = new NCheckBox("Handle Input Char");
            stack.Add(m_HandleInputChar);

            // Create the events log
            m_EventsLog = new NExampleEventsLog();
            stack.Add(m_EventsLog);

            return stack;
        }
        protected override string GetExampleDescription()
        {
            return @"
<p>
	Demonstrates the keyboard support in NOV. Click on the canvas and start pressing the keyboard keys to see the keyboard events that NOV sends to the application.
    Keyboard support is available to most desktop enviroments.
</p>
";
        }

        #endregion

        #region Canvas Event Handlers

        void OnCanvasPrePaint(NCanvasPaintEventArgs arg)
        {
            NPaintVisitor visitor = arg.PaintVisitor;

            NRectangle outerRect = m_Canvas.GetContentEdge();
            
            NRectangle borderRect = outerRect;
            borderRect.Deflate(3);

            NRectangle textRect = borderRect;
            textRect.Deflate(3);

            // paint background
            visitor.SetFill(NColor.Ivory);
            visitor.PaintRectangle(outerRect);

            // paint string
            NFont font = (NFont)NSystem.SafeDeepClone(Font);
            if (font != null)
            {
                font.Size = 16;
                visitor.SetFont(font);
                NPaintTextRectSettings settings = new NPaintTextRectSettings();
                visitor.SetFill(NColor.Black);
                visitor.PaintString(textRect, m_sInputString, ref settings);
            }

            // paint focus frame
            if (NKeyboard.IsFocused(m_Canvas))
            {
                NGraphicsPath path = new NGraphicsPath();
                path.AddRectangle(outerRect);
                path.AddRectangle(borderRect);

                visitor.ClearStyles();
                visitor.SetFill(NColor.Red);
                visitor.PaintPath(path);
            }
        }

        void OnCanvasKeyUp(NKeyEventArgs arg)
        {
            m_EventsLog.LogEvent("Key Up: " + arg.Key.ToString());
            if (m_HandleKeyUp.Checked)
            {
                arg.Cancel = true;
            }
        }
        void OnCanvasKeyDown(NKeyEventArgs arg)
        {
            m_EventsLog.LogEvent("Key Down: " + arg.Key.ToString());
            if (m_HandleKeyDown.Checked)
            {
                arg.Cancel = true;
            }
        }
        void OnCanvasInputChar(NInputCharEventArgs arg)
        {
            m_EventsLog.LogEvent("Input Char: " + arg.Char);

            m_sInputString += arg.Char;
            m_Canvas.InvalidateDisplay();
        }

        void OnCanvasGotFocus(NFocusChangeEventArgs arg)
        {
            m_EventsLog.LogEvent("Got Focus");
            m_Canvas.InvalidateDisplay();
        }
        void OnCanvasLostFocus(NFocusChangeEventArgs arg)
        {
            m_EventsLog.LogEvent("Lost Focus");
            m_Canvas.InvalidateDisplay();
        }

        void OnCanvasMouseDown(NMouseButtonEventArgs arg)
        {
            m_Canvas.Focus();
        }

        #endregion

        #region Fields

        NCheckBox m_HandleKeyDown;
        NCheckBox m_HandleKeyUp;
        NCheckBox m_HandleInputChar;
        NCanvas m_Canvas;
        NExampleEventsLog m_EventsLog;
        string m_sInputString = "";

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NKeyboardEventsExample.
        /// </summary>
        public static readonly NSchema NKeyboardEventsExampleSchema;

        #endregion

        #region Constants

        private const string EnglishLanguageName = "English (US)";
        private const string BulgarianLanguageName = "Bulgarian";
        private const string GermanLanguageName = "German";

        #endregion
    }
}