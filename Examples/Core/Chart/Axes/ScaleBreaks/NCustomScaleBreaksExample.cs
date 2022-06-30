using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Custom Scale Breaks Example
	/// </summary>
	public class NCustomScaleBreaksExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NCustomScaleBreaksExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NCustomScaleBreaksExample()
		{
			NCustomScaleBreaksExampleSchema = NSchema.Create(typeof(NCustomScaleBreaksExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Custom Scale Breaks";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];
			
			// configure axes
			m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XYLinear);

			Random random = new Random();

			// create three random point series
			for (int i = 0; i < 3; i++)
			{
				NPointSeries point = new NPointSeries();
				point.UseXValues = true;

				point.DataLabelStyle = new NDataLabelStyle(false);
				point.Size = 5;

				// fill in some random data
				for (int j = 0; j < 30; j++)
				{
					point.DataPoints.Add(new NPointDataPoint(5 + random.Next(90), 5 + random.Next(90)));
				}

				m_Chart.Series.Add(point);
			}

			// create scale breaks

			NScale xScale = m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale;

			m_FirstHorzScaleBreak = CreateCustomScaleBreak(NColor.Orange, new NRange(10, 20));
			xScale.ScaleBreaks.Add(m_FirstHorzScaleBreak);

			m_SecondHorzScaleBreak = CreateCustomScaleBreak(NColor.Green, new NRange(80, 90));
			xScale.ScaleBreaks.Add(m_SecondHorzScaleBreak);

			NScale yScale = m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale;

			m_FirstVertScaleBreak = CreateCustomScaleBreak(NColor.Red, new NRange(10, 20));
			yScale.ScaleBreaks.Add(m_FirstVertScaleBreak);

			m_SecondVertScaleBreak = CreateCustomScaleBreak(NColor.Blue, new NRange(80, 90));
			yScale.ScaleBreaks.Add(m_SecondVertScaleBreak);

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

			stack.Add(new  NLabel("First Horizontal Scale Break"));

			NNumericUpDown firstHScaleBreakBegin = new NNumericUpDown();
			firstHScaleBreakBegin.ValueChanged +=new Function<NValueChangeEventArgs>(OnFirstHScaleBreakBeginValueChanged);
			stack.Add(NPairBox.Create("Begin:", firstHScaleBreakBegin));

			NNumericUpDown firstHScaleBreakEnd = new NNumericUpDown();
			firstHScaleBreakEnd.ValueChanged +=new Function<NValueChangeEventArgs>(OnFirstHScaleBreakEndValueChanged);
			stack.Add(NPairBox.Create("End:", firstHScaleBreakEnd));

			stack.Add(new  NLabel("Second Horizontal Scale Break"));

			NNumericUpDown secondHScaleBreakBegin = new NNumericUpDown();
			secondHScaleBreakBegin.ValueChanged +=new Function<NValueChangeEventArgs>(OnSecondHScaleBreakBeginValueChanged);
			stack.Add(NPairBox.Create("Begin:", secondHScaleBreakBegin));

			NNumericUpDown secondHScaleBreakEnd = new NNumericUpDown();
			secondHScaleBreakEnd.ValueChanged +=new Function<NValueChangeEventArgs>(OnSecondHScaleBreakEndValueChanged);
			stack.Add(NPairBox.Create("End:", secondHScaleBreakEnd));

			stack.Add(new NLabel("First Vertical Scale Break"));

			NNumericUpDown firstVScaleBreakBegin = new NNumericUpDown();
			firstVScaleBreakBegin.ValueChanged += new Function<NValueChangeEventArgs>(OnFirstVScaleBreakBeginValueChanged);
			stack.Add(NPairBox.Create("Begin:", firstVScaleBreakBegin));

			NNumericUpDown firstVScaleBreakEnd = new NNumericUpDown();
			firstVScaleBreakEnd.ValueChanged += new Function<NValueChangeEventArgs>(OnFirstVScaleBreakEndValueChanged);
			stack.Add(NPairBox.Create("End:", firstVScaleBreakEnd));

			stack.Add(new NLabel("Second Vertical Scale Break"));

			NNumericUpDown secondVScaleBreakBegin = new NNumericUpDown();
			secondVScaleBreakBegin.ValueChanged += new Function<NValueChangeEventArgs>(OnSecondVScaleBreakBeginValueChanged);
			stack.Add(NPairBox.Create("Begin:", secondVScaleBreakBegin));

			NNumericUpDown secondVScaleBreakEnd = new NNumericUpDown();
			secondVScaleBreakEnd.ValueChanged += new Function<NValueChangeEventArgs>(OnSecondVScaleBreakEndValueChanged);
			stack.Add(NPairBox.Create("End:", secondVScaleBreakEnd));

			firstHScaleBreakBegin.Value = 10;
			firstHScaleBreakEnd.Value = 20;

			secondHScaleBreakBegin.Value = 80;
			secondHScaleBreakEnd.Value = 90;

			firstVScaleBreakBegin.Value = 10;
			firstVScaleBreakEnd.Value = 20;

			secondVScaleBreakBegin.Value = 80;
			secondVScaleBreakEnd.Value = 90;
		
			return boxGroup;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to add custom scale breaks.</p>";
		}

		#endregion

		#region Implementation

		private NCustomScaleBreak CreateCustomScaleBreak(NColor color, NRange range)
		{
			NCustomScaleBreak scaleBreak = new NCustomScaleBreak();

			scaleBreak.Fill = new NColorFill(new NColor(color, 124));
			scaleBreak.Length = 10;
			scaleBreak.Range = range;
			
			return scaleBreak;
		}

		#endregion

		#region Event Handlers

		void OnSecondVScaleBreakEndValueChanged(NValueChangeEventArgs arg)
		{
			m_SecondVertScaleBreak.Range = new NRange(m_SecondVertScaleBreak.Range.Begin, ((NNumericUpDown)arg.TargetNode).Value);
		}

		void OnSecondVScaleBreakBeginValueChanged(NValueChangeEventArgs arg)
		{
			m_SecondVertScaleBreak.Range = new NRange(((NNumericUpDown)arg.TargetNode).Value, m_SecondVertScaleBreak.Range.End);
		}

		void OnFirstVScaleBreakEndValueChanged(NValueChangeEventArgs arg)
		{
			m_FirstVertScaleBreak.Range = new NRange(m_FirstVertScaleBreak.Range.Begin, ((NNumericUpDown)arg.TargetNode).Value);
		}

		void OnFirstVScaleBreakBeginValueChanged(NValueChangeEventArgs arg)
		{
			m_FirstVertScaleBreak.Range = new NRange(((NNumericUpDown)arg.TargetNode).Value, m_FirstVertScaleBreak.Range.End);
		}

		void OnSecondHScaleBreakEndValueChanged(NValueChangeEventArgs arg)
		{
			m_SecondHorzScaleBreak.Range = new NRange(m_SecondHorzScaleBreak.Range.Begin, ((NNumericUpDown)arg.TargetNode).Value);
		}

		void OnSecondHScaleBreakBeginValueChanged(NValueChangeEventArgs arg)
		{
			m_SecondHorzScaleBreak.Range = new NRange(((NNumericUpDown)arg.TargetNode).Value, m_SecondHorzScaleBreak.Range.End);
		}

		void OnFirstHScaleBreakEndValueChanged(NValueChangeEventArgs arg)
		{
			m_FirstHorzScaleBreak.Range = new NRange(m_FirstHorzScaleBreak.Range.Begin, ((NNumericUpDown)arg.TargetNode).Value);
		}

		void OnFirstHScaleBreakBeginValueChanged(NValueChangeEventArgs arg)
		{
			m_FirstHorzScaleBreak.Range = new NRange(((NNumericUpDown)arg.TargetNode).Value, m_FirstHorzScaleBreak.Range.End);
		}

		#endregion

		#region Fields

		NCartesianChart m_Chart;

		NCustomScaleBreak m_FirstHorzScaleBreak;
		NCustomScaleBreak m_SecondHorzScaleBreak;
		NCustomScaleBreak m_FirstVertScaleBreak;
		NCustomScaleBreak m_SecondVertScaleBreak;

		#endregion

		#region Schema

		public static readonly NSchema NCustomScaleBreaksExampleSchema;

		#endregion
	}
}