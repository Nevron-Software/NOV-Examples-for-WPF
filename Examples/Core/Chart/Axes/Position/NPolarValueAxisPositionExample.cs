using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Demonstrates how to position polar value axes
	/// </summary>
	public class NPolarValueAxisPositionExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NPolarValueAxisPositionExample()
		{

		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NPolarValueAxisPositionExample()
		{
			NPolarValueAxisPositionExampleSchema = NSchema.Create(typeof(NPolarValueAxisPositionExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = CreatePolarChartView();

			// configure title
			chartView.Surface.Titles[0].Text = "Polar Value Axis Position";

			// configure chart
			m_Chart = (NPolarChart)chartView.Surface.Charts[0];

			m_Chart.SetPredefinedPolarAxes(ENPredefinedPolarAxes.AngleValue);

			// setup chart
			m_Chart.InnerRadius = 20;

			// setup polar axis
			NLinearScale linearScale = (NLinearScale)m_Chart.Axes[ENPolarAxis.PrimaryValue].Scale;
			linearScale.ViewRangeInflateMode = ENScaleViewRangeInflateMode.MajorTick;
			linearScale.InflateViewRangeBegin = true;
			linearScale.InflateViewRangeEnd = true;
			linearScale.Labels.OverlapResolveLayouts = new NDomArray<ENLevelLabelsLayout>(new ENLevelLabelsLayout[] { ENLevelLabelsLayout.AutoScale });

			linearScale.MajorGridLines.Visible = true;
			linearScale.MajorGridLines.Stroke.DashStyle = ENDashStyle.Dash;

			// setup polar angle axis
			NAngularScale angularScale = (NAngularScale)m_Chart.Axes[ENPolarAxis.PrimaryAngle].Scale;

			angularScale.MajorGridLines.Visible = true;
			angularScale.Labels.Style.Angle = new NScaleLabelAngle(ENScaleLabelAngleMode.Scale, 0);
			NScaleStrip strip = new NScaleStrip();
			strip.Fill = new NColorFill(new NColor(192, 192, 192, 125));
			strip.Interlaced = true;
			angularScale.Strips.Add(strip);

			// add a const line
			NAxisReferenceLine referenceLine = new NAxisReferenceLine();
			referenceLine.Value = 1.0;
			referenceLine.Stroke = new NStroke(1, NColor.SlateBlue);
			m_Chart.Axes[ENPolarAxis.PrimaryValue].ReferenceLines.Add(referenceLine);

			// create a polar line series
			NPolarLineSeries series1 = new NPolarLineSeries();
			m_Chart.Series.Add(series1);
			series1.Name = "Series 1";
			series1.CloseContour = true;
			series1.UseXValues = true;
			series1.DataLabelStyle = new NDataLabelStyle(false);

			NMarkerStyle markerStyle = new NMarkerStyle();
			markerStyle.Visible = false;
			markerStyle.Size = new NSize(2, 2);
			series1.MarkerStyle = markerStyle;
			Curve1(series1, 50);

			// create a polar line series
			NPolarLineSeries series2 = new NPolarLineSeries();
			m_Chart.Series.Add(series2);
			series2.Name = "Series 2";
			series2.CloseContour = true;
			series2.UseXValues = true;
			series2.DataLabelStyle = new NDataLabelStyle(false);

			markerStyle = new NMarkerStyle();
			markerStyle.Visible = false;
			series2.MarkerStyle = markerStyle;
			Curve2(series2, 100);

			// add a second value axes
			m_RedAxis = m_Chart.Axes[ENPolarAxis.PrimaryValue];
			m_GreenAxis = m_Chart.AddCustomAxis(ENPolarAxisOrientation.Value);

			m_RedAxis.Anchor = new NValueCrossPolarAxisAnchor(0.0, m_Chart.Axes[ENPolarAxis.PrimaryAngle], ENPolarAxisOrientation.Value, ENScaleOrientation.Auto);
			m_GreenAxis.Anchor = new NValueCrossPolarAxisAnchor(90, m_Chart.Axes[ENPolarAxis.PrimaryAngle], ENPolarAxisOrientation.Value, ENScaleOrientation.Auto);

			// color code the axes and series after the stylesheet is applied
			m_RedAxis.Scale.SetColor(NColor.Red);
			m_GreenAxis.Scale.SetColor(NColor.Green);

			series1.Stroke = new NStroke(2, NColor.DarkRed);
			series2.Stroke = new NStroke(2, NColor.DarkGreen);

			series2.ValueAxis = m_GreenAxis;

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);

			// begin angle
			NNumericUpDown beginAngleUpDown = new NNumericUpDown();
			beginAngleUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnBeginAngleUpDownValueChanged);
			stack.Add(NPairBox.Create("Begin Angle:", beginAngleUpDown));

			// radian angle step
			NComboBox radianAngleStepComboBox = new NComboBox();

			radianAngleStepComboBox.Items.Add(new NComboBoxItem("15"));
			radianAngleStepComboBox.Items.Add(new NComboBoxItem("30"));
			radianAngleStepComboBox.Items.Add(new NComboBoxItem("45"));
			radianAngleStepComboBox.Items.Add(new NComboBoxItem("90"));
			radianAngleStepComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnRadianAngleStepComboBoxSelectedIndexChanged);

			stack.Add(NPairBox.Create("Radian Angle Step", radianAngleStepComboBox));

			// red axis position
			stack.Add(new NLabel("Red Axis:"));

			{
				NCheckBox dockRedAxisToBottomCheckBox = new NCheckBox("Dock Bottom");
				dockRedAxisToBottomCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnDockRedAxisToBottomCheckBoxCheckedChanged);
				stack.Add(dockRedAxisToBottomCheckBox);

				m_RedAxisAngleUpDown = new NNumericUpDown();
				m_RedAxisAngleUpDown.Value = 0;
				m_RedAxisAngleUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnRedAxisAngleUpDownValueChanged);
				stack.Add(NPairBox.Create("Angle:", m_RedAxisAngleUpDown));

				NCheckBox redAxisPaintRefelectionCheckBox = new NCheckBox("Paint Reflection");
				redAxisPaintRefelectionCheckBox.Checked = true;
				redAxisPaintRefelectionCheckBox.CheckedChanged += OnRedAxisPaintRefelectionCheckBoxCheckedChanged;
				stack.Add(redAxisPaintRefelectionCheckBox);

				m_RedAxisScaleLabelAngleMode = new NComboBox();
				m_RedAxisScaleLabelAngleMode.FillFromEnum<ENScaleLabelAngleMode>();
				m_RedAxisScaleLabelAngleMode.SelectedIndex = (int)ENScaleLabelAngleMode.View;
				m_RedAxisScaleLabelAngleMode.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnUpdateRedAxisSaleLabelAngle);
				stack.Add(NPairBox.Create("Scale Label Angle Mode:", m_RedAxisScaleLabelAngleMode));

				m_RedAxisSaleLabelAngleUpDown = new NNumericUpDown();
				m_RedAxisSaleLabelAngleUpDown.ValueChanged += OnUpdateRedAxisSaleLabelAngle;
				stack.Add(NPairBox.Create("Scale Label Angle:", m_RedAxisSaleLabelAngleUpDown));

				NNumericUpDown redAxisBeginPercentUpDown = new NNumericUpDown();
				redAxisBeginPercentUpDown.Value = m_RedAxis.Anchor.BeginPercent;
				redAxisBeginPercentUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnRedAxisBeginPercentUpDownValueChanged);
				stack.Add(NPairBox.Create("Begin percent:", redAxisBeginPercentUpDown));

				NNumericUpDown redAxisEndPercentUpDown = new NNumericUpDown();
				redAxisEndPercentUpDown.Value = m_RedAxis.Anchor.EndPercent;
				redAxisEndPercentUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnRedAxisEndPercentUpDownValueChanged);
				stack.Add(NPairBox.Create("End percent:", redAxisEndPercentUpDown));
			}

			// green axis position
			stack.Add(new NLabel("Green Axis:"));

			{
				NCheckBox dockGreenAxisToLeftCheckBox = new NCheckBox("Dock Left");
				dockGreenAxisToLeftCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnDockGreenAxisToLeftCheckBoxCheckedChanged);
				stack.Add(dockGreenAxisToLeftCheckBox);

				m_GreenAxisAngleUpDown = new NNumericUpDown();
				m_GreenAxisAngleUpDown.Value = 90;
				m_GreenAxisAngleUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnGreenAxisAngleUpDownValueChanged);
				stack.Add(NPairBox.Create("Angle:", m_GreenAxisAngleUpDown));

				NCheckBox greenAxisPaintRefelectionCheckBox = new NCheckBox("Paint Reflection");
				greenAxisPaintRefelectionCheckBox.Checked = true;
				greenAxisPaintRefelectionCheckBox.CheckedChanged += OnGreenAxisPaintRefelectionCheckBoxCheckedChanged;
				stack.Add(greenAxisPaintRefelectionCheckBox);

				m_GreenAxisScaleLabelAngleMode = new NComboBox();
				m_GreenAxisScaleLabelAngleMode.FillFromEnum<ENScaleLabelAngleMode>();
				m_GreenAxisScaleLabelAngleMode.SelectedIndex = (int)ENScaleLabelAngleMode.View;
				m_GreenAxisScaleLabelAngleMode.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnUpdateGreenAxisSaleLabelAngle);
				stack.Add(NPairBox.Create("Scale Label Angle Mode:", m_GreenAxisScaleLabelAngleMode));

				m_GreenAxisSaleLabelAngleUpDown = new NNumericUpDown();
				m_GreenAxisSaleLabelAngleUpDown.ValueChanged += OnUpdateGreenAxisSaleLabelAngle;
				stack.Add(NPairBox.Create("Scale Label Angle:", m_GreenAxisSaleLabelAngleUpDown));

				NNumericUpDown greenAxisBeginPercentUpDown = new NNumericUpDown();
				greenAxisBeginPercentUpDown.Value = m_GreenAxis.Anchor.BeginPercent;
				greenAxisBeginPercentUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnGreenAxisBeginPercentUpDownValueChanged);
				stack.Add(NPairBox.Create("Begin percent:", greenAxisBeginPercentUpDown));

				NNumericUpDown greenAxisEndPercentUpDown = new NNumericUpDown();
				greenAxisEndPercentUpDown.Value = m_GreenAxis.Anchor.EndPercent;
				greenAxisEndPercentUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnGreenAxisEndPercentUpDownValueChanged);
				stack.Add(NPairBox.Create("End percent:", greenAxisEndPercentUpDown));
			}


			return group;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to control the polar value axis position.</p>";
		}

		#endregion

		#region Event Handlers

		void OnUpdateGreenAxisSaleLabelAngle(NValueChangeEventArgs arg)
		{
			(m_GreenAxis.Scale as NLinearScale).Labels.Style.Angle = new NScaleLabelAngle((ENScaleLabelAngleMode)m_GreenAxisScaleLabelAngleMode.SelectedIndex, m_GreenAxisSaleLabelAngleUpDown.Value);
		}

		void OnGreenAxisPaintRefelectionCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			m_GreenAxis.PaintReflection = ((NCheckBox)arg.TargetNode).Checked;
		}

		void OnGreenAxisEndPercentUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_GreenAxis.Anchor.EndPercent = (float)((NNumericUpDown)arg.TargetNode).Value;
		}

		void OnGreenAxisBeginPercentUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_GreenAxis.Anchor.BeginPercent = (float)((NNumericUpDown)arg.TargetNode).Value;
		}

		void OnGreenAxisAngleUpDownValueChanged(NValueChangeEventArgs arg)
		{
			NValueCrossPolarAxisAnchor valueCrossPolarAnchor = m_GreenAxis.Anchor as NValueCrossPolarAxisAnchor;

			if (valueCrossPolarAnchor != null)
			{
				valueCrossPolarAnchor.Value = m_GreenAxisAngleUpDown.Value;
			}
		}

		void OnDockGreenAxisToLeftCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			if (((NCheckBox)arg.TargetNode).Checked)
			{
				m_GreenAxis.Anchor = new NDockPolarAxisAnchor(ENPolarAxisDockZone.Left);
				m_GreenAxisAngleUpDown.Enabled = false;
			}
			else
			{
				m_GreenAxis.Anchor = new NValueCrossPolarAxisAnchor(m_GreenAxisAngleUpDown.Value, m_Chart.Axes[ENPolarAxis.PrimaryAngle], ENPolarAxisOrientation.Value, ENScaleOrientation.Auto);
				m_GreenAxisAngleUpDown.Enabled = true;
			}
		}

		void OnUpdateRedAxisSaleLabelAngle(NValueChangeEventArgs arg)
		{
			(m_RedAxis.Scale as NLinearScale).Labels.Style.Angle = new NScaleLabelAngle((ENScaleLabelAngleMode)m_RedAxisScaleLabelAngleMode.SelectedIndex, m_RedAxisSaleLabelAngleUpDown.Value);
		}

		void OnRedAxisPaintRefelectionCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			m_RedAxis.PaintReflection = ((NCheckBox)arg.TargetNode).Checked;
		}

		void OnRedAxisEndPercentUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_RedAxis.Anchor.EndPercent = (float)((NNumericUpDown)arg.TargetNode).Value;
		}

		void OnRedAxisBeginPercentUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_RedAxis.Anchor.BeginPercent = (float)((NNumericUpDown)arg.TargetNode).Value;
		}

		void OnRedAxisAngleUpDownValueChanged(NValueChangeEventArgs arg)
		{
			NValueCrossPolarAxisAnchor valueCrossPolarAnchor = m_RedAxis.Anchor as NValueCrossPolarAxisAnchor;

			if (valueCrossPolarAnchor != null)
			{
				valueCrossPolarAnchor.Value = m_RedAxisAngleUpDown.Value;
			}
		}

		void OnDockRedAxisToBottomCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			if (((NCheckBox)arg.TargetNode).Checked)
			{
				m_RedAxis.Anchor = new NDockPolarAxisAnchor(ENPolarAxisDockZone.Bottom);
				m_RedAxisAngleUpDown.Enabled = false;
			}
			else
			{
				m_RedAxis.Anchor = new NValueCrossPolarAxisAnchor(m_RedAxisAngleUpDown.Value, m_Chart.Axes[ENPolarAxis.PrimaryAngle], ENPolarAxisOrientation.Value, ENScaleOrientation.Auto);
				m_RedAxisAngleUpDown.Enabled = true;
			}
		}

		void OnRadianAngleStepComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			NAngularScale angleScale = (NAngularScale)m_Chart.Axes[ENPolarAxis.PrimaryAngle].Scale;

			angleScale.MajorTickMode = ENMajorTickMode.CustomStep;

			switch (((NComboBox)arg.TargetNode).SelectedIndex)
			{
				case 0:
					angleScale.CustomStep = new NAngle(15, NUnit.Degree);
					break;

				case 1:
					angleScale.CustomStep = new NAngle(30, NUnit.Degree);
					break;

				case 2:
					angleScale.CustomStep = new NAngle(45, NUnit.Degree);
					break;
				case 3:
					angleScale.CustomStep = new NAngle(90, NUnit.Degree);
					break;

				default:
					NDebug.Assert(false);
					break;
			}
		}

		void OnBeginAngleUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_Chart.BeginAngle = new NAngle(((NNumericUpDown)arg.TargetNode).Value, NUnit.Degree);
		}

		#endregion

		#region Implementation

		internal static void Curve1(NPolarLineSeries series, int count)
		{
			series.DataPoints.Clear();

			double angleStep = 2 * Math.PI / count;

			for (int i = 0; i < count; i++)
			{
				double angle = i * angleStep;
				double radius = 1 + Math.Cos(angle);

				series.DataPoints.Add(new NPolarLineDataPoint((double)(angle * 180.0 / Math.PI), radius));
			}
		}

		internal static void Curve2(NPolarLineSeries series, int count)
		{
			series.DataPoints.Clear();

			double angleStep = 2 * Math.PI / count;

			for (int i = 0; i < count; i++)
			{
				double angle = i * angleStep;
				double radius = 0.2 + 1.7 * Math.Sin(2 * angle) + 1.7 * Math.Cos(2 * angle);

				radius = Math.Abs(radius);

				series.DataPoints.Add(new NPolarLineDataPoint((double)angle * 180.0 / Math.PI, radius));
			}
		}

		#endregion

		#region Fields

		NPolarChart m_Chart;

		NPolarAxis m_RedAxis;
		NPolarAxis m_GreenAxis;

		NNumericUpDown m_RedAxisAngleUpDown;
		NNumericUpDown m_GreenAxisAngleUpDown;

		NNumericUpDown m_RedAxisSaleLabelAngleUpDown;
		NComboBox m_RedAxisScaleLabelAngleMode;

		NNumericUpDown m_GreenAxisSaleLabelAngleUpDown;
		NComboBox m_GreenAxisScaleLabelAngleMode;

		#endregion

		#region Schema

		public static readonly NSchema NPolarValueAxisPositionExampleSchema;

		#endregion

		#region Static Methods

		private static NChartView CreatePolarChartView()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Polar);
			return chartView;
		}

		#endregion
	}
}