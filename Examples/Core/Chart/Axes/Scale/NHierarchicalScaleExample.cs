using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Hierarchical Scale Example
	/// </summary>
	public class NHierarchicalScaleExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NHierarchicalScaleExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NHierarchicalScaleExample()
		{
			NHierarchicalScaleExampleSchema = NSchema.Create(typeof(NHierarchicalScaleExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Hierarchical Scale";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

			// configure axes
			m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XOrdinalYLinear);

			// add the first bar
			m_Bar1 = new NBarSeries();
			m_Chart.Series.Add(m_Bar1);
			m_Bar1.Name = "Coca Cola";
			m_Bar1.MultiBarMode = ENMultiBarMode.Series;
			m_Bar1.DataLabelStyle = new NDataLabelStyle(false);

			// add the second bar
			m_Bar2 = new NBarSeries();
			m_Chart.Series.Add(m_Bar2);
			m_Bar2.Name = "Pepsi";
			m_Bar2.MultiBarMode = ENMultiBarMode.Clustered;
			m_Bar2.DataLabelStyle = new NDataLabelStyle(false);

			// add custom labels to the Y axis
			NLinearScale linearScale = (NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale;

			// add interlace stripe
			NScaleStrip stripStyle = new NScaleStrip(new NColorFill(NColor.Beige), null, true, 0, 0, 1, 1);
			stripStyle.Interlaced = true;
			linearScale.Strips.Add(stripStyle);

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

			m_FirstRowGridStyleComboBox = new NComboBox();
			m_FirstRowGridStyleComboBox.FillFromEnum<ENFirstRowGridStyle>();
			m_FirstRowGridStyleComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnUpdateScale);
			stack.Add(NPairBox.Create("Grid Style:", m_FirstRowGridStyleComboBox));
			
			m_FirstRowTickModeComboBox = new NComboBox();
			m_FirstRowTickModeComboBox.FillFromEnum<ENRangeLabelTickMode>();
			m_FirstRowTickModeComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnUpdateScale);
			stack.Add(NPairBox.Create("Tick Mode:", m_FirstRowTickModeComboBox));

			m_GroupRowGridStyleComboBox = new NComboBox();
			m_GroupRowGridStyleComboBox.FillFromEnum<ENGroupRowGridStyle>();
			m_GroupRowGridStyleComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnUpdateScale);
			stack.Add(NPairBox.Create("Grid Style:", m_GroupRowGridStyleComboBox));

			m_GroupRowTickModeComboBox = new NComboBox();
			m_GroupRowTickModeComboBox.FillFromEnum<ENRangeLabelTickMode>();
			m_GroupRowTickModeComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnUpdateScale);
			stack.Add(NPairBox.Create("Tick Mode:", m_GroupRowTickModeComboBox));
																 
			m_CreateSeparatorForEachLevelCheckBox = new NCheckBox("Create Separator For Each Level");
			m_CreateSeparatorForEachLevelCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnUpdateScale);
			stack.Add(m_CreateSeparatorForEachLevelCheckBox);

			NButton changeDataButton = new NButton("Change Data");
			changeDataButton.Click += new Function<NEventArgs>(OnChangeDataButtonClick);
			stack.Add(changeDataButton);

			m_FirstRowGridStyleComboBox.SelectedIndex = (int)ENFirstRowGridStyle.Individual;
			m_FirstRowTickModeComboBox.SelectedIndex = (int)ENRangeLabelTickMode.Separators;
			m_GroupRowGridStyleComboBox.SelectedIndex = (int)ENGroupRowGridStyle.Individual;
			m_GroupRowTickModeComboBox.SelectedIndex = (int)ENRangeLabelTickMode.Separators;
			m_CreateSeparatorForEachLevelCheckBox.Checked = true;

			OnChangeDataButtonClick(null);
			OnUpdateScale(null);

			return boxGroup;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a hierarchical scale.</p>";
		}

		#endregion

		#region Event Handlers

		void OnChangeDataButtonClick(NEventArgs arg)
		{
			// fill with random data
			m_Bar1.DataPoints.Clear();
			m_Bar2.DataPoints.Clear();

			Random random = new Random();
			for (int i = 0; i < 24; i++)
			{
				m_Bar1.DataPoints.Add(new NBarDataPoint(random.Next(10, 200)));
				m_Bar2.DataPoints.Add(new NBarDataPoint(random.Next(10, 300)));
			}
		}

		void OnUpdateScale(NValueChangeEventArgs arg)
		{
			// add custom labels to the X axis
			NHierarchicalScale scale = new NHierarchicalScale();
			NHierarchicalScaleNodeCollection nodes = scale.ChildNodes;

			scale.FirstRowGridStyle = (ENFirstRowGridStyle)m_FirstRowGridStyleComboBox.SelectedIndex;
			scale.GroupRowGridStyle = (ENGroupRowGridStyle)m_GroupRowGridStyleComboBox.SelectedIndex;
			scale.InnerMajorTicks.Visible = false;
			scale.OuterMajorTicks.Visible = false;

			string[] months = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

			for (int i = 0; i < 2; i++)
			{
				NHierarchicalScaleNode yearNode = new NHierarchicalScaleNode(0, (i + 2007).ToString());
				yearNode.LabelStyle.TickMode = (ENRangeLabelTickMode)m_GroupRowTickModeComboBox.SelectedIndex;
				nodes.AddChild(yearNode);

				for (int j = 0; j < 4; j++)
				{
					NHierarchicalScaleNode quarterNode = new NHierarchicalScaleNode(3, "Q" + (j + 1).ToString());
					quarterNode.LabelStyle.TickMode = (ENRangeLabelTickMode)m_GroupRowTickModeComboBox.SelectedIndex;
					yearNode.Nodes.Add(quarterNode);

					for (int k = 0; k < 3; k++)
					{
						NHierarchicalScaleNode monthNode = new NHierarchicalScaleNode(1, months[j * 3 + k]);
						monthNode.LabelStyle.Angle = new NScaleLabelAngle(90);
						monthNode.LabelStyle.TickMode = (ENRangeLabelTickMode)m_FirstRowTickModeComboBox.SelectedIndex;
						quarterNode.Nodes.Add(monthNode);
					}
				}
			}

			// update control state
			m_FirstRowTickModeComboBox.Enabled = ((ENFirstRowGridStyle)m_FirstRowGridStyleComboBox.SelectedIndex) == ENFirstRowGridStyle.Individual;
			m_GroupRowTickModeComboBox.Enabled = ((ENGroupRowGridStyle)m_GroupRowGridStyleComboBox.SelectedIndex) == ENGroupRowGridStyle.Individual;

			NScaleStrip stripStyle = new NScaleStrip();
			stripStyle.Length = 3;
			stripStyle.Interval = 3;
			stripStyle.Fill = new NColorFill(NColor.FromColor(NColor.LightGray, 0.5f));
			scale.Strips.Add(stripStyle);

			scale.CreateSeparatorForEachLevel = m_CreateSeparatorForEachLevelCheckBox.Checked;

			m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale = scale;
		}

		#endregion

		#region Fields

		NCartesianChart m_Chart;

		NBarSeries m_Bar1;
		NBarSeries m_Bar2;   		

		NComboBox m_FirstRowGridStyleComboBox;
		NComboBox m_FirstRowTickModeComboBox;

		NComboBox m_GroupRowGridStyleComboBox;
		NComboBox m_GroupRowTickModeComboBox;

		NCheckBox m_CreateSeparatorForEachLevelCheckBox;

		#endregion

		#region Schema

		public static readonly NSchema NHierarchicalScaleExampleSchema;

		#endregion
	}
}
