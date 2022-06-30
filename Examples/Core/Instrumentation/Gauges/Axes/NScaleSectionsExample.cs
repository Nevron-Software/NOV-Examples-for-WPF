using System;
using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;
using Nevron.Nov.Layout;
using Nevron.Nov.Editors;

namespace Nevron.Nov.Examples.Gauge
{
    /// <summary>
	/// This example demonstrates how to control the size of the gauge axes
    /// </summary>
	public class NScaleSectionsExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public NScaleSectionsExample()
        {
        }
        /// <summary>
        /// Static constructor
        /// </summary>
        static NScaleSectionsExample()
        {
			NScaleSectionsExampleSchema = NSchema.Create(typeof(NScaleSectionsExample), NExampleBaseSchema);
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
			controlStack.Direction = ENHVDirection.LeftToRight;
			stack.Add(controlStack);

			m_LinearGauge = new NLinearGauge();
			m_LinearGauge.Orientation = ENLinearGaugeOrientation.Vertical;
			m_LinearGauge.PreferredSize = defaultLinearVerticalGaugeSize;
			m_LinearGauge.BackgroundFill = new NStockGradientFill(NColor.DarkGray, NColor.Black);
			m_LinearGauge.CapEffect = new NGelCapEffect();
			m_LinearGauge.Border = CreateBorder();
			m_LinearGauge.Padding = new NMargins(20);
			m_LinearGauge.BorderThickness = new NMargins(6);

			controlStack.Add(m_LinearGauge);

			NMarkerValueIndicator markerIndicator = new NMarkerValueIndicator();
			m_LinearGauge.Indicators.Add(markerIndicator);

			InitSections(m_LinearGauge);

			// create the radial gauge
			m_RadialGauge = new NRadialGauge();
			m_RadialGauge.PreferredSize = defaultRadialGaugeSize;
			NEdgeDialRim dialRim = new NEdgeDialRim();
			dialRim.OuterBevelWidth = 2.0;
			dialRim.InnerBevelWidth = 2.0;
			dialRim.MiddleBevelWidth = 2.0;
			m_RadialGauge.Dial = new NDial(ENDialShape.CutCircle, dialRim);
			m_RadialGauge.Dial.BackgroundFill = new NStockGradientFill(NColor.DarkGray, NColor.Black);
			m_RadialGauge.InnerRadius = 15;

			NGlassCapEffect glassCapEffect = new NGlassCapEffect();
			glassCapEffect.LightDirection = new NAngle(130, NUnit.Degree);
			glassCapEffect.EdgeOffset = 0;
			glassCapEffect.EdgeDepth = 0.30;
			m_RadialGauge.CapEffect = glassCapEffect;

			controlStack.Add(m_RadialGauge);

			NNeedleValueIndicator needleIndicator = new NNeedleValueIndicator();
			m_RadialGauge.Indicators.Add(needleIndicator);
			m_RadialGauge.SweepAngle = new NAngle(180, NUnit.Degree);

			InitSections(m_RadialGauge);

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
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        protected override NWidget CreateExampleControls()
        {
			NStackPanel stack = new NStackPanel();

			NStackPanel propertyStack = new NStackPanel();
			stack.Add(new NUniSizeBoxGroup(propertyStack));

			m_BlueSectionBeginUpDown = CreateUpDown(0.0);
			propertyStack.Add(new NPairBox("Begin:", m_BlueSectionBeginUpDown, true));

			m_BlueSectionEndUpDown = CreateUpDown(20.0);
			propertyStack.Add(new NPairBox("End:", m_BlueSectionEndUpDown, true));

			m_RedSectionBeginUpDown = CreateUpDown(80.0);
			propertyStack.Add(new NPairBox("Begin:", m_RedSectionBeginUpDown, true));

			m_RedSectionEndUpDown = CreateUpDown(100.0);
			propertyStack.Add(new NPairBox("End:", m_RedSectionEndUpDown, true));

			m_StopStartTimerButton = new NButton("Stop Timer");
			propertyStack.Add(m_StopStartTimerButton);
		

			return stack;
        }

		protected override string GetExampleDescription()
		{
			return @"<p>The example demonstrates how to create scale sections. Scale sections allow you to modify the appearance of scale elements if they fall in certain range.</p>";
		}

		#endregion 

		#region Event Handlers

		/// <summary>
		/// 
		/// </summary>
		void OnDataFeedTimerTick()
		{
			// update linear gauge
			NGauge[] gauges = new NGauge[] { m_RadialGauge, m_LinearGauge };

			for (int i = 0; i < gauges.Length; i++) 
			{
				NGauge gauge = gauges[i];

				NValueIndicator valueIndicator = (NValueIndicator)gauge.Indicators[0];
				NStandardScale scale = (NStandardScale)gauge.Axes[0].Scale;

				NScaleSection blueSection = (NScaleSection)scale.Sections[0];
				NScaleSection redSection = (NScaleSection)scale.Sections[1];

				m_FirstIndicatorAngle += 0.02;
				valueIndicator.Value = 50.0 - Math.Cos(m_FirstIndicatorAngle) * 50.0;

				// FIX: Smart Shapes
				if (blueSection.Range.Contains(valueIndicator.Value))
				{
//					valueIndicator.Shape.FillStyle = new NStockGradientFill(ENGradientStyle.Horizontal, ENGradientVariant.Variant1, NColor.White, NColor.Blue);
//					valueIndicator.Shape.StrokeStyle = new NStrokeStyle(Color.Blue);
				}
				else if (redSection.Range.Contains(valueIndicator.Value))
				{
//					valueIndicator.Shape.FillStyle = new NGradientFillStyle(GradientStyle.Horizontal, GradientVariant.Variant1, Color.White, Color.Red);
//					valueIndicator.Shape.StrokeStyle = new NStrokeStyle(Color.Red);
				}
				else
				{
//					valueIndicator.Shape.FillStyle = new NColorFillStyle(Color.LightGreen);
//					valueIndicator.Shape.StrokeStyle = new NStrokeStyle(Color.DarkGreen);
				}
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnStopStartTimerButtonClick(object sender, System.EventArgs e)
		{
			if (this.m_DataFeedTimer.IsStarted)
			{
				this.m_DataFeedTimer.Stop();
				m_StopStartTimerButton.Content = new NLabel("Start Timer");
			}
			else
			{
				this.m_DataFeedTimer.Start();
				m_StopStartTimerButton.Content = new NLabel("Stop Timer");
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="arg"></param>
		void UpdateSections(NValueChangeEventArgs arg)
		{
			NGauge[] gauges = new NGauge[] { m_RadialGauge, m_LinearGauge };

			for (int i = 0; i < gauges.Length; i++)
			{
				NGauge gauge = gauges[i];

				NGaugeAxis axis = (NGaugeAxis)gauge.Axes[0];
				NStandardScale scale = (NStandardScale)axis.Scale;

				if (scale.Sections.Count == 2)
				{
					NScaleSection blueSection = (NScaleSection)scale.Sections[0];
					blueSection.Range = new NRange(m_BlueSectionBeginUpDown.Value, m_BlueSectionEndUpDown.Value);

					NScaleSection redSection = (NScaleSection)scale.Sections[1];
					redSection.Range = new NRange(m_RedSectionBeginUpDown.Value, m_RedSectionEndUpDown.Value);
				}
			}
		}
	
		#endregion

		#region Implementation

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private NNumericUpDown CreateUpDown(double value)
		{
			NNumericUpDown numericUpDown = new NNumericUpDown();

			numericUpDown.Minimum = 0.0;
			numericUpDown.Maximum = 100.0;
			numericUpDown.Value = value;
			numericUpDown.ValueChanged += new Function<NValueChangeEventArgs>(UpdateSections);

			return numericUpDown;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="gauge"></param>
		private void InitSections(NGauge gauge)
		{
			gauge.Axes.Clear();
			NGaugeAxis axis = new NGaugeAxis();
			gauge.Axes.Add(axis);

			axis.Anchor = new NDockGaugeAxisAnchor(ENGaugeAxisDockZone.Top);

			NStandardScale scale = (NStandardScale)axis.Scale;

			// init text style for regular labels
			scale.Labels.Style.TextStyle.Fill = new NColorFill(NColor.White);
			scale.Labels.Style.TextStyle.Font = new NFont("Arimo", 10, ENFontStyle.Bold);

			// init ticks
			scale.MajorGridLines.Visible = true;
			scale.MinTickDistance = 25;
			scale.MinorTickCount = 1;
			scale.SetPredefinedScale(ENPredefinedScaleStyle.Scientific);

			// create sections
			NScaleSection blueSection = new NScaleSection();
			blueSection.Range = new NRange(0, 20);
			blueSection.RangeFill = new NColorFill(NColor.FromColor(NColor.Blue, 0.5f));
			blueSection.MajorGridStroke = new NStroke(NColor.Blue);
			blueSection.MajorTickStroke = new NStroke(NColor.DarkBlue);
			blueSection.MinorTickStroke = new NStroke(1, NColor.Blue, ENDashStyle.Dot);

			NTextStyle labelStyle = new NTextStyle();
			labelStyle.Fill = new NColorFill(NColor.Blue);
			labelStyle.Font = new NFont("Arimo", 10, ENFontStyle.Bold);
			blueSection.LabelTextStyle = labelStyle;

			scale.Sections.Add(blueSection);

			NScaleSection redSection = new NScaleSection();
			redSection.Range = new NRange(80, 100);

			redSection.RangeFill = new NColorFill(NColor.FromColor(NColor.Red, 0.5f));
			redSection.MajorGridStroke = new NStroke(NColor.Red);
			redSection.MajorTickStroke = new NStroke(NColor.DarkRed);
			redSection.MinorTickStroke = new NStroke(1, NColor.Red, ENDashStyle.Dot);

			labelStyle = new NTextStyle();
			labelStyle.Fill = new NColorFill(NColor.Red);
			labelStyle.Font = new NFont("Arimo", 10.0, ENFontStyle.Bold);
			redSection.LabelTextStyle = labelStyle;

			scale.Sections.Add(redSection);
		}

		#endregion

		#region Fields


		NNumericUpDown m_BlueSectionBeginUpDown;
		NNumericUpDown m_BlueSectionEndUpDown;

		NNumericUpDown m_RedSectionBeginUpDown;
		NNumericUpDown m_RedSectionEndUpDown;

		NRadialGauge m_RadialGauge;
		NLinearGauge m_LinearGauge;

		NTimer m_DataFeedTimer;
		NButton m_StopStartTimerButton;
		double m_FirstIndicatorAngle;

		#endregion

		#region Schema

		public static readonly NSchema NScaleSectionsExampleSchema;

		#endregion

		#region Static Methods

		protected NBorder CreateBorder()
		{
			return NBorder.CreateThreeColorBorder(NColor.LightGray, NColor.White, NColor.DarkGray, 10, 10);
		}

		#endregion

		#region Constants

		private static readonly NSize defaultRadialGaugeSize = new NSize(300, 300);
		private static readonly NSize defaultLinearVerticalGaugeSize = new NSize(100, 300);

		#endregion
	}
}
