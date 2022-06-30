using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Stacked Area Example
	/// </summary>
	public class NStackedAreaExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NStackedAreaExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NStackedAreaExample()
		{
			NStackedAreaExampleSchema = NSchema.Create(typeof(NStackedAreaExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_ChartView = new NChartView();
			m_ChartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			m_ChartView.Surface.Titles[0].Text = "Stacked Area";

			// configure chart
			NCartesianChart chart = (NCartesianChart)m_ChartView.Surface.Charts[0];

			chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XOrdinalYLinear);

			// setup X axis
			NOrdinalScale scaleX = (NOrdinalScale)chart.Axes[ENCartesianAxis.PrimaryX].Scale;
			scaleX.InflateContentRange = false;

			// add interlaced stripe for Y axis
			NScaleStrip strip = new NScaleStrip(new NColorFill(NColor.Beige), null, true, 0, 0, 1, 1);
			strip.Interlaced = true;

			NLinearScale scaleY = (NLinearScale)chart.Axes[ENCartesianAxis.PrimaryY].Scale;
			scaleY.Strips.Add(strip);

			// add the first area
			m_Area1 = new NAreaSeries();
			m_Area1.MultiAreaMode = ENMultiAreaMode.Series;
			m_Area1.Name = "Product A";
			m_Area1.DataLabelStyle = new NDataLabelStyle();
			chart.Series.Add(m_Area1);
			SetupDataLabels(m_Area1);

			// add the second Area
			m_Area2 = new NAreaSeries();
			m_Area2.MultiAreaMode = ENMultiAreaMode.Stacked;
			m_Area2.Name = "Product B";
			m_Area2.DataLabelStyle = new NDataLabelStyle();
			chart.Series.Add(m_Area2);
			SetupDataLabels(m_Area2);

			// add the third Area
			m_Area3 = new NAreaSeries();
			m_Area3.MultiAreaMode = ENMultiAreaMode.Stacked;
			m_Area3.Name = "Product C";
			m_Area3.DataLabelStyle = new NDataLabelStyle();
			chart.Series.Add(m_Area3);
			SetupDataLabels(m_Area3);

			// fill with random data
			Random random = new Random();
			for (int i = 0; i < 10; i++)
			{
				m_Area1.DataPoints.Add(new NAreaDataPoint(random.Next(20, 50)));
				m_Area2.DataPoints.Add(new NAreaDataPoint(random.Next(20, 50)));
				m_Area3.DataPoints.Add(new NAreaDataPoint(random.Next(20, 50)));
			}

			return m_ChartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

			NComboBox stackModeComboBox = new NComboBox();
			stackModeComboBox.Items.Add(new NComboBoxItem("Stacked"));
			stackModeComboBox.Items.Add(new NComboBoxItem("Stacked Percent"));
			stackModeComboBox.SelectedIndex = 0;
			stackModeComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnStackModeComboBoxSelectedIndexChanged);
			stack.Add(NPairBox.Create("Stack Mode: ", stackModeComboBox));

			NCheckBox showDataLabelsCheckBox = new NCheckBox("Show Data Labels");
			showDataLabelsCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnShowDataLabelsCheckedChanged);
			stack.Add(showDataLabelsCheckBox);
			
			m_Area1LabelFormatCombox = CreateLabelsCombo();
			m_Area1LabelFormatCombox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnArea1LabelFormatComboxSelectedIndexChanged);
			stack.Add(NPairBox.Create("Area 1 Label Format: ", m_Area1LabelFormatCombox));

			m_Area2LabelFormatCombox = CreateLabelsCombo();
			m_Area2LabelFormatCombox.SelectedIndexChanged +=new Function<NValueChangeEventArgs>(OnArea2LabelFormatComboxSelectedIndexChanged);
			stack.Add(NPairBox.Create("Area 2 Label Format: ", m_Area2LabelFormatCombox));

			m_Area3LabelFormatCombox = CreateLabelsCombo();
			m_Area3LabelFormatCombox.SelectedIndexChanged +=new Function<NValueChangeEventArgs>(OnArea3LabelFormatComboxSelectedIndexChanged);
			stack.Add(NPairBox.Create("Area 3 Label Format: ", m_Area3LabelFormatCombox));

			showDataLabelsCheckBox.Checked = true;

			return boxGroup;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a stacked area chart.</p>";
		}

		#endregion

		#region Event Handlers

		void OnStackModeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			// configure chart
			NCartesianChart chart = (NCartesianChart)m_ChartView.Surface.Charts[0];
			NLinearScale scale = (NLinearScale)chart.Axes[ENCartesianAxis.PrimaryY].Scale;

			switch (((NComboBox)arg.TargetNode).SelectedIndex)
			{
				case 0:
					m_Area2.MultiAreaMode = ENMultiAreaMode.Stacked;
					m_Area3.MultiAreaMode = ENMultiAreaMode.Stacked;
					scale.Labels.TextProvider = new NFormattedScaleLabelTextProvider(new NNumericValueFormatter(ENNumericValueFormat.General));
					break;

				case 1:
					m_Area2.MultiAreaMode = ENMultiAreaMode.StackedPercent;
					m_Area3.MultiAreaMode = ENMultiAreaMode.StackedPercent;
					scale.Labels.TextProvider = new NFormattedScaleLabelTextProvider(new NNumericValueFormatter(ENNumericValueFormat.Percentage));
					break;
			}
		}

		void OnShowDataLabelsCheckedChanged(NValueChangeEventArgs arg)
		{
			NCheckBox showDataLabelsCheckBox = (NCheckBox)arg.TargetNode;

			m_Area1.DataLabelStyle.Visible = showDataLabelsCheckBox.Checked;
			m_Area2.DataLabelStyle.Visible = showDataLabelsCheckBox.Checked;
			m_Area3.DataLabelStyle.Visible = showDataLabelsCheckBox.Checked;

			m_Area1LabelFormatCombox.Enabled = showDataLabelsCheckBox.Checked;
			m_Area2LabelFormatCombox.Enabled = showDataLabelsCheckBox.Checked;
			m_Area3LabelFormatCombox.Enabled = showDataLabelsCheckBox.Checked;
		}

		void OnArea1LabelFormatComboxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_Area1.DataLabelStyle.Format = GetFormatStringFromIndex(m_Area1LabelFormatCombox.SelectedIndex);
		}

		void OnArea2LabelFormatComboxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_Area2.DataLabelStyle.Format = GetFormatStringFromIndex(m_Area2LabelFormatCombox.SelectedIndex);
		}

		void OnArea3LabelFormatComboxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_Area3.DataLabelStyle.Format = GetFormatStringFromIndex(m_Area3LabelFormatCombox.SelectedIndex);
		}

		#endregion

		#region Implementation

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

		private NComboBox CreateLabelsCombo()
		{
			NComboBox comboBox = new NComboBox();

			comboBox.Items.Add(new NComboBoxItem("Value"));
			comboBox.Items.Add(new NComboBoxItem("Total"));
			comboBox.Items.Add(new NComboBoxItem("Cumulative"));
			comboBox.Items.Add(new NComboBoxItem("Percent"));
			comboBox.SelectedIndex = 0;

			return comboBox;
		}

		private void SetupDataLabels(NAreaSeries area)
		{
			NDataLabelStyle dataLabel = area.DataLabelStyle;

			dataLabel.ArrowLength = 0;
			dataLabel.VertAlign = ENVerticalAlignment.Center;
			dataLabel.TextStyle.Background.Padding = new NMargins(5);
			dataLabel.TextStyle.Font = new NFont("Arial", 8, ENFontStyle.Bold);
		}

		#endregion

		#region Fields

		private NChartView m_ChartView;

		private NAreaSeries m_Area1;
		private NAreaSeries m_Area2;
		private NAreaSeries m_Area3;

		private NComboBox m_Area1LabelFormatCombox;
		private NComboBox m_Area2LabelFormatCombox;
		private NComboBox m_Area3LabelFormatCombox;

		#endregion

		#region Schema

		public static readonly NSchema NStackedAreaExampleSchema;

		#endregion
	}
}