using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Error Bar Example
	/// </summary>
	public class NStandardErrorBarExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NStandardErrorBarExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NStandardErrorBarExample()
		{
			NStandardErrorBarExampleSchema = NSchema.Create(typeof(NStandardErrorBarExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Standard Error Bar";

			// configure chart
			NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];

			chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XYLinear);

			// add interlace stripe
			NScaleStrip stripStyle = new NScaleStrip(new NColorFill(NColor.Beige), null, true, 0, 0, 1, 1);
			stripStyle.Interlaced = true;
			chart.Axes[ENCartesianAxis.PrimaryY].Scale.Strips.Add(stripStyle);
			chart.Axes[ENCartesianAxis.PrimaryY].Scale.MajorGridLines.Visible = true;

			chart.Axes[ENCartesianAxis.PrimaryY].Scale.ViewRangeInflateMode = ENScaleViewRangeInflateMode.MajorTick;
			chart.Axes[ENCartesianAxis.PrimaryY].Scale.InflateViewRangeBegin = true;
			chart.Axes[ENCartesianAxis.PrimaryY].Scale.InflateViewRangeEnd = true;
			chart.Axes[ENCartesianAxis.PrimaryX].Scale.ViewRangeInflateMode = ENScaleViewRangeInflateMode.MajorTick;
			chart.Axes[ENCartesianAxis.PrimaryX].Scale.InflateViewRangeBegin = true;
			chart.Axes[ENCartesianAxis.PrimaryX].Scale.InflateViewRangeEnd = true;

			// add an error bar series
			m_ErrorBar = new NErrorBarSeries();
			chart.Series.Add(m_ErrorBar);
			m_ErrorBar.DataLabelStyle = new NDataLabelStyle(false);
			m_ErrorBar.Stroke = new NStroke(NColor.Black);
			m_ErrorBar.UseXValues = true;

			GenerateData();
			
			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);
			
			NCheckBox inflateMarginsCheckBox = new NCheckBox("Inflate Margins");
			inflateMarginsCheckBox.CheckedChanged += OnInflateMarginsCheckBoxCheckedChanged;
			inflateMarginsCheckBox.Checked = true;
			stack.Add(inflateMarginsCheckBox);

			NCheckBox showUpperXErrorCheckBox = new NCheckBox("Show Upper X Error");
			showUpperXErrorCheckBox.CheckedChanged += OnShowUpperXErrorCheckBoxCheckedChanged;
			showUpperXErrorCheckBox.Checked = true;
			stack.Add(showUpperXErrorCheckBox);

			NCheckBox showLowerXErrorCheckBox = new NCheckBox("Show Lower X Error");
			showLowerXErrorCheckBox.CheckedChanged += OnShowLowerXErrorCheckBoxCheckedChanged;
			showLowerXErrorCheckBox.Checked = true;
			stack.Add(showLowerXErrorCheckBox);

			NNumericUpDown xErrorSizeUpDown = new NNumericUpDown();
			xErrorSizeUpDown.Value = m_ErrorBar.XErrorSize;
			xErrorSizeUpDown.ValueChanged += OnXErrorSizeUpDownValueChanged;
			stack.Add(NPairBox.Create("X Error Size:", xErrorSizeUpDown));

			NCheckBox showUpperYErrorCheckBox = new NCheckBox("Show Upper Y Error");
			showUpperYErrorCheckBox.CheckedChanged += OnShowUpperYErrorCheckBoxCheckedChanged;
			showUpperYErrorCheckBox.Checked = true;
			stack.Add(showUpperYErrorCheckBox);

			NCheckBox showLowerYErrorCheckBox = new NCheckBox("Show Lower Y Error");
			showLowerYErrorCheckBox.CheckedChanged += OnShowLowerYErrorCheckBoxCheckedChanged;
			showLowerYErrorCheckBox.Checked = true;
			stack.Add(showLowerYErrorCheckBox);

			NNumericUpDown yErrorSizeUpDown = new NNumericUpDown();
			yErrorSizeUpDown.Value = m_ErrorBar.YErrorSize;
			yErrorSizeUpDown.ValueChanged +=OnYErrorSizeUpDownValueChanged;
			stack.Add(NPairBox.Create("X Error Size:", yErrorSizeUpDown));
						
			return group;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates the functionality of the error bar series.</p>";
		}

		#endregion

		#region Implementation

		private void GenerateData()
		{
			m_ErrorBar.DataPoints.Clear();

			double y;
			double x = 50.0;

			Random random = new Random();

			for (int i = 0; i < 15; i++)
			{
				y = 20 + random.NextDouble() * 30;
				x += 2.0 + random.NextDouble() * 2;

				double lowerYError = 1 + random.NextDouble();
				double upperYError = 1 + random.NextDouble();

				double lowerXError = 1 + random.NextDouble();
				double upperXError = 1 + random.NextDouble();

				m_ErrorBar.DataPoints.Add(new NErrorBarDataPoint(x, y, upperXError, lowerXError, upperYError, lowerYError));
			}
		}

		#endregion

		#region Event Handlers

		void OnInflateMarginsCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			m_ErrorBar.InflateMargins = ((NCheckBox)arg.TargetNode).Checked;
		}

				void OnYErrorSizeUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_ErrorBar.YErrorSize = ((NNumericUpDown)arg.TargetNode).Value;
		}

		void OnXErrorSizeUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_ErrorBar.XErrorSize = ((NNumericUpDown)arg.TargetNode).Value;
		}

		void OnShowLowerYErrorCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			m_ErrorBar.ShowLowerYError = ((NCheckBox)arg.TargetNode).Checked;
		}

		void OnShowUpperYErrorCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			m_ErrorBar.ShowUpperYError = ((NCheckBox)arg.TargetNode).Checked;
		}

		void OnShowLowerXErrorCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			m_ErrorBar.ShowLowerXError = ((NCheckBox)arg.TargetNode).Checked;
		}

		void OnShowUpperXErrorCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			m_ErrorBar.ShowUpperXError = ((NCheckBox)arg.TargetNode).Checked;
		}

		#endregion

		#region Fields

		NErrorBarSeries m_ErrorBar;

		#endregion

		#region Schema

		public static readonly NSchema NStandardErrorBarExampleSchema;

		#endregion
	}
}