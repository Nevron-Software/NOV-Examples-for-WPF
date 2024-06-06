using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// XY Scatter Line Example
	/// </summary>
	public class NXYZScatterLineExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NXYZScatterLineExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NXYZScatterLineExample()
		{
			NXYZScatterLineExampleSchema = NSchema.Create(typeof(NXYZScatterLineExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "XYZ Scatter Line";

			// configure chart
			NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];

            chart.Enable3D = true;
            chart.ModelWidth = 50;
            chart.ModelHeight = 50;
            chart.ModelDepth = 50;
            chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XYZLinear);
			chart.FitMode = ENCartesianChartFitMode.Aspect;
			chart.Projection.SetPredefinedProjection(ENPredefinedProjection.PerspectiveTilted);
			chart.LightModel.SetPredefinedLightModel(ENPredefinedLightModel.GlitterLeft);
            chart.Interactor = new NInteractor(new NTrackballTool());

            // add interlaced stripe to the Y axis
            NScaleStrip stripStyle = new NScaleStrip(new NColorFill(ENNamedColor.Beige), null, true, 0, 0, 1, 1);
			stripStyle.Interlaced = true;
			chart.Axes[ENCartesianAxis.PrimaryY].Scale.Strips.Add(stripStyle);

			m_Line = new NLineSeries();
			m_Line.UseXValues = true;
			m_Line.UseZValues = true;

            chart.Series.Add(m_Line);

			m_Line.DataLabelStyle = new NDataLabelStyle(false);
			m_Line.InflateMargins = true;

			m_Line.Name = "Line Series";
			m_Line.UseXValues = true;

            chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, ENChartPaletteTarget.Series));

            return chartViewWithCommandBars;
		}
		/// <summary>
		/// Called when windings / complexity changes
		/// </summary>
        private void ChangeData()
		{
			// add xy values
			float fSpringHeight = 20;

			int nWindings = (int)m_WindingsNumericUpDown.Value;
            int nComplexity = (int)m_ComplexityNumericUpDown.Value;

            double dCurrentAngle = 0;
            double dCurrentHeight = 0;
            double dX, dY, dZ;

            float fHeightStep = fSpringHeight / (nWindings * nComplexity);
            float fAngleStepRad = (float)(((360 / nComplexity) * 3.1415926535f) / 180.0f);

			m_Line.DataPoints.Clear();

            while (nWindings > 0)
            {
                for (int i = 0; i < nComplexity; i++)
                {
                    dZ = Math.Cos(dCurrentAngle) * (dCurrentHeight);
                    dX = Math.Sin(dCurrentAngle) * (dCurrentHeight);
                    dY = dCurrentHeight;

					m_Line.DataPoints.Add(new NLineDataPoint(dX, dY, dZ));

                    dCurrentAngle += fAngleStepRad;
                    dCurrentHeight += fHeightStep;
                }

                nWindings--;
            }
        }

        protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);
			
			m_WindingsNumericUpDown = new NNumericUpDown();
            m_WindingsNumericUpDown.ValueChanged += OnWindingsUpDownValueChanged;
            stack.Add(NPairBox.Create("Windings:", m_WindingsNumericUpDown));

            m_ComplexityNumericUpDown = new NNumericUpDown();
            m_ComplexityNumericUpDown.ValueChanged += OnComplexityUpDownValueChanged;
            stack.Add(NPairBox.Create("Complexity:", m_ComplexityNumericUpDown));

            m_ComplexityNumericUpDown.Value = 20;
            m_WindingsNumericUpDown.Value = 5;

            return group;
		}

        private void OnComplexityUpDownValueChanged(NValueChangeEventArgs arg)
        {
            ChangeData();
        }

        private void OnWindingsUpDownValueChanged(NValueChangeEventArgs arg)
        {
			ChangeData();
        }

        protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a xy scatter line chart.</p>";
		}

		#endregion

		#region Event Handlers

		void OnChangeXValuesButtonClick(NEventArgs arg)
		{
			Random random = new Random();
			m_Line.DataPoints[0].X = random.Next(10);

			for (int i = 1; i < m_Line.DataPoints.Count; i++)
			{
				m_Line.DataPoints[i].X = m_Line.DataPoints[i - 1].X + random.Next(1, 10);
			}
		}

		#endregion

		#region Fields

		NNumericUpDown m_WindingsNumericUpDown;
		NNumericUpDown m_ComplexityNumericUpDown;

        NLineSeries m_Line;

		#endregion

		#region Schema

		public static readonly NSchema NXYZScatterLineExampleSchema;

		#endregion
	}
}
