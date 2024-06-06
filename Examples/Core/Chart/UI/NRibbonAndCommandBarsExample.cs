using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	public class NRibbonAndCommandBarsExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NRibbonAndCommandBarsExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NRibbonAndCommandBarsExample()
		{
			NRibbonAndCommandBarsExampleSchema = NSchema.Create(typeof(NRibbonAndCommandBarsExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create a simple chart
			m_ChartView = new NChartView();

			m_ChartView.Document.HistoryService.Pause();
			try
			{
				InitChart(m_ChartView.Document);
			}
			finally
			{
				m_ChartView.Document.HistoryService.Resume();
			}

			// Create and execute a ribbon UI builder
			m_RibbonBuilder = new NChartRibbonBuilder();
			return m_RibbonBuilder.CreateUI(m_ChartView);
		}
		protected override NWidget CreateExampleControls()
		{
			// Switch UI button
			NButton switchUIButton = new NButton(SwitchToCommandBars);
			switchUIButton.VerticalPlacement = ENVerticalPlacement.Top;
			switchUIButton.Click += OnSwitchUIButtonClick;

			return switchUIButton;
		}
		protected override string GetExampleDescription()
		{
			return "<p>This example demonstrates how to add a commanding interface to the NOV Chart and how to switch between ribbon and command bars.</p>";
		}

		private void InitChart(NChartDocument chartDocument)
		{
			NChartViewSurface chartSurface = chartDocument.Content;
			chartSurface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// Set the title
			chartSurface.Titles[0].Text = "Chart Commanding Interface";

			// Configure the chart
			NCartesianChart chart = (NCartesianChart)chartSurface.Charts[0];
			chart.Enable3D = true;
			chart.Projection.SetPredefinedProjection(ENPredefinedProjection.Perspective);

			// Add an interlace stripe
			NLinearScale linearScale = chart.Axes[ENCartesianAxis.PrimaryY].Scale as NLinearScale;
			NScaleStrip strip = new NScaleStrip(new NColorFill(ENNamedColor.Beige), null, true, 0, 0, 1, 1);
			strip.Interlaced = true;
			linearScale.Strips.Add(strip);

			// Add a bar series
			Random random = new Random();
			for (int i = 0; i < 6; i++)
			{
				NBarSeries bar = new NBarSeries();
				bar.Name = "Bar" + i.ToString();
				bar.MultiBarMode = ENMultiBarMode.Clustered;
				bar.DataLabelStyle = new NDataLabelStyle(false);
				bar.ValueFormatter = new NNumericValueFormatter("0.###");
				chart.Series.Add(bar);

				for (int j = 0; j < 6; j++)
				{
					bar.DataPoints.Add(new NBarDataPoint(random.Next(10, 100)));
				}
			}
		}

		#endregion

		#region Implementation

		private void SetUI(NCommandUIHolder oldUiHolder, NWidget widget)
		{
			if (oldUiHolder.ParentNode is NTabPage)
			{
				((NTabPage)oldUiHolder.ParentNode).Content = widget;
			}
			else if (oldUiHolder.ParentNode is NPairBox)
			{
				((NPairBox)oldUiHolder.ParentNode).Box1 = widget;
			}
		}

		#endregion

		#region Event Handlers

		private void OnSwitchUIButtonClick(NEventArgs arg)
		{
			NButton switchUIButton = (NButton)arg.TargetNode;
			NLabel label = (NLabel)switchUIButton.Content;

			// Remove the chart view from its parent
			NCommandUIHolder uiHolder = m_ChartView.GetFirstAncestor<NCommandUIHolder>();
			m_ChartView.ParentNode.RemoveChild(m_ChartView);

			if (label.Text == SwitchToRibbon)
			{
				// We are in "Command Bars" mode, so switch to "Ribbon"
				label.Text = SwitchToCommandBars;

				// Create the ribbon
				SetUI(uiHolder, m_RibbonBuilder.CreateUI(m_ChartView));
			}
			else
			{
				// We are in "Ribbon" mode, so switch to "Command Bars"
				label.Text = SwitchToRibbon;

				// Create the command bars
				if (m_CommandBarBuilder == null)
				{
					m_CommandBarBuilder = new NChartCommandBarBuilder();
				}

				SetUI(uiHolder, m_CommandBarBuilder.CreateUI(m_ChartView));
			}
		}

		#endregion

		#region Fields

		private NChartView m_ChartView;
		private NChartRibbonBuilder m_RibbonBuilder;
		private NChartCommandBarBuilder m_CommandBarBuilder;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NRibbonAndCommandBarsSwitchingExample.
		/// </summary>
		public static readonly NSchema NRibbonAndCommandBarsExampleSchema;

		#endregion

		#region Constants

		private const string SwitchToCommandBars = "Switch to Command Bars";
		private const string SwitchToRibbon = "Switch to Ribbon";

		#endregion
	}
}