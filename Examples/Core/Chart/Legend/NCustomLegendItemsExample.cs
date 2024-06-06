using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Custom Legend Items Example
	/// </summary>
	public class NCustomLegendItemsExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public NCustomLegendItemsExample()
        {

        }
        /// <summary>
        /// Static constructor
        /// </summary>
        static NCustomLegendItemsExample()
        {
            NCustomLegendItemsExampleSchema = NSchema.Create(typeof(NCustomLegendItemsExample), NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, ENChartPaletteTarget.Series));

            NDockPanel dockPanel = new NDockPanel();

            // set a chart title
            NLabel header = new NLabel("Legend Custom Items");
            NDockLayout.SetDockArea(header, ENDockArea.Top);
            header.Margins = new NMargins(0, 10, 0, 10);
            dockPanel.AddChild(header);

            NDockPanel container = new NDockPanel();
            NDockLayout.SetDockArea(container, ENDockArea.Center);
            container.Margins = new NMargins(10, 10, 10, 10);
            container.BackgroundFill = new NColorFill(NColor.Cyan);
            dockPanel.AddChild(container);

            // configure the legend
            CreateCustomLegend1(container);
            CreateCustomLegend2(container);
            CreateCustomLegend3(container);

            chartView.Surface.Content = dockPanel;

            return chartViewWithCommandBars;
        }
        protected override NWidget CreateExampleControls()
        {
            return null;
        }
        protected override string GetExampleDescription()
        {
            return @"<p>This example demonstrates how to add custom legend items.</p>";
        }

        #endregion

        #region Implementation

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        private void CreateCustomLegend1(NDockPanel container)
        {
            NLegend markShapesLegend = CreateLegend(container, "Mark Shapes");

            Array markShapes = Enum.GetValues(typeof(ENLegendMarkShape));
            NColor[] palette = NChartPalette.GetColors(ENChartPalette.Fresh);

            for (int i = 0; i < markShapes.Length; i++)
            {
                ENLegendMarkShape markShape = (ENLegendMarkShape)markShapes.GetValue(i);

                NFill markFill = new NColorFill(palette[i % palette.Length]);
                NWidget legendItemSymbol = NLegend.CreateLegendSymbol(markShape, new NSize(20, 20), new NMargins(2), markFill, new NStroke(NColor.Black), new NStroke(NColor.Black));;
                NLabel legendItemLabel = new NLabel(markShape.ToString());
                legendItemLabel.Margins = new NMargins(2);

                markShapesLegend.Items.Add(new NPairBox(legendItemSymbol, legendItemLabel));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        private void CreateCustomLegend2(NDockPanel container)
        {
            NLegend markShapesNoStroke = CreateLegend(container, "Mark Shapes (No stroke)");

            Array markShapes = Enum.GetValues(typeof(ENLegendMarkShape));
            NColor[] palette = NChartPalette.GetColors(ENChartPalette.Fresh);

            for (int i = 0; i < markShapes.Length; i++)
            {
                ENLegendMarkShape markShape = (ENLegendMarkShape)markShapes.GetValue(i);

                NFill markFill = new NColorFill(palette[i % palette.Length]);
                NWidget legendItemSymbol = NLegend.CreateLegendSymbol(markShape, new NSize(20, 20), new NMargins(2), markFill, null, null); ;

                NLabel legendItemLabel = new NLabel(markShape.ToString());
                legendItemLabel.Margins = new NMargins(2);

                markShapesNoStroke.Items.Add(new NPairBox(legendItemSymbol, legendItemLabel));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        private void CreateCustomLegend3(NDockPanel container)
        {
            NLegend markShapesBackground = CreateLegend(container, "Mark Shapes (Margins, Background)");

            Array markShapes = Enum.GetValues(typeof(ENLegendMarkShape));
            NColor[] palette = NChartPalette.GetColors(ENChartPalette.Fresh);

            for (int i = 0; i < markShapes.Length; i++)
            {
                ENLegendMarkShape markShape = (ENLegendMarkShape)markShapes.GetValue(i);

                NWidget legendItemSymbol = NLegend.CreateLegendSymbol(markShape, new NSize(20, 20), new NMargins(2), new NColorFill(NColor.White), null, null); ;

                NLabel legendItemLabel = new NLabel(markShape.ToString());
                legendItemLabel.TextFill = new NColorFill(NColor.White);
                legendItemLabel.Font = new NFont("Arial", 10 + i);
                legendItemLabel.Margins = new NMargins(2);

                NPairBox legendItem = new NPairBox(legendItemSymbol, legendItemLabel);
                legendItem.BackgroundFill = new NColorFill(palette[i % palette.Length]);
                legendItem.Margins = new NMargins(4);

                markShapesBackground.Items.Add(legendItem);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        private NLegend CreateLegend(NDockPanel container, string title)
        {
            // configure the legend
            NLegend legend = new NLegend();
            legend.Header = new NLabel(title);
            legend.Mode = ENLegendMode.Custom;
            legend.ExpandMode = ENLegendExpandMode.ColsOnly;
            NDockLayout.SetDockArea(legend, ENDockArea.Top);

            container.AddChild(legend);

            return legend;
        }

        #endregion

        #region Fields


        #endregion

        #region Schema

        public static readonly NSchema NCustomLegendItemsExampleSchema;

        #endregion
    }
}