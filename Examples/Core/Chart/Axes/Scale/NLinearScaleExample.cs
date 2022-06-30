using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Linear Scale Example
	/// </summary>
	public class NLinearScaleExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NLinearScaleExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NLinearScaleExample()
		{
			NLinearScaleExampleSchema = NSchema.Create(typeof(NLinearScaleExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Linear Scale";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

			// configure axes
			m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XOrdinalYLinear);

			// configure the y axis
			NLinearScale linearScale = (NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale;
			
			NScaleGridLines majorGrid = new NScaleGridLines();
			majorGrid.Stroke = new NStroke(1, NColor.DarkGray, ENDashStyle.Dot);
			linearScale.MajorGridLines = majorGrid;

			// add a strip line style
			NScaleStrip strip = new NScaleStrip();
			strip.Fill = new NColorFill(NColor.Beige);
			strip.Interlaced = true;
			linearScale.Strips.Add(strip);

			NLineSeries line = new NLineSeries();
			m_Chart.Series.Add(line);

			Random random = new Random();
			for (int i = 0; i < 7; i++)
			{
				line.DataPoints.Add(new NLineDataPoint(random.Next(-100, 100)));
			}

			line.LegendView.Mode = ENSeriesLegendMode.None;
			line.InflateMargins = true;

			// assign marker
			NMarkerStyle markerStyle = new NMarkerStyle();

			markerStyle.Visible = true;
			markerStyle.Shape = ENPointShape.Ellipse;
			markerStyle.Size = new NSize(6, 6);

			line.MarkerStyle = markerStyle;

			// assign data label style
			NDataLabelStyle dataLabelStyle = new NDataLabelStyle();

			dataLabelStyle.Format = "<value>";
			dataLabelStyle.ArrowStroke.Color = NColor.CornflowerBlue;

			line.DataLabelStyle = dataLabelStyle;

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

			m_MajorTickModeComboBox = new NComboBox();
			m_MajorTickModeComboBox.FillFromEnum<ENMajorTickMode>();
			m_MajorTickModeComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnMajorTickModeComboBoxSelectedIndexChanged);
			stack.Add(NPairBox.Create("Major Tick Mode:", m_MajorTickModeComboBox));

			m_MinDistanceUpDown = new NNumericUpDown();
			m_MinDistanceUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnMinDistanceUpDownValueChanged);
			stack.Add(NPairBox.Create("Min Distance:", m_MinDistanceUpDown));
			m_MinDistanceUpDown.Value = 25;

			m_MaxCountNumericUpDown = new NNumericUpDown();
			m_MaxCountNumericUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnMaxCountNumericUpDownValueChanged);
			stack.Add(NPairBox.Create("Max Count:", m_MaxCountNumericUpDown));
			m_MaxCountNumericUpDown.Value = 10;
			
			m_CustomStepUpDown = new NNumericUpDown();
			m_CustomStepUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnCustomStepUpDownValueChanged);
			stack.Add(NPairBox.Create("Custom Step:", m_CustomStepUpDown));
			m_CustomStepUpDown.Value = 1;

			NCheckBox invertedCheckBox = new NCheckBox("Inverted");
			invertedCheckBox.CheckedChanged += OnInvertedCheckBoxCheckedChanged;
			invertedCheckBox.Checked = false;
			stack.Add(invertedCheckBox);

			NButton generateRandomDataButton = new NButton("Generate Random Data");
			generateRandomDataButton.Click += new Function<NEventArgs>(OnGenerateRandomDataButtonClick);
			stack.Add(generateRandomDataButton);

			m_MajorTickModeComboBox.SelectedIndex = (int)ENMajorTickMode.AutoMinDistance;
			OnMajorTickModeComboBoxSelectedIndexChanged(null);
			
			return boxGroup;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a linear (numeric) scale.</p>";
		}

		#endregion

		#region Event Handlers

		void OnInvertedCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			((NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale).Invert = ((NCheckBox)arg.TargetNode).Checked;
		}

		void OnGenerateRandomDataButtonClick(NEventArgs arg)
		{
			NLineSeries line = (NLineSeries)m_Chart.Series[0];

			line.DataPoints.Clear();

			Random random = new Random();
			for (int i = 0; i < 10; i++)
			{
				line.DataPoints.Add(new NLineDataPoint(random.Next(100)));
			}
		}

		void OnCustomStepUpDownValueChanged(NValueChangeEventArgs arg)
		{
			((NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale).CustomStep = ((NNumericUpDown)arg.TargetNode).Value;
		}

		void OnMinDistanceUpDownValueChanged(NValueChangeEventArgs arg)
		{
			((NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale).MinTickDistance = (int)((NNumericUpDown)arg.TargetNode).Value;
		}

		void OnMaxCountNumericUpDownValueChanged(NValueChangeEventArgs arg)
		{
			((NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale).MaxTickCount = (int)((NNumericUpDown)arg.TargetNode).Value;
		}

		void OnMajorTickModeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			ENMajorTickMode majorTickMode = (ENMajorTickMode)m_MajorTickModeComboBox.SelectedIndex;

			m_MaxCountNumericUpDown.Enabled = majorTickMode == ENMajorTickMode.AutoMaxCount;
			m_MinDistanceUpDown.Enabled = majorTickMode == ENMajorTickMode.AutoMinDistance;
			m_CustomStepUpDown.Enabled = majorTickMode == ENMajorTickMode.CustomStep;

			((NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale).MajorTickMode = majorTickMode;
		}

		#endregion

		#region Fields

		NCartesianChart m_Chart;

		NComboBox m_MajorTickModeComboBox;
		NNumericUpDown m_MaxCountNumericUpDown;
		NNumericUpDown m_MinDistanceUpDown;
		NNumericUpDown m_CustomStepUpDown;

		#endregion

		#region Schema

		public static readonly NSchema NLinearScaleExampleSchema;

		#endregion
	}
}