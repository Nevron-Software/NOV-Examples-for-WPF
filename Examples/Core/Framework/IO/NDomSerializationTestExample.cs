using System;
using System.IO;
using System.Text;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Examples.Text;
using Nevron.Nov.Serialization;
using Nevron.Nov.Text;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Framework
{
	/// <summary>
	/// The example automatically tests all all node types for serialization.
	/// </summary>
	public class NDomSerializationTestExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public NDomSerializationTestExample()
		{
		}
		/// <summary>
		/// 
		/// </summary>
		static NDomSerializationTestExample()
		{
			NDomSerializationTestExampleSchema = NSchema.Create(typeof(NDomSerializationTestExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_TextBox = new NTextBox();
			m_TextBox.Multiline = true;
			m_TextBox.VScrollMode = ENScrollMode.WhenNeeded;

			return m_TextBox;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			
			NButton testBinarySerializationButton = new NButton("Test Binary Serialization");
			testBinarySerializationButton.Tag = ENPersistencyFormat.Binary;
			testBinarySerializationButton.MouseDown += OnTestSerializationButtonMouseDown;
			testBinarySerializationButton.Click += OnTestSerializationButtonClick;
			stack.Add(testBinarySerializationButton);

			NButton testXmlSerializationButton = new NButton("Test XML Serialization");
			testXmlSerializationButton.Tag = ENPersistencyFormat.Xml;
			testXmlSerializationButton.MouseDown += OnTestSerializationButtonMouseDown;
			testXmlSerializationButton.Click += OnTestSerializationButtonClick;
			stack.Add(testXmlSerializationButton);

			NButton clearLogButton = new NButton("Clear Log");
			clearLogButton.Click += new Function<NEventArgs>(OnClearLogButtonClick);
			stack.Add(clearLogButton);

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>The example performs automatic serialization test to all NNode derived objects in the Nevron.Nov.Presentation assembly.</p>";
		}

		#endregion

		#region Event Handlers

		private void OnTestSerializationButtonMouseDown(NMouseButtonEventArgs arg)
		{
			// Set Wait cursor
			m_TextBox.DisplayWindow.Cursor = new NCursor(ENPredefinedCursor.Wait);
		}
		private void OnTestSerializationButtonClick(NEventArgs arg)
		{
			ENPersistencyFormat persistencyFormat = (ENPersistencyFormat)arg.TargetNode.Tag;
			NStopwatch stopwatch;
			try
			{
				Type nodeType = typeof(NNode);
				Type[] types = nodeType.Assembly.GetTypes();
				int nodeCount = 0, successfullySerialized = 0;

				stopwatch = NStopwatch.StartNew();
				StringBuilder output = new StringBuilder();
				for (int i = 0, count = types.Length; i < count; i++)
				{
					Type type = types[i];

					// not a NNode type, abstract or generic => skip
					if (!nodeType.IsAssignableFrom(type) || type.IsAbstract || type.IsGenericType)
						continue;

					NNode node;
					try
					{
						nodeCount++;
						NNode typeInstance = (NNode)Activator.CreateInstance(type);

						// Serialize
						MemoryStream memoryStream = new MemoryStream();
						NDomNodeSerializer serializer = new NDomNodeSerializer();
						serializer.SerializeDefaultValues = true;
						serializer.SaveToStream(new NNode[] { typeInstance }, memoryStream, persistencyFormat);

						// Deserialize to check if the serialization has succeeded
						NDomNodeDeserializer deserializer = new NDomNodeDeserializer();
						memoryStream = new MemoryStream(memoryStream.ToArray());
						node = deserializer.LoadFromStream(memoryStream, persistencyFormat)[0];

						output.AppendLine("Sucessfully serialized node type [" + type.Name + "].");
						successfullySerialized++;
					}
					catch (Exception ex)
					{
						output.AppendLine("Failed to serialize node type [" + type.Name + "]. Exception was [" + ex.Message + "]");
					}
				}

				stopwatch.Stop();

				output.AppendLine("==================================================");
				output.AppendLine("Nodes serialized: " + successfullySerialized.ToString() + " of " + nodeCount.ToString());
				output.AppendLine("Time elapsed: " + stopwatch.ElapsedMilliseconds.ToString() + " ms");

				m_TextBox.Text = output.ToString();
				m_TextBox.SetCaretPos(new NTextPosition(m_TextBox.Text.Length, false));
				m_TextBox.EnsureCaretVisible();
			}
			catch (Exception ex)
			{
				NTrace.WriteLine(ex.Message);
			}

			// Restore the default cursor
			this.DisplayWindow.Cursor = null;
		}
		private void OnClearLogButtonClick(NEventArgs arg)
		{
			m_TextBox.Text = String.Empty;
		}

		#endregion

		#region Fields

		private NTextBox m_TextBox;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NDomSerializationTestExample.
		/// </summary>
		public static readonly NSchema NDomSerializationTestExampleSchema;

		#endregion
	}
}