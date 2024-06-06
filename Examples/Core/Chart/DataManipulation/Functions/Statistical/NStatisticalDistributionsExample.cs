using System;

using Nevron.Nov.Chart;
using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Formulas;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Demonstrates the basic statistical distribution functions - Normal Distribution, F-Distribution and T-Distribution
	/// </summary>
	public class NStatisticalDistributionsExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public NStatisticalDistributionsExample()
        {

        }
        /// <summary>
        /// Static constructor
        /// </summary>
        static NStatisticalDistributionsExample()
        {
            NStatisticalDistributionsExampleSchema = NSchema.Create(typeof(NStatisticalDistributionsExample), NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

            // configure title
            chartView.Surface.Titles[0].Text = "Statistical Distributions Functions";

            // configure chart
            m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

            // hide legend
            m_Chart.Legend.Mode = ENLegendMode.Disabled;

            // create area series
            m_DistributionArea = new NAreaSeries();
            m_DistributionArea.Visible = false;
            m_DistributionArea.Fill = new NColorFill(DefaultColor);
            m_Chart.Series.Add(m_DistributionArea);
            m_DistributionArea.UseXValues = true;

            // create bar sereis
            m_DistributionBars = new NBarSeries();
            m_DistributionBars.Visible = false;
            m_DistributionBars.Fill = new NColorFill(DefaultColor);
            m_Chart.Series.Add(m_DistributionBars);
            m_DistributionBars.UseXValues = false; 

            return chartViewWithCommandBars;
        }
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();
            NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

            // function combo
            m_DistributionTypeComboBox = new NComboBox();
            m_DistributionTypeComboBox.SelectedIndexChanged += (e) =>
            {
                RebuildParams();
                FillDistribution();
            };
            stack.Add(NPairBox.Create("Function: ", m_DistributionTypeComboBox));

            // parameters stack
            m_ParamsStack = new NStackPanel();
            stack.Add(new NGroupBox("Parameters", m_ParamsStack));

            // inverse function
            NStackPanel inverseStack = new NStackPanel();

            m_InverseGroup = new NGroupBox("Inverse Function", inverseStack);
            stack.Add(m_InverseGroup);

            m_ProbabilityNUD = new NNumericUpDown();
            m_ProbabilityNUD.Minimum = 1;
            m_ProbabilityNUD.Maximum = 99;
            m_ProbabilityNUD.Value = 90;
            m_ProbabilityNUD.ValueChanged += (e) => 
            { 
                SolveInverted(); 
            };
            inverseStack.Add(NPairBox.Create("Probability %: ", m_ProbabilityNUD));

            // one tail check
            m_OneTailCheckBox = new NCheckBox();
            m_OneTailCheckBox.Checked = false;
            m_OneTailCheckBox.CheckedChanged += (e) => 
            { 
                SolveInverted(); 
            };
            inverseStack.Add(NPairBox.Create("One Tail: ", m_OneTailCheckBox));

            // Result X Value label
            m_XValueLabel = new NLabel();
            inverseStack.Add(NPairBox.Create("X Value: ", m_XValueLabel));

			// finally when all controls are created fill the distributions combo
			m_DistributionTypeComboBox.FillFromEnum<StatisticalDistribution>();

            return boxGroup;
        }
        protected override string GetExampleDescription()
        {
            return @"<p>This example demonstrates statistical distribution formulas.</p>
            <p>A statistical distribution, or probability distribution, describes how values are distributed for a field. In other words, the statistical distribution shows which values are common and uncommon.</p>
            <p>The NOV NFormulaCalculator supports most of the statistical distribution functions used in Microsoft Excel. In this example we demonstrate how to use NOV Chart for .NET to display and analyze these functions.</p>";
        }

        #endregion
        
        #region Implementation

        private void RebuildParams()
        {
            m_ParamsStack.Clear();
            m_ParamToWidgetMap.Clear();

            switch ((StatisticalDistribution)m_DistributionTypeComboBox.SelectedItem.Tag)
            {
                case StatisticalDistribution.Beta:
                    AddNumberParameter("Alpha", 5, 1, 30, 0);
                    AddNumberParameter("Beta", 5, 1, 30, 0);

                    m_InverseGroup.Visibility = ENVisibility.Visible;
                    m_OneTailCheckBox.Enabled = false;
                    break;

                case StatisticalDistribution.Binomial:
                    AddNumberParameter("Trials", 21, 10, 100, 0);
                    AddNumberParameter("Probability of Success", 0.6, 0, 1, 2);
                    
                    m_InverseGroup.Visibility = ENVisibility.Collapsed;
                    break;

                case StatisticalDistribution.ChiSquared:
                    AddNumberParameter("Degree of Freedom", 4, 1, 30, 0);
                
                    m_InverseGroup.Visibility = ENVisibility.Visible;
                    m_OneTailCheckBox.Enabled = false;
                    break;

                case StatisticalDistribution.Exponential:
                    AddNumberParameter("Lambda", 1, 0, 4, 2);

                    m_InverseGroup.Visibility = ENVisibility.Visible;
                    m_OneTailCheckBox.Enabled = false;
                    break;

                case StatisticalDistribution.F:
                    AddNumberParameter("Degree of Freedom 1", 4, 1, 30, 0);
                    AddNumberParameter("Degree of Freedom 2", 4, 1, 30, 0);

                    m_InverseGroup.Visibility = ENVisibility.Visible;
                    m_OneTailCheckBox.Enabled = false;
                    break;

                case StatisticalDistribution.Gamma:
                    AddNumberParameter("Alpha", 2, 0.01, 4, 2);
                    AddNumberParameter("Beta", 1, 0.01, 4, 2);

                    m_InverseGroup.Visibility = ENVisibility.Visible;
                    m_OneTailCheckBox.Enabled = false;
                    break;                

                case StatisticalDistribution.Hypergeometric:
                    AddNumberParameter("Population Size", 200, 0, 100, 0);
                    AddNumberParameter("Success Size", 100, 0, 100, 0);
                    AddNumberParameter("Sample Size", 50, 0, 100, 0);

                    m_InverseGroup.Visibility = ENVisibility.Collapsed;
                    break;

                case StatisticalDistribution.LogNormal:
                    AddNumberParameter("Mean", 0, -100, 100, 2);
                    AddNumberParameter("Standard Deviation", 1, 0, 100, 2);

                    m_InverseGroup.Visibility = ENVisibility.Visible;
                    m_OneTailCheckBox.Enabled = false;
                    break;

                case StatisticalDistribution.NegativeBinomial:
                    AddNumberParameter("Successses", 50, 1, 100, 0);
                    AddNumberParameter("Probability of Success", 0.6, 0.01, 1, 2);

                    m_InverseGroup.Visibility = ENVisibility.Collapsed;
                    break;

                case StatisticalDistribution.Normal:                    
                    m_InverseGroup.Visibility = ENVisibility.Visible;
                    m_OneTailCheckBox.Enabled = true;
                    break;

                case StatisticalDistribution.Poisson:
                    AddNumberParameter("Mean", 10, 1, 100, 0);

                    m_InverseGroup.Visibility = ENVisibility.Collapsed;
                    break;

                case StatisticalDistribution.T:
                    AddNumberParameter("Degree of Freedom", 4, 1, 30, 0);

                    m_InverseGroup.Visibility = ENVisibility.Visible;
                    m_OneTailCheckBox.Enabled = true;
                    break;

                case StatisticalDistribution.Weilbull:
                    AddNumberParameter("Alpha", 2, 0.01, 10, 2);
                    AddNumberParameter("Beta", 5, 0.01, 10, 2);

                    m_InverseGroup.Visibility = ENVisibility.Collapsed;
                    break;

                default:
                    break;
            }
        }
        private void FillDistribution()
        {
            switch ((StatisticalDistribution)m_DistributionTypeComboBox.SelectedItem.Tag)
            {
                case StatisticalDistribution.Beta:
                    FillBetaDistribution();
                    break;

                case StatisticalDistribution.Binomial:
                    FillBinomialDistribution();
                    break;

                case StatisticalDistribution.ChiSquared:
                    FillChiSquaredDistribution();
                    break;

                case StatisticalDistribution.Exponential:
                    FillExponentialDistribution();
                    break;

                case StatisticalDistribution.F:
                    FillFDistribution();
                    break;

                case StatisticalDistribution.Gamma:
                    FillGammaDistribution();
                    break;

                case StatisticalDistribution.Hypergeometric:
                    FillHypergeometricDistribution();
                    break;

                case StatisticalDistribution.LogNormal:
                    FillLogNormalDistribution();
                    break;

                case StatisticalDistribution.NegativeBinomial:
                    FillNegativeBinomialDistrubution();
                    break;

                case StatisticalDistribution.Normal:
                    FillNormalDistribution();
                    break;

                case StatisticalDistribution.Poisson:
                    FillPoissonDistribution();
                    break;

                case StatisticalDistribution.T:
                    FillTDistribution();
                    break;

                case StatisticalDistribution.Weilbull:
                    FillWeibullDistribution();
                    break;

                default:
                    break;
            }
        }
        private void SolveInverted()
        {
            switch ((StatisticalDistribution)m_DistributionTypeComboBox.SelectedItem.Tag)
            {
                case StatisticalDistribution.Beta:
                    SolveBetaInverted();
                    break;

                case StatisticalDistribution.Binomial:
                    break;

                case StatisticalDistribution.ChiSquared:
                    SolveChiSquaredInverted();
                    break;

                case StatisticalDistribution.Exponential:
                    SolveExponentialInverted();
                    break;

                case StatisticalDistribution.F:
                    SolveFInverted();
                    break;

                case StatisticalDistribution.Gamma:
                    SolveGammaInverted();
                    break;

                case StatisticalDistribution.Hypergeometric:
                    break;

                case StatisticalDistribution.LogNormal:
                    SolveLogNormalInverted();
                    break;

                case StatisticalDistribution.NegativeBinomial:
                    break;

                case StatisticalDistribution.Normal:
                    SolveNormalInverted();
                    break;

                case StatisticalDistribution.Poisson:
                    break;

                case StatisticalDistribution.T:
                    SolveTInverted();
                    break;

                case StatisticalDistribution.Weilbull:
                    break;

                default:
                    break;
            }
        }

        private void FillBetaDistribution()
        {
            // rebuild data points with values from the Beta-Distribution probability density function
            NFormulaCalculator calculator = new NFormulaCalculator();
            calculator.Formula = string.Format("BETADIST(x, alpha, beta, false)");
            calculator.Variables["alpha"] = GetParameterValue("Alpha");
            calculator.Variables["beta"] = GetParameterValue("Beta");

            // rebuild data points
            RebuildDistributionAreaDataPoints(calculator, 0, 1);

            // solve inverted function
            SolveBetaInverted();
        }
        private void SolveBetaInverted()
        {
            // Probability value
            double probability = m_ProbabilityNUD.Value / 100;

            // Calculate Inverse BETA distribution
            NFormulaCalculator calculator = new NFormulaCalculator();
            calculator.Formula = "BETAINV(probability, alpha, beta)";
            calculator.Variables["probability"] = new NVariant(probability);
            calculator.Variables["alpha"] = GetParameterValue("Alpha");
            calculator.Variables["beta"] = GetParameterValue("Beta");
            double x = calculator.Evaluate().ToDouble(null);

            // update X result label
            m_XValueLabel.Text = x.ToString("G5");

            // highlight result
            HighlightDataPoints(x, true);
        }

        private void FillBinomialDistribution()
        {
            // rebuild data points with values from the Beta-Distribution probability density function
            NFormulaCalculator calculator = new NFormulaCalculator();
            calculator.Formula = string.Format("BINOMDIST(x, trials, probability, false)");
            calculator.Variables["trials"] = GetParameterValue("Trials");
            calculator.Variables["probability"] = GetParameterValue("Probability of Success");

            // rebuild data points
            RebuildDistributionBarsDataPoints(calculator, 0, (int)GetParameterValue("Trials"));
        }

        private void FillChiSquaredDistribution()
        {
            // rebuild data points with values from the ChiSq-Distribution probability density function
            NFormulaCalculator calculator = new NFormulaCalculator();
            calculator.Formula = string.Format("CHISQDIST(x, dof, false)");
            calculator.Variables["dof"] = GetParameterValue("Degree of Freedom");

            // rebuild data points
            RebuildDistributionAreaDataPoints(calculator, 0, 15);

            // solve inverted function
            SolveChiSquaredInverted();
        }
        private void SolveChiSquaredInverted()
        {
            // Probability value
            double probability = m_ProbabilityNUD.Value / 100;

            // Calculate Inverse ChiSquared distribution
            NFormulaCalculator calculator = new NFormulaCalculator();
            calculator.Formula = "CHISQINV(probability, dof)";
            calculator.Variables["probability"] = new NVariant(probability);
            calculator.Variables["dof"] = GetParameterValue("Degree of Freedom");
            double x = calculator.Evaluate().ToDouble(null);

            // update X result label
            m_XValueLabel.Text = x.ToString("G5");

            // highlight result
            HighlightDataPoints(x, true);
        }

        private void FillExponentialDistribution()
        {
            // rebuild data points with values from the Exponent distribution probability density function
            NFormulaCalculator calculator = new NFormulaCalculator();
            calculator.Formula = string.Format("EXPONDIST(x, lambda, false)");
            calculator.Variables["lambda"] = GetParameterValue("Lambda");

            // rebuild data points
            RebuildDistributionAreaDataPoints(calculator, 0, 5);

            // solve inverted function
            SolveExponentialInverted();
        }
        private void SolveExponentialInverted()
        {
            // get probability value
            double probability = m_ProbabilityNUD.Value / 100;

            // calculate Inverse Exponent distribution
            NFormulaCalculator calculator = new NFormulaCalculator();
            calculator.Formula = "EXPONINV(probability, lambda)";
            calculator.Variables["probability"] = new NVariant(probability);
            calculator.Variables["lambda"] = GetParameterValue("Lambda");
            double x = calculator.Evaluate().ToDouble(null);

            // update X result label
            m_XValueLabel.Text = x.ToString("G5");

            // highlight result
            HighlightDataPoints(x, true);
        }

        private void FillFDistribution()
        {
            // rebuild data points with values from the F distribution probability density function, using the specified degrees of freedom
            NFormulaCalculator calculator = new NFormulaCalculator();
            calculator.Formula = "FDIST(x, dof1, dof2, false)";
            calculator.Variables["dof1"]  = GetParameterValue("Degree of Freedom 1");
            calculator.Variables["dof2"] = GetParameterValue("Degree of Freedom 2");

            // rebuild data points of distribution area
            RebuildDistributionAreaDataPoints(calculator, 0, 15);

            // solve inverted
            SolveFInverted();
        }
        private void SolveFInverted()
        {
            // Probability value
            double probability = m_ProbabilityNUD.Value / 100;

            // Calculate Inverse F distribution
            NFormulaCalculator calculator = new NFormulaCalculator();
            calculator.Formula = "FINV(probability, dof1, dof1)";
            calculator.Variables["probability"] = new NVariant(probability);
            calculator.Variables["dof1"] = GetParameterValue("Degree of Freedom 1");
            calculator.Variables["dof2"] = GetParameterValue("Degree of Freedom 2");
            double x = calculator.Evaluate().ToDouble(null);

            // update X result label
            m_XValueLabel.Text = x.ToString("G5");

            // highlight result
            HighlightDataPoints(x, true);
        }

        private void FillGammaDistribution()
        {
            // rebuild data points with values from the Gamma-Distribution probability density function
            NFormulaCalculator calculator = new NFormulaCalculator();
            calculator.Formula = string.Format("GAMMADIST(x, alpha, beta, false)");
            calculator.Variables["alpha"] = GetParameterValue("Alpha");
            calculator.Variables["beta"] = GetParameterValue("Beta");

            // rebuild data points
            RebuildDistributionAreaDataPoints(calculator, 0, 15);

            // solve inverted function
            SolveGammaInverted();
        }
        private void SolveGammaInverted()
        {
            // Probability value
            double probability = m_ProbabilityNUD.Value / 100;

            // Calculate Inverse ChiSquared distribution
            NFormulaCalculator calculator = new NFormulaCalculator();
            calculator.Formula = "GAMMAINV(probability, alpha, beta)";
            calculator.Variables["probability"] = new NVariant(probability);
            calculator.Variables["alpha"] = GetParameterValue("Alpha");
            calculator.Variables["beta"] = GetParameterValue("Beta");
            double x = calculator.Evaluate().ToDouble(null);

            // update X result label
            m_XValueLabel.Text = x.ToString("G5");

            // highlight result
            HighlightDataPoints(x, true);
        }

        private void FillHypergeometricDistribution()
        {
            // rebuild data points with values from the Gamma-Distribution probability density function
            NFormulaCalculator calculator = new NFormulaCalculator();
            calculator.Formula = string.Format("HYPGEOMDIST(x, sampleSize, popSuccess, popSize, false)");

            NVariant popSize = GetParameterValue("Population Size");
            NVariant popSuccess = GetParameterValue("Success Size");
            NVariant sampleSize = GetParameterValue("Sample Size");

            calculator.Variables["popSize"] = popSize;
            calculator.Variables["popSuccess"] = popSuccess;
            calculator.Variables["sampleSize"] = sampleSize;

            // rebuild data points
            RebuildDistributionBarsDataPoints(calculator, 0, NMath.Min((int)sampleSize, (int)popSuccess, (int)popSize));
        }

        private void FillLogNormalDistribution()
        {
            NFormulaCalculator calculator = new NFormulaCalculator();
            calculator.Formula = "LOGNORMDIST(x, mean, stdDev, false)";
            calculator.Variables["mean"] = GetParameterValue("Mean");
            calculator.Variables["stdDev"] = GetParameterValue("Standard Deviation");

            // rebuild data points of distribution area
            RebuildDistributionAreaDataPoints(calculator, 0, 12);

            // solve inverted
            SolveLogNormalInverted();
        }
        private void SolveLogNormalInverted()
        {
            // Probability value
            double probability = m_ProbabilityNUD.Value / 100;

            // solve the LOGNORMINV
            NFormulaCalculator calculator = new NFormulaCalculator();
            calculator.Formula = string.Format("LOGNORMINV(probability, mean, stdDev)");
            calculator.Variables["probability"] = new NVariant(probability);
            calculator.Variables["mean"] = GetParameterValue("Mean");
            calculator.Variables["stdDev"] = GetParameterValue("Standard Deviation");
            double x = calculator.Evaluate().ToDouble(null);

            // update X result label
            m_XValueLabel.Text = x.ToString("G5");

            // highlight result
            HighlightDataPoints(x, false);
        }

        private void FillNegativeBinomialDistrubution()
        {
            NFormulaCalculator calculator = new NFormulaCalculator();

            calculator.Formula = "NEGBINOMDIST(x, number_s, probability_s, false)";

            NVariant number_s = GetParameterValue("Successses");
            calculator.Variables["number_s"] = number_s;
            calculator.Variables["probability_s"] = GetParameterValue("Probability of Success");

            // rebuild data points of distribution area
            RebuildDistributionBarsDataPoints(calculator, 0, 100 + (int)number_s);
        }
        
        private void FillNormalDistribution()
        {
            // rebuild data points with values from the Standard Normal distribution (mean = 0, stdDev = 1)
            NFormulaCalculator calculator = new NFormulaCalculator();
            calculator.Formula = "NORMDIST(x, 0, 1, false)";

            // rebuild data points of distribution area
            RebuildDistributionAreaDataPoints(calculator, -5, 5);

            // solve inverted
            SolveNormalInverted();
        }
        private void SolveNormalInverted()
        {
            // One tailed normal distribution or two tails
            bool oneTail = m_OneTailCheckBox.Checked;

            // Probability value
            double probability = m_ProbabilityNUD.Value / 100.0;

            // for two-tail normal inversion -> calculate probability like this
            if (!oneTail)
            {
                probability = (probability + 1.0) / 2.0;
            }

            // solve the NORM distribution invert function for the specified probability
            NFormulaCalculator calculator = new NFormulaCalculator();
            calculator.Formula = string.Format("NORMINV(probability, 0, 1)");
            calculator.Variables["probability"] = new NVariant(probability);
            double x = calculator.Evaluate().ToDouble(null);

            // update X result label
            m_XValueLabel.Text = x.ToString("G5");

            // highlight result
            HighlightDataPoints(x, oneTail);
        }

        private void FillPoissonDistribution()
        {
            // rebuild data points with values from the Poison Normal distribution
            NFormulaCalculator calculator = new NFormulaCalculator();
            calculator.Formula = "POISSONDIST(x, mean, false)";
            calculator.Variables["mean"] = GetParameterValue("Mean");

            // rebuild data points of distribution area
            RebuildDistributionBarsDataPoints(calculator, 0, 40);
        }

        private void FillTDistribution()
        {
            // rebuild data points with values from the T-Distribution probability density function, using the specified Degree of Freedom
            NFormulaCalculator calculator = new NFormulaCalculator();
            calculator.Formula = string.Format("TDIST(x, dof, false)");
            calculator.Variables["dof"] = GetParameterValue("Degree of Freedom");

            // rebuild data points of distribution area
            RebuildDistributionAreaDataPoints(calculator, -12, 12);

            // solve inverted function
            SolveTInverted();
        }
        private void SolveTInverted()
        {
            // One tailed normal distribution or two tails
            bool oneTail = m_OneTailCheckBox.Checked;

            // Probability value
            double probability = m_ProbabilityNUD.Value / 100;

            // determine the formula
            string formula;
            if (oneTail)
            {
                formula = "TINV(probability, dof)";
            }
            else
            {
                formula = "TINV2T(probability, dof)";
            }

            // Calculate Inverse T Distribution (Students Distribution)
            NFormulaCalculator calculator = new NFormulaCalculator();
            calculator.Formula = formula;
            calculator.Variables["probability"] = new NVariant(probability);
            calculator.Variables["dof"] = GetParameterValue("Degree of Freedom");
            double x = calculator.Evaluate().ToDouble(null);

            // update X result label
            m_XValueLabel.Text = x.ToString("G5");

            // highligh data points
            HighlightDataPoints(x, oneTail);
        }

        private void FillWeibullDistribution()
        {
            // rebuild data points with values from the Weibull distribution
            NFormulaCalculator calculator = new NFormulaCalculator();
            calculator.Formula = "WEIBULLDIST(x, alpha, beta, false)";
            calculator.Variables["alpha"] = GetParameterValue("Alpha");
            calculator.Variables["beta"] = GetParameterValue("Beta");

            // rebuild data points of distribution area
            RebuildDistributionAreaDataPoints(calculator, 0, 20);
        }

        /// <summary>
        /// Rebuilds the data points of the m_DistributionArea
        /// </summary>
        /// <param name="calculator"></param>
        /// <param name="xmin"></param>
        /// <param name="xmax"></param>
        /// <param name="xstep"></param>
        private void RebuildDistributionAreaDataPoints(NFormulaCalculator calculator, double xmin, double xmax)
        {
            // show area and hide bars
            m_DistributionArea.Visible = true;
            m_DistributionBars.Visible = false;

            // switch X axis in linear scale
            m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale = new NLinearScale();

            // clear data points
            m_DistributionArea.DataPoints.Clear();
            m_DistributionBars.DataPoints.Clear();

            // we aim for 300 data points to make a smooth graph
            double step = (xmax - xmin) / 300;

            for (double x = xmin; x <= xmax; x += step)
            {
                // calculate y for the current x
                calculator.Variables["x"] = new NVariant(x);
                double y = (double)calculator.Evaluate();

                // create new data point
                NAreaDataPoint dataPoint = new NAreaDataPoint(x, y);
                m_DistributionArea.DataPoints.Add(dataPoint);
            }
        }
        /// <summary>
        /// Rebuilds the data points of the m_DistributionBars
        /// </summary>
        /// <param name="calculator"></param>
        /// <param name="xmin"></param>
        /// <param name="xmax"></param>
        /// <param name="xstep"></param>
        private void RebuildDistributionBarsDataPoints(NFormulaCalculator calculator, int xmin, int xmax)
        {
            // show area and hide bars
            m_DistributionArea.Visible = false;
            m_DistributionBars.Visible = true;

            // switch X axis in linear scale
            m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale = new NOrdinalScale();

            // clear data points
            m_DistributionArea.DataPoints.Clear();
            m_DistributionBars.DataPoints.Clear();

            // fill data poionts
            for (int x = xmin; x <= xmax; x++)
            {
                // calculate y for the current x
                calculator.Variables["x"] = new NVariant(x);
                double y = (double)calculator.Evaluate();

                // create new data point
                NBarDataPoint dataPoint = new NBarDataPoint(x, y);
                m_DistributionBars.DataPoints.Add(dataPoint);
            }
        }

        /// <summary>
        /// Fills the m_DistributionArea data points with a color, that highlights its X location relative to the specified xValue.
        /// </summary>
        /// <param name="xValue"></param>
        /// <param name="oneTail"></param>
        private void HighlightDataPoints(double xValue, bool oneTail)
        {
            for (int i = 0; i < m_DistributionArea.DataPoints.Count; i++)
            {
                NAreaDataPoint point = m_DistributionArea.DataPoints[i];

                if (oneTail)
                {
                    if (xValue < point.X)
                    {
                        point.Fill = new NColorFill(SelectedColor);
                    }
                    else
                    {
                        point.Fill = new NColorFill(DeselectedColor);
                    }
                }
                else
                {
                    if (xValue < Math.Abs(point.X))
                    {
                        point.Fill = new NColorFill(SelectedColor);
                    }
                    else
                    {
                        point.Fill = new NColorFill(DeselectedColor);
                    }
                }
            }
        }

        /// <summary>
        /// Adds a number parameter widget
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="decimalPlaces"></param>
        private void AddNumberParameter(string paramName, double value, double min, double max, int decimalPlaces)
        {
            // add numeric up-down for the number parameters
            NNumericUpDown nud = new NNumericUpDown();
            nud.Minimum = min;
            nud.Maximum = max;
            nud.Value = value;
            nud.DecimalPlaces = decimalPlaces;
            
            if (decimalPlaces == 2)
            {
                nud.Step = 0.01;
            }
            else 
            {
                nud.Step = 1;
            }
            
            nud.ValueChanged += (e) => { FillDistribution(); };
            m_ParamsStack.Add(NPairBox.Create(paramName + ": ", nud));

            // register in params map
            m_ParamToWidgetMap.Add(paramName, nud);
        }
        /// <summary>
        /// Gets the value of a number parameter
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        private NVariant GetParameterValue(string paramName)
        {
            NWidget widget = m_ParamToWidgetMap[paramName];
            if (widget is NNumericUpDown)
                return new NVariant(((NNumericUpDown)widget).Value);

            return NVariant.Empty;
        }

        #endregion

        #region Fields

        // chart elements
        NCartesianChart m_Chart;
        NAreaSeries m_DistributionArea;
        NBarSeries m_DistributionBars;

        // parameter ui controls for distribution function
        NComboBox m_DistributionTypeComboBox;
        NStackPanel m_ParamsStack;
        NMap<string, NWidget> m_ParamToWidgetMap = new NMap<string, NWidget>();

        // parameter ui controls for inverse function
        NGroupBox m_InverseGroup;
        NNumericUpDown m_ProbabilityNUD;
        NCheckBox m_OneTailCheckBox;
        NLabel m_XValueLabel;

        #endregion

        #region Colors

        public static NColor SelectedColor
        {
            get
            {
                return new NColor(NChartPalette.CoolPalette[0], 200);
            }
        }

        public static NColor DeselectedColor
        {
            get
            {
                return new NColor(NChartPalette.CoolPalette[1], 200);
            }
        }

        public static NColor DefaultColor
        {
            get
            {
                return new NColor(NChartPalette.CoolPalette[2], 200);
            }
        }

        #endregion

        #region Schema

        public static readonly NSchema NStatisticalDistributionsExampleSchema;

        #endregion

        public enum StatisticalDistribution
        {
            Beta,
            Binomial,
            ChiSquared,
            Exponential,
            F,
            Gamma,
            Hypergeometric,
            LogNormal,
            NegativeBinomial,           
            Normal,
            Poisson,
            T,
            Weilbull
        }
    }
}