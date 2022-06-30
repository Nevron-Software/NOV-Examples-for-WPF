using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Batches;
using Nevron.Nov.Diagram.Expressions;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;

namespace Nevron.Nov.Examples.Diagram
{
    /// <summary>
    /// 
    /// </summary>
    public class NCustomShapesExample : NDiagramExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NCustomShapesExample()
        {
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static NCustomShapesExample()
        {
            NCustomShapesExampleSchema = NSchema.Create(typeof(NCustomShapesExample), NDiagramExampleBase.NDiagramExampleBaseSchema);
        }

        #endregion

        #region Overrides from NDiagramExampleBase

        protected override string GetExampleDescription()
        {
            return @"
<p>
    Demonstrates how to create custom shapes by combining shapes and also how to create custom formula shapes with control points.
</p>
<p>
    The Coffee Cup shape is created by grouping shapes with different fill styles inside a group. 
    By using this approach you can create shapes that mix different fill and stroke styles.
</p>
<p>
    The Trapezoid Shape is a replica of the Visio Trapezoid Smart Shape. 
    It demonstrates that with NOV Diagram you can replicate the Shape Sheet behavior of almost any Visio shape.
    Select the shape and move its control point to modify the trapezoid strip width. 
    Note that the geometry, left and right ports and XControl point behavior of this shape are driven by formula expressions.
</p>
            ";
        }
        protected override void InitDiagram()
        {
            base.InitDiagram();

            m_DrawingView.ActivePage.Interaction.GluingShapes += Interaction_GluingShapes;

            // create the coffee cup
            NShape coffeeCup = CreateCoffeeCupShape();
            coffeeCup.SetBounds(new NRectangle(50, 50, 100, 200));
            m_DrawingDocument.Content.ActivePage.Items.Add(coffeeCup);

            NShape trapedzoid = CreateTrapedzoidShape();
            trapedzoid.SetBounds(new NRectangle(200, 150, 100, 100));
            m_DrawingDocument.Content.ActivePage.Items.Add(trapedzoid);
        }

        void Interaction_GluingShapes(NGlueShapesEventArgs args)
        {
            // safely get the ports collection
            NPortCollection ports = (NPortCollection)args.Shape2D.GetChild(NShape.PortsChild, false);
            if (ports == null && ports.Count == 0)
                return;

            // get the anchor point in page coordinates
            NPoint anchorInPage = args.ConnectBegin ? args.Shape1D.GetEndPointInPage() : args.Shape1D.GetBeginPointInPage();

            // get the nearest port
            NPort neartestPort = ports[0];
            double neartestDistance = NGeometry2D.PointsDistance(anchorInPage, neartestPort.GetLocationInPage());
            for (int i = 1; i < ports.Count; i++)
            {
                NPort curPort = ports[i];
                double curDistance = NGeometry2D.PointsDistance(anchorInPage, curPort.GetLocationInPage());
                if (curDistance < neartestDistance)
                {
                    neartestDistance = curDistance;
                    neartestPort = curPort;
                }
            }

            // connect begin or end 
            if (args.ConnectBegin)
            {
                args.Shape1D.GlueBeginToPort(neartestPort);
            }
            else
            {
                args.Shape1D.GlueEndToPort(neartestPort);
            }

            // cancel the event so that the diagram does not perform default connection
            args.Cancel = true;
        }
        /// <summary>
        /// Creates a custom shape that is essentially a group consisting of three other shapes each with different filling.
        /// You need to use groups to have shapes that mix different fill, or stroke styles.
        /// </summary>
        /// <returns></returns>
        protected NShape CreateCoffeeCupShape()
        {
            // create the points and paths from which the shape consits
            NPoint[] cupPoints = new NPoint[] { 
                new NPoint(45, 268), 
                new NPoint(63, 331), 
                new NPoint(121, 331), 
                new NPoint(140, 268) };

            NGraphicsPath handleGraphicsPath = new NGraphicsPath();
            handleGraphicsPath.AddClosedCurve(new NPoint[] { 
                new NPoint(175, 295), 
                new NPoint(171, 278), 
                new NPoint(140, 283), 
                new NPoint(170, 290), 
                new NPoint(128, 323) }, 1);
            
            NGraphicsPath steamGraphicsPath = new NGraphicsPath();
            steamGraphicsPath.AddCubicBeziers(new NPoint[] { 
                new NPoint(92, 270), 
                new NPoint(53, 163), 
                new NPoint(145, 160), 
                new NPoint(86, 50), 
                new NPoint(138, 194), 
                new NPoint(45, 145), 
                new NPoint(92, 270) });
            steamGraphicsPath.CloseFigure();

            // calculate some bounds
            NRectangle handleBounds = handleGraphicsPath.ExactBounds;
            NRectangle cupBounds = NGeometry2D.GetBounds(cupPoints);
            NRectangle steamBounds = steamGraphicsPath.ExactBounds;
            NRectangle geometryBounds = NRectangle.Union(cupBounds, handleBounds, steamBounds);

            // normalize the points and paths by transforming them to relative coordinates
            NRectangle normalRect = new NRectangle(0, 0, 1, 1);
            NMatrix transform = NMatrix.CreateBoundsStretchMatrix(geometryBounds, normalRect);
            transform.TransformPoints(cupPoints);
            handleGraphicsPath.Transform(transform);
            steamGraphicsPath.Transform(transform);

            // create the cup shape
            NDrawPolygon cupPolygon = new NDrawPolygon(normalRect, cupPoints);
            cupPolygon.Relative = true;
            NShape cupShape = new NShape();
            cupShape.Init2DShape();
            cupShape.Geometry.Fill = new NColorFill(NColor.Brown);
            cupShape.Geometry.Add(cupPolygon);
            cupShape.SetBounds(geometryBounds);
            
            // create the cup handle
            NDrawPath handlePath = new NDrawPath(normalRect, handleGraphicsPath);
            handlePath.Relative = true;
            NShape handleShape = new NShape();
            handleShape.Init2DShape();
            handleShape.Geometry.Fill = new NColorFill(NColor.LightSalmon);
            handleShape.Geometry.Add(handlePath);
            handleShape.SetBounds(geometryBounds);
            
            // create the steam
            NDrawPath steamPath = new NDrawPath(steamGraphicsPath.Bounds, steamGraphicsPath);
            steamPath.Relative = true;
            NShape steamShape = new NShape();
            steamShape.Init2DShape();
            steamShape.Geometry.Fill = new NColorFill(new NColor(50, 122, 122, 122));
            steamShape.Geometry.Add(steamPath);
            steamShape.SetBounds(geometryBounds);
            
            // group the shapes as a single group
            NGroup group;
            NBatchGroup batch = new NBatchGroup(m_DrawingDocument);
            batch.Build(cupShape, handleShape, steamShape);
            batch.Group(null, out group);

            // alter some properties of the group
            group.SelectionMode = ENGroupSelectionMode.GroupOnly;
            group.SnapToShapes = false;

            return group;
        }
        /// <summary>
        /// Creates a custom shape that is a replica of the Visio Trapedzoid shape. With NOV diagram you can replicate the smart behavior of any Visio smart shape.
        /// </summary>
        /// <returns></returns>
        protected NShape CreateTrapedzoidShape()
        {
            NShape shape = new NShape();
            shape.Init2DShape();

            // add controls
            NControl control = new NControl();
            control.SetFx(NControl.XProperty, new NShapeWidthFactorFx(0.3));
            control.Y = 0.0d;
            control.SetFx(NControl.XBehaviorProperty, string.Format("IF(X<Width/2,{0},{1})", ((int)ENCoordinateBehavior.OffsetFromMin), ((int)ENCoordinateBehavior.OffsetFromMax)));
            control.YBehavior = ENCoordinateBehavior.Locked;
            control.Tooltip = "Modify strip width";
            shape.Controls.Add(control);

            // add a geometry
            NGeometry geometry1 = shape.Geometry;
            {
                NMoveTo plotFigure = 
                geometry1.MoveTo("MIN(Controls.0.X,Width-Controls.0.X)", 0.0d);
                geometry1.LineTo("Width-Geometry.0.X", 0.0d);
                geometry1.LineTo("Width", "Height");
                geometry1.LineTo(0.0d, "Height");
                geometry1.LineTo("Geometry.0.X", "Geometry.0.Y");
                plotFigure.CloseFigure = true;
            }

            // add ports
            for (int i = 0; i < 4; i++)
            {
                NPort port = new NPort();
                shape.Ports.Add(port);

                switch (i)
                {
                    case 0: // top
                        port.Relative = true;
                        port.X = 0.5;
                        port.Y = 0.0d;
                        port.SetDirection(ENBoxDirection.Up);
                        break;

                    case 1: // right
                        port.SetFx(NPort.XProperty, "(Geometry.1.X + Geometry.2.X) / 2");
                        port.SetFx(NPort.YProperty, new NShapeHeightFactorFx(0.5));
                        port.SetDirection(ENBoxDirection.Right);
                        break;

                    case 2: // bottom
                        port.Relative = true;
                        port.X = 0.5;
                        port.Y = 1.0d;
                        port.SetDirection(ENBoxDirection.Down);
                        break;

                    case 3: // left
                        port.SetFx(NPort.XProperty, "(Geometry.0.X + Geometry.3.X) / 2");
                        port.SetFx(NPort.YProperty, new NShapeHeightFactorFx(0.5));
                        port.SetDirection(ENBoxDirection.Left);
                        break;
                }
            }

            return shape;
        }

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NCustomShapesExample.
        /// </summary>
        public static readonly NSchema NCustomShapesExampleSchema;

        #endregion
    }
}