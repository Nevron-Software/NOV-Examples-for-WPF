using System.IO;

using Nevron.Nov.Dom;
using Nevron.Nov.Serialization;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples
{
	/// <summary>
	/// Stores examples options, recent examples and favorites.
	/// </summary>
	public class NExamplesOptions : NNode
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NExamplesOptions()
		{
			m_RecentExamples = new NDomArray<string>();
			m_FavoriteExamples = new NDomArray<string>();
			m_ThemeScheme = DefaultThemeScheme;
			m_DeveloperMode = DefaultDeveloperMode;
			m_bLoading = false;
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NExamplesOptions()
		{
			NExamplesOptionsSchema = NSchema.Create(typeof(NExamplesOptions), NNodeSchema);

			// Properties
			RecentExamplesProperty = NExamplesOptionsSchema.AddMember("RecentExamples", typeof(NDomArray<string>), null,
				delegate (NNode o) { return ((NExamplesOptions)o).m_RecentExamples; },
				delegate (NNode o, object v) { ((NExamplesOptions)o).m_RecentExamples = (NDomArray<string>)v; });
			FavoriteExamplesProperty = NExamplesOptionsSchema.AddMember("FavoriteExamples", typeof(NDomArray<string>), null,
				delegate (NNode o) { return ((NExamplesOptions)o).m_FavoriteExamples; },
				delegate (NNode o, object v) { ((NExamplesOptions)o).m_FavoriteExamples = (NDomArray<string>)v; });
			ThemeSchemeProperty = NExamplesOptionsSchema.AddMember("ThemeScheme", typeof(ENUIThemeScheme), DefaultThemeScheme,
				delegate(NNode o) { return ((NExamplesOptions) o).m_ThemeScheme; },
				delegate (NNode o, object v) { ((NExamplesOptions) o).m_ThemeScheme = (ENUIThemeScheme) v;});
			DeveloperModeProperty = NExamplesOptionsSchema.AddMember("DeveloperMode", NDomType.Boolean, DefaultDeveloperMode,
				delegate (NNode o) { return ((NExamplesOptions)o).m_DeveloperMode; },
				delegate (NNode o, object v) { ((NExamplesOptions)o).m_DeveloperMode = (bool)v; });

			// The single instance of the Examples options
			Instance = new NExamplesOptions();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets/Sets the value of the RecentExamples property.
		/// </summary>
		public NDomArray<string> RecentExamples
		{
			get
			{
				return m_RecentExamples;
			}
			set
			{
				SetValue(RecentExamplesProperty, value);
			}
		}
		/// <summary>
		/// Gets/Sets the value of the FavoriteExamples property.
		/// </summary>
		public NDomArray<string> FavoriteExamples
		{
			get
			{
				return m_FavoriteExamples;
			}
			set
			{
				SetValue(FavoriteExamplesProperty, value);
			}
		}
		/// <summary>
		/// Gets/Sets the scheme of the UI theme that should be applied to the NOV Examples application.
		/// </summary>
		public ENUIThemeScheme ThemeScheme
		{
			get
			{
				return m_ThemeScheme;
			}
			set
			{
				SetValue(ThemeSchemeProperty, value);
			}
		}
		/// <summary>
		/// Gets/Sets the value of the DeveloperMode property.
		/// </summary>
		public bool DeveloperMode
		{
			get
			{
				return m_DeveloperMode;
			}
			set
			{
				SetValue(DeveloperModeProperty, value);
			}
		}

		#endregion

		#region Public Methods - Serialization

		/// <summary>
		/// Saves the NOV Examples options.
		/// </summary>
		public void Save()
		{
			byte[] settingsBytes = SaveToBytes();
			NApplication.SetSetting(SettingsName, settingsBytes);
		}
		/// <summary>
		/// Loads the NOV Examples options.
		/// </summary>
		/// <returns></returns>
		public NPromise<NUndefined> Load()
		{
			return NApplication.GetSetting(SettingsName).ThenPromise(
				delegate (byte[] settingsBytes)
				{
					LoadFromBytes(settingsBytes);
					return NUndefined.Instance;
				}
			);
		}

		#endregion

		#region Public Methods - Recent and Favorite Examples

		public void AddRecentExample(string examplePath)
		{
			NDomArray<string> recentExamples = RecentExamples;

			// If the given example is already in the array, remove it first
			int index = recentExamples.IndexOf(examplePath);
			if (index != -1)
			{
				recentExamples = NDomArray<string>.RemoveAt(recentExamples, index);
			}

			// Add the given example to the array
			recentExamples = NDomArray<string>.Add(recentExamples, examplePath);

			// Make sure the array contains no more than ExampleListMaxCount items
			if (recentExamples.Count > ExampleListMaxCount)
			{
				recentExamples = NDomArray<string>.RemoveAt(recentExamples, 0);
			}

			// Update the RecentExamples property
			RecentExamples = recentExamples;
		}

		public void AddFavoriteExample(string examplePath)
		{
			// Add the given example to Favorites
			NDomArray<string> favoriteExamples = NDomArray<string>.Add(FavoriteExamples, examplePath);

			// Keep favorite examples sorted by path
			favoriteExamples.Sort();

			// Update the FavoriteExamples property
			FavoriteExamples = favoriteExamples;
		}
		public void RemoveFavoriteExample(string examplePath)
		{
			int index = FavoriteExamples.IndexOf(examplePath);
			if (index != -1)
			{
				FavoriteExamples = NDomArray<string>.RemoveAt(FavoriteExamples, index);
			}
		}

		#endregion

		#region Protected Overrides
		
		/// <summary>
		/// Called when the application options have changed. Overriden to save them to
		/// the application settings storage.
		/// </summary>
		/// <param name="data"></param>
		protected override void OnChanged(NChangeData data)
		{
			base.OnChanged(data);

			if (!m_bLoading && this == Instance)
			{
				Save();
			}
		}

		#endregion

		#region Implementation - Serialization

		private byte[] SaveToBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				SaveToStream(memoryStream);
				return memoryStream.ToArray();
			}
		}
		private void SaveToStream(Stream stream)
		{
			NDomNodeSerializer serializer = new NDomNodeSerializer();
			serializer.SaveToStream(new NNode[] { this }, stream, PersistencyFormat);
		}

		private void LoadFromBytes(byte[] bytes)
		{
			using (MemoryStream memoryStream = new MemoryStream(bytes))
			{
				LoadFromStream(memoryStream);
			}
		}
		private void LoadFromStream(Stream stream)
		{
			m_bLoading = true;

			// Load the example options from the given stream
			if (stream.Length == 0)
				return;

			NDomNodeDeserializer deserializer = new NDomNodeDeserializer();
			NNode[] nodes = deserializer.LoadFromStream(stream, PersistencyFormat);

			if (nodes.Length == 0)
				return;

			NExamplesOptions loadedOptions = (NExamplesOptions)nodes[0];

			// Copy the loaded options into the current ones
			DeepCopyCore(loadedOptions, new NDomDeepCopyContext());

			m_bLoading = false;
		}

		#endregion

		#region Fields

		private NDomArray<string> m_RecentExamples;
		private NDomArray<string> m_FavoriteExamples;
		private ENUIThemeScheme m_ThemeScheme;
		private bool m_DeveloperMode;

		private bool m_bLoading;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NExamplesOptions.
		/// </summary>
		public static readonly NSchema NExamplesOptionsSchema;
		/// <summary>
		/// Reference to the RecentExamples property.
		/// </summary>
		public static readonly NProperty RecentExamplesProperty;
		/// <summary>
		/// Reference to the FavoriteExamples property.
		/// </summary>
		public static readonly NProperty FavoriteExamplesProperty;
		/// <summary>
		/// Reference to the ThemeScheme property.
		/// </summary>
		public static readonly NProperty ThemeSchemeProperty;
		/// <summary>
		/// Reference to the DeveloperMode property.
		/// </summary>
		public static readonly NProperty DeveloperModeProperty;

		#endregion

		#region Constants

		private const int ExampleListMaxCount = 10;
		private const string SettingsName = "NovExamplesOptions";
		private const ENPersistencyFormat PersistencyFormat = ENPersistencyFormat.Binary;

		#endregion

		#region Instance

		/// <summary>
		/// Gets the single instance of NExamplesOptions.
		/// </summary>
		public static readonly NExamplesOptions Instance;

		#endregion

		#region Default Values

		private static readonly ENUIThemeScheme DefaultThemeScheme = NApplication.IntegrationPlatform == ENIntegrationPlatform.XamarinMac ?
			ENUIThemeScheme.MacElCapitan :
			ENUIThemeScheme.Windows10;
		private const bool DefaultDeveloperMode = false;

		#endregion
	}
}