using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;
using System;

namespace Nevron.Nov.Examples.Gauge
{
    /// <summary>
    /// This example demonstrates how to control the size of the gauge axes
    /// </summary>
    public class NAutoAdjustScaleExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public NAutoAdjustScaleExample()
        {

        }
        /// <summary>
        /// 
        /// </summary>

        static NAutoAdjustScaleExample()
        {
            NAutoAdjustScaleExampleSchema = NSchema.Create(typeof(NAutoAdjustScaleExample), NExampleBaseSchema);
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
            m_RadialGauge.Dial.BackgroundFill = new NStockGradientFill(NColor.Black, NColor.Gray);

            NGelCapEffect gelEffect = new NGelCapEffect(ENCapEffectShape.Ellipse);
            gelEffect.Margins = new NMargins(0, 0, 0, 0.5);

            m_RadialGauge.Axes.Clear();

            // create the radial gauge
            m_RadialGauge.SweepAngle = new NAngle(180, NUnit.Degree);
            m_RadialGauge.BeginAngle = new NAngle(32, NUnit.Degree);

            // create the first axis
            NGaugeAxis axis1 = new NGaugeAxis();
            axis1.Anchor = new NDockGaugeAxisAnchor(ENGaugeAxisDockZone.Top, true, 30,180);
            
            m_RadialGauge.Axes.Add(axis1);

            //Cache the initial begin and end
            m_CurBegin = m_RadialGauge.Axes[0].Range.Begin;
            m_CurEnd = m_RadialGauge.Axes[0].Range.End;

            m_InitBegin =  m_CurBegin;
            m_InitEnd = m_CurEnd;

            // scale 
            m_Scale = (NStandardScale)axis1.Scale;
            m_Scale.SetPredefinedScale(ENPredefinedScaleStyle.PresentationNoStroke);
            m_Scale.MajorTickMode = ENMajorTickMode.AutoMaxCount;
            m_Scale.MaxTickCount = 9;
            m_Scale.MinorTickCount = 3;
            m_Scale.Ruler.Fill = new NColorFill(NColor.FromColor(NColor.White, 0.4f));
            m_Scale.OuterMajorTicks.Fill = new NColorFill(NColor.Orange);            
            m_Scale.Labels.Style.TextStyle.Font = new NFont("Arimo", 12, ENFontStyle.Bold);
            m_Scale.Labels.Style.TextStyle.Fill = new NColorFill(NColor.White);
            m_Scale.Labels.Style.Angle = new NScaleLabelAngle(ENScaleLabelAngleMode.Scale, 0);
            m_Scale.ViewRangeInflateMode = ENScaleViewRangeInflateMode.None;

            // Value Indicator
            m_NeedleValueIndicator = new NNeedleValueIndicator();
            m_NeedleValueIndicator.Fill = new NStockGradientFill(ENGradientStyle.Horizontal, ENGradientVariant.Variant1, NColor.White, NColor.Red);
            m_NeedleValueIndicator.Stroke.Color = NColor.Red;
            m_NeedleValueIndicator.OffsetFromScale = 2;
            m_NeedleValueIndicator.Shape = ENNeedleShape.Triangle;
            m_RadialGauge.SweepAngle = new NAngle(360, NUnit.Degree);

            m_RadialGauge.Indicators.Add(m_NeedleValueIndicator);

            // Timer
            m_Timer = new NTimer();
            m_Timer.Interval = 200;
            m_Timer.Tick += new Function(OnTimerTick);
            m_Timer.Start();

            m_Range = 100;

            // radio buttons
            m_StepAndRotateScaleRadioButton = new NRadioButton("Step and Rotate Scale");
            m_ContinuouslyRotateRadioButton = new NRadioButton("Continuously Rotate");
            m_ContinuouslyRotateRadioButton.Checked = true;

            return stack;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();

            NRadioButtonGroup dynamicScaleRotationStylesGroup = new NRadioButtonGroup(stack);
            dynamicScaleRotationStylesGroup.SelectedIndex = 0;
            
            stack.Add(m_StepAndRotateScaleRadioButton);
            stack.Add(m_ContinuouslyRotateRadioButton);

            return dynamicScaleRotationStylesGroup;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string GetExampleDescription()
        {
            return @"<p> </p>";
        }

        #endregion

        #region Event Handlers        
       
        private void OnTimerTick()
        {
            if (m_ContinuouslyRotateRadioButton.Checked)
            {
                // code to execute when the "Continuously Rotate" radio button is selected
                //Continuously change the scale minimum.                
                double val = ChangeRange();

                NGaugeAxis axis = m_RadialGauge.Axes[0];
                axis.Range = new NRange(val, val + 100);

                m_NeedleValueIndicator.Value = (axis.Range.End + axis.Range.Begin) / 2;
            }

            else if (m_StepAndRotateScaleRadioButton.Checked)
            {
                // TODO: 
                SetNeedleValueWithinRange(m_CurBegin, m_CurEnd);
            }
        }

        private double ChangeRange()
        {
            if (m_Increase)
                m_Range++;
            else
                m_Range--;

            if(m_Range >= 80)
                m_Increase = false;

            if (m_Range <= 20)
                m_Increase = true;
                        
            return m_Range;
        }

        /// <summary>
        /// Randomly adjust the axis range with range of 50 and step 20.
        /// </summary>
        private void AdjustRandomRange(bool forwrad)
        {
            // move the range depending on forward parameter
            if (forwrad)
            {
                m_CurBegin += 2;
                m_CurEnd += 2;
            }
            else
            {
                m_CurBegin -= 2;
                m_CurEnd -= 2;
            }

            // if range is moving forward and current begin is increased with 30 reset the range and change the direction.
            if ((forwrad && m_CurEnd - 30 == m_InitEnd) ||
                (!forwrad && m_CurBegin + 30 == m_InitBegin) ||
                (m_CurBegin == 0) || (m_CurBegin == 200))
            {
                m_InitEnd = m_CurEnd;
                m_InitBegin = m_CurBegin;
                m_Direction *= -1;                
            }

            m_RadialGauge.Axes[0].Range = new NRange(m_CurBegin, m_CurEnd);
        }


        /// <summary>
        /// The m_StepCounter tracks  how many steps the needle has taken, and the m_StepThreshold  
        /// determines how many steps should be taken before the range is adjusted. 
        /// </summary>
        /// <param name="m_CurBegin"></param>
        /// <param name="m_CurEnd"></param>
        private void SetNeedleValueWithinRange(double m_CurBegin, double m_CurEnd)
        {
            m_FirstIndicatorAngle += Math.PI / 180;
            double needleValue = 20 + Math.Sin(m_FirstIndicatorAngle) + (20 + rand.Next(50));
            m_NeedleValueIndicator.EnableDampening = true;
            m_NeedleValueIndicator.Value = needleValue;

            // m_StepCounter++;

            //if (m_StepCounter >= m_StepThreshold)
            //{
            //    AdjustRandomRange();
            //    m_StepCounter = 0;
            //}

            bool direction = m_Direction == 1 ? true : false;

            if (m_MoveRange)
                AdjustRandomRange(direction);

            // 
            if ((direction && needleValue > m_CurEnd - 30) ||
                    (!direction && needleValue < m_CurBegin + 30))
                m_MoveRange = true;
            else
                m_MoveRange = false;

            //if (needleValue > m_CurEnd)
            //{
            //    m_StepCounter++;

            //    if (m_StepCounter >= m_StepThreshold)
            //    {
            //        m_CurBegin += 2;
            //        m_CurEnd += 2;
            //        m_RadialGauge.Axes[0].Range = new NRange(m_CurBegin, m_CurEnd);
            //        m_StepCounter = 0;
            //    }
            //}
            //else if (needleValue < m_CurBegin)
            //{
            //    m_StepCounter++;
            //    if (m_StepCounter >= m_StepThreshold)
            //    {
            //        m_CurBegin -= 2;
            //        m_CurEnd -= 2;
            //        m_RadialGauge.Axes[0].Range = new NRange(m_CurBegin, m_CurEnd);
            //        m_StepCounter = 0;
            //    }
            //}

            //     Debug.WriteLine("Step Count " + m_StepCounter, "needle Value " + needleValue + "range begin " + m_CurBegin + "range end " + m_CurEnd);

        }

        #endregion

        #region Fields

        NRadialGauge m_RadialGauge;
        NNeedleValueIndicator m_NeedleValueIndicator;
        NStandardScale m_Scale;
        
        NTimer m_Timer;
        NRadioButton m_StepAndRotateScaleRadioButton;
        NRadioButton m_ContinuouslyRotateRadioButton;

        #endregion

        #region Schema

        public static readonly NSchema NAutoAdjustScaleExampleSchema;

        #endregion

        #region Constants

        private static readonly NSize defaultRadialGaugeSize = new NSize(300, 300);
        private Random rand = new Random();
        private double m_CurBegin;
        private double m_CurEnd;

        private double m_InitBegin;
        private double m_InitEnd;

        double m_Range;
        double m_FirstIndicatorAngle;
        const double c_ScaleRange = 50;
        const double c_RotateIncrement = 2;
        bool m_Increase = true;
        int m_Direction = 1;

        bool m_MoveRange = false;
        #endregion
    }
}
