using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Axis value crossing example
	/// </summary>
	public class NAxisValueCrossingExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NAxisValueCrossingExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NAxisValueCrossingExample()
		{
			NAxisValueCrossingExampleSchema = NSchema.Create(typeof(NAxisValueCrossingExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			chartView.Surface.Titles[0].Text = "Axis Value Crossing";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];
			m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XYLinear);

			NCartesianAxis primaryX = m_Chart.Axes[ENCartesianAxis.PrimaryX];
			NCartesianAxis primaryY = m_Chart.Axes[ENCartesianAxis.PrimaryY];

			// configure axes
			NLinearScale scaleY = (NLinearScale)primaryY.Scale;

			// configure scales
			NLinearScale yScale = (NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale;
			yScale.MajorGridLines = CreateDottedGrid();

			NScaleStrip yStrip = new NScaleStrip(new NColorFill(new NColor(NColor.LightGray, 40)), null, true, 0, 0, 1, 1);
			yStrip.Interlaced = true;
			yScale.Strips.Add(yStrip);

			NLinearScale xScale = (NLinearScale)primaryX.Scale;
			xScale.MajorGridLines = CreateDottedGrid();

			NScaleStrip xStrip = new NScaleStrip(new NColorFill(new NColor(NColor.LightGray, 40)), null, true, 0, 0, 1, 1);
			xStrip.Interlaced = true;
			xScale.Strips.Add(xStrip);

			// cross X and Y axes at their 0 values
			primaryX.Anchor = new NValueCrossCartesianAxisAnchor(0, primaryY, ENCartesianAxisOrientation.Horizontal, ENScaleOrientation.Right, 0.0f, 100.0f);

			primaryY.Anchor = new NValueCrossCartesianAxisAnchor(0, primaryX, ENCartesianAxisOrientation.Vertical, ENScaleOrientation.Left, 0.0f, 100.0f);

			// setup bubble series
			NBubbleSeries bubble = new NBubbleSeries();
			bubble.Name = "Bubble Series";
			bubble.InflateMargins = true;
			bubble.DataLabelStyle = new NDataLabelStyle(false);
			bubble.UseXValues = true;

			// fill with random data
			Random random = new Random();

			for (int i = 0; i < 10; i++)
			{
				bubble.DataPoints.Add(new NBubbleDataPoint(random.Next(-20, 20), random.Next(-20, 20), random.Next(1, 6)));
			}

			m_Chart.Series.Add(bubble);

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, true));

			return chartView;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

			stack.Add(new NLabel("Vertical Axis"));
			NCheckBox verticalAxisUsePositionCheckBox = new NCheckBox("Use Position");
			verticalAxisUsePositionCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnVerticalAxisUsePositionCheckBoxCheckedChanged);
			stack.Add(verticalAxisUsePositionCheckBox);

			m_VerticalAxisPositionValueUpDown = new NNumericUpDown();
			m_VerticalAxisPositionValueUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnVerticalAxisPositionValueUpDownValueChanged);
			stack.Add(NPairBox.Create("Position Value:", m_VerticalAxisPositionValueUpDown));

			stack.Add(new NLabel("Horizontal Axis"));
			NCheckBox horizontalAxisUsePositionCheckBox = new NCheckBox("Use Position");
			horizontalAxisUsePositionCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnHorizontalAxisUsePositionCheckBoxCheckedChanged);
			stack.Add(horizontalAxisUsePositionCheckBox);

			m_HorizontalAxisPositionValueUpDown = new NNumericUpDown();
			m_HorizontalAxisPositionValueUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnHorizontalAxisPositionValueUpDownValueChanged);
			stack.Add(NPairBox.Create("Position Value:", m_HorizontalAxisPositionValueUpDown));

			verticalAxisUsePositionCheckBox.Checked = true;
			horizontalAxisUsePositionCheckBox.Checked = true;

			return boxGroup;
		}

		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to cross two axes at a specified value.</p>";
		}

		#endregion

		#region Implementation

		private NScaleGridLines CreateDottedGrid()
		{
			NScaleGridLines scaleGrid = new NScaleGridLines();

			scaleGrid.Visible = true;
			scaleGrid.Stroke.Width = 1;
			scaleGrid.Stroke.DashStyle = ENDashStyle.Dot;
			scaleGrid.Stroke.Color = NColor.Gray;

			return scaleGrid;

		}

		#endregion

		#region Event Handlers

		void OnHorizontalAxisUsePositionCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			bool usePosition = ((NCheckBox)arg.TargetNode).Checked;
			m_HorizontalAxisPositionValueUpDown.Enabled = usePosition;

			if (usePosition)
			{
				double posValue = m_HorizontalAxisPositionValueUpDown.Value;

				m_Chart.Axes[ENCartesianAxis.PrimaryX].Anchor = new NValueCrossCartesianAxisAnchor(posValue, m_Chart.Axes[ENCartesianAxis.PrimaryY], ENCartesianAxisOrientation.Horizontal, ENScaleOrientation.Right, 0.0f, 100.0f);
			}
			else
			{
				m_Chart.Axes[ENCartesianAxis.PrimaryX].Anchor = new NDockCartesianAxisAnchor(ENCartesianAxisDockZone.Bottom, true);
			}
		}

		void OnHorizontalAxisPositionValueUpDownValueChanged(NValueChangeEventArgs arg)
		{
			NValueCrossCartesianAxisAnchor crossAnchor = m_Chart.Axes[ENCartesianAxis.PrimaryX].Anchor as NValueCrossCartesianAxisAnchor;

			if (crossAnchor != null)
			{
				crossAnchor.Value = ((NNumericUpDown)arg.TargetNode).Value;
			}
		}

		void OnVerticalAxisPositionValueUpDownValueChanged(NValueChangeEventArgs arg)
		{
			NValueCrossCartesianAxisAnchor crossAnchor = m_Chart.Axes[ENCartesianAxis.PrimaryY].Anchor as NValueCrossCartesianAxisAnchor;

			if (crossAnchor != null)
			{
				crossAnchor.Value = ((NNumericUpDown)arg.TargetNode).Value;
			}
		}

		void OnVerticalAxisUsePositionCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			bool usePosition = ((NCheckBox)arg.TargetNode).Checked;
			m_VerticalAxisPositionValueUpDown.Enabled = usePosition;

			if (usePosition)
			{
				double posValue = m_VerticalAxisPositionValueUpDown.Value;

				m_Chart.Axes[ENCartesianAxis.PrimaryY].Anchor = new NValueCrossCartesianAxisAnchor(posValue, m_Chart.Axes[ENCartesianAxis.PrimaryX], ENCartesianAxisOrientation.Vertical, ENScaleOrientation.Left, 0.0f, 100.0f);
			}
			else
			{
				m_Chart.Axes[ENCartesianAxis.PrimaryY].Anchor = new NDockCartesianAxisAnchor(ENCartesianAxisDockZone.Left, true);
			}
		}

		#endregion

		#region Fields

		NCartesianChart m_Chart;

		NNumericUpDown m_HorizontalAxisPositionValueUpDown;
		NNumericUpDown m_VerticalAxisPositionValueUpDown;

		#endregion

		#region Schema

		public static readonly NSchema NAxisValueCrossingExampleSchema;

		#endregion
	}
}
