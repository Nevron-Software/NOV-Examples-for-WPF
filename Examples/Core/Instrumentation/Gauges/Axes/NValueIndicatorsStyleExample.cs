using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;
using System;

namespace Nevron.Nov.Examples.Gauge
{
	/// <summary>
	/// 
	/// </summary>
	public class NValueIndicatorsStyleExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public NValueIndicatorsStyleExample()
        {

        }
		/// <summary>
		/// 
		/// </summary>
		
		static NValueIndicatorsStyleExample()
		{
            NValueIndicatorsStyleExampleSchema = NSchema.Create(typeof(NValueIndicatorsStyleExample), NExampleBaseSchema);
        }

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NStackPanel stack = new NStackPanel();
			
			NStackPanel controlStack = new NStackPanel();
			stack.Add(controlStack);

			// create the radial gauge
			m_RadialGauge = new NRadialGauge();
			m_RadialGauge.PreferredSize = defaultRadialGaugeSize;
			controlStack.Add(m_RadialGauge);

			m_RadialGauge.Dial = new NDial(ENDialShape.Circle, new NEdgeDialRim());
			m_RadialGauge.Dial.BackgroundFill = new NStockGradientFill(NColor.DarkGray, NColor.Black);

			NGelCapEffect gelEffect = new NGelCapEffect(ENCapEffectShape.Ellipse);
			gelEffect.Margins = new NMargins(0, 0, 0, 0.5);

            m_RadialGauge.Axes.Clear();

            m_RadialGauge.SweepAngle = new NAngle(360, NUnit.Degree);

            // create the first axis
            NGaugeAxis axis1 = new NGaugeAxis();
			axis1.Anchor = new NDockGaugeAxisAnchor(ENGaugeAxisDockZone.Top, true, 0, 70);
           
            m_RadialGauge.Axes.Add(axis1);

            // Scale 
            NStandardScale scale1 = (NStandardScale)axis1.Scale;
			scale1.SetPredefinedScale(ENPredefinedScaleStyle.PresentationNoStroke);
			scale1.MinorTickCount = 3;
			scale1.Ruler.Fill = new NColorFill(NColor.FromColor(NColor.White, 0.4f));
			scale1.OuterMajorTicks.Fill = new NColorFill(NColor.Orange);
			scale1.Labels.Style.TextStyle.Font  = new NFont("Arimo", 12, ENFontStyle.Bold);
			scale1.Labels.Style.TextStyle.Fill = new NColorFill(NColor.White);
			scale1.Labels.Style.Angle = new NScaleLabelAngle(ENScaleLabelAngleMode.Scale, 0);

			// create the second axis
			NGaugeAxis axis2 = new NGaugeAxis();
			axis2.Anchor = new NDockGaugeAxisAnchor(ENGaugeAxisDockZone.Top, false, 75, 95);
           
            m_RadialGauge.Axes.Add(axis2);
           
            // scale
            NStandardScale scale2 = (NStandardScale)axis2.Scale;
            scale2.SetPredefinedScale(ENPredefinedScaleStyle.PresentationNoStroke);
			scale2.MinorTickCount = 3;
			scale2.Ruler.Fill = new NColorFill(NColor.FromColor(NColor.White, 0.4f));
			scale2.OuterMajorTicks.Fill = new NColorFill(NColor.Blue);
			scale2.Labels.Style.TextStyle.Font = new NFont("Arimo", 12, ENFontStyle.Bold);
			scale2.Labels.Style.TextStyle.Fill = new NColorFill(NColor.White);
			scale2.Labels.Style.Angle = new NScaleLabelAngle(ENScaleLabelAngleMode.Scale, 0);

			// add range indicators
			NRangeIndicator rangeIndicator = new NRangeIndicator();
            rangeIndicator.OriginMode = ENRangeIndicatorOriginMode.ScaleMax;
            rangeIndicator.Value = 100;
			rangeIndicator.Palette = new NThreeColorPalette(NColor.Green, NColor.Red, NColor.Orange, 50.0);
			rangeIndicator.Stroke.Width = 0;
			rangeIndicator.OffsetFromScale = 3;
			rangeIndicator.BeginWidth = 15;
			rangeIndicator.EndWidth = 25;

			m_RadialGauge.Indicators.Add(rangeIndicator);

			// needle value indicator1
            m_ValueIndicator = new NNeedleValueIndicator();
            m_ValueIndicator.Value = 79;
            m_ValueIndicator.Fill = new NStockGradientFill(ENGradientStyle.Horizontal, ENGradientVariant.Variant1, NColor.White, NColor.Red);
            m_ValueIndicator.Stroke.Color = NColor.Red;
			m_ValueIndicator.ScaleAxis = axis1;
			m_ValueIndicator.OffsetFromScale = 2;
				
			m_RadialGauge.Indicators.Add(m_ValueIndicator);

            // needle value indicator2
            m_ValueIndicator1 = new NNeedleValueIndicator();
            m_ValueIndicator1.Value = 79;
            m_ValueIndicator1.ScaleAxis = axis2;
            m_ValueIndicator1.OffsetFromScale = 2;
            
			m_RadialGauge.Indicators.Add(m_ValueIndicator1);

            // timer
            m_DataFeedTimer = new NTimer();
            m_DataFeedTimer.Tick += new Function(OnDataFeedTimerTick);
            m_DataFeedTimer.Start();

            return stack;
		}
		protected override NWidget CreateExampleControls()
        {
			NStackPanel stack = new NStackPanel();

			NStackPanel propertyStack = new NStackPanel();
			stack.Add(new NUniSizeBoxGroup(propertyStack));

			// pointer properties group
			NGroupBox controlsGroupBox = new NGroupBox("Controls");
			propertyStack.Add(controlsGroupBox);
			NStackPanel controlsGroupBoxContent = new NStackPanel();
            controlsGroupBox.Content = new NUniSizeBoxGroup(controlsGroupBoxContent);

            // value indicator 1 shape Combo
            m_ValueIndicatorShapeComboBox = new NComboBox();
            m_ValueIndicatorShapeComboBox.FillFromEnum<ENNeedleShape>();
            m_ValueIndicatorShapeComboBox.SelectedIndex = (int)m_ValueIndicator.Shape;
            m_ValueIndicatorShapeComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnValueIndicatorShapeComboBoxSelectedIndexChanged);
            controlsGroupBoxContent.Add(new NPairBox("Value Indicator 1 Shape:", m_ValueIndicatorShapeComboBox, true));

            // value indicator 1 width 
            m_ValueIndicatorWidthUpDown = new NNumericUpDown();
            m_ValueIndicatorWidthUpDown.Value = m_ValueIndicator.Width;
            m_ValueIndicatorWidthUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnValueIndicatorWidthUpDownValueChanged);
            controlsGroupBoxContent.Add(new NPairBox("Value Indicator 1 Width:", m_ValueIndicatorWidthUpDown, true));

            // value indicator 2 shape Combo
            m_ValueIndicator1ShapeComboBox = new NComboBox();
            m_ValueIndicator1ShapeComboBox.FillFromEnum<ENNeedleShape>();
            m_ValueIndicator1ShapeComboBox.SelectedIndex = (int)m_ValueIndicator1.Shape;
            m_ValueIndicator1ShapeComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnValueIndicatorShape1ComboBoxSelectedIndexChanged);
            controlsGroupBoxContent.Add(new NPairBox("Value Indicator 2 Shape:", m_ValueIndicator1ShapeComboBox, true));

            // value indicator 2 width 
            m_ValueIndicator1WidthUpDown = new NNumericUpDown();
            m_ValueIndicator1WidthUpDown.Value = m_ValueIndicator.Width;
            m_ValueIndicator1WidthUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnValueIndicator1WidthUpDownValueChanged);
            controlsGroupBoxContent.Add(new NPairBox("Value Indicator 2 Width:", m_ValueIndicator1WidthUpDown, true));

            // needle cap visability
            m_CheckBoxShowNeedleCap = new NCheckBox("Show Needle Cap");
            m_CheckBoxShowNeedleCap.Checked = m_CheckBoxShowNeedleCap.Checked;
            m_CheckBoxShowNeedleCap.CheckedChanged += new Function<NValueChangeEventArgs>(OnShowNeedleCapChecked);
            controlsGroupBoxContent.Add(m_CheckBoxShowNeedleCap);
			
			// cap size
			m_UpDownNeedleCapSize = new NNumericUpDown();
			m_UpDownNeedleCapSize.Value = m_UpDownNeedleCapSize.Value;
			m_UpDownNeedleCapSize.ValueChanged += new Function<NValueChangeEventArgs>(OnUpDownNeedleCapSizeChanged);
            controlsGroupBoxContent.Add(new NPairBox("Change Cap Size", m_UpDownNeedleCapSize, true));

            // scroll bar
            m_ScrollBar = new NHScrollBar();
			m_ScrollBar.Value = 70.0;
			m_ScrollBar.ValueChanged += new Function<NValueChangeEventArgs>(OnScrollBarValueChanged);
			propertyStack.Add(new NPairBox("Percent:", m_ScrollBar, true));

            return stack;
        }

		protected override string GetExampleDescription()
		{
			return @"<p>The example demonstrates how use the begin and end percent properties of the anchor in order to change the gauge axis size.</p>";
		}

        #endregion

        #region Event Handlers

        void OnValueIndicatorShapeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            m_ValueIndicator.Shape = (ENNeedleShape)m_ValueIndicatorShapeComboBox.SelectedIndex;
        }
        private void OnValueIndicatorWidthUpDownValueChanged(NValueChangeEventArgs arg)
        {
			if (!(arg.NewValue is double))
				return;

            m_ValueIndicator.Width = (double)arg.NewValue;

        }
        private void OnValueIndicatorShape1ComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
        {            
            m_ValueIndicator1.Shape = (ENNeedleShape)m_ValueIndicator1ShapeComboBox.SelectedIndex;
        }
        private void OnValueIndicator1WidthUpDownValueChanged(NValueChangeEventArgs arg)
        {
            if (!(arg.NewValue is double))
                return;

            m_ValueIndicator1.Width = (double)arg.NewValue;
        }
        private void OnShowNeedleCapChecked(NValueChangeEventArgs arg)
        {
            m_RadialGauge.NeedleCap.Visible = m_CheckBoxShowNeedleCap.Checked;
        }

		private void OnUpDownNeedleCapSizeChanged(NValueChangeEventArgs arg)
		{
            double capSize = m_UpDownNeedleCapSize.Value;
            m_RadialGauge.NeedleCap.Size = new NSize(capSize,capSize); 
        }

		private void OnScrollBarValueChanged(NValueChangeEventArgs arg)
		{
			 NGaugeAxis axis1 = (NGaugeAxis)m_RadialGauge.Axes[0];
			 NGaugeAxis axis2 = (NGaugeAxis)m_RadialGauge.Axes[1];

			 axis1.Anchor = new NDockGaugeAxisAnchor(ENGaugeAxisDockZone.Top, true, 0, (float)(m_ScrollBar.Value - 5));
			 axis2.Anchor = new NDockGaugeAxisAnchor(ENGaugeAxisDockZone.Top, false, (float)m_ScrollBar.Value, 95);
//			 RedAxisTextBox.Text = m_ScrollBar.Value.ToString();
		}
        void OnDataFeedTimerTick()
        {
            // update the indicator 
            m_FirstIndicatorAngle += 0.02;
            double value = 50.0 - Math.Cos(m_FirstIndicatorAngle) * 50.0;

            m_ValueIndicator.Value = value;
            m_ValueIndicator1.Value = value;
        }

        #endregion

        #region Fields

        NRadialGauge m_RadialGauge;
		NHScrollBar m_ScrollBar;
        NNeedleValueIndicator m_ValueIndicator;
        NNeedleValueIndicator m_ValueIndicator1;

        NComboBox m_ValueIndicatorShapeComboBox;
        NCheckBox m_CheckBoxShowNeedleCap;
        NComboBox m_ValueIndicator1ShapeComboBox;

        NNumericUpDown m_ValueIndicatorWidthUpDown;
		NNumericUpDown m_ValueIndicator1WidthUpDown;
        NNumericUpDown m_UpDownNeedleCapSize;

		NTimer m_DataFeedTimer;
        double m_FirstIndicatorAngle;


        #endregion

        #region Schema

        public static readonly NSchema NValueIndicatorsStyleExampleSchema;

		#endregion

		#region Constants

		private static readonly NSize defaultRadialGaugeSize = new NSize(300, 300);

		#endregion
	}
}
