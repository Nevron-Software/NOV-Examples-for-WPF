using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Gauge
{
    /// <summary>
    /// This example demonstrates properties relating to the major and minor tick marks
    /// </summary>
    public class NScaleTickMarksExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public NScaleTickMarksExample()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        static NScaleTickMarksExample()
        {
            NScaleTickMarksExampleSchema = NSchema.Create(typeof(NScaleTickMarksExample), NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            NStackPanel stack = new NStackPanel();

            NStackPanel controlStack = new NStackPanel();
            controlStack.Direction = ENHVDirection.LeftToRight;
            stack.Add(controlStack);
           
            // create the radial gauge
            m_RadialGauge = new NRadialGauge();
            controlStack.Add(m_RadialGauge);

            m_RadialGauge.CapEffect = new NGlassCapEffect();
            m_RadialGauge.Dial = new NDial(ENDialShape.Circle, new NEdgeDialRim());
            m_RadialGauge.PreferredSize = defaultRadialGaugeSize;
            m_RadialGauge.SweepAngle = new NAngle(280, NUnit.Degree);
            m_RadialGauge.BeginAngle = new NAngle(120, NUnit.Degree);

            NAdvancedGradientFill gradientFill = new NAdvancedGradientFill();
            gradientFill.BackgroundColor = NColor.DarkBlue;
            gradientFill.Points.Add(new NAdvancedGradientPoint(NColor.DeepSkyBlue, new NAngle(10, NUnit.Degree), 0.1f, 0, 1.0f, ENAdvancedGradientPointShape.Circle));
            m_RadialGauge.Dial.BackgroundFill = gradientFill;
            m_RadialGauge.CapEffect = new NGlassCapEffect(ENCapEffectShape.Ellipse);

            m_RadialGauge.Dial.BackgroundFill = gradientFill;
            m_RadialGauge.CapEffect = new NGlassCapEffect(ENCapEffectShape.Region);

            // add axis 
            NGaugeAxis axis = new NGaugeAxis();
            m_RadialGauge.Axes.Clear();
            m_RadialGauge.Axes.Add(axis);

            // add scale
            m_Scale = (NStandardScale)axis.Scale;
            m_Scale.SetPredefinedScale(ENPredefinedScaleStyle.PresentationNoStroke);
            m_Scale.Ruler.Fill = new NColorFill(NColor.FromColor(NColor.Transparent, 0.3f));
            m_Scale.Labels.Style.TextStyle.Font = new NFont("Microsoft Sans Serif", 8, ENFontStyle.Bold);
            m_Scale.Labels.Style.TextStyle.Fill = new NColorFill(NColor.DarkOrange);
            m_Scale.MajorTickMode = ENMajorTickMode.AutoMaxCount;
            m_Scale.OuterMinorTicks.Visible = true;
            m_Scale.MinorTickCount = 5;
        
            // add needle indicators
            NNeedleValueIndicator needle = new NNeedleValueIndicator(60);
            needle.OffsetOriginMode = ENIndicatorOffsetOriginMode.ScaleMiddle;
            needle.OffsetFromScale = 15;
            m_RadialGauge.Indicators.Add(needle);

            return stack;
        }
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();

            NStackPanel propertyStack = new NStackPanel();
            stack.Add(new NUniSizeBoxGroup(propertyStack));

            // major tick properties
            NGroupBox majorTicksGroupBox = new NGroupBox("Major Ticks");
            propertyStack.Add(majorTicksGroupBox);

            NStackPanel majorTicksGroupBoxGroupBoxContent = new NStackPanel();
            majorTicksGroupBox.Content = new NUniSizeBoxGroup(majorTicksGroupBoxGroupBoxContent);

            // minor tick properties
            NGroupBox minorTicksGroupBox = new NGroupBox("Minor Ticks");
            propertyStack.Add(minorTicksGroupBox);

            NStackPanel minorTicksGroupBoxContent = new NStackPanel();
            minorTicksGroupBox.Content = new NUniSizeBoxGroup(minorTicksGroupBoxContent);

            //  major tick visability checkbox 
            m_OuterMajorTicksVisibility = new NCheckBox("Major Tick Visible");
            m_OuterMajorTicksVisibility.Checked = true;
            m_OuterMajorTicksVisibility.CheckedChanged += new Function<NValueChangeEventArgs>(OnOuterMajorTicksVisibilityCheckedChanged);
            majorTicksGroupBoxGroupBoxContent.Add(m_OuterMajorTicksVisibility);

            // major ticks placement
            m_MajorTickPlacmentComboBox = new NComboBox();
            m_MajorTickPlacmentComboBox.FillFromEnum<ENTicksPlacement>();
            m_MajorTickPlacmentComboBox.SelectedIndex = (int)m_MajorTickPlacmentComboBox.SelectedIndex;
            m_MajorTickPlacmentComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnMajorTickPlacementComboBoxSelectedIndexChanged);
            majorTicksGroupBoxGroupBoxContent.Add(new NPairBox("Placement: ", m_MajorTickPlacmentComboBox, true));

            //  major ticks shape 
            m_MajorTickShapeComboBox = new NComboBox();
            m_MajorTickShapeComboBox.FillFromEnum<ENScaleValueMarkerShape>();
            m_MajorTickShapeComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnMajorTickShapeComboBoxSelectedIndexChanged);
            majorTicksGroupBoxGroupBoxContent.Add(new NPairBox("Shape: ", m_MajorTickShapeComboBox, true));

            // major ticks fill color button
            m_MajorTicksFillColorButton = new NButton("Major Ticks Fill Color");
            m_MajorTicksFillColorButton.Click += new Function<NEventArgs>(OnOuterMajorTickFillButtonClick);
            majorTicksGroupBoxGroupBoxContent.Add(m_MajorTicksFillColorButton);

            // major ticks width 
            m_MajorTicksWidthUpDown = new NNumericUpDown();
            m_MajorTicksWidthUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnUpDownMajorTickWidthChanged);
            m_MajorTicksWidthUpDown.Value = 5;
            //m_MajorTicksWidthUpDown.Value = Math.Min(Math.Max(m_MajorTicksWidthUpDown.Value, 2), 20);
            majorTicksGroupBoxGroupBoxContent.Add(new NPairBox("Width:",m_MajorTicksWidthUpDown, true));

            // major ticks length
            m_MajorTicskLengthUpDown = new NNumericUpDown();
            m_MajorTicksWidthUpDown.Value = 3;
            m_MajorTicskLengthUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnUpDownMajorTickLengthChanged);
            majorTicksGroupBoxGroupBoxContent.Add(new NPairBox("Length:", m_MajorTicskLengthUpDown, true));

            // minor ticks visible check box
            m_MinorTicksVisability = new NCheckBox("Minor Tick Visible");
            m_MinorTicksVisability.Checked = true;
            m_MinorTicksVisability.CheckedChanged += new Function<NValueChangeEventArgs>(OnMinorTicksVisibilityCheckedChanged);
            minorTicksGroupBoxContent.Add(m_MinorTicksVisability);

            // minor ticks placement
            m_MinorTickPlacmentComboBox = new NComboBox();
            m_MinorTickPlacmentComboBox.FillFromEnum<ENTicksPlacement>();
            m_MinorTickPlacmentComboBox.SelectedIndex = (int)m_MinorTickPlacmentComboBox.SelectedIndex;
            m_MinorTickPlacmentComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnMinorTickPlacementComboBoxSelectedIndexChanged);
            minorTicksGroupBoxContent.Add(new NPairBox("Placement: ", m_MinorTickPlacmentComboBox, true));

            // minor ticks Shape 
            m_MinorTickShapeComboBox = new NComboBox();
            m_MinorTickShapeComboBox.FillFromEnum<ENScaleValueMarkerShape>();
            m_MinorTickShapeComboBox.SelectedIndex = (int)m_MinorTickShapeComboBox.SelectedIndex;
            m_MinorTickShapeComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnMinorTickShapeComboBoxSelectedIndexChanged);
            minorTicksGroupBoxContent.Add(new NPairBox("Shape: ", m_MinorTickShapeComboBox, true));

            // minor ticks Fill
            m_MinorTicksFillColorButton = new NButton("Minor Ticks Fill Color");
            m_MinorTicksFillColorButton.Click += new Function<NEventArgs>(OnMinorTickFillButtonClick);
            minorTicksGroupBoxContent.Add(m_MinorTicksFillColorButton);
           
            // minor tick width 
            m_MinorTicskWidthUpDown = new NNumericUpDown();
            m_MinorTicskWidthUpDown.Value = 3;
            m_MinorTicskWidthUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnUpDownMinorTickWidthChanged);
          //  m_MinorTicskWidthUpDown.Value = Math.Min(Math.Max(m_MinorTicskWidthUpDown.Value, 2), 20);
            minorTicksGroupBoxContent.Add(new NPairBox("Width:", m_MinorTicskWidthUpDown, true));
        
            return stack;
        }

      
        protected override string GetExampleDescription()
        {
            return @"<p>Major tick marks are the primary indicators of a gauge. 
                 Minor tick marks are secondary indicators displayed in between the major tick marks.</p>";
        }

        #endregion

        #region Implementation

        #endregion

        #region Event Handlers
        private void OnOuterMajorTicksVisibilityCheckedChanged(NValueChangeEventArgs arg)
        {
            bool checkedValue = m_OuterMajorTicksVisibility.Checked;

            m_Scale.OuterMajorTicks.Visible = checkedValue;

            m_MajorTickShapeComboBox.Enabled = checkedValue;
            m_MajorTicksFillColorButton.Enabled = checkedValue;
            m_MajorTicksWidthUpDown.Enabled = checkedValue;
            m_MajorTicskLengthUpDown.Enabled = checkedValue;
            m_MajorTickPlacmentComboBox.Enabled = checkedValue;
        }

        private void OnMajorTickPlacementComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            ENTicksPlacement selectedPlacement = (ENTicksPlacement)m_MajorTickPlacmentComboBox.SelectedIndex;

        }
        private void OnMajorTickShapeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            m_Scale.OuterMajorTicks.Shape = (ENScaleValueMarkerShape)m_MajorTickShapeComboBox.SelectedIndex;
        }
        private void OnOuterMajorTickFillButtonClick(NEventArgs arg)
        {
            NEditorWindow.CreateForType(
               (NFill)m_Scale.OuterMajorTicks.Fill.DeepClone(),
               null,
               m_Scale.OuterMajorTicks.DisplayWindow,
               false,
               OnOuterMajorTicksFillEdited).Open();
        }

        private void OnOuterMajorTicksFillEdited(NFill fill)
        {
            m_Scale.OuterMajorTicks.Fill = (NFill)(fill.DeepClone());
        }

        private void OnUpDownMajorTickWidthChanged(NValueChangeEventArgs arg)
        {
            double majorTickWidth = (double)arg.NewValue;

            if (majorTickWidth >= 2 && majorTickWidth <= 20)
            {
                m_Scale.OuterMajorTicks.Width = majorTickWidth;
            }
            else
            {
                // Revert the value change
                m_MajorTicksWidthUpDown.Value = m_Scale.OuterMajorTicks.Width;
            }
        }

        private void OnUpDownMajorTickLengthChanged(NValueChangeEventArgs arg)
        {
            double majorTickLength= (double)arg.NewValue;

            if (majorTickLength >= 2 && majorTickLength <= 20)
            {
                m_Scale.OuterMajorTicks.Length = majorTickLength;
            }
            else
            {
                m_MajorTicskLengthUpDown.Value = m_Scale.OuterMajorTicks.Length;
            }
        }

        private void OnMinorTicksVisibilityCheckedChanged(NValueChangeEventArgs arg)
        {
            bool checkedValue = m_MinorTicksVisability.Checked;

            m_Scale.OuterMinorTicks.Visible = checkedValue;

            m_MinorTickShapeComboBox.Enabled = checkedValue;
            m_MinorTicksFillColorButton.Enabled = checkedValue;
            m_MinorTicskWidthUpDown.Enabled = checkedValue;
            m_MajorTickPlacmentComboBox.Enabled = checkedValue;
        }
     
        private void OnUpDownMinorTickWidthChanged(NValueChangeEventArgs arg)
        {
            double minorTickWidth = (double)arg.NewValue;

            if (minorTickWidth >= 2 && minorTickWidth <= 20)
            {
                m_Scale.OuterMinorTicks.Width = minorTickWidth;
            }
            else
            {
                // Revert the value change
                m_MinorTicskWidthUpDown.Value = m_Scale.OuterMinorTicks.Width;
            }
        }
        private void OnMinorTickPlacementComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            ENTicksPlacement selectedPlacement = (ENTicksPlacement)m_MinorTickPlacmentComboBox.SelectedIndex;
        }
        private void OnMinorTickShapeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            m_Scale.OuterMinorTicks.Shape = (ENScaleValueMarkerShape)m_MinorTickShapeComboBox.SelectedIndex;

        }
        private void OnMinorTickFillButtonClick(NEventArgs arg)
        {
            NEditorWindow.CreateForType(
               (NFill)m_Scale.OuterMajorTicks.Fill.DeepClone(),
               null,
               m_Scale.OuterMajorTicks.DisplayWindow,
               false,
               OnMinorTicksFillEdited).Open();
        }
        private void OnMinorTicksFillEdited(NFill fill)
        {
            m_Scale.OuterMinorTicks.Fill = (NFill)(fill.DeepClone());
        }

        #endregion

        #region Fields

        NRadialGauge m_RadialGauge;
        NStandardScale m_Scale;

        NCheckBox m_OuterMajorTicksVisibility;
        NCheckBox m_MinorTicksVisability;

        NComboBox m_MajorTickShapeComboBox;
        NComboBox m_MajorTickPlacmentComboBox;
        NComboBox m_MinorTickPlacmentComboBox;
        NComboBox m_MinorTickShapeComboBox;

        NNumericUpDown m_MajorTicksWidthUpDown;
        NNumericUpDown m_MajorTicskLengthUpDown;
        NNumericUpDown m_MinorTicskWidthUpDown;

        NButton m_MajorTicksFillColorButton;
        NButton m_MinorTicksFillColorButton;

        #endregion

        #region Schema

        public static readonly NSchema NScaleTickMarksExampleSchema;

        #endregion

        #region Static Methods


        #endregion

        #region Constants

        private static readonly NSize defaultRadialGaugeSize = new NSize(300, 300);
        #endregion
    }
}
