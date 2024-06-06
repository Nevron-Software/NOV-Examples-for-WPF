using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Standard Bar Example
	/// </summary>
	public class NStandard3DBarExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NStandard3DBarExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NStandard3DBarExample()
		{
			NStandard3DBarExampleSchema = NSchema.Create(typeof(NStandard3DBarExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// Set the title
			chartView.Surface.Titles[0].Text = "Standard 3D Bar";

			// Configure the chart
			NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];

			chart.Enable3D = true;
            chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.YLinearXZOrdinal);

            chart.Axes[ENCartesianAxis.Depth].Visible = true;
            chart.Projection.SetPredefinedProjection(ENPredefinedProjection.Perspective1);
            chart.LightModel.SetPredefinedLightModel(ENPredefinedLightModel.GlitterLeft);
			chart.Interactor = new NInteractor(new NTrackballTool());

            // Add an interlace stripe
            NLinearScale linearScale = chart.Axes[ENCartesianAxis.PrimaryY].Scale as NLinearScale;
			NScaleStrip strip = new NScaleStrip(new NColorFill(ENNamedColor.Beige), null, true, 0, 0, 1, 1);
            strip.Interlaced = true;
            linearScale.Strips.Add(strip);

			// Setup a bar series
			m_Bar = new NBarSeries();
			m_Bar.Name = "Bar Series";
			m_Bar.InflateMargins = true;
			m_Bar.UseXValues = false;

			m_Bar.Shadow = new NShadow(NColor.LightGray, 2, 2);

			// Add some data to the bar series
			m_Bar.LegendView.Mode = ENSeriesLegendMode.DataPoints;
			m_Bar.DataPoints.Add(new NBarDataPoint(18, "C++"));
			m_Bar.DataPoints.Add(new NBarDataPoint(15, "Ruby"));
			m_Bar.DataPoints.Add(new NBarDataPoint(21, "Python"));
			m_Bar.DataPoints.Add(new NBarDataPoint(23, "Java"));
			m_Bar.DataPoints.Add(new NBarDataPoint(27, "JavaScript"));
			m_Bar.DataPoints.Add(new NBarDataPoint(29, "C#"));
			m_Bar.DataPoints.Add(new NBarDataPoint(26, "PHP"));
			chart.Series.Add(m_Bar);

			// Configure the X axis to show the language names
			string[] labels = new string[m_Bar.DataPoints.Count];
			for (int i = 0; i < m_Bar.DataPoints.Count; i++)
			{
				labels[i] = m_Bar.DataPoints[i].Label;
			}

			NOrdinalScale xAxisScale = (NOrdinalScale)chart.Axes[ENCartesianAxis.PrimaryX].Scale;
			xAxisScale.Labels.TextProvider = new NOrdinalScaleLabelTextProvider(labels);

			return chartViewWithCommandBars;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

			{
				NComboBox originModeComboBox = new NComboBox();
				originModeComboBox.FillFromEnum<ENSeriesOriginMode>();
				originModeComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnOriginModeComboBoxSelectedIndexChanged);
				stack.Add(NPairBox.Create("Origin Mode: ", originModeComboBox));
			}

			{
				NNumericUpDown customOriginUpDown = new NNumericUpDown();
				customOriginUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnCustomOriginUpDownValueChanged);
				stack.Add(NPairBox.Create("Custom Origin: ", customOriginUpDown));
			}

			{
				NComboBox barShapeCompboBox = new NComboBox();
				barShapeCompboBox.FillFromEnum<ENBarShape>();
				barShapeCompboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnBarShapeComboBoxSelectedIndexChanged);
				stack.Add(NPairBox.Create("Bar Shape: ", barShapeCompboBox));
			}

			{
				NHScrollBar widthPercentScrollbar = new NHScrollBar();
				widthPercentScrollbar.Minimum = 0;
                widthPercentScrollbar.Maximum = 100;
                widthPercentScrollbar.ValueChanged += OnWidthPercentScrollbarValueChanged;
				widthPercentScrollbar.Value = 20;
                stack.Add(NPairBox.Create("Width Gap %: ", widthPercentScrollbar));
			}

            {
                NHScrollBar depthPercentScrollbar = new NHScrollBar();
                depthPercentScrollbar.Minimum = 0;
                depthPercentScrollbar.Maximum = 100;
                depthPercentScrollbar.ValueChanged += OnDepthPercentScrollbarValueChanged;
				depthPercentScrollbar.Value = 20;
                stack.Add(NPairBox.Create("Depth Gap %: ", depthPercentScrollbar));
            }

            return boxGroup;
		}
        protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a standard 3D bar chart.</p>";
		}

		#endregion

		#region Event Handlers

		void OnCustomOriginUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_Bar.CustomOrigin = (double)((NNumericUpDown)arg.TargetNode).Value;
		}

		void OnOriginModeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_Bar.OriginMode = (ENSeriesOriginMode)((NComboBox)arg.TargetNode).SelectedIndex;
		}

		void OnBarShapeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
            m_Bar.Shape = (ENBarShape)((NComboBox)arg.TargetNode).SelectedIndex;
        }

        private void OnWidthPercentScrollbarValueChanged(NValueChangeEventArgs arg)
        {
            m_Bar.WidthGapFactor = ((NHScrollBar)arg.TargetNode).Value / 100;
        }

        private void OnDepthPercentScrollbarValueChanged(NValueChangeEventArgs arg)
        {
            m_Bar.DepthGapFactor = ((NHScrollBar)arg.TargetNode).Value / 100;
        }

        #endregion

        #region Fields

        NBarSeries m_Bar;

		#endregion

		#region Schema

		public static readonly NSchema NStandard3DBarExampleSchema;

		#endregion
	}
}