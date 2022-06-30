using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples
{
    public abstract class NChartExampleBase : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NChartExampleBase()
        {
        }
        /// <summary>
        /// Static constructor.
        /// </summary>
        static NChartExampleBase()
        {
            NChartExampleBaseSchema = NSchema.Create(typeof(NChartExampleBase), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Protected Overrides - Example

		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			
			return stack;
		}

        #endregion

        #region Protected Overridable

		protected virtual void InitChart()
		{
		}

		#endregion

		#region Implementation


		protected NChartView CreateCartesianChartView()
		{
			m_ChartView = new NChartView();

			m_ChartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			return m_ChartView;
		}

		protected NChartView CreateFunnelChartView()
		{
			m_ChartView = new NChartView();

			m_ChartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Funnel);

			return m_ChartView;
		}

		protected NChartView CreateTreeMapView()
		{
			m_ChartView = new NChartView();

			m_ChartView.Surface.CreatePredefinedChart(ENPredefinedChartType.TreeMap);

			return m_ChartView;
		}

		protected NChartView CreatePieChartView()
		{
			m_ChartView = new NChartView();

			m_ChartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Pie);

			return m_ChartView;
		}

		protected NChartView CreatePolarChartView()
		{
			m_ChartView = new NChartView();

			m_ChartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Polar);

			return m_ChartView;
		}

		protected NChartView CreateRadarChartView()
		{
			m_ChartView = new NChartView();

			m_ChartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Radar);

			return m_ChartView;
		}



        #endregion

		#region Fields

		protected NChartView m_ChartView;

		#endregion

		#region Schema

		/// <summary>
        /// Schema associated with NChartExampleBase.
        /// </summary>
        public static readonly NSchema NChartExampleBaseSchema;

        #endregion

		#region Constants

		private const string SwitchToCommandBars = "Switch to Command Bars";
		private const string SwitchToRibbon = "Switch to Ribbon";

		#endregion
	}
}