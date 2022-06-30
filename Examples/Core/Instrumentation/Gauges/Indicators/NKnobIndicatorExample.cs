using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Gauge
{
    /// <summary>
	/// This example demonstrates how to add create a knob indicator
    /// </summary>
	public class NKnobIndicatorExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public NKnobIndicatorExample()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        static NKnobIndicatorExample()
        {
			NKnobIndicatorExampleSchema = NSchema.Create(typeof(NKnobIndicatorExample), NExampleBaseSchema);
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

			//			radialGauge.PreferredSize = new NSize(0, 50);
			//			radialGauge.ContentAlignment = ContentAlignment.MiddleCenter;
			m_RadialGauge.SweepAngle = new NAngle(270, NUnit.Degree);
			m_RadialGauge.BeginAngle = new NAngle(-225, NUnit.Degree);
			m_RadialGauge.NeedleCap.Visible = false;
			m_RadialGauge.PreferredSize = defaultRadialGaugeSize;

			// configure scale
			NGaugeAxis axis = new NGaugeAxis();
			m_RadialGauge.Axes.Add(axis);

			NStandardScale scale = (NStandardScale)axis.Scale;

			scale.SetPredefinedScale(ENPredefinedScaleStyle.PresentationNoStroke);
			scale.Labels.Style.TextStyle.Font = new NFont("Arimo", 12.0, ENFontStyle.Italic);
			scale.Labels.Style.TextStyle.Fill = new NColorFill(NColor.Black);
			scale.Labels.Style.Angle = new NScaleLabelAngle(ENScaleLabelAngleMode.Scale, 0.0);
			scale.MinorTickCount = 4;
			scale.Ruler.Stroke.Width = 0;
			scale.Ruler.Fill = new NColorFill(NColor.DarkGray);

			// create the knob indicator
			m_KnobIndicator = new NKnobIndicator();
			m_KnobIndicator.OffsetFromScale = -3;
			m_KnobIndicator.AllowDragging = true;

			// apply fill style to the marker
			NAdvancedGradientFill advancedGradientFill = new NAdvancedGradientFill();
			advancedGradientFill.BackgroundColor = NColor.Red;
			advancedGradientFill.Points.Add(new NAdvancedGradientPoint(NColor.White, new NAngle(20, NUnit.Degree), 20, 0, 100, ENAdvancedGradientPointShape.Circle));
			m_KnobIndicator.Fill = advancedGradientFill;
			m_KnobIndicator.ValueChanged += new Function<NValueChangeEventArgs>(OnKnobValueChanged);
			m_RadialGauge.Indicators.Add(m_KnobIndicator);

			// create the numeric display
			m_NumericDisplay = new NNumericLedDisplay();

			m_NumericDisplay.PreferredSize = new NSize(0, 60);
			m_NumericDisplay.BackgroundFill = new NColorFill(NColor.Black);
			m_NumericDisplay.Border = NBorder.CreateSunken3DBorder(new NUIThemeColorMap(ENUIThemeScheme.WindowsClassic));
			m_NumericDisplay.BorderThickness = new NMargins(6);
			m_NumericDisplay.ContentAlignment = ENContentAlignment.MiddleCenter;
			m_NumericDisplay.Margins = new NMargins(5);
			m_NumericDisplay.Padding = new NMargins(5);
			m_NumericDisplay.CapEffect = new NGelCapEffect();

			controlStack.Add(m_RadialGauge);
			controlStack.Add(m_NumericDisplay);

			return stack;
		}

        protected override NWidget CreateExampleControls()
        {
			NStackPanel stack = new NStackPanel();

			NStackPanel propertyStack = new NStackPanel();
			stack.Add(new NUniSizeBoxGroup(propertyStack));

			// marker properties
			NGroupBox markerGroupBox = new NGroupBox("Marker");
			propertyStack.Add(markerGroupBox);
			NStackPanel markerGroupBoxContent = new NStackPanel();
			markerGroupBox.Content = new NUniSizeBoxGroup(markerGroupBoxContent);

			// fill the marker shape combo
			m_MarkerShapeComboBox = new NComboBox();
			m_MarkerShapeComboBox.FillFromEnum<ENScaleValueMarkerShape>();
			m_MarkerShapeComboBox.SelectedIndex = (int)m_KnobIndicator.Shape;
			m_MarkerShapeComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnKnobAppearanceChanged);
			markerGroupBoxContent.Add(new NPairBox("Shape:", m_MarkerShapeComboBox, true));

			m_MarkerOffsetUpDown = new NNumericUpDown();
			m_MarkerOffsetUpDown.Value = m_KnobIndicator.OffsetFromScale;
			m_MarkerOffsetUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnKnobAppearanceChanged);
			markerGroupBoxContent.Add(new NPairBox("Offset:", m_MarkerOffsetUpDown, true));
			
			m_MarkerPaintOrderComboBox = new NComboBox();
			m_MarkerPaintOrderComboBox.FillFromEnum<ENKnobMarkerPaintOrder>();
			m_MarkerPaintOrderComboBox.SelectedIndex = (int)m_KnobIndicator.MarkerPaintOrder;
			m_MarkerPaintOrderComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnKnobAppearanceChanged);
			markerGroupBoxContent.Add(new NPairBox("Paint Order:", m_MarkerPaintOrderComboBox, true));

			// outer rim properties
			NGroupBox outerRimGroupBox = new NGroupBox("Outer Rim");
			propertyStack.Add(outerRimGroupBox);
			NStackPanel outerRimGroupBoxContent = new NStackPanel();
			outerRimGroupBox.Content = new NUniSizeBoxGroup(outerRimGroupBoxContent);

			m_OuterRimPatternComboBox = new NComboBox();
			m_OuterRimPatternComboBox.FillFromEnum<ENCircularRimPattern>();
			m_OuterRimPatternComboBox.SelectedIndex = (int)m_KnobIndicator.OuterRim.Pattern;
			m_OuterRimPatternComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnKnobAppearanceChanged);
			outerRimGroupBoxContent.Add(new NPairBox("Pattern", m_OuterRimPatternComboBox, true));

			m_OuterRimPatternRepeatCountUpDown = new NNumericUpDown();
			m_OuterRimPatternRepeatCountUpDown.Value = m_KnobIndicator.OuterRim.PatternRepeatCount;
			m_OuterRimPatternRepeatCountUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnKnobAppearanceChanged);
			outerRimGroupBoxContent.Add(new NPairBox("Repeat Count:", m_OuterRimPatternRepeatCountUpDown, true));

			m_OuterRimRadiusOffsetUpDown = new NNumericUpDown();
			m_OuterRimRadiusOffsetUpDown.Value = m_KnobIndicator.OuterRim.Offset;
			m_OuterRimRadiusOffsetUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnKnobAppearanceChanged);
			outerRimGroupBoxContent.Add(new NPairBox("Radius Offset:", m_OuterRimRadiusOffsetUpDown, true));

			// inner rim properties
			NGroupBox innerRimGroupBox = new NGroupBox("Inner Rim");
			propertyStack.Add(innerRimGroupBox);
			NStackPanel innerRimGroupBoxContent = new NStackPanel();
			innerRimGroupBox.Content = new NUniSizeBoxGroup(innerRimGroupBoxContent);

			m_InnerRimPatternComboBox = new NComboBox();
			m_InnerRimPatternComboBox.FillFromEnum<ENCircularRimPattern>();
			m_InnerRimPatternComboBox.SelectedIndex = (int)m_KnobIndicator.InnerRim.Pattern;
			m_InnerRimPatternComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnKnobAppearanceChanged);
			innerRimGroupBoxContent.Add(new NPairBox("Pattern", m_InnerRimPatternComboBox, true));

			m_InnerRimPatternRepeatCountUpDown = new NNumericUpDown();
			m_InnerRimPatternRepeatCountUpDown.Value = m_KnobIndicator.InnerRim.PatternRepeatCount;
			m_InnerRimPatternRepeatCountUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnKnobAppearanceChanged);
			innerRimGroupBoxContent.Add(new NPairBox("Repeat Count:", m_InnerRimPatternRepeatCountUpDown, true));

			m_InnerRimRadiusOffsetUpDown = new NNumericUpDown();
			m_InnerRimRadiusOffsetUpDown.Value = m_KnobIndicator.InnerRim.Offset;
			m_InnerRimRadiusOffsetUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnKnobAppearanceChanged);
			innerRimGroupBoxContent.Add(new NPairBox("Radius Offset:", m_InnerRimRadiusOffsetUpDown, true));

            return stack;
        }

		protected override string GetExampleDescription()
		{
			return @"<p>The example demonstrates the properties of the knob indicator.</p>";
		}

		#endregion 

		#region Implementation
		
		#endregion

		#region Event Handlers

		void OnKnobValueChanged(NValueChangeEventArgs arg)
		{
			if (m_RadialGauge == null)
				return;

			m_NumericDisplay.Value = ((NIndicator)m_RadialGauge.Indicators[0]).Value;
		}

		void OnKnobAppearanceChanged(NValueChangeEventArgs arg)
		{
			// update the knob marker shape
			m_KnobIndicator.Shape = (ENScaleValueMarkerShape)m_MarkerShapeComboBox.SelectedIndex;
			m_KnobIndicator.OffsetFromScale = m_MarkerOffsetUpDown.Value;
			m_KnobIndicator.MarkerPaintOrder = (ENKnobMarkerPaintOrder)m_MarkerPaintOrderComboBox.SelectedIndex;

			 // update the outer rim style
			m_KnobIndicator.OuterRim.Pattern = (ENCircularRimPattern)m_OuterRimPatternComboBox.SelectedIndex;
			m_KnobIndicator.OuterRim.PatternRepeatCount = (int)m_OuterRimPatternRepeatCountUpDown.Value;
			m_KnobIndicator.OuterRim.Offset = m_OuterRimRadiusOffsetUpDown.Value;

			// update the inner rim style
			m_KnobIndicator.InnerRim.Pattern = (ENCircularRimPattern)m_InnerRimPatternComboBox.SelectedIndex;
			m_KnobIndicator.InnerRim.PatternRepeatCount = (int)m_InnerRimPatternRepeatCountUpDown.Value;
			m_KnobIndicator.InnerRim.Offset = m_InnerRimRadiusOffsetUpDown.Value;
		}

		#endregion

		#region Fields

		NRadialGauge m_RadialGauge;
		NNumericLedDisplay m_NumericDisplay;
		NKnobIndicator m_KnobIndicator;

		NNumericUpDown m_MarkerOffsetUpDown;
		NComboBox m_MarkerPaintOrderComboBox;
		NComboBox m_MarkerShapeComboBox;

		NComboBox m_OuterRimPatternComboBox;
		NNumericUpDown m_OuterRimPatternRepeatCountUpDown;
		NNumericUpDown m_OuterRimRadiusOffsetUpDown;

		NNumericUpDown m_InnerRimRadiusOffsetUpDown;
		NNumericUpDown m_InnerRimPatternRepeatCountUpDown;
		NComboBox m_InnerRimPatternComboBox;

		#endregion

		#region Schema

		public static readonly NSchema NKnobIndicatorExampleSchema;

		#endregion

		#region Constants

		private static readonly NSize defaultRadialGaugeSize = new NSize(300, 300);

		#endregion
	}
}
