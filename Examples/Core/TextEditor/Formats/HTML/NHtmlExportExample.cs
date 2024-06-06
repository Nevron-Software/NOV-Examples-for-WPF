using System;
using System.IO;

using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.IO;
using Nevron.Nov.Layout;
using Nevron.Nov.Text;
using Nevron.Nov.Text.Formats;
using Nevron.Nov.Text.Formats.Html;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Text
{
	public class NHtmlExportExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NHtmlExportExample()
        {
        }
        /// <summary>
        /// Static constructor.
        /// </summary>
        static NHtmlExportExample()
        {
            NHtmlExportExampleSchema = NSchema.Create(typeof(NHtmlExportExample), NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            NSplitter splitter = new NSplitter();
            splitter.SplitMode = ENSplitterSplitMode.Proportional;
            splitter.SplitFactor = 0.5;

            // Create the rich text view
            NRichTextViewWithRibbon richTextWithRibbon = new NRichTextViewWithRibbon();
            m_RichText = richTextWithRibbon.View;
            m_RichText.AcceptsTab = true;
            m_RichText.Content.Sections.Clear();

            // Stack the rich text with ribbon and an export button
            NButton exportButton = new NButton("Export");
            exportButton.Content.HorizontalPlacement = ENHorizontalPlacement.Center;
            exportButton.Click += OnExportButtonClick;

            splitter.Pane1.Content = CreatePairBox(richTextWithRibbon, exportButton);

            // Create the HTML rich text box
			m_HtmlTextBox = new NTextBox();
			m_HtmlTextBox.AcceptsEnter = true;
			m_HtmlTextBox.AcceptsTab = true;
			m_HtmlTextBox.Multiline = true;
			m_HtmlTextBox.WordWrap = false;
			m_HtmlTextBox.VScrollMode = ENScrollMode.WhenNeeded;
			m_HtmlTextBox.HScrollMode = ENScrollMode.WhenNeeded;
			m_HtmlTextBox.ReadOnly = true;
			splitter.Pane2.Content = new NGroupBox("Exported HTML", m_HtmlTextBox);

            return splitter;
        }
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();
            stack.VerticalSpacing = 10;

			// Create the export settings check boxes
			m_InlineStylesCheckBox = new NCheckBox("Inline styles", false);
			m_InlineStylesCheckBox.CheckedChanged += OnSettingsCheckBoxCheckedChanged;
			stack.Add(m_InlineStylesCheckBox);

			m_MinifyHtmlCheckBox = new NCheckBox("Minify HTML", false);
			m_MinifyHtmlCheckBox.CheckedChanged += OnSettingsCheckBoxCheckedChanged;
			stack.Add(m_MinifyHtmlCheckBox);

            // Create the predefined tests list box
            NListBox testListBox = new NListBox();
            testListBox.Items.Add(CreateTestListBoxItem(new NRichTextBorders()));
            testListBox.Items.Add(CreateTestListBoxItem(new NRichTextLists()));
            testListBox.Items.Add(CreateTestListBoxItem(new NRichTextTables()));
            testListBox.Items.Add(CreateTestListBoxItem(new NRichTextTextStyles()));
			testListBox.Items.Add(CreateTestListBoxItem(new NRichTextElementPositioning()));
            testListBox.Selection.Selected += OnTestListBoxItemSelected;

            // Add the list box in a group box
            stack.Add(new NGroupBox("Predefined text documents", testListBox));

            // Create the Load from file group box
            NDockPanel dockPanel = new NDockPanel();
            dockPanel.HorizontalSpacing = 3;
            dockPanel.VerticalSpacing = 3;

            NButton loadButton = new NButton("Load");
            loadButton.Click += OnLoadButtonClick;
            NDockLayout.SetDockArea(loadButton, ENDockArea.Bottom);
            dockPanel.Add(loadButton);

            m_FileNameTextBox = new NTextBox();
            m_FileNameTextBox.VerticalPlacement = ENVerticalPlacement.Center;
            NDockLayout.SetDockArea(m_FileNameTextBox, ENDockArea.Center);
            dockPanel.Add(m_FileNameTextBox);

            NButton browseButton = new NButton("...");
            browseButton.Click += OnBrowseButtonClick;
            NDockLayout.SetDockArea(browseButton, ENDockArea.Right);
            dockPanel.Add(browseButton);

            stack.Add(new NGroupBox("Load from file", dockPanel));

            m_ElapsedTimeLabel = new NLabel();
            stack.Add(m_ElapsedTimeLabel);

            // Select the initial test
            testListBox.Selection.SingleSelect(testListBox.Items[0]);

            return stack;
        }
		protected override string GetExampleDescription()
        {
            return @"
<p>
	This example demonstrates the HTML export capabilities of the Nevron Rich Text widget. Simply select one of the preloaded examples
    from the combo box to the right and see it exported. You can also edit the text document and press the <b>Export</b> button when ready.
</p>
";
        }

        #endregion

        #region Implementation

        private void ExportToHtml()
        {
            m_ElapsedTimeLabel.Text = null;

            NStopwatch stopwatch = NStopwatch.StartNew();
            using (MemoryStream stream = new MemoryStream())
            {
				// Create and configure HTML save settings
				NHtmlSaveSettings saveSettings = new NHtmlSaveSettings();
				saveSettings.InlineStyles = m_InlineStylesCheckBox.Checked;
				saveSettings.MinifyHtml = m_MinifyHtmlCheckBox.Checked;

				// Save to HTML
                m_RichText.SaveToStreamAsync(stream, NTextFormat.Html, saveSettings);
                stopwatch.Stop();

                LoadHtmlSource(stream);
            }

            m_ElapsedTimeLabel.Text = "Export done in: " + stopwatch.ElapsedMilliseconds.ToString() + " ms.";
        }
        private void LoadHtmlSource(Stream stream)
        {
			stream.Position = 0;
			byte[] bytes = NStreamHelpers.ReadToEnd(stream);
			m_HtmlTextBox.Text = NEncoding.UTF8.GetString(bytes);
        }
        private bool LoadRtfFromFile(string fileName)
        {
            try
            {
                m_RichText.LoadFromFileAsync(fileName);
            }
            catch
            {
                m_ElapsedTimeLabel.Text = "RTF loading failed.";
                return false;
            }

            return true;
        }

        #endregion

        #region Event Handlers

        private void OnExportButtonClick(NEventArgs arg1)
        {
            ExportToHtml();
        }
		private void OnSettingsCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			ExportToHtml();
		}
		private void OnTestListBoxItemSelected(NSelectEventArgs<NListBoxItem> arg1)
        {
            NListBoxItem selectedItem = arg1.Item;
            if (selectedItem == null)
                return;

            NRichTextToHtmlExample example = selectedItem.Tag as NRichTextToHtmlExample;
            if (example == null)
                return;

            // Recreate the content of the Nevron rich text widget
			NDocumentBlock documentRoot = example.CreateDocument();
			
			NRichTextDocument document = new NRichTextDocument();
			document.Content = documentRoot;
			document.Evaluate();

			m_RichText.Document = document;
            ExportToHtml();
        }
        private void OnBrowseButtonClick(NEventArgs arg1)
        {
            NOpenFileDialog openFileDialog = new NOpenFileDialog();
            openFileDialog.FileTypes = new NFileDialogFileType[] { new NFileDialogFileType("Word Documents and Rich Text Files",
				new string[] { "docx", "rtf" }) };
            openFileDialog.SelectedFilterIndex = 0;
            openFileDialog.MultiSelect = false;
            openFileDialog.InitialDirectory = String.Empty;

            openFileDialog.Closed += new Function<NOpenFileDialogResult>(
                delegate(NOpenFileDialogResult result)
                {
                    if (result.Result != ENCommonDialogResult.OK)
                        return;

                    string fileName = result.Files[0].Path;
                    m_FileNameTextBox.Text = fileName;

                    if (LoadRtfFromFile(fileName))
                    {
                        ExportToHtml();
                    }
                }
            );

            openFileDialog.RequestShow();
        }
        private void OnLoadButtonClick(NEventArgs arg1)
        {
            string fileName = m_FileNameTextBox.Text;
            NFile file = NFileSystem.Current.GetFile(fileName);

            if (file == null)
                return;
            
            file.Exists().Then(delegate (bool exists)
            { 
                if(exists && LoadRtfFromFile(fileName))
                    ExportToHtml();
            });                        
        }

        #endregion

        #region Fields

        private NRichTextView m_RichText;
        private NTextBox m_HtmlTextBox;
		private NCheckBox m_InlineStylesCheckBox;
		private NCheckBox m_MinifyHtmlCheckBox;
        private NTextBox m_FileNameTextBox;
        private NLabel m_ElapsedTimeLabel;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NHtmlExportExample.
        /// </summary>
        public static readonly NSchema NHtmlExportExampleSchema;

        #endregion

        #region Static Methods

        private static NListBoxItem CreateTestListBoxItem(NRichTextToHtmlExample example)
        {
            NListBoxItem item = new NListBoxItem(example.Title);
            item.Tag = example;
            return item;
        }
        private static NPairBox CreatePairBox(NWidget widget1, NWidget widget2)
        {
            NPairBox pairBox = new NPairBox(widget1, widget2, ENPairBoxRelation.Box1AboveBox2);
            pairBox.FitMode = ENStackFitMode.First;
            pairBox.FillMode = ENStackFillMode.First;
			pairBox.Spacing = NDesign.VerticalSpacing;

            return pairBox;
        }

        #endregion

        #region Nested Types

        private abstract class NRichTextToHtmlExample
        {
            public NRichTextToHtmlExample(string title)
            {
                m_Title = title;
            }

            public string Title
            {
                get
                {
                    return m_Title;
                }
            }

			public virtual NDocumentBlock CreateDocument()
            {
				NDocumentBlock document = new NDocumentBlock();
                document.Information.Title = m_Title;

                NSection section = new NSection();
                document.Sections.Add(section);

                NParagraph heading = new NParagraph(m_Title);
                section.Blocks.Add(heading);
                heading.HorizontalAlignment = ENAlign.Center;
				heading.FontSize = 24;

                return document;
            }

            private string m_Title;
        }

        private class NRichTextBorders : NRichTextToHtmlExample
        {
            public NRichTextBorders()
                : base("Borders")
            {
            }

			public override NDocumentBlock CreateDocument()
            {
				NDocumentBlock document = base.CreateDocument();

                NSection section = document.Sections[0];
                NParagraph p = new NParagraph("Black solid border");
                section.Blocks.Add(p);
                p.Border = NBorder.CreateFilledBorder(NColor.Black);
                p.BorderThickness = new NMargins(1);

                p = new NParagraph("Black dashed border");
                section.Blocks.Add(p);
                p.Border = new NBorder();
                p.Border.MiddleStroke = new NStroke(5, NColor.Black, ENDashStyle.Dash);
                p.BorderThickness = new NMargins(5);

                p = new NParagraph("Green/DarkGreen two-color border");
                section.Blocks.Add(p);
                p.Border = NBorder.CreateTwoColorBorder(NColor.Green, NColor.DarkGreen);
                p.BorderThickness = new NMargins(10);

                p = new NParagraph("A border with left, right and bottom sides and wide but not set top side");
                section.Blocks.Add(p);
                p.Border = new NBorder();
                p.Border.LeftSide = new NThreeColorsBorderSide(NColor.Black, NColor.Gray, NColor.LightGray);
                p.Border.RightSide = new NBorderSide();
                p.Border.RightSide.OuterStroke = new NStroke(10, NColor.Blue, ENDashStyle.Dot);
                p.Border.BottomSide = new NBorderSide(NColor.Red);
                p.BorderThickness = new NMargins(9, 50, 5, 5);

                return document;
            }
        }
        private class NRichTextTextStyles : NRichTextToHtmlExample
        {
            public NRichTextTextStyles()
                : base("Text Styles")
            {
            }

			public override NDocumentBlock CreateDocument()
            {
				NDocumentBlock document = base.CreateDocument();

                NSection section = document.Sections[0];
                section.Blocks.Add(new NParagraph("This is the first paragraph."));
                section.Blocks.Add(new NParagraph("This is the second paragraph.\nThis is part of the second paragraph, too."));

				NGroupBlock div = new NGroupBlock();
                section.Blocks.Add(div);
                div.Fill = new NColorFill(NColor.Red);
                NParagraph p = new NParagraph("This is a paragraph in a div. It should have red underlined text.");
                div.Blocks.Add(p);
                p.FontStyle = ENFontStyle.Underline;

                p = new NParagraph("This is another paragraph in the div. It contains a ");
                div.Blocks.Add(p);
                NTextInline inline = new NTextInline("bold italic blue inline");
                p.Inlines.Add(inline);
                inline.Fill = new NColorFill(NColor.Blue);
                inline.FontStyle = ENFontStyle.Bold | ENFontStyle.Italic;

                p.Inlines.Add(new NTextInline("."));

                return document;
            }
        }
        private class NRichTextTables : NRichTextToHtmlExample
        {
            public NRichTextTables()
                : base("Tables")
            {
            }

			public override NDocumentBlock CreateDocument()
            {
				NDocumentBlock document = base.CreateDocument();

                // Create a simple 2x2 table
                NSection section = document.Sections[0];
                NTable table = new NTable(2, 2);
                section.Blocks.Add(table);

                for (int row = 0, i = 1; row < table.Rows.Count; row++)
                {
                    for (int col = 0; col < table.Columns.Count; col++, i++)
                    {
                        InitCell(table.Rows[row].Cells[col], "Cell " + i.ToString());
                    }
                }

                // Create a 3x3 table with rowspans and colspans
                table = new NTable(4, 3);
                section.Blocks.Add(table);
                InitCell(table.Rows[0].Cells[0], 2, 1, "Cell 1 (2 rows)");
                InitCell(table.Rows[0].Cells[1], "Cell 2");
                InitCell(table.Rows[0].Cells[2], "Cell 3");
                InitCell(table.Rows[1].Cells[1], 1, 2, "Cell 4 (2 cols)");
                InitCell(table.Rows[2].Cells[0], "Cell 5");
                InitCell(table.Rows[2].Cells[1], 2, 2, "Cell 6 (2 rows x 2 cols)");
                InitCell(table.Rows[3].Cells[0], "Cell 7");

                return document;
            }

            private static void InitCell(NTableCell cell, string text)
            {
                InitCell(cell, 1, 1, text);
            }
            private static void InitCell(NTableCell cell, int rowSpan, int colSpan, string text)
            {
                if (rowSpan != 1)
                {
                    cell.RowSpan = rowSpan;
                }

                if (colSpan != 1)
                {
                    cell.ColSpan = colSpan;
                }

                // By default cells contain a single paragraph
                cell.Blocks.Clear();
                cell.Blocks.Add(new NParagraph(text));

                // Create a border
                cell.Border = NBorder.CreateFilledBorder(NColor.Black);
                cell.BorderThickness = new NMargins(1);
            }
        }
        private class NRichTextLists : NRichTextToHtmlExample
        {
            public NRichTextLists()
                : base("Lists")
            {
            }

			public override NDocumentBlock CreateDocument()
            {
				NDocumentBlock document = base.CreateDocument();
                NSection section = document.Sections[0];

                // Add bullet lists of all unordered types
                ENBulletListTemplateType[] bulletTypes = new ENBulletListTemplateType[] { ENBulletListTemplateType.Bullet };

                for (int i = 0; i < bulletTypes.Length; i++)
                {
                    NBulletList bulletList = new NBulletList(ENBulletListTemplateType.Bullet);
					document.BulletLists.Add(bulletList);

					for (int j = 1; j <= 3; j++)
                    {
						NParagraph paragraph = new NParagraph("This is parargaph number " + j.ToString() +
                            ". This paragraph is contained in a bullet list of type " + bulletTypes[i].ToString());
						paragraph.SetBulletList(bulletList, 0);
						section.Blocks.Add(paragraph);
                    }
                }

                // Add bullet lists of all ordered types
                bulletTypes = new ENBulletListTemplateType[] { ENBulletListTemplateType.Decimal, ENBulletListTemplateType.LowerAlpha,
                    ENBulletListTemplateType.LowerRoman, ENBulletListTemplateType.UpperAlpha, ENBulletListTemplateType.UpperRoman };

                for (int i = 0; i < bulletTypes.Length; i++)
                {
                    section.Blocks.Add(new NParagraph());

                    NBulletList bulletList = new NBulletList(bulletTypes[i]);

                    for (int j = 1; j <= 3; j++)
                    {
                        NParagraph paragraph = new NParagraph("Bullet List Item " + j.ToString(), bulletList, 0);
                        section.Blocks.Add(paragraph);

                        for (int z = 1; z <= 3; z++)
                        {
                            NParagraph par2 = new NParagraph("Bullet List Sub Item " + z.ToString(), bulletList, 1);
                            section.Blocks.Add(par2);
                        }
                    }
                }

                return document;
            }
        }
		private class NRichTextElementPositioning : NRichTextToHtmlExample
		{
			public NRichTextElementPositioning()
				: base("Element Positioning")
			{
			}

			public override NDocumentBlock CreateDocument()
			{
				NDocumentBlock document = base.CreateDocument();

				NSection section = document.Sections[0];
				NParagraph p = new NParagraph("This is a red paragraph on the left.");
				p.HorizontalAnchor = ENHorizontalAnchor.Ancestor;
				p.HorizontalBlockAlignment = ENHorizontalBlockAlignment.Left;
                p.VerticalAnchor = ENVerticalAnchor.Ancestor;
				p.XOffset = 20;
				p.YOffset = 200;
				p.PreferredWidth = NMultiLength.NewPercentage(25);
				p.BackgroundFill = new NColorFill(NColor.Red);
				section.Blocks.Add(p);

				p = new NParagraph("This is a green paragraph on the top.");
                p.HorizontalAnchor = ENHorizontalAnchor.Ancestor;
                p.VerticalAnchor = ENVerticalAnchor.Ancestor;
				p.VerticalBlockAlignment = ENVerticalBlockAlignment.Top;
				p.XOffset = 120;
				p.YOffset = 100;
				p.PreferredWidth = NMultiLength.NewPercentage(50);
				p.BackgroundFill = new NColorFill(NColor.Green);
				section.Blocks.Add(p);

				p = new NParagraph("This is a blue paragraph on the right.");
                p.HorizontalAnchor = ENHorizontalAnchor.Ancestor;
				p.HorizontalBlockAlignment = ENHorizontalBlockAlignment.Right;
                p.VerticalAnchor = ENVerticalAnchor.Ancestor;
				p.XOffset = 20;
				p.YOffset = 200;
				p.PreferredWidth = NMultiLength.NewPercentage(25);
				p.BackgroundFill = new NColorFill(NColor.Blue);
				p.Fill = new NColorFill(NColor.White);
				section.Blocks.Add(p);

				return document;
			}
		}

        #endregion
    }
}