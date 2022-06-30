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
	/// Standard TreeMap Example
	/// </summary>
	public class NStandardTreeMapExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NStandardTreeMapExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NStandardTreeMapExample()
		{
			NStandardTreeMapExampleSchema = NSchema.Create(typeof(NStandardTreeMapExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = CreateTreeMapView();

			// configure title
			chartView.Surface.Titles[0].Text = "Standard TreeMap";

			NTreeMapChart treeMap = (NTreeMapChart)chartView.Surface.Charts[0];

			// Get the country list XML stream
			Stream stream = NResources.Instance.GetResourceStream("RSTR_TreeMapData_xml");

			// Load an xml document from the stream
			NXmlDocument xmlDocument = NXmlDocument.LoadFromStream(stream);

			m_RootTreeMapNode = new NGroupTreeMapNode();

			NThreeColorPalette palette = new NThreeColorPalette();
			palette.OriginColor = NColor.White;
			palette.BeginColor = NColor.Red;
			palette.EndColor = NColor.Green;
			m_RootTreeMapNode.Label = "Tree Map - Industry by Sector";
			m_RootTreeMapNode.Palette = palette;

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

				for (int j = 0; j  < industry.ChildrenCount; j++)
				{
					NXmlElement company = (NXmlElement)industry.GetChildAt(j);

					double value = Double.Parse(company.GetAttributeValue("Size"), CultureInfo.InvariantCulture);
					double change = Double.Parse(company.GetAttributeValue("Change"), CultureInfo.InvariantCulture);
					string label = company.GetAttributeValue("Name");

					NValueTreeMapNode node = new NValueTreeMapNode(value, change, label);
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
			
			NComboBox horizontalFillModeComboBox = new NComboBox();
			horizontalFillModeComboBox.FillFromEnum<ENTreeMapHorizontalFillMode>();
			horizontalFillModeComboBox.SelectedIndexChanged += OnHorizontalFillModeComboBoxSelectedIndexChanged;
			horizontalFillModeComboBox.SelectedIndex = (int)ENTreeMapHorizontalFillMode.LeftToRight;
			stack.Add(NPairBox.Create("Horizontal Fill Mode:", horizontalFillModeComboBox));

			NComboBox verticalFillModeComboBox = new NComboBox();
			verticalFillModeComboBox.FillFromEnum<ENTreeMapVerticalFillMode>();
			verticalFillModeComboBox.SelectedIndexChanged += OnVerticalFillModeComboBoxSelectedIndexChanged;
			verticalFillModeComboBox.SelectedIndex = (int)ENTreeMapVerticalFillMode.TopToBottom;
			stack.Add(NPairBox.Create("Vertical Fill Mode:", verticalFillModeComboBox));

			NComboBox sortOrderComboBox = new NComboBox();
			sortOrderComboBox.FillFromEnum<ENTreeMapNodeSortOrder>();
			sortOrderComboBox.SelectedIndexChanged += OnSortOrderComboBoxSelectedIndexChanged;
			sortOrderComboBox.SelectedIndex = (int)ENTreeMapNodeSortOrder.Ascending;
			stack.Add(NPairBox.Create("Sort Order:", sortOrderComboBox));

			return group;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a standard treemap chart.</p>";
		}

		#endregion

		#region Event Handlers

		void OnHorizontalFillModeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_RootTreeMapNode.HorizontalFillMode = (ENTreeMapHorizontalFillMode)((NComboBox)arg.TargetNode).SelectedIndex;
		}

		void OnVerticalFillModeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_RootTreeMapNode.VerticalFillMode = (ENTreeMapVerticalFillMode)((NComboBox)arg.TargetNode).SelectedIndex;
		}

		void OnSortOrderComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_RootTreeMapNode.SortOrder = (ENTreeMapNodeSortOrder)((NComboBox)arg.TargetNode).SelectedIndex;
		}

		#endregion

		#region Fields

		NGroupTreeMapNode m_RootTreeMapNode;

		#endregion

		#region Schema

		public static readonly NSchema NStandardTreeMapExampleSchema;

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