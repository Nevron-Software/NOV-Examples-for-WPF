using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Axis stripes example
	/// </summary>
	public class NAxisStripsExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NAxisStripsExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NAxisStripsExample()
		{
			NAxisStripsExampleSchema = NSchema.Create(typeof(NAxisStripsExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			chartView.Surface.Titles[0].Text = "Axis Strips";

			// configure chart
			NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];

			// configure axes
			chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XOrdinalYLinear);
			NLinearScale scaleY = (NLinearScale)chart.Axes[ENCartesianAxis.PrimaryY].Scale;

			// add interlaced stripe
			m_Strip = new NScaleStrip(new NColorFill(NColor.DarkGray), null, true, 0, 0, 1, 1);
			m_Strip.Interlaced = true;
			scaleY.Strips.Add(m_Strip);

			// enable the major y grid lines
			scaleY.MajorGridLines = new NScaleGridLines();

			NOrdinalScale scaleX = (NOrdinalScale)chart.Axes[ENCartesianAxis.PrimaryX].Scale;

			// enable the major x grid lines
			scaleX.MajorGridLines = new NScaleGridLines();

			// create dummy data
			NBarSeries bar = new NBarSeries();
			bar.Name = "Bars";
			bar.DataLabelStyle = new NDataLabelStyle(false);
			Random random = new Random();
			for (int i = 0; i < 10; i++)
			{
				bar.DataPoints.Add(new NBarDataPoint(random.Next(100)));
			}

			chart.Series.Add(bar);

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

			stack.Add(new NLabel("Y Axis Grid"));

			NNumericUpDown beginUpDown = new NNumericUpDown();
			beginUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnBeginUpDownValueChanged);
			stack.Add(NPairBox.Create("Begin:", beginUpDown));
			beginUpDown.Value = 0;

			NNumericUpDown endUpDown = new NNumericUpDown();
			endUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnEndUpDownValueChanged);
			stack.Add(NPairBox.Create("End:", endUpDown));
			endUpDown.Value = 0;

			NCheckBox infiniteCheckBox = new NCheckBox("Infinite");
			infiniteCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnInfiniteCheckBoxCheckedChanged);
			stack.Add(infiniteCheckBox);
			infiniteCheckBox.Checked = true;

			NNumericUpDown lengthUpDown = new NNumericUpDown();
			lengthUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnLengthUpDownValueChanged);
			stack.Add(NPairBox.Create("Length:", lengthUpDown));
			lengthUpDown.Value = 1;

			NNumericUpDown intervalUpDown = new NNumericUpDown();
			intervalUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnIntervalUpDownValueChanged);
			stack.Add(NPairBox.Create("Interval:", intervalUpDown));
			intervalUpDown.Value = 1;

			NColorBox colorBox = new NColorBox();
			colorBox.SelectedColorChanged += new Function<NValueChangeEventArgs>(OnColorBoxSelectedColorChanged);
			stack.Add(NPairBox.Create("Color:", colorBox));
			colorBox.SelectedColor = NColor.DarkGray;

			return boxGroup;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to configure axis strips.</p>";
		}

		#endregion

		#region Event Handlers

		void OnIntervalUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_Strip.Interval = (int)((NNumericUpDown)arg.TargetNode).Value;
		}

		void OnLengthUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_Strip.Length = (int)((NNumericUpDown)arg.TargetNode).Value;
		}

		void OnColorBoxSelectedColorChanged(NValueChangeEventArgs arg)
		{
			m_Strip.Fill = new NColorFill(((NColorBox)arg.TargetNode).SelectedColor);
		}

		void OnInfiniteCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			m_Strip.Infinite = ((NCheckBox)arg.TargetNode).Checked;
		}

		void OnEndUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_Strip.End = (int)((NNumericUpDown)arg.TargetNode).Value;
		}

		void OnBeginUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_Strip.Begin = (int)((NNumericUpDown)arg.TargetNode).Value;
		}

		#endregion

		#region Fields

		NScaleStrip m_Strip;

		#endregion

		#region Schema

		public static readonly NSchema NAxisStripsExampleSchema;

		#endregion
	}
}