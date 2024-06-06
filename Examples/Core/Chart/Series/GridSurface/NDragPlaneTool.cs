using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;
using System;

namespace Nevron.Nov.Examples.Chart
{
    public enum DragPlaneSurface
    {
        XY,
        XZ,
        ZY
    }

    [Serializable]
    public class NDragPlaneTool : NDragTool
    {
        #region Constructors

        /// <summary>
        /// Initializer constructor
        /// </summary>
        /// <param name="dragPlane"></param>
        public NDragPlaneTool(NDragPlane dragPlane)
        {
            this.Enabled = true;

            m_DragPlane = dragPlane;
            m_OriginalPosition = new NVector3DD();
        }
        /// <summary>
        /// Static constructor
        /// </summary>
        static NDragPlaneTool()
        {
            NDragPlaneToolSchema = NSchema.Create(typeof(NDragPlaneTool), NDragTool.NDragToolSchema);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The freedom plane
        /// </summary>
        public DragPlaneSurface DragPlaneSurface
        {
            get
            {
                return m_DragPlaneSurface;
            }
            set
            {
                m_DragPlaneSurface = value;
            }
        }
        /// <summary>
        /// Whether to use x locking
        /// </summary>
        public static bool LockX
        {
            get
            {
                return (m_LockXKey & NKeyboard.PressedModifiers) != 0;
            }
        }
        /// <summary>
        /// Whether to use Z locking
        /// </summary>
        public static bool LockZ
        {
            get
            {
                return (m_LockZKey & NKeyboard.PressedModifiers) != 0;
            }
        }

        #endregion

        #region Overrides

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void OnMouseMove(NMouseEventArgs args)
        {
            base.OnMouseMove(args);

            if (base.IsActive)
            {
                NCartesianChart chart = this.GetFirstAncestor<NCartesianChart>();
                NPoint3D viewToScale;

                switch (m_DragPlaneSurface)
                {
                    case DragPlaneSurface.XY:
                        if (!chart.TransformViewToLogical3D(args.CurrentTargetPosition, ENCartesianAxis.PrimaryX, ENCartesianAxis.PrimaryY, ENCartesianAxis.Depth, m_OriginalPosition.Z, out viewToScale))
                            return;
                        break;
                    case DragPlaneSurface.XZ:
                        if (!chart.TransformViewToLogical3D(args.CurrentTargetPosition, ENCartesianAxis.PrimaryX, ENCartesianAxis.Depth, ENCartesianAxis.PrimaryY, m_OriginalPosition.Y, out viewToScale))
                            return;
                        break;
                    case DragPlaneSurface.ZY:
                        if (!chart.TransformViewToLogical3D(args.CurrentTargetPosition, ENCartesianAxis.Depth, ENCartesianAxis.PrimaryY, ENCartesianAxis.PrimaryX, m_OriginalPosition.X, out viewToScale))
                            return;
                        break;
                    default:
                        NDebug.Assert(false); // new drag plane
                        return;
                }

                m_DragPlane.LockX = false;
                m_DragPlane.LockZ = false;

                if (LockX)
                {
                    m_DragPlane.LockX = true;
                }
                else if (LockZ)
                {
                    m_DragPlane.LockZ = true;
                }

                m_DragPlane.MovePoint(m_DragPlaneSurface, viewToScale, m_DataPointIndex);
            }
        }
        /// <summary>
        /// Return true if dragging can start
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected override bool CanActivate(NMouseButtonEventArgs args)
        {
            if (!base.CanActivate(args))
                return false;

            int dataPointIndex = GetDataPointIndexFromPoint(args.CurrentTargetPosition);

            if (dataPointIndex == -1)
                return false;

            m_DataPointIndex = dataPointIndex;
            m_OriginalPosition = m_DragPlane.GetVectorFromPoint(m_DataPointIndex);

            return true;
        }
        /// <summary>
        /// Fired when key down is pressed
        /// </summary>
        /// <param name="args"></param>
        public override void OnKeyDown(NKeyEventArgs args)
        {
            base.OnKeyDown(args);

            if (LockX || LockZ)
            {
                m_DragPlane.LockX = LockX;
                m_DragPlane.LockZ = LockZ;

                NCartesianChart chart = GetFirstAncestor<NCartesianChart>();

                int dataPointIndex = GetDataPointIndexFromPoint(NMouse.ScreenPosition);

                if (dataPointIndex != -1)
                {
                    if (LockX)
                    {
                        m_DragPlane.OrientPlaneX(dataPointIndex);
                    }
                    else if (LockZ)
                    {
                        m_DragPlane.OrientPlaneZ(dataPointIndex);
                    }
                }
            }
        }
        /// <summary>
        /// Overriden to rever the state to the original one if the user presses Esc key
        /// </summary>
        protected override void OnAborted()
        {
            base.OnAborted();

            m_DragPlane.RestorePoint(m_DataPointIndex, m_OriginalPosition);
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Gets the data point from the specified point
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        protected int GetDataPointIndexFromPoint(NPoint point)
        {
            NPointSeries pointSeries = m_DragPlane.PointSeries;
            NCartesianChart chart = pointSeries.GetFirstAncestor<NCartesianChart>();

            point = chart.TransformViewToModel2D(point);

            float xHotSpotArea = 10;
            float yHotSpotArea = 10;

            int dataPointIndex = -1;

            for (int i = 0; i < pointSeries.DataPoints.Count; i++)
            {
                double x = (double)pointSeries.DataPoints[i].X;
                double y = (double)pointSeries.DataPoints[i].Y;
                double z = (double)pointSeries.DataPoints[i].Z;

                NPoint viewPoint = chart.TransformLogicalToView3D(new NPoint3D(x, y, z), ENCartesianAxis.PrimaryX, ENCartesianAxis.PrimaryY, ENCartesianAxis.Depth);

                if (Math.Abs(viewPoint.X - point.X) < xHotSpotArea &&
                    Math.Abs(viewPoint.Y - point.Y) < yHotSpotArea)
                {
                    dataPointIndex = i;
                    break;
                }
            }

            return dataPointIndex;
        }


        #endregion

        #region Fields

        /// <summary>
        /// The original position of the point
        /// </summary>
        protected NVector3DD m_OriginalPosition;
        /// <summary>
        /// The data point index
        /// </summary>
        protected int m_DataPointIndex;
        /// <summary>
        /// The freedom plane
        /// </summary>
        protected DragPlaneSurface m_DragPlaneSurface;
        /// <summary>
        /// 
        /// </summary>
        protected NDragPlane m_DragPlane;

        #endregion

        #region Static Fields

        private static ENModifierKeys m_LockXKey = ENModifierKeys.Shift;
        private static ENModifierKeys m_LockZKey = ENModifierKeys.Alt;

        private static NSchema NDragPlaneToolSchema;

        #endregion
    }
}
