using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Diagram.UI;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Gauge
{
	/// <summary>
	/// This example demonstrates how to control the paint order of gauge indicators
	/// </summary>
	public class NIndicatorPaintOrderExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        ///	Initializer constructor
        /// </summary>
        public NIndicatorPaintOrderExample()
        {
        }
        /// <summary>
        /// Static constructor
        /// </summary>
        static NIndicatorPaintOrderExample()
        {
			NIndicatorPaintOrderExampleSchema = NSchema.Create(typeof(NIndicatorPaintOrderExample), NExampleBaseSchema);
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
			m_RadialGauge.SweepAngle = new NAngle(270, NUnit.Degree);
			m_RadialGauge.BeginAngle = new NAngle(-225, NUnit.Degree);

			m_RadialGauge.PreferredSize = defaultRadialGaugeSize;

			m_RadialGauge.CapEffect = new NGlassCapEffect();
			m_RadialGauge.Dial = new NDial(ENDialShape.RoundedOutline, new NEdgeDialRim());
			m_RadialGauge.Dial.BackgroundFill = new NStockGradientFill(NColor.DarkGray, NColor.Black);

			// configure scale
			NGaugeAxis axis = new NGaugeAxis();
			m_RadialGauge.Axes.Add(axis);

			NLinearScale scale = (NLinearScale)axis.Scale;

			scale.SetPredefinedScale(ENPredefinedScaleStyle.Presentation);
			scale.Labels.Style.TextStyle.Font = new NFont("Arimo", 10, ENFontStyle.Bold);
			scale.Labels.Style.TextStyle.Fill = new NColorFill(NColor.White);
			scale.Labels.Style.Angle = new NScaleLabelAngle(ENScaleLabelAngleMode.Scale, 90.0);
			scale.MinorTickCount = 4;
			scale.Ruler.Stroke.Width = 0;
			scale.Ruler.Fill = new NColorFill(NColor.DarkGray);

			// add radial gauge indicators
			m_ValueIndicator = new NNeedleValueIndicator();
			m_ValueIndicator.Fill = new NStockGradientFill(ENGradientStyle.Horizontal, ENGradientVariant.Variant1, NColor.White, NColor.Red);
			m_ValueIndicator.Stroke.Color = NColor.Red;
			m_ValueIndicator.Width = 15;
			m_ValueIndicator.OffsetFromScale = -10;
			m_RadialGauge.Indicators.Add(m_ValueIndicator);

			NStackPanel verticalStack = new NStackPanel();
			verticalStack.Direction = Layout.ENHVDirection.TopToBottom;
			verticalStack.Padding = new NMargins(80, 200, 80, 0);

			m_NumericLedDisplay = new NNumericLedDisplay();

			m_NumericLedDisplay.Value = 0.0;
			m_NumericLedDisplay.CellCountMode = ENDisplayCellCountMode.Fixed;
			m_NumericLedDisplay.CellCount = 7;
			m_NumericLedDisplay.BackgroundFill = new NColorFill(NColor.Black);

			m_NumericLedDisplay.Border = NBorder.CreateSunken3DBorder(new NUIThemeColorMap(ENUIThemeScheme.WindowsClassic));
			m_NumericLedDisplay.BorderThickness = new NMargins(6);
			m_NumericLedDisplay.Margins = new NMargins(5);
			m_NumericLedDisplay.Padding = new NMargins(5);
			NGelCapEffect gelCap = new NGelCapEffect();
			gelCap.Shape = ENCapEffectShape.RoundedRect;
			m_NumericLedDisplay.CapEffect = gelCap;
			m_NumericLedDisplay.PreferredHeight = 60;

			verticalStack.Add(m_NumericLedDisplay);

			m_RadialGauge.Content = verticalStack;

			// add radial gauge
			controlStack.Add(m_RadialGauge);

			m_DataFeedTimer = new NTimer();
			m_DataFeedTimer.Tick += new Function(OnDataFeedTimerTick);
			m_DataFeedTimer.Start();

			return stack;
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void OnUnregistered()
		{
			base.OnUnregistered();

			m_DataFeedTimer.Stop();
		}

        protected override NWidget CreateExampleControls()
        {
			NStackPanel stack = new NStackPanel();

			NStackPanel propertyStack = new NStackPanel();
			stack.Add(new NUniSizeBoxGroup(propertyStack));

			propertyStack.Add(new NLabel("Paint Order:"));
			m_PaintOrderComboBox = new NComboBox();
			propertyStack.Add(m_PaintOrderComboBox);
			m_PaintOrderComboBox.FillFromEnum<ENIndicatorPaintOrder>();
			m_PaintOrderComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnPaintOrderComboBoxSelectedIndexChanged);
			m_PaintOrderComboBox.SelectedIndex = (int)ENIndicatorPaintOrder.AfterScale;

            return stack;
        }

		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to control the paint order of gauge indicators.</p>";
		}


		#endregion 

		#region Event Handlers

		/// <summary>
		/// 
		/// </summary>
		/// <param name="arg"></param>
		void OnPaintOrderComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			// set the paint order
			m_ValueIndicator.PaintOrder = (ENIndicatorPaintOrder)m_PaintOrderComboBox.SelectedIndex;
		}
		/// <summary>
		/// 
		/// </summary>
		void OnDataFeedTimerTick()
		{
			// update the indicator and the numeric led display
			m_FirstIndicatorAngle += 0.02;
			double value = 50.0 - Math.Cos(m_FirstIndicatorAngle) * 50.0;

			m_ValueIndicator.Value = value;
			m_NumericLedDisplay.Value = value;
		}

		#endregion

		#region Fields

		NRadialGauge m_RadialGauge;
		NNeedleValueIndicator m_ValueIndicator;
		NNumericLedDisplay m_NumericLedDisplay;
		NTimer m_DataFeedTimer;
		double m_FirstIndicatorAngle;

		NComboBox m_PaintOrderComboBox;

		#endregion

		#region Schema

		public static readonly NSchema NIndicatorPaintOrderExampleSchema;

		#endregion

		#region Constants

		private static readonly NSize defaultRadialGaugeSize = new NSize(300, 300);

		#endregion
	}
}
