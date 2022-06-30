using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Stacked Percent Bar Example
	/// </summary>
	public class NStackedPercentBarExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NStackedPercentBarExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NStackedPercentBarExample()
		{
			NStackedPercentBarExampleSchema = NSchema.Create(typeof(NStackedPercentBarExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Stacked Percent Bar";

			// configure chart
			NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];

			chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XOrdinalYLinear);

			// add interlace stripe
			NLinearScale linearScale = (NLinearScale)chart.Axes[ENCartesianAxis.PrimaryY].Scale;
			linearScale.Labels.TextProvider = new NFormattedScaleLabelTextProvider(new NNumericValueFormatter("P"));
			NScaleStrip stripStyle = new NScaleStrip(new NColorFill(NColor.Beige), null, true, 0, 0, 1, 1);
			stripStyle.Interlaced = true;
			linearScale.Strips.Add(stripStyle);

			// add the first bar
			m_Bar1 = new NBarSeries();
			m_Bar1.Name = "Bar1";
			m_Bar1.MultiBarMode = ENMultiBarMode.Series;
			chart.Series.Add(m_Bar1);

			// add the second bar
			m_Bar2 = new NBarSeries();
			m_Bar2.Name = "Bar2";
			m_Bar2.MultiBarMode = ENMultiBarMode.StackedPercent;
			chart.Series.Add(m_Bar2);

			// add the third bar
			m_Bar3 = new NBarSeries();
			m_Bar3.Name = "Bar3";
			m_Bar3.MultiBarMode = ENMultiBarMode.StackedPercent;
			chart.Series.Add(m_Bar3);

			// setup value formatting
			m_Bar1.ValueFormatter = new NNumericValueFormatter("0.###");
			m_Bar2.ValueFormatter = new NNumericValueFormatter("0.###");
			m_Bar3.ValueFormatter = new NNumericValueFormatter("0.###");

			// position data labels in the center of the bars
			m_Bar1.DataLabelStyle = CreateDataLabelStyle();
			m_Bar2.DataLabelStyle = CreateDataLabelStyle();
			m_Bar3.DataLabelStyle = CreateDataLabelStyle();

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			// pass some data
			OnPositiveDataButtonClick(null);

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

			NComboBox firstBarLabelFormatComboBox = CreateLabelFormatComboBox();
			firstBarLabelFormatComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnFirstBarLabelFormatComboBoxSelectedIndexChanged);
			stack.Add(NPairBox.Create("First Bar Label Format: ", firstBarLabelFormatComboBox));

			NComboBox secondBarLabelFormatComboBox = CreateLabelFormatComboBox();
			secondBarLabelFormatComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnSecondBarLabelFormatComboBoxSelectedIndexChanged);
			stack.Add(NPairBox.Create("Second Bar Label Format: ", secondBarLabelFormatComboBox));

			NComboBox thirdBarLabelFormatComboBox = CreateLabelFormatComboBox();
			thirdBarLabelFormatComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnThirdBarLabelFormatComboBoxSelectedIndexChanged);
			stack.Add(NPairBox.Create("Third Bar Label Format: ", thirdBarLabelFormatComboBox));

			NButton positiveDataButton = new NButton("Positive Values");
			positiveDataButton.Click += new Function<NEventArgs>(OnPositiveDataButtonClick);
			stack.Add(positiveDataButton);

			return boxGroup;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a stacked percent bar chart.</p>";
		}

		#endregion

		#region Event Handlers

		void OnFirstBarLabelFormatComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			NComboBox comboBox = (NComboBox)arg.TargetNode;
			m_Bar1.DataLabelStyle.Format = (string)comboBox.Items[comboBox.SelectedIndex].Tag;
		}

		void OnSecondBarLabelFormatComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			NComboBox comboBox = (NComboBox)arg.TargetNode;
			m_Bar2.DataLabelStyle.Format = (string)comboBox.Items[comboBox.SelectedIndex].Tag;
		}

		void OnThirdBarLabelFormatComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			NComboBox comboBox = (NComboBox)arg.TargetNode;
			m_Bar3.DataLabelStyle.Format = (string)comboBox.Items[comboBox.SelectedIndex].Tag;
		}

		void OnPositiveDataButtonClick(NEventArgs arg)
		{
			m_Bar1.DataPoints.Clear();
			m_Bar2.DataPoints.Clear();
			m_Bar3.DataPoints.Clear();

			Random random = new Random();
			for (int i = 0; i < 12; i++)
			{
				m_Bar1.DataPoints.Add(new NBarDataPoint(random.Next(90) + 10));
				m_Bar2.DataPoints.Add(new NBarDataPoint(random.Next(90) + 10));
				m_Bar3.DataPoints.Add(new NBarDataPoint(random.Next(90) + 10));
			}
		}

		#endregion

		#region Implementation

		/// <summary>
		/// Creates a new data label style object
		/// </summary>
		/// <returns></returns>
		private NDataLabelStyle CreateDataLabelStyle()
		{
			NDataLabelStyle dataLabelStyle = new NDataLabelStyle();

			dataLabelStyle.VertAlign = ENVerticalAlignment.Center;
			dataLabelStyle.ArrowLength = 0;

			return dataLabelStyle;
		}
		/// <summary>
		/// Gets a format string from the specified index
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		private string GetFormatStringFromIndex(int index)
		{
			switch (index)
			{
				case 0:
					return "<value>";

				case 1:
					return "<total>";

				case 2:
					return "<cumulative>";

				case 3:
					return "<percent>";

				default:
					return "";
			}
		}
		/// <summary>
		/// Creates a label format combo box
		/// </summary>
		/// <returns></returns>
		private NComboBox CreateLabelFormatComboBox()
		{
			NComboBox comboBox = new NComboBox();

			NComboBoxItem comboBoxItem = new NComboBoxItem("Value");
			comboBoxItem.Tag = "<value>";
			comboBox.Items.Add(comboBoxItem);

			comboBoxItem = new NComboBoxItem("Total");
			comboBoxItem.Tag = "<total>";
			comboBox.Items.Add(comboBoxItem);

			comboBoxItem = new NComboBoxItem("Cumulative");
			comboBoxItem.Tag = "<cumulative>";
			comboBox.Items.Add(comboBoxItem);

			comboBoxItem = new NComboBoxItem("Percent");
			comboBoxItem.Tag = "<percent>";
			comboBox.Items.Add(comboBoxItem);

			comboBox.SelectedIndex = 0;

			return comboBox;
		}

		#endregion

		#region Fields

		NBarSeries m_Bar1;
		NBarSeries m_Bar2;
		NBarSeries m_Bar3;

		#endregion

		#region Schema

		public static readonly NSchema NStackedPercentBarExampleSchema;

		#endregion
	}
}
