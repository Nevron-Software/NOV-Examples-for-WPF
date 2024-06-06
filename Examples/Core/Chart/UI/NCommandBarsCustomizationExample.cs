using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Commands;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	public class NCommandBarsCustomizationExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NCommandBarsCustomizationExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NCommandBarsCustomizationExample()
		{
			NCommandBarsCustomizationExampleSchema = NSchema.Create(typeof(NCommandBarsCustomizationExample), NExampleBaseSchema);
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

			// Create and customize a chart command bar UI builder
			NChartCommandBarBuilder commandBarBuilder = new NChartCommandBarBuilder();

			// Add the custom command action to the chart view's commander
			m_ChartView.Commander.Add(new CustomCommandAction());

			// Remove the "Standard" toolbar and insert a custom one
			commandBarBuilder.ToolBarBuilders.Remove(NChartCommandBarBuilder.ToolBarStandardName);
			commandBarBuilder.ToolBarBuilders.Insert(0, new CustomToolBarBuilder());

			// Create the commanding UI
			NCommandUIHolder chartViewWithRibbon = commandBarBuilder.CreateUI(m_ChartView);

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
			return "<p>This example demonstrates how to customize the NOV Chart command bars.</p>";
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
		/// Schema associated with NCommandBarsCustomizationExample.
		/// </summary>
		public static readonly NSchema NCommandBarsCustomizationExampleSchema;

		#endregion

		#region Constants

		public static readonly NCommand CustomCommand = NCommand.Create(typeof(NCommandBarsCustomizationExample),
			"CustomCommand", "Custom Command");

		#endregion

		#region Nested Types

		public class CustomToolBarBuilder : NToolBarBuilder
		{
			public CustomToolBarBuilder()
				: base("Custom Toolbar")
			{
			}

			protected override void AddItems(NCommandBarItemCollection items)
			{
				// Add the "Open" button
				items.Add(CreateButton(Presentation.NResources.Image_File_Open_png, NChartView.OpenCommand));

				// Add the custom command button
				items.Add(CreateButton(NResources.Image_Ribbon_16x16_smiley_png, CustomCommand));
			}
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