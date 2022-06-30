using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;
using System;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Axis sections example
	/// </summary>
	public class NAxisSectionsExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NAxisSectionsExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NAxisSectionsExample()
		{
			NAxisSectionsExampleSchema = NSchema.Create(typeof(NAxisSectionsExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Axis Sections";

			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

			m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XYLinear);

			// create a point series
			NPointSeries point = new NPointSeries();
			point.Name = "Point Series";
			point.DataLabelStyle = new NDataLabelStyle(false);
			point.Size = 5;

			Random random = new Random();

			for (int i = 0; i < 30; i++)
			{
				point.DataPoints.Add(new NPointDataPoint(random.Next(100), random.Next(100)));
			}

			point.InflateMargins = true;

			m_Chart.Series.Add(point);

			// tell the x axis to display major and minor grid lines
			NLinearScale linearScale = new NLinearScale();
			m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale = linearScale;
			linearScale.MajorGridLines = new NScaleGridLines();
			linearScale.MajorGridLines.Stroke.DashStyle = ENDashStyle.Solid;
			linearScale.MinorGridLines = new NScaleGridLines();
			linearScale.MinorGridLines.Stroke.DashStyle = ENDashStyle.Dot;
			linearScale.MinorTickCount = 1;

			// tell the y axis to display major and minor grid lines
			linearScale = new NLinearScale();
			m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale = linearScale;
			linearScale.MajorGridLines = new NScaleGridLines();
			linearScale.MajorGridLines.Stroke.DashStyle = ENDashStyle.Solid;
			linearScale.MinorGridLines = new NScaleGridLines();
			linearScale.MinorGridLines.Stroke.DashStyle = ENDashStyle.Dot;
			linearScale.MinorTickCount = 1;

			NTextStyle labelStyle;

			// configure the first horizontal section
			m_FirstHorizontalSection = new NScaleSection();
			m_FirstHorizontalSection.Range = new NRange(2, 8);
			m_FirstHorizontalSection.RangeFill = new NColorFill(new NColor(NColor.Red, 60));
			m_FirstHorizontalSection.MajorGridStroke = new NStroke(NColor.Red);
			m_FirstHorizontalSection.MajorTickStroke = new NStroke(NColor.DarkRed);
			m_FirstHorizontalSection.MinorTickStroke = new NStroke(1, NColor.Red, ENDashStyle.Dot);

			labelStyle = new NTextStyle();
			labelStyle.Fill = new NStockGradientFill(NColor.Red, NColor.DarkRed);
			labelStyle.Font.Style = ENFontStyle.Bold;
			m_FirstHorizontalSection.LabelTextStyle = labelStyle;

			// configure the second horizontal section
			m_SecondHorizontalSection = new NScaleSection();
			m_SecondHorizontalSection.Range = new NRange(14, 18);
			m_SecondHorizontalSection.RangeFill = new NColorFill(new NColor(NColor.Green, 60));
			m_SecondHorizontalSection.MajorGridStroke = new NStroke(NColor.Green);
			m_SecondHorizontalSection.MajorTickStroke = new NStroke(NColor.DarkGreen);
			m_SecondHorizontalSection.MinorTickStroke = new NStroke(1, NColor.Green, ENDashStyle.Dot);

			labelStyle = new NTextStyle();
			labelStyle.Fill = new NStockGradientFill(NColor.Green, NColor.DarkGreen);
			labelStyle.Font.Style = ENFontStyle.Bold;
			m_SecondHorizontalSection.LabelTextStyle = labelStyle;

			// configure the first vertical section
			m_FirstVerticalSection = new NScaleSection();
			m_FirstVerticalSection.Range = new NRange(20, 40);
			m_FirstVerticalSection.RangeFill = new NColorFill(new NColor(NColor.Blue, 60));
			m_FirstVerticalSection.MajorGridStroke = new NStroke(NColor.Blue);
			m_FirstVerticalSection.MajorTickStroke = new NStroke(NColor.DarkBlue);
			m_FirstVerticalSection.MinorTickStroke= new NStroke(1, NColor.Blue, ENDashStyle.Dot);

			labelStyle = new NTextStyle();
			labelStyle.Fill = new NStockGradientFill(NColor.Blue, NColor.DarkBlue);
			labelStyle.Font.Style = ENFontStyle.Bold;
			m_FirstVerticalSection.LabelTextStyle = labelStyle;

			// configure the second vertical section
			m_SecondVerticalSection = new NScaleSection();
			m_SecondVerticalSection.Range = new NRange(70, 90);
			m_SecondVerticalSection.RangeFill = new NColorFill(new NColor(NColor.Orange, 60));
			m_SecondVerticalSection.MajorGridStroke = new NStroke(NColor.Orange);
			m_SecondVerticalSection.MajorTickStroke = new NStroke(NColor.DarkOrange);
			m_SecondVerticalSection.MinorTickStroke = new NStroke(1, NColor.Orange, ENDashStyle.Dot);

			labelStyle = new NTextStyle();
			labelStyle.Fill = new NStockGradientFill(NColor.Orange, NColor.DarkOrange);
			labelStyle.Font.Style = ENFontStyle.Bold;
			m_SecondVerticalSection.LabelTextStyle = labelStyle;

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

			m_ShowFirstYSectionCheckBox = new NCheckBox("Show Y Section [20, 40]");
			stack.Add(m_ShowFirstYSectionCheckBox);
			m_ShowFirstYSectionCheckBox.Checked = true;

			m_ShowSecondYSectionCheckBox = new NCheckBox("Show Y Section [70, 90]");
			stack.Add(m_ShowSecondYSectionCheckBox);
			m_ShowSecondYSectionCheckBox.Checked = true;

			m_ShowFirstXSectionCheckBox = new NCheckBox("Show X Section [2, 8]");
			stack.Add(m_ShowFirstXSectionCheckBox);
			m_ShowFirstXSectionCheckBox.Checked = true;

			m_ShowSecondXSectionCheckBox = new NCheckBox("Show X Section [14, 18]");
			stack.Add(m_ShowSecondXSectionCheckBox);
			m_ShowSecondXSectionCheckBox.Checked = true;

			m_ShowFirstYSectionCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnUpdateSections);
			m_ShowSecondYSectionCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnUpdateSections);
			m_ShowFirstXSectionCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnUpdateSections);
			m_ShowSecondXSectionCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnUpdateSections);

			OnUpdateSections(null);

			return boxGroup;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates axis sections. Axis sections allow you to alter the appearance of different axis elements if they fall in a specified range.</p>";
		}

		#endregion

		#region Event Handlers

		void OnUpdateSections(NValueChangeEventArgs arg)
		{
			NStandardScale standardScale;

			// configure horizontal axis sections
			standardScale = (NStandardScale)m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale;
			standardScale.Sections.Clear();

			if (m_ShowFirstXSectionCheckBox.Checked)
			{
				standardScale.Sections.Add(m_FirstHorizontalSection);
			}

			if (m_ShowSecondXSectionCheckBox.Checked)
			{
				standardScale.Sections.Add(m_SecondHorizontalSection);
			}

			// configure vertical axis sections
			standardScale = (NStandardScale)m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale;
			standardScale.Sections.Clear();

			if (m_ShowFirstYSectionCheckBox.Checked)
			{
				standardScale.Sections.Add(m_FirstVerticalSection);
			}

			if (m_ShowSecondYSectionCheckBox.Checked)
			{
				standardScale.Sections.Add(m_SecondVerticalSection);
			}
		}

		#endregion

		#region Fields

		private NCartesianChart m_Chart;

		private NCheckBox m_ShowFirstYSectionCheckBox;
		private NCheckBox m_ShowSecondYSectionCheckBox;
		private NCheckBox m_ShowFirstXSectionCheckBox;
		private NCheckBox m_ShowSecondXSectionCheckBox;

		private NScaleSection m_FirstVerticalSection;
		private NScaleSection m_SecondVerticalSection;
		private NScaleSection m_FirstHorizontalSection;
		private NScaleSection m_SecondHorizontalSection;

		#endregion

		#region Schema

		public static readonly NSchema NAxisSectionsExampleSchema;

		#endregion
	}
}
