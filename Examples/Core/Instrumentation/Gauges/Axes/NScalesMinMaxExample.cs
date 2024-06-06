using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Formulas;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;
using System;
using System.ComponentModel;
using System.Reflection;

namespace Nevron.Nov.Examples.Gauge
{
    /// <summary>
	///  This exaple demonstrates how to set the min, max and interval values of a scale.
    /// </summary>
	public class NScalesMinMaxExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public NScalesMinMaxExample()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        static NScalesMinMaxExample()
        {
            NScalesMinMaxExampleSchema = NSchema.Create(typeof(NScalesMinMaxExample), NExampleBaseSchema);
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
            m_RadialGauge.Dial = new NDial(ENDialShape.Circle, new NEdgeDialRim());

            NAdvancedGradientFill advancedGradient = new NAdvancedGradientFill();
            advancedGradient.BackgroundColor = NColor.BlueViolet;
            advancedGradient.Points.Add(new NAdvancedGradientPoint(NColor.MediumPurple, new NAngle(180, NUnit.Degree), 0.2f, 4, 0.7f, ENAdvancedGradientPointShape.Circle));
            m_RadialGauge.Dial.BackgroundFill = advancedGradient;
            m_RadialGauge.CapEffect = new NGlassCapEffect(ENCapEffectShape.Ellipse);

            m_RadialGauge.SweepAngle = new NAngle(280, NUnit.Degree);
            m_RadialGauge.BeginAngle = new NAngle(120, NUnit.Degree);

            // configure scale
            NGaugeAxis axis = new NGaugeAxis();
            m_RadialGauge.Axes.Clear();
            m_RadialGauge.Axes.Add(axis);

            m_Scale = (NNumericScale)axis.Scale;
            m_Scale.SetPredefinedScale(ENPredefinedScaleStyle.Presentation);

            m_Scale.Ruler.Fill = new NColorFill(NColor.FromColor(NColor.Transparent, 0.4f));
            m_Scale.OuterMajorTicks.Fill = new NColorFill(NColor.White);
            m_Scale.Labels.Style.TextStyle.Font = new NFont("Arial", 12, ENFontStyle.Bold);
            m_Scale.Labels.Style.TextStyle.Fill = new NColorFill(NColor.White);
            m_Scale.MajorTickMode = ENMajorTickMode.CustomStep;
            m_Scale.CustomStep = 10;
            m_Scale.MinorTickCount = 4;
            m_Scale.ViewRangeInflateMode = ENScaleViewRangeInflateMode.None;
            //TODO: interval offset 

            // needle value indicator
            m_ValueIndicator = new NNeedleValueIndicator();
            m_ValueIndicator.Value = 79;
            m_ValueIndicator.Fill = new NStockGradientFill(ENGradientStyle.Horizontal, ENGradientVariant.Variant1, NColor.White, NColor.Red);
            m_ValueIndicator.Stroke.Color = NColor.White;
            m_ValueIndicator.EnableDampening = true;
            m_ValueIndicator.OffsetFromCenter = -10;
            m_RadialGauge.Indicators.Add(m_ValueIndicator);

            // adds radial gauge
            controlStack.Add(m_RadialGauge);

            return stack;
        }
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();

            NStackPanel propertyStack = new NStackPanel();
            stack.Add(new NUniSizeBoxGroup(propertyStack));

            // Range minimum 
            m_RangeMinimumUpDown = new NNumericUpDown();
            m_RangeMinimumUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnUpDownRangeMinimumChanged);
            m_RangeMinimumUpDown.Value = 0;
            propertyStack.Add(new NPairBox("Range Minimum:", m_RangeMinimumUpDown, true));

            // Range max 
            m_RangeMaximumUpDown = new NNumericUpDown();
            m_RangeMaximumUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnUpDownRangeMaximumChanged);
            m_RangeMaximumUpDown.Value = 120;
            propertyStack.Add(new NPairBox("Range Maximum:", m_RangeMaximumUpDown, true));

            // reversed scale check box
            m_CustomInterval = new NCheckBox("Custom Interval");
            m_CustomInterval.Checked = true;
            m_CustomInterval.CheckedChanged += new Function<NValueChangeEventArgs>(OnCustomIntervalCheckBoxValueChanged);
            propertyStack.Add(m_CustomInterval);

            // Interval - scale custom step
            m_IntervalUpDown = new NNumericUpDown();
            m_IntervalUpDown.Value = m_Scale.CustomStep;
            m_IntervalUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnUpDownIntervalChanged);
            m_IntervalUpDown.Value = Math.Min(Math.Max(m_IntervalUpDown.Value, 10), 50);
            propertyStack.Add(new NPairBox("Interval: ", m_IntervalUpDown, true));



            return stack;
        }
        protected override string GetExampleDescription()
        {
            return @"<p> This sample demonstrates how to set a scale's minimum, maximum, and interval values.Scale range is specified using minimum and maximum values, 
                        and the Minimum value can't be equal to or more than the maximum. 
                        The scale interval defines the distance between the major tick marks and the labels. </p>";
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        private void OnUpDownRangeMinimumChanged(NValueChangeEventArgs arg)
        {
            int newMinimum = (int)m_RangeMinimumUpDown.Value;

            if (newMinimum > 100)
            {
                newMinimum = 100;
            }
            else if(newMinimum <= 0)
            {
                newMinimum = 0;
            }
            UpdateAxisRange(newMinimum, (int)m_RadialGauge.Axes[0].Range.End);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        private void OnUpDownRangeMaximumChanged(NValueChangeEventArgs arg)
        {
            int newMaximum = (int)m_RangeMaximumUpDown.Value;

            if (newMaximum > 300)
            {
                newMaximum = 300;
            }
            else if (newMaximum <= 120)
            {
                newMaximum = 120;
            }
           
            ChangeFontSize(newMaximum); 
            UpdateAxisRange((int)m_RadialGauge.Axes[0].Range.Begin, newMaximum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCustomIntervalCheckBoxValueChanged(NValueChangeEventArgs arg)
        {
            NCheckBox checkbox = (NCheckBox)arg.TargetNode;
            if (!checkbox.Checked)
                m_Scale.CustomStep = 10;

            m_IntervalUpDown.Enabled = checkbox.Checked;
        }

        /// <summary>
        ///  Get the new interval value. The value should be between 10 and 50
        /// </summary>
        /// <param name="arg"></param>
        private void OnUpDownIntervalChanged(NValueChangeEventArgs arg)
        {
            int newInterval = (int)m_IntervalUpDown.Value;

            if (newInterval > 50 || newInterval <= 10)
                return;
            
            m_Scale.CustomStep = newInterval;
        }

        /// <summary> 
        /// Updates axis range of the radial gauge
        /// </summary>
        /// <param name="newMinimum"> The new minimum value for the axis range </param>
        /// <param name="newMaximum"> The new maximum value for the axis range </param>
        private void UpdateAxisRange(int newMinimum, int newMaximum)
        {
            // Check if the new minimum is greater than or equal to the new maximum
            if (newMinimum >= newMaximum)
                return;

            m_RadialGauge.Axes[0].Range = new NRange(newMinimum, newMaximum);
        }

        /// <summary>
        /// Changes the font size of the scale labels based on the new maximum value
        /// </summary>
        /// <param name="newMaximum"></param>
        private void ChangeFontSize(int newMaximum)
        {

            if (newMaximum < 150)
            {
                m_Scale.Labels.Style.TextStyle.Font = new NFont("Arial", 12, ENFontStyle.Bold);
            }
            else if (newMaximum >= 150 && newMaximum <= 200)
            {
                m_Scale.Labels.Style.TextStyle.Font = new NFont("Arial", 8, ENFontStyle.Bold);
            }
            else if (newMaximum >= 201 && newMaximum <= 300)
            {
                m_Scale.Labels.Style.TextStyle.Font = new NFont("Arial", 6, ENFontStyle.Bold);
            }
        }


        #endregion

        #region Fields

        NRadialGauge m_RadialGauge;
        NNeedleValueIndicator m_ValueIndicator;
        NNumericScale m_Scale;

        NNumericUpDown m_RangeMinimumUpDown;
        NNumericUpDown m_RangeMaximumUpDown;
        NNumericUpDown m_IntervalUpDown;
        NCheckBox m_CustomInterval;

        #endregion

        #region Schema

        public static readonly NSchema NScalesMinMaxExampleSchema;

        #endregion

        #region Constants

        private static readonly NSize defaultRadialGaugeSize = new NSize(300, 300);

        #endregion
    }
}
