using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Axis labels orientation example
	/// </summary>
	public class NAxisLabelsOrientationtExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NAxisLabelsOrientationtExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NAxisLabelsOrientationtExample()
		{
			NAxisLabelsOrientationtExampleSchema = NSchema.Create(typeof(NAxisLabelsOrientationtExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_ChartView = new NChartView();
			m_ChartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			m_ChartView.Surface.Titles[0].Text = "Axis Labels Orientation";

			// configure chart
			m_Chart = (NCartesianChart)m_ChartView.Surface.Charts[0];

			m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.PrimaryAndSecondaryLinear);

			for (int i = 0; i < m_Chart.Axes.Count; i++)
			{
				// configure the axes
				NCartesianAxis axis = m_Chart.Axes[i];

				// set the range to [0, 100]
				axis.ViewRangeMode = ENAxisViewRangeMode.FixedRange;
				axis.MinViewRangeValue = 0;
				axis.MaxViewRangeValue = 100;

				NLinearScale linearScale = (NLinearScale)axis.Scale;

				string title = string.Empty;
				switch (i)
				{
					case 0:
						title = "Primary Y";
						break;
					case 1:
						title = "Primary X";
						break;
					case 2:
						title = "Secondary Y";
						break;
					case 3:
						title = "Secondary X";
						break;
				}

				linearScale.Title.Text = title;
				linearScale.MinTickDistance = 30;
				linearScale.Labels.Style.Angle = new NScaleLabelAngle(ENScaleLabelAngleMode.Scale, 0, false);
				linearScale.Labels.Style.AlwaysInsideScale = true;
				linearScale.Labels.OverlapResolveLayouts = new NDomArray<ENLevelLabelsLayout>(new ENLevelLabelsLayout[] { ENLevelLabelsLayout.Stagger2 } );
			}

			m_ChartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			return m_ChartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

			NComboBox orientationComboBox = new NComboBox();
			orientationComboBox.FillFromEnum<ENCartesianChartOrientation>();
			orientationComboBox.SelectedIndex = (int)m_Chart.Orientation;
			orientationComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnOrientationComboBoxSelectedIndexChanged);
			stack.Add(NPairBox.Create("Orientation:", orientationComboBox));

			m_AngleModeComboBox = new NComboBox();
			m_AngleModeComboBox.FillFromEnum<ENScaleLabelAngleMode>();
			m_AngleModeComboBox.SelectedIndex = (int)ENScaleLabelAngleMode.Scale;
			m_AngleModeComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnAxisLabelChanged);
			stack.Add(NPairBox.Create("Angle Mode:", m_AngleModeComboBox));

			m_CustomAngleNumericUpDown = new NNumericUpDown();
			m_CustomAngleNumericUpDown.Minimum = 0;
			m_CustomAngleNumericUpDown.Maximum = 360;
			m_CustomAngleNumericUpDown.Value = 0;
			m_CustomAngleNumericUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnAxisLabelChanged);
			stack.Add(NPairBox.Create("Custom Angle:", m_CustomAngleNumericUpDown));

			m_AllowLabelsToFlipCheckBox = new NCheckBox("Allow Label To Flip");
			m_AllowLabelsToFlipCheckBox.Checked = false;
			m_AllowLabelsToFlipCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnAxisLabelChanged);
			stack.Add(m_AllowLabelsToFlipCheckBox);

			return boxGroup;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to change the orientation of axis labels.</p>";
		}

		#endregion

		#region Event Handlers

		void OnOrientationComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_Chart.Orientation = (ENCartesianChartOrientation)((NComboBox)arg.TargetNode).SelectedIndex;
		}

		void OnAxisLabelChanged(NValueChangeEventArgs arg)
		{
			NCartesianChart chart = (NCartesianChart)m_ChartView.Surface.Charts[0];

			for (int i = 0; i < chart.Axes.Count; i++)
			{
				// configure the axes
				NLinearScale scale = (NLinearScale)chart.Axes[i].Scale;

				scale.Labels.Style.Angle = new NScaleLabelAngle((ENScaleLabelAngleMode)m_AngleModeComboBox.SelectedIndex, (float)m_CustomAngleNumericUpDown.Value, m_AllowLabelsToFlipCheckBox.Checked);
			}
		}

		#endregion

		#region Fields

		private NChartView m_ChartView;
		NCartesianChart m_Chart;
		NComboBox m_AngleModeComboBox;
		NNumericUpDown m_CustomAngleNumericUpDown;
		NCheckBox m_AllowLabelsToFlipCheckBox;

		#endregion

		#region Schema

		public static readonly NSchema NAxisLabelsOrientationtExampleSchema;

		#endregion
	}
}