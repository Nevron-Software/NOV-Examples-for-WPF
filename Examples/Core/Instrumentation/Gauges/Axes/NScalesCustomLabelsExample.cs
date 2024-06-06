using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;
using System;

namespace Nevron.Nov.Examples.Gauge
{
    /// <summary>
	/// This example demonstrates how to add a custom labels to a scale.
    /// </summary>
	public class NScalesCustomLabelsExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public NScalesCustomLabelsExample()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        static NScalesCustomLabelsExample()
        {
            NScalesCustomLabelsExampleSchema = NSchema.Create(typeof(NScalesCustomLabelsExample), NExampleBaseSchema);
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
            m_RadialGauge.Dial = new NDial(ENDialShape.CutCircle, new NEdgeDialRim());
            m_RadialGauge.NeedleCap.Visible = false;
            controlStack.Add(m_RadialGauge);

            // add axis 
            NGaugeAxis axis = new NGaugeAxis();
            m_RadialGauge.Axes.Clear();
            m_RadialGauge.Axes.Add(axis);
            axis.Range = new NRange(0, 20);
            axis.Anchor = new NDockGaugeAxisAnchor(ENGaugeAxisDockZone.Top, true, ENScaleOrientation.Left, 2f, 98f);

            // scale
            m_RadialGauge.SweepAngle = new NAngle(180, NUnit.Degree);
            m_RadialGauge.BeginAngle = new NAngle(-180, NUnit.Degree);

            // needle value 
            m_NeedleIndicator = new NNeedleValueIndicator();
            m_NeedleIndicator.ScaleAxis = axis;
            m_NeedleIndicator.Width = 12;
            m_NeedleIndicator.Fill = new NColorFill(NColor.Red);
            m_NeedleIndicator.OffsetFromScale = 15;
            m_RadialGauge.Indicators.Add(m_NeedleIndicator);

            // Timer
            m_Timer = new NTimer();
            m_Timer.Interval = 800;
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

            m_LabelsTypeRadioGroup = new NRadioButtonGroup();
            m_LabelsTypeRadioGroup.SelectedIndexChanged += OnLabelsTypeRadioGroupIndexChanged;
            
            NStackPanel radioStack = new NStackPanel();
            m_LabelsTypeRadioGroup.Content = radioStack;

            radioStack.Add(new NRadioButton("Standart Labels"));
            radioStack.Add(new NRadioButton("Custom Labels"));
            radioStack.Add(new NRadioButton("Both"));

            m_LabelsTypeRadioGroup.SelectedIndex = 0;

            ((NRadioButton)radioStack[0]).Checked = true;
            stack.Add(m_LabelsTypeRadioGroup);
         
            return stack;
        }
              
        protected override string GetExampleDescription()
        {
            return @"<p>This example demonstrates how to use a minimum scale pin to define a pointer's off position.</p>";
        }

        #endregion

        #region Event Handlers

        private void OnLabelsTypeRadioGroupIndexChanged(NValueChangeEventArgs arg)
        {
            int selectedIndex = m_LabelsTypeRadioGroup.SelectedIndex;

            // scale
            NLinearScale scale =  new NLinearScale();
            m_RadialGauge.Axes[0].Scale = scale;

            scale.SetPredefinedScale(ENPredefinedScaleStyle.Presentation);
            scale.Labels.Style.TextStyle.Font = new NFont("Microsoft Sans Serif", 15, ENFontStyle.Bold);
            scale.Labels.Style.TextStyle.Fill = new NColorFill(NColor.Black);

            scale.MajorTickMode = ENMajorTickMode.AutoMaxCount;
            scale.OuterMinorTicks.Visible = true;
            scale.OuterMajorTicks.Width = 7;
            scale.OuterMajorTicks.Fill = new NColorFill(NColor.Black);
            scale.MinorTickCount = 3;
            scale.OuterMinorTicks.Fill = new NColorFill(NColor.Gray);

            scale.Title.Text = "GAS";
            scale.Title.TextStyle = new NTextStyle(NColor.Red);
            scale.Title.RulerAlignment = ENHorizontalAlignment.Center;
            scale.Title.ContentAlignment = ENContentAlignment.BottomCenter;

            switch (selectedIndex)
            {
                case 0: // Standard labels
                    break;

                case 1: // Custom labels
                    scale.Labels.Visible = false;
                    scale.CreateNewLevelForCustomLabels = true;
                    AddCustomLabels(scale);
                    break;

                case 2: // Both
                    // add custom labels
                    scale.CreateNewLevelForCustomLabels = false;
                    AddCustomLabels(scale);
                    break;
            }
        }
        /// <summary>
        /// Timer tick event
        /// </summary>
        private void OnTimerTick()
        {
            // update the indicator 
            m_FirstIndicatorAngle += 0.02;
            double value = 50.0 - Math.Cos(m_FirstIndicatorAngle) * 50.0;
            m_NeedleIndicator.Value = value;
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Adds custom labels to the scle
        /// </summary>
        /// <param name="scale"></param>
        private void AddCustomLabels(NLinearScale scale)
        {
            scale.CustomLabelOverlapResolveLayouts = new NDomArray<ENLevelLabelsLayout>(new ENLevelLabelsLayout[] { ENLevelLabelsLayout.RemoveOverlap, ENLevelLabelsLayout.AutoScale });

            scale.CustomLabels.Add(CreateCustomLabel(0, "E"));
            scale.CustomLabels.Add(CreateCustomLabel(10, "1/2"));
            scale.CustomLabels.Add(CreateCustomLabel(20, "F"));
        }
        /// <summary>
        /// Creates a custom label give the specified value and text
        /// </summary>
        /// <param name="value"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        private NCustomValueLabel CreateCustomLabel(double value, string text)
        {
            NCustomValueLabel label = new NCustomValueLabel(value, text);

            label.LabelStyle.ZOrder = 1;
            label.LabelStyle.TextStyle.Font = new NFont("Microsoft Sans Serif", 15, ENFontStyle.Bold);
            label.LabelStyle.TextStyle.Fill = new NColorFill(NColor.Black);

            return label;
        }

        #endregion

        #region Fields

        NRadialGauge m_RadialGauge;
        NNeedleValueIndicator m_NeedleIndicator;

        NRadioButtonGroup m_LabelsTypeRadioGroup;

        NTimer m_Timer;
        double m_FirstIndicatorAngle;

        #endregion

        #region Schema
         
        public static readonly NSchema NScalesCustomLabelsExampleSchema;

        #endregion

        #region Constants

        private static readonly NSize defaultRadialGaugeSize = new NSize(300, 300);

        #endregion
    }
}
