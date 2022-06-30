using System;

using Nevron.Nov.Chart;
using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Text;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Text
{
	/// <summary>
	/// The example demonstrates how to programmatically manipulate text documents, find content etc.
	/// </summary>
	public class NSampleReport2Example : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public NSampleReport2Example()
		{
		}
		/// <summary>
		/// 
		/// </summary>
		static NSampleReport2Example()
		{
			NSampleReport2ExampleSchema = NSchema.Create(typeof(NSampleReport2Example), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create the rich text
			NRichTextViewWithRibbon richTextWithRibbon = new NRichTextViewWithRibbon();
			m_RichText = richTextWithRibbon.View;
			m_RichText.AcceptsTab = true;
			m_RichText.Content.Sections.Clear();

			// Populate the rich text
			PopulateRichText();

			return richTextWithRibbon;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			
			NButton highlightAllChartsButton = new NButton("Highlight All Charts");
			highlightAllChartsButton.Click += OnHighlightAllChartsButtonClick;
			stack.Add(highlightAllChartsButton);

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>This example demonstrates how to programmatically create reports as well as how to collect text elements of different type.</p>
";
		}

		private void PopulateRichText()
		{
			NSection section = new NSection();
			m_RichText.Content.Sections.Add(section);

			NParagraph paragraph = new NParagraph();

			paragraph.HorizontalAlignment = ENAlign.Center;
			paragraph.Inlines.Add(CreateHeaderText("ACME Corporation"));
			paragraph.Inlines.Add(new NLineBreakInline());
			paragraph.Inlines.Add(CreateNormalText("Monthly Health Report"));

			section.Blocks.Add(paragraph);

			// generate sample data
			double[] sales = new double[12];
			double[] hours = new double[12];
			double[] profitloss = new double[12];
			string[] months = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
			Random random = new Random();

			for (int i = 0; i < 12; i++)
			{
				sales[i] = 50 + random.Next(50);
				hours[i] = 50 + random.Next(50);
				profitloss[i] = 25 - random.Next(50);
			}

			NTable table = new NTable();
			section.Blocks.Add(table);

			table.Columns.Add(new NTableColumn());
			table.Columns.Add(new NTableColumn());

			{
				NTableRow tableRow = new NTableRow();

				NTableCell tableCell1 = new NTableCell();

				NParagraph par1 = new NParagraph();
				par1.Inlines.Add(CreateHeaderText("Sales New Projects"));
				tableCell1.Blocks.Add(par1);
				tableRow.Cells.Add(tableCell1);

				NTableCell tableCell2 = new NTableCell();
				tableCell2.Blocks.Add(new NParagraph());
				tableRow.Cells.Add(tableCell2);

				table.Rows.Add(tableRow);
			}

			{
				NTableRow tableRow = new NTableRow();

				NTableCell tableCell1 = new NTableCell();

				NParagraph par1 = new NParagraph();
				par1.Inlines.Add(CreateHeaderText("Total Sales: " + GetTotal(sales).ToString()));
				par1.Inlines.Add(new NLineBreakInline());
				par1.Inlines.Add(CreateNormalText("Last Month: " + sales[11].ToString()));
				tableCell1.Blocks.Add(par1);
				tableRow.Cells.Add(tableCell1);

				NTableCell tableCell2 = new NTableCell();
				tableCell2.Blocks.Add(CreateBarChart(true, new NSize(400, 200), "Sales", sales, months));
				tableRow.Cells.Add(tableCell2);

				table.Rows.Add(tableRow);
			}

			{
				NTableRow tableRow = new NTableRow();

				NTableCell tableCell1 = new NTableCell();

				NParagraph par1 = new NParagraph();
				par1.Inlines.Add(CreateHeaderText("Billable Hours: " + GetTotal(hours).ToString()));
				par1.Inlines.Add(new NLineBreakInline());
				par1.Inlines.Add(CreateNormalText("Last Month: " + hours[11].ToString()));
				tableCell1.Blocks.Add(par1);
				tableRow.Cells.Add(tableCell1);

				NTableCell tableCell2 = new NTableCell();
				tableCell2.Blocks.Add(CreateBarChart(false, new NSize(400, 200), "Hours", hours, months));
				tableRow.Cells.Add(tableCell2);

				table.Rows.Add(tableRow);
			}

			{
				NTableRow tableRow = new NTableRow();

				NTableCell tableCell1 = new NTableCell();

				NParagraph par1 = new NParagraph();
				par1.Inlines.Add(CreateHeaderText("Profit / Loss: " + GetTotal(profitloss).ToString()));
				par1.Inlines.Add(new NLineBreakInline());
				par1.Inlines.Add(CreateNormalText("Last Month: " + profitloss[11].ToString()));
				tableCell1.Blocks.Add(par1);
				tableRow.Cells.Add(tableCell1);

				NTableCell tableCell2 = new NTableCell();
				tableCell2.Blocks.Add(CreateBarChart(false, new NSize(400, 200), "Profit / Loss", hours, months));
				tableRow.Cells.Add(tableCell2);

				table.Rows.Add(tableRow);
			}
		}

		#endregion

		#region Event Handlers

		void OnHighlightAllChartsButtonClick(NEventArgs arg)
		{

			
			NList<NNode> charts = m_RichText.Content.GetDescendants(NChartView.NChartViewSchema);

			for (int i = 0; i < charts.Count; i++)
			{
				NChartView chartView = (NChartView)charts[i];

				((NCartesianChart)chartView.Surface.Charts[0]).PlotFill = new NColorFill(NColor.LightBlue);
			}
		}


		#endregion

		#region Fields

		private NRichTextView m_RichText;

		#endregion

		#region Schema

		public static readonly NSchema NSampleReport2ExampleSchema;

		#endregion

		#region Static Methods

		private static double GetTotal(double[] values)
		{
			double total = 0.0;

			for (int i = 0; i < values.Length; i++)
			{
				total += values[i];
			}

			return total;
		}
		private static NTextInline CreateHeaderText(string text)
		{
			NTextInline inline = new NTextInline(text);

			inline.FontSize = 14;
			inline.FontStyle = ENFontStyle.Bold;

			return inline;
		}
		private static NTextInline CreateNormalText(string text)
		{
			NTextInline inline = new NTextInline(text);

			inline.FontSize = 9;

			return inline;
		}

		/// <summary>
		/// Creates a sample bar chart given title, values and labels
		/// </summary>
		/// <param name="area"></param>
		/// <param name="size"></param>
		/// <param name="title"></param>
		/// <param name="values"></param>
		/// <param name="labels"></param>
		/// <returns></returns>
		private static NParagraph CreateBarChart(bool area, NSize size, string title, double[] values, string[] labels)
		{
			NChartView chartView = new NChartView();

			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);
			chartView.PreferredSize = size;

			// configure title
			chartView.Surface.Titles[0].Text = title;
			chartView.Surface.Titles[0].Margins = NMargins.Zero;
			chartView.Surface.Legends[0].Visibility = ENVisibility.Hidden;
			chartView.BorderThickness = NMargins.Zero;

			// configure chart
			NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];
			chart.Padding = new NMargins(20);

			// configure axes
			chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XOrdinalYLinear);
			chart.Margins = NMargins.Zero;

			if (area)
			{
				NAreaSeries areaSeries = new NAreaSeries();
				areaSeries.LegendView.Mode = ENSeriesLegendMode.None;
				areaSeries.DataLabelStyle = new NDataLabelStyle(false);

				chart.Series.Add(areaSeries);

				for (int i = 0; i < values.Length; i++)
				{
					areaSeries.DataPoints.Add(new NAreaDataPoint(values[i]));
				}
			}
			else
			{
				NBarSeries barSeries = new NBarSeries();
				barSeries.LegendView.Mode = ENSeriesLegendMode.None;
				barSeries.DataLabelStyle = new NDataLabelStyle(false);

				chart.Series.Add(barSeries);

				for (int i = 0; i < values.Length; i++)
				{
					barSeries.DataPoints.Add(new NBarDataPoint(values[i]));
				}
			}

			NOrdinalScale scaleX = (NOrdinalScale)chart.Axes[ENCartesianAxis.PrimaryX].Scale;
			scaleX.Labels.TextProvider = new NOrdinalScaleLabelTextProvider(labels);
			scaleX.MajorTickMode = ENMajorTickMode.CustomStep;
			scaleX.CustomStep = 1;

			NParagraph paragraph = new NParagraph();

			NWidgetInline chartInline = new NWidgetInline();
			chartInline.Content = chartView;
			paragraph.Inlines.Add(chartInline);

			return paragraph;
		}

		#endregion
	}
}