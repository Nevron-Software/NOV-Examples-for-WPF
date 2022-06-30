using System.Text;

using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NTaskDialogExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NTaskDialogExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NTaskDialogExample()
		{
			NTaskDialogExampleSchema = NSchema.Create(typeof(NTaskDialogExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NStackPanel stack = new NStackPanel();
			stack.HorizontalPlacement = ENHorizontalPlacement.Left;
			
			NButton messageBoxLikeButton = new NButton("Message Box Like");
			messageBoxLikeButton.Click += OnMessageBoxLikeButtonClick;
			stack.Add(messageBoxLikeButton);

			NButton customButtonsButton = new NButton("Custom Buttons");
			customButtonsButton.Click += OnCustomButtonsButtonClick;
			stack.Add(customButtonsButton);

			NButton advancedCustomButtonsButton = new NButton("Advanced Custom Buttons");
			advancedCustomButtonsButton.Click += OnAdvancedCustomButtonsButtonClick;
			stack.Add(advancedCustomButtonsButton);

			NButton radioButtonsButton = new NButton("Radio Buttons");
			radioButtonsButton.Click += OnRadioButtonsButtonClick;
			stack.Add(radioButtonsButton);

			NButton verificationButton = new NButton("Verification Check Box");
			verificationButton.Click += OnVerificationButtonClick;
			stack.Add(verificationButton);

			NButton allFeaturesButton = new NButton("All Features");
			allFeaturesButton.Click += OnAllFeaturesButtonClick;
			stack.Add(allFeaturesButton);

			return stack;
		}
		protected override NWidget CreateExampleControls()
		{
			m_EventsLog = new NExampleEventsLog();
			return m_EventsLog;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create task dialogs in NOV. Click any of the buttons above to show
	a preconfigured task dialog, which demonstrates different task dialog features.
</p>
";
		}

		#endregion

		#region Event Handlers

		private void OnMessageBoxLikeButtonClick(NEventArgs arg)
		{
			NTaskDialog taskDialog = NTaskDialog.Create(DisplayWindow);
			taskDialog.Title = "Task Dialog";
			taskDialog.Header = new NTaskDialogHeader(ENMessageBoxIcon.Question, "Do you want to save the changes?");
			taskDialog.Buttons = ENTaskDialogButton.Yes | ENTaskDialogButton.No | ENTaskDialogButton.Cancel;

			// Change the texts of the Yes and No buttons
			NStackPanel stack = taskDialog.ButtonStrip.GetPredefinedButtonsStack();
			NLabel label = (NLabel)stack[0].GetFirstDescendant(NLabel.NLabelSchema);
			label.Text = "Save";

			label = (NLabel)stack[1].GetFirstDescendant(NLabel.NLabelSchema);
			label.Text = "Don't Save";

			// Subscribe to the Closed event and open the task dialog
			taskDialog.Closed += OnTaskDialogClosed;
			taskDialog.Open();
		}
		private void OnCustomButtonsButtonClick(NEventArgs arg)
		{
			NTaskDialog taskDialog = NTaskDialog.Create(DisplayWindow);
			taskDialog.Title = "Task Dialog";
			taskDialog.Header = new NTaskDialogHeader(ENMessageBoxIcon.Information, "This is a task dialog with custom buttons.");
			taskDialog.CustomButtons = new NTaskDialogCustomButtonCollection("Custom Button 1",
				"Custom Button 2", "Custom Button 3");

			// Subscribe to the Closed event and open the task dialog
			taskDialog.Closed += OnTaskDialogClosed;
			taskDialog.Open();
		}
		private void OnAdvancedCustomButtonsButtonClick(NEventArgs arg)
		{
			NTaskDialog taskDialog = NTaskDialog.Create(DisplayWindow);
			taskDialog.Title = "Task Dialog";
			taskDialog.Header = new NTaskDialogHeader(ENMessageBoxIcon.Information, "This is a task dialog with custom buttons.");
			taskDialog.Content = new NLabel("These custom buttons contain a symbol/image, a title and a description.");

			// Create some custom buttons
			taskDialog.CustomButtons = new NTaskDialogCustomButtonCollection();
			taskDialog.CustomButtons.Add(new NTaskDialogCustomButton("Title Only"));
			taskDialog.CustomButtons.Add(new NTaskDialogCustomButton("Title and Description", "This button has a title and a description."));
			taskDialog.CustomButtons.Add(new NTaskDialogCustomButton(NTaskDialogCustomButton.CreateDefaultSymbol(),
				"Symbol, Title, and Description", "This button has a symbol, a title and a description."));
			taskDialog.CustomButtons.Add(new NTaskDialogCustomButton(NResources.Image__16x16_Mail_png,
				"Image, Title and Description", "This button has an icon, a title and a description."));

			// Subscribe to the Closed event and open the task dialog
			taskDialog.Closed += OnTaskDialogClosed;
			taskDialog.Open();
		}
		private void OnRadioButtonsButtonClick(NEventArgs arg)
		{
			NTaskDialog taskDialog = NTaskDialog.Create(DisplayWindow);
			taskDialog.Title = "Task Dialog";
			taskDialog.Header = new NTaskDialogHeader(ENMessageBoxIcon.Information, "This is a task dialog with radio buttons.");
			taskDialog.RadioButtonGroup = new NTaskDialogRadioButtonGroup("Radio Button 1", "Radio Button 2");
			taskDialog.Buttons = ENTaskDialogButton.OK;

			// Subscribe to the Closed event and open the task dialog
			taskDialog.Closed += OnTaskDialogClosed;
			taskDialog.Open();
		}
		private void OnVerificationButtonClick(NEventArgs arg)
		{
			NTaskDialog taskDialog = NTaskDialog.Create(DisplayWindow);
			taskDialog.Title = "Task Dialog";
			taskDialog.Header = new NTaskDialogHeader(ENMessageBoxIcon.Information, "This is the header.");
			taskDialog.Content = new NLabel("This is the content.");
			taskDialog.VerificationCheckBox = new NCheckBox("This is the verification check box.");
			taskDialog.Buttons = ENTaskDialogButton.OK | ENTaskDialogButton.Cancel;

			// Subscribe to the Closed event and open the task dialog
			taskDialog.Closed += OnTaskDialogClosed;
			taskDialog.Open();
		}
		private void OnAllFeaturesButtonClick(NEventArgs arg)
		{
			NTaskDialog taskDialog = NTaskDialog.Create(DisplayWindow);
			taskDialog.Title = "Task Dialog";

			// Add header and content
			taskDialog.Header = new NTaskDialogHeader(ENMessageBoxIcon.Information, "This is the header.");
			taskDialog.Content = new NLabel("This is the content.");

			// Add some radio buttons and custom buttons
			taskDialog.RadioButtonGroup = new NTaskDialogRadioButtonGroup("Radio Button 1",
				"Radio Button 2", "Radio Button 3");
			taskDialog.CustomButtons = new NTaskDialogCustomButtonCollection("Custom Button 1",
				"Custom Button 2", "Custom Button 3");

			// Set the common buttons
			taskDialog.Buttons = ENTaskDialogButton.OK | ENTaskDialogButton.Cancel;

			// Add a verification check box and a footer
			taskDialog.VerificationCheckBox = new NCheckBox("This is the verification check box.");
			taskDialog.Footer = new NLabel("This is the footer.");

			// Subscribe to the Closed event and open the task dialog
			taskDialog.Closed += OnTaskDialogClosed;
			taskDialog.Open();
		}

		private void OnTaskDialogClosed(NEventArgs arg)
		{
			NTaskDialog taskDialog = (NTaskDialog)arg.TargetNode;
			int radioButtonIndex = taskDialog.RadioButtonGroup != null ? taskDialog.RadioButtonGroup.SelectedIndex : -1;
			bool verificationChecked = taskDialog.VerificationCheckBox != null ? taskDialog.VerificationCheckBox.Checked : false;

			StringBuilder sb = new StringBuilder();
			sb.AppendLine("Task result: " + taskDialog.TaskResult.ToString());
			sb.AppendLine("Radio button: " + radioButtonIndex.ToString());
			sb.AppendLine("Custom button: " + taskDialog.CustomButtonIndex.ToString());
			sb.AppendLine("Verification: " + verificationChecked.ToString());

			m_EventsLog.LogEvent(sb.ToString());
		}

		#endregion

		#region Fields

		private NExampleEventsLog m_EventsLog;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NTaskDialogExample.
		/// </summary>
		public static readonly NSchema NTaskDialogExampleSchema;

		#endregion
	}
}