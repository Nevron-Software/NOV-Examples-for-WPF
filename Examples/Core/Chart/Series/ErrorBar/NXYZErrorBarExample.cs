using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Error Bar Example
	/// </summary>
	public class NXYZErrorBarExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NXYZErrorBarExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NXYZErrorBarExample()
		{
			NXYZErrorBarExampleSchema = NSchema.Create(typeof(NXYZErrorBarExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "XYZ Scatter Error Bar";

			// configure chart
			NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];

            chart.Enable3D = true;
            chart.Projection.SetPredefinedProjection(ENPredefinedProjection.PerspectiveTilted);
            chart.LightModel.SetPredefinedLightModel(ENPredefinedLightModel.GlitterLeft);
			chart.FitMode = ENCartesianChartFitMode.Aspect;
            chart.ModelDepth = 55.0f;
            chart.ModelWidth = 55.0f;
            chart.ModelHeight = 55.0f;

			chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XYZLinear);
            chart.Interactor = new NInteractor(new NTrackballTool());

            // setup the x axis
            NLinearScale scaleX = (NLinearScale)chart.Axes[ENCartesianAxis.PrimaryX].Scale;
            scaleX.ViewRangeInflateMode = ENScaleViewRangeInflateMode.None;
            scaleX.MajorGridLines.Stroke.DashStyle = ENDashStyle.Dot;
			scaleX.MajorGridLines.SetShowAtWall(ENChartWall.Back, true);
			scaleX.MajorGridLines.SetShowAtWall(ENChartWall.Bottom, true);

			// setup the y axis
            NLinearScale scaleY = (NLinearScale)chart.Axes[ENCartesianAxis.PrimaryY].Scale;
            scaleY.ViewRangeInflateMode = ENScaleViewRangeInflateMode.None;
            scaleY.MajorGridLines.Stroke.DashStyle = ENDashStyle.Dot;
			scaleY.MajorGridLines.SetShowAtWall(ENChartWall.Left, true);
			scaleY.MajorGridLines.SetShowAtWall(ENChartWall.Back, true);

			// add interlace stripe
			NScaleStrip strip = new NScaleStrip(new NColorFill(NColor.Beige), null, true, 0, 0, 1, 1);
			strip.Interlaced = true;
			strip.SetShowAtWall(ENChartWall.Back, true);
			strip.SetShowAtWall(ENChartWall.Left, true);
			scaleY.Strips.Add(strip);

			// setup the z axis
            NLinearScale scaleZ = (NLinearScale)chart.Axes[ENCartesianAxis.Depth].Scale;
			scaleZ.ViewRangeInflateMode = ENScaleViewRangeInflateMode.None;
            scaleZ.MajorGridLines.Stroke.DashStyle = ENDashStyle.Dot;
			scaleZ.MajorGridLines.SetShowAtWall(ENChartWall.Bottom, true);
			scaleZ.MajorGridLines.SetShowAtWall(ENChartWall.Left, true);

			// add an error bar series
			m_ErrorBar = new NErrorBarSeries();
            chart.Series.Add(m_ErrorBar);

            m_ErrorBar.Stroke = new NStroke(NColor.Black);
            m_ErrorBar.Fill = new NColorFill(NColor.DarkBlue);
			m_ErrorBar.Size = 10;

            m_ErrorBar.UseXValues = true;
            m_ErrorBar.UseZValues = true;

            GenerateNewData();
			
			// chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			return chartViewWithCommandBars;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);
			
			NCheckBox inflateMarginsCheckBox = new NCheckBox("Inflate Margins");
			inflateMarginsCheckBox.CheckedChanged += OnInflateMarginsCheckBoxCheckedChanged;
			inflateMarginsCheckBox.Checked = true;
			stack.Add(inflateMarginsCheckBox);

			NCheckBox showUpperXErrorCheckBox = new NCheckBox("Show Upper X Error");
			showUpperXErrorCheckBox.CheckedChanged += OnShowUpperXErrorCheckBoxCheckedChanged;
			showUpperXErrorCheckBox.Checked = true;
			stack.Add(showUpperXErrorCheckBox);

			NCheckBox showLowerXErrorCheckBox = new NCheckBox("Show Lower X Error");
			showLowerXErrorCheckBox.CheckedChanged += OnShowLowerXErrorCheckBoxCheckedChanged;
			showLowerXErrorCheckBox.Checked = true;
			stack.Add(showLowerXErrorCheckBox);

			NNumericUpDown xErrorSizeUpDown = new NNumericUpDown();
			xErrorSizeUpDown.Value = m_ErrorBar.XErrorSize;
			xErrorSizeUpDown.ValueChanged += OnXErrorSizeUpDownValueChanged;
			stack.Add(NPairBox.Create("X Error Size:", xErrorSizeUpDown));

			NCheckBox showUpperYErrorCheckBox = new NCheckBox("Show Upper Y Error");
			showUpperYErrorCheckBox.CheckedChanged += OnShowUpperYErrorCheckBoxCheckedChanged;
			showUpperYErrorCheckBox.Checked = true;
			stack.Add(showUpperYErrorCheckBox);

			NCheckBox showLowerYErrorCheckBox = new NCheckBox("Show Lower Y Error");
			showLowerYErrorCheckBox.CheckedChanged += OnShowLowerYErrorCheckBoxCheckedChanged;
			showLowerYErrorCheckBox.Checked = true;
			stack.Add(showLowerYErrorCheckBox);

			NNumericUpDown yErrorSizeUpDown = new NNumericUpDown();
			yErrorSizeUpDown.Value = m_ErrorBar.YErrorSize;
			yErrorSizeUpDown.ValueChanged += OnYErrorSizeUpDownValueChanged;
			stack.Add(NPairBox.Create("X Error Size:", yErrorSizeUpDown));

			//
			NCheckBox showUpperZErrorCheckBox = new NCheckBox("Show Upper Z Error");
			showUpperZErrorCheckBox.CheckedChanged += OnShowUpperZErrorCheckBoxCheckedChanged;
			showUpperZErrorCheckBox.Checked = true;
			stack.Add(showUpperZErrorCheckBox);

			NCheckBox showLowerZErrorCheckBox = new NCheckBox("Show Lower Z Error");
			showLowerZErrorCheckBox.CheckedChanged += OnShowLowerZErrorCheckBoxCheckedChanged;
			showLowerZErrorCheckBox.Checked = true;
			stack.Add(showLowerZErrorCheckBox);

			NNumericUpDown zErrorSizeUpDown = new NNumericUpDown();
			zErrorSizeUpDown.Value = m_ErrorBar.YErrorSize;
			zErrorSizeUpDown.ValueChanged += OnZErrorSizeUpDownValueChanged;
			stack.Add(NPairBox.Create("Z Error Size:", zErrorSizeUpDown));

            NButton changeDataButton = new NButton("Change Data");
			changeDataButton.Click += OnChangeDataButtonClick;
            stack.Add(changeDataButton);

            return group;
		}

        protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates a XYZ Error Bar chart. XYZ Errors bars indicate the uncertainty in the X, Y and Z values. You can use the controls to modify different properties of the error bar series like error whisker size, upper / lower error visibility etc.</p>";
		}

		#endregion

		#region Implementation

		private void GenerateNewData()
		{
			m_ErrorBar.DataPoints.Clear();

            Random random = new Random();

			for (int i = 0; i < 10; i++)
			{
				double y = 20 + random.NextDouble() * 30;
                double x = 20 + random.NextDouble() * 30;
                double z = 20 + random.NextDouble() * 30;

                double lowerXError = (2 + 2 * random.NextDouble());
                double upperXError = (2 + 2 * random.NextDouble());

                double lowerYError = (2 + 2 * random.NextDouble());
                double upperYError = (2 + 2 * random.NextDouble());

                double lowerZError = (2 + 2 * random.NextDouble());
                double upperZError = (2 + 2 * random.NextDouble());

                m_ErrorBar.DataPoints.Add(new NErrorBarDataPoint(x, y, z, upperXError, lowerXError, upperYError, lowerYError, upperZError, lowerZError));
			}
		}

        #endregion

        #region Event Handlers

        private void OnChangeDataButtonClick(NEventArgs arg)
        {
            GenerateNewData();
        }

        void OnInflateMarginsCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			m_ErrorBar.InflateMargins = ((NCheckBox)arg.TargetNode).Checked;
		}

		void OnXErrorSizeUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_ErrorBar.XErrorSize = ((NNumericUpDown)arg.TargetNode).Value;
		}

		void OnShowLowerXErrorCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			m_ErrorBar.ShowLowerXError = ((NCheckBox)arg.TargetNode).Checked;
		}

		void OnShowUpperXErrorCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			m_ErrorBar.ShowUpperXError = ((NCheckBox)arg.TargetNode).Checked;
		}

        void OnYErrorSizeUpDownValueChanged(NValueChangeEventArgs arg)
        {
            m_ErrorBar.YErrorSize = ((NNumericUpDown)arg.TargetNode).Value;
        }

        void OnShowLowerYErrorCheckBoxCheckedChanged(NValueChangeEventArgs arg)
        {
            m_ErrorBar.ShowLowerYError = ((NCheckBox)arg.TargetNode).Checked;
        }

        void OnShowUpperYErrorCheckBoxCheckedChanged(NValueChangeEventArgs arg)
        {
            m_ErrorBar.ShowUpperYError = ((NCheckBox)arg.TargetNode).Checked;
        }

        void OnZErrorSizeUpDownValueChanged(NValueChangeEventArgs arg)
        {
            m_ErrorBar.ZErrorSize = ((NNumericUpDown)arg.TargetNode).Value;
        }

        void OnShowLowerZErrorCheckBoxCheckedChanged(NValueChangeEventArgs arg)
        {
            m_ErrorBar.ShowLowerZError = ((NCheckBox)arg.TargetNode).Checked;
        }

        void OnShowUpperZErrorCheckBoxCheckedChanged(NValueChangeEventArgs arg)
        {
            m_ErrorBar.ShowUpperZError = ((NCheckBox)arg.TargetNode).Checked;
        }

        #endregion

        #region Fields

        NErrorBarSeries m_ErrorBar;

		#endregion

		#region Schema

		public static readonly NSchema NXYZErrorBarExampleSchema;

		#endregion
	}
}