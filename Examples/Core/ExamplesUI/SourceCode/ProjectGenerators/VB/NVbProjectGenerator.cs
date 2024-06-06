using Nevron.Nov.Text;

namespace Nevron.Nov.Examples
{
	internal abstract class NVbProjectGenerator : NProjectGenerator
	{
		#region Properties - Override

		/// <summary>
		/// Gets the source file extension.
		/// </summary>
		public override string SourceFileExtension
		{
			get
			{
				return "vb";
			}
		}

		protected override ENProgrammingLanguage Language
		{
			get
			{
				return ENProgrammingLanguage.VisualBasic;
			}
		}
		/// <summary>
		/// Gets the project file extension.
		/// </summary>
		protected override string ProjectFileExtension
		{
			get
			{
				return "vbproj";
			}
		}

		#endregion

		#region Protected Overrides

		protected override NSourceCodeParser CreateSourceCodeParser()
		{
			return new NVbSourceCodeParser();
		}

		#endregion

		#region Static Methods

		/// <summary>
		/// Creates a project generator for the current integration platform.
		/// </summary>
		/// <returns></returns>
		public static NVbProjectGenerator Create()
		{
			switch (NApplication.IntegrationPlatform)
			{
				case ENIntegrationPlatform.Unknown:
					break;
				case ENIntegrationPlatform.WinForms:
					return new NVbWinFormsProjectGenerator();
				case ENIntegrationPlatform.WPF:
					return new NVbWpfProjectGenerator();
				case ENIntegrationPlatform.Silverlight:
					break;
				case ENIntegrationPlatform.XamarinMac:
					return new NVbXamarinMacProjectGenerator();
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
