using Nevron.Nov.Chart;
using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.UI;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;
using System;
using System.Threading;
using System.Timers;

namespace Nevron.Nov.Examples.Gauge
{
    /// <summary>
    /// This example demonstrates dampening of a pointer, which simulates a fluid-filled gauge.
    /// </summary>
    public class NPointerDampeningExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public NPointerDampeningExample()
        {

        }
        /// <summary>
        /// 
        /// </summary>

        static NPointerDampeningExample()
        {
            NPointerDampeningExampleSchema = NSchema.Create(typeof(NPointerDampeningExample), NExampleBaseSchema);
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
            m_RadialGauge.Dial = new NDial(ENDialShape.RoundedOutline, new NEdgeDialRim());
            m_RadialGauge.Dial.CornerRounding = 23;
            m_RadialGauge.Dial.BackgroundFill = new NStockGradientFill(NColor.Black, NColor.WhiteSmoke);
            
            m_RadialGauge.SweepAngle = new NAngle(60, NUnit.Degree);
            m_RadialGauge.BeginAngle = new NAngle(240, NUnit.Degree);

            controlStack.Add(m_RadialGauge);

            // create the first axis
            NGaugeAxis axis1 = new NGaugeAxis();
            axis1.Anchor = new NDockGaugeAxisAnchor(ENGaugeAxisDockZone.Top, true, 0, 100);

            m_RadialGauge.Axes.Add(axis1);

            // Scale 
            NStandardScale scale1 = (NStandardScale)axis1.Scale;
            scale1.SetPredefinedScale(ENPredefinedScaleStyle.PresentationNoStroke);
            scale1.MinorTickCount = 10;
            scale1.Ruler.Fill = new NColorFill(NColor.FromColor(NColor.DarkGray,5.0f));
            scale1.OuterMajorTicks.Fill = new NColorFill(NColor.OrangeRed);
            scale1.Labels.Style.TextStyle.Font = new NFont("Arial", 12, ENFontStyle.Regular);
            scale1.Labels.Style.TextStyle.Fill = new NColorFill(NColor.White);
            scale1.Labels.Style.Angle = new NScaleLabelAngle(ENScaleLabelAngleMode.Scale, 0);

            // needle value indicator1
            m_NeedleValueIndicator = new NNeedleValueIndicator();
            m_NeedleValueIndicator.Value = 20;
            m_NeedleValueIndicator.Fill = new NStockGradientFill(ENGradientStyle.Horizontal, ENGradientVariant.Variant1, NColor.White, NColor.Red);
            m_NeedleValueIndicator.Stroke.Color = NColor.Red;
            m_NeedleValueIndicator.ScaleAxis = axis1;
            m_NeedleValueIndicator.OffsetFromScale = 2;
            m_NeedleValueIndicator.EnableDampening = true;
        
            m_RadialGauge.Indicators.Add(m_NeedleValueIndicator);

            // marker value indicator 
            m_MarkerValueIndicator = new NMarkerValueIndicator();
            m_MarkerValueIndicator.EnableDampening = true;
            m_MarkerValueIndicator.Value = 20;

            // range value indicator 
            m_RangeValueIndicator = new NRangeIndicator();
            m_RangeValueIndicator.EnableDampening = true;
            m_RangeValueIndicator.Value = 20;

            // Timer
            m_Timer = new NTimer();
            m_Timer.Interval = 1000;
            m_Timer.Tick += new Function(OnTimerTick);
            m_Timer.Start();
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

            NButton toggleTimerButton = new NButton("Stop Timer");
            toggleTimerButton.Click += OnToggleTimerButtonClick;
            toggleTimerButton.Tag = 0;
            stack.Add(toggleTimerButton);

            // enable dampening checkbox
            m_EnableDampeningCheckBox = new NCheckBox("Enable Value Dampening");
            m_EnableDampeningCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnEnableDampeningCheckBoxCheckedChanged);
            m_EnableDampeningCheckBox.Checked = true;
            propertyStack.Add(m_EnableDampeningCheckBox);

            // dampening steps up and down value 
            m_DampeningStepsUpDown = new NNumericUpDown();
            m_DampeningStepsUpDown.Minimum = 1;
            m_DampeningStepsUpDown.Maximum = 100;
            m_DampeningStepsUpDown.ValueChanged += new Function<NValueChangeEventArgs>(UpdateDampeingStepsUpAndDown);
            m_DampeningStepsUpDown.Value = 20;
            propertyStack.Add(new NPairBox("Steps: ", m_DampeningStepsUpDown, true));

            // dampening interval up and down value 
            m_DampeningIntervalUpDown = new NNumericUpDown();
            m_DampeningIntervalUpDown.Minimum = 1;
            m_DampeningIntervalUpDown.ValueChanged += new Function<NValueChangeEventArgs>(UpdateDampeingIntervalUpAndDown);
            m_DampeningIntervalUpDown.Value = 50;
            propertyStack.Add(new NPairBox("Interval: ", m_DampeningIntervalUpDown, true));

            // value indicator combo box
            m_ValueIndicatorTypeComboBox = new NComboBox();
            m_ValueIndicatorTypeComboBox.Items.Add(new NComboBoxItem("Needle"));
            m_ValueIndicatorTypeComboBox.Items.Add(new NComboBoxItem("Marker"));
            m_ValueIndicatorTypeComboBox.Items.Add(new NComboBoxItem("Bar"));
            m_ValueIndicatorTypeComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnValueIndicatorComboBoxIndexChanged);
            m_ValueIndicatorTypeComboBox.SelectedIndex = 0;
            controlsGroupBoxContent.Add(new NPairBox("Marker Pointer Scale:", m_ValueIndicatorTypeComboBox, true));

            return stack;
        }
       
        protected override string GetExampleDescription()
        {
            return @"<p>This example demonstrates how to enable the value dampening feature of the gauge indicators. When enabled the value dampening will smooths the transition of indicators when their value changes.
          Use the controls on the right side to modify various parameters of the dampening effect.  </p>";
        }

        #endregion

        #region Event Handlers 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        private void OnValueIndicatorComboBoxIndexChanged(NValueChangeEventArgs arg)
        {
            m_RadialGauge.Indicators.Clear();

            switch ((int)arg.NewValue)
            {
                case 0:
                    m_RadialGauge.Indicators.Add(m_NeedleValueIndicator);
                    break;
                case 1:
                    m_RadialGauge.Indicators.Add(m_MarkerValueIndicator); 
                    break;
                case 2:
                    m_RadialGauge.Indicators.Add(m_RangeValueIndicator);
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        private void UpdateDampeingIntervalUpAndDown(NValueChangeEventArgs arg)
        {
            SetValueIndicator(NValueIndicator.DampeningIntervalProperty, (int)m_DampeningIntervalUpDown.Value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        private void UpdateDampeingStepsUpAndDown(NValueChangeEventArgs arg)
        {
            SetValueIndicator(NValueIndicator.DampeningStepsProperty, (int)m_DampeningStepsUpDown.Value);
        }
        /// <summary>
        ///  Set whether dampening is on or off.
        /// </summary>
        /// <param name="arg"></param>
        private void OnEnableDampeningCheckBoxCheckedChanged(NValueChangeEventArgs arg)
        {
            bool enableDampening = (bool)arg.NewValue;

            m_NeedleValueIndicator.EnableDampening = enableDampening;
            m_MarkerValueIndicator.EnableDampening = enableDampening;
            m_RangeValueIndicator.EnableDampening = enableDampening;
        }
        /// <summary>
        /// Handles timer's click as evaluating random value and set it on the indicators. 
        /// </summary>
        private void OnTimerTick()
        {
            m_Angle += Math.PI / 180.0;
            double value = 50 + Math.Sin(m_Angle) * (20 + rand.Next(30));

            SetValueIndicator(NValueIndicator.ValueProperty, value);
        }
        /// <summary>
        /// Set the value of specified properties of the indicators
        /// </summary>
        /// <param name="indicatorProperty"></param>
        /// <param name="value"></param>
        private void SetValueIndicator(NProperty indicatorProperty, double value)
        {
            string stringValue = value.ToString();

            m_NeedleValueIndicator.SetFx(indicatorProperty, stringValue);
            m_MarkerValueIndicator.SetFx(indicatorProperty, stringValue);
            m_RangeValueIndicator.SetFx(indicatorProperty, stringValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        void OnToggleTimerButtonClick(NEventArgs arg)
        {
            NButton button = (NButton)arg.TargetNode;
            if ((int)button.Tag == 0)
            {
                m_Timer.Stop();

                button.Content = new NLabel("Start Timer");
                button.Tag = 1;
            }
            else
            {
                m_Timer.Start();

                button.Content = new NLabel("Stop Timer");
                button.Tag = 0;
            }
        }

        #endregion

        #region Implementation

        #endregion

        #region Fields

        NRadialGauge m_RadialGauge;
        NNeedleValueIndicator m_NeedleValueIndicator;
        NMarkerValueIndicator m_MarkerValueIndicator;
        NRangeIndicator m_RangeValueIndicator;

        NComboBox m_ValueIndicatorTypeComboBox;
        NNumericUpDown m_DampeningStepsUpDown;
        NNumericUpDown m_DampeningIntervalUpDown;
        NCheckBox m_EnableDampeningCheckBox;
        
        NTimer m_Timer;

        double m_Angle;
        private Random rand = new Random();
        #endregion

        #region Schema

        public static readonly NSchema NPointerDampeningExampleSchema;

        #endregion

        #region Constants

        private static readonly NSize defaultRadialGaugeSize = new NSize(300, 300);
       
        #endregion
    }
}
