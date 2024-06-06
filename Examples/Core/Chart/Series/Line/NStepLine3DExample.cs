using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Step Line 3D Example
	/// </summary>
	public class NStepLine3DExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NStepLine3DExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NStepLine3DExample()
		{
			NStepLine3DExampleSchema = NSchema.Create(typeof(NStepLine3DExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Step Line 3D";

            // configure chart
            m_Chart = (NCartesianChart)chartView.Surface.Charts[0];
            m_Chart.Enable3D = true;
            m_Chart.ModelWidth = 65;
            m_Chart.ModelHeight = 40;

            m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.Standard);
            m_Chart.Projection.SetPredefinedProjection(ENPredefinedProjection.Perspective2);
            m_Chart.LightModel.SetPredefinedLightModel(ENPredefinedLightModel.GlitterLeft);
            m_Chart.Interactor = new NInteractor(new NTrackballTool());

            // add interlaced stripe to the Y axis
            NScaleStrip strip = new NScaleStrip(new NColorFill(ENNamedColor.Beige), null, true, 0, 0, 1, 1);
			strip.Interlaced = true;
			m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale.Strips.Add(strip);

			m_Line = new NLineSeries();
			m_Line.Name = "Line Series";
			m_Line.InflateMargins = true;
			m_Line.DataLabelStyle = new NDataLabelStyle("<value>");
			m_Line.MarkerStyle = new NMarkerStyle(new NSize(4, 4));

			Random random = new Random();

			for (int i = 0; i < 8; i++)
			{
				m_Line.DataPoints.Add(new NLineDataPoint(random.Next(80) + 20));
			}

			m_Chart.Series.Add(m_Line);

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, ENChartPaletteTarget.Series));

			return chartViewWithCommandBars;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);
			
			NComboBox lineSegmentModeComboBox = new NComboBox();

			lineSegmentModeComboBox.Items.Add(new NComboBoxItem("HV Step Line"));
			lineSegmentModeComboBox.Items.Add(new NComboBoxItem("VH Step Line"));
			lineSegmentModeComboBox.Items.Add(new NComboBoxItem("HV Ascending VH Descending Step Line"));
			lineSegmentModeComboBox.Items.Add(new NComboBoxItem("VH Ascending HV Descending Step Line"));

			lineSegmentModeComboBox.SelectedIndexChanged += OnLineSegmentModeComboBoxSelectedIndexChanged;
			lineSegmentModeComboBox.SelectedIndex = 0;

			stack.Add(NPairBox.Create("Mode:", lineSegmentModeComboBox));

            NComboBox lineStyleCombo = new NComboBox();
            lineStyleCombo.FillFromEnum<ENLineSegmentShape>();
            lineStyleCombo.SelectedIndexChanged += OnLineStyleComboSelectedIndexChanged;
            lineStyleCombo.SelectedIndex = (int)ENLineSegmentShape.Tape;
            stack.Add(NPairBox.Create("Line Segment Shape:", lineStyleCombo));


            return group;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create step line charts.</p>";
		}

		#endregion

		#region Implementation

		void OnLineSegmentModeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			switch (((NComboBox)arg.TargetNode).SelectedIndex)
			{
				case 0:
					m_Line.LineSegmentMode = ENLineSegmentMode.HVStep;
					break;
				case 1:
					m_Line.LineSegmentMode = ENLineSegmentMode.VHStep;
					break;
				case 2:
					m_Line.LineSegmentMode = ENLineSegmentMode.HVAscentVHDescentStep;
					break;
				case 3:
					m_Line.LineSegmentMode = ENLineSegmentMode.VHAscentHVDescentStep;
					break;
			}
		}

        #endregion

        #region Event Handlers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        private void OnLineStyleComboSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            m_Line.LineSegmentShape = (ENLineSegmentShape)arg.NewValue;

            switch (m_Line.LineSegmentShape)
            {
                case ENLineSegmentShape.Line: // simple line
                    SetupTubeMarkers(m_Line.MarkerStyle);
                    break;

                case ENLineSegmentShape.Tape: // tape
                    SetupTapeMarkers(m_Line.MarkerStyle);
                    break;

                case ENLineSegmentShape.Tube: // tube
                    SetupTubeMarkers(m_Line.MarkerStyle);
                    break;
            }
        }
        private void SetupTapeMarkers(NMarkerStyle marker)
        {
            marker.Shape = ENPointShape3D.Cylinder;
            marker.AutoDepth = true;
			marker.AutoDepthPercent = 120;
        }

        private void SetupTubeMarkers(NMarkerStyle marker)
        {
            marker.Shape = ENPointShape3D.Sphere;
            marker.AutoDepth = false;
            marker.Width = 10;
            marker.Height = 10;
            marker.Depth = 10;
        }

        #endregion

        #region Fields

        NCartesianChart m_Chart;
		NLineSeries m_Line;

		#endregion

		#region Schema

		public static readonly NSchema NStepLine3DExampleSchema;

		#endregion
	}
}
