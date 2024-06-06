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
	public class NSampleReport3Example : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public NSampleReport3Example()
		{
		}
		/// <summary>
		/// 
		/// </summary>
		static NSampleReport3Example()
		{
			NSampleReport3ExampleSchema = NSchema.Create(typeof(NSampleReport3Example), NExampleBaseSchema);
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

			// create the styles used in the report
			CreateHeaderStyles();

			// Populate the rich text
			PopulateRichText();

			return richTextWithRibbon;
		}

		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();


			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>This example demonstrates how to programmatically create reports as well as how to collect text elements of different type.</p>
";
		}

		private void CreateHeaderStyles()
		{
			m_ParStyleHeader1 = new NParagraphStyle("HeaderStyle1");
			m_ParStyleHeader2 = new NParagraphStyle("HeaderStyle2");
			m_ParStyleHeader3 = new NParagraphStyle("HeaderStyle3");

			m_ParStyleHeader1 = new NParagraphStyle("HeaderStyle1");
			m_ParStyleHeader1.InlineRule = new NInlineRule();
			m_ParStyleHeader1.InlineRule.FontSize = 18;
			m_ParStyleHeader1.InlineRule.FontName = "Arial";
			m_ParStyleHeader1.InlineRule.FontStyle = ENFontStyle.Bold;

			m_ParStyleHeader2 = new NParagraphStyle("HeaderStyle2");
			m_ParStyleHeader2.InlineRule = new NInlineRule();
			m_ParStyleHeader2.InlineRule.Fill = new NColorFill(NColor.Blue);
			m_ParStyleHeader2.InlineRule.FontSize = 14;
			m_ParStyleHeader2.InlineRule.FontName = "Arial";
			m_ParStyleHeader2.InlineRule.FontStyle = ENFontStyle.Bold;

			m_ParStyleHeader3 = new NParagraphStyle("HeaderStyle3");
			m_ParStyleHeader3.InlineRule = new NInlineRule();
			m_ParStyleHeader3.InlineRule.Fill = new NColorFill(NColor.White);
			m_ParStyleHeader3.InlineRule.FontSize = 10;
			m_ParStyleHeader3.InlineRule.FontName = "Arial";
			m_ParStyleHeader3.ParagraphRule = new NParagraphRule();
			m_ParStyleHeader3.ParagraphRule.BackgroundFill = new NColorFill(NColor.Blue);
			m_ParStyleHeader3.InlineRule.FontStyle = ENFontStyle.Bold;

			m_ContentStyle = new NParagraphStyle("ContentStyle");
			m_ContentStyle.InlineRule = new NInlineRule();
			m_ContentStyle.InlineRule.FontSize = 8;
			m_ContentStyle.InlineRule.FontName = "Arial";
			m_ContentStyle.InlineRule.FontStyle = ENFontStyle.Regular;
		}

		private void PopulateRichText()
		{
			m_RichText.Content.Sections.Clear();
			m_RichText.Content.Selection.EnableTextIntegrityValidation = false;

			NSection section = new NSection();
			section.PageMargins = new NMargins(10);
			m_RichText.Content.Sections.Add(section);

			// Create the header
			NParagraph titlePar = new NParagraph("SmallCap Growth Fund");
			section.Blocks.Add(titlePar);
			m_ParStyleHeader1.Apply(titlePar);

			{
				NTable headerTable = new NTable(1, 2);
				headerTable.PreferredWidth = new NMultiLength(ENMultiLengthUnit.Percentage, 100);
				section.Blocks.Add(headerTable);

				headerTable.Columns[0].PreferredWidth = new NMultiLength(ENMultiLengthUnit.Percentage, 20);
				headerTable.Columns[1].PreferredWidth = new NMultiLength(ENMultiLengthUnit.Percentage, 80);

				NParagraph title2 = new NParagraph("March 31, 2021");
				headerTable.Rows[0].Cells[0].Blocks.Clear();
				headerTable.Rows[0].Cells[0].Blocks.Add(title2);
				m_ParStyleHeader2.Apply(title2);

				NParagraph title3 = new NParagraph("Institutional Shares (BGRIX)");
				headerTable.Rows[0].Cells[1].Blocks.Clear();
				headerTable.Rows[0].Cells[1].Blocks.Add(title3);
				m_ParStyleHeader2.Apply(title3);

				headerTable.Border = NBorder.CreateFilledBorder(NColor.Gray);
			}

			// Create main table and start adding content to it
			NTable mainTable = new NTable(1, 3);
			section.Blocks.Add(mainTable);
			mainTable.Columns[0].PreferredWidth = new NMultiLength(ENMultiLengthUnit.Percentage, 20);
			mainTable.Columns[1].PreferredWidth = new NMultiLength(ENMultiLengthUnit.Percentage, 50);
			mainTable.Columns[2].PreferredWidth = new NMultiLength(ENMultiLengthUnit.Percentage, 30);

			// Create content for first column
			{
				mainTable.Rows[0].Cells[0].Blocks.Clear();
				CreateHeaderStory(mainTable.Rows[0].Cells[0], false, "Portfolio Managers", new string[] {   "Peter Hughes is SmallCap Capital’s founder, chairman, and CEO. He has 51 years of research and investment experience.",
																											"Adrian Watkins joined SmallCap in 2009 as a research analyst and was named co-portfolio manager of SmallCap Growth Fund in 2018. He has 18 years of research experience.",
																											"Please visit our website for details on their experience and education." });


				CreateHeaderStory(mainTable.Rows[0].Cells[0], true, "Investment Principles", new string[] { "Long-term perspective allows us to think like an owner of a business",
																											"Independent and exhaustive research is essential to understanding the long-term fundamental growth prospects of a business",
																											"We seek open-ended growth opportunities, exceptional leadership, and sustainable competitive advantages.",
																											"Purchase price and risk management are integral to our investment process" });
			}

			// Create content for second column
			{
				NGroupBlock groupBlock = new NGroupBlock();

				groupBlock.Blocks.Add(new NParagraph("The Fund invests in small-sized U.S. companies with significant growth potential. Diversified."));

				NTable categoryTable = new NTable(4, 4);
				categoryTable.AllowSpacingBetweenCells = false;

				// fix the size of the table cells
				for (int i = 0; i < 3; i++)
				{
					//categoryTable.Columns[i].PreferredWidth = new NMultiLength(ENMultiLengthUnit.Dip, 40);
					//categoryTable.Rows[i + 1].PreferredHeight = new NMultiLength(ENMultiLengthUnit.Dip, 40);
				}

				for (int i = 0; i < 4; i++)
				{
					for (int j = 0; j < 4; j++)
					{
						categoryTable.Rows[i].Cells[j].Border = NBorder.CreateFilledBorder(NColor.Black);
						categoryTable.Rows[i].Cells[j].BorderThickness = new NMargins(1);
					}
				}

				categoryTable.Rows[0].Cells[0].Blocks.Clear();
				categoryTable.Rows[0].Cells[0].Blocks.Add(new NParagraph("Value"));
				categoryTable.Rows[0].Cells[1].Blocks.Clear();
				categoryTable.Rows[0].Cells[1].Blocks.Add(new NParagraph("Blend"));
				categoryTable.Rows[0].Cells[2].Blocks.Clear();
				categoryTable.Rows[0].Cells[2].Blocks.Add(new NParagraph("Growth"));

				categoryTable.Rows[1].Cells[3].Blocks.Clear();
				categoryTable.Rows[1].Cells[3].Blocks.Add(new NParagraph("Large"));
				categoryTable.Rows[2].Cells[3].Blocks.Clear();
				categoryTable.Rows[2].Cells[3].Blocks.Add(new NParagraph("Medium"));
				categoryTable.Rows[3].Cells[3].Blocks.Clear();
				categoryTable.Rows[3].Cells[3].Blocks.Add(new NParagraph("Small"));

				categoryTable.Rows[3].Cells[2].BackgroundFill = new NColorFill(NColor.Blue);
				groupBlock.Blocks.Add(categoryTable);

				mainTable.Rows[0].Cells[1].Blocks.Clear();
				CreateHeaderStory(mainTable.Rows[0].Cells[1], "Investment Strategy", new NBlock[] { groupBlock });
			}

			{
				string[] contents = new string[] {
					"Inception Date", "December 31, 1994",
					"Net Assets", "$8.64 billion",
					"# of Equity Securities / % of Net Assets", "53 / 100.1%",
					"Turnover(3 Year Average)", "1.73 %",
					"Active Share", "97.3 %",
					"Median Market Cap²", "$5.96 billion",
					"Weighted Average Market Cap²", "$16.63 billion",
					"As of FYE 9 / 30 / 2020", "Institutional Shares",
					"CUSIP", "068278704",
					"Expense Ratio", "1.04 %" };

				NTable contentTable = new NTable(0, 2);
				contentTable.Columns[0].PreferredWidth = new NMultiLength(ENMultiLengthUnit.Percentage, 70);
				contentTable.Columns[1].PreferredWidth = new NMultiLength(ENMultiLengthUnit.Percentage, 30);

				for (int i = 0; i < contents.Length; i += 2)
				{
					NTableRow tableRow = new NTableRow();

					tableRow.Cells.Add(new NTableCell(contents[i]));
					tableRow.Cells.Add(new NTableCell(contents[i + 1]));

					contentTable.Rows.Add(tableRow);
				}

				CreateHeaderStory(mainTable.Rows[0].Cells[1], "Portfolio Facts and Characteristics", new NBlock[] { contentTable });
			}

			{
				NTable table = new NTable(1, 2);

				string[] contents = new string[] {
					"", "% of Net Assets",
					"MSCI, Inc.", "8.4",
					"Penn National Gaming, Inc.", "7.6",
					"Vail Resorts, Inc.",  "6.7",
					"CoStar Group, Inc.",  "5.7",
					"ANSYS, Inc.",  "5.0",
					"IDEXX Laboratories, Inc.", "4.6",
					"FactSet Research Systems, Inc.",  "4.3",
					"Iridium Communications Inc.", "4.1",
					"Arch Capital Group Ltd.", "4.1",
					"Bio-Techne Corporation", "3.8",
					"Total", "54.3"
				};

				NTable contentTable = new NTable(0, 2);
				contentTable.Columns[0].PreferredWidth = new NMultiLength(ENMultiLengthUnit.Percentage, 70);
				contentTable.Columns[1].PreferredWidth = new NMultiLength(ENMultiLengthUnit.Percentage, 30);

				for (int i = 0; i < contents.Length; i += 2)
				{
					NTableRow tableRow = new NTableRow();

					tableRow.Cells.Add(new NTableCell(contents[i]));
					tableRow.Cells.Add(new NTableCell(contents[i + 1]));

					contentTable.Rows.Add(tableRow);
				}

				CreateHeaderStory(mainTable.Rows[0].Cells[1], "Top 10 Holdings", new NBlock[] { contentTable });
			}

			// Create pie chart
			{
				string[] labels = new string[]
				{
					"Financials",
					"Consumer Discretionary",
					"Information Technology",
					"Health Care",
					"Industrials",
					"Real Estate",
					"Communication Services",
					"Materials",
					"Unclassified"
				};

				double[] values = new double[]
				{
					27.6,
					24.2,
					14.5,
					13.7,
					9.2,
					5.6,
					4.9,
					0.3,
					0.0
				};

				mainTable.Rows[0].Cells[2].Blocks.Clear();
				CreatePieChartStory(mainTable.Rows[0].Cells[2], "GICS Sector Breakdown¹", labels, values);
			}

			// Create bar chart
			{
				string[] labels = new string[]
				{
					"Financial Exchanges & Data",
					"Application Software",
					"Casinos and Gaming",
					"Leisure Facilities",
					"Hotels, Resorts & Cruise Lines",
					"Property & Casual Insurance",
					"Research & Consulting Services",
					"Life Sciences Tools & Services",
					"Health Care Equipment",
					"Alternative Careers"
				};

				double[] values = new double[]
				{
					15.4,
					10.2,
					8.5,
					6.7,
					6.3,
					6.0,
					5.7,
					5.2,
					4.6,
					4.1
				};

				CreateBarChartStory(mainTable.Rows[0].Cells[2], "Top GICS Sub-Industry Breakdown¹", labels, values);
			}

			{
				NParagraph par = new NParagraph();

				NTextInline text = new NTextInline("Risks: ");
				text.FontStyle = ENFontStyle.Bold;
				par.Inlines.Add(text);

				par.Inlines.Add(new NTextInline("Specific risks associated with investing in smaller companies include that the securities may be thinly traded and more difficult to sell during market downturns.Even though the Fund is diversified, it may establish significant positions where the Adviser has the greatest conviction. This could increase volatility of the Fund’s returns. "));

				//mainTable.Rows[0].Cells[2].Blocks.Clear();
				CreateHeaderStory(mainTable.Rows[0].Cells[2], "Portfolio Facts and Characteristics", new NBlock[] { par });
			}
		}

		private void CreateHeaderStory(NGroupBlock groupBlock, string title, NBlock[] content)
		{
			// groupBlock.Blocks.Clear();

			NParagraph header = new NParagraph(title);
			groupBlock.Blocks.Add(header);
			m_ParStyleHeader3.Apply(header);

			for (int i = 0; i < content.Length; i++)
			{
				groupBlock.Blocks.Add(content[i]);

				NList<NNode> paragraphs = content[i].GetDescendants(NParagraph.NParagraphSchema);

				for (int j = 0; j < paragraphs.Count; j++)
				{
					m_ContentStyle.Apply((NParagraph)paragraphs[j]);
				}
			}
		}

		private void CreateHeaderStory(NGroupBlock groupBlock, bool hasBullets, string title, string[] paragraphContent)
		{
			NParagraph header = new NParagraph(title);
			groupBlock.Blocks.Add(header);
			m_ParStyleHeader3.Apply(header);

			NBulletList bulletList = null;
			if (hasBullets)
			{
				bulletList = new NBulletList(ENBulletListTemplateType.Bullet);
				m_RichText.Content.BulletLists.Add(bulletList);
			}

			for (int i = 0; i < paragraphContent.Length; i++)
			{
				NParagraph content = new NParagraph(paragraphContent[i]);
				groupBlock.Blocks.Add(content);

				if (hasBullets)
				{
					content.SetBulletList(bulletList, 0);
				}

				m_ContentStyle.Apply(content);
			}
		}

		private void CreatePieChartStory(NGroupBlock groupBlock, string title, string[] labels, double[] values)
		{
			NParagraph header = new NParagraph(title);
			groupBlock.Blocks.Add(header);
			m_ParStyleHeader3.Apply(header);

			NWidgetInline widgetInline = new NWidgetInline();
			NParagraph dummyPar = new NParagraph();
			groupBlock.Blocks.Add(dummyPar);
			dummyPar.Inlines.Add(widgetInline);

			NChartView chartView = new NChartView();
			chartView.BorderThickness = new NMargins(0);
			chartView.PreferredWidth = 250;
			chartView.PreferredHeight = 300;
			widgetInline.Content = chartView;

			NStackPanel vStack = new NStackPanel();
			vStack.FillMode = Nevron.Nov.Layout.ENStackFillMode.Last;
			vStack.Direction = Nevron.Nov.Layout.ENHVDirection.BottomToTop;
			chartView.Surface.Content = vStack;

			NLegend legend = new NLegend();
			vStack.Add(legend);

			NPieChart pieChart = new NPieChart();
			vStack.Add(pieChart);

			pieChart.Legend = legend;

			NPieSeries pieSeries = new NPieSeries();
			pieSeries.LegendView.Mode = ENSeriesLegendMode.DataPoints;
			pieSeries.LegendView.TextStyle.Font.Size = 8;
			pieChart.Series.Add(pieSeries);

			for (int i = 0; i < labels.Length; i++)
			{
				pieSeries.DataPoints.Add(new NPieDataPoint(values[i], labels[i]));
			}

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, ENChartPaletteTarget.DataPoints));
		}

		private void CreateBarChartStory(NGroupBlock groupBlock, string title, string[] labels, double[] values)
		{
			NParagraph header = new NParagraph(title);
			groupBlock.Blocks.Add(header);
			m_ParStyleHeader3.Apply(header);

			NWidgetInline widgetInline = new NWidgetInline();
			NParagraph dummyPar = new NParagraph();
			groupBlock.Blocks.Add(dummyPar);
			dummyPar.Inlines.Add(widgetInline);

			NChartView chartView = new NChartView();
			chartView.BorderThickness = new NMargins(0);
			chartView.PreferredWidth = 250;
			chartView.PreferredHeight = 300;
			widgetInline.Content = chartView;



			NCartesianChart cartesianChart = new NCartesianChart();
			cartesianChart.LabelLayout.EnableInitialPositioning = false;
			cartesianChart.LabelLayout.EnableLabelAdjustment = false;
			cartesianChart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XOrdinalYLinear);
			cartesianChart.Margins = new NMargins(0);
			cartesianChart.Orientation = ENCartesianChartOrientation.LeftToRight;

			chartView.Surface.Content = cartesianChart;

			NOrdinalScale xScale = (NOrdinalScale)cartesianChart.Axes[ENCartesianAxis.PrimaryX].Scale;
			xScale.Labels.Style.TextStyle.Font = new NFont("Aria", 6);
			xScale.Labels.TextProvider = new NOrdinalScaleLabelTextProvider(labels);

			NBarSeries barSeries = new NBarSeries();
			cartesianChart.Series.Add(barSeries);


			NDataLabelStyle dataLabelStyle = new NDataLabelStyle();
			barSeries.DataLabelStyle = dataLabelStyle;
			barSeries.DataLabelStyle.TextStyle.Background.Visible = false;
			barSeries.DataLabelStyle.TextStyle.Font = new NFont("Arial", 7, ENFontStyle.Regular);
			barSeries.DataLabelStyle.Format = "<value>";

			for (int i = 0; i < labels.Length; i++)
			{
				barSeries.DataPoints.Add(new NBarDataPoint(values[i]));
			}

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, ENChartPaletteTarget.DataPoints));
		}

		#endregion

		#region Fields

		NParagraphStyle m_ParStyleHeader1;
		NParagraphStyle m_ParStyleHeader2;
		NParagraphStyle m_ParStyleHeader3;
		NParagraphStyle m_ContentStyle;

		private NRichTextView m_RichText;

		#endregion

		#region Schema

		public static readonly NSchema NSampleReport3ExampleSchema;

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