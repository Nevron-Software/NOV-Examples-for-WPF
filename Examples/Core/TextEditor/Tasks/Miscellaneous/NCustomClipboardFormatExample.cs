using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Serialization;
using Nevron.Nov.Text;
using Nevron.Nov.Text.Formats;
using Nevron.Nov.UI;
using System;
using System.IO;

namespace Nevron.Nov.Examples.Text
{
    /// <summary>
    /// The example demonstrates how to create a custom clipboard format that allows the user to selectively copy/paste text, images or both.
    /// </summary>
    public class NCustomClipboardFormatExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public NCustomClipboardFormatExample()
		{
		}
		/// <summary>
		/// 
		/// </summary>
		static NCustomClipboardFormatExample()
		{
			NCustomClipboardFormatExampleSchema = NSchema.Create(typeof(NCustomClipboardFormatExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create the rich text
			m_RichText = new NRichTextView();
			m_RichText.AcceptsTab = true;
            
            m_RichText.Content.Sections.Clear();
            m_RichText.Selection.ClipboardTextFormats = new NClipboardTextFormat[] { new NCustomClipboardFormat(this) };

            NSection section = new NSection();
            m_RichText.Content.Sections.Add(section);

            section.Blocks.Add(new NParagraph("The example demonstrates how to implement a custom clipboard format."));
			section.Blocks.Add(new NParagraph("This example demonstrates a scenario where the user can selectively copy/paste just text or images."));

			for (int i = 0; i < 3; i++)
			{
				NParagraph paragraph = new NParagraph("This paragraph contains text and");
				section.Blocks.Add(paragraph);

				paragraph.Inlines.Add(new NLineBreakInline());

				NImageInline imageInline = new NImageInline();
				imageInline.Image = NResources.Image_Artistic_FishBowl_jpg;
				imageInline.PreferredWidth = new NMultiLength(ENMultiLengthUnit.Dip, 250);
				imageInline.PreferredHeight = new NMultiLength(ENMultiLengthUnit.Dip, 200);
				paragraph.Inlines.Add(imageInline);

				paragraph.Inlines.Add(new NLineBreakInline());

				paragraph.Inlines.Add(new NTextInline("image inline content."));
			}

			return m_RichText;
		}

        protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();

			m_ContentTypeComboBox = new NComboBox();
			m_ContentTypeComboBox.Items.Add(new NComboBoxItem("Text"));
			m_ContentTypeComboBox.Items.Add(new NComboBoxItem("Image"));
			m_ContentTypeComboBox.Items.Add(new NComboBoxItem("Text and Image"));
            m_ContentTypeComboBox.SelectedIndex = 0;

            stack.Add(NPairBox.Create("Custom Clipboard Content: ", m_ContentTypeComboBox));

			return stack;
		}

        protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates a scenario where the user can selectively copy/paste text, images, or both. The purpose of the example is to demonstrate how to implement a custom clipboard format.</p>";
		}

        #endregion

        #region Custom Clipboard Example Code

        /// <summary>
        /// Represents a custom clipboard format
        /// </summary>
        public class NCustomClipboardFormat : NClipboardTextFormat
        {
            #region Constructors

            /// <summary>
            /// Initializer constructor
            /// </summary>
            /// <param name="example"></param>
            public NCustomClipboardFormat(NCustomClipboardFormatExample example)
            {
                m_Example = example;
            }
            /// <summary>
            /// Static constructor
            /// </summary>
            static NCustomClipboardFormat()
            {
                // create the Data Format associated with MyFirstDataEchangeObject
                s_DataFormat = NDataFormat.Create("CustomClipboardFormat",
                      new FunctionResult<byte[], NDataFormat, object>(SerializeDataObject),
                      new FunctionResult<object, NDataFormat, byte[]>(DeserializeDataObject));
            }
            #endregion

            #region Overrides

            /// <summary>
            /// Imports a document from a data object
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override NRichTextDocument FromDataObject(object obj)
            {
                return (NRichTextDocument)obj;
            }
            /// <summary>
            /// Exports the specified document to the specified data object
            /// </summary>
            /// <param name="document"></param>
            /// <param name="dataObject"></param>
            /// <returns></returns>
            public override void ToDataObject(NRichTextDocument document, NDataObject dataObject)
            {
                // create a clone of the document so that we can modify it
                document = (NRichTextDocument)document.DeepClone();

                // TODO Implement your own document filtering
                switch (m_Example.m_ContentTypeComboBox.SelectedIndex)
                {
                    case 0: // text
                        {
                            // remove all images from the document
                            NList<NNode> inlines = document.GetDescendants(NImageInline.NImageInlineSchema);

                            for (int i = 0; i < inlines.Count; i++)
                            {
                                NImageInline imageInline = (NImageInline)inlines[i];

                                NParagraph par = (NParagraph)imageInline.ParentBlock;

                                par.Inlines.Remove(imageInline);
                            }
                        }
                        break;
                    case 1: // image
                        {
                            // remove all text inlines from the document
                            NList<NNode> inlines = document.GetDescendants(NTextInline.NTextInlineSchema);

                            for (int i = 0; i < inlines.Count; i++)
                            {
                                NTextInline textInline = (NTextInline)inlines[i];

                                NParagraph par = (NParagraph)textInline.ParentBlock;

                                par.Inlines.Remove(textInline);
                            }
                        }

                            break;
                    case 2: // image and text
                        // do nothing
                        break;
                }

                dataObject.SetData(s_DataFormat, document);
            }
            /// <summary>
            /// The underling text format
            /// </summary>
            public override NTextFormat TextFormat
            {
                get
                {
                    return null;
                }
            }
            /// <summary>
            /// The underling text format
            /// </summary>
            public override NDataFormat DataFormat
            {
                get
                {
                    return s_DataFormat;
                }
            }

            #endregion

            #region DataFormat Implementation

            /// <summary>
            /// Serialization function for the data format
            /// </summary>
            /// <param name="format"></param>
            /// <param name="obj"></param>
            /// <returns></returns>
            private static byte[] SerializeDataObject(NDataFormat format, object obj)
            {
                NRichTextDocument document = (NRichTextDocument)obj;
                MemoryStream stream = new MemoryStream(10240);

                NDomNodeSerializer serializer = new NDomNodeSerializer();

                serializer.SaveToStream(new NNode[] { document }, stream, ENPersistencyFormat.Xml);

                return stream.ToArray();
            }
            /// <summary>
            /// Deserialization function for the custom data format
            /// </summary>
            /// <param name="format"></param>
            /// <param name="bytes"></param>
            /// <returns></returns>
            private static object DeserializeDataObject(NDataFormat format, byte[] bytes)
            {
                MemoryStream stream = new MemoryStream(bytes);

                NDomNodeDeserializer serializer = new NDomNodeDeserializer();
                NRichTextDocument myObject = (NRichTextDocument)serializer.LoadFromStream(stream, ENPersistencyFormat.Xml)[0];
                return myObject;
            }

            #endregion

            #region Fields

            NCustomClipboardFormatExample m_Example;

            #endregion

            #region Static Fields

            internal static NDataFormat s_DataFormat;

            #endregion
        }
    
        #endregion

        #region Fields

        private NRichTextView m_RichText;
		private NComboBox m_ContentTypeComboBox;

		#endregion

		#region Schema

		public static readonly NSchema NCustomClipboardFormatExampleSchema;

		#endregion
	}
}