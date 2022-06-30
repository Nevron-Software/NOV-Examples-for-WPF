using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Axis model crossing example
	/// </summary>
	public class NAxisModelCrossingExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NAxisModelCrossingExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NAxisModelCrossingExample()
		{
			NAxisModelCrossingExampleSchema = NSchema.Create(typeof(NAxisModelCrossingExample), NExampleBaseSchema);
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

			// configure title
			chartView.Surface.Titles[0].Text = "Axis Model Crossing";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];
			m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XYLinear);

			NCartesianAxis primaryX = m_Chart.Axes[ENCartesianAxis.PrimaryX];
			NCartesianAxis primaryY = m_Chart.Axes[ENCartesianAxis.PrimaryY];

			// configure axes
			NLinearScale yScale = (NLinearScale)primaryY.Scale;
			yScale.MajorGridLines = CreateDottedGrid();

			NScaleStrip yStrip = new NScaleStrip(new NColorFill(new NColor(NColor.LightGray, 40)), null, true, 0, 0, 1, 1);
			yStrip.Interlaced = true;
			yScale.Strips.Add(yStrip);

			NLinearScale xScale = (NLinearScale)primaryX.Scale;
			xScale.MajorGridLines = CreateDottedGrid();

			NScaleStrip xStrip = new NScaleStrip(new NColorFill(new NColor(NColor.LightGray, 40)), null, true, 0, 0, 1, 1);
			xStrip.Interlaced = true;
			xScale.Strips.Add(xStrip);

			// cross X and Y axes
			primaryX.Anchor = new NModelCrossCartesianAxisAnchor(0, ENAxisCrossAlignment.Center, primaryY, ENCartesianAxisOrientation.Horizontal, ENScaleOrientation.Right, 0.0f, 100.0f);

			primaryY.Anchor = new NModelCrossCartesianAxisAnchor(0, ENAxisCrossAlignment.Center, primaryX, ENCartesianAxisOrientation.Vertical, ENScaleOrientation.Left, 0.0f, 100.0f);

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

			NComboBox verticalAxisAlignmentComboBox = new NComboBox();
			verticalAxisAlignmentComboBox.FillFromEnum<ENAxisCrossAlignment>();
			verticalAxisAlignmentComboBox.SelectedIndexChanged +=new Function<NValueChangeEventArgs>(OnVerticalAxisAlignmentComboBoxSelectedIndexChanged);
			stack.Add(NPairBox.Create("Alignment:", verticalAxisAlignmentComboBox));
			verticalAxisAlignmentComboBox.SelectedIndex = (int)ENAxisCrossAlignment.Center;

			NNumericUpDown verticalAxisOffsetUpDown = new NNumericUpDown();
			verticalAxisOffsetUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnVerticalAxisOffsetUpDownValueChanged);
			stack.Add(NPairBox.Create("Offset:", verticalAxisOffsetUpDown));

			stack.Add(new NLabel("Horizontal Axis"));
			NComboBox horizontalAxisAlignmentComboBox = new NComboBox();
			horizontalAxisAlignmentComboBox.FillFromEnum<ENAxisCrossAlignment>();
			horizontalAxisAlignmentComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnHorizontalAxisAlignmentComboBoxSelectedIndexChanged);
			stack.Add(NPairBox.Create("Alignment:", horizontalAxisAlignmentComboBox));
			horizontalAxisAlignmentComboBox.SelectedIndex = (int)ENAxisCrossAlignment.Center;

			NNumericUpDown horizontalAxisOffsetUpDown = new NNumericUpDown();
			horizontalAxisOffsetUpDown.ValueChanged +=new Function<NValueChangeEventArgs>(OnHorizontalAxisOffsetUpDownValueChanged);
			stack.Add(NPairBox.Create("Offset:", horizontalAxisOffsetUpDown));

			return boxGroup;
		}

		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to cross two axes at a specified model offset.</p>";
		}

		#endregion

		#region Event Handlers

		void OnHorizontalAxisOffsetUpDownValueChanged(NValueChangeEventArgs arg)
		{
			NModelCrossCartesianAxisAnchor anchor = m_Chart.Axes[ENCartesianAxis.PrimaryX].Anchor as NModelCrossCartesianAxisAnchor;
			anchor.Offset = ((NNumericUpDown)arg.TargetNode).Value;
		}

		void OnVerticalAxisOffsetUpDownValueChanged(NValueChangeEventArgs arg)
		{
			NModelCrossCartesianAxisAnchor anchor = m_Chart.Axes[ENCartesianAxis.PrimaryY].Anchor as NModelCrossCartesianAxisAnchor;
			anchor.Offset = ((NNumericUpDown)arg.TargetNode).Value;
		}

		void OnHorizontalAxisAlignmentComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			NModelCrossCartesianAxisAnchor anchor = m_Chart.Axes[ENCartesianAxis.PrimaryX].Anchor as NModelCrossCartesianAxisAnchor;
			anchor.Alignment = (ENAxisCrossAlignment)((NComboBox)arg.TargetNode).SelectedIndex;
		}

		void OnVerticalAxisAlignmentComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			NModelCrossCartesianAxisAnchor anchor = m_Chart.Axes[ENCartesianAxis.PrimaryY].Anchor as NModelCrossCartesianAxisAnchor;
			anchor.Alignment = (ENAxisCrossAlignment)((NComboBox)arg.TargetNode).SelectedIndex;
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

		#region Fields

		NCartesianChart m_Chart;

		#endregion

		#region Schema

		public static readonly NSchema NAxisModelCrossingExampleSchema;

		#endregion
	}
}
