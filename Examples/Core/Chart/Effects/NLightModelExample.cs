using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Chart Light Model Example.
	/// </summary>
	public class NLightModelExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public NLightModelExample()
        {

        }
        /// <summary>
        /// Static constructor
        /// </summary>
        static NLightModelExample()
        {
            NLightModelExampleSchema = NSchema.Create(typeof(NLightModelExample), NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

            // configure title
            chartView.Surface.Titles[0].Text = "Lighting in 3D";

            // configure chart
            m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

            m_Chart.Enable3D = true;
            m_Chart.Projection.SetPredefinedProjection(ENPredefinedProjection.PerspectiveTilted);
            m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XYZLinear);
            m_Chart.Interactor = new NInteractor(new NTrackballTool());

            // setup chart
            m_Chart.ModelWidth = 50;
            m_Chart.ModelHeight = 50;
            m_Chart.ModelDepth = 50;

            // setup X axis
            NCartesianAxis axisX = m_Chart.Axes[ENCartesianAxis.PrimaryX];
            NLinearScale scaleX = new NLinearScale();
            scaleX.ViewRangeInflateMode = ENScaleViewRangeInflateMode.MajorTick;
            axisX.Scale = scaleX;

            // setup Y axis
            NCartesianAxis axisY = m_Chart.Axes[ENCartesianAxis.PrimaryY];
            NLinearScale scaleY = new NLinearScale();
            scaleY.MajorGridLines.ShowAtWalls = ENChartWall.NoneMask;
            scaleY.ViewRangeInflateMode = ENScaleViewRangeInflateMode.MajorTick;
            scaleY.MinTickDistance = 15;
			axisY.Scale = scaleY;

			// setup Z axis
			NCartesianAxis axisZ = m_Chart.Axes[ENCartesianAxis.Depth];
            NLinearScale scaleZ = new NLinearScale();
            scaleZ.ViewRangeInflateMode = ENScaleViewRangeInflateMode.MajorTick;
            axisZ.Scale = scaleZ;

            // create chart series
            CreateBoxes(m_Chart);

            chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, ENChartPaletteTarget.Series));

            return chartViewWithCommandBars;
        }
        private void CreateBoxes(NCartesianChart chart)
        {
            NRangeSeries rangeSeries = new NRangeSeries();
            chart.Series.Add(rangeSeries);
            rangeSeries.DataLabelStyle = new NDataLabelStyle(false);
            rangeSeries.Shape = ENBarShape.Rectangle;
            rangeSeries.InflateMargins = true;
            rangeSeries.UseXValues = true;
            rangeSeries.UseZValues = true;

            NColor color = NColor.FromRGB(147, 120, 197);
            rangeSeries.Fill = new NColorFill(color);
            rangeSeries.Stroke = new NStroke(1, color);

            Array barShapes = Enum.GetValues(typeof(ENBarShape));
            for (int i = 0; i < barShapes.Length; i++)
            {
                double size = i * 5 + 5;
                double center = i * 20 + 10;

                NRangeDataPoint dataPoint = new NRangeDataPoint(center - size, center - size, center - size, center + size, center + size, center + size);
                dataPoint.Shape = (ENBarShape)barShapes.GetValue(i);
                rangeSeries.DataPoints.Add(dataPoint);
            }
        }

        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();
            NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

            NRadioButtonGroup radioButtonGroup = new NRadioButtonGroup();
            radioButtonGroup.Content = boxGroup;

            // Custom light model group
            NRadioButton useCustomLightModelRadioButton = new NRadioButton("Use Custom Light Model");
            stack.Add(useCustomLightModelRadioButton);
            m_CustomLightModelComboBox = new NComboBox();
            m_CustomLightModelComboBox.FillFromArray(new string[] {
                                                                "Directional Light",
                                                                "Point Light",
                                                                "Point Light in Camera Space",
                                                                "Spot Light",
                                                                "Multiple Light Sources" });
            stack.Add(NPairBox.Create("Custom Light Model:", m_CustomLightModelComboBox));

            // predefined light model group
            NRadioButton usePredefinedLightModelRadioButton = new NRadioButton("Use Predefined Light Model");
            stack.Add(usePredefinedLightModelRadioButton);

            m_PredefinedLightModelComboBox = new NComboBox();
            m_PredefinedLightModelComboBox.FillFromEnum<ENPredefinedLightModel>();
            stack.Add(NPairBox.Create("Predefined Light Model:", m_PredefinedLightModelComboBox));

            m_CustomLightModelComboBox.SelectedIndexChanged += OnCustomLightModelComboBoxSelectedIndexChanged;
            m_PredefinedLightModelComboBox.SelectedIndexChanged += OnPredefinedLightModelComboBoxSelectedIndexChanged;
            useCustomLightModelRadioButton.CheckedChanged += UseCustomLightModelRadioButton_CheckedChanged;
            usePredefinedLightModelRadioButton.CheckedChanged += UsePredefinedLightModelRadioButton_CheckedChanged;

            useCustomLightModelRadioButton.Checked = true;
            OnCustomLightModelComboBoxSelectedIndexChanged(null);

            return radioButtonGroup;
        }

        protected override string GetExampleDescription()
        {
            return @"<p>This example demonstrates the chart lighting model. 
            The lighting model is a collection of light sources, each of which can emit light with different ambient, diffuse and specular color.
            There are three types of light sources:
</p>
            <ul>

            <li>Directional - directional light sources are treated as if they are located infinitely far away from the scene. 
            A directional light source has only a direction vector, but no location. 
            The effect of an infinite location is that the rays of light can be considered parallel by the time they reach an object. 
            An example of a real-world directional light source is the sun. Directional lights are rendered faster than point lights and spot lights.
            </li>

            <li>Point - point light sources have position but no direction, so they emit light equally in all directions. 
            Light intensity can attenuate with distance, so that objects located near a point light source get more illuminated than distant objects.
            </li>

            <li>Spot - spot light sources have both position and direction vectors. 
            They illuminate a part of the 3D scene that is enclosed by a cone. A real world example of a spot light is a desk lamp.
            </li>

            </ul>

<p>
            NOV Chart for .NET also features a large selection of predefined lighting sources, which are also demonstrated by this example.
            </p>";
        }

        #endregion

        #region Implementation

        private void ConfigureDirectionalLight()
        {
            NDirectionalLightSource light = new NDirectionalLightSource();
            
            light.CoordinateMode = ENLightSourceCoordinateMode.Model;
            light.Direction = new NVector3DF(-2, -4, -3);
            light.AmbientColor = NColor.FromRGB(60, 60, 60);
            light.DiffuseColor = NColor.FromRGB(230, 230, 230);
            light.SpecularColor = NColor.FromRGB(50, 50, 50);

            m_Chart.LightModel.LightSources.Clear();
            m_Chart.LightModel.LightSources.Add(light);
            m_Chart.LightModel.EnableLighting = true;
            m_Chart.LightModel.LocalViewpointLighting = true;
            m_Chart.LightModel.GlobalAmbientColor = NColor.FromRGB(60, 60, 60);
        }
        private void ConfigurePointLight()
        {
            NPointLightSource light = new NPointLightSource();

            light.CoordinateMode = ENLightSourceCoordinateMode.Model;
            light.Position = new NVector3DF(9, 36, 14);
            light.AmbientColor = NColor.FromRGB(100, 100, 100);
            light.DiffuseColor = NColor.FromRGB(210, 210, 210);
            light.SpecularColor = NColor.FromRGB(70, 70, 70);
            light.ConstantAttenuation = 0.6f;
            light.LinearAttenuation = 0.004f;
            light.QuadraticAttenuation = 0.0002f;

            m_Chart.LightModel.LightSources.Clear();
            m_Chart.LightModel.LightSources.Add(light);
            m_Chart.LightModel.EnableLighting = true;
            m_Chart.LightModel.LocalViewpointLighting = true;
            m_Chart.LightModel.GlobalAmbientColor = NColor.FromRGB(60, 60, 60);
        }
        private void ConfigurePointLightInCameraSpace()
        {
            NPointLightSource light = new NPointLightSource();

            light.CoordinateMode = ENLightSourceCoordinateMode.Camera;
            light.Position = new NVector3DF(0, 0, 0);
            light.AmbientColor = NColor.FromRGB(100, 100, 100);
            light.DiffuseColor = NColor.FromRGB(210, 210, 210);
            light.SpecularColor = NColor.FromRGB(90, 90, 90);
            light.ConstantAttenuation = 0.3f;
            light.LinearAttenuation = 0.0003f;
            light.QuadraticAttenuation = 0.00003f;

            m_Chart.LightModel.LightSources.Clear();
            m_Chart.LightModel.LightSources.Add(light);
            m_Chart.LightModel.EnableLighting = true;
            m_Chart.LightModel.LocalViewpointLighting = true;
            m_Chart.LightModel.GlobalAmbientColor = NColor.FromRGB(60, 60, 60);
        }
        private void ConfigureMultipleLightSources()
        {
            NPointLightSource light1 = new NPointLightSource();
            light1.CoordinateMode = ENLightSourceCoordinateMode.Model;
            light1.Position = new NVector3DF(0, 36, 16);
            light1.AmbientColor = NColor.FromRGB(60, 0, 0);
            light1.DiffuseColor = NColor.FromRGB(110, 20, 20);
            light1.SpecularColor = NColor.FromRGB(80, 60, 60);
            light1.ConstantAttenuation = 0.6f;
            light1.LinearAttenuation = 0.004f;
            light1.QuadraticAttenuation = 0.0002f;

            NPointLightSource light2 = new NPointLightSource();
            light2.CoordinateMode = ENLightSourceCoordinateMode.Model;
            light2.Position = new NVector3DF(13.85f, 36, -8);
            light2.AmbientColor = NColor.FromRGB(0, 60, 0);
            light2.DiffuseColor = NColor.FromRGB(20, 110, 20);
            light2.SpecularColor = NColor.FromRGB(60, 80, 60);
            light2.ConstantAttenuation = 0.6f;
            light2.LinearAttenuation = 0.004f;
            light2.QuadraticAttenuation = 0.0002f;

            NPointLightSource light3 = new NPointLightSource();
            light3.CoordinateMode = ENLightSourceCoordinateMode.Model;
            light3.Position = new NVector3DF(-13.85f, 36, -8);
            light3.AmbientColor = NColor.FromRGB(0, 0, 60);
            light3.DiffuseColor = NColor.FromRGB(20, 20, 110);
            light3.SpecularColor = NColor.FromRGB(60, 60, 80);
            light3.ConstantAttenuation = 0.6f;
            light3.LinearAttenuation = 0.004f;
            light3.QuadraticAttenuation = 0.0002f;

            NPointLightSource light4 = new NPointLightSource();
            light4.CoordinateMode = ENLightSourceCoordinateMode.Camera;
            light4.Position = new NVector3DF(0, 0, 0);
            light4.AmbientColor = NColor.FromRGB(0, 0, 0);
            light4.DiffuseColor = NColor.FromRGB(90, 90, 90);
            light4.SpecularColor = NColor.FromRGB(80, 80, 80);
            light4.ConstantAttenuation = 0.9f;
            light4.LinearAttenuation = 0.0004f;
            light4.QuadraticAttenuation = 0.0f;

            m_Chart.LightModel.LightSources.Clear();
            m_Chart.LightModel.LightSources.Add(light1);
            m_Chart.LightModel.LightSources.Add(light2);
            m_Chart.LightModel.LightSources.Add(light3);
            m_Chart.LightModel.LightSources.Add(light4);
            m_Chart.LightModel.EnableLighting = true;
            m_Chart.LightModel.LocalViewpointLighting = true;
            m_Chart.LightModel.GlobalAmbientColor = NColor.FromRGB(60, 60, 60);
        }
        private void SpotLight(NChart chart)
        {
            NSpotLightSource light = new NSpotLightSource();

            light.CoordinateMode = ENLightSourceCoordinateMode.Model;
            light.Position = new NVector3DF(14, 30, 14);
            light.Direction = new NVector3DF(-0.5f, -1, -0.4f);
            light.AmbientColor = NColor.FromRGB(50, 50, 50);
            light.DiffuseColor = NColor.FromRGB(180, 180, 210);
            light.SpecularColor = NColor.FromRGB(80, 80, 80);
            light.ConstantAttenuation = 0.3f;
            light.LinearAttenuation = 0.001f;
            light.QuadraticAttenuation = 0.0001f;
            light.SpotCutoff = 45;
            light.SpotExponent = 15;

            chart.LightModel.LightSources.Clear();
            chart.LightModel.LightSources.Add(light);
            chart.LightModel.EnableLighting = true;
            chart.LightModel.LocalViewpointLighting = true;
            chart.LightModel.GlobalAmbientColor = NColor.FromRGB(100, 100, 100);
        }

        #endregion

        #region Event Handlers

        private void OnCustomLightModelComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            switch (m_CustomLightModelComboBox.SelectedIndex)
            {
                case 0:
                    ConfigureDirectionalLight();
                    break;

                case 1:
                    ConfigurePointLight();
                    break;

                case 2:
                    ConfigurePointLightInCameraSpace();
                    break;

                case 3:
                    SpotLight(m_Chart);
                    break;

                case 4:
                    ConfigureMultipleLightSources();
                    break;
            }
        }
        private void OnPredefinedLightModelComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            ENPredefinedLightModel lm = (ENPredefinedLightModel)m_PredefinedLightModelComboBox.SelectedIndex;

            m_Chart.LightModel.SetPredefinedLightModel(lm);
        }

        private void UsePredefinedLightModelRadioButton_CheckedChanged(NValueChangeEventArgs arg)
        {
            m_CustomLightModelComboBox.Enabled = false;
            m_PredefinedLightModelComboBox.Enabled = true;

            OnPredefinedLightModelComboBoxSelectedIndexChanged(null);
        }

        private void UseCustomLightModelRadioButton_CheckedChanged(NValueChangeEventArgs arg)
        {
            m_CustomLightModelComboBox.Enabled = true;
            m_PredefinedLightModelComboBox.Enabled = false;

            OnPredefinedLightModelComboBoxSelectedIndexChanged(null);
        }

        #endregion

        #region Fields

        private NCartesianChart m_Chart;
        private NComboBox m_CustomLightModelComboBox;
        private NComboBox m_PredefinedLightModelComboBox;

        #endregion

        #region Schema

        public static readonly NSchema NLightModelExampleSchema;

        #endregion
    }
}