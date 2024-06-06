using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Commands;
using Nevron.Nov.Chart.UI;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	public class NRibbonCustomizationExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NRibbonCustomizationExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NRibbonCustomizationExample()
		{
			NRibbonCustomizationExampleSchema = NSchema.Create(typeof(NRibbonCustomizationExample), NExampleBaseSchema);
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

			// Create and customize a ribbon UI builder
			NChartRibbonBuilder ribbonBuilder = new NChartRibbonBuilder();

			// Add the custom command action to the chart view's commander
			m_ChartView.Commander.Add(new CustomCommandAction());

			// Rename the "Home" ribbon tab page
			NRibbonTabPageBuilder homeTabBuilder = ribbonBuilder.TabPageBuilders[NChartRibbonBuilder.TabPageHomeName];
			homeTabBuilder.Name = "Start";

			// Rename the "Export" ribbon group of the "Home" tab page
			NRibbonGroupBuilder exportGroupBuilder = homeTabBuilder.RibbonGroupBuilders[NHomeTabPageBuilder.GroupExportName];
			exportGroupBuilder.Name = "Custom Name";

			// Remove the "Design" ribbon group from the "Home" tab page
			homeTabBuilder.RibbonGroupBuilders.Remove(NHomeTabPageBuilder.GroupDesignName);

			// Insert the custom ribbon group at the beginning of the home tab page
			homeTabBuilder.RibbonGroupBuilders.Insert(0, new CustomRibbonGroupBuilder());

			// Create the commanding UI
			NCommandUIHolder chartViewWithRibbon = ribbonBuilder.CreateUI(m_ChartView);

			// Remove the Open, Save and SaveAs commands
			chartViewWithRibbon.RemoveCommands(NChartView.OpenCommand, NChartView.SaveCommand, NChartView.SaveAsCommand);

			return chartViewWithRibbon;
		}
		protected override NWidget CreateExampleControls()
		{
			return null;
		}
		protected override string GetExampleDescription()
		{
			return "<p>This example demonstrates how to customize the NOV Chart ribbon.</p>";
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

		#region Fields

		private NChartView m_ChartView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NRibbonCustomizationExample.
		/// </summary>
		public static readonly NSchema NRibbonCustomizationExampleSchema;

		#endregion

		#region Constants

		public static readonly NCommand CustomCommand = NCommand.Create(typeof(NRibbonCustomizationExample),
			"CustomCommand", "Custom Command");

		#endregion

		#region Nested Types

		public class CustomRibbonGroupBuilder : NRibbonGroupBuilder
		{
			#region Constructors

			public CustomRibbonGroupBuilder()
				: base("Custom Group", NResources.Image_Ribbon_16x16_smiley_png)
			{
			}

			#endregion

			#region Protected Overrides

			protected override void AddRibbonGroupItems(NRibbonGroupItemCollection items)
			{
				// Add the "Copy" command
				items.Add(CreateRibbonButton(Presentation.NResources.Image_Ribbon_AppMenu_Open_png,
					Presentation.NResources.Image_File_Open_png, NChartView.OpenCommand));

				// Add the custom command
				items.Add(CreateRibbonButton(NResources.Image_Ribbon_32x32_smiley_png,
					NResources.Image_Ribbon_16x16_smiley_png, CustomCommand));
			}

			#endregion
		}

		public class CustomCommandAction : NChartCommandAction
		{
			#region Constructors

			/// <summary>
			/// Default constructor.
			/// </summary>
			public CustomCommandAction()
			{
			}

			/// <summary>
			/// Static constructor.
			/// </summary>
			static CustomCommandAction()
			{
				CustomCommandActionSchema = NSchema.Create(typeof(CustomCommandAction), NChartCommandActionSchema);
			}

			#endregion

			#region Public Overrides

			/// <summary>
			/// Gets the command associated with this command action.
			/// </summary>
			/// <returns></returns>
			public override NCommand GetCommand()
			{
				return CustomCommand;
			}
			/// <summary>
			/// Executes the command action.
			/// </summary>
			/// <param name="target"></param>
			/// <param name="parameter"></param>
			public override void Execute(NNode target, object parameter)
			{
				NChartView chartView = GetChartView(target);

				NMessageBox.Show("Chart Custom Command executed!", "Custom Command", ENMessageBoxButtons.OK,
					ENMessageBoxIcon.Information);
			}

			#endregion

			#region Schema

			/// <summary>
			/// Schema associated with CustomCommandAction.
			/// </summary>
			public static readonly NSchema CustomCommandActionSchema;

			#endregion
		}

		#endregion
	}
}