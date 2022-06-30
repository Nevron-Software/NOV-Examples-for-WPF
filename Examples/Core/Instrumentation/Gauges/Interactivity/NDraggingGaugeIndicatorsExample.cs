using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Gauge
{
	/// <summary>
	/// This example demonstrates how to configure the gauge so that the user can drag gauge indicators
	/// </summary>
	public class NDraggingGaugeIndicatorsExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public NDraggingGaugeIndicatorsExample()
		{
		}
		/// <summary>
		/// 
		/// </summary>
		static NDraggingGaugeIndicatorsExample()
		{
			NDraggingGaugeIndicatorsExampleSchema = NSchema.Create(typeof(NDraggingGaugeIndicatorsExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NStackPanel stack = new NStackPanel();

			NStackPanel controlStack = new NStackPanel();
			stack.Add(controlStack);

			// create the radial gauge
			NRadialGauge radialGauge = new NRadialGauge();
			controlStack.Add(radialGauge);

			radialGauge.Dial = new NDial(ENDialShape.Circle, new NEdgeDialRim());
			radialGauge.PreferredSize = defaultRadialGaugeSize;
			radialGauge.Dial.BackgroundFill = new NStockGradientFill(NColor.DarkGray, NColor.Black);
			radialGauge.CapEffect = new NGlassCapEffect();

			// configure the axis
			NGaugeAxis axis = new NGaugeAxis();
			radialGauge.Axes.Add(axis);

			NStandardScale scale = (NStandardScale)axis.Scale;

			scale.SetPredefinedScale(ENPredefinedScaleStyle.Scientific);
			scale.Labels.Style.TextStyle.Fill = new NColorFill(NColor.White);
			scale.Labels.Style.TextStyle.Font = new NFont("Tinos", 10, ENFontStyle.Italic | ENFontStyle.Bold);

			scale.OuterMajorTicks.Stroke.Color = NColor.White;
			scale.OuterMajorTicks.Length = 6;

			scale.OuterMinorTicks.Stroke.Color = NColor.White;
			scale.OuterMinorTicks.Length = 4;

			scale.Ruler.Stroke.Color = NColor.White;
			scale.MinorTickCount = 4;

			// add some indicators
			m_RangeIndicator = new NRangeIndicator();
			m_RangeIndicator.Value = 50;
			m_RangeIndicator.Palette = new NTwoColorPalette(NColor.DarkBlue, NColor.LightBlue);
			m_RangeIndicator.Stroke = null;
			m_RangeIndicator.EndWidth = 20;
			m_RangeIndicator.AllowDragging = true;
			radialGauge.Indicators.Add(m_RangeIndicator);

			m_NeedleIndicator = new NNeedleValueIndicator();
			m_NeedleIndicator.Value = 79;
			m_NeedleIndicator.AllowDragging = true;
			radialGauge.Indicators.Add(m_NeedleIndicator);
			radialGauge.SweepAngle = new NAngle(270, NUnit.Degree);

			m_MarkerIndicator = new NMarkerValueIndicator();
			m_MarkerIndicator.Value = 90;
			m_MarkerIndicator.AllowDragging = true;
			m_MarkerIndicator.OffsetOriginMode = ENIndicatorOffsetOriginMode.ScaleEnd;
			m_MarkerIndicator.OffsetFromScale = 0.0;
			radialGauge.Indicators.Add(m_MarkerIndicator);

			return stack;
		}

		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();

			NStackPanel propertyStack = new NStackPanel();
			stack.Add(new NUniSizeBoxGroup(propertyStack));

			m_IndicatorSnapModeComboBox = new NComboBox();
			m_IndicatorSnapModeComboBox.FillFromArray(new string[] {    "None",
																		"Ruler",
																		"Major ticks",
																		"Minor ticks",
																		"Ruler Min/Max",
																		"Numeric" });
			m_IndicatorSnapModeComboBox.SelectedIndex = 0;
			m_IndicatorSnapModeComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnUdpdateIndicatorValueSnapper);
			propertyStack.Add(new NPairBox("Indicator Snap Mode:", m_IndicatorSnapModeComboBox, true));

			m_StepNumericUpDown = new NNumericUpDown();
			m_StepNumericUpDown.Enabled = false;
			m_StepNumericUpDown.Value = 5.0;
			m_StepNumericUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnUdpdateIndicatorValueSnapper);
			propertyStack.Add(new NPairBox("Step:", m_StepNumericUpDown, true));

			m_AllowDraggingRangeIndicator = new NCheckBox("Allow Dragging Range");
			m_AllowDraggingRangeIndicator.Checked = m_RangeIndicator.AllowDragging;
			m_AllowDraggingRangeIndicator.CheckedChanged += new Function<NValueChangeEventArgs>(OnAllowDraggingRangeIndicator);
			propertyStack.Add(m_AllowDraggingRangeIndicator);

			m_AllowDraggingNeedleIndicator = new NCheckBox("Allow Dragging Needle");
			m_AllowDraggingNeedleIndicator.Checked = m_NeedleIndicator.AllowDragging;
			m_AllowDraggingNeedleIndicator.CheckedChanged += new Function<NValueChangeEventArgs>(OnAllowDraggingNeedleIndicator);
			propertyStack.Add(m_AllowDraggingNeedleIndicator);

			m_AllowDraggingMarkerIndicator = new NCheckBox("Allow Dragging Marker");
			m_AllowDraggingMarkerIndicator.Checked = m_MarkerIndicator.AllowDragging;
			m_AllowDraggingMarkerIndicator.CheckedChanged += new Function<NValueChangeEventArgs>(OnAllowDraggingMarkerIndicator);
			propertyStack.Add(m_AllowDraggingMarkerIndicator);

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>The example demonstrates how to enable dragging of gauge indicators.</p>";
		}

		#endregion

		#region Implementation

		/// <summary>
		/// Creates an indicator value snapper
		/// </summary>
		/// <returns></returns>
		private NValueSnapper CreateValueSnapper()
		{
			switch (m_IndicatorSnapModeComboBox.SelectedIndex)
			{
				case 0: //None, snapping is disabled
					return null;
				case 1: // Ruler, values are constrained to the ruler begin and end values.
					return new NAxisRulerClampSnapper();
				case 2: // Major ticks, values are snapped to axis major ticks
					return new NAxisMajorTickSnapper();
				case 3: // Minor ticks, values are snapped to axis minor ticks
					return new NAxisMinorTickSnapper();
				case 4: // Ruler Min Max, values are snapped to the ruler min and max values
					return new NAxisRulerMinMaxSnapper();
				case 5:
					return new NNumericValueSnapper(0.0, m_StepNumericUpDown.Value);
				default:
					return null;
			}
		}

		#endregion

		#region Event Handlers

		void OnAllowDraggingMarkerIndicator(NValueChangeEventArgs arg)
		{
			m_MarkerIndicator.AllowDragging = m_AllowDraggingMarkerIndicator.Checked;
		}

		void OnAllowDraggingNeedleIndicator(NValueChangeEventArgs arg)
		{
			m_NeedleIndicator.AllowDragging = m_AllowDraggingNeedleIndicator.Checked;
		}

		void OnAllowDraggingRangeIndicator(NValueChangeEventArgs arg)
		{
			m_RangeIndicator.AllowDragging = m_AllowDraggingRangeIndicator.Checked;
		}

		void OnUdpdateIndicatorValueSnapper(NValueChangeEventArgs arg)
		{
			m_RangeIndicator.ValueSnapper = CreateValueSnapper();
			m_NeedleIndicator.ValueSnapper = CreateValueSnapper();
			m_MarkerIndicator.ValueSnapper = CreateValueSnapper();
		}

		void OnAllowDragRangeIndicatorsCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			// m_Indicator1.AllowDragging = m_AllowDragRangeIndicatorsCheckBox.Checked;
		}

		#endregion

		#region Fields

		NRangeIndicator m_RangeIndicator;
		NNeedleValueIndicator m_NeedleIndicator;
		NMarkerValueIndicator m_MarkerIndicator;

		NNumericUpDown m_StepNumericUpDown;

		NComboBox m_IndicatorSnapModeComboBox;
		NCheckBox m_AllowDraggingRangeIndicator;
		NCheckBox m_AllowDraggingNeedleIndicator;
		NCheckBox m_AllowDraggingMarkerIndicator;

		#endregion

		#region Schema

		public static readonly NSchema NDraggingGaugeIndicatorsExampleSchema;

		#endregion

		#region Constants

		private static readonly NSize defaultRadialGaugeSize = new NSize(300, 300);

		#endregion
	}
}
