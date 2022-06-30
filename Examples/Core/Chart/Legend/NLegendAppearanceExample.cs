using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Legend Appearance Example
	/// </summary>
	public class NLegendAppearanceExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NLegendAppearanceExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NLegendAppearanceExample()
		{
			NLegendAppearanceExampleSchema = NSchema.Create(typeof(NLegendAppearanceExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
            NChartView chartView = new NChartView();
            chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

            // configure title
            chartView.Surface.Titles[0].Text = "Legend Appearance";

            m_Legend = chartView.Surface.Legends[0];
            m_Legend.ExpandMode = ENLegendExpandMode.ColsFixed;
            m_Legend.ColCount = 3;

            m_Legend.Border = NBorder.CreateFilledBorder(NColor.Black);
            m_Legend.BorderThickness = new NMargins(2);
            m_Legend.BackgroundFill = new NStockGradientFill(NColor.White, NColor.LightGray);
            m_Legend.VerticalPlacement = ENVerticalPlacement.Top;

            // configure chart
            NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];

			chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XOrdinalYLinear);

            // add interlace stripe
			NLinearScale linearScale = chart.Axes[ENCartesianAxis.PrimaryY].Scale as NLinearScale;
			NScaleStrip strip = new NScaleStrip(new NColorFill(ENNamedColor.Beige), null, true, 0, 0, 1, 1);
            strip.Interlaced = true;
            //linearScale.Strips.Add(strip);

			// setup a bar series
			NBarSeries bar = new NBarSeries();
			bar.Name = "Bar Series";
			bar.InflateMargins = true;
			bar.UseXValues = false;
            bar.LegendView.Mode = ENSeriesLegendMode.DataPoints;

			// add some data to the bar series
			bar.LegendView.Mode = ENSeriesLegendMode.DataPoints;
			bar.DataPoints.Add(new NBarDataPoint(18, "C++"));
			bar.DataPoints.Add(new NBarDataPoint(15, "Ruby"));
			bar.DataPoints.Add(new NBarDataPoint(21, "Python"));
			bar.DataPoints.Add(new NBarDataPoint(23, "Java"));
			bar.DataPoints.Add(new NBarDataPoint(27, "Javascript"));
			bar.DataPoints.Add(new NBarDataPoint(29, "C#"));
			bar.DataPoints.Add(new NBarDataPoint(26, "PHP"));
            bar.DataPoints.Add(new NBarDataPoint(17, "Objective C"));
            bar.DataPoints.Add(new NBarDataPoint(24, "SQL"));
            bar.DataPoints.Add(new NBarDataPoint(13, "Object Pascal"));
            bar.DataPoints.Add(new NBarDataPoint(19, "Visual Basic"));
            bar.DataPoints.Add(new NBarDataPoint(16, "Open Edge ABL"));

			chart.Series.Add(bar);

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

            NTextBox legendHeaderTextBox = new NTextBox();
            legendHeaderTextBox.TextChanged += OnLegendHeaderTextBoxChanged;
			stack.Add(NPairBox.Create("Header: ", legendHeaderTextBox));

            NTextBox legendFooterTextBox = new NTextBox();
            legendFooterTextBox.TextChanged += OnLegendFooterTextBoxChanged;
            stack.Add(NPairBox.Create("Footer: ", legendFooterTextBox));

            m_HorizontalInterlaceStripesCheckBox = new NCheckBox("Horizontal Interlace Stripes");
            m_HorizontalInterlaceStripesCheckBox.CheckedChanged += OnVerticalInterlaceStripesCheckBoxCheckedChanged;
            stack.Add(m_HorizontalInterlaceStripesCheckBox);

            m_VerticalInterlaceStripesCheckBox = new NCheckBox("Vertical Interlace Stripes");
            m_VerticalInterlaceStripesCheckBox.CheckedChanged += OnHorizontalInterlaceStripesCheckBoxCheckedChanged;
            stack.Add(m_VerticalInterlaceStripesCheckBox);

            NCheckBox showHorizontalGridLinesCheckBox = new NCheckBox("Show Horizontal Gridlines");
            showHorizontalGridLinesCheckBox.Checked = true;
            showHorizontalGridLinesCheckBox.CheckedChanged += OnShowHorizontalGridLinesCheckBoxCheckedChanged;
            stack.Add(showHorizontalGridLinesCheckBox);

            NCheckBox showVerticalGridLinesCheckBox = new NCheckBox("Show Vertical Gridlines");
            showVerticalGridLinesCheckBox.Checked = true;
            showVerticalGridLinesCheckBox.CheckedChanged += OnShowVerticalGridLinesCheckBoxCheckedChanged;
            stack.Add(showVerticalGridLinesCheckBox);



            return boxGroup;
		}
        protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to modify the legend appearance.</p>";
		}

		#endregion

        #region Implementation

        private void ApplyInterlaceStyles()
        {
            NLegendInterlaceStylesCollection interlaceStyles = new NLegendInterlaceStylesCollection();
            
            if (m_HorizontalInterlaceStripesCheckBox.Checked)
            {
                NLegendInterlaceStyle horzInterlaceStyle = new NLegendInterlaceStyle();
                horzInterlaceStyle.Fill = new NColorFill(NColor.FromColor(NColor.LightBlue, 0.5f));
                horzInterlaceStyle.Type = ENLegendInterlaceStyleType.Row;
                horzInterlaceStyle.Length = 1;
                horzInterlaceStyle.Interval = 1;

                interlaceStyles.Add(horzInterlaceStyle);
            }

            if (m_VerticalInterlaceStripesCheckBox.Checked)
            {
                NLegendInterlaceStyle vertInterlaceStyle = new NLegendInterlaceStyle();
                vertInterlaceStyle.Fill = new NColorFill(NColor.FromColor(NColor.DarkGray, 0.5f));
                vertInterlaceStyle.Type = ENLegendInterlaceStyleType.Col;
                vertInterlaceStyle.Length = 1;
                vertInterlaceStyle.Interval = 1;

                interlaceStyles.Add(vertInterlaceStyle);
            }

            m_Legend.InterlaceStyles = interlaceStyles;
        }

        #endregion

		#region Event Handlers

        private void OnShowVerticalGridLinesCheckBoxCheckedChanged(NValueChangeEventArgs arg)
        {
            if (((NCheckBox)arg.TargetNode).Checked)
            {
                m_Legend.ClearLocalValue(NLegend.VerticalGridStrokeProperty);
            }
            else
            {
                m_Legend.VerticalGridStroke = null;
            }
        }

        private void OnShowHorizontalGridLinesCheckBoxCheckedChanged(NValueChangeEventArgs arg)
        {
            if (((NCheckBox)arg.TargetNode).Checked)
            {
                m_Legend.ClearLocalValue(NLegend.HorizontalGridStrokeProperty);
            }
            else
            {
                m_Legend.HorizontalGridStroke = null;
            }
        }

        void OnLegendHeaderTextBoxChanged(NValueChangeEventArgs arg)
        {
            NLabel header = new NLabel(((NTextBox)arg.TargetNode).Text);
            header.HorizontalPlacement = ENHorizontalPlacement.Center;
            header.TextAlignment = ENContentAlignment.MiddleCenter;
            header.Font = new NFont("Arimo", 14, ENFontStyle.Bold);

            m_Legend.Header = header;
        }

        void OnLegendFooterTextBoxChanged(NValueChangeEventArgs arg)
        {
            NLabel footer = new NLabel(((NTextBox)arg.TargetNode).Text);
            footer.HorizontalPlacement = ENHorizontalPlacement.Center;
            footer.TextAlignment = ENContentAlignment.MiddleCenter;
            footer.Font = new NFont("Arimo", 14, ENFontStyle.Bold);

            m_Legend.Footer = footer;
        }

        void OnVerticalInterlaceStripesCheckBoxCheckedChanged(NValueChangeEventArgs arg)
        {
            ApplyInterlaceStyles();
        }

        void OnHorizontalInterlaceStripesCheckBoxCheckedChanged(NValueChangeEventArgs arg)
        {
            ApplyInterlaceStyles();
        }

		#endregion

        #region Fields

        NLegend m_Legend;

        NCheckBox m_HorizontalInterlaceStripesCheckBox;
        NCheckBox m_VerticalInterlaceStripesCheckBox;

		#endregion

		#region Schema

		public static readonly NSchema NLegendAppearanceExampleSchema;

        #endregion
    }
}
