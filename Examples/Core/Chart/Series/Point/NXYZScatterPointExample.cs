using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// XYZ Scatter Point Example
	/// </summary>
	public class NXYZScatterPointExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NXYZScatterPointExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NXYZScatterPointExample()
		{
			NXYZScatterPointExampleSchema = NSchema.Create(typeof(NXYZScatterPointExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "XYZ Scatter Point";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];
			m_Chart.Enable3D = true;
			m_Chart.FitMode = ENCartesianChartFitMode.Aspect;
			m_Chart.ModelWidth = m_Chart.ModelHeight = m_Chart.ModelHeight = 50;

			m_Chart.Projection.SetPredefinedProjection(ENPredefinedProjection.PerspectiveTilted);
			m_Chart.LightModel.SetPredefinedLightModel(ENPredefinedLightModel.NorthernLights);

            m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XYZLinear);
            m_Chart.Interactor = new NInteractor(new NTrackballTool());

            // setup X axis
            {
				NLinearScale linearScaleX = new NLinearScale();
				m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale = linearScaleX;
				linearScaleX.MajorGridLines.Stroke.DashStyle = ENDashStyle.Dot;
				linearScaleX.MajorGridLines.SetShowAtWall(ENChartWall.Back, true);
				linearScaleX.MajorGridLines.SetShowAtWall(ENChartWall.Bottom, true);

				NScaleStrip m_Strip = new NScaleStrip(new NColorFill(NColor.DarkGray), null, true, 0, 0, 1, 1);
                m_Strip.Interlaced = true;
                linearScaleX.Strips.Add(m_Strip);
            }

            // setup Y axis
            {
                NLinearScale linearScaleY = new NLinearScale();
                m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale = linearScaleY;
				linearScaleY.MajorGridLines.Stroke.DashStyle = ENDashStyle.Dot;
				linearScaleY.MajorGridLines.SetShowAtWall(ENChartWall.Back, true);
				linearScaleY.MajorGridLines.SetShowAtWall(ENChartWall.Left, true);

                NScaleStrip m_Strip = new NScaleStrip(new NColorFill(NColor.DarkGray), null, true, 0, 0, 1, 1);
                m_Strip.Interlaced = true;
                linearScaleY.Strips.Add(m_Strip);
            }

            // setup Z axis
            {
                NLinearScale linearScaleZ = new NLinearScale();
                m_Chart.Axes[ENCartesianAxis.Depth].Scale = linearScaleZ;
                linearScaleZ.MajorGridLines.Stroke.DashStyle = ENDashStyle.Dot;
                linearScaleZ.MajorGridLines.SetShowAtWall(ENChartWall.Bottom, true);
                linearScaleZ.MajorGridLines.SetShowAtWall(ENChartWall.Left, true);

                NScaleStrip m_Strip = new NScaleStrip(new NColorFill(NColor.DarkGray), null, true, 0, 0, 1, 1);
                m_Strip.Interlaced = true;
                linearScaleZ.Strips.Add(m_Strip);
            }

            // add a point series
            m_Point = new NPointSeries();
			m_Point.Name = "Point Series";
			m_Point.DataLabelStyle = new NDataLabelStyle(false);
			m_Point.Fill = new NColorFill(new NColor(NColor.DarkOrange, 160));
			m_Point.Size = 20;
			m_Point.Shape = ENPointShape3D.Sphere;
			m_Point.UseXValues = true;
            m_Point.UseZValues = true;
			m_Point.ValueFormatter = new NNumericValueFormatter(ENNumericValueFormat.LimitedPrecision2);
            m_Chart.Series.Add(m_Point);

			OnNewDataButtonClick(null);

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, ENChartPaletteTarget.DataPoints));

			return chartViewWithCommandBars;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);
			
			NCheckBox inflateMarginsCheckBox = new NCheckBox();
			inflateMarginsCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnInflateMarginsCheckBoxCheckedChanged);
			stack.Add(NPairBox.Create("Inflate Margins: ", inflateMarginsCheckBox));

			NCheckBox verticalAxisRoundToTick = new NCheckBox();
			verticalAxisRoundToTick.CheckedChanged += new Function<NValueChangeEventArgs>(OnAxesRoundToTickCheckedChanged);
			stack.Add(NPairBox.Create("Axes Round To Tick: ", verticalAxisRoundToTick));
			verticalAxisRoundToTick.Checked = true;

            NCheckBox showDataLabels = new NCheckBox("Show Data Labels");
            showDataLabels.CheckedChanged += new Function<NValueChangeEventArgs>(OnShowDataLabelsCheckedChanged);
            showDataLabels.Checked = false;
            stack.Add(showDataLabels);

            NButton newDataButton = new NButton("New Data");
			newDataButton.Click += new Function<NEventArgs>(OnNewDataButtonClick);
			stack.Add(newDataButton);

			return group;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a xy scatter point chart.</p>";
		}

        #endregion

        #region Event Handlers

        void OnShowDataLabelsCheckedChanged(NValueChangeEventArgs arg)
        {
            if ((arg.TargetNode as NCheckBox).Checked)
            {
                m_Point.DataLabelStyle = new NDataLabelStyle(true);
                m_Point.DataLabelStyle.Format = "<value>";
            }
            else
            {
                m_Point.DataLabelStyle = new NDataLabelStyle(false);
            }
        }

        void OnNewDataButtonClick(NEventArgs arg)
		{
			m_Point.DataPoints.Clear();
			NDataPointCollection<NPointDataPoint> dataPoints = m_Point.DataPoints;

			Random random = new Random();

			for (int i = 0; i < 10; i++)
			{
				double u1 = random.NextDouble();
				double u2 = random.NextDouble();
                double u3 = random.NextDouble();

                if (u1 == 0)
					u1 += 0.0001;

				if(u2 == 0)
					u2 += 0.0001;

				double z0 = Math.Sqrt(-2 * Math.Log(u1)) * Math.Cos(2 * Math.PI * u2);
				double z1 = Math.Sqrt(-2 * Math.Log(u1)) * Math.Sin(2 * Math.PI * u2);
                double z2 = Math.Sqrt(-2 * Math.Log(u1)) * Math.Sin(2 * Math.PI * u3);

                dataPoints.Add(new NPointDataPoint(z0, z1, z2));
			}
		}

		void OnAxesRoundToTickCheckedChanged(NValueChangeEventArgs arg)
		{
			for (int i = 0; i < m_Chart.Axes.Count; i++)
			{
				NLinearScale linearScale = m_Chart.Axes[i].Scale as NLinearScale;

				if (linearScale != null)
				{
					if ((arg.TargetNode as NCheckBox).Checked)
					{
						linearScale.ViewRangeInflateMode = ENScaleViewRangeInflateMode.MajorTick;
						linearScale.InflateViewRangeBegin = true;
						linearScale.InflateViewRangeEnd = true;
					}
					else
					{
						linearScale.ViewRangeInflateMode = ENScaleViewRangeInflateMode.Logical;
					}
				}
			}
		}

		void OnInflateMarginsCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			m_Point.InflateMargins = (arg.TargetNode as NCheckBox).Checked;
		}

		#endregion

		#region Fields

		NPointSeries m_Point;
		NCartesianChart m_Chart;

		#endregion

		#region Schema

		public static readonly NSchema NXYZScatterPointExampleSchema;

		#endregion
	}
}
