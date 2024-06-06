using System;
using System.IO;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Examples.Text;
using Nevron.Nov.Serialization;
using Nevron.Nov.Text;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Framework
{
	class NTestNode : NNode
	{
		public NTestNode()
		{
		}

		static NTestNode()
		{
			NTestNodeSchema = NSchema.Create(typeof(NTestNode), NNode.NNodeSchema);

			ExtendedPropertyEx = NProperty.CreateExtended(NTestNodeSchema, "Extended", NDomType.Boolean, true);
		}

		public static readonly NProperty ExtendedPropertyEx;
		public static NSchema NTestNodeSchema;
	}
	/// <summary>
	/// The example demonstrates how to modify the table borders, spacing etc.
	/// </summary>
	public class NDomSerializationExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public NDomSerializationExample()
		{
		}
		/// <summary>
		/// 
		/// </summary>
		static NDomSerializationExample()
		{
			NDomSerializationExampleSchema = NSchema.Create(typeof(NDomSerializationExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create the rich text
			NRichTextViewWithRibbon richTextWithRibbon = new NRichTextViewWithRibbon();
			m_RichText = richTextWithRibbon.View;
			m_RichText.AcceptsTab = true;
			m_RichText.Content.Sections.Clear();

			// Populate the rich text
			PopulateRichText();

			return richTextWithRibbon;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			
			NButton saveStateButton = new NButton("Save");
			saveStateButton.Click += new Function<NEventArgs>(OnSaveStateButtonClick);
			stack.Add(saveStateButton);

			m_LoadStateButton = new NButton("Load");
			m_LoadStateButton.Enabled = false;
			m_LoadStateButton.Click += new Function<NEventArgs>(OnLoadStateButtonClick);
			stack.Add(m_LoadStateButton);

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>This example demonstrates how to use DOM serialization in order to serialize / deserialize NOV NNode derived objects.</p>
<p>Press the Save button on the right to save the contents of the document and load to restore the contents to the last saved one.</p>";
		}

		private void PopulateRichText()
		{
			NSection section = new NSection();
			m_RichText.Content.Sections.Add(section);

			section.Blocks.Add(new NParagraph("Type some text here..."));
		}

		#endregion

		#region Event Handlers

		private void OnSaveStateButtonClick(NEventArgs arg1)
		{
			try
			{
				m_MemoryStream = new MemoryStream();

				NDomNodeSerializer serializer = new NDomNodeSerializer();

				NTestNode testNode = new NTestNode();

				testNode.SetValue(NTestNode.ExtendedPropertyEx, false);
				serializer.SaveToStream(new NNode[] { testNode }, m_MemoryStream, ENPersistencyFormat.Binary);

//				serializer.SaveToStream(new NNode[] { m_RichText.Content }, m_MemoryStream, ENPersistencyFormat.Binary);

				m_LoadStateButton.Enabled = true;
			}
			catch (Exception ex)
			{
				NDebug.WriteLine(ex.Message);
			}
		}
		private void OnLoadStateButtonClick(NEventArgs arg1)
		{
			if (m_MemoryStream == null)
				return;

			m_MemoryStream.Seek(0, SeekOrigin.Begin);

			try
			{
				NDomNodeDeserializer deserializer = new NDomNodeDeserializer();

				NTestNode root = (NTestNode)deserializer.LoadFromStream(m_MemoryStream, ENPersistencyFormat.Binary)[0];
/*				NDocumentBlock root = (NDocumentBlock)deserializer.LoadFromStream(m_MemoryStream, ENPersistencyFormat.Binary)[0];

				if (root != null)
				{
					m_RichText.Document = new NRichTextDocument(root);
				}*/
			}
			catch (Exception ex)
			{
				NDebug.WriteLine(ex.Message);
			}
		}
		private void OnLoadDocumentButtonClick(NEventArgs args)
		{
			
		}

		#endregion

		#region Fields

		private NRichTextView m_RichText;
		private NButton m_LoadStateButton;
		private MemoryStream m_MemoryStream;

		#endregion

		#region Schema

		public static readonly NSchema NDomSerializationExampleSchema;

		#endregion
	}
}