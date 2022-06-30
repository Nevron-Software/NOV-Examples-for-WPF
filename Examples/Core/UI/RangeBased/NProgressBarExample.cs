using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NProgressBarExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NProgressBarExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NProgressBarExample()
		{
			NProgressBarExampleSchema = NSchema.Create(typeof(NProgressBarExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NStackPanel stack = new NStackPanel();
			stack.HorizontalPlacement = ENHorizontalPlacement.Left;
			stack.VerticalPlacement = ENVerticalPlacement.Top;

			// Horizontal progress bar
			m_HorizontalProgressBar = new NProgressBar();
			m_HorizontalProgressBar.Style = ENProgressBarStyle.Horizontal;
			m_HorizontalProgressBar.Mode = DefaultMode;
			m_HorizontalProgressBar.Value = DefaultValue;
			m_HorizontalProgressBar.BufferedValue = DefaultBufferedValue;
			m_HorizontalProgressBar.PreferredSize = new NSize(300, 30);
			m_HorizontalProgressBar.VerticalPlacement = ENVerticalPlacement.Top;
			stack.Add(new NGroupBox("Horizontal", m_HorizontalProgressBar));

			// Vertical progress bar
			m_VerticalProgressBar = new NProgressBar();
			m_VerticalProgressBar.Style = ENProgressBarStyle.Vertical;
			m_VerticalProgressBar.Mode = DefaultMode;
			m_VerticalProgressBar.Value = DefaultValue;
			m_VerticalProgressBar.BufferedValue = DefaultBufferedValue;
			m_VerticalProgressBar.PreferredSize = new NSize(30, 300);
			m_VerticalProgressBar.HorizontalPlacement = ENHorizontalPlacement.Left;			
			stack.Add(new NGroupBox("Vertical", m_VerticalProgressBar));

			// Circular progress bar - 50% rim
			m_CircularProgressBar1 = new NProgressBar();
			m_CircularProgressBar1.Style = ENProgressBarStyle.Circular;
			m_CircularProgressBar1.Mode = DefaultMode;
			m_CircularProgressBar1.Value = DefaultValue;
			m_CircularProgressBar1.BufferedValue = DefaultBufferedValue;
			m_CircularProgressBar1.PreferredSize = new NSize(150, 150);

			// Circular progress bar - 100% rim
			m_CircularProgressBar2 = new NProgressBar();
			m_CircularProgressBar2.Style = ENProgressBarStyle.Circular;
			m_CircularProgressBar2.Mode = DefaultMode;
			m_CircularProgressBar2.Value = DefaultValue;
			m_CircularProgressBar2.BufferedValue = DefaultBufferedValue;
			m_CircularProgressBar2.RimWidthPercent = 100;
			m_CircularProgressBar2.PreferredSize = new NSize(150, 150);

			NPairBox pairBox = new NPairBox(m_CircularProgressBar1, m_CircularProgressBar2);
			pairBox.Spacing = 30;

			stack.Add(new NGroupBox("Circular", pairBox));

			// Create the Progress bars array
			m_ProgressBars = new NProgressBar[4];
			m_ProgressBars[0] = m_HorizontalProgressBar;
			m_ProgressBars[1] = m_VerticalProgressBar;
			m_ProgressBars[2] = m_CircularProgressBar1;
			m_ProgressBars[3] = m_CircularProgressBar2;

			return stack;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			stack.HorizontalPlacement = ENHorizontalPlacement.Left;
			stack.Direction = ENHVDirection.TopToBottom;

			// The Mode combo box
			NComboBox comboBox = new NComboBox();
			comboBox.FillFromEnum<ENProgressBarMode>();
			comboBox.SelectedIndex = (int)m_HorizontalProgressBar.Mode;
			comboBox.SelectedIndexChanged += OnModeSelected;
			stack.Add(NPairBox.Create("Mode:", comboBox));

			// The Label style combo box
			comboBox = new NComboBox();
			comboBox.FillFromEnum<ENProgressBarLabelStyle>();
			comboBox.SelectedIndex = (int)m_HorizontalProgressBar.LabelStyle;
			comboBox.SelectedIndexChanged += OnLabelStyleSelected;
			stack.Add(NPairBox.Create("Label Style:", comboBox));

			// The Value numeric up down
			NNumericUpDown valueUpDown = new NNumericUpDown(0, 100, DefaultValue);
			valueUpDown.ValueChanged += OnValueChanged;

			m_ValuePairBox = NPairBox.Create("Value:", valueUpDown);
			stack.Add(m_ValuePairBox);

			// The Buffered value numeric up down
			NNumericUpDown bufferedValueUpDown = new NNumericUpDown(0, 100, DefaultBufferedValue);
			bufferedValueUpDown.ValueChanged += OnBufferedValueChanged;

			m_BufferedValuePairBox = NPairBox.Create("Buffered Value:", bufferedValueUpDown);
			stack.Add(m_BufferedValuePairBox);

			// The Indeterminate part size numeric up down
			NNumericUpDown indeterminateSizeUpDown = new NNumericUpDown(1, 100, 25);
			indeterminateSizeUpDown.ValueChanged += OnIndeterminateSizeUpDownValueChanged;

			m_IndeterminatePartSizePairBox = NPairBox.Create("Indeterminate Size (%):", indeterminateSizeUpDown);
			stack.Add(m_IndeterminatePartSizePairBox);

			// The Animation speed numeric up down
			NNumericUpDown animationSpeedUpDown = new NNumericUpDown(0.1, 99, 2);
			animationSpeedUpDown.DecimalPlaces = 1;
			animationSpeedUpDown.Step = 0.1;
			animationSpeedUpDown.ValueChanged += OnAnimationSpeedUpDownValueChanged;

			m_AnimationSpeedPairBox = NPairBox.Create("Animation Speed (%):", animationSpeedUpDown);
			stack.Add(m_AnimationSpeedPairBox);

			// Update controls visibility
			UpdateControlsVisibility(DefaultMode);

			return new NUniSizeBoxGroup(stack);
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create and use progress bars. The progress bar is a widget that
	fills to indicate the progress of an operation. The <b>Style</b> property determines whether
	it is horizontally, vertically oriented or circular. The <b>Minimum</b> and <b>Maximum</b> properties
	determine the start and the end of the operation and the <b>Value</b> property indicates its current progress.
	All progress bars can have a label and its style is controlled through the <b>LabelStyle</b> property.
	Circular progress bars let you specify the width of their rim in percent relative to the size of the
	progress bar as this example demonstrates.
</p>
<p>
	The <b>Mode</b> property determines the progress bar mode and can be:
</p>
<ul>
	<li><b>Determinate</b> - the progress bar shows the progress of an operation from 0 to 100%. This is the default mode.</li>
	<li><b>Indeterminate</b> - the progress bar shows an animation indicating that a long-running operation is executing. No specific
		progress is shown. This mode should be used for operations whose progress cannot be estimated, for example a long-running database query.</li>
	<li><b>Buffered</b> - the progress bar shows the progress of an operation from 0 to 100%. It also additionally shows the buffered
		value of the operation with a lighter color. The buffered value is specified through the <b>BufferedValue</b> property.</li>
</ul>
";
		}

		#endregion

		#region Implementation

		private void UpdateControlsVisibility(ENProgressBarMode mode)
		{
			switch (mode)
			{
				case ENProgressBarMode.Determinate:
					m_ValuePairBox.Visibility = ENVisibility.Visible;
					m_BufferedValuePairBox.Visibility = ENVisibility.Collapsed;
					m_IndeterminatePartSizePairBox.Visibility = ENVisibility.Collapsed;
					m_AnimationSpeedPairBox.Visibility = ENVisibility.Collapsed;
					break;
				case ENProgressBarMode.Indeterminate:
					m_ValuePairBox.Visibility = ENVisibility.Collapsed;
					m_BufferedValuePairBox.Visibility = ENVisibility.Collapsed;
					m_IndeterminatePartSizePairBox.Visibility = ENVisibility.Visible;
					m_AnimationSpeedPairBox.Visibility = ENVisibility.Visible;
					break;
				case ENProgressBarMode.Buffered:
					m_ValuePairBox.Visibility = ENVisibility.Visible;
					m_BufferedValuePairBox.Visibility = ENVisibility.Visible;
					m_IndeterminatePartSizePairBox.Visibility = ENVisibility.Collapsed;
					m_AnimationSpeedPairBox.Visibility = ENVisibility.Collapsed;
					break;
			}
		}

		#endregion

		#region Event Handlers

		private void OnModeSelected(NValueChangeEventArgs arg)
		{
			ENProgressBarMode mode = (ENProgressBarMode)(int)arg.NewValue;
			for (int i = 0; i < m_ProgressBars.Length; i++)
			{
				m_ProgressBars[i].Mode = mode;
			}

			UpdateControlsVisibility(mode);
		}
		private void OnLabelStyleSelected(NValueChangeEventArgs arg)
		{
			ENProgressBarLabelStyle labelStyle = (ENProgressBarLabelStyle)(int)arg.NewValue;
			for (int i = 0; i < m_ProgressBars.Length; i++)
			{
				m_ProgressBars[i].LabelStyle = labelStyle;
			}
		}
		private void OnValueChanged(NValueChangeEventArgs args)
		{
			double value = (double)args.NewValue;
			for (int i = 0; i < m_ProgressBars.Length; i++)
			{
				m_ProgressBars[i].Value = value;
			}
		}
		private void OnBufferedValueChanged(NValueChangeEventArgs args)
		{
			double bufferedValue = (double)args.NewValue;
			for (int i = 0; i < m_ProgressBars.Length; i++)
			{
				m_ProgressBars[i].BufferedValue = bufferedValue;
			}
		}
		private void OnIndeterminateSizeUpDownValueChanged(NValueChangeEventArgs arg)
		{
			double indeterminateSize = (double)arg.NewValue;
			for (int i = 0; i < m_ProgressBars.Length; i++)
			{
				m_ProgressBars[i].IndeterminatePartSizePercent = indeterminateSize;
			}

		}
		private void OnAnimationSpeedUpDownValueChanged(NValueChangeEventArgs arg)
		{
			double animationSpeed = (double)arg.NewValue;
			for (int i = 0; i < m_ProgressBars.Length; i++)
			{
				m_ProgressBars[i].AnimationSpeed = animationSpeed;
			}
		}

		#endregion

		#region Fields

		private NProgressBar m_HorizontalProgressBar;
		private NProgressBar m_VerticalProgressBar;
		private NProgressBar m_CircularProgressBar1;
		private NProgressBar m_CircularProgressBar2;
		private NProgressBar[] m_ProgressBars;

		private NPairBox m_ValuePairBox;
		private NPairBox m_BufferedValuePairBox;
		private NPairBox m_IndeterminatePartSizePairBox;
		private NPairBox m_AnimationSpeedPairBox;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NProgressBarExample.
		/// </summary>
		public static readonly NSchema NProgressBarExampleSchema;

		#endregion

		#region Constants

		private const ENProgressBarMode DefaultMode = ENProgressBarMode.Buffered;
		private const double DefaultValue = 40;
		private const double DefaultBufferedValue = 60;

		#endregion
	}
}