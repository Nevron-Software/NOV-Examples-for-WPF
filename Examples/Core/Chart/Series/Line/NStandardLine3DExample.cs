using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Examples.Framework;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;
using System;

namespace Nevron.Nov.Examples.Chart
{
    /// <summary>
    /// Standard 3D Line Example
    /// </summary>
    public class NStandardLine3DExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NStandardLine3DExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NStandardLine3DExample()
		{
			NStandardLine3DExampleSchema = NSchema.Create(typeof(NStandardLine3DExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Standard Line 3D";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

            m_Chart.Enable3D = true;
            m_Chart.ModelWidth = 65;
            m_Chart.ModelHeight = 40;
            
            m_Chart.Projection.SetPredefinedProjection(ENPredefinedProjection.Perspective2);
            m_Chart.LightModel.SetPredefinedLightModel(ENPredefinedLightModel.GlitterLeft);

			// configure axes
            m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.Standard);
            m_Chart.Axes[ENCartesianAxis.Depth].Visible = false;
            m_Chart.Interactor = new NInteractor(new NTrackballTool());

            // add interlaced stripe to the Y axis
            NScaleStrip strip = new NScaleStrip(new NColorFill(ENNamedColor.Beige), null, true, 0, 0, 1, 1);
			strip.Interlaced = true;
			strip.SetShowAtWall(ENChartWall.Back, true);
            strip.SetShowAtWall(ENChartWall.Left, true);
            m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale.Strips.Add(strip);

			m_Line = new NLineSeries();
            m_Chart.Series.Add(m_Line);

            m_Line.Name = "Line Series";
			m_Line.InflateMargins = true;
			m_Line.DataLabelStyle = new NDataLabelStyle("<value>");
			m_Line.MarkerStyle = new NMarkerStyle(new NSize(4, 4));

            Random random = new Random();

			for (int i = 0; i < 8; i++)
			{
				m_Line.DataPoints.Add(new NLineDataPoint(random.Next(80) + 20));
			}

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, ENChartPaletteTarget.Series));

			return chartViewWithCommandBars;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);

            NComboBox lineStyleCombo = new NComboBox();
            lineStyleCombo.FillFromEnum<ENLineSegmentShape>();
            lineStyleCombo.SelectedIndexChanged += OnLineStyleComboSelectedIndexChanged;
            stack.Add(NPairBox.Create("Line Segment Shape:", lineStyleCombo));

            m_LineStrokeButton = new NButton("Line Stroke...");
            m_LineStrokeButton.Click += OnLineStrokeButtonClick;
            stack.Add(m_LineStrokeButton);

            m_LineFillButton = new NButton("Line Fill...");
            m_LineFillButton.Click += OnLineFillButtonClick;
            stack.Add(m_LineFillButton);

            m_LineDepthScrollBar = new NHScrollBar();
            m_LineDepthScrollBar.Minimum = 0;
            m_LineDepthScrollBar.Maximum = 100;
            m_LineDepthScrollBar.ValueChanged += OnLineDepthScrollBarValueChanged;
            m_LineDepthScrollBar.Value = 50;
            stack.Add(NPairBox.Create("Line Depth:", m_LineDepthScrollBar));

            m_LineSizeScrollBar = new NHScrollBar();
            m_LineSizeScrollBar.Minimum = 0;
            m_LineSizeScrollBar.Maximum = 5.0f;
            m_LineSizeScrollBar.SmallChange = 0.1f;
            m_LineSizeScrollBar.LargeChange = 0.2f;
            m_LineSizeScrollBar.ValueChanged += OnLineSizeScrollBarValueChanged;
            m_LineSizeScrollBar.Value = 2.0f;
            stack.Add(NPairBox.Create("Line Size:", m_LineSizeScrollBar));

            lineStyleCombo.SelectedIndex = (int)ENLineSegmentShape.Tape;

            return group;
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        private void OnLineDepthScrollBarValueChanged(NValueChangeEventArgs arg)
        {
            m_Line.DepthPercent = System.Convert.ToSingle(arg.NewValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        private void OnLineSizeScrollBarValueChanged(NValueChangeEventArgs arg)
        {
            m_Line.LineSize = System.Convert.ToSingle(arg.NewValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        private void OnLineFillButtonClick(NEventArgs arg)
        {
            try
            {
                NNode node = m_Line.Fill;
                NEditorWindow editorWindow = NEditorWindow.CreateForInstance(
                    node,
                    null,
                    this.DisplayWindow,
                    null);

                if (node is NStyleNodeCollectionTree)
                {
                    editorWindow.PreferredSize = new NSize(500, 360);
                }

                editorWindow.Open();
            }
            catch (Exception ex)
            {
                NTrace.WriteException("OnShowDesignerClick failed.", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        private void OnLineStrokeButtonClick(NEventArgs arg)
        {
            try
            {
                NNode node = m_Line.Stroke;
                NEditorWindow editorWindow = NEditorWindow.CreateForInstance(
                    node,
                    null,
                    this.DisplayWindow,
                    null);

                if (node is NStyleNodeCollectionTree)
                {
                    editorWindow.PreferredSize = new NSize(500, 360);
                }

                editorWindow.Open();
            }
            catch (Exception ex)
            {
                NTrace.WriteException("OnShowDesignerClick failed.", ex);
            }
        }
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
                    m_LineDepthScrollBar.Enabled = false;
                    m_LineSizeScrollBar.Enabled = false;
                    m_LineFillButton.Enabled = false;
                    SetupTubeMarkers(m_Line.MarkerStyle);
                    break;

                case ENLineSegmentShape.Tape: // tape
                    m_LineDepthScrollBar.Enabled = true;
                    m_LineSizeScrollBar.Enabled = false;
                    m_LineFillButton.Enabled = true;
                    SetupTapeMarkers(m_Line.MarkerStyle);
                    break;

                case ENLineSegmentShape.Tube: // tube
                    m_LineDepthScrollBar.Enabled = false;
                    m_LineSizeScrollBar.Enabled = true;
                    m_LineFillButton.Enabled = true;
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

        protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a standard 3D line chart.</p>";
		}

        #endregion

        #region Fields

        NCartesianChart m_Chart;
		NLineSeries m_Line;

        NButton m_LineStrokeButton;
        NButton m_LineFillButton;
        NHScrollBar m_LineDepthScrollBar;
        NHScrollBar m_LineSizeScrollBar;

        #endregion

        #region Schema

        public static readonly NSchema NStandardLine3DExampleSchema;

		#endregion
	}
}
