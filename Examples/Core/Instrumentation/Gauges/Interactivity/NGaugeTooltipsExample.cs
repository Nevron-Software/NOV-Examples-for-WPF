using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Gauge
{
	/// <summary>
	/// This example demonstrates how to add tooltips to gauge indicators
	/// </summary>
	public class NGaugeTooltipsExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public NGaugeTooltipsExample()
		{
		}
		/// <summary>
		/// 
		/// </summary>
		static NGaugeTooltipsExample()
		{
			NGaugeTooltipsExampleSchema = NSchema.Create(typeof(NGaugeTooltipsExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NStackPanel stack = new NStackPanel();
			
			NStackPanel controlStack = new NStackPanel();
			stack.Add(controlStack);

			NRadialGauge radialGauge = new NRadialGauge();
			radialGauge.PreferredSize = defaultRadialGaugeSize;
			radialGauge.CapEffect = new NGlassCapEffect();
			radialGauge.Dial = new NDial(ENDialShape.Circle, new NEdgeDialRim());
			radialGauge.Dial.BackgroundFill = NAdvancedGradientFill.Create(ENAdvancedGradientColorScheme.Ocean2, 0);

			// configure scale
			NGaugeAxis axis = new NGaugeAxis();
			radialGauge.Axes.Add(axis);
			NLinearScale scale = axis.Scale as NLinearScale;
			scale.SetPredefinedScale(ENPredefinedScaleStyle.PresentationNoStroke);
			scale.Labels.OverlapResolveLayouts = new NDomArray<ENLevelLabelsLayout>();
			scale.MinorTickCount = 3;
			scale.Ruler.Fill = new NColorFill(NColor.FromColor(NColor.White, 0.4f));
			scale.OuterMajorTicks.Fill = new NColorFill(NColor.Orange);
			scale.Labels.Style.TextStyle.Font = new NFont("Arimo", 12.0, ENFontStyle.Bold | ENFontStyle.Italic);
			scale.Labels.Style.TextStyle.Fill = new NColorFill(NColor.White);

			m_Axis = (NGaugeAxis)radialGauge.Axes[0];

			controlStack.Add(radialGauge);

			m_Indicator1 = new NRangeIndicator();
			m_Indicator1.Value = 50;
			m_Indicator1.Fill = new NColorFill(NColor.LightBlue);
			m_Indicator1.Stroke.Color = NColor.DarkBlue;
			m_Indicator1.EndWidth = 20;
			m_Indicator1.AllowDragging = true;
			radialGauge.Indicators.Add(m_Indicator1);

			m_Indicator2 = new NNeedleValueIndicator();
			m_Indicator2.Value = 79;
			m_Indicator2.Fill = new NStockGradientFill(ENGradientStyle.Horizontal, ENGradientVariant.Variant1, NColor.White, NColor.Red);
			m_Indicator2.Stroke.Color = NColor.Red;
			m_Indicator2.AllowDragging = true;
			radialGauge.Indicators.Add(m_Indicator2);

			m_Indicator3 = new NMarkerValueIndicator();
			m_Indicator3.Value = 90;
			m_Indicator3.AllowDragging = true;
			radialGauge.Indicators.Add(m_Indicator3);

			radialGauge.SweepAngle = new NAngle(270.0, NUnit.Degree);

			return stack;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();

			NStackPanel propertyStack = new NStackPanel();
			stack.Add(new NUniSizeBoxGroup(propertyStack));

			m_RangeTooltipTextBox = new NTextBox();
			m_RangeTooltipTextBox.Text = "Range Tooltip";
			propertyStack.Add(new NPairBox("Range Tooltip:", m_RangeTooltipTextBox, true));

			m_NeedleTooltipTextBox = new NTextBox();
			m_NeedleTooltipTextBox.Text = "Needle Tooltip";
			propertyStack.Add(new NPairBox("Needle Tooltip:", m_NeedleTooltipTextBox, true));

			m_MarkerTooltipTextBox = new NTextBox();
			m_MarkerTooltipTextBox.Text = "Marker Tooltip";
			propertyStack.Add(new NPairBox("Marker Tooltip:", m_MarkerTooltipTextBox, true));

			m_ScaleTooltipTextBox = new NTextBox();
			m_ScaleTooltipTextBox.Text = "Scale Tooltip";
			propertyStack.Add(new NPairBox("Scale Tooltip:", m_ScaleTooltipTextBox, true));

			m_RangeTooltipTextBox.TextChanged += new Function<NValueChangeEventArgs>(UpdateTooltips);
			m_NeedleTooltipTextBox.TextChanged += new Function<NValueChangeEventArgs>(UpdateTooltips);
			m_MarkerTooltipTextBox.TextChanged += new Function<NValueChangeEventArgs>(UpdateTooltips);
			m_ScaleTooltipTextBox.TextChanged += new Function<NValueChangeEventArgs>(UpdateTooltips);

            UpdateTooltips(null);

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>The example demonstrates how to assign tooltips to different gauge elements.</p>";
		}

		#endregion

		#region Implementation

		private void UpdateTooltips(NValueChangeEventArgs arg)
		{
			if (m_Axis == null)
				return;

			m_Indicator1.Tooltip = new NTooltip(m_RangeTooltipTextBox.Text);
			m_Indicator2.Tooltip = new NTooltip(m_NeedleTooltipTextBox.Text);
			m_Indicator3.Tooltip = new NTooltip(m_MarkerTooltipTextBox.Text);
		}

		#endregion

		#region Fields

		NRangeIndicator m_Indicator1;
		NNeedleValueIndicator m_Indicator2;
		NMarkerValueIndicator m_Indicator3;
		NGaugeAxis m_Axis;

		NTextBox m_RangeTooltipTextBox;
		NTextBox m_NeedleTooltipTextBox;
		NTextBox m_MarkerTooltipTextBox;
		NTextBox m_ScaleTooltipTextBox;

		#endregion

		#region Schema

		public static readonly NSchema NGaugeTooltipsExampleSchema;

		#endregion

		#region Constants

		private static readonly NSize defaultRadialGaugeSize = new NSize(300, 300);

		#endregion
	}
}