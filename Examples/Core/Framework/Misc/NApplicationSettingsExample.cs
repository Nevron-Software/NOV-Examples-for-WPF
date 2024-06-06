using System;
using System.IO;

using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Layout;
using Nevron.Nov.Serialization;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Framework
{
	public class NApplicationSettingsExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NApplicationSettingsExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NApplicationSettingsExample()
		{
			NApplicationSettingsExampleSchema = NSchema.Create(typeof(NApplicationSettingsExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_AppSettings = new MyAppSettings();

			NEditor editor = NDesigner.GetDesigner(m_AppSettings).CreateInstanceEditor(m_AppSettings);

			NGroupBox groupBox = new NGroupBox("My Application Settings", editor);
			groupBox.HorizontalPlacement = ENHorizontalPlacement.Left;
			groupBox.VerticalPlacement = ENVerticalPlacement.Top;

			return groupBox;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			stack.HorizontalPlacement = ENHorizontalPlacement.Left;
			stack.VerticalPlacement = ENVerticalPlacement.Top;

			m_LoadSettingsButton = NButton.CreateImageAndText(Nevron.Nov.Presentation.NResources.Image_File_Open_png, "Load Settings");
			m_LoadSettingsButton.Enabled = false;
			m_LoadSettingsButton.Click += LoadSettingsButton_Click;
			stack.Add(m_LoadSettingsButton);

			NButton saveSettingsButton = NButton.CreateImageAndText(Nevron.Nov.Presentation.NResources.Image_File_Save_png, "Save Settings");
			saveSettingsButton.Click += SaveSettingsButton_Click;
			stack.Add(saveSettingsButton);

			// If the app settings key exists, enable the "Load Settings" button
			NApplication.GetAllSettingsNamesAsync().Then(
				delegate (string[] settingsName)
				{
					if (Array.IndexOf(settingsName, AppSettingsKey) != -1)
					{
						m_LoadSettingsButton.Enabled = true;
					}
				}
			);

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	Demonstrates how to work with the NOV application settings. Change the application settings (theme and developer mode) and
	then click the <b>Save Settings</b> button on the right. Then change the settings again and click the <b>Load Settings</b>
	button to load the saved settings. You can also switch to another example, then open this one again and click the
	<b>Load Settings</b> button to load the saved settings.
</p>";
		}

		#endregion

		#region Event Handlers

		private void LoadSettingsButton_Click(NEventArgs arg)
		{
			// Load the settings data
			NApplication.GetSettingAsync(AppSettingsKey).Then(
				delegate (byte[] settingsData)
				{
					using (MemoryStream stream = new MemoryStream(settingsData))
					{
						// Use a DOM node deserializer to load the app settings from the memory stream
						NDomNodeDeserializer deserializer = new NDomNodeDeserializer();
						MyAppSettings loadedSettings = (MyAppSettings)deserializer.LoadFromStream(stream, SettingsFormat)[0];

						// Copy the loaded app settings to the original app settings object
						m_AppSettings.DeepCopy(loadedSettings, new NDomDeepCopyContext());
					}
				},
				delegate (Exception ex)
				{
					NMessageBox.ShowError("Failed to load the app settings. Exception was: " + ex.Message, "Error");
				}
			);
		}
		private void SaveSettingsButton_Click(NEventArgs arg)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				// Use a DOM node serializer to save the app settings to a memory stream
				NDomNodeSerializer serializer = new NDomNodeSerializer();
				serializer.SaveToStream(m_AppSettings, stream, SettingsFormat);

				// Save the settings data
				byte[] settingsData = stream.ToArray();
				NApplication.SetSettingAsync(AppSettingsKey, settingsData).Then(
					delegate (NUndefined ud)
					{
						// Settings saved successfully, so enable the "Load Settings" button
						m_LoadSettingsButton.Enabled = true;
					}
				);
			}
		}

		#endregion

		#region Fields

		private MyAppSettings m_AppSettings;
		private NButton m_LoadSettingsButton;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NApplicationSettingsExample.
		/// </summary>
		public static readonly NSchema NApplicationSettingsExampleSchema;

		#endregion

		#region Constants

		private const string AppSettingsKey = "MyAppSettings";
		private static readonly ENPersistencyFormat SettingsFormat = ENPersistencyFormat.Binary;

		#endregion
	}

	#region Nested Types

	public class MyAppSettings : NNode
	{
		public MyAppSettings()
		{
		}
		static MyAppSettings()
		{
			MyAppSettingsSchema = NSchema.Create(typeof(MyAppSettings), NNodeSchema);
			ThemeProperty = MyAppSettingsSchema.AddSlot("Theme", typeof(ENUITheme), ENUITheme.Windows10);
			DeveloperModeProperty = MyAppSettingsSchema.AddSlot("DeveloperMode", NDomType.Boolean, false);
		}

		public ENUITheme Theme
		{
			get
			{
				return (ENUITheme)GetValue(ThemeProperty);
			}
			set
			{
				SetValue(ThemeProperty, value);
			}
		}
		public bool DeveloperMode
		{
			get
			{
				return (bool)GetValue(DeveloperModeProperty);
			}
			set
			{
				SetValue(DeveloperModeProperty, value);
			}
		}

		public static readonly NSchema MyAppSettingsSchema;
		public static readonly NProperty ThemeProperty;
		public static readonly NProperty DeveloperModeProperty;
	}

	#endregion
}