using System;
using System.IO;

using Nevron.Nov.Compression;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.IO;
using Nevron.Nov.Layout;
using Nevron.Nov.Text;
using Nevron.Nov.Text.Formats;
using Nevron.Nov.Threading;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples
{
	public abstract class NExampleBase : NContentHolder
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NExampleBase()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NExampleBase()
		{
			NExampleBaseSchema = NSchema.Create(typeof(NExampleBase), NContentHolder.NContentHolderSchema);

			// Get the examples source code archives
			CSharpSourceCodeArchiveStream = NResources.RBIN_SourceCode_CSharp_zip.Stream;
			VbSourceCodeArchiveStream = NResources.RBIN_SourceCode_VB_zip.Stream;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets/Sets the example title.
		/// </summary>
		public string Title
		{
			get
			{
				return m_Title;
			}
			set
			{
				m_Title = value;
			}
		}

		#endregion

		#region Public Methods - Initialize

		/// <summary>
		/// Creates the example content, controls and description.
		/// </summary>
		public void Initialize()
		{
			NWidget content = CreateExampleContent();
			NWidget controls = CreateExampleControls();

			NSplitter mainSplitter = new NSplitter();
			mainSplitter.Orientation = ENHVOrientation.Vertical;
			mainSplitter.SplitMode = ENSplitterSplitMode.OffsetFromFarSide;
			mainSplitter.SplitOffset = 150;

			// create the description group, which is the master in the first split
			mainSplitter.Pane2.Content = CreateExampleDescription();

			// create the example tab control
			NWidget exampleHolder;
			NTab exampleTab = new NTab();
			exampleTab.SelectedIndexChanged += OnExampleTabSelectedIndexChanged;

			m_ExampleTabPage = new NTabPage("Example", content);
			exampleTab.TabPages.Add(m_ExampleTabPage);

			m_CSharpSourceCodeHolder = new NContentHolder();
			NTabPage csharpTab = new NTabPage("C# Source", m_CSharpSourceCodeHolder);
			csharpTab.Tag = ENProgrammingLanguage.CSharp;
			exampleTab.TabPages.Add(csharpTab);

			m_VbSourceCodeHolder = new NContentHolder();
			NTabPage vbTab = new NTabPage("VB Source", m_VbSourceCodeHolder);
			vbTab.Tag = ENProgrammingLanguage.VisualBasic;
			exampleTab.TabPages.Add(vbTab);

			exampleTab.TabPages.Add(CreateExportSolutionTabPage());
			exampleHolder = exampleTab;

			if (controls != null)
			{
				// create the second split, which is the slave of the first slit
				NSplitter exampleSplitter = new NSplitter();
				exampleSplitter.SplitOffset = 300;
				exampleSplitter.SplitMode = ENSplitterSplitMode.OffsetFromFarSide;
				mainSplitter.Pane1.Content = exampleSplitter;

				// create the control group, which is the slave in the second split
				NGroupBox controlGroup = new NGroupBox("Controls", controls);
				exampleSplitter.Pane2.Content = controlGroup;

				// set the example tab control as the slave in the second split
				exampleSplitter.Pane1.Content = exampleHolder;
			}
			else
			{
				mainSplitter.Pane1.Content = exampleHolder;
			}

			Content = mainSplitter;
		}

		#endregion

		#region Protected Must Override

		protected abstract NWidget CreateExampleControls();
		protected abstract NWidget CreateExampleContent();
		/// <summary>
		/// Gets the description of this example in HTML format.
		/// </summary>
		/// <returns></returns>
		protected abstract string GetExampleDescription();

		#endregion

		#region Protected Overridable

		/// <summary>
		/// Gets a rich text view that shows the syntax highlighted source code of the example.
		/// </summary>
		/// <param name="language"></param>
		/// <returns></returns>
		protected virtual NWidget GetExampleSource(ENProgrammingLanguage language)
		{
			NRichTextView richText = new NRichTextView();
			richText.HRuler.Visibility = ENVisibility.Collapsed;
			richText.VRuler.Visibility = ENVisibility.Collapsed;
			richText.ReadOnly = true;

			try
			{
				// Decompress the source code of the example
				Stream decompressedStream = GetExampleSourceCodeStream(Schema, language);

				if (decompressedStream != null)
				{
					// Highlight the decompressed source code
					NSyntaxHighlighter syntaxHighlighter = NSyntaxHighlighter.Create(language);
					Stream htmlStream = syntaxHighlighter.Highlight(decompressedStream);

					// avoid showing up a paged rich text views until document loads
					richText.Content.Layout = ENTextLayout.Web;

					// Load the colorized source code in the source code rich text view
					richText.LoadFromStream(htmlStream, NTextFormat.Html);
				}
			}
			catch (Exception ex)
			{
				NTrace.WriteException("Failed to get example source.", ex);
			}

			return richText;
		}
		/// <summary>
		/// Creates the example description.
		/// </summary>
		/// <returns></returns>
		protected virtual NWidget CreateExampleDescription()
		{
			NRichTextView richTextView = new NRichTextView();
			richTextView.Padding = new NMargins(NDesign.HorizontalSpacing, NDesign.VerticalSpacing);

			richTextView.HRuler.Visibility = ENVisibility.Collapsed;
			richTextView.VRuler.Visibility = ENVisibility.Collapsed;
			richTextView.ReadOnly = true;

			byte[] descriptionData = NEncoding.UTF8.GetBytes(GetExampleDescription());
			MemoryStream stream = new MemoryStream(descriptionData);

			richTextView.Content.Layout = ENTextLayout.Normal;
			
			richTextView.LoadFromStream(stream, NTextFormat.Html).Finally(delegate () 
			{
				richTextView.Content.Padding = new NMargins(NDesign.HorizontalSpacing * 2, NDesign.VerticalSpacing);
				stream.Dispose(); 
			});

			NGroupBox groupBox = new NGroupBox("Description");
			groupBox.Content = richTextView;
			return groupBox;
		}
		/// <summary>
		/// Called when the user has switched to a new example and this example is about to be closed.
		/// </summary>
		protected internal virtual void OnClosing()
		{
		}

		#endregion

		#region Implementation

		/// <summary>
		/// Creates the example "Source" tab page.
		/// </summary>
		/// <returns></returns>
		private NTabPage CreateExportSolutionTabPage()
		{
			NMargins padding = new NMargins(NDesign.HorizontalSpacing * 2, NDesign.VerticalSpacing * 2);

			// Create the root stack
			NStackPanel stack = new NStackPanel();
			stack.Padding = padding;
			NStylePropertyEx.SetRelativeFontSize(stack, ENRelativeFontSize.Large);
			NStylePropertyEx.SetExtendedLook(stack, ENExtendedLook.Flat);

			// Add an info label to the root stack
			stack.Add(new NLabel("Click the buttons below to export a Visual Studio 2019 solution for this example."));

			// Create the Export Solution buttons
			NStackPanel buttonsStack = new NStackPanel();
			buttonsStack.HorizontalPlacement = ENHorizontalPlacement.Left;
			stack.Add(buttonsStack);

			NButton createCsWinFormsProjectButton = NButton.CreateImageAndText(NResources.Image_ExamplesUI_Icons_CsProject_png,
				"Create C# " + NApplication.IntegrationPlatform.ToString() + " Project");
			createCsWinFormsProjectButton.Click += OnCreateCsProjectButtonClick;
			buttonsStack.Add(createCsWinFormsProjectButton);

			NButton createVbWinFormsProjectButton = NButton.CreateImageAndText(NResources.Image_ExamplesUI_Icons_VbProject_png,
				"Create VB.NET " + NApplication.IntegrationPlatform.ToString() + " Project");
			createVbWinFormsProjectButton.Click += OnCreateVbProjectButtonClick;
			buttonsStack.Add(createVbWinFormsProjectButton);

			// Create and return the "Export Solution" tab page
			return new NTabPage("Export Solution", stack);
		}
		/// <summary>
		/// Shows a dialog for selecting a ZIP file, generates a project ZIP and saves it to the selected file.
		/// </summary>
		/// <param name="projectGenerator"></param>
		private void CreateProject(NProjectGenerator projectGenerator)
		{
			NSaveFileDialog saveDialog = new NSaveFileDialog();
			saveDialog.FileTypes = new NFileDialogFileType[] {
				new NFileDialogFileType(NLoc.Get("ZIP archive"), "zip")
			};

			string name = Schema.DomType.CLRType.Name;
			saveDialog.DefaultFileName = name + NStringHelpers.CapitalizeEachWord(projectGenerator.SourceFileExtension) + ".zip";

			saveDialog.Closed += delegate (NSaveFileDialogResult arg)
			{
				if (arg.Result == ENCommonDialogResult.OK)
				{
					// Write the generated ZIP archive to the selected file
					NThreadPool.StartTask(new NExportExampleTask(this, projectGenerator, arg.File));
				}
			};

			saveDialog.RequestShow();
		}
		private NContentHolder GetSourceCodeHolder(ENProgrammingLanguage language)
		{
			return language == ENProgrammingLanguage.CSharp ?
				m_CSharpSourceCodeHolder :
				m_VbSourceCodeHolder;
		}

		#endregion

		#region Event Handlers

		/// <summary>
		/// Occurs when the user opens a new tab.
		/// </summary>
		/// <param name="arg"></param>
		private void OnExampleTabSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			NTab tab = (NTab)arg.TargetNode;
			if (tab.SelectedPage.Tag is ENProgrammingLanguage)
			{
				ENProgrammingLanguage language = (ENProgrammingLanguage)tab.SelectedPage.Tag;

				NContentHolder holder = GetSourceCodeHolder(language);
				if (holder.Content == null)
				{
					holder.Content = GetExampleSource(language);
				}
			}
		}
		private void OnCreateCsProjectButtonClick(NEventArgs arg)
		{
			NCsProjectGenerator projectGenerator = NCsProjectGenerator.Create();
			CreateProject(projectGenerator);
		}
		private void OnCreateVbProjectButtonClick(NEventArgs arg)
		{
			NVbProjectGenerator projectGenerator = NVbProjectGenerator.Create();
			CreateProject(projectGenerator);
		}

		#endregion

		#region Fields

		protected NTabPage m_ExampleTabPage;

		private string m_Title;
		private NContentHolder m_CSharpSourceCodeHolder;
		private NContentHolder m_VbSourceCodeHolder;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NExampleBase.
		/// </summary>
		public static readonly NSchema NExampleBaseSchema;

		#endregion

		#region Static Methods

		internal static Stream GetExampleSourceCodeStream(NSchema exampleSchema, ENProgrammingLanguage language)
		{
			Stream sourceCodeStream = GetSourceCodeArchiveStream(language);
			sourceCodeStream.Position = 0;

			Type exampleType = exampleSchema.DomType.CLRType;
			NSourceCodeDecompressor decompressor = new NSourceCodeDecompressor(exampleType, language);
			NCompression.DecompressZip(sourceCodeStream, decompressor);
			return decompressor.GetSourceCodeStream();
		}
		private static Stream GetSourceCodeArchiveStream(ENProgrammingLanguage language)
		{
			return language == ENProgrammingLanguage.CSharp ?
				CSharpSourceCodeArchiveStream :
				VbSourceCodeArchiveStream;
		}

		#endregion

		#region Constants

		private static readonly Stream CSharpSourceCodeArchiveStream;
		private static readonly Stream VbSourceCodeArchiveStream;

		#endregion

		#region Nested Types

		/// <summary>
		/// A task that exports an example to a Visual Studio solution.
		/// </summary>
		private class NExportExampleTask : INThreadedTask
		{
			public NExportExampleTask(NExampleBase example, NProjectGenerator projectGenerator, NFile file)
			{
				m_Example = example;
				m_ProjectGenerator = projectGenerator;
				m_File = file;
			}

			public void DoThreadedTask()
			{
				NApplication.BeginInvoke(new Function(OpenProgressWindow));

				byte[] zipData;
				try
				{
					zipData = m_ProjectGenerator.Generate(m_Example);
				}
				catch (Exception ex)
				{
					CloseProgressWindow(ex);
					return;
				}

				m_File.WriteAllBytes(zipData).Then(
					delegate (NUndefined ud)
					{
						NApplication.BeginInvoke(
							delegate()
							{
								CloseProgressWindow();
								NMessageBox.ShowInformation(m_Example.DisplayWindow, "Example solution exported successfully.", "Solution Export Succeeded");
							}
						);
					},
					delegate (Exception ex)
					{
						CloseProgressWindow(ex);
					}
				);
			}

			private void OpenProgressWindow()
			{
				string name = m_Example.Schema.DomType.CLRType.Name;
				m_ProgressWindow = NProgressWindow.Create(m_Example.DisplayWindow, "Exporting Example");
				m_ProgressWindow.Modal = true;
				m_ProgressWindow.Content = new NLabel("Exporting example \"" + name + "\"");
				m_ProgressWindow.ProgressBar.Mode = ENProgressBarMode.Indeterminate;
				m_ProgressWindow.Open();
			}
			private void CloseProgressWindow()
			{
				m_ProgressWindow.Close();
			}
			private void CloseProgressWindow(Exception error)
			{
				NApplication.BeginInvoke(
					delegate()
					{
						CloseProgressWindow();
						NMessageBox.ShowError(
							m_Example.DisplayWindow, 
							"Example solution export failed with the following error:" + Environment.NewLine + error.Message, 
							"Solution Export Failed");
					}
				);
			}

			private NExampleBase m_Example;
			private NProjectGenerator m_ProjectGenerator;
			private NFile m_File;
			private NProgressWindow m_ProgressWindow;
		}

		#endregion
	}
}