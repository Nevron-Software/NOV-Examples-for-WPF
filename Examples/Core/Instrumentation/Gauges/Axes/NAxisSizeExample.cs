using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Gauge
{
	/// <summary>
	/// This example demonstrates how to control the size of the gauge axes
	/// </summary>
	public class NAxisSizeExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public NAxisSizeExample()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        static NAxisSizeExample()
        {
			NAxisSizeExampleSchema = NSchema.Create(typeof(NAxisSizeExample), NExampleBaseSchema);
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

			m_RadialGauge.Dial = new NDial(ENDialShape.CutCircle, new NEdgeDialRim());
			m_RadialGauge.Dial.BackgroundFill = new NStockGradientFill(NColor.DarkGray, NColor.Black);
            m_RadialGauge.SweepAngle = new NAngle(360, NUnit.Degree);

            NGelCapEffect gelEffect = new NGelCapEffect(ENCapEffectShape.Ellipse);
			gelEffect.Margins = new NMargins(0, 0, 0, 0.5);

            m_RadialGauge.Axes.Clear();

			// create the first axis
			NGaugeAxis axis1 = new NGaugeAxis();
			axis1.Anchor = new NDockGaugeAxisAnchor(ENGaugeAxisDockZone.Top, true, 0, 70);
			NStandardScale scale1 = (NStandardScale)axis1.Scale;
			scale1.SetPredefinedScale(ENPredefinedScaleStyle.PresentationNoStroke);
			scale1.MinorTickCount = 3;
			scale1.Ruler.Fill = new NColorFill(NColor.FromColor(NColor.White, 0.4f));
			scale1.OuterMajorTicks.Fill = new NColorFill(NColor.Orange);
			scale1.Labels.Style.TextStyle.Font  = new NFont("Arimo", 12, ENFontStyle.Bold);
			scale1.Labels.Style.TextStyle.Fill = new NColorFill(NColor.White);
			scale1.Labels.Style.Angle = new NScaleLabelAngle(ENScaleLabelAngleMode.Scale, 0);

			m_RadialGauge.Axes.Add(axis1);

			// create the second axis
			NGaugeAxis axis2 = new NGaugeAxis();
			axis2.Anchor = new NDockGaugeAxisAnchor(ENGaugeAxisDockZone.Top, false, 75, 95);
			NStandardScale scale2 = (NStandardScale)axis2.Scale;
			scale2.SetPredefinedScale(ENPredefinedScaleStyle.PresentationNoStroke);
			scale2.MinorTickCount = 3;
			scale2.Ruler.Fill = new NColorFill(NColor.FromColor(NColor.White, 0.4f));
			scale2.OuterMajorTicks.Fill = new NColorFill(NColor.Blue);
			scale2.Labels.Style.TextStyle.Font = new NFont("Arimo", 12, ENFontStyle.Bold);
			scale2.Labels.Style.TextStyle.Fill = new NColorFill(NColor.White);
			scale2.Labels.Style.Angle = new NScaleLabelAngle(ENScaleLabelAngleMode.Scale, 0);

			m_RadialGauge.Axes.Add(axis2);

			// add indicators
			NRangeIndicator rangeIndicator = new NRangeIndicator();
			rangeIndicator.Value = 50;
			rangeIndicator.Fill = new NStockGradientFill(NColor.Orange, NColor.Red);
			rangeIndicator.Stroke.Width = 0;
			rangeIndicator.OffsetFromScale = 3;
			rangeIndicator.BeginWidth = 6;
			rangeIndicator.EndWidth = 12;
			m_RadialGauge.Indicators.Add(rangeIndicator);

			NNeedleValueIndicator needleValueIndicator1 = new NNeedleValueIndicator();
			needleValueIndicator1.Value = 79;
//			needleValueIndicator1.Shape.FillStyle = new NGradientFillStyle(GradientStyle.Vertical, GradientVariant.Variant2, Color.White, Color.Red);
//			needleValueIndicator1.Shape.StrokeStyle.Color = Color.Red;
			needleValueIndicator1.ScaleAxis = axis1;
			needleValueIndicator1.OffsetFromScale = 2;
			m_RadialGauge.Indicators.Add(needleValueIndicator1);

			NNeedleValueIndicator needleValueIndicator2 = new NNeedleValueIndicator();
			needleValueIndicator2.Value = 79;
//			needleValueIndicator2.Shape.FillStyle = new NGradientFillStyle(GradientStyle.Vertical, GradientVariant.Variant2, Color.White, Color.Blue);
//			needleValueIndicator2.Shape.StrokeStyle.Color = Color.Blue;
			needleValueIndicator2.ScaleAxis = axis2;
			needleValueIndicator2.OffsetFromScale = 2;
			m_RadialGauge.Indicators.Add(needleValueIndicator2);
			
			return stack;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();

			NStackPanel propertyStack = new NStackPanel();
			stack.Add(new NUniSizeBoxGroup(propertyStack));

			m_ScrollBar = new NHScrollBar();
			m_ScrollBar.Value = 70.0;
			m_ScrollBar.ValueChanged += new Function<NValueChangeEventArgs>(OnScrollBarValueChanged);
			propertyStack.Add(new NPairBox("Percent:", m_ScrollBar, true));

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>The example demonstrates how use the begin and end percent properties of the anchor in order to change the gauge axis size.</p>";
		}

		#endregion 

		#region Event Handlers

		private void OnScrollBarValueChanged(NValueChangeEventArgs arg)
		{
			 NGaugeAxis axis1 = (NGaugeAxis)m_RadialGauge.Axes[0];
			 NGaugeAxis axis2 = (NGaugeAxis)m_RadialGauge.Axes[1];

			 axis1.Anchor = new NDockGaugeAxisAnchor(ENGaugeAxisDockZone.Top, true, 0, (float)(m_ScrollBar.Value - 5));
			 axis2.Anchor = new NDockGaugeAxisAnchor(ENGaugeAxisDockZone.Top, false, (float)m_ScrollBar.Value, 95);
//			 RedAxisTextBox.Text = m_ScrollBar.Value.ToString();
		}

		#endregion

		#region Fields

		NRadialGauge m_RadialGauge;
		NHScrollBar m_ScrollBar;

		#endregion

		#region Schema

		public static readonly NSchema NAxisSizeExampleSchema;

		#endregion

		#region Constants

		private static readonly NSize defaultRadialGaugeSize = new NSize(300, 300);

		#endregion
	}
}
