using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Gauge
{
    /// <summary>
    /// This example demonstrates how to use state indicators in combination with other gauges
    /// </summary>
    public class NCarDashboardExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public NCarDashboardExample()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        static NCarDashboardExample()
        {
            NCarDashboardExampleSchema = NSchema.Create(typeof(NCarDashboardExample), NExampleBaseSchema);
        }

        #endregion

        #region Example
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override NWidget CreateExampleContent()
        {
            NStackPanel stack = new NStackPanel();

            NStackPanel controlStack = new NStackPanel();
            controlStack.Direction = Layout.ENHVDirection.LeftToRight;
            controlStack.HorizontalPlacement = Layout.ENHorizontalPlacement.Left;
            controlStack.VerticalPlacement = Layout.ENVerticalPlacement.Top;

            controlStack.Add(CreateGaugePanel("km/h", new NRange(0, 260), new NRange(200, 220), out m_SpeedIndicator, out m_SpeedLedDisplay, out m_SpeedStateIndicator));
            controlStack.Add(CreateGaugePanel("rpm", new NRange(0, 7000), new NRange(5000, 7000), out m_RpmIndicator, out m_RpmLedDisplay, out m_RpmStateIndicator));

            stack.Add(controlStack);

            return stack;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();

            m_AccelerationScrollBar = new NHScrollBar();
            m_AccelerationScrollBar.Minimum = -100;
            m_AccelerationScrollBar.Maximum = 100;
            m_AccelerationScrollBar.Value = 0;
            stack.Add(NPairBox.Create("Acceleration: ", m_AccelerationScrollBar));

            m_Timer = new NTimer();
            m_Timer.Tick += OnTimerTick;
            m_Timer.Start();

            return stack;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string GetExampleDescription()
        {
            return @"<p>The example demonstrates how to use state indicators in combination with other controls. Move the accelerator scrollbar on the right to see the effect of the speed / rpm entering into the red axis range.</p>";
        }

        #endregion

        #region Event Handlers

        #endregion

        #region Implementation

        /// <summary>
        /// Called when the example is unloaded
        /// </summary>
        protected override void OnUnregistered()
        {
            base.OnUnregistered();

            // stop the update timer
            m_Timer.Stop();
            m_Timer = null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="needleValueIndicator"></param>
        /// <param name="numericLedDisplay"></param>
        /// <param name="stateIndicator"></param>
        /// <returns></returns>
        private static NUserPanel CreateGaugePanel(string text, NRange axisRange, NRange redRange, out NNeedleValueIndicator needleValueIndicator, out NNumericLedDisplay numericLedDisplay, out NStateIndicator stateIndicator)
        {
            NUserPanel userPanel = new NUserPanel();
            userPanel.PreferredSize = defaultRadialGaugeSize;

            NRadialGauge radialGauge = new NRadialGauge();
            userPanel.Add(radialGauge);

            // create the radial gauge
            radialGauge.CapEffect = new NGlassCapEffect();
            radialGauge.SetFx(NRadialGauge.WidthProperty, "$Parent.Width");
            radialGauge.SetFx(NRadialGauge.HeightProperty, "$Parent.Height");

            radialGauge.PreferredSize = defaultRadialGaugeSize;
            radialGauge.Dial = new NDial(ENDialShape.Circle, new NEdgeDialRim());
            radialGauge.Dial.BackgroundFill = CreateAdvancedGradient();

            // configure axis
            NGaugeAxis axis = new NGaugeAxis();
            axis.Anchor = new NDockGaugeAxisAnchor(ENGaugeAxisDockZone.Top, true, 0, 100);
            radialGauge.Axes.Add(axis);
            axis.Range = axisRange;

            NStandardScale scale = (NStandardScale)axis.Scale;
            ConfigureScale(scale, redRange);
            radialGauge.Indicators.Add(CreateRangeIndicator(redRange.Begin));

            needleValueIndicator = new NNeedleValueIndicator();
            needleValueIndicator.Value = 0;
            needleValueIndicator.Fill = new NStockGradientFill(ENGradientStyle.Horizontal, ENGradientVariant.Variant1, NColor.White, NColor.Red);
            needleValueIndicator.Stroke.Color = NColor.Red;
            needleValueIndicator.PaintOrder = ENIndicatorPaintOrder.PostPaint;
            radialGauge.Indicators.Add(needleValueIndicator);

            radialGauge.BeginAngle = new NAngle(-240, NUnit.Degree);
            radialGauge.SweepAngle = new NAngle(300, NUnit.Degree);

            NUserPanel radialGaugeContentPanel = new NUserPanel();
            radialGaugeContentPanel.SetFx(NUserPanel.WidthProperty, "$Parent.Width");
            radialGaugeContentPanel.SetFx(NUserPanel.HeightProperty, "$Parent.Height");
            radialGauge.Content = radialGaugeContentPanel;

            // configure led display
            numericLedDisplay = new NNumericLedDisplay();
            radialGaugeContentPanel.Add(numericLedDisplay);
            numericLedDisplay.PreferredSize = new NSize(150, 120);
            numericLedDisplay.Value = 0.0;
            numericLedDisplay.CellCountMode = ENDisplayCellCountMode.Fixed;
            numericLedDisplay.CellCount = 7;
            numericLedDisplay.BackgroundFill = new NColorFill(NColor.Black);

            numericLedDisplay.Border = NBorder.CreateSunken3DBorder(new NUIThemeColorMap(ENUIThemeScheme.WindowsClassic));
            numericLedDisplay.BorderThickness = new NMargins(2);
            numericLedDisplay.Padding = new NMargins(2);
            numericLedDisplay.CapEffect = new NGelCapEffect();

            numericLedDisplay.SetFx(NNumericLedDisplay.XProperty, "$Parent.Width / 2 - (Width / 2)");
            numericLedDisplay.SetFx(NNumericLedDisplay.YProperty, "$Parent.Height / 2 + 10");
            numericLedDisplay.SetFx(NNumericLedDisplay.WidthProperty, "$Parent.Width / 2");
            numericLedDisplay.SetFx(NNumericLedDisplay.HeightProperty, "$Parent.Height/ 10");

            // configure state indicator
            stateIndicator = new NStateIndicator();
            radialGaugeContentPanel.Add(stateIndicator);
            stateIndicator.SetFx(NButton.XProperty, "$Parent.Width / 2 - (Width / 2)");
            stateIndicator.SetFx(NButton.YProperty, "$Parent.Height * 3/ 4");
            stateIndicator.SetFx(NStateIndicator.WidthProperty, "10");
            stateIndicator.SetFx(NStateIndicator.HeightProperty, "10");
            stateIndicator.States.Add(new NIndicatorState(new NRange(axisRange.Begin, redRange.Begin), ENSymbolShape.Ellipse, stateIndicatorSize, new NColorFill(NColor.LimeGreen), null));
            stateIndicator.States.Add(new NIndicatorState(redRange, ENSymbolShape.Ellipse, stateIndicatorSize, new NColorFill(NColor.OrangeRed), null));

            // add labels
            NLabel label = new NLabel(text);
            userPanel.Add(label);
            label.Font = new NFont("Times New Roman", 20, ENFontStyle.Italic);
            label.TextFill = new NColorFill(NColor.White);

            label.SetFx(NLabel.XProperty, "$Parent.Width / 2 - (Width / 2)");
            label.SetFx(NLabel.YProperty, "$Parent.Height / 2 + 50");

            return userPanel;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static NFill CreateAdvancedGradient()
        {
            NAdvancedGradientFill agfs = new NAdvancedGradientFill();

            agfs.BackgroundColor = NColor.FromRGB(234, 234, 234);

            NAdvancedGradientPoint point1 = new NAdvancedGradientPoint();
            point1.PositionX = 0.5f;
            point1.PositionY = 0.5f;
            point1.Color = NColor.FromRGB(51, 51, 51);
            point1.Intensity = 0.7f;
            agfs.Points.Add(point1);

            NAdvancedGradientPoint point2 = new NAdvancedGradientPoint();
            point2.PositionX = 0.5f;
            point2.PositionY = 0.5f;
            point2.Color = NColor.FromRGB(41, 41, 41);
            point2.Intensity = 0.5f;
            agfs.Points.Add(point2);

            return agfs;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scale"></param>
        /// <param name="redRange"></param>
        private static void ConfigureScale(NStandardScale scale, NRange redRange)
        {
            scale.SetPredefinedScale(ENPredefinedScaleStyle.PresentationNoStroke);
            scale.Labels.Style.TextStyle.Font = new NFont("Arial", 11, ENFontStyle.Bold);
            scale.Labels.Style.TextStyle.Fill = new NColorFill(NColor.White);
            scale.Labels.Style.Angle = new NScaleLabelAngle(ENScaleLabelAngleMode.Scale, 0);
            scale.MinorTickCount = 1;
            scale.Ruler.Stroke.Width = 0;
            scale.Ruler.Fill = new NColorFill(NColor.FromColor(NColor.SlateGray, 125));

            NScaleSection scaleSection = new NScaleSection();
            scaleSection.Range = redRange;
            scaleSection.MajorTickFill = new NColorFill(NColor.Red);
            scaleSection.MinorTickFill = new NColorFill(NColor.Red);

            NTextStyle labelStyle = new NTextStyle();
            labelStyle.Fill = new NStockGradientFill(NColor.Red, NColor.DarkRed);
            labelStyle.Font = new NFont("Arial", 11, ENFontStyle.Bold);
            scaleSection.LabelTextStyle = labelStyle;

            scale.Sections.Add(scaleSection);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="minValue"></param>
        /// <returns></returns>
        private static NRangeIndicator CreateRangeIndicator(double minValue)
        {
            NRangeIndicator rangeIndicator = new NRangeIndicator();

            rangeIndicator.Value = minValue;
            rangeIndicator.OriginMode = ENRangeIndicatorOriginMode.ScaleMax;
            rangeIndicator.Fill = new NColorFill(NColor.Red);
            rangeIndicator.Stroke.Width = 0;
            rangeIndicator.BeginWidth = 2;
            rangeIndicator.EndWidth = 10;

            return rangeIndicator;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private double GetSpeedFromRotation()
        {
            switch (m_Gear)
            {
                case 1:
                    return m_Rpm * 0.005;
                case 2:
                    return m_Rpm * 0.01;
                case 3:
                    return m_Rpm * 0.015;
                case 4:
                    return m_Rpm * 0.02;
                case 5:
                    return m_Rpm * 0.034;
            }

            return 0;
        }

        private double GetRotationFromSpeed()
        {
            switch (m_Gear)
            {
                case 1:
                    return m_Speed / 0.005;
                case 2:
                    return m_Speed / 0.01;
                case 3:
                    return m_Speed / 0.015;
                case 4:
                    return m_Speed / 0.02;
                case 5:
                    return m_Speed / 0.034;
            }

            return 0;
        }

        private void OnTimerTick()
        {
            m_Rpm += m_AccelerationScrollBar.Value;

            if (m_Rpm < 0)
            {
                m_Rpm = 0;
            }
            else if (m_Rpm > 7000)
            {
                m_Rpm = 7000;
            }

            m_Speed = GetSpeedFromRotation();
            m_SpeedIndicator.Value = m_Speed;
            m_SpeedLedDisplay.Value = m_Speed;
            m_SpeedStateIndicator.Value = m_Speed;

            // change gear
            if (m_Rpm < 1000)
            {
                m_Gear--;
            }
            else if (m_Rpm > 3500)
            {
                m_Gear++;
            }

            if (m_Gear < 1)
                m_Gear = 1;
            if (m_Gear > 5)
                m_Gear = 5;

            m_Rpm = GetRotationFromSpeed();
            m_RpmIndicator.Value = m_Rpm;
            m_RpmLedDisplay.Value = m_Rpm;
            m_RpmStateIndicator.Value = m_Rpm;
        }

        #endregion

        #region Fields

        double m_Rpm;
        double m_Speed;
        int m_Gear;

        NTimer m_Timer;
        NHScrollBar m_AccelerationScrollBar;

        NNeedleValueIndicator m_RpmIndicator;
        NNeedleValueIndicator m_SpeedIndicator;

        NNumericLedDisplay m_RpmLedDisplay;
        NNumericLedDisplay m_SpeedLedDisplay;

        NStateIndicator m_RpmStateIndicator;
        NStateIndicator m_SpeedStateIndicator;

        #endregion

        #region Schema

        public static readonly NSchema NCarDashboardExampleSchema;

        #endregion

        #region Constants

        private static readonly NSize defaultRadialGaugeSize = new NSize(300, 300);
        private static readonly NSize stateIndicatorSize = new NSize(10, 10);

        #endregion
    }
}
