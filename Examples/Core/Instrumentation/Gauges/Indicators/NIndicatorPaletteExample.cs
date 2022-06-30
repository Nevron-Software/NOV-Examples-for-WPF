using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Gauge
{
	/// <summary>
	/// This example demonstrates how to associate a palette with an indicator
	/// </summary>
	public class NIndicatorPaletteExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public NIndicatorPaletteExample()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        static NIndicatorPaletteExample()
        {
			NIndicatorPaletteExampleSchema = NSchema.Create(typeof(NIndicatorPaletteExample), NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
		{
			NStackPanel stack = new NStackPanel();
			stack.HorizontalPlacement = ENHorizontalPlacement.Left;

			m_AxisRange = new NRange(80, 130);
			int indicatorCount = 4;
			m_IndicatorPhase = new double[indicatorCount];

			// create gauges
			CreateLinearGauge();
			CreateRadialGauge();

			// add to stack
			stack.Add(m_LinearGauge);
			stack.Add(m_RadialGauge);

			// add axes
			m_LinearGauge.Axes.Add(CreateGaugeAxis());
			m_RadialGauge.Axes.Add(CreateGaugeAxis());
			
			double offset = 10;

			// now add two indicators
			for (int i = 0; i < indicatorCount; i++)
			{
				m_IndicatorPhase[i] = i * 30;

				m_LinearGauge.Indicators.Add(CreateRangeIndicator(offset));
				offset += 20;
			}

			m_RadialGauge.Indicators.Add(CreateRangeIndicator(0));

			stack.Registered += OnStackRegistered;
			stack.Unregistered += OnStackUnregistered;

			return stack;
		}
		protected override NWidget CreateExampleControls()
        {
			NStackPanel stack = new NStackPanel();

			NStackPanel propertyStack = new NStackPanel();
			stack.Add(new NUniSizeBoxGroup(propertyStack));

			NButton toggleTimerButton = new NButton("Stop Timer");
			toggleTimerButton.Click += OnToggleTimerButtonClick;
			toggleTimerButton.Tag = 0;
			stack.Add(toggleTimerButton);

			NComboBox rangeIndicatorShapeCombo = new NComboBox();
			rangeIndicatorShapeCombo.FillFromEnum<ENRangeIndicatorShape>();
			rangeIndicatorShapeCombo.SelectedIndexChanged += OnRangeIndicatorShapeComboSelectedIndexChanged;
			rangeIndicatorShapeCombo.SelectedIndex = (int)ENRangeIndicatorShape.Bar;
			stack.Add(NPairBox.Create("Shape:", rangeIndicatorShapeCombo));

            NComboBox paletteColorModeCombo = new NComboBox();
            paletteColorModeCombo.FillFromEnum<ENPaletteColorMode>();
            paletteColorModeCombo.SelectedIndexChanged += OnPaletteColorModeComboSelectedIndexChanged;
            paletteColorModeCombo.SelectedIndex = (int)ENPaletteColorMode.Spread;
            stack.Add(NPairBox.Create("Palette Color Mode:", paletteColorModeCombo));
            
			NComboBox orientationCombo = new NComboBox();
			orientationCombo.FillFromEnum<ENLinearGaugeOrientation>();
			orientationCombo.SelectedIndexChanged +=OnOrientationComboSelectedIndexChanged;
			orientationCombo.SelectedIndex = (int)ENLinearGaugeOrientation.Vertical;
			stack.Add(NPairBox.Create("Orientation", orientationCombo));

			NNumericUpDown beginAngleUpDown = new NNumericUpDown();
			beginAngleUpDown.Value = m_RadialGauge.BeginAngle.Value;
			beginAngleUpDown.ValueChanged += OnBeginAngleUpDownValueChanged;
			stack.Add(NPairBox.Create("Begin Angle:", beginAngleUpDown));

			NNumericUpDown sweepAngleUpDown = new NNumericUpDown();
			sweepAngleUpDown.Value = m_RadialGauge.BeginAngle.Value;
			sweepAngleUpDown.ValueChanged += OnSweepAngleUpDownValueChanged;
			stack.Add(NPairBox.Create("Sweep Angle:", sweepAngleUpDown));

			NCheckBox invertScaleCheckBox = new NCheckBox("Invert Scale");
			invertScaleCheckBox.CheckedChanged += OnInvertScaleCheckBoxCheckedChanged;
			invertScaleCheckBox.Checked = false;
			stack.Add(invertScaleCheckBox);

			NCheckBox smoothPaletteCheckBox = new NCheckBox("Smooth Palette");
			smoothPaletteCheckBox.CheckedChanged += OnSmoothPaletteCheckBoxCheckedChanged;
			smoothPaletteCheckBox.Checked = true;
			stack.Add(smoothPaletteCheckBox);

			return stack;
        }
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to associate a palette with an indicator.</p>";
		}

		#endregion 

		#region Implementation

		private void CreateRadialGauge()
		{
			m_RadialGauge = new NRadialGauge();

			m_RadialGauge.PreferredSize = defaultRadialGaugeSize;
			m_RadialGauge.CapEffect = new NGelCapEffect();
			m_RadialGauge.Border = CreateBorder();
			m_RadialGauge.BorderThickness = new NMargins(6);
			m_RadialGauge.BackgroundFill = new NStockGradientFill(NColor.DarkGray, NColor.Black);
			m_RadialGauge.NeedleCap.Visible = false;
			m_RadialGauge.PreferredWidth = 200;
			m_RadialGauge.PreferredHeight = 200;
			m_RadialGauge.Padding = new NMargins(5);
		}

		private void CreateLinearGauge()
		{
			m_LinearGauge = new NLinearGauge();
			m_LinearGauge.Orientation = ENLinearGaugeOrientation.Vertical;
			m_LinearGauge.PreferredWidth = 200;
			m_LinearGauge.PreferredHeight = 300;
			m_LinearGauge.CapEffect = new NGlassCapEffect();
			m_LinearGauge.Border = CreateBorder();
			m_LinearGauge.Padding = new NMargins(20);
			m_LinearGauge.BorderThickness = new NMargins(6);
			m_LinearGauge.BackgroundFill = new NStockGradientFill(NColor.DarkGray, NColor.Black);

			m_LinearGauge.Axes.Clear();
		}

		private NGaugeAxis CreateGaugeAxis()
		{
			NGaugeAxis axis = new NGaugeAxis();
			axis.Range = m_AxisRange;
			axis.Anchor = new NDockGaugeAxisAnchor(ENGaugeAxisDockZone.Top, true, ENScaleOrientation.Left, 0, 100);
			axis.Scale.SetColor(NColor.White);
			((NLinearScale)axis.Scale).Labels.Style.TextStyle.Font.Style = ENFontStyle.Bold;
			return axis;
		}

		private NRangeIndicator CreateRangeIndicator(double offsetFromScale)
		{
			NRangeIndicator rangeIndicator = new NRangeIndicator();
			rangeIndicator.Value = 0;
			rangeIndicator.Stroke = null;
			rangeIndicator.OffsetFromScale = offsetFromScale;
			rangeIndicator.BeginWidth = 10;
			rangeIndicator.EndWidth = 10;

			rangeIndicator.BeginWidth = 10;
			rangeIndicator.EndWidth = 10;

			// assign palette to the indicator
			NColorValuePalette palette = new NColorValuePalette(new NColorValuePair[] { new NColorValuePair(80, NColor.Green), new NColorValuePair(100, NColor.Yellow), new NColorValuePair(120, NColor.Red) });
			rangeIndicator.Palette = palette; 

			return rangeIndicator;
		}

		#endregion

		#region Event Handlers

		private void OnStackRegistered(NEventArgs arg)
		{
			m_Timer = new NTimer();
			m_Timer.Tick += OnTimerTick;
			m_Timer.Start();
		}
		private void OnStackUnregistered(NEventArgs arg)
		{
			m_Timer.Stop();
			m_Timer.Tick -= OnTimerTick;
			m_Timer = null;
		}

		void OnSweepAngleUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_RadialGauge.SweepAngle = new NAngle(((NNumericUpDown)arg.TargetNode).Value);
		}

		void OnBeginAngleUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_RadialGauge.BeginAngle = new NAngle(((NNumericUpDown)arg.TargetNode).Value);
		}

		void OnInvertScaleCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			m_LinearGauge.Axes[0].Scale.Invert = ((NCheckBox)arg.TargetNode).Checked;
			m_RadialGauge.Axes[0].Scale.Invert = ((NCheckBox)arg.TargetNode).Checked;
		}

        void OnPaletteColorModeComboSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            ENPaletteColorMode paletteColorMode = (ENPaletteColorMode)((NComboBox)arg.TargetNode).SelectedIndex;

            for (int i = 0; i < m_LinearGauge.Indicators.Count; i++)
            {
                ((NRangeIndicator)m_LinearGauge.Indicators[i]).PaletteColorMode = paletteColorMode;
            }

            ((NRangeIndicator)m_RadialGauge.Indicators[0]).PaletteColorMode = paletteColorMode;
        }

		void OnOrientationComboSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_LinearGauge.Orientation = (ENLinearGaugeOrientation)((NComboBox)arg.TargetNode).SelectedIndex;

			switch (m_LinearGauge.Orientation)
			{
				case ENLinearGaugeOrientation.Horizontal:
					m_LinearGauge.PreferredWidth = 300;
					m_LinearGauge.PreferredHeight = 200;
					break;
				case ENLinearGaugeOrientation.Vertical:
					m_LinearGauge.PreferredWidth = 200;
					m_LinearGauge.PreferredHeight = 300;
					break;
			}
		}

		void OnRangeIndicatorShapeComboSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			ENRangeIndicatorShape shape = (ENRangeIndicatorShape)((NComboBox)arg.TargetNode).SelectedIndex;

			for (int i = 0; i < m_LinearGauge.Indicators.Count; i++)
			{
				((NRangeIndicator)m_LinearGauge.Indicators[i]).Shape = shape;
			}

			((NRangeIndicator)m_RadialGauge.Indicators[0]).Shape = shape;
		}

		void OnToggleTimerButtonClick(NEventArgs arg)
		{
			NButton button = (NButton)arg.TargetNode;
			if ((int)button.Tag == 0)
			{
				m_Timer.Stop();

				button.Content = new NLabel("Start Timer");
				button.Tag = 1;
			}
			else
			{
				m_Timer.Start();
				button.Content = new NLabel("Stop Timer");
				button.Tag = 0;
			}
		}

		void OnTimerTick()
		{
			Random random = new Random();

			for (int i = 0; i < m_LinearGauge.Indicators.Count; i++)
			{
				double value = (m_AxisRange.Begin + m_AxisRange.End) / 2.0 + Math.Sin(m_IndicatorPhase[i] * NAngle.Degree2Rad) * m_AxisRange.GetLength() / 2 + random.Next(20);
				value = m_AxisRange.GetValueInRange(value);

				((NRangeIndicator)m_LinearGauge.Indicators[i]).Value = value;
				m_IndicatorPhase[i] += 10;
			}

			((NRangeIndicator)m_RadialGauge.Indicators[0]).Value = ((NRangeIndicator)m_LinearGauge.Indicators[0]).Value;
		}

		void OnSmoothPaletteCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			bool smoothPalette = ((NCheckBox)arg.TargetNode).Checked;

			for (int i = 0; i < m_LinearGauge.Indicators.Count; i++)
			{
				m_LinearGauge.Indicators[i].Palette.SmoothColors = smoothPalette;
			}

			m_RadialGauge.Indicators[0].Palette.SmoothColors = smoothPalette;
		}

		#endregion

		#region Fields

		NLinearGauge m_LinearGauge;
		NRadialGauge m_RadialGauge;
		NTimer m_Timer;
		double[] m_IndicatorPhase;
		NRange m_AxisRange;

		#endregion

		#region Schema

		public static readonly NSchema NIndicatorPaletteExampleSchema;

		#endregion

		#region Static Methods

		private static NBorder CreateBorder()
		{
			return NBorder.CreateThreeColorBorder(NColor.LightGray, NColor.White, NColor.DarkGray, 10, 10);
		}

		#endregion

		#region Constants

		private static readonly NSize defaultRadialGaugeSize = new NSize(300, 300);

		#endregion
	}
}