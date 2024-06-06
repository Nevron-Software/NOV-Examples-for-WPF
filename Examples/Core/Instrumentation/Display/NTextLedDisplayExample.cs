using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;
using System;

namespace Nevron.Nov.Examples.Gauge
{
	/// <summary>
	/// This example demonstrates the functionality of the NTextLedDisplayExample.
	/// </summary>
	public class NTextLedDisplayExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public NTextLedDisplayExample()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        static NTextLedDisplayExample()
        {
			NTextLedDisplayExampleSchema = NSchema.Create(typeof(NTextLedDisplayExample), NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
		{
			NStackPanel stack = new NStackPanel();

			stack.HorizontalPlacement = ENHorizontalPlacement.Left;

			m_TextDisplay1 = CreateTextLedDisplay();
			stack.Add(m_TextDisplay1);

			m_TextDisplay2 = CreateTextLedDisplay();
			stack.Add(m_TextDisplay2);

			m_TextDisplay3 = CreateTextLedDisplay();
			stack.Add(m_TextDisplay3);
			
			return stack;
		}
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();

			NStackPanel propertyStack = new NStackPanel();
			stack.Add(new NUniSizeBoxGroup(propertyStack));

			m_CellCountUpDown = new NNumericUpDown();
            m_CellCountUpDown.ValueChanged += OnCellCountValueChanged;
			m_CellCountUpDown.Value = 20;
            propertyStack.Add(NPairBox.Create("Cell Count:", m_CellCountUpDown));

            // init form controls
            m_CellSizeComboBox = new NComboBox();
			propertyStack.Add(new NPairBox("Cell Size:", m_CellSizeComboBox, true));

			m_CellSizeComboBox.Items.Add(new NComboBoxItem("Small"));
			m_CellSizeComboBox.Items.Add(new NComboBoxItem("Normal"));
			m_CellSizeComboBox.Items.Add(new NComboBoxItem("Large"));
			m_CellSizeComboBox.SelectedIndex = 1;
			m_CellSizeComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnCellSizeComboBoxSelectedIndexChanged);

			m_DisplayStyleComboBox = new NComboBox();
			propertyStack.Add(new NPairBox("Display Style:", m_DisplayStyleComboBox, true));
			m_DisplayStyleComboBox.FillFromEnum<ENDisplayStyle>();
            m_DisplayStyleComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnDisplayStyleComboBoxSelectedIndexChanged);
            m_DisplayStyleComboBox.SelectedIndex = (int)ENDisplayStyle.MatrixCircle;
            

			NButton litFillButton = new NButton("Lit Fill");
			litFillButton.Click += new Function<NEventArgs>(OnLitFillButtonClick);
			propertyStack.Add(litFillButton);

			NButton dimFillButton = new NButton("Dim Fill");
			dimFillButton.Click += new Function<NEventArgs>(OnDimFillButtonClick);
			propertyStack.Add(dimFillButton);


			NTextBox textBox1 = new NTextBox();
            textBox1.TextChanged += OnTextBox1TextChanged;
			textBox1.Text = "Custom Text 1";
            propertyStack.Add(NPairBox.Create("Text 1", textBox1));

            NTextBox textBox2 = new NTextBox();
            textBox2.TextChanged += OnTextBox2TextChanged;
            textBox2.Text = "Custom Text 2";
            propertyStack.Add(NPairBox.Create("Text 2", textBox2));

            NTextBox textBox3 = new NTextBox();
            textBox3.TextChanged += OnTextBox3TextChanged;
            textBox3.Text = "Custom Text 3";
            propertyStack.Add(NPairBox.Create("Text 3", textBox3));

            return stack;
        }

        protected override string GetExampleDescription()
		{
			return @"<p>The example demonstrates the properties of the text led display.</p>";
		}

		#endregion 

		#region Implementation

		private NTextLedDisplay CreateTextLedDisplay()
		{
            NTextLedDisplay textLedDisply = new NTextLedDisplay();

			textLedDisply.CellCountMode = ENDisplayCellCountMode.Fixed;
			textLedDisply.CellCount = 7;
			textLedDisply.BackgroundFill = new NColorFill(NColor.Black);

			textLedDisply.Border = NBorder.CreateSunken3DBorder(new NUIThemeColorMap(ENUIThemeScheme.WindowsClassic));
			textLedDisply.BorderThickness = new NMargins(6);
			textLedDisply.Margins = new NMargins(5);
			textLedDisply.Padding = new NMargins(5);
			textLedDisply.CapEffect = new NGelCapEffect();

            return textLedDisply;
		}

		#endregion

		#region Event Handlers

		void OnLitFillButtonClick(NEventArgs arg)
		{
			NEditorWindow.CreateForType(
                (NFill)m_TextDisplay1.LitFill.DeepClone(), 
                null,
                m_TextDisplay1.DisplayWindow, 
                false, 
                OnLitFillEdited).Open();
		}

		void OnLitFillEdited(NFill fill)
		{
			m_TextDisplay1.LitFill = (NFill)(fill.DeepClone());
			m_TextDisplay2.LitFill = (NFill)(fill.DeepClone());
			m_TextDisplay3.LitFill = (NFill)(fill.DeepClone());
		}

		void OnDimFillButtonClick(NEventArgs arg)
		{
			NEditorWindow.CreateForType(
                (NFill)m_TextDisplay1.DimFill.DeepClone(), 
                null,
				m_TextDisplay1.DisplayWindow, 
                false, 
                OnDimFillEdited).Open();
		}

		void OnDimFillEdited(NFill fill)
		{
			m_TextDisplay1.DimFill = (NFill)(fill.DeepClone());
            m_TextDisplay2.DimFill = (NFill)(fill.DeepClone());
			m_TextDisplay3.DimFill = (NFill)(fill.DeepClone());
		}

		void OnCellSizeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			double segmentWidth = 0.0;
			double segmentGap = 0.0;
			NSize cellSize = new NSize(0.0, 0.0);

			switch (m_CellSizeComboBox.SelectedIndex)
			{
				case 0: // small
					segmentWidth = 2.0;
					segmentGap = 1.0;
					cellSize = new NSize(15, 30);
					break;
				case 1: // normal
					segmentWidth = 3;
					segmentGap = 1;
					cellSize = new NSize(20, 40);
					break;
				case 2: // large
					segmentWidth = 4;
					segmentGap = 2;
					cellSize = new NSize(26, 52);
					break;
			}

            NTextLedDisplay[] displays = new NTextLedDisplay[] { m_TextDisplay1, m_TextDisplay2, m_TextDisplay3 };

			for (int i = 0; i < displays.Length; i++)
			{
                NTextLedDisplay display = displays[i];

				display.CellSize = cellSize;
				display.SegmentGap = segmentGap;
				display.SegmentWidth = segmentWidth;
			}
		}

		void OnDisplayStyleComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_TextDisplay1.DisplayStyle = (ENDisplayStyle)m_DisplayStyleComboBox.SelectedIndex;
			m_TextDisplay2.DisplayStyle = (ENDisplayStyle)m_DisplayStyleComboBox.SelectedIndex;
			m_TextDisplay3.DisplayStyle = (ENDisplayStyle)m_DisplayStyleComboBox.SelectedIndex;
		}

        private void OnTextBox1TextChanged(NValueChangeEventArgs arg)
        {
			m_TextDisplay1.Text = (string)arg.NewValue;
        }

		private void OnTextBox2TextChanged(NValueChangeEventArgs arg)
        {
            m_TextDisplay2.Text = (string)arg.NewValue;
        }

        private void OnTextBox3TextChanged(NValueChangeEventArgs arg)
        {
            m_TextDisplay3.Text = (string)arg.NewValue;
        }

        private void OnCellCountValueChanged(NValueChangeEventArgs arg)
        {
            m_TextDisplay1.CellCount = (int)m_CellCountUpDown.Value;
            m_TextDisplay2.CellCount = (int)m_CellCountUpDown.Value;
            m_TextDisplay3.CellCount = (int)m_CellCountUpDown.Value;
        }

        #endregion

        #region Fields

        private NComboBox m_DisplayStyleComboBox;
		private NTextLedDisplay m_TextDisplay1;
		private NTextLedDisplay m_TextDisplay2;
		private NTextLedDisplay m_TextDisplay3;
		private NComboBox m_CellSizeComboBox;

		private NNumericUpDown m_CellCountUpDown;

        #endregion

        #region Schema

        public static readonly NSchema NTextLedDisplayExampleSchema;

        #endregion
	}
}