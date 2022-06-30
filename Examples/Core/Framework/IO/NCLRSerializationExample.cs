using System;
using System.IO;

using Nevron.Nov.Dom;
using Nevron.Nov.Layout;
using Nevron.Nov.Serialization;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Framework
{
	/// <summary>
	/// The example demonstrates how to serialize / deserialize .NET (CLR) objects
	/// </summary>
	public class NCLRSerializationExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public NCLRSerializationExample()
		{
		}
		/// <summary>
		/// 
		/// </summary>
		static NCLRSerializationExample()
		{
			NCLRSerializationExampleSchema = NSchema.Create(typeof(NCLRSerializationExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Overrides

		protected override NWidget CreateExampleContent()
		{
			NStackPanel stack = new NStackPanel();
			
			stack.HorizontalPlacement = ENHorizontalPlacement.Left;
			stack.VerticalPlacement = ENVerticalPlacement.Top;
			stack.MinWidth = 400;
			
			m_NameTextBox = new NTextBox();
			m_AddressTextBox = new NTextBox();
			m_MarriedCheckBox = new NCheckBox();
			m_GenderComboBox  = new NComboBox();
			m_GenderComboBox.Items.Add(new NComboBoxItem("Male"));
			m_GenderComboBox.Items.Add(new NComboBoxItem ("Female"));
			m_GenderComboBox.SelectedIndex = 0;

			m_OtherTextBox = new NTextBox();

			stack.Add(new NPairBox(new NLabel("Name (string):"), m_NameTextBox, true));
			stack.Add(new NPairBox(new NLabel("Address (string):"), m_AddressTextBox, true));
			stack.Add(new NPairBox(new NLabel("Married (boolean):"), m_MarriedCheckBox, true));
			stack.Add(new NPairBox(new NLabel("Gender (singleton):"), m_GenderComboBox, true));
			stack.Add(new NPairBox(new NLabel("Other (string, non serialized):"), m_OtherTextBox, true));

			return new NUniSizeBoxGroup(stack);
		}

		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			
			NButton saveStateButton = new NButton("Save");
			saveStateButton.Click += new Function<NEventArgs>(OnSaveStateButtonClick);
			stack.Add(saveStateButton);

			NButton loadStateButton = new NButton("Load");
			loadStateButton.Click += new Function<NEventArgs>(OnLoadStateButtonClick);
			stack.Add(loadStateButton);

			return stack;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override string GetExampleDescription()
		{
			return @"
<p>This example demonstrates how to use CLR serialization in order to serialize .NET objects.</p>
<p>Press the ""Save"" button to save the current state of the form.</p>
<p>Press the ""Load"" button to load a previously loaded form state.</p>
<p><b>Note:</b> The value of ""Other"" is not persisted, because this field is marked as non serialized.</p>
";
		}

		#endregion

		#region Event Handlers

		void OnSaveStateButtonClick(NEventArgs arg1)
		{
			try
			{
				m_MemoryStream = new MemoryStream();

				NSerializer serializer = new NSerializer();
				PersonInfo serializationObject = new PersonInfo(
					m_NameTextBox.Text,
					m_AddressTextBox.Text,
					m_MarriedCheckBox.Checked,
					m_GenderComboBox.SelectedIndex == 0 ? GenderSingleton.Male : GenderSingleton.Female,
					m_OtherTextBox.Text);
				serializer.SaveToStream(serializationObject, m_MemoryStream, ENPersistencyFormat.Binary);
			}
			catch (Exception ex)
			{
				NTrace.WriteLine(ex.Message);
			}
		}

		void OnLoadStateButtonClick(NEventArgs arg1)
		{
			if (m_MemoryStream == null)
				return;

			m_MemoryStream.Seek(0, SeekOrigin.Begin);

			NDeserializer deserializer = new NDeserializer();
			PersonInfo serializationObject = (PersonInfo)deserializer.LoadFromStream(m_MemoryStream, ENPersistencyFormat.Binary);

			m_NameTextBox.Text = serializationObject.Name;
			m_AddressTextBox.Text = serializationObject.Address;
			m_MarriedCheckBox.Checked = serializationObject.Married;
			m_GenderComboBox.SelectedIndex = serializationObject.Gender == GenderSingleton.Male ? 0 : 1;
			m_OtherTextBox.Text = serializationObject.Other;
		}

		#endregion

		#region Nested Types

		/// <summary>
		/// Represents a singleton object
		/// </summary>
		public class GenderSingleton
		{
			#region Constructors

			/// <summary>
			/// Initializer constructor
			/// </summary>
			/// <param name="genderName"></param>
			private GenderSingleton(string genderName)
			{
				m_GenderName = genderName;
			}

			#endregion

			#region Static Methods

			public static object GetSurrogateSerializer_NoObf(GenderSingleton singleton)
			{
				return new GenderSurrogate(singleton);
			}

			#endregion

			#region Fields

			string m_GenderName;

			#endregion

			#region Static Fields

			public static GenderSingleton Male = new GenderSingleton("Male");
			public static GenderSingleton Female = new GenderSingleton("Female");

			#endregion
		}
		/// <summary>
		/// Represents a class used to perform the actual serialization of the NGenderSingleton class
		/// </summary>
		public class GenderSurrogate : INSurrogateSerializer
		{
			#region Constructors

			/// <summary>
			/// Default constructor
			/// </summary>
			public GenderSurrogate()
			{

			}
			/// <summary>
			/// Initializer constructor
			/// </summary>
			/// <param name="instance"></param>
			public GenderSurrogate(GenderSingleton instance)
			{
				IsMale = instance == GenderSingleton.Male;
			}

			#endregion

			#region INSurrogateSerializer

			public object GetRealObject()
			{
				if (IsMale)
				{
					return GenderSingleton.Male;
				}
				else
				{
					return GenderSingleton.Female;
				}
			}

			public void ApplyToRealObject(object obj)
			{
				// not implemented
			}

			#endregion

			#region Fields

			public bool IsMale;

			#endregion
		}
		/// <summary>
		/// Represents a class showing some 
		/// </summary>
		public class PersonInfo
		{
			#region Constructors

			/// <summary>
			/// Default constructor
			/// </summary>
			public PersonInfo()
			{

			}
			/// <summary>
			/// Initializer constructor
			/// </summary>
			/// <param name="a"></param>
			/// <param name="b"></param>
			/// <param name="c"></param>
			public PersonInfo(string name, string address, bool married, GenderSingleton gender, string other)
			{
				Name = name;
				Address = address;
				Married = married;
				Gender = gender;
				Other = other;
			}

			#endregion

			#region Fields

			public string Name;
			public string Address;
			public bool Married;
			public GenderSingleton Gender;

			[NNonSerialized]
			public string Other;

			#endregion
		}

		#endregion

		#region Fields

		NTextBox m_NameTextBox;
		NTextBox m_AddressTextBox;
		NCheckBox m_MarriedCheckBox;
		NComboBox m_GenderComboBox;
		NTextBox m_OtherTextBox;

		MemoryStream m_MemoryStream;

		#endregion

		#region Static

		public static readonly NSchema NCLRSerializationExampleSchema;

		#endregion
	}
}
