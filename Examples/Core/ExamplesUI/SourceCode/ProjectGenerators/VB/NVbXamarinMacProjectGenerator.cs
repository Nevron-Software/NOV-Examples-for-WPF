using System;
using System.IO;
using System.Reflection;

using Nevron.Nov.Compression;
using Nevron.Nov.DataStructures;

namespace Nevron.Nov.Examples
{
	internal class NVbXamarinMacProjectGenerator : NVbProjectGenerator
	{
		#region Constructors

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NVbXamarinMacProjectGenerator()
		{
			PlatformAssemblies = new NAssemblyInfo[PlatformAssemblyNames.Length];

			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			for (int j = 0; j < PlatformAssemblyNames.Length; j++)
			{
				string platformAssemblyName = PlatformAssemblyNames[j];

				for (int i = 0; i < assemblies.Length; i++)
				{
					Assembly assembly = assemblies[i];
					string assemblyName = assembly.GetName().Name;

					if (assemblyName.IndexOf(platformAssemblyName, StringComparison.OrdinalIgnoreCase) != -1)
					{
						// This is a platform specific assembly
						PlatformAssemblies[j] = new NAssemblyInfo(assembly);
						break;
					}
				}
			}
		}

		#endregion

		#region Property Overrides

		/// <summary>
		/// Gets an embedded resource that contains the zipped project template.
		/// </summary>
		protected override NEmbeddedResource ProjectTemplate
		{
			get
			{
				return null;
				//return NResources.RBIN_ProjectTemplates_VB_NovXamarinMacProject_zip;
			}
		}
		/// <summary>
		/// Gets the name of the main source code file - the main window application.
		/// </summary>
		protected override string MainSourceCodeFileName
		{
			get
			{
				return "NovXamarinMacProject/MainWindow.vb";
			}
		}

		#endregion

		#region Protected Overrides - Assemblies

		protected override void AddPlatformAssemblies(NList<NZipItem> items, NExampleBase example)
		{
			base.AddPlatformAssemblies(items, example);

			string exampleName = GetExampleName(example);
			for (int i = 0; i < PlatformAssemblies.Length; i++)
			{
				string assemblyName = PlatformAssemblyNames[i];
				string path = exampleName + "/References/" + assemblyName + ".dll";

				if (PlatformAssemblies[i] != null)
				{
					items.Add(new NZipItem(path, new MemoryStream(PlatformAssemblies[i].Data)));
				}
			}
		}
		/// <summary>
		/// Indicates whether the given reference hint path should be updated.
		/// </summary>
		/// <param name="referenceName">The reference name.</param>
		/// <param name="oldHintPath">The old hint path.</param>
		/// <param name="newHintPath">The new hint path.</param>
		/// <returns>True if the reference hint path should be updated.</returns>
		protected override bool ShouldUpdateHintPath(string referenceName, string oldHintPath, out string newHintPath)
		{
			if (base.ShouldUpdateHintPath(referenceName, oldHintPath, out newHintPath))
				return true;

			if (Array.IndexOf(PlatformAssemblyNames, referenceName) != -1)
			{
				newHintPath = @"References\" + referenceName + ".dll";
				return true;
			}
			else
			{
				return false;
			}
		}

		#endregion

		#region Constants

		private static readonly string[] PlatformAssemblyNames = new string[] {
			"Microsoft.CodeAnalysis.CSharp",
			"Microsoft.CodeAnalysis",
			"System.Collections.Immutable",
			"System.Memory",
			"System.Numerics.Vectors",
			"System.Reflection.Metadata",
			"System.Runtime.CompilerServices.Unsafe",
			"System.Threading.Tasks.Extensions"
		};

		private static readonly NAssemblyInfo[] PlatformAssemblies;

		#endregion
	}
}