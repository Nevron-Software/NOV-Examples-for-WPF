using System;
using System.IO;

using Nevron.Nov.Compression;
using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.IO;
using Nevron.Nov.Layout;
using Nevron.Nov.Text;
using Nevron.Nov.Text.Formats;
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

            // Get the examples source code archive
            SourceCodeStream = NResources.Instance.GetResourceStream("RBIN_SourceCode_zip");
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

		#region Public Overridable

		/// <summary>
        /// Creates the example content, controls and description.
        /// </summary>
        public virtual void Initialize()
        {
			m_SourceCodeHolder = new NContentHolder();
			NWidget content = CreateExampleContent();
			NWidget controls = CreateExampleControls();

			NSplitter mainSplitter = new NSplitter();
			mainSplitter.Orientation = ENHVOrientation.Vertical;
			mainSplitter.SplitMode = ENSplitterSplitMode.OffsetFromFarSide;
			mainSplitter.SplitOffset = 150;

			// create the description group, which is the master in the first split
			mainSplitter.Pane2.Content = CreateExampleDescription();

			// create the example tab control
			NTab exampleTab = new NTab();
			exampleTab.SelectedIndexChanged += OnExampleTabSelectedIndexChanged;
			m_ExampleTabPage = new NTabPage("Example", content);
			exampleTab.TabPages.Add(m_ExampleTabPage);
			exampleTab.TabPages.Add(new NTabPage("Source", m_SourceCodeHolder));

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
				exampleSplitter.Pane1.Content = exampleTab;
			}
			else
			{
				mainSplitter.Pane1.Content = exampleTab;
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
        /// Gets a rich text box that show the syntax highlighted source code of the example.
        /// </summary>
        /// <returns></returns>
        protected virtual NWidget GetExampleSource()
        {
            NRichTextView richText = new NRichTextView();
            richText.HRuler.Visibility = ENVisibility.Collapsed;
            richText.VRuler.Visibility = ENVisibility.Collapsed;
            richText.ReadOnly = true;

            try
            {
				// Decompress the source code of the example
				Type exampleType = Schema.DomType.Type;
                NSourceCodeDecompressor decompressor = new NSourceCodeDecompressor(exampleType);
                SourceCodeStream.Position = 0;
                NCompression.DecompressZip(SourceCodeStream, decompressor);

                // Highlight the decompressed source code
                NSyntaxHighlighter syntaxHighlighter = new NSyntaxHighlighter();

				MemoryStream decompressedStream = decompressor.GetSourceCodeStream();
				if (decompressedStream != null)
				{
					Stream htmlStream = syntaxHighlighter.Highlight(decompressedStream);

					// Load the colorized source code in the source code rich text view
					richText.LoadFromStream(htmlStream, new NHtmlTextFormat());
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
			using (MemoryStream stream = new MemoryStream(descriptionData))
			{
				richTextView.LoadFromStream(stream, new NHtmlTextFormat());
			}

			richTextView.Content.Padding = new NMargins(NDesign.HorizontalSpacing * 2, NDesign.VerticalSpacing);

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

        #region Event Handlers

        /// <summary>
        /// Occurs when the user clicks on the Source tab. Loads the source code of the example, if not already loaded.
        /// </summary>
        /// <param name="arg"></param>
        private void OnExampleTabSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            if (m_SourceCode != null)
                return;

            m_SourceCode = GetExampleSource();
            m_SourceCodeHolder.Content = m_SourceCode;
        }

        #endregion

        #region Fields

		protected NTabPage m_ExampleTabPage;

		private string m_Title;
        private NWidget m_SourceCode;
        private NContentHolder m_SourceCodeHolder;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NExampleBase.
        /// </summary>
        public static readonly NSchema NExampleBaseSchema;

        #endregion

        #region Constants

        private static readonly Stream SourceCodeStream;

        #endregion

        #region Nested Types

        private class NSourceCodeDecompressor : INZipDecompressor
        {
            public NSourceCodeDecompressor(Type exampleType)
            {
				m_ExampleNamespace = exampleType.Namespace;
                m_ExampleFileName = exampleType.Name + ".cs";
				m_Items = new NList<NZipItem>();
            }

            public bool Filter(NZipItem item)
            {
				string fileName = NPath.GetFileName(item.Name);
				return String.Equals(fileName, m_ExampleFileName, StringComparison.OrdinalIgnoreCase);
            }
            public void OnItemDecompressed(NZipItem item)
            {
				m_Items.Add(item);
            }

			/// <summary>
			/// Gets the source code stream.
			/// </summary>
			/// <returns></returns>
			public MemoryStream GetSourceCodeStream()
			{
				if (m_Items.Count == 0)
					return null;
				else if (m_Items.Count == 1)
					return (MemoryStream)m_Items[0].Stream;

				// Analyze the example names and returns the best matching one
				NZipItem bestItem = null;
				int bestScore = -1;
				string[] tokens = m_ExampleNamespace.Split('.');

				for (int i = 0; i < m_Items.Count; i++)
				{
					NZipItem item = m_Items[i];

					// Find how many tokens from the namespace are present in the current file name
					string dirName = NPath.GetFullDirectoryName(m_Items[i].Name);
					int score = 0;

					for (int j = 0; j < tokens.Length; j++)
					{
						if (dirName.Contains(tokens[j]))
						{
							score++;
						}
					}

					if (score > bestScore)
					{
						// The current item is a better match, so store it
						bestItem = item;
						bestScore = score;
					}
				}

				return (MemoryStream)bestItem.Stream;
			}

			private string m_ExampleNamespace;
            private string m_ExampleFileName;
			private NList<NZipItem> m_Items;
        }

        #endregion
    }
}