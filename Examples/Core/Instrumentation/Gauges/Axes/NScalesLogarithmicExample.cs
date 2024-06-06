using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;
using System;

namespace Nevron.Nov.Examples.Gauge
{
    /// <summary>
	///  This example demonstrates how to use logarithmic and reversed scales. 
    /// </summary>
	public class NScalesLogarithmicExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public NScalesLogarithmicExample()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        static NScalesLogarithmicExample()
        {
            NScalesLogarithmicExampleSchema = NSchema.Create(typeof(NScalesLogarithmicExample), NExampleBaseSchema);
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
            m_RadialGauge.Dial = new NDial(ENDialShape.CutCircle, new NEdgeDialRim());

            m_RadialGauge.SweepAngle = new NAngle(-180, NUnit.Degree);
            m_RadialGauge.BeginAngle = new NAngle(-1, NUnit.Degree);

            NAdvancedGradientFill advancedGradient = new NAdvancedGradientFill();
            advancedGradient.BackgroundColor = NColor.DarkCyan;
            advancedGradient.Points.Add(new NAdvancedGradientPoint(NColor.LightSteelBlue, new NAngle(10, NUnit.Degree), 0.1f, 0, 1.0f, ENAdvancedGradientPointShape.Circle));
            m_RadialGauge.Dial.BackgroundFill = advancedGradient;

            // configure scale
            NGaugeAxis axis = new NGaugeAxis();
            m_RadialGauge.Axes.Clear();
            m_RadialGauge.Axes.Add(axis);

            axis.Range = new NRange(1, 100);
            axis.Scale = new NLogarithmicScale();

            m_Scale = (NLogarithmicScale)axis.Scale;
            m_Scale.SetPredefinedScale(ENPredefinedScaleStyle.Standard);
            m_Scale.MajorTickMode = ENMajorTickMode.CustomStep;
            m_Scale.CustomStep = 1;
            m_Scale.MinTickDistance = 30;
            m_Scale.Ruler.Stroke.Width = 0;
            m_Scale.Labels.Style.TextStyle.Fill = new NColorFill(NColor.Black);
            m_Scale.Labels.Style.TextStyle.Font = new NFont("Microsoft Sans Serif", 8, ENFontStyle.Bold);
            m_Scale.OuterMajorTicks.Fill = new NColorFill(NColor.White);
          
            // needle value indicator
            m_ValueIndicator = new NNeedleValueIndicator();
            m_ValueIndicator.Value = 59;
            m_ValueIndicator.Fill = new NStockGradientFill(ENGradientStyle.Horizontal, ENGradientVariant.Variant1, NColor.White, NColor.Red);
            m_ValueIndicator.Stroke.Color = NColor.Red;
            m_ValueIndicator.EnableDampening = true;
            m_ValueIndicator.OffsetFromCenter = -10;
            m_RadialGauge.Indicators.Add(m_ValueIndicator);

            // add radial gauge
            controlStack.Add(m_RadialGauge);

            // Timer
            m_Timer = new NTimer();
            m_Timer.Interval = 200;
            m_Timer.Tick += new Function(OnTimerTick);
            m_Timer.Start();

            return stack;
        }

        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();

            NStackPanel propertyStack = new NStackPanel();
            stack.Add(new NUniSizeBoxGroup(propertyStack));

            // reversed scale check box
            m_ReverseScaleCheckBox = new NCheckBox("Reversed");
            m_ReverseScaleCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnReverseCheckBoxCheckedChange);
            m_ReverseScaleCheckBox.Checked = true;
            propertyStack.Add(m_ReverseScaleCheckBox);

            // logarithmic base 
            m_LogarithmicBaseComboBox = new NComboBox();
            m_LogarithmicBaseComboBox.Items.Add(new NComboBoxItem("10"));
            m_LogarithmicBaseComboBox.Items.Add(new NComboBoxItem("5"));
            m_LogarithmicBaseComboBox.Items.Add(new NComboBoxItem("2"));
            m_LogarithmicBaseComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnLogarithmicBaseComboBoxIndexChanged);
            m_LogarithmicBaseComboBox.SelectedIndex = 2;
			propertyStack.Add(new NPairBox("Logarithmic Base:", m_LogarithmicBaseComboBox, true));

            return stack;
        }

        protected override string GetExampleDescription()
        {
            return @"<p>In this example, the logarithm base is limited to a few predefined values, 
                    but it is possible to set the logarithm base to any integer value.</p>";
        }

        #endregion

        #region Event Handlers
        private void OnTimerTick()
        {
            m_FirstIndicatorAngle += 0.02;
            double needleValue = 50.0 - Math.Cos(m_FirstIndicatorAngle) * 50.0;
            m_ValueIndicator.Value = needleValue;
        }
      
        private void OnReverseCheckBoxCheckedChange(NValueChangeEventArgs arg)
        {
            bool enableInvert = (bool)arg.NewValue;

             m_RadialGauge.Axes[0].Scale.Invert = enableInvert;
        }

        private void OnLogarithmicBaseComboBoxIndexChanged(NValueChangeEventArgs arg)
        {
            int logBase = (int)arg.NewValue;

            switch (logBase)
            {
                case 0:
                    m_Scale.LogarithmBase = 10;
                    break;

                case 1:
                    m_Scale.LogarithmBase = 5;
                    break;

                case 2:
                    m_Scale.LogarithmBase = 2;
                    break;
            }
        }

        #endregion

        #region Fields

        NRadialGauge m_RadialGauge;
        NNeedleValueIndicator m_ValueIndicator;
        NLogarithmicScale m_Scale;
       
        NTimer m_Timer;

        NCheckBox m_ReverseScaleCheckBox;
        NComboBox m_LogarithmicBaseComboBox;

        double m_FirstIndicatorAngle;
        #endregion

        #region Schema

        public static readonly NSchema NScalesLogarithmicExampleSchema;

        #endregion

        #region Constants

        private static readonly NSize defaultRadialGaugeSize = new NSize(300, 300);

        #endregion
    }
}
