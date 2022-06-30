using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using Nevron.Nov;
using Nevron.Nov.Barcode;
using Nevron.Nov.Chart;
using Nevron.Nov.Diagram;
using Nevron.Nov.Grid;
using Nevron.Nov.IO;
using Nevron.Nov.Layout;
using Nevron.Nov.Schedule;
using Nevron.Nov.Text;
using Nevron.Nov.UI;
using Nevron.Nov.Windows;

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

			// use this to Enable/Disable GPU rendering of all NOV Content
			NApplication.EnableGPURendering = true;

			// Install Nevron Open Vision for WPF
			NNovApplicationInstaller.Install(
				NBarcodeModule.Instance,
				NTextModule.Instance,
				NChartModule.Instance,
				NDiagramModule.Instance,
				NScheduleModule.Instance,
				NGridModule.Instance);

// #if !DEBUG
			// Change the Resources folder to the one where the NOV installer places the resources, which is by default:
			// C:\Program Files (x86)\Nevron Software\Nevron Open Vision 2021.1\Resources
			string resourcesPath = NPath.Current.Normalize(NPath.Current.Combine(NApplication.ResourcesFolder.Path, @"..\..\..\..\..\Resources"));
			NApplication.ResourcesFolder = NFileSystem.Current.GetFolder(resourcesPath);

			if (!Directory.Exists(resourcesPath))
			{
				Console.Write("Failed to locate resources path [" + resourcesPath + "]");
			}
// #endif
		}
	}
}