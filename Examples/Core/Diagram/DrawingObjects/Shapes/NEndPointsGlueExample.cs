using System;

using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NEndPointsGlueExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NEndPointsGlueExample()
        {
            m_Timer.Tick += OnTimerTick;
        }
        /// <summary>
        /// Static constructor.
        /// </summary>
        static NEndPointsGlueExample()
        {
            NEndPointsGlueExampleSchema = NSchema.Create(typeof(NEndPointsGlueExample), NExampleBaseSchema);
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

			m_DrawingView.Registered += OnDrawingViewRegistered;
            m_DrawingView.Unregistered += OnDrawingViewUnregistered;

            return drawingViewWithRibbon;
        }
		protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.Last;

            // selection mode
            {
                m_RadioGroup = new NRadioButtonGroup();

                NStackPanel radioStack = new NStackPanel();
                m_RadioGroup.Content = radioStack;

                radioStack.Add(new NRadioButton("Glue To Nearest Port"));
                radioStack.Add(new NRadioButton("Glue To Shape Box Intersection"));
                radioStack.Add(new NRadioButton("Glue To Geometry Intersection"));
                radioStack.Add(new NRadioButton("Glue To Shape Box"));;
                radioStack.Add(new NRadioButton("Glue To Geometry Contour"));
                radioStack.Add(new NRadioButton("Glue To Port"));

                stack.Add(new NGroupBox("Select Glue Mode", m_RadioGroup));
            }

            // glue properties
            {
                NStackPanel holdersStack = new NStackPanel();
                stack.Add(holdersStack);

                m_BeginGlueHolder = new NGroupBox("Begin Glue Properties");
                holdersStack.Add(m_BeginGlueHolder);

                m_EndGlueHolder = new NGroupBox("End Glue Properties");
                holdersStack.Add(m_EndGlueHolder);
            }

            // timer
            {
                NStackPanel timerStack = new NStackPanel();

                NButton startRotationButton = new NButton("Start Rotation");
                startRotationButton.Click += delegate(NEventArgs args) { m_Timer.Start(); };
                timerStack.Add(startRotationButton);

                NButton stopRotationButton = new NButton("Stop Rotation");
                stopRotationButton.Click += delegate(NEventArgs args) { m_Timer.Stop(); };
                timerStack.Add(stopRotationButton);

                stack.Add(timerStack);
            }

            // select the first glue mode
            m_RadioGroup.SelectedIndexChanged += OnRadioGroupSelectedIndexChanged;
            m_RadioGroup.SelectedIndex = 0;

            return stack;
        }
        protected override string GetExampleDescription()
        {
            return @"
<p>
    Demonstrates the End-Points glue and the API you can use to glue the begin and end points.
</p>
<p>
    The begin or end point of a 1D shape can be glued in the following ways:
    <ul>
        <li>
            <b>Glue To Nearest Port</b> The begin or end point is glued to the nearest port in respect to the other end-point.
        </li>
        <li>
            <b>Glue To Shape Box Intersection</b> The begin or end point is glued to the intersection of the shape box and the line formed by the shape center and the other end-point.
        </li>
        <li>
            <b>Glue To Geometry Intersection</b> The begin or end point is glued to the intersection of the shape geometry and the line formed by the shape center and the other end-point.
        </li>
        <li>
            <b>Glue To Shape Box</b> The begin or end point is glued to a point in the shape Width-Height box. The point is defined in relative coordinates.
        </li>
        <li>
            <b>Glue To Geometry Contour</b> The begin or end point is glued to a point along the shape geometry contour (outline). The point is defined a factor - 0 is the contour begin, 1 is the contour end.
        </li>
        <li>
            <b>Glue To Port</b> The begin or end point is glued to a port of the target shape. The port is defined by its index.
        </li>
    </ul>
</p>
            ";
        }

        private void InitDiagram(NDrawingDocument drawingDocument)
        {
            NDrawing drawing = drawingDocument.Content;

            // Hide the grid
            drawing.ScreenVisibility.ShowGrid = false;

            // Create two shapes and a line connector between them
            NBasicShapeFactory basicShapes = new NBasicShapeFactory();
            NConnectorShapeFactory connectorShapes = new NConnectorShapeFactory();

            m_BeginShape = basicShapes.CreateShape(ENBasicShape.Ellipse);
            m_BeginShape.Size = new NSize(150, 100);
            drawing.ActivePage.Items.Add(m_BeginShape);

            m_EndShape = basicShapes.CreateShape(ENBasicShape.Pentagram);
            m_EndShape.Size = new NSize(100, 100);
            drawing.ActivePage.Items.Add(m_EndShape);

            m_Connector = connectorShapes.CreateShape(ENConnectorShape.Line);
            m_Connector.GlueBeginToNearestPort(m_BeginShape);
            m_Connector.GlueEndToNearestPort(m_EndShape);
            drawing.ActivePage.Items.Add(m_Connector);

            // Perform inital layout of shapes
            OnTimerTick();
        }
        
        #endregion

        #region Event Handlers

        private void OnDrawingViewRegistered(NEventArgs arg)
        {
            m_Timer.Start();
        }
        private void OnDrawingViewUnregistered(NEventArgs arg)
        {
            m_Timer.Stop();
        }
        private void OnTimerTick()
        {
            NPoint centerOfRotation = new NPoint(m_DrawingView.ActivePage.Bounds.CenterX, 300);
            const double radius = 150;

            NPoint beginCenter = centerOfRotation + new NPoint(Math.Cos(m_dAngle) * radius, Math.Sin(m_dAngle) * radius);
            m_BeginShape.SetBounds(NRectangle.FromCenterAndSize(beginCenter, m_BeginShape.Width, m_BeginShape.Height));

            NPoint endCenter = centerOfRotation + new NPoint(Math.Cos(m_dAngle + NMath.PI) * radius, Math.Sin(m_dAngle + NMath.PI) * radius);
            m_EndShape.SetBounds(NRectangle.FromCenterAndSize(endCenter, m_EndShape.Width, m_EndShape.Height));

            m_dAngle += NMath.PI / 180;
        }
        private void OnRadioGroupSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            switch (m_RadioGroup.SelectedIndex)
            {
                case 0: // glue to nearest port
                    m_Connector.GlueBeginToNearestPort(m_BeginShape);
                    m_Connector.GlueEndToNearestPort(m_EndShape);
                    break;

                case 1: // glue to box intersection
                    m_Connector.GlueBeginToShapeBoxIntersection(m_BeginShape);
                    m_Connector.GlueEndToShapeBoxIntersection(m_EndShape);
                    break;

                case 2: // glue to box intersection
                    m_Connector.GlueBeginToGeometryIntersection(m_BeginShape);
                    m_Connector.GlueEndToGeometryIntersection(m_EndShape);
                    break;

                case 3: // glue to box location
                    m_Connector.GlueBeginToShapeBox(m_BeginShape, 0.3d, 0.3d);
                    m_Connector.GlueEndToShapeBox(m_EndShape, 0.3d, 0.3d);
                    break;

                case 4: // glue to geometry contour
                    m_Connector.GlueBeginToGeometryContour(m_BeginShape, 0.5d);
                    m_Connector.GlueEndToGeometryContour(m_EndShape, 0.5d);
                    break;

                case 5: // glue to port
                    m_Connector.GlueBeginToPort(m_BeginShape.Ports[0]);
                    m_Connector.GlueEndToPort(m_EndShape.Ports[0]);
                    break;
            }

            // update the begin point glue properties
            if (m_Connector.BeginPointGlue == null)
            {
                m_BeginGlueHolder.Content = null;
                m_BeginGlueHolder.Visibility = ENVisibility.Collapsed;
            }
            else
            {
                m_BeginGlueHolder.Visibility = ENVisibility.Visible;
                m_BeginGlueHolder.Content = NDesigner.GetDesigner(m_Connector.BeginPointGlue).CreateInstanceEditor(m_Connector.BeginPointGlue);
            }

            // update the end point glue properties
            if (m_Connector.EndPointGlue == null)
            {
                m_EndGlueHolder.Content = null;
                m_EndGlueHolder.Visibility = ENVisibility.Collapsed;
            }
            else
            {
                m_EndGlueHolder.Visibility = ENVisibility.Visible;
                m_EndGlueHolder.Content = NDesigner.GetDesigner(m_Connector.EndPointGlue).CreateInstanceEditor(m_Connector.EndPointGlue);
            }
            
        }

        #endregion

        #region Fields

        private NDrawingView m_DrawingView;

        private NShape m_BeginShape;
        private NShape m_EndShape;
        private NShape m_Connector;
        private NTimer m_Timer = new NTimer(50);
        private NRadioButtonGroup m_RadioGroup;
        private NGroupBox m_BeginGlueHolder;
        private NGroupBox m_EndGlueHolder;
        private double m_dAngle = 0;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NEndPointsGlueExample.
        /// </summary>
        public static readonly NSchema NEndPointsGlueExampleSchema;

        #endregion
    }
}