using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Gauge
{
    /// <summary>
	/// This example demonstrates how to add indicators to a radial gauge 
    /// </summary>
	public class NRadialGaugeIndicatorsExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public NRadialGaugeIndicatorsExample()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        static NRadialGaugeIndicatorsExample()
        {
			NRadialGaugeIndicatorsExampleSchema = NSchema.Create(typeof(NRadialGaugeIndicatorsExample), NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
		{
			NStackPanel stack = new NStackPanel();
			stack.HorizontalPlacement = ENHorizontalPlacement.Left;

			NStackPanel controlStack = new NStackPanel();
			stack.Add(controlStack);

			// create the radial gauge
			m_RadialGauge = new NRadialGauge();

			m_RadialGauge.PreferredSize = defaultRadialGaugeSize;
			m_RadialGauge.CapEffect = new NGlassCapEffect();
			m_RadialGauge.Dial = new NDial(ENDialShape.CutCircle, new NEdgeDialRim());
			m_RadialGauge.Dial.BackgroundFill = new NStockGradientFill(NColor.DarkGray, NColor.Black);

			// configure scale
			NGaugeAxis axis = new NGaugeAxis();
			m_RadialGauge.Axes.Add(axis);

			NLinearScale scale = (NLinearScale)axis.Scale;

			scale.SetPredefinedScale(ENPredefinedScaleStyle.Presentation);
			scale.Labels.Style.TextStyle.Font = new NFont("Arimo", 10, ENFontStyle.Bold);
			scale.Labels.Style.TextStyle.Fill = new NColorFill(NColor.White);
			scale.Labels.Style.Angle = new NScaleLabelAngle(ENScaleLabelAngleMode.Scale, 90.0);
			scale.MinorTickCount = 4;
			scale.Ruler.Stroke.Width = 0;
			scale.Ruler.Fill = new NColorFill(NColor.DarkGray);

			// add radial gauge indicators
			m_RangeIndicator = new NRangeIndicator();
			m_RangeIndicator.Value = 20;
			m_RangeIndicator.Palette = new NTwoColorPalette(NColor.Green, NColor.Red);
			m_RangeIndicator.Stroke = null;
			m_RangeIndicator.EndWidth = 20;
			m_RadialGauge.Indicators.Add(m_RangeIndicator);

			m_ValueIndicator = new NNeedleValueIndicator();
			m_ValueIndicator.Value = 79;
			m_ValueIndicator.Fill = new NStockGradientFill(ENGradientStyle.Horizontal, ENGradientVariant.Variant1, NColor.White, NColor.Red);
			m_ValueIndicator.Stroke.Color = NColor.Red;

			m_ValueIndicator.OffsetFromCenter = -20;

			m_RadialGauge.Indicators.Add(m_ValueIndicator);
			m_RadialGauge.SweepAngle = new NAngle(270.0, NUnit.Degree);

			// add radial gauge
			controlStack.Add(m_RadialGauge);

			return stack;
		}
        protected override NWidget CreateExampleControls()
        {
			NStackPanel stack = new NStackPanel();

			NStackPanel propertyStack = new NStackPanel();
			stack.Add(new NUniSizeBoxGroup(propertyStack));

			// value indicator properties
			NGroupBox valueIndicatorGroupBox = new NGroupBox("Value");
			propertyStack.Add(valueIndicatorGroupBox);
			NStackPanel valueIndicatorGroupBoxContent = new NStackPanel();
			valueIndicatorGroupBox.Content = new NUniSizeBoxGroup(valueIndicatorGroupBoxContent);

			NMarkerValueIndicator markerValueIndicator = new NMarkerValueIndicator();
			markerValueIndicator.Value = 10;
			m_RadialGauge.Indicators.Add(markerValueIndicator);

			m_ValueIndicatorUpDown = new NNumericUpDown();
			m_ValueIndicatorUpDown.Value = m_ValueIndicator.Value;
			m_ValueIndicatorUpDown.ValueChanged +=new Function<NValueChangeEventArgs>(OnValueIndicatorUpDownValueChanged);
			valueIndicatorGroupBoxContent.Add(new NPairBox("Value:", m_ValueIndicatorUpDown, true));

			m_ValueIndicatorWidthUpDown = new NNumericUpDown();
			m_ValueIndicatorWidthUpDown.Value = m_ValueIndicator.Width;
			m_ValueIndicatorWidthUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnValueIndicatorWidthUpDownValueChanged);
			valueIndicatorGroupBoxContent.Add(new NPairBox("Width:", m_ValueIndicatorWidthUpDown, true));

			m_ValueIndicatorOffsetFromCenterUpDown = new NNumericUpDown();
			m_ValueIndicatorOffsetFromCenterUpDown.Value = m_ValueIndicator.OffsetFromCenter;
			m_ValueIndicatorOffsetFromCenterUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnValueIndicatorOffsetFromCenterUpDownValueChanged);
			valueIndicatorGroupBoxContent.Add(new NPairBox("Offset From Center:", m_ValueIndicatorOffsetFromCenterUpDown, true));

			m_ValueIndicatorShapeComboBox = new NComboBox();
			m_ValueIndicatorShapeComboBox.FillFromEnum<ENNeedleShape>();
			m_ValueIndicatorShapeComboBox.SelectedIndex = (int)m_ValueIndicator.Shape;
			m_ValueIndicatorShapeComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnValueIndicatorShapeComboBoxSelectedIndexChanged);
			valueIndicatorGroupBoxContent.Add(new NPairBox("Shape:", m_ValueIndicatorShapeComboBox, true));

			// Range indicator properties
			NGroupBox rangeIndicatorGroupBox = new NGroupBox("Range");
			propertyStack.Add(rangeIndicatorGroupBox);
			NStackPanel rangeIndicatorGroupBoxContent = new NStackPanel();
			rangeIndicatorGroupBox.Content = new NUniSizeBoxGroup(rangeIndicatorGroupBoxContent);

			m_RangeIndicatorOriginModeComboBox = new NComboBox();
			m_RangeIndicatorOriginModeComboBox.FillFromEnum<ENRangeIndicatorOriginMode>();
			m_RangeIndicatorOriginModeComboBox.SelectedIndex = (int)m_RangeIndicator.OriginMode;
			rangeIndicatorGroupBoxContent.Add(new NPairBox("Origin Mode:", m_RangeIndicatorOriginModeComboBox, true));

			m_RangeIndicatorOriginUpDown = new NNumericUpDown();
			m_RangeIndicatorOriginUpDown.Value = m_RangeIndicator.Origin;
			m_RangeIndicatorOriginUpDown.ValueChanged +=new Function<NValueChangeEventArgs>(OnRangeIndicatorOriginUpDownValueChanged);
			rangeIndicatorGroupBoxContent.Add(new NPairBox("Origin:", m_RangeIndicatorOriginUpDown, true));

			m_RangeIndicatorValueUpDown = new NNumericUpDown();
			m_RangeIndicatorValueUpDown.Value = m_RangeIndicator.Value; 
			m_RangeIndicatorValueUpDown.ValueChanged +=new Function<NValueChangeEventArgs>(OnRangeIndicatorValueUpDownValueChanged);
			rangeIndicatorGroupBoxContent.Add(new NPairBox("Value:", m_RangeIndicatorValueUpDown, true));

			m_BeginAngleUpDown = new NNumericUpDown();
			m_BeginAngleUpDown.Maximum = 360;
			m_BeginAngleUpDown.Minimum = -360;
			m_BeginAngleUpDown.Value = m_RadialGauge.BeginAngle.ToDegrees();
			m_BeginAngleUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnBeginAngleUpDownValueChanged);
			propertyStack.Add(new NPairBox("Begin Angle:", m_BeginAngleUpDown, true));

			m_SweepAngleUpDown = new NNumericUpDown();
			m_SweepAngleUpDown.Maximum = 360;
			m_SweepAngleUpDown.Minimum = -360;
			m_SweepAngleUpDown.Value = m_RadialGauge.SweepAngle.ToDegrees();
			m_SweepAngleUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnSweepAngleUpDownValueChanged);
			propertyStack.Add(new NPairBox("Sweep Angle:", m_SweepAngleUpDown, true));

            return stack;
        }
		protected override string GetExampleDescription()
		{
			return @"<p>The example demonstrates how to create range and needle gauge indicators.</p>";
		}

		#endregion 

		#region Event Handlers

		void OnValueIndicatorWidthUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_ValueIndicator.Width = m_ValueIndicatorWidthUpDown.Value;
		}

		void OnValueIndicatorOffsetFromCenterUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_ValueIndicator.OffsetFromCenter = m_ValueIndicatorOffsetFromCenterUpDown.Value;
		}

		void OnValueIndicatorShapeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_ValueIndicator.Shape = (ENNeedleShape)m_ValueIndicatorShapeComboBox.SelectedIndex;
		}

		void OnValueIndicatorUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_ValueIndicator.Value = m_ValueIndicatorUpDown.Value;
		}

		void  OnRangeIndicatorValueUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_RangeIndicator.Value = m_RangeIndicatorValueUpDown.Value;
		}
		void  OnRangeIndicatorOriginUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_RangeIndicator.Origin = m_RangeIndicatorOriginUpDown.Value;
		}

		void OnSweepAngleUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_RadialGauge.SweepAngle = new NAngle(m_SweepAngleUpDown.Value, NUnit.Degree);
		}
		void OnBeginAngleUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_RadialGauge.BeginAngle = new NAngle(m_BeginAngleUpDown.Value, NUnit.Degree);
		}

		#endregion 

		#region Fields

		NRadialGauge m_RadialGauge;
		NRangeIndicator m_RangeIndicator;
		NNeedleValueIndicator m_ValueIndicator;

		NComboBox m_RangeIndicatorOriginModeComboBox;
		NNumericUpDown m_RangeIndicatorOriginUpDown;
		NNumericUpDown m_RangeIndicatorValueUpDown;

		NNumericUpDown m_ValueIndicatorUpDown;
		NNumericUpDown m_ValueIndicatorWidthUpDown;
		NNumericUpDown m_ValueIndicatorOffsetFromCenterUpDown;
		NComboBox m_ValueIndicatorShapeComboBox;
		
		NNumericUpDown m_BeginAngleUpDown;
		NNumericUpDown m_SweepAngleUpDown;

		#endregion

		#region Schema

		public static readonly NSchema NRadialGaugeIndicatorsExampleSchema;

		#endregion

		#region Constants

		private static readonly NSize defaultRadialGaugeSize = new NSize(300, 300);

		#endregion
	}
}
