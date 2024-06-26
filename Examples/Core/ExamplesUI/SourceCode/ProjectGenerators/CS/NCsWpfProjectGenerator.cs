﻿namespace Nevron.Nov.Examples
{
	internal class NCsWpfProjectGenerator : NCsProjectGenerator
	{
		#region Property Overrides

		/// <summary>
		/// Gets an embedded resource that contains the zipped project template.
		/// </summary>
		protected override NEmbeddedResource ProjectTemplate
		{
			get
			{
				return IsNet50orNewer ?
					NResources.RBIN_ProjectTemplates_CS_Net70_NovWpfProject_zip :
					NResources.RBIN_ProjectTemplates_CS_Net472_NovWpfProject_zip;
			}
		}
		/// <summary>
		/// Gets the name of the main source code file - the main window of the WPF application.
		/// </summary>
		protected override string MainSourceCodeFileName
		{
			get
			{
				return "NovWpfProject/MainWindow.xaml.cs";
			}
		}

		#endregion
	}
}