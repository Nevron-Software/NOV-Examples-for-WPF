using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NMessageBoxExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NMessageBoxExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NMessageBoxExample()
		{
			NMessageBoxExampleSchema = NSchema.Create(typeof(NMessageBoxExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NStackPanel stack = new NStackPanel();
			stack.PreferredWidth = 300;
			stack.HorizontalPlacement = ENHorizontalPlacement.Left;
			
			// Create a text box for the message box title
			m_TitleTextBox = new NTextBox("Message Box");
			stack.Add(CreatePairBox("Title:", m_TitleTextBox));

			// Create a text box for the message box content
			m_ContentTextBox = new NTextBox("Here goes the content.\nIt can be multiline.");
			m_ContentTextBox.Multiline = true;
			m_ContentTextBox.AcceptsEnter = true;
			m_ContentTextBox.AcceptsTab = true;
			m_ContentTextBox.PreferredHeight = 100;
			m_ContentTextBox.HScrollMode = ENScrollMode.WhenNeeded;
			m_ContentTextBox.VScrollMode = ENScrollMode.WhenNeeded;
			m_ContentTextBox.WordWrap = false;
			stack.Add(CreatePairBox("Content:", m_ContentTextBox));

			// Create the message box buttons combo box
			m_ButtonsComboBox = new NComboBox();
			m_ButtonsComboBox.FillFromEnum<ENMessageBoxButtons>();
			stack.Add(CreatePairBox("Buttons:", m_ButtonsComboBox));

			// Create the message box icon combo box
			m_IconComboBox = new NComboBox();
			m_IconComboBox.FillFromEnum<ENMessageBoxIcon>();
			m_IconComboBox.SelectedIndex = 1;
			stack.Add(CreatePairBox("Icon:", m_IconComboBox));

			// Create the show button
			NLabel label = new NLabel("Show");
			label.HorizontalPlacement = ENHorizontalPlacement.Center;
			NButton showButton = new NButton(label);
			showButton.Click += new Function<NEventArgs>(OnShowButtonClick);
			stack.Add(showButton);

			return new NUniSizeBoxGroup(stack);
		}
		protected override NWidget CreateExampleControls()
		{
			// Create the events log
			m_EventsLog = new NExampleEventsLog();
			return m_EventsLog;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create message boxes in NOV. Use the controls at the top to set
	the title, the content and the buttons of the message box.
</p>
";
		}

		#endregion

		#region Event Handlers

		private void OnShowButtonClick(NEventArgs args)
		{
			NButton button = (NButton)args.TargetNode;

			NMessageBoxSettings settings = new NMessageBoxSettings(
				m_ContentTextBox.Text,                                  // the message box content
				m_TitleTextBox.Text,                                    // the message box title
				(ENMessageBoxButtons)m_ButtonsComboBox.SelectedIndex,   // the button configuration of the message box
				(ENMessageBoxIcon)m_IconComboBox.SelectedIndex,         // the icon to use
				ENMessageBoxDefaultButton.Button1,						// the default focused button
				DisplayWindow);                                           // the parent window of the message box

			NMessageBox.Show(settings).Then(delegate(ENWindowResult result)     // delegate that gets called when the message box is closed
			{
				m_EventsLog.LogEvent("Message box result: '" + result.ToString() + "'");
			});
		}

		#endregion

		#region Fields

		private NTextBox m_TitleTextBox;
		private NTextBox m_ContentTextBox;
		private NComboBox m_ButtonsComboBox;
		private NComboBox m_IconComboBox;
		private NExampleEventsLog m_EventsLog;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NMessageBoxExample.
		/// </summary>
		public static readonly NSchema NMessageBoxExampleSchema;

		#endregion

		#region Static Methods

		private static NPairBox CreatePairBox(string text, NWidget widget)
		{
			NLabel label = new NLabel(text);
			label.HorizontalPlacement = ENHorizontalPlacement.Right;
			label.VerticalPlacement = ENVerticalPlacement.Center;
			widget.VerticalPlacement = ENVerticalPlacement.Center;

			return new NPairBox(label, widget, true);
		}

		#endregion
	}
}