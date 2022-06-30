using System;
using System.Globalization;
using System.IO;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;
using Nevron.Nov.Xml;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// TreeMap Legend Example
	/// </summary>
	public class NTreeMapLegendExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NTreeMapLegendExample()
		{

		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NTreeMapLegendExample()
		{
			NTreeMapLegendExampleSchema = NSchema.Create(typeof(NTreeMapLegendExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = CreateTreeMapView();

			// configure title
			chartView.Surface.Titles[0].Text = "TreeMap Legend";

			NTreeMapChart treeMap = (NTreeMapChart)chartView.Surface.Charts[0];

			// Get the country list XML stream
			Stream stream = NResources.Instance.GetResourceStream("RSTR_TreeMapDataSmall_xml");

			// Load an xml document from the stream
			NXmlDocument xmlDocument = NXmlDocument.LoadFromStream(stream);

			m_RootTreeMapNode = new NGroupTreeMapNode();

			NThreeColorPalette palette = new NThreeColorPalette();
			palette.OriginColor = NColor.White;
			palette.BeginColor = NColor.Red;
			palette.EndColor = NColor.Green;
			m_RootTreeMapNode.Label = "Tree Map - Industry by Sector";
			m_RootTreeMapNode.Palette = palette;

			m_RootTreeMapNode.LegendView = new NGroupTreeMapNodeLegendView();

			treeMap.RootTreeMapNode = m_RootTreeMapNode;

			NXmlElement rootElement = (NXmlElement)xmlDocument.GetChildAt(0);

			for (int i = 0; i < rootElement.ChildrenCount; i++)
			{
				NXmlElement industry = (NXmlElement)rootElement.GetChildAt(i);
				NGroupTreeMapNode treeMapSeries = new NGroupTreeMapNode();

				treeMapSeries.BorderThickness = new NMargins(4.0);
				treeMapSeries.Border = NBorder.CreateFilledBorder(NColor.Black);
				treeMapSeries.Padding = new NMargins(2.0);

				m_RootTreeMapNode.ChildNodes.Add(treeMapSeries);

				treeMapSeries.Label = industry.GetAttributeValue("Name");
				treeMapSeries.Tooltip = new NTooltip(treeMapSeries.Label);

				for (int j = 0; j < industry.ChildrenCount; j++)
				{
					NXmlElement company = (NXmlElement)industry.GetChildAt(j);

					double value = Double.Parse(company.GetAttributeValue("Size"), CultureInfo.InvariantCulture);
					double change = Double.Parse(company.GetAttributeValue("Change"), CultureInfo.InvariantCulture);
					string label = company.GetAttributeValue("Name");

					NValueTreeMapNode node = new NValueTreeMapNode(value, change, label);
					node.Format = "<label> <change_percent>";
					node.ChangeValueType = ENChangeValueType.Percentage;
					node.Tooltip = new NTooltip(label);

					treeMapSeries.ChildNodes.Add(node);
				}
			}

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);

			NComboBox legendModeComboBox = new NComboBox();
			legendModeComboBox.FillFromEnum<ENTreeMapNodeLegendMode>();
			legendModeComboBox.SelectedIndexChanged += OnLegendModeComboBoxSelectedIndexChanged;
			legendModeComboBox.SelectedIndex = (int)ENTreeMapNodeLegendMode.Group;
			stack.Add(NPairBox.Create("Legend Mode:", legendModeComboBox));

			return group;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to control the treemap legend.</p>";
		}

		#endregion

		#region Event Handlers

		void OnLegendModeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_RootTreeMapNode.LegendView.Mode = (ENTreeMapNodeLegendMode)((NComboBox)arg.TargetNode).SelectedIndex;

			switch (m_RootTreeMapNode.LegendView.Mode)
			{
				case ENTreeMapNodeLegendMode.None:
				case ENTreeMapNodeLegendMode.Group:
				case ENTreeMapNodeLegendMode.ValueNodes:
				case ENTreeMapNodeLegendMode.GroupAndChildNodes:
					m_RootTreeMapNode.LegendView.Format = "<label> <value>";
					break;

				case ENTreeMapNodeLegendMode.Palette:
					m_RootTreeMapNode.LegendView.Format = "<change_begin>";
					break;
			}
		}

		#endregion

		#region Fields

		NGroupTreeMapNode m_RootTreeMapNode;

		#endregion

		#region Schema

		public static readonly NSchema NTreeMapLegendExampleSchema;

		#endregion

		#region Static Methods

		private static NChartView CreateTreeMapView()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.TreeMap);
			return chartView;
		}

		#endregion
	}
}