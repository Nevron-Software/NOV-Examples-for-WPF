using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Gauge
{
    /// <summary>
	/// This example demonstrates how to control the size of the gauge axes
    /// </summary>
	public class NAxisDockingExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public NAxisDockingExample()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        static NAxisDockingExample()
        {
			NAxisDockingExampleSchema = NSchema.Create(typeof(NAxisDockingExample), NExampleBaseSchema);
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

            m_LinearGauge = new NLinearGauge();
			m_LinearGauge.Padding = new NMargins(30);
			m_LinearGauge.PreferredSize = defaultLinearHorizontalGaugeSize;
            controlStack.Add(m_LinearGauge);

			NGaugeAxis axis = new NGaugeAxis();
			m_LinearGauge.Axes.Add(axis);
            axis.Anchor = new NDockGaugeAxisAnchor(ENGaugeAxisDockZone.Top, true, ENScaleOrientation.Left, 0, 100);

			axis = new NGaugeAxis();
			m_LinearGauge.Axes.Add(axis);
			axis.Anchor = new NDockGaugeAxisAnchor(ENGaugeAxisDockZone.Top, true, ENScaleOrientation.Left, 0, 50);

            // create the radial gauge
			m_RadialGauge = new NRadialGauge();
			m_RadialGauge.PreferredSize = defaultRadialGaugeSize;
			m_RadialGauge.NeedleCap.Visible = false;
            controlStack.Add(m_RadialGauge);

            // create the radial gauge
            m_RadialGauge.SweepAngle = new NAngle(270, NUnit.Degree);
            m_RadialGauge.BeginAngle = new NAngle(-90, NUnit.Degree);
			m_RadialGauge.PreferredHeight = 400;

            // configure the axis
            axis = new NGaugeAxis();
			m_RadialGauge.Axes.Add(axis);
            axis.Range = new NRange(0, 100);
			axis.Anchor = new NDockGaugeAxisAnchor(ENGaugeAxisDockZone.Top, true, ENScaleOrientation.Left, 0.0f, 100.0f);

			// configure the axis
			axis = new NGaugeAxis();
			m_RadialGauge.Axes.Add(axis);
			axis.Range = new NRange(0, 100);
			axis.Anchor = new NDockGaugeAxisAnchor(ENGaugeAxisDockZone.Top, true, ENScaleOrientation.Left, 0.0f, 50.0f);

			NNeedleValueIndicator indicator = new NNeedleValueIndicator();
			indicator.ScaleAxis = axis;
			indicator.OffsetFromScale = 20;
			m_RadialGauge.Indicators.Add(indicator);

			NMarkerValueIndicator markerValueIndicator = new NMarkerValueIndicator(10);
			markerValueIndicator.Shape = ENScaleValueMarkerShape.Bar;
			m_RadialGauge.Indicators.Add(new NMarkerValueIndicator(10));
			m_RadialGauge.Indicators.Add(new NMarkerValueIndicator(90));


			NNeedleValueIndicator needle = new NNeedleValueIndicator();
			needle.Value = 10;
			needle.Shape = ENNeedleShape.Needle4;

			needle.Fill = new NColorFill(NColor.DarkGreen);
			needle.Stroke = new NStroke(NColor.DarkGreen);

//			radialGauge.Indicators.Add(needle);

			markerValueIndicator.Width = 20;
			markerValueIndicator.Height = 20;
			markerValueIndicator.Fill = new NColorFill(NColor.DarkGreen);
			markerValueIndicator.Stroke = new NStroke(1.0, NColor.DarkGreen); 


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

			return stack;
        }
		protected override string GetExampleDescription()
		{
			return @"<p>The example demonstrates how to control the gauge axis position using gauge axis dock anchors.</p>";
		}

		#endregion 

		#region Fields

		NRadialGauge m_RadialGauge;
		NLinearGauge m_LinearGauge;

		#endregion

		#region Schema

		public static readonly NSchema NAxisDockingExampleSchema;

		#endregion

		#region Constants

		private static readonly NSize defaultLinearHorizontalGaugeSize = new NSize(300, 100);
		private static readonly NSize defaultRadialGaugeSize = new NSize(300, 300);

		#endregion
	}
}
