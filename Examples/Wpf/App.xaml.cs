using System;
using System.IO;
using System.Windows;

using Nevron.Nov.Barcode;
using Nevron.Nov.Chart;
using Nevron.Nov.Compiler;
using Nevron.Nov.Diagram;
using Nevron.Nov.Grid;
using Nevron.Nov.IO;
using Nevron.Nov.Schedule;
using Nevron.Nov.Text;
using Nevron.Nov.Windows.Wpf;

namespace Nevron.Nov.Examples.Wpf
{
	/// <summary>
	/// Interaction logic for App.xaml.
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			// TODO: Apply license for redistribution here. You can skip this code when evaluating NOV.
			// NLicenseManager.Instance.SetLicense(new NLicense("LICENSE KEY"));

			// Use this to Enable/Disable GPU rendering of all NOV Content
			NApplication.EnableGPURendering = true;

			// Install Nevron Open Vision for WPF
			NNovApplicationInstaller.Install(
				NBarcodeModule.Instance,
				NTextModule.Instance,
				NChartModule.Instance,
				NDiagramModule.Instance,
				NScheduleModule.Instance,
				NGridModule.Instance);

            // Optional: If you intend to use NCodeAssembly (for example for Family Tree Shapes in in NOV Diagram for .NET), 
            // you need to specify the compiler service used to compile them.
            NApplication.CompilerService = new NRoslynCompilerService();

#if DEBUG
            string resourcesPath = NPath.Current.Normalize(NPath.Current.Combine(NApplication.ResourcesFolder.Path, @"..\..\..\NOV\Resources"));
#else
			// Change the Resources folder to the one where the NOV installer places the resources, which is by default:
			// C:\Program Files (x86)\Nevron Software\Nevron Open Vision [Version]\Resources
			string resourcesPath = NPath.Current.Normalize(NPath.Current.Combine(NApplication.ResourcesFolder.Path, @"..\..\..\..\Resources"));
#endif

			NApplication.ResourcesFolder = NFileSystem.Current.GetFolder(resourcesPath);
			if (!Directory.Exists(resourcesPath))
			{
				Console.Write("Failed to locate resources path [" + resourcesPath + "]");
			}

			if (e.Args.Length == 1)
			{
				NDiagramModule.Instance.PredefinedLibrariesLoaded += (sender, args) =>
				{
					// Navigate to the URI after the predefined libraries of NOV Diagram has been loaded
					NExamplesContent examplesContent = ((NNovWidgetHost<NExamplesContent>)MainWindow.Content).Widget;
					examplesContent.NavigateToExampleUri(e.Args[0]);
				};
			}
		}
	}
}