using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Hit testing Example
	/// </summary>
	public class NHitTestingExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NHitTestingExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NHitTestingExample()
		{
			NHitTestingExampleSchema = NSchema.Create(typeof(NHitTestingExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Hit Testing";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

			m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XYLinear);

			// setup X axis
			NLinearScale scaleX = new NLinearScale();
			scaleX.MajorGridLines = new NScaleGridLines();
			scaleX.MajorGridLines.Stroke.DashStyle = ENDashStyle.Dot;
			m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale = scaleX;

			// setup Y axis
			NLinearScale scaleY = new NLinearScale();
			scaleY.MajorGridLines = new NScaleGridLines();
			scaleY.MajorGridLines.Stroke.DashStyle = ENDashStyle.Dot;
			m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale = scaleY;

			// add a point series
			m_Point = new NPointSeries();
			m_Point.Name = "Point Series";
			m_Point.DataLabelStyle = new NDataLabelStyle(false);
			m_Point.Fill = new NColorFill(new NColor(NColor.DarkOrange, 160));
			m_Point.Size = 20;
			m_Point.Shape = ENPointShape.Rectangle;
			m_Point.UseXValues = true;
			m_Chart.Series.Add(m_Point);

			// add some sample data
			NDataPointCollection<NPointDataPoint> dataPoints = m_Point.DataPoints;

			Random random = new Random();

			for (int i = 0; i < 10; i++)
			{
				double u1 = random.NextDouble();
				double u2 = random.NextDouble();

				if (u1 == 0)
					u1 += 0.0001;

				if (u2 == 0)
					u2 += 0.0001;

				double z0 = Math.Sqrt(-2 * Math.Log(u1)) * Math.Cos(2 * Math.PI * u2);
				double z1 = Math.Sqrt(-2 * Math.Log(u1)) * Math.Sin(2 * Math.PI * u2);

				NPointDataPoint dataPoint = new NPointDataPoint(z0, z1);
				dataPoint.MouseDown += OnDataPointMouseDown;
				dataPoints.Add(dataPoint);
			}

			m_Chart.Enabled = true;
			NInteractor interactor = new NInteractor();

			m_AxisCursorsTool = new NAxisCursorTool();
			m_AxisCursorsTool.Enabled = true;
			interactor.Add(m_AxisCursorsTool);
			m_Chart.Interactor = interactor;

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);
			
			NComboBox hitTestModeComboBox = new NComboBox();
			hitTestModeComboBox.FillFromEnum<ENHitTestMode>();
			hitTestModeComboBox.SelectedIndexChanged += OnHitTestModeComboBoxSelectedIndexChanged;
			hitTestModeComboBox.SelectedIndex = (int)ENScaleOrientation.Auto;
			stack.Add(NPairBox.Create("Hit Test Mode:", hitTestModeComboBox));

			NComboBox orientationComboBox = new NComboBox();
			orientationComboBox.FillFromEnum<ENCartesianChartOrientation>();
			orientationComboBox.SelectedIndex = (int)m_Chart.Orientation;
			orientationComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnOrientationComboBoxSelectedIndexChanged);
			stack.Add(NPairBox.Create("Orientation:", orientationComboBox));

			NButton resetColorsButton = new NButton("Reset Colors");
			resetColorsButton.Click += OnResetColorsButtonClick;
			stack.Add(resetColorsButton);

			return group;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to implement chart element hit testing.</p>";
		}

		#endregion

		#region Event Handlers

		void OnResetColorsButtonClick(NEventArgs arg)
		{
			int seriesCount = m_Chart.Series.Count;
			for (int i = 0; i < seriesCount; i++)
			{
				NSeries series = m_Chart.Series[i];

				int dataPointCount = series.GetDataPointsChild().GetChildrenCount();

				for (int j = 0; j < dataPointCount; j++)
				{
					NDataPoint dataPoint = (NDataPoint)series.GetDataPointsChild().GetChildAt(j);

					dataPoint.ClearLocalValue(NDataPoint.FillProperty);
					dataPoint.ClearLocalValue(NDataPoint.StrokeProperty);
				}
			}
		}
		void OnHitTestModeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_Chart.HitTestMode = (ENHitTestMode)((NComboBox)arg.TargetNode).SelectedIndex;
		}
		void OnOrientationComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_Chart.Orientation = (ENCartesianChartOrientation)((NComboBox)arg.TargetNode).SelectedIndex;
		}
	
		public static void OnDataPointMouseDown(NMouseButtonEventArgs arg)
		{
			if (arg.TargetNode is NDataPoint)
			{
				((NDataPoint)arg.TargetNode).Fill = new NColorFill(NColor.Blue);
				((NDataPoint)arg.TargetNode).Stroke = new NStroke(2, NColor.Blue);
			}
			else if (arg.TargetNode is NSeries)
			{
				((NSeries)arg.TargetNode).Fill = new NColorFill(NColor.Blue);
			}
		}

		#endregion

		#region Fields

		NPointSeries m_Point;
		NCartesianChart m_Chart;

		NAxisCursorTool m_AxisCursorsTool;

		#endregion

		#region Schema

		public static readonly NSchema NHitTestingExampleSchema;

		#endregion
	}
}