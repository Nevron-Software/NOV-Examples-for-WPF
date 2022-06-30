using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Axis Cursor Tool Example
	/// </summary>
	public class NAxisCursorToolExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NAxisCursorToolExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NAxisCursorToolExample()
		{
			NAxisCursorToolExampleSchema = NSchema.Create(typeof(NAxisCursorToolExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Axis Cursor Tool";

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
			m_Point.Size = 5;
			m_Point.Shape = ENPointShape.Rectangle;
			m_Point.UseXValues = true;
			m_Chart.Series.Add(m_Point);

			// add some sample data
			NDataPointCollection<NPointDataPoint> dataPoints = m_Point.DataPoints;

			Random random = new Random();

			for (int i = 0; i < 1000; i++)
			{
				double u1 = random.NextDouble();
				double u2 = random.NextDouble();

				if (u1 == 0)
					u1 += 0.0001;

				if (u2 == 0)
					u2 += 0.0001;

				double z0 = Math.Sqrt(-2 * Math.Log(u1)) * Math.Cos(2 * Math.PI * u2);
				double z1 = Math.Sqrt(-2 * Math.Log(u1)) * Math.Sin(2 * Math.PI * u2);

				dataPoints.Add(new NPointDataPoint(z0, z1));
			}

			m_Chart.Enabled = true;
			NInteractor interactor = new NInteractor();

			m_AxisCursorsTool = new NAxisCursorTool();
			m_AxisCursorsTool.Enabled = true;
			m_AxisCursorsTool.HorizontalValueChanged += OnAxisCursorsToolHorizontalValueChanged;
			m_AxisCursorsTool.VerticalValueChanged += OnAxisCursorsToolVerticalValueChanged;

			interactor.Add(m_AxisCursorsTool);
			m_Chart.Interactor = interactor;

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);
			
			NComboBox orientationComboBox = new NComboBox();
			orientationComboBox.FillFromEnum<ENCartesianChartOrientation>();
			orientationComboBox.SelectedIndex = (int)m_Chart.Orientation;
			orientationComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnOrientationComboBoxSelectedIndexChanged);
			stack.Add(NPairBox.Create("Orientation:", orientationComboBox));

			NCheckBox snapToMajorTicksCheckBox = new NCheckBox("Snap To Major Ticks");
			snapToMajorTicksCheckBox.CheckedChanged += OnSnapToMajorTicksCheckBoxCheckedChanged;
			stack.Add(snapToMajorTicksCheckBox);

			NCheckBox autoHideCheckBox = new NCheckBox("Auto Hide");
			autoHideCheckBox.CheckedChanged += OnAutoHideCheckBoxCheckedChanged;
			stack.Add(autoHideCheckBox);

			NCheckBox invertScaleCheckBox = new NCheckBox("Invert Scale");
			invertScaleCheckBox.CheckedChanged += OnInvertScaleCheckBoxCheckedChanged;
			stack.Add(invertScaleCheckBox);

			m_HorizontalValueLabel = new NLabel();
			stack.Add(NPairBox.Create("Horizontal Value:", m_HorizontalValueLabel));

			m_VerticalValueLabel = new NLabel();
			stack.Add(NPairBox.Create("Vertical Value:", m_VerticalValueLabel));

			return group;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to use the axis cursors tool.</p>";
		}

		#endregion

		#region Event Handlers

		void OnAutoHideCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="arg"></param>
		void OnSnapToMajorTicksCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			if (((NCheckBox)arg.TargetNode).Checked)
			{
				m_AxisCursorsTool.HorizontalValueSnapper = new NAxisMajorTickSnapper();
				m_AxisCursorsTool.VerticalValueSnapper = new NAxisMajorTickSnapper();
			}
			else
			{
				m_AxisCursorsTool.HorizontalValueSnapper = null;
				m_AxisCursorsTool.VerticalValueSnapper = null;
			}
		}

		void OnInvertScaleCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			for (int i = 0; i < m_Chart.Axes.Count; i++)
			{
				m_Chart.Axes[i].Scale.Invert = ((NCheckBox)arg.TargetNode).Checked;
			}
		}

		void OnOrientationComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_Chart.Orientation = (ENCartesianChartOrientation)((NComboBox)arg.TargetNode).SelectedIndex;
		}

		void OnAxisCursorsToolVerticalValueChanged(NValueChangeEventArgs arg)
		{
			NAxisCursorTool tool = (NAxisCursorTool)arg.TargetNode;

			if (double.IsNaN(tool.VerticalValue))
			{
				m_VerticalValueLabel.Text = string.Empty;
			}
			else
			{
				m_VerticalValueLabel.Text = tool.VerticalValue.ToString();
			}
		}

		void OnAxisCursorsToolHorizontalValueChanged(NValueChangeEventArgs arg)
		{
			NAxisCursorTool tool = (NAxisCursorTool)arg.TargetNode;

			if (double.IsNaN(tool.HorizontalValue))
			{
				m_HorizontalValueLabel.Text = string.Empty;
			}
			else
			{
				m_HorizontalValueLabel.Text = tool.HorizontalValue.ToString();
			}
		}

		#endregion

		#region Fields

		NPointSeries m_Point;
		NCartesianChart m_Chart;

		NAxisCursorTool m_AxisCursorsTool;
		NLabel m_HorizontalValueLabel;
		NLabel m_VerticalValueLabel;

		#endregion

		#region Schema

		public static readonly NSchema NAxisCursorToolExampleSchema;

		#endregion
	}
}
