using System;
using System.Globalization;

using Nevron.Nov.Chart;
using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Date Time Scale Example
	/// </summary>
	public class NDateTimeScaleExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NDateTimeScaleExample()
		{
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NDateTimeScaleExample()
		{
			NDateTimeScaleExampleSchema = NSchema.Create(typeof(NDateTimeScaleExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Date Time Scale";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];
			m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XDateTimeYLinear);

			// add interlaced stripe to the Y axis
			NLinearScale yScale = (NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale;
			NScaleStrip stripStyle = new NScaleStrip(new NColorFill(NColor.Beige), null, true, 0, 0, 1, 1);
			stripStyle.Interlaced = true;
			yScale.Strips.Add(stripStyle);

			// Add a line series (data will be generated with example controls)
			NLineSeries line = new NLineSeries();
			m_Chart.Series.Add(line);

			line.UseXValues = true;

			line.DataLabelStyle = new NDataLabelStyle(false);
			line.InflateMargins = true;

			NMarkerStyle markerStyle = new NMarkerStyle();
			markerStyle.Visible = true;
			line.MarkerStyle = markerStyle;

			// create a date time scale
			m_DateTimeScale = (NDateTimeScale)m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale;

			m_DateTimeScale.Labels.Style.Angle = new NScaleLabelAngle(ENScaleLabelAngleMode.Scale, 90);
			m_DateTimeScale.Labels.Style.ContentAlignment = ENContentAlignment.TopCenter;

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

			stack.Add(new NLabel("Allowed Date Time Units"));

			NDateTimeUnit[] dateTimeUnits = new NDateTimeUnit[] {
						NDateTimeUnit.Century,
						NDateTimeUnit.Decade,
						NDateTimeUnit.Year,
						NDateTimeUnit.HalfYear,
						NDateTimeUnit.Quarter,
						NDateTimeUnit.Month,
						NDateTimeUnit.Week,
						NDateTimeUnit.Day,
						NDateTimeUnit.HalfDay,
						NDateTimeUnit.Hour,
						NDateTimeUnit.Minute,
						NDateTimeUnit.Second,
						NDateTimeUnit.Millisecond,
						NDateTimeUnit.Tick
				};

			m_DateTimeUnitListBox = new NListBox();
			for (int i = 0; i < dateTimeUnits.Length; i++)
			{
				NDateTimeUnit dateTimeUnit = dateTimeUnits[i];
                NCheckBox dateTimeUnitCheckBox = new NCheckBox(NStringHelpers.InsertSpacesBeforeUppersAndDigits(dateTimeUnit.DateTimeUnit.ToString()));

				dateTimeUnitCheckBox.Checked = true;
				dateTimeUnitCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnDateTimeUnitCheckBoxCheckedChanged);
				dateTimeUnitCheckBox.Tag = dateTimeUnit;				

				m_DateTimeUnitListBox.Items.Add(new NListBoxItem(dateTimeUnitCheckBox));
			}

			stack.Add(m_DateTimeUnitListBox);
			OnDateTimeUnitCheckBoxCheckedChanged(null);

			NCheckBox enableUnitSensitiveFormattingCheckBox = new NCheckBox("Enable Unit Sensitive Formatting");
			enableUnitSensitiveFormattingCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnEnableUnitSensitiveFormattingCheckBoxCheckedChanged);
			stack.Add(enableUnitSensitiveFormattingCheckBox);

			enableUnitSensitiveFormattingCheckBox.Checked = true;

			stack.Add(new NLabel("Start Date:"));
			m_StartDateTimeBox = new NDateTimeBox();
			m_StartDateTimeBox.SelectedDateChanged += new Function<NValueChangeEventArgs>(OnStartDateTimeBoxSelectedDateChanged);
			stack.Add(m_StartDateTimeBox);

			stack.Add(new NLabel("End Date:"));
			m_EndDateTimeBox = new NDateTimeBox();
			m_EndDateTimeBox.SelectedDateChanged += new Function<NValueChangeEventArgs>(OnEndDateTimeBoxSelectedDateChanged);
			stack.Add(m_EndDateTimeBox);

			NButton generateRandomDataButton = new NButton("Generate Random Data");
			generateRandomDataButton.Click +=new Function<NEventArgs>(OnGenerateRandomDataButtonClick);
			stack.Add(generateRandomDataButton);

			m_StartDateTimeBox.SelectedDate = DateTime.Now;
			m_EndDateTimeBox.SelectedDate = CultureInfo.CurrentCulture.Calendar.AddYears(m_StartDateTimeBox.SelectedDate, 2);

			return boxGroup;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a standard date/time scale.</p>";
		}

		#endregion

		#region Event Handlers

		void OnEnableUnitSensitiveFormattingCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			if (((NCheckBox)arg.TargetNode).Checked)
			{
				m_DateTimeScale.Labels.TextProvider = new NDateTimeUnitSensitiveLabelTextProvider();
			}
			else
			{
				m_DateTimeScale.Labels.TextProvider = new NFormattedScaleLabelTextProvider(new NDateTimeValueFormatter(ENDateTimeValueFormat.Date));
			}
		}

		void OnEndDateTimeBoxSelectedDateChanged(NValueChangeEventArgs arg)
		{
			OnGenerateRandomDataButtonClick(null);
		}

		void OnStartDateTimeBoxSelectedDateChanged(NValueChangeEventArgs arg)
		{
			OnGenerateRandomDataButtonClick(null);
		}

		void OnGenerateRandomDataButtonClick(NEventArgs arg)
		{
			DateTime startDate = m_StartDateTimeBox.SelectedDate;
			DateTime endDate = m_EndDateTimeBox.SelectedDate;

			if (startDate > endDate)
			{
				DateTime temp = startDate;

				startDate = endDate;
				endDate = temp;
			}

			// Get the line series from the chart
			NLineSeries line = (NLineSeries)m_Chart.Series[0];

			TimeSpan span = endDate - startDate;
			span = new TimeSpan(span.Ticks / 30);

			line.DataPoints.Clear();

			if (span.Ticks > 0)
			{
				Random random = new Random();
				while (startDate < endDate)
				{
					line.DataPoints.Add(new NLineDataPoint(NDateTimeHelpers.ToOADate(startDate), random.Next(100)));

					startDate += span;
				}
			}
		}

		void OnDateTimeUnitCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			NList<NDateTimeUnit> dateTimeUnits = new NList<NDateTimeUnit>();

			// collect the checked date time units
			for (int i = 0; i < m_DateTimeUnitListBox.Items.Count; i++)
			{
				NCheckBox dateTimeUnitCheckBox = m_DateTimeUnitListBox.Items[i].Content as NCheckBox;

				if (dateTimeUnitCheckBox.Checked)
				{
					dateTimeUnits.Add((NDateTimeUnit)dateTimeUnitCheckBox.Tag);
				}
			}

			m_DateTimeScale.AutoDateTimeUnits = new NDomArray<NDateTimeUnit>(dateTimeUnits.ToArray());
		}

		#endregion

		#region Fields

		NCartesianChart m_Chart;

		NDateTimeScale m_DateTimeScale;
		NListBox m_DateTimeUnitListBox;
		NDateTimeBox m_StartDateTimeBox;
		NDateTimeBox m_EndDateTimeBox;

		#endregion

		#region Schema

		public static readonly NSchema NDateTimeScaleExampleSchema;

		#endregion
	}
}