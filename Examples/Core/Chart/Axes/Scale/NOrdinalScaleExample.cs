using System;

using Nevron.Nov.Chart;
using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Ordinal Scale Example
	/// </summary>
	public class NOrdinalScaleExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NOrdinalScaleExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NOrdinalScaleExample()
		{
			NOrdinalScaleExampleSchema = NSchema.Create(typeof(NOrdinalScaleExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Ordinal Scale";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

			// configure axes
			m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XOrdinalYLinear);

			// add interlaced stripe to the Y axis
			NLinearScale linearScale = (NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale;
			NScaleStrip stripStyle = new NScaleStrip();
			stripStyle.Fill = new NColorFill(NColor.Beige);
			stripStyle.Interlaced = true;
			linearScale.Strips.Add(stripStyle);

			// add some series
			int dataItemsCount = 6;
			NBarSeries bar = new NBarSeries();

			bar.InflateMargins = true;
			bar.DataLabelStyle = new NDataLabelStyle(false);

			Random random = new Random();

			for (int i = 0; i < dataItemsCount; i++)
			{
				bar.DataPoints.Add(new NBarDataPoint(random.Next(10, 30)));
			}

			m_Chart.Series.Add(bar);

			NOrdinalScale ordinalScale = (NOrdinalScale)m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale;
			ordinalScale.Labels.OverlapResolveLayouts = new NDomArray<ENLevelLabelsLayout>(new ENLevelLabelsLayout[] { ENLevelLabelsLayout.AutoScale });

			NList<string> labels = new NList<string>();
			for (int j = 0; j < dataItemsCount; j++)
			{
				labels.Add("Category " + j.ToString());
			}

			ordinalScale.Labels.TextProvider = new NOrdinalScaleLabelTextProvider(labels);

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

			NCheckBox displayDataPointsBetweenTicksCheckBox = new NCheckBox("Display Data Points Between Ticks");
			displayDataPointsBetweenTicksCheckBox.CheckedChanged +=new Function<NValueChangeEventArgs>(OnDisplayDataPointsBetweenTicksCheckBoxCheckedChanged);
			displayDataPointsBetweenTicksCheckBox.Checked = true;
			stack.Add(displayDataPointsBetweenTicksCheckBox);

			NCheckBox autoLabelsCheckBox = new NCheckBox("Auto Labels");
			autoLabelsCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnAutoLabelsCheckBoxCheckedChanged);
			autoLabelsCheckBox.Checked = true;
			stack.Add(autoLabelsCheckBox);

			NCheckBox invertedCheckBox = new NCheckBox("Inverted");
			invertedCheckBox.CheckedChanged += OnInvertedCheckBoxCheckedChanged;
			invertedCheckBox.Checked = false;
			stack.Add(invertedCheckBox);

			return boxGroup;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create an ordinal scale.</p>";
		}

		#endregion

		#region Event Handlers

		void OnAutoLabelsCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			if (((NCheckBox)arg.TargetNode).Checked)
			{
				((NOrdinalScale)m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale).Labels.TextProvider = new NFormattedScaleLabelTextProvider(new NNumericValueFormatter());
			}
			else
			{
				NList<string> labels = new NList<string>();
				int dataPointCount = ((NBarSeries)m_Chart.Series[0]).DataPoints.Count;

				for (int j = 0; j < dataPointCount; j++)
				{
					labels.Add("Category " + j.ToString());
				}

				((NOrdinalScale)m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale).Labels.TextProvider = new NOrdinalScaleLabelTextProvider(labels);
			}
		}

		void OnDisplayDataPointsBetweenTicksCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			((NOrdinalScale)m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale).DisplayDataPointsBetweenTicks = ((NCheckBox)arg.TargetNode).Checked;
		}

		void OnInvertedCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			((NOrdinalScale)m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale).Invert = ((NCheckBox)arg.TargetNode).Checked;
		}

		#endregion

		#region Implementation


		#endregion

		#region Fields

		NCartesianChart m_Chart;

		#endregion

		#region Schema

		public static readonly NSchema NOrdinalScaleExampleSchema;

		#endregion
	}
}