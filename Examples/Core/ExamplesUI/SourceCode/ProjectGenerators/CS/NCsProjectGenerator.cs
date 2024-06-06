using Nevron.Nov.Text;

namespace Nevron.Nov.Examples
{
	/// <summary>
	/// Base class for Visual Studio C# project generators.
	/// </summary>
	internal abstract class NCsProjectGenerator : NProjectGenerator
	{
		#region Properties - Override

		/// <summary>
		/// Gets the source file extension.
		/// </summary>
		public override string SourceFileExtension
		{
			get
			{
				return "cs";
			}
		}

		protected override ENProgrammingLanguage Language
		{
			get
			{
				return ENProgrammingLanguage.CSharp;
			}
		}
		/// <summary>
		/// Gets the project file extension.
		/// </summary>
		protected override string ProjectFileExtension
		{
			get
			{
				return "csproj";
			}
		}

		#endregion

		#region Protected Overrides

		protected override NSourceCodeParser CreateSourceCodeParser()
		{
			return new NCsSourceCodeParser();
		}

		#endregion

		#region Static Methods

		/// <summary>
		/// Creates a project generator for the current integration platform.
		/// </summary>
		/// <returns></returns>
		public static NCsProjectGenerator Create()
		{
			switch (NApplication.IntegrationPlatform)
			{
				case ENIntegrationPlatform.Unknown:
					break;
				case ENIntegrationPlatform.WinForms:
					return new NCsWinFormsProjectGenerator();
				case ENIntegrationPlatform.WPF:
					return new NCsWpfProjectGenerator();
				case ENIntegrationPlatform.Silverlight:
					break;
				case ENIntegrationPlatform.XamarinMac:
					return new NCsXamarinMacProjectGenerator();
				case ENIntegrationPlatform.WebAssembly:
					break;
				default:
					break;
			}

			return null;
		}

		#endregion
	}
}