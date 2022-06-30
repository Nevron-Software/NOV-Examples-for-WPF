using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Text;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Text
{
	/// <summary>
	/// The example demonstrates how to programmatically create a sample report.
	/// </summary>
	public class NSampleReport1Example : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public NSampleReport1Example()
		{
		}
		/// <summary>
		/// 
		/// </summary>
		static NSampleReport1Example()
		{
			NSampleReport1ExampleSchema = NSchema.Create(typeof(NSampleReport1Example), NExampleBaseSchema);
		}

		#endregion

		#region Protected Overrides

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
			return null;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>The example demonstrates how to programmatically create a sample report.</p>";
		}

		private void PopulateRichText()
		{
			{
				NSection headerSection = new NSection();
				m_RichText.Content.Sections.Add(headerSection);

				headerSection.Blocks.Add(CreateTitleParagraph("Welcome to our annual report. Further information on Sample Group can be found at: www.samplegroup.com"));
				headerSection.Blocks.Add(CreateContentParagraph("Sample Group is a diversified international market infrastructure and capital markets business sitting at the heart of the world’s financial community."));
				headerSection.Blocks.Add(CreateContentParagraph("The Group operates a broad range of international equity, bond and derivatives markets, including Stock Exchange; Europe’s leading fixed income market; and a pan-European equities MTF. Through its platforms, the Group offers international business and investors unrivalled access to Europe’s capital markets."));
				headerSection.Blocks.Add(CreateContentParagraph("Post trade and risk management services are a significant part of the Group’s business operations. In addition to majority ownership of multi-asset global CCP operator, Sunset Group, the Group operates G&B, a clearing house; Monte Span, the European settlement business; and AutoSettle, the Group’s newly established central securities depository based in Luxembourg. The Group is a global leader in indexing and analytic solutions. The Group also provides customers with an extensive range of real time and reference data products. The Group is a leading developer of high performance trading platforms and capital markets software for customers around the world, through MillenniumIT. Since December 2014, the Group has owned Bonita Investments, an investment management business."));
				headerSection.Blocks.Add(CreateContentParagraph("Headquartered in London, with significant operations in North America, China and Russia, the Group employs approximately 6000 people"));
			}

			{
				NSection financialHighlightsSection = new NSection();
				financialHighlightsSection.BreakType = ENSectionBreakType.NextPage;
				m_RichText.Content.Sections.Add(financialHighlightsSection);
				financialHighlightsSection.Blocks.Add(CreateTitleParagraph("Financial highlights"));
				financialHighlightsSection.Blocks.Add(CreateContentParagraph("The following charts provide insight to the group's total income, operating profit, and earnings per share for the years since 2008."));

				NSize chartSize = new NSize(300, 200);
				{
					NTable table = new NTable();
					table.AllowSpacingBetweenCells = false;
					table.Columns.Add(new NTableColumn());
					table.Columns.Add(new NTableColumn());
					financialHighlightsSection.Blocks.Add(table);

					{
						NTableRow tableRow = new NTableRow();
						table.Rows.Add(tableRow);
						{
							NTableCell tableCell = new NTableCell();
							tableCell.Blocks.Add(CreateSampleBarChart(chartSize, "Adjusted total income", new double[] { 674.9, 814.8, 852.9, 1, 213.1, 1, 043.9, 1, 096.4, 1, 381.1 }, new string[] { "2008", "2009", "2010", "2011", "2012", "2013", "2014" }));
							tableRow.Cells.Add(tableCell);
						}

						{
							NTableCell tableCell = new NTableCell();
							tableCell.Blocks.Add(CreateSampleBarChart(chartSize, "Adjusted operating profit", new double[] { 341.1, 441.9, 430.2, 514.7, 417.5, 479.9, 558.0 }, new string[] { "2008", "2009", "2010", "2011", "2012", "2013", "2014" }));
							tableRow.Cells.Add(tableCell);
						}
					}

					{
						NTableRow tableRow = new NTableRow();
						table.Rows.Add(tableRow);
						{
							NTableCell tableCell = new NTableCell();
							tableCell.Blocks.Add(CreateSampleBarChart(chartSize, "Operating profit", new double[] { 283.0, 358.5, 348.4, 353.1, 242.1, 329.4, 346.0 }, new string[] { "2008", "2009", "2010", "2011", "2012", "2013", "2014" }));
							tableRow.Cells.Add(tableCell);
						}

						{
							NTableCell tableCell = new NTableCell();
							tableCell.Blocks.Add(CreateSampleBarChart(chartSize, "Adjusted earnings per share", new double[] { 67.9, 92.6, 97.0, 98.6, 75.6, 96.5, 103.3 }, new string[] { "2008", "2009", "2010", "2011", "2012", "2013", "2014" }));
							tableRow.Cells.Add(tableCell);
						}
					}
				}
			}

			{
				NSection operationalHighlights = new NSection();
				operationalHighlights.ColumnCount = 2;
				operationalHighlights.BreakType = ENSectionBreakType.NextPage;
				operationalHighlights.Blocks.Add(CreateTitleParagraph("Operational highlights"));
				m_RichText.Content.Sections.Add(operationalHighlights);

				operationalHighlights.Blocks.Add(CreateContentParagraph("The Group is delivering on its strategy, leveraging its range of products and services and further diversifying its offering through new product development and strategic investments. A few examples of the progress being made are highlighted below: "));

				operationalHighlights.Blocks.Add(CreateContentParagraph("Capital Markets"));

				{
					NBulletList bulletList = new NBulletList(ENBulletListTemplateType.Bullet);
					m_RichText.Content.BulletLists.Add(bulletList);

					{
						NParagraph par = CreateContentParagraph("Revenues for calendar year 2014 increased by 12 per cent to £333.2 million (2013: £296.8 million). Primary Markets saw a seven year high in new issue activity with 219 new companies admitted, including AA, the largest UK capital raising IPO of the year");
						par.SetBulletList(bulletList, 0);
						operationalHighlights.Blocks.Add(par);
					}

					{
						NParagraph par = CreateContentParagraph("UK cash equity average daily value traded increased 15 per cent and average daily number of trades in Italy increased 16 per cent");
						par.SetBulletList(bulletList, 0);
						operationalHighlights.Blocks.Add(par);
					}

					{
						NParagraph par = CreateContentParagraph("Average daily value traded on Turquoise, our European cash equities MTF, increased 42 per cent to €3.7 billion per day and share of European trading increased to over 9 per cent");
						par.SetBulletList(bulletList, 0);
						operationalHighlights.Blocks.Add(par);
					}

					{
						NParagraph par = CreateContentParagraph("In Fixed Income, MTS cash and BondVision value traded increased by 32 per cent, while MTS Repo value traded increased by 3 per cent");
						par.SetBulletList(bulletList, 0);
						operationalHighlights.Blocks.Add(par);
					}
				}
				operationalHighlights.Blocks.Add(CreateContentParagraph("Post Trade Services"));
				{
					NBulletList bulletList = new NBulletList(ENBulletListTemplateType.Bullet);
					m_RichText.Content.BulletLists.Add(bulletList);

					{
						NParagraph par = CreateContentParagraph("Revenues for calendar year 2014 increased by 3 per cent in constant currency terms. In sterling terms revenues declined by 2 per cent to £96.5 million");
						par.SetBulletList(bulletList, 0);
						operationalHighlights.Blocks.Add(par);
					}

					{
						NParagraph par = CreateContentParagraph("Our Group  cleared 69.7 million equity trades, up 16 per cent and 39.0 million derivative contracts up 20 per cent");
						par.SetBulletList(bulletList, 0);
						operationalHighlights.Blocks.Add(par);
					}

					{
						NParagraph par = CreateContentParagraph("Our Group is the largest CSD entering the first wave of TARGET2-Securities from June 2015. Successful testing with the European Central Bank finished in December 2014. In addition, Our Group moved settlement of contracts executed on the Italian market from T+3 to T+2 in October 2014");
						par.SetBulletList(bulletList, 0);
						operationalHighlights.Blocks.Add(par);
					}
				}

				operationalHighlights.Blocks.Add(CreateContentParagraph("Post Trade Services 2"));

				{
					NBulletList bulletList = new NBulletList(ENBulletListTemplateType.Bullet);
					m_RichText.Content.BulletLists.Add(bulletList);

					{
						NParagraph par = CreateContentParagraph("Adjusted income for the calendar year 2014 was £389.4 million, up 24 per cent on a pro forma constant currency basis. LCH.Clearnet received EMIR reauthorisation for the UK and France businesses — SwapClear, the world’s leading interest rate swap clearing service, cleared $642 trillion notional, up 26 per cent");
						par.SetBulletList(bulletList, 0);
						operationalHighlights.Blocks.Add(par);
					}

					{
						NParagraph par = CreateContentParagraph("Compression services at SwapClear reduced level of notional outstanding, from $426 trillion to $362 trillion");
						par.SetBulletList(bulletList, 0);
						operationalHighlights.Blocks.Add(par);
					}

					{
						NParagraph par = CreateContentParagraph("Our Group was granted clearing house recognition in Canada and Australia");
						par.SetBulletList(bulletList, 0);
						operationalHighlights.Blocks.Add(par);
					}
					{
						NParagraph par = CreateContentParagraph("Clearing of commodities for the London Metal Exchange ceased in September 2014 as expected");
						par.SetBulletList(bulletList, 0);
						operationalHighlights.Blocks.Add(par);
					}
					{
						NParagraph par = CreateContentParagraph("RepoClear, one of Europe’s largest fixed income clearers, cleared €73.4 trillion in nominal value, up 1 per cent");
						par.SetBulletList(bulletList, 0);
						operationalHighlights.Blocks.Add(par);
					}

					operationalHighlights.Blocks.Add(CreateContentParagraph("Group Adjusted Total Income by segment"));

					NTable table = new NTable();
					table.Margins = new NMargins(10);
					table.AllowSpacingBetweenCells = false;
					operationalHighlights.Blocks.Add(table);

					table.Columns.Add(new NTableColumn());
					table.Columns.Add(new NTableColumn());
					table.Columns.Add(new NTableColumn());

					{
						NTableRow tableRow = new NTableRow();
						table.Rows.Add(tableRow);

						NTableCell tc1 = CreateTableCellWithBorder();
						tableRow.Cells.Add(tc1);

						NTableCell tc2 = CreateTableCellWithBorder();
						tc2.Blocks.Add(CreateContentParagraph("2013"));
						tableRow.Cells.Add(tc2);

						NTableCell tc3 = CreateTableCellWithBorder();
						tc3.Blocks.Add(CreateContentParagraph("2014"));
						tableRow.Cells.Add(tc3);
					}

					{
						NTableRow tableRow = new NTableRow();
						table.Rows.Add(tableRow);

						NTableCell tc1 = CreateTableCellWithBorder();
						tc1.Blocks.Add(CreateContentParagraph("Capital Markets"));
						tableRow.Cells.Add(tc1);

						NTableCell tc2 = CreateTableCellWithBorder();
						tc2.Blocks.Add(CreateContentParagraph("249.1"));
						tableRow.Cells.Add(tc2);

						NTableCell tc3 = CreateTableCellWithBorder();
						tc3.Blocks.Add(CreateContentParagraph("333.2"));
						tableRow.Cells.Add(tc3);
					}

					{
						NTableRow tableRow = new NTableRow();
						table.Rows.Add(tableRow);

						NTableCell tc1 = CreateTableCellWithBorder();
						tc1.Blocks.Add(CreateContentParagraph("Post Trade Service"));
						tableRow.Cells.Add(tc1);

						NTableCell tc2 = CreateTableCellWithBorder();
						tc2.Blocks.Add(CreateContentParagraph("94.7"));
						tableRow.Cells.Add(tc2);

						NTableCell tc3 = CreateTableCellWithBorder();
						tc3.Blocks.Add(CreateContentParagraph("129.1"));
						tableRow.Cells.Add(tc3);
					}

					{
						NTableRow tableRow = new NTableRow();
						table.Rows.Add(tableRow);

						NTableCell tc1 = CreateTableCellWithBorder();
						tc1.Blocks.Add(CreateContentParagraph("Information Services "));
						tableRow.Cells.Add(tc1);

						NTableCell tc2 = CreateTableCellWithBorder();
						tc2.Blocks.Add(CreateContentParagraph("281.0"));
						tableRow.Cells.Add(tc2);

						NTableCell tc3 = CreateTableCellWithBorder();
						tc3.Blocks.Add(CreateContentParagraph("373.0"));
						tableRow.Cells.Add(tc3);
					}

					{
						NTableRow tableRow = new NTableRow();
						table.Rows.Add(tableRow);

						NTableCell tc1 = CreateTableCellWithBorder();
						tc1.Blocks.Add(CreateContentParagraph("Technology Services"));
						tableRow.Cells.Add(tc1);

						NTableCell tc2 = CreateTableCellWithBorder();
						tc2.Blocks.Add(CreateContentParagraph("47.3"));
						tableRow.Cells.Add(tc2);

						NTableCell tc3 = CreateTableCellWithBorder();
						tc3.Blocks.Add(CreateContentParagraph("66.0"));
						tableRow.Cells.Add(tc3);
					}

					{
						NTableRow tableRow = new NTableRow();
						table.Rows.Add(tableRow);

						NTableCell tc1 = CreateTableCellWithBorder();
						tc1.Blocks.Add(CreateContentParagraph("Other"));
						tableRow.Cells.Add(tc1);

						NTableCell tc2 = CreateTableCellWithBorder();
						tc2.Blocks.Add(CreateContentParagraph("87.2"));
						tableRow.Cells.Add(tc2);

						NTableCell tc3 = CreateTableCellWithBorder();
						tc3.Blocks.Add(CreateContentParagraph("90.4"));
						tableRow.Cells.Add(tc3);
					}
				}
			}
		}

		#endregion

		#region Event Handlers


		#endregion

		#region Implementation


		/// <summary>
		/// Creates a table cell with border
		/// </summary>
		/// <returns></returns>
		private NTableCell CreateTableCellWithBorder()
		{
			NTableCell tableCell = new NTableCell();

			tableCell.Border = NBorder.CreateFilledBorder(NColor.Black);
			tableCell.BorderThickness = new NMargins(1);

			return tableCell;
		}
		/// <summary>
		/// Creates a section title paragraph
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		private NParagraph CreateTitleParagraph(string text)
		{
			NParagraph paragraph = new NParagraph();

			NTextInline textInline = new NTextInline();
			textInline.Text = text;
			textInline.FontSize = 14;
			textInline.Fill = new NColorFill(NColor.FromRGB(89, 76, 46));
			paragraph.Inlines.Add(textInline);

			return paragraph;
		}
		/// <summary>
		/// Creates a content paragraph
		/// </summary>
		/// <returns></returns>
		private NParagraph CreateContentParagraph(string text)
		{
			NParagraph paragraph = new NParagraph(text);

			paragraph.HorizontalAlignment = ENAlign.Justify;

			return paragraph;
		}
		/// <summary>
		/// Creates a sample bar chart given title, values and labels
		/// </summary>
		/// <param name="size"></param>
		/// <param name="title"></param>
		/// <param name="values"></param>
		/// <param name="labels"></param>
		/// <returns></returns>
		private NParagraph CreateSampleBarChart(NSize size, string title, double[] values, string[] labels)
		{
			NChartView chartView = CreateCartesianChartView();

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

			NBarSeries bar = new NBarSeries();
			bar.LegendView.Mode = ENSeriesLegendMode.None;
			bar.DataLabelStyle = new NDataLabelStyle(false);

			chart.Series.Add(bar);

			for (int i = 0; i < values.Length; i++)
			{
				bar.DataPoints.Add(new NBarDataPoint(values[i]));
			}

			NOrdinalScale scaleX = (NOrdinalScale)chart.Axes[ENCartesianAxis.PrimaryX].Scale;
			scaleX.Labels.TextProvider = new NOrdinalScaleLabelTextProvider(labels);

			NParagraph paragraph = new NParagraph();

			NWidgetInline chartInline = new NWidgetInline();
			chartInline.Content = chartView;
			paragraph.Inlines.Add(chartInline);

			return paragraph;
		}

		protected NChartView CreateCartesianChartView()
		{
			NChartView chartView = new NChartView();

			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			return chartView;
		}

		#endregion

		#region Fields

		private NRichTextView m_RichText;

		#endregion

		#region Schema

		public static readonly NSchema NSampleReport1ExampleSchema;

		#endregion
	}
}