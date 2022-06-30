using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Gauge
{
	/// <summary>
	/// This example demonstrates how to add indicators to a linear gauge 
	/// </summary>
	public class NLinearGaugeIndicatorsExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public NLinearGaugeIndicatorsExample()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        static NLinearGaugeIndicatorsExample()
        {
			NLinearGaugeIndicatorsExampleSchema = NSchema.Create(typeof(NLinearGaugeIndicatorsExample), NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
		{
			NStackPanel stack = new NStackPanel();
			stack.HorizontalPlacement = ENHorizontalPlacement.Left;

			// create a linear gauge
			m_LinearGauge = new NLinearGauge();
			stack.Add(m_LinearGauge);
			m_LinearGauge.CapEffect = new NGelCapEffect();
			m_LinearGauge.Border = CreateBorder();
			m_LinearGauge.Padding = new NMargins(20);
			m_LinearGauge.BorderThickness = new NMargins(6);
			m_LinearGauge.BackgroundFill = new NStockGradientFill(NColor.Gray, NColor.Black);
            m_LinearGauge.PreferredSize = new NSize(400, 150);

			m_LinearGauge.Axes.Clear();

			NRange celsiusRange = new NRange(-40.0, 60.0);

			// add celsius and farenheit axes
			NGaugeAxis celsiusAxis = new NGaugeAxis();
			celsiusAxis.Range = celsiusRange;
			celsiusAxis.Anchor = new NModelGaugeAxisAnchor(-5, ENVerticalAlignment.Center, ENScaleOrientation.Left, 0.0f, 100.0f);
			m_LinearGauge.Axes.Add(celsiusAxis);

			NGaugeAxis farenheitAxis = new NGaugeAxis();
			farenheitAxis.Range = new NRange(CelsiusToFarenheit(celsiusRange.Begin), CelsiusToFarenheit(celsiusRange.End));
			farenheitAxis.Anchor = new NModelGaugeAxisAnchor(5, ENVerticalAlignment.Center, ENScaleOrientation.Right, 0.0f, 100.0f);
			m_LinearGauge.Axes.Add(farenheitAxis);

			// configure the scales
			NLinearScale celsiusScale = (NLinearScale)celsiusAxis.Scale;
			ConfigureScale(celsiusScale, "°C");

			celsiusScale.Sections.Add(CreateSection(NColor.Red, NColor.Red, new NRange(40, 60)));
			celsiusScale.Sections.Add(CreateSection(NColor.Blue, NColor.SkyBlue, new NRange(-40, -20)));

			NLinearScale farenheitScale = (NLinearScale)farenheitAxis.Scale;
			ConfigureScale(farenheitScale, "°F");

			farenheitScale.Sections.Add(CreateSection(NColor.Red, NColor.Red, new NRange(CelsiusToFarenheit(40), CelsiusToFarenheit(60))));
			farenheitScale.Sections.Add(CreateSection(NColor.Blue, NColor.SkyBlue, new NRange(CelsiusToFarenheit(-40), CelsiusToFarenheit(-20))));

			// now add two indicators
			m_Indicator1 = new NRangeIndicator();
			m_Indicator1.Value = 10;
			m_Indicator1.Stroke.Color = NColor.DarkBlue;
			m_Indicator1.Fill = new NStockGradientFill(ENGradientStyle.Vertical, ENGradientVariant.Variant1, NColor.LightBlue, NColor.Blue);

			m_Indicator1.BeginWidth = 10;
			m_Indicator1.EndWidth = 10;
			m_LinearGauge.Indicators.Add(m_Indicator1);

			m_Indicator2 = new NMarkerValueIndicator();
			m_Indicator2.Value = 33;
//			m_Indicator2.ShapFillStyle = new NStockGradientFill(ENGradientStyle.Horizontal, ENGradientVariant.Variant1, NColor.White, NColor.Red);
//			m_Indicator2.Shape.StrokeStyle.Color = Color.DarkRed;
			m_LinearGauge.Indicators.Add(m_Indicator2);

			return stack;
		}
        protected override NWidget CreateExampleControls()
        {
			NStackPanel stack = new NStackPanel();

			NStackPanel propertyStack = new NStackPanel();
			stack.Add(new NUniSizeBoxGroup(propertyStack));

			m_RangeIndicatorValueUpDown = new NNumericUpDown();
			propertyStack.Add(new NPairBox("Range Indicator Value:", m_RangeIndicatorValueUpDown, true));
			m_RangeIndicatorValueUpDown.Value = m_Indicator1.Value;
			m_RangeIndicatorValueUpDown.ValueChanged +=new Function<NValueChangeEventArgs>(OnRangeIndicatorValueUpDownValueChanged);

			m_RangeIndicatorOriginModeComboBox = new NComboBox();
			propertyStack.Add(new NPairBox("Range Indicator Origin Mode:", m_RangeIndicatorOriginModeComboBox, true));
			m_RangeIndicatorOriginModeComboBox.FillFromEnum<ENRangeIndicatorOriginMode>();
			m_RangeIndicatorOriginModeComboBox.SelectedIndex = (int)ENRangeIndicatorOriginMode.ScaleMin;
			m_RangeIndicatorOriginModeComboBox.SelectedIndexChanged +=new Function<NValueChangeEventArgs>(OnRangeIndicatorOriginModeComboBoxSelectedIndexChanged);

			m_RangeIndicatorOriginUpDown = new NNumericUpDown();
			m_RangeIndicatorOriginUpDown.Value = 0.0;
			m_RangeIndicatorOriginUpDown.Enabled = false;
			propertyStack.Add(new NPairBox("Range Indicator Origin:", m_RangeIndicatorOriginUpDown, true));
			m_RangeIndicatorOriginUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnRangeIndicatorOriginUpDownValueChanged);

			m_ValueIndicatorUpDown = new NNumericUpDown();
			propertyStack.Add(new NPairBox("Value Indicator Value:", m_ValueIndicatorUpDown, true));
			m_ValueIndicatorUpDown.Value = m_Indicator2.Value;
			m_ValueIndicatorUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnValueIndicatorUpDownValueChanged);

			m_ValueIndicatorShapeComboBox = new NComboBox();
			propertyStack.Add(new NPairBox("Value Indicator Shape", m_ValueIndicatorShapeComboBox, true));
			m_ValueIndicatorShapeComboBox.FillFromEnum<ENScaleValueMarkerShape>();
			m_ValueIndicatorShapeComboBox.SelectedIndex = (int)m_Indicator2.Shape;
			m_ValueIndicatorShapeComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnValueIndicatorShapeComboBoxSelectedIndexChanged);
			

			m_GaugeOrientationCombo = new NComboBox();
			propertyStack.Add(new NPairBox("Gauge Orientation:", m_GaugeOrientationCombo, true));

			m_GaugeOrientationCombo.FillFromEnum<ENLinearGaugeOrientation>();
			m_GaugeOrientationCombo.SelectedIndex = (int)ENLinearGaugeOrientation.Horizontal;
			m_GaugeOrientationCombo.SelectedIndexChanged +=new Function<NValueChangeEventArgs>(OnGaugeOrientationComboSelectedIndexChanged);

			m_MarkerWidthUpDown = new NNumericUpDown();
			propertyStack.Add(new NPairBox("Marker Width:", m_MarkerWidthUpDown, true));
			m_MarkerWidthUpDown.Value = m_Indicator2.Width;

			m_MarkerHeightUpDown = new NNumericUpDown();
			propertyStack.Add(new NPairBox("Marker Height:", m_MarkerHeightUpDown, true));
			m_MarkerHeightUpDown.Value = m_Indicator2.Height;

            return stack;
        }
		protected override string GetExampleDescription()
		{
			return @"<p>The example demonstrates how to create range and marker gauge indicators.</p>";
		}

		#endregion 

		#region Implementation

		private NScaleSection CreateSection(NColor tickColor, NColor labelColor, NRange range)
		{
			NScaleSection scaleSection = new NScaleSection();
			scaleSection.Range = range;
			scaleSection.MajorTickFill = new NColorFill(tickColor);

			NTextStyle labelStyle = new NTextStyle();
			labelStyle.Fill = new NColorFill(labelColor);
			labelStyle.Font = new NFont("Arimo", 12, ENFontStyle.Bold);
			scaleSection.LabelTextStyle = labelStyle;

			return scaleSection;
		}

		private void ConfigureScale(NLinearScale scale, string text)
		{
			scale.SetPredefinedScale(ENPredefinedScaleStyle.PresentationNoStroke);
			scale.Labels.Style.TextStyle.Font = new NFont("Arimo", 12.0, ENFontStyle.Bold);
			scale.Labels.Style.TextStyle.Fill = new NColorFill(NColor.White);
			scale.Labels.Style.Angle = new NScaleLabelAngle(ENScaleLabelAngleMode.Scale, 90);
			scale.MinorTickCount = 4;
			scale.Ruler.Stroke.Width = 0;
			scale.Ruler.Fill = new NColorFill(NColor.FromColor(NColor.DarkGray, 0.4f));

			scale.Title.RulerAlignment = ENHorizontalAlignment.Left;
			scale.Title.Text = text;
			scale.Title.Angle = new NScaleLabelAngle(ENScaleLabelAngleMode.View, 0);
			scale.Title.Offset = 0.0;
			scale.Title.TextStyle.Font.Size = 12;
			scale.Title.TextStyle.Font.Style = ENFontStyle.Bold;
			scale.Title.TextStyle.Fill = new NStockGradientFill(NColor.White, NColor.LightBlue);

			scale.InflateViewRangeBegin = false;
			scale.InflateViewRangeEnd = false;
		}
		
		static double FarenheitToCelsius(double farenheit)
		{
			return (farenheit - 32.0) * 5.0 / 9.0;
		}

		static double CelsiusToFarenheit(double celsius)
		{
			return (celsius * 9.0) / 5.0 + 32.0f;
		}

		#endregion

		#region Event Handlers

		void OnRangeIndicatorOriginModeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_Indicator1.OriginMode = (ENRangeIndicatorOriginMode)m_RangeIndicatorOriginModeComboBox.SelectedIndex;
			if (m_Indicator1.OriginMode != ENRangeIndicatorOriginMode.Custom)
			{
				m_RangeIndicatorOriginUpDown.Enabled = false;
			}
			else
			{
				m_RangeIndicatorOriginUpDown.Enabled = true;
			}
		}
		
		void  OnGaugeOrientationComboSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_LinearGauge.Orientation = (ENLinearGaugeOrientation)m_GaugeOrientationCombo.SelectedIndex;

			if (m_LinearGauge.Orientation == ENLinearGaugeOrientation.Horizontal)
			{
                m_LinearGauge.PreferredSize = new NSize(400, 150);
				m_LinearGauge.Padding = new NMargins(20, 0, 10, 0);
			}
			else
			{
                m_LinearGauge.PreferredSize = new NSize(150, 400);
				m_LinearGauge.Padding = new NMargins(0, 10, 0, 20);
			}
		}


		void  OnValueIndicatorUpDownValueChanged(NValueChangeEventArgs arg)
		{
 			m_Indicator2.Value = m_ValueIndicatorUpDown.Value;
		}

		void OnRangeIndicatorValueUpDownValueChanged(NValueChangeEventArgs arg)
		{
 			m_Indicator1.Value = m_RangeIndicatorValueUpDown.Value;
		}

		void OnRangeIndicatorOriginUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_Indicator1.Origin = m_RangeIndicatorOriginUpDown.Value;
		}

		void OnValueIndicatorShapeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_Indicator2.Shape = (ENScaleValueMarkerShape)m_ValueIndicatorShapeComboBox.SelectedIndex;
		}

		private void ShowMarkerEditorButton_Click(object sender, System.EventArgs e)
		{
		}

		private void MarkerWidthUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			m_Indicator2.Width = m_MarkerWidthUpDown.Value;
		}

		private void MarkerHeightUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			m_Indicator2.Height = m_MarkerHeightUpDown.Value;
		}

		#endregion

		#region Fields

		NLinearGauge m_LinearGauge;

		NRangeIndicator m_Indicator1;
		NMarkerValueIndicator m_Indicator2;

		NNumericUpDown m_RangeIndicatorValueUpDown;
		NNumericUpDown m_RangeIndicatorOriginUpDown;
		NNumericUpDown m_ValueIndicatorUpDown;

		NComboBox m_RangeIndicatorOriginModeComboBox;
		NComboBox m_ValueIndicatorShapeComboBox;
		NComboBox m_GaugeOrientationCombo;

		NNumericUpDown m_MarkerWidthUpDown;
		NNumericUpDown m_MarkerHeightUpDown;

		#endregion

		#region Schema

		public static readonly NSchema NLinearGaugeIndicatorsExampleSchema;

		#endregion

		#region Static Methods

		protected NBorder CreateBorder()
		{
			return NBorder.CreateThreeColorBorder(NColor.LightGray, NColor.White, NColor.DarkGray, 10, 10);
		}

		#endregion
	}
}
