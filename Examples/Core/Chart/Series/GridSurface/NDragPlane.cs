using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Graphics;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Simple class for maintaining a draggable plane
	/// </summary>
	public class NDragPlane
    {
        #region Constructors

        /// <summary>
        /// Initializer constructor
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="vecA"></param>
        /// <param name="vecB"></param>
        /// <param name="vecC"></param>
        /// <param name="vecD"></param>
        public NDragPlane(NCartesianChart chart, NVector3DD vecA, NVector3DD vecB, NVector3DD vecC, NVector3DD vecD)
        {
            NPointSeries pointSeries = new NPointSeries();

            pointSeries.Tag = (int)1;
            pointSeries.Shape = ENPointShape3D.Sphere;
            pointSeries.UseXValues = true;
            pointSeries.UseZValues = true;
            pointSeries.DataLabelStyle = new NDataLabelStyle(false);
            pointSeries.InflateMargins = false;
            pointSeries.Size = 8;

            pointSeries.DataPoints.Add(new NPointDataPoint(vecA.X, vecA.Y, vecA.Z, new NColorFill(NColor.Red), null, ENPointShape3D.Sphere));
            pointSeries.DataPoints.Add(new NPointDataPoint(vecB.X, vecB.Y, vecB.Z, new NColorFill(NColor.Blue), null, ENPointShape3D.Sphere));
            pointSeries.DataPoints.Add(new NPointDataPoint(vecC.X, vecC.Y, vecC.Z, new NColorFill(NColor.Blue), null, ENPointShape3D.Sphere));
            pointSeries.DataPoints.Add(new NPointDataPoint(vecD.X, vecD.Y, vecD.Z, new NColorFill(NColor.Red), null, ENPointShape3D.Sphere));

            m_PointSeries = pointSeries;

            NMeshSurfaceSeries meshSeries = new NMeshSurfaceSeries();
            meshSeries.Data.SetGridSize(2, 2);

            m_MeshSurface = meshSeries;
            m_MeshSurface.FillMode = ENSurfaceFillMode.Uniform;
            m_MeshSurface.FrameMode = ENSurfaceFrameMode.None;
            m_MeshSurface.Fill = new NColorFill(NColor.FromARGB(125, 0, 0, 255));

            UpdateMeshSurface();

            chart.Series.Add(m_MeshSurface);
            chart.Series.Add(m_PointSeries);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the point series
        /// </summary>
        public NPointSeries PointSeries
        {
            get
            {
                return m_PointSeries;
            }
        }
        /// <summary>
        /// Gets the A point
        /// </summary>
        public NVector3DD PointA
        {
            get
            {
                return GetVectorFromPoint(0);
            }
        }
        /// <summary>
        /// Gets the B point
        /// </summary>
        public NVector3DD PointB
        {
            get
            {
                return GetVectorFromPoint(1);
            }
        }
        /// <summary>
        /// Gets the C point
        /// </summary>
        public NVector3DD PointC
        {
            get
            {
                return GetVectorFromPoint(2);
            }
        }
        /// <summary>
        /// Gets the D point
        /// </summary>
        public NVector3DD PointD
        {
            get
            {
                return GetVectorFromPoint(3);
            }
        }
        /// <summary>
        /// Gets or sets whether to lock the x coordinate
        /// </summary>
        public bool LockX
        {
            get
            {
                return m_LockX;
            }
            set
            {
                m_LockX = value;
            }
        }
        /// <summary>
        /// Gets or sets whether to lock the z coordinate
        /// </summary>
        public bool LockZ
        {
            get
            {
                return m_LockZ;
            }
            set
            {
                m_LockZ = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the horizontal plane length 
        /// </summary>
        /// <param name="axisRange"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public double GetPlaneLength(NRange axisRange, double origin, int originPoint, bool xOrZ)
        {
            NVector3DD vecA = GetVectorFromPoint(0);
            NVector3DD vecB = GetVectorFromPoint(1);

            NVector3DD lengthVector = new NVector3DD();
            lengthVector.Subtract(ref vecB, ref vecA);
            double orgPlaneLength = lengthVector.GetLength();
            double sign;

            if (originPoint == 0 || originPoint == 2)
            {
                // left point
                if (xOrZ)
                {
                    sign = vecA.X < vecB.X ? 1 : -1;
                }
                else
                {
                    sign = vecA.Z < vecB.Z ? 1 : -1;
                }
            }
            else
            {
                // right point
                if (xOrZ)
                {
                    sign = vecB.X < vecA.X ? 1 : -1;
                }
                else
                {
                    sign = vecB.Z < vecA.Z ? 1 : -1;
                }
            }

            axisRange.Normalize();

            if (sign > 0)
            {
                if (origin + orgPlaneLength > axisRange.End)
                {
                    orgPlaneLength = axisRange.End - origin;
                }
            }
            else
            {
                if (origin - orgPlaneLength < axisRange.Begin)
                {
                    orgPlaneLength = origin - axisRange.Begin;
                }
            }

            return orgPlaneLength * sign;
        }
        /// <summary>
        /// Drags the specified point
        /// </summary>
        /// <param name="dragPlane"></param>
        /// <param name="vector"></param>
        /// <param name="dataPointIndex"></param>
        public void MovePoint(DragPlaneSurface dragPlane, NPoint3D vector, int dataPointIndex)
        {
            // modify the point coordinates. Don't modify the y coords only x, z or xz
            // take into account the currently selected axes from NViewToScale3DTransformation
            switch (dragPlane)
            {
                case DragPlaneSurface.XY:
                    {
                        SetXPointCoordinate(dataPointIndex, ClampXCoordinateToRuler(vector.X));
                    }
                    break;
                case DragPlaneSurface.XZ:
                    SetXPointCoordinate(dataPointIndex, ClampXCoordinateToRuler(vector.X));
                    SetZPointCoordinate(dataPointIndex, ClampZCoordinateToRuler(vector.Y));
                    break;
                case DragPlaneSurface.ZY:
                    SetZPointCoordinate(dataPointIndex, ClampXCoordinateToRuler(vector.X));
                    break;
            }

            SynchronizePoints(dataPointIndex);
            UpdateMeshSurface();

            FireDragPlaneChanged();
        }
        /// <summary>
        /// Orients the plane in the X direction
        /// </summary>
        /// <param name="anchorPoint"></param>
        public void OrientPlaneX(int anchorPoint)
        {
            double z = GetVectorFromPoint(anchorPoint).Z;
            double x = GetVectorFromPoint(anchorPoint).X;

            bool hasDifferentXPoint = false;
            for (int i = 0; i < 4; i++)
            {
                if (GetVectorFromPoint(i).X != x)
                {
                    hasDifferentXPoint = true;
                }
            }

            if (!hasDifferentXPoint)
                return;

            NRange viewRange = m_PointSeries.GetFirstAncestor<NCartesianChart>().Axes[ENCartesianAxis.Depth].ViewRange;
            double planeLength = GetPlaneLength(viewRange, z, anchorPoint, false);

            // make the x coordinate equal
            for (int i = 0; i < 4; i++)
            {
                if (i != anchorPoint)
                {
                    SetXPointCoordinate(i, x);
                }
            }

            for (int i = 0; i < 4; i++)
            {
                if (GetVectorFromPoint(i).Z != z)
                {
                    SetZPointCoordinate(i, z + planeLength);
                }
            }

            // update the plane / intersections
            UpdateMeshSurface();
            FireDragPlaneChanged();
        }
        /// <summary>
        /// Orients the plane in the Z direction
        /// </summary>
        public void OrientPlaneZ(int anchorPoint)
        {
            double z = GetVectorFromPoint(anchorPoint).Z;
            double x = GetVectorFromPoint(anchorPoint).X;

            bool hasDifferentZPoint = false;
            for (int i = 0; i < 4; i++)
            {
                if (GetVectorFromPoint(i).Z != z)
                {
                    hasDifferentZPoint = true;
                }
            }

            if (!hasDifferentZPoint)
                return;

            NRange viewRange = m_PointSeries.GetFirstAncestor<NCartesianChart>().Axes[ENCartesianAxis.PrimaryX].ViewRange;
            double planeLength = GetPlaneLength(viewRange, x, anchorPoint, true);

            // make the z coordinate equal
            for (int i = 0; i < 4; i++)
            {
                if (i != anchorPoint)
                {
                    SetZPointCoordinate(i, z);
                }
            }

            for (int i = 0; i < 4; i++)
            {
                if (GetVectorFromPoint(i).X != x)
                {
                    SetXPointCoordinate(i, x + planeLength);
                }
            }

            UpdateMeshSurface();
            FireDragPlaneChanged();
        }
        /// <summary>
        /// Synchronizes the points so that they are coplanar
        /// </summary>
        /// <param name="modifiedPointIndex"></param>
        public void SynchronizePoints(int modifiedPointIndex)
        {
            // then align points depending on which point is being dragged
            NVector3DD vecA = GetVectorFromPoint(0);
            NVector3DD vecB = GetVectorFromPoint(1);
            NVector3DD vecC = GetVectorFromPoint(2);
            NVector3DD vecD = GetVectorFromPoint(3);

            switch (modifiedPointIndex)
            {
                case 0: // left top
                        // sync point 3 (left bottom)
                    {
                        NVector3DD vecCB = new NVector3DD();
                        vecCB.Subtract(ref vecC, ref vecB);

                        vecD.Add(ref vecA, ref vecCB);

                        SetVectorToPoint(3, vecD);
                    }
                    break;
                case 1: // right top
                    {
                        // sync point 2 (right bottom)
                        NVector3DD vecDA = new NVector3DD();
                        vecDA.Subtract(ref vecD, ref vecA);

                        vecC.Add(ref vecB, ref vecDA);

                        SetVectorToPoint(2, vecC);
                    }
                    break;
                case 2: // right bottom
                    {
                        // sync point 1 (right top)
                        NVector3DD vecAD = new NVector3DD();
                        vecAD.Subtract(ref vecA, ref vecD);

                        vecB.Add(ref vecC, ref vecAD);

                        SetVectorToPoint(1, vecB);
                    }
                    break;
                case 3: // left bottom
                    {
                        // sync point 0 (left top)
                        NVector3DD vecCB = new NVector3DD();
                        vecCB.Subtract(ref vecB, ref vecC);

                        vecA.Add(ref vecD, ref vecCB);

                        SetVectorToPoint(0, vecA);
                    }
                    break;
            }

            // handle x / z locking
            if (m_LockX)
            {
                double x = GetVectorFromPoint(modifiedPointIndex).X;
                for (int i = 0; i < 4; i++)
                {
                    if (i != modifiedPointIndex)
                    {
                        SetXPointCoordinate(i, x);
                    }
                }
            }

            if (m_LockZ)
            {
                double z = GetVectorFromPoint(modifiedPointIndex).Z;

                for (int i = 0; i < 4; i++)
                {
                    if (i != modifiedPointIndex)
                    {
                        SetZPointCoordinate(i, z);
                    }
                }
            }
        }
        /// <summary>
        /// Restores the position of the point
        /// </summary>
        /// <param name="dataPointIndex"></param>
        /// <param name="vector"></param>
        public void RestorePoint(int dataPointIndex, NVector3DD vector)
        {
            SetVectorToPoint(dataPointIndex, vector);

            SynchronizePoints(dataPointIndex);
            UpdateMeshSurface();

            FireDragPlaneChanged();
        }
        /// <summary>
        /// Gets the vector from the currently selected point
        /// </summary>
        /// <param name="dataPointIndex"></param>
        /// <returns></returns>
        public NVector3DD GetVectorFromPoint(int dataPointIndex)
        {
            NVector3DD vector;

            NPointDataPoint dataPoint = m_PointSeries.DataPoints[dataPointIndex];

            vector.X = (double)dataPoint.X;
            vector.Y = (double)dataPoint.Y;
            vector.Z = (double)dataPoint.Z;

            return vector;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the drag plane has changed
        /// </summary>
        [field: NonSerialized]
        public event EventHandler DragPlaneChanged;

        /// <summary>
        /// Raises the drag plane changed event
        /// </summary>
        internal void FireDragPlaneChanged()
        {
            if (m_DragPlaneChanged)
            {
                m_DragPlaneChanged = false;

                if (DragPlaneChanged != null)
                {
                    DragPlaneChanged(this, new EventArgs());
                }
            }
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Clamps the passed x coordinate to the x axis range
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private double ClampXCoordinateToRuler(double x)
        {
            return m_PointSeries.GetFirstAncestor<NCartesianChart>().Axes[ENCartesianAxis.PrimaryX].RulerRange.GetValueInRange(x);
        }
        /// <summary>
        /// Clamps the passed z coordinate to the x axis range
        /// </summary>
        /// <param name="z"></param>
        /// <returns></returns>
        private double ClampZCoordinateToRuler(double z)
        {
            return m_PointSeries.GetFirstAncestor<NCartesianChart>().Axes[ENCartesianAxis.Depth].RulerRange.GetValueInRange(z);
        }
        /// <summary>
        /// Sets the vector to the specified point
        /// </summary>
        /// <param name="dataPointIndex"></param>
        /// <param name="vector"></param>
        private void SetVectorToPoint(int dataPointIndex, NVector3DD vector)
        {
            SetXPointCoordinate(dataPointIndex, vector.X);
            SetYPointCoordinate(dataPointIndex, vector.Y);
            SetZPointCoordinate(dataPointIndex, vector.Z);
        }
        /// <summary>
        /// Sets an x point coordinate
        /// </summary>
        /// <param name="dataPointIndex"></param>
        /// <param name="x"></param>
        private void SetXPointCoordinate(int dataPointIndex, double x)
        {
            if ((double)m_PointSeries.DataPoints[dataPointIndex].X != x)
            {
                m_DragPlaneChanged = true;
                m_PointSeries.DataPoints[dataPointIndex].X = x;
            }
        }
        /// <summary>
        /// Sets an y point coordinate
        /// </summary>
        /// <param name="dataPointIndex"></param>
        /// <param name="x"></param>
        private void SetYPointCoordinate(int dataPointIndex, double y)
        {
            if ((double)m_PointSeries.DataPoints[dataPointIndex].Y != y)
            {
                m_DragPlaneChanged = true;
                m_PointSeries.DataPoints[dataPointIndex].Y = y;
            }
        }
        /// <summary>
        /// Sets an y point coordinate
        /// </summary>
        /// <param name="dataPointIndex"></param>
        /// <param name="x"></param>
        private void SetZPointCoordinate(int dataPointIndex, double z)
        {
            if ((double)m_PointSeries.DataPoints[dataPointIndex].Z != z)
            {
                m_DragPlaneChanged = true;
                m_PointSeries.DataPoints[dataPointIndex].Z = z;
            }
        }
        /// <summary>
        /// Updates the mesh surface from the point series
        /// </summary>
        private void UpdateMeshSurface()
        {
            NVector3DD vecA = GetVectorFromPoint(0);
            NVector3DD vecB = GetVectorFromPoint(1);
            NVector3DD vecC = GetVectorFromPoint(2);
            NVector3DD vecD = GetVectorFromPoint(3);

            m_MeshSurface.Data.SetValue(0, 0, vecA.Y, vecA.X, vecA.Z);
            m_MeshSurface.Data.SetValue(0, 1, vecB.Y, vecB.X, vecB.Z);
            m_MeshSurface.Data.SetValue(1, 1, vecC.Y, vecC.X, vecC.Z);
            m_MeshSurface.Data.SetValue(1, 0, vecD.Y, vecD.X, vecD.Z);
            m_MeshSurface.Data.OnDataChanged();
        }

        #endregion

        #region Fields

        NPointSeries m_PointSeries;
        NMeshSurfaceSeries m_MeshSurface;

        bool m_LockX;
        bool m_LockZ;

        bool m_DragPlaneChanged;

        #endregion
    }
}
