using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Gauge
{
    /// <summary>
    /// This example demonstrates variuos appearance-related properties of the scale bar.
    /// </summary>
    public class NScalesAppearanceExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public NScalesAppearanceExample()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        static NScalesAppearanceExample()
        {
            NScalesAppearanceExampleSchema = NSchema.Create(typeof(NScalesAppearanceExample), NExampleBaseSchema);
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
            m_RadialGauge.Dial = new NDial(ENDialShape.Circle, new NEdgeDialRim());
            m_RadialGauge.PreferredSize = defaultRadialGaugeSize;

            m_RadialGauge.SweepAngle = new NAngle(280, NUnit.Degree);
            m_RadialGauge.BeginAngle = new NAngle(120, NUnit.Degree);

            controlStack.Add(m_RadialGauge);

            m_RadialGauge.SweepAngle = new NAngle(310, NUnit.Degree);
            m_RadialGauge.BeginAngle = new NAngle(-240, NUnit.Degree);

            // configure the axis
            NGaugeAxis axis = new NGaugeAxis();
            axis.Anchor.ScaleOrientation = ENScaleOrientation.Right;
            axis.Range = new NRange(0, 100);
            axis.Anchor = new NDockGaugeAxisAnchor(ENGaugeAxisDockZone.Top, true, ENScaleOrientation.Right, 0.0f, 100.0f);
            
            m_RadialGauge.Axes.Add(axis);
         
            // create scale 
            m_Scale = (NStandardScale)axis.Scale;
            m_Scale.SetPredefinedScale(ENPredefinedScaleStyle.PresentationNoStroke);
            m_Scale.Labels.OverlapResolveLayouts = new NDomArray<ENLevelLabelsLayout>();
            m_Scale.MinorTickCount = 3;
            m_Scale.Ruler.Fill = new NColorFill(NColor.FromColor(NColor.Transparent, 0.4f));
            m_Scale.OuterMajorTicks.Fill = new NColorFill(NColor.Black);
            m_Scale.Labels.Style.TextStyle.Font = new NFont("Arimo", 12, ENFontStyle.Bold);
            m_Scale.Labels.Style.TextStyle.Fill = new NColorFill(NColor.Black);
            m_Scale.MajorTickMode = ENMajorTickMode.AutoMaxCount;
            m_Scale.OuterMajorTicks.Fill = new NColorFill(NColor.Red);
            m_Scale.OuterMinorTicks.Shape = ENScaleValueMarkerShape.Line;
            m_Scale.OuterMajorTicks.Shape = ENScaleValueMarkerShape.Star;

            // Value indicator - Needle 
            m_NeedleIndicator = new NNeedleValueIndicator(60);
            m_NeedleIndicator.OffsetOriginMode = ENIndicatorOffsetOriginMode.ScaleMiddle;
            m_NeedleIndicator.Value = 79;
            m_NeedleIndicator.Fill = new NStockGradientFill(ENGradientStyle.Horizontal, ENGradientVariant.Variant1, NColor.White, NColor.Red);
            m_NeedleIndicator.OffsetFromScale = 15.0;
            m_NeedleIndicator.Stroke.Color = NColor.Red;
           
            m_RadialGauge.Indicators.Add(m_NeedleIndicator);

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

            // Scale Ruler Color Fill 
            m_ScaleFillButton = new NButton("Scale Fill Style");
            m_ScaleFillButton.Click += new Function<NEventArgs>(OnScaleFillButtonClick);
            propertyStack.Add(m_ScaleFillButton);

            // Load Predifined Scale Type 
            m_ScaleTypeComboBox = new NComboBox();
            m_ScaleTypeComboBox.FillFromEnum<ENPredefinedScaleStyle>();
            m_ScaleTypeComboBox.SelectedIndex = (int)m_ScaleTypeComboBox.SelectedIndex;
            m_ScaleTypeComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnScaleStyleComboBoxSelectedIndexChanged);
            propertyStack.Add(new NPairBox("Load Predifined Scale Style:", m_ScaleTypeComboBox, true));

            // Change Major Tick Mode
            m_MajorTickModeComboBox = new NComboBox();
            m_MajorTickModeComboBox.FillFromEnum<ENMajorTickMode>();
            m_MajorTickModeComboBox.SelectedIndex = (int)m_MajorTickModeComboBox.SelectedIndex;
            m_MajorTickModeComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnMajorTickModeComboBoxSelectedIndexChanged);
            majorTicksGroupBoxGroupBoxContent.Add(new NPairBox("Change Major Tick Mode:", m_MajorTickModeComboBox, true));

            // Change Major Tick Shape 
            m_MajorTickShapeComboBox = new NComboBox();
            m_MajorTickShapeComboBox.FillFromEnum<ENScaleValueMarkerShape>();
            m_MajorTickShapeComboBox.SelectedIndex = (int)m_MajorTickShapeComboBox.SelectedIndex;
            m_MajorTickShapeComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnMajorTickShapeComboBoxSelectedIndexChanged);
            majorTicksGroupBoxGroupBoxContent.Add(new NPairBox("Change Major Tick Shape:", m_MajorTickShapeComboBox, true));

            //  Outer Major Ticks Fill
            m_OuterMajorTicksFillButton = new NButton("Outer Major Ticks Fill");
            m_OuterMajorTicksFillButton.Click += new Function<NEventArgs>(OnOuterMajorTickFillButtonClick);
            majorTicksGroupBoxGroupBoxContent.Add(m_OuterMajorTicksFillButton);

            // Change Major Tick Shape 
            m_MinorTickShapeComboBox = new NComboBox();
            m_MinorTickShapeComboBox.FillFromEnum<ENScaleValueMarkerShape>();
            m_MinorTickShapeComboBox.SelectedIndex = (int)m_MajorTickShapeComboBox.SelectedIndex;
            m_MinorTickShapeComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnMinorTickShapeComboBoxSelectedIndexChanged);
            minorTicksGroupBoxContent.Add(new NPairBox("Change Minor Tick Shape:", m_MinorTickShapeComboBox, true));
            
            // Change Minor Tick Mode 
            m_MinorTickModeComboBox = new NComboBox();
            m_MinorTickModeComboBox.FillFromEnum<ENMajorTickMode>();
            m_MinorTickModeComboBox.SelectedIndex = (int)m_MinorTickModeComboBox.SelectedIndex;
            m_MinorTickModeComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnMinorTickModeComboBoxSelectedIndexChanged);
            minorTicksGroupBoxContent.Add(new NPairBox("Change Minor Tick Mode:", m_MinorTickModeComboBox, true));
           
            // Minor Tick Color Fill 
            m_MinorTickButton = new NButton("Minor Tick Fill Style");
            m_MinorTickButton.Click += new Function<NEventArgs>(OnMinorTickFillButtonClick);
            minorTicksGroupBoxContent.Add(m_MinorTickButton);
          
            return stack;
        }
      
        protected override string GetExampleDescription()
        {
            return @"<p>The example demonstrates various appearance-related properties of the scale bar.</p>";
        }

        #endregion

        #region Event Handlers
        private void OnScaleFillButtonClick(NEventArgs arg)
        {
            NEditorWindow.CreateForType(
                (NFill)m_Scale.Ruler.Fill.DeepClone(),
                null,
                m_Scale.Ruler.DisplayWindow,
                false,
                OnColorFillEdited).Open();
        }
        private void OnColorFillEdited(NFill fill)
        {
            m_Scale.Ruler.Fill = (NFill)(fill.DeepClone());
        }
        private void OnScaleStyleComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            m_Scale.SetPredefinedScale((ENPredefinedScaleStyle)m_ScaleTypeComboBox.SelectedIndex);

            m_MajorTickModeComboBox.SelectedIndex = (int)m_Scale.MajorTickMode;
            m_MinorTickModeComboBox.SelectedIndex = (int)m_Scale.MajorTickMode;

            m_MajorTickShapeComboBox.SelectedIndex = (int)m_Scale.OuterMajorTicks.Shape;
            m_MinorTickShapeComboBox.SelectedIndex = (int)m_Scale.OuterMinorTicks.Shape;
        }
        private void OnMajorTickModeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            m_Scale.MajorTickMode = (ENMajorTickMode)m_MajorTickModeComboBox.SelectedIndex;
        }
        private void OnMinorTickModeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            m_Scale.MajorTickMode = (ENMajorTickMode)m_MinorTickModeComboBox.SelectedIndex;
        }
        private void OnMinorTickFillButtonClick(NEventArgs arg)
        {
            NEditorWindow.CreateForType(
                 (NFill)m_Scale.OuterMinorTicks.Fill.DeepClone(),
                 null,
                 m_Scale.OuterMinorTicks.DisplayWindow,
                 false,
                 OnOuterMinorTicksFillEdited).Open();
        }
        private void OnOuterMinorTicksFillEdited(NFill fill)
        {
            m_Scale.OuterMinorTicks.Fill = (NFill)(fill.DeepClone());
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
        private void OnMinorTickShapeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            m_Scale.OuterMinorTicks.Shape = (ENScaleValueMarkerShape)m_MinorTickShapeComboBox.SelectedIndex;
        }
        private void OnMajorTickShapeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            m_Scale.OuterMajorTicks.Shape = (ENScaleValueMarkerShape)m_MajorTickShapeComboBox.SelectedIndex;
        }

        #endregion

        #region Implementation

        #endregion

        #region Fields

        NRadialGauge m_RadialGauge;
        NStandardScale m_Scale;
        NNeedleValueIndicator m_NeedleIndicator;
     
        NComboBox m_ScaleTypeComboBox;
        NComboBox m_MajorTickModeComboBox;
        NComboBox m_MinorTickModeComboBox;
        NComboBox m_MinorTickShapeComboBox;
        NComboBox m_MajorTickShapeComboBox;

        NButton m_MinorTickButton;
        NButton m_OuterMajorTicksFillButton;
        NButton m_ScaleFillButton;

        #endregion

        #region Schema

        public static readonly NSchema NScalesAppearanceExampleSchema;

        #endregion

        #region Static Methods

        #endregion

        #region Constants
        private static readonly NSize defaultRadialGaugeSize = new NSize(300, 300);
        #endregion
    }
}