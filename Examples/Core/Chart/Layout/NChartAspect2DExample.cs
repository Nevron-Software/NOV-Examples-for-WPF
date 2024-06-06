using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Chart Aspect 2D example
	/// </summary>
	public class NChartAspect2DExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NChartAspect2DExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NChartAspect2DExample()
		{
			NChartAspect2DExampleSchema = NSchema.Create(typeof(NChartAspect2DExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			m_ChartView = chartViewWithCommandBars.View;
			m_ChartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			m_ChartView.Surface.Titles[0].Text = "Chart Aspect 2D";

			// configure chart
			NCartesianChart chart = (NCartesianChart)m_ChartView.Surface.Charts[0];

			chart.FitMode = ENCartesianChartFitMode.Aspect;

			// Add a x linear axis
			NCartesianAxis primaryXAxis = new NCartesianAxis();
			primaryXAxis.Anchor = new NDockCartesianAxisAnchor(ENCartesianAxisDockZone.Bottom);
			NLinearScale primaryXScale = new NLinearScale();
			primaryXScale.Title.Text = "X Scale Title";
			primaryXScale.Labels.Style.AlwaysInsideScale = true;
			primaryXScale.Labels.Style.Angle = new NScaleLabelAngle(ENScaleLabelAngleMode.View, 90, false);
			primaryXAxis.Scale = primaryXScale;
			chart.Axes.Add(primaryXAxis);

			// Add a y linear axis
			NCartesianAxis primaryYAxis = new NCartesianAxis();
			primaryYAxis.Anchor = new NDockCartesianAxisAnchor(ENCartesianAxisDockZone.Left);
			NLinearScale primaryYScale = new NLinearScale();
			primaryYScale.Title.Text = "Y Scale Title";
			primaryYScale.Title.Angle = new NScaleLabelAngle(ENScaleLabelAngleMode.Scale, 0);
			primaryYScale.Labels.Style.AlwaysInsideScale = true;

			primaryYAxis.Scale = primaryYScale;
			chart.Axes.Add(primaryYAxis);

			// Create the x / y crossed axes
			NCartesianAxis secondaryXAxis = new NCartesianAxis();
			NLinearScale secondaryXScale = new NLinearScale();

			secondaryXScale.Labels.Visible = false;
			secondaryXAxis.Scale = secondaryXScale;
			chart.Axes.Add(secondaryXAxis);

			NCartesianAxis secondaryYAxis = new NCartesianAxis();
			NLinearScale secondaryYScale = new NLinearScale();
			secondaryYScale.Labels.Visible = false;
			secondaryYAxis.Scale = secondaryYScale;
			chart.Axes.Add(secondaryYAxis);

			// cross them
			NAxisCrossing secondaryXAxisCrossing = new NValueAxisCrossing(secondaryYAxis, 0);
			NCrossCartesianAxisAnchor secondaryXAxisAnchor = new NCrossCartesianAxisAnchor(ENCartesianAxisOrientation.Horizontal, ENScaleOrientation.Right, 0, 100);
			secondaryXAxisAnchor.YAxisCrossing = secondaryXAxisCrossing;

            secondaryXAxis.Anchor = secondaryXAxisAnchor;

            NAxisCrossing secondaryYAxisCrossing = new NValueAxisCrossing(secondaryXAxis, 0);
            NCrossCartesianAxisAnchor secondaryYAxisAnchor = new NCrossCartesianAxisAnchor(ENCartesianAxisOrientation.Vertical, ENScaleOrientation.Right, 0, 100);
			secondaryYAxisAnchor.XAxisCrossing = secondaryYAxisCrossing;

            secondaryYAxis.Anchor = secondaryYAxisAnchor;

			// add some dummy data
			NPointSeries point = new NPointSeries();
			chart.Series.Add(point);

			NDataLabelStyle dataLabelStyle = new NDataLabelStyle();
			dataLabelStyle.Visible = false;
			point.DataLabelStyle = dataLabelStyle;

			point.UseXValues = true;
			point.Size = 2;

			// add some random data in the range [-100, 100]
			Random rand = new Random();

			for (int i = 0; i < 3000; i++)
			{
				double x = rand.Next(190) - 95;
				double y = rand.Next(190) - 95;
				point.DataPoints.Add(new NPointDataPoint(x, y));
			}

			m_ChartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, ENChartPaletteTarget.Series));

			return chartViewWithCommandBars;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

			NCartesianChart chart = (NCartesianChart)m_ChartView.Surface.Charts[0];

			NComboBox chartFitMode = new NComboBox();
			chartFitMode.FillFromEnum<ENCartesianChartFitMode>();
			chartFitMode.SelectedIndex = (int)chart.FitMode; 
			chartFitMode.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnChartFitModeSelectedIndexChanged);
			stack.Add(NPairBox.Create("Fit Mode:", chartFitMode));

			m_ProportionXComboBox = CreateProportionComboBox();
			m_ProportionXComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnProportionComboBoxSelectedIndexChanged);
			stack.Add(NPairBox.Create("X:", m_ProportionXComboBox));

			m_ProportionYComboBox = CreateProportionComboBox();
			m_ProportionYComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnProportionComboBoxSelectedIndexChanged);
			stack.Add(NPairBox.Create("Y:", m_ProportionYComboBox));

			return boxGroup;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how change the chart aspect ratio in 2D.</p>";
		}

		#endregion

		#region Implementation

		private NComboBox CreateProportionComboBox()
		{
			NComboBox comboBox = new NComboBox();

			for (int i = 0; i < 5; i++)
			{
				comboBox.Items.Add(new NComboBoxItem((i + 1).ToString()));
			}

			comboBox.SelectedIndex = 0;

			return comboBox;
		}

		#endregion

		#region Event Handlers

		void OnChartFitModeSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			NCartesianChart chart = (NCartesianChart)m_ChartView.Surface.Charts[0];
			chart.FitMode = (ENCartesianChartFitMode)((NComboBox)arg.TargetNode).SelectedIndex;

			switch (chart.FitMode)
			{
				case ENCartesianChartFitMode.Aspect:
				case ENCartesianChartFitMode.AspectWithAxes:
					m_ProportionXComboBox.Enabled = true;
					m_ProportionYComboBox.Enabled = true;
					break;
				case ENCartesianChartFitMode.Stretch:
					m_ProportionXComboBox.Enabled = false;
					m_ProportionYComboBox.Enabled = false;
					break;
			}
		}

		void OnProportionComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			NCartesianChart chart = (NCartesianChart)m_ChartView.Surface.Charts[0];

			chart.Aspect = (double)(m_ProportionXComboBox.SelectedIndex + 1) / (double)(m_ProportionYComboBox.SelectedIndex + 1);
		}

		#endregion

		#region Fields

		private NChartView m_ChartView;
		private NComboBox m_ProportionXComboBox;
		private NComboBox m_ProportionYComboBox;
			
		#endregion

		#region Schema

		public static readonly NSchema NChartAspect2DExampleSchema;

		#endregion
	}
}