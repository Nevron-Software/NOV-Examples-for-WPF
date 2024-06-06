using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;
using System;

namespace Nevron.Nov.Examples.Gauge
{
    /// <summary>
    /// 
    /// </summary>
    public class NScaleMultiplierExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public NScaleMultiplierExample()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        static NScaleMultiplierExample()
        {
            NScaleMultiplierExampleSchema = NSchema.Create(typeof(NScaleMultiplierExample), NExampleBaseSchema);
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
            m_RadialGauge.Dial = new NDial(ENDialShape.Circle, new NEdgeDialRim());
            m_RadialGauge.SweepAngle = new NAngle(270, NUnit.Degree);
            m_RadialGauge.BeginAngle = new NAngle(-225, NUnit.Degree);

            NAdvancedGradientFill advancedGradient = new NAdvancedGradientFill();
            advancedGradient.BackgroundColor = NColor.Black;
            advancedGradient.Points.Add(new NAdvancedGradientPoint(NColor.CadetBlue, new NAngle(10, NUnit.Degree), 0.1f, 0, 1.0f, ENAdvancedGradientPointShape.Circle));
            m_RadialGauge.Dial.BackgroundFill = advancedGradient;
            m_RadialGauge.CapEffect = new NGlassCapEffect(ENCapEffectShape.Ellipse);

            controlStack.Add(m_RadialGauge);

            // configure scale
            NGaugeAxis axis = new NGaugeAxis();
            m_RadialGauge.Axes.Add(axis);
            axis.Range = new NRange(0, 10000);

            // add Scale
            m_Scale = (NLinearScale)axis.Scale;
            m_Scale.SetPredefinedScale(ENPredefinedScaleStyle.Presentation);
            m_Scale.Labels.Style.TextStyle.Font = new NFont("Arimo", 10, ENFontStyle.Bold);
            m_Scale.Labels.Style.TextStyle.Fill = new NColorFill(NColor.White);
            m_Scale.MajorTickMode = ENMajorTickMode.CustomStep;
            m_Scale.CustomStep = 1000;
            m_Scale.MinorTickCount = 10;
            m_Scale.Ruler.Fill = new NColorFill(NColor.Gray);
       
            // add radial gauge indicators
            m_ValueIndicator = new NNeedleValueIndicator();
            m_ValueIndicator.Fill = new NStockGradientFill(ENGradientStyle.Horizontal, ENGradientVariant.Variant1, NColor.White, NColor.Red);
            m_ValueIndicator.Width = 7;
            m_ValueIndicator.OffsetFromScale = -10;
            m_RadialGauge.Indicators.Add(m_ValueIndicator);

            NStackPanel verticalStack = new NStackPanel();
            verticalStack.Direction = Layout.ENHVDirection.TopToBottom;
            verticalStack.Padding = new NMargins(80, 230, 80, 0);

            // add Numeric Led dusplay 
            m_NumericLedDisplay = new NNumericLedDisplay();
            m_NumericLedDisplay.CellCountMode = ENDisplayCellCountMode.Fixed;
            m_NumericLedDisplay.CellCount = 7;
            m_NumericLedDisplay.BackgroundFill = new NColorFill(NColor.Black);
            m_NumericLedDisplay.Border = NBorder.CreateSunken3DBorder(new NUIThemeColorMap(ENUIThemeScheme.Brick));
            m_NumericLedDisplay.BorderThickness = new NMargins(6);
            m_NumericLedDisplay.Margins = new NMargins(5);
            m_NumericLedDisplay.VerticalPlacement = ENVerticalPlacement.Bottom;
            m_NumericLedDisplay.PreferredSize = new NSize(30, 50);

            verticalStack.Add(m_NumericLedDisplay);

            m_RadialGauge.Content = verticalStack;

            // timer 
            m_DataFeedTimer = new NTimer();
            m_DataFeedTimer.Tick += new Function(OnDataFeedTimerTick);
            m_DataFeedTimer.Start();

            return stack;
        }

        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();

            m_MultiplierRadioGroup = new NRadioButtonGroup();
            m_MultiplierRadioGroup.SelectedIndexChanged += OnMultiplierRadioGroupIndexChanged;

            NStackPanel radioStack = new NStackPanel();
            m_MultiplierRadioGroup.Content = radioStack;

            radioStack.Add(new NRadioButton("10"));
            radioStack.Add(new NRadioButton("1"));
            radioStack.Add(new NRadioButton("0.1"));
            radioStack.Add(new NRadioButton("0.01")); 
            radioStack.Add(new NRadioButton("0.001"));
            ((NRadioButton)radioStack[1]).Checked = true;
            stack.Add(new NGroupBox("Multiplier Value: ", m_MultiplierRadioGroup));

            return stack;
        }

     
        protected override string GetExampleDescription()
        {
            return @"<p> An RPM gauge is displayed using a mlutiplier, which allows for the increase or decrease in scale labels.</p>";
        }

        #endregion

        #region Event Handlers

        private void OnMultiplierRadioGroupIndexChanged(NValueChangeEventArgs arg)
        {
            int selectedIndex = m_MultiplierRadioGroup.SelectedIndex;

            switch (selectedIndex)
            {
                case 0:
                    // handle "10" radio button
                    m_RadialGauge.Axes[0].Range = new NRange(0, 100000);
                    m_Scale.CustomStep = 10000;

                    // create a scale title  - RPM X 10
                    m_RadialGauge.Axes[0].Scale.Title.Text = "RPM X 10";
                    m_RadialGauge.Axes[0].Scale.Title.TextStyle = new NTextStyle(NColor.Red);
                    break;

                case 1:
                    // handle "1" radio button
                    m_RadialGauge.Axes[0].Range = new NRange(0, 10000);
                    m_Scale.CustomStep = 1000;

                    // create a scale title  - RPM 
                    m_RadialGauge.Axes[0].Scale.Title.Text = "RPM";
                    m_RadialGauge.Axes[0].Scale.Title.TextStyle = new NTextStyle(NColor.Red);
                    break;

                case 2:
                    // handle "0.1" radio button
                    m_RadialGauge.Axes[0].Range = new NRange(0, 1000);
                    m_Scale.CustomStep = 100;
                    
                    // create a scale title  - RPM X 0.1 
                    m_RadialGauge.Axes[0].Scale.Title.Text = "RPM X 0.1";
                    m_RadialGauge.Axes[0].Scale.Title.TextStyle = new NTextStyle(NColor.Red);
                    break;

                case 3:
                    // handle "0.01" radio button
                    m_RadialGauge.Axes[0].Range = new NRange(0, 100);
                    m_Scale.CustomStep = 10;

                    // create a scale title  - RPM X 0.01 
                    m_RadialGauge.Axes[0].Scale.Title.Text = "RPM X 0.01";
                    m_RadialGauge.Axes[0].Scale.Title.TextStyle = new NTextStyle(NColor.Red);
                    break;

                case 4:
                    // handle "0.001" radio button
                    m_RadialGauge.Axes[0].Range = new NRange(0, 10);
                    m_Scale.CustomStep = 1;

                    // create a scale title  - RPM X 0.001
                    m_RadialGauge.Axes[0].Scale.Title.Text = "RPM X 0.001";
                    m_RadialGauge.Axes[0].Scale.Title.TextStyle = new NTextStyle(NColor.Red);
                    break;
            }
        }

        private void OnDataFeedTimerTick()
        {
            int selectedIndex = m_MultiplierRadioGroup.SelectedIndex;

           // update the indicator
           m_FirstIndicatorAngle += 0.02;
           double value = 50.0 - Math.Cos(m_FirstIndicatorAngle) * 50.0;
           m_ValueIndicator.Value = value;

            switch (selectedIndex)
            {
                case 0:
                    // handle "10" radio button
                    m_ValueIndicator.Value = value * 1000;
                    m_NumericLedDisplay.Value = m_ValueIndicator.Value;
                    break;

                case 1:
                    // handle "1" radio button
                    m_ValueIndicator.Value = value * 100;
                    m_NumericLedDisplay.Value = m_ValueIndicator.Value;
                    break;

                case 2:
                    // handle "0.1" radio button
                    m_ValueIndicator.Value = value * 10;
                    m_NumericLedDisplay.Value = m_ValueIndicator.Value;
                    break;

                case 3:
                    // handle "0.01" radio button
                    m_ValueIndicator.Value = value;
                    m_NumericLedDisplay.Value = m_ValueIndicator.Value;
                    break;

                case 4:
                    // handle "0.001" radio button
                    m_ValueIndicator.Value = value * 0.1;
                    m_NumericLedDisplay.Value = m_ValueIndicator.Value;
                    break;
            }
        }

        #endregion

        #region Implementation


        #endregion

        #region Fields

        NRadialGauge m_RadialGauge;
        NLinearScale m_Scale;
        NNeedleValueIndicator m_ValueIndicator;

        NNumericLedDisplay m_NumericLedDisplay;
        NRadioButtonGroup m_MultiplierRadioGroup;
       
        NTimer m_DataFeedTimer;
        double m_FirstIndicatorAngle;

        #endregion

        #region Schema

        public static readonly NSchema NScaleMultiplierExampleSchema;

        #endregion

        #region Static Methods

        #endregion

        #region Constants

        private static readonly NSize defaultRadialGaugeSize = new NSize(300, 300);

        #endregion
    }
}