using System.IO;
using System.Text;

using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.IO;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NFileDialogsExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NFileDialogsExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NFileDialogsExample()
		{
			NFileDialogsExampleSchema = NSchema.Create(typeof(NFileDialogsExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_TextBox = new NTextBox();
			m_TextBox.Multiline = true;
			m_TextBox.AcceptsEnter = true;
			m_TextBox.AcceptsTab = true;
			m_TextBox.VScrollMode = ENScrollMode.WhenNeeded;
			m_TextBox.Text = "This is a sample text.\n\nYou can edit and save it.\n\nYou can also load text from a text file.";
			return m_TextBox;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.Last;
			stack.FitMode = ENStackFitMode.Last;

			// create the buttons group
			NGroupBox buttonsGroup = new NGroupBox("Open Dialogs from Buttons");
			stack.Add(buttonsGroup);

			NStackPanel buttonsStack = new NStackPanel();
			buttonsStack.Direction = ENHVDirection.LeftToRight;
			buttonsGroup.Content = buttonsStack;

			NButton openButton = new NButton("Open File...");
			openButton.Content.HorizontalPlacement = ENHorizontalPlacement.Center;
			openButton.Click += new Function<NEventArgs>(OnOpenButtonClick);
			buttonsStack.Add(openButton);

			NButton openMultiselectButton = new NButton("Choose Multiple Files...");
			openMultiselectButton.Content.HorizontalPlacement = ENHorizontalPlacement.Center;
			openMultiselectButton.Click += new Function<NEventArgs>(OnMultiselectOpenButtonClick);
			buttonsStack.Add(openMultiselectButton);

			NButton saveButton = new NButton("Save to File...");
			saveButton.Content.HorizontalPlacement = ENHorizontalPlacement.Center;
			saveButton.Click += new Function<NEventArgs>(OnSaveButtonClick);
			buttonsStack.Add(saveButton);

			// create the menu group
			NGroupBox menuGroup = new NGroupBox("Open Dialogs from Menu Items");
			stack.Add(menuGroup);
			
			NMenuBar menuBar = new NMenuBar();
			menuGroup.Content = menuBar;

			NMenuDropDown fileMenu = new NMenuDropDown("File");
			menuBar.Items.Add(fileMenu);

			NMenuItem openFileMenuItem = new NMenuItem("Open File...");
			openFileMenuItem.Click += new Function<NEventArgs>(OnOpenFileMenuItemClick);
			fileMenu.Items.Add(openFileMenuItem);

			NMenuItem saveFileMenuItem = new NMenuItem("Save File...");
			saveFileMenuItem.Click += new Function<NEventArgs>(OnSaveFileMenuItemClick);
			fileMenu.Items.Add(saveFileMenuItem);

			// create the dialog group
			NGroupBox dialogGroup = new NGroupBox("Open Dialogs from Dialog");
			stack.Add(dialogGroup);

			NButton showDialogButton = new NButton("Show Dialog...");
			showDialogButton.Click += new Function<NEventArgs>(OnShowDialogButtonClick);
			dialogGroup.Content = showDialogButton;

			// add the events log
			m_EventsLog = new NExampleEventsLog();
			stack.Add(m_EventsLog);

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create and use the open and save file dialogs provided by NOV.
</p>
";
		}

		#endregion

		#region Event Handlers

		private void OnOpenButtonClick(NEventArgs args)
		{
			ShowOpenFileDialog();
		}
		private void OnMultiselectOpenButtonClick(NEventArgs args)
		{
			ShowMultiselectOpenFileDialog();
		}
		private void OnSaveButtonClick(NEventArgs args)
		{
			ShowSaveFileDialog();
		}

		private void OnOpenFileMenuItemClick(NEventArgs arg1)
		{
			ShowOpenFileDialog();
		}
		private void OnSaveFileMenuItemClick(NEventArgs arg1)
		{
			ShowSaveFileDialog();
		}

		private void OnShowDialogButtonClick(NEventArgs arg1)
		{
			NStackPanel stack = new NStackPanel();
			stack.Margins = new NMargins(10);
			stack.VerticalSpacing = 10;
			
			NButton openButton = new NButton("Open File...");
			openButton.Content.HorizontalPlacement = ENHorizontalPlacement.Center;
			openButton.Click += new Function<NEventArgs>(OnOpenButtonClick);
			stack.Add(openButton);

			NButton saveButton = new NButton("Save to File...");
			saveButton.Content.HorizontalPlacement = ENHorizontalPlacement.Center;
			saveButton.Click += new Function<NEventArgs>(OnSaveButtonClick);
			stack.Add(saveButton);

			NButtonStrip closeButtonStrip = new NButtonStrip();
			closeButtonStrip.AddCloseButton();
			stack.Add(closeButtonStrip);

			// create a dialog that is owned by this widget window
			NTopLevelWindow dialog = NApplication.CreateTopLevelWindow();
			dialog.SetupDialogWindow("Show File Dialogs", false);
			dialog.Content = stack;
			dialog.Open();
		}

		#endregion

		#region Implementation - Show Open And Save Dialog

		private void ShowOpenFileDialog()
		{
			NOpenFileDialog openFileDialog = new NOpenFileDialog();
			openFileDialog.FileTypes = new NFileDialogFileType[]
			{
				new NFileDialogFileType("Text Files", "txt"),
				new NFileDialogFileType("XML Files", "xml"),
				new NFileDialogFileType("All Files", "*")
			};
			openFileDialog.SelectedFilterIndex = 0;
			openFileDialog.MultiSelect = false;
			openFileDialog.InitialDirectory = "";
			openFileDialog.Title = "Open Text File";

			openFileDialog.Closed += new Function<NOpenFileDialogResult>(OnOpenFileDialogClosed);
			openFileDialog.RequestShow();
		}
		private void OnOpenFileDialogClosed(NOpenFileDialogResult result)
		{
			switch (result.Result)
			{
				case ENCommonDialogResult.OK:
					NFile file = result.Files[0];
					file.OpenRead().Then(
						delegate (Stream stream)
						{
							using (stream)
							{
								m_TextBox.Text = NStreamHelpers.ReadToEndAsString(stream);
							}

							m_EventsLog.LogEvent("File opened: " + file.Name);
						}
					);
					break;

				case ENCommonDialogResult.Cancel:
					m_EventsLog.LogEvent("File not selected");
					break;

				case ENCommonDialogResult.Error:
					m_EventsLog.LogEvent("Error message: " + result.ErrorException.Message);
					break;
			}
		}

		private void ShowMultiselectOpenFileDialog()
		{
			NOpenFileDialog openFileDialog = new NOpenFileDialog();
			openFileDialog.MultiSelect = true;
			openFileDialog.Title = "Select Multiple Files";

			openFileDialog.Closed += new Function<NOpenFileDialogResult>(OnMultiselectOpenFileDialogClosed);
			openFileDialog.RequestShow();
		}
		private void OnMultiselectOpenFileDialogClosed(NOpenFileDialogResult result)
		{
			switch (result.Result)
			{
				case ENCommonDialogResult.OK:
					StringBuilder sb = new StringBuilder();

					for (int i = 0; i < result.Files.Length; i++)
					{
						sb.AppendLine(result.Files[i].Name);
					}

					m_TextBox.Text = sb.ToString();

					m_EventsLog.LogEvent("Multiple files selected");
					break;

				case ENCommonDialogResult.Cancel:
					m_EventsLog.LogEvent("File not selected");
					break;

				case ENCommonDialogResult.Error:
					m_EventsLog.LogEvent("Error message: " + result.ErrorException.Message);
					break;
			}
		}

		private void ShowSaveFileDialog()
		{
			NSaveFileDialog saveFileDialog = new NSaveFileDialog();
			saveFileDialog.FileTypes = new NFileDialogFileType[]
			{
				new NFileDialogFileType("Text Files", "txt"),
				new NFileDialogFileType("All Files", "*")
			};
			saveFileDialog.SelectedFilterIndex = 0;
			saveFileDialog.DefaultFileName = "NevronTest";
			saveFileDialog.DefaultExtension = "txt";
			saveFileDialog.Title = "Save File As";

			saveFileDialog.Closed += new Function<NSaveFileDialogResult>(OnSaveFileDialogClosed);
			saveFileDialog.RequestShow();
		}
		private void OnSaveFileDialogClosed(NSaveFileDialogResult result)
		{
			switch (result.Result)
			{
				case ENCommonDialogResult.OK:
					result.File.Create().Then(
						delegate (Stream stream)
						{
							using (StreamWriter writer = new StreamWriter(stream))
							{
								writer.Write(m_TextBox.Text);
							}

							m_EventsLog.LogEvent("File saved: " + result.SafeFileName);
						}
					);

					break;

				case ENCommonDialogResult.Cancel:
					m_EventsLog.LogEvent("File not selected");
					break;

				case ENCommonDialogResult.Error:
					m_EventsLog.LogEvent("Error Message: " + result.ErrorException.Message);
					break;
			}
		}

		#endregion

		#region Fields

		private NTextBox m_TextBox;
		private NExampleEventsLog m_EventsLog;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NFileDialogsExample.
		/// </summary>
		public static readonly NSchema NFileDialogsExampleSchema;

		#endregion
	}
}