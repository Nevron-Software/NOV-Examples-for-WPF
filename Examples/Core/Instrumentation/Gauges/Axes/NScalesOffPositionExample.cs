using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;
using System;

namespace Nevron.Nov.Examples.Gauge
{
    /// <summary>
	/// This example demonstrated how to use a minimum scale pin to define pointer's "Off Poisition"
    /// </summary>
	public class NScalesOffPositionExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public NScalesOffPositionExample()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        static NScalesOffPositionExample()
        {
            NScalesOffPositionExampleSchema = NSchema.Create(typeof(NScalesOffPositionExample), NExampleBaseSchema);
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
            controlStack.Padding = new NMargins(20);
            stack.Add(controlStack);

            // create the radial gauge
            m_RadialGauge = new NRadialGauge();
            m_RadialGauge.PreferredSize = defaultRadialGaugeSize;
            m_RadialGauge.NeedleCap.Visible = true;
            m_RadialGauge.NeedleCap.Size = new NSize(30, 30);

            m_RadialGauge.SweepAngle = new NAngle(270, NUnit.Degree);
            m_RadialGauge.BeginAngle = new NAngle(135, NUnit.Degree);

            controlStack.Add(m_RadialGauge);

            // add axis 
            NGaugeAxis axis = new NGaugeAxis();
            m_RadialGauge.Axes.Clear();
            m_RadialGauge.Axes.Add(axis);
            axis.Range = new NRange(-10, 100);
            axis.Anchor = new NDockGaugeAxisAnchor(ENGaugeAxisDockZone.Top, true, ENScaleOrientation.Left, 0.0f, 100.0f);

            // add scale
            m_Scale = (NLinearScale)axis.Scale;
            m_Scale.SetPredefinedScale(ENPredefinedScaleStyle.PresentationNoStroke);
            m_Scale.Labels.Style.TextStyle.Font = new NFont("Microsoft Sans Serif", 15, ENFontStyle.Bold);
            m_Scale.Labels.Style.TextStyle.Fill = new NColorFill(NColor.CornflowerBlue);
          
            m_Scale.MajorTickMode = ENMajorTickMode.CustomStep;
            m_Scale.CustomStep = 10;
            m_Scale.OuterMinorTicks.Visible = true;
            m_Scale.OuterMajorTicks.Length = 10;
            m_Scale.OuterMajorTicks.Width = 7;
            m_Scale.OuterMajorTicks.Fill = new NColorFill(NColor.CornflowerBlue);
            m_Scale.MinorTickCount = 5;
            m_Scale.OuterMinorTicks.Length = 4;
            m_Scale.OuterMinorTicks.Width = 3;
            m_Scale.OuterMinorTicks.Fill = new NColorFill(NColor.Gray);

            // create a custom label "Off"
            NCustomValueLabel label = new NCustomValueLabel(-10, "Off");
            label.LabelStyle.ZOrder = 1;
            m_Scale.CustomLabelOverlapResolveLayouts = new NDomArray<ENLevelLabelsLayout>(new ENLevelLabelsLayout[] { ENLevelLabelsLayout.RemoveOverlap });
            m_Scale.CreateNewLevelForCustomLabels = false;
            m_Scale.CustomLabels.Add(label);
        
            // needle value 
            m_NeedleIndicator = new NNeedleValueIndicator();
            m_NeedleIndicator.ScaleAxis = axis;
            m_NeedleIndicator.Fill = new NColorFill(NColor.CornflowerBlue);
            m_NeedleIndicator.EnableDampening = true;
            m_RadialGauge.Indicators.Add(m_NeedleIndicator);

            // Timer
            m_Timer = new NTimer();
            m_Timer.Interval = 200;
            m_Timer.Tick += new Function(OnTimerTick);
            m_Timer.Start();

            return stack;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();

            NStackPanel propertyStack = new NStackPanel();
            stack.Add(new NUniSizeBoxGroup(propertyStack));

            NButton toggleTimerButton = new NButton("Turn off");
            toggleTimerButton.Click += OnToggleTimerButtonClick;
            toggleTimerButton.Tag = 1;
            stack.Add(toggleTimerButton);

            return stack;
        }

        protected override string GetExampleDescription()
        {
            return @"<p>This example demonstrates how to use a minimum scale pin to define a pointer's off position.</p>";
        }

        #endregion

        #region Event Handlers
        private void OnToggleTimerButtonClick(NEventArgs arg)
        {
            NButton button = (NButton)arg.TargetNode;
            if ((int)button.Tag == 0)
            {
                m_Timer.Start();

                button.Content = new NLabel("Turn On");
                button.Tag = 1;
            }
            else
            {
                m_Timer.Stop();
                
                button.Content = new NLabel("Turn Off");
                button.Tag = 0;

                m_NeedleIndicator.Value = -10;
            }
        }
        private void OnTimerTick()
        {
            // update the indicator 
            m_FirstIndicatorAngle += Math.PI / 180.0;
            double value = 50 + Math.Sin(m_FirstIndicatorAngle) * (20 + rand.Next(30));

            m_NeedleIndicator.Value = value;
        }
      
        #endregion

        #region Fields

        NRadialGauge m_RadialGauge;
        NLinearScale m_Scale;
        NNeedleValueIndicator m_NeedleIndicator;

        NTimer m_Timer;

        private Random rand = new Random();
        double m_FirstIndicatorAngle;

        #endregion

        #region Schema

        public static readonly NSchema NScalesOffPositionExampleSchema;

        #endregion

        #region Constants

        private static readonly NSize defaultRadialGaugeSize = new NSize(300, 300);

        #endregion
    }
}
