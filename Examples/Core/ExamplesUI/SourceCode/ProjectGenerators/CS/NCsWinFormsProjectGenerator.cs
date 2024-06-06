namespace Nevron.Nov.Examples
{
	internal class NCsWinFormsProjectGenerator : NCsProjectGenerator
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
					NResources.RBIN_ProjectTemplates_CS_Net70_NovWinFormsProject_zip :
					NResources.RBIN_ProjectTemplates_CS_Net472_NovWinFormsProject_zip;
			}
		}
		/// <summary>
		/// Gets the name of the main source code file - the main form of the WinForms application.
		/// </summary>
		protected override string MainSourceCodeFileName
		{
			get
			{
				return "NovWinFormsProject/Form1.cs";
			}
		}

		#endregion
	}
}