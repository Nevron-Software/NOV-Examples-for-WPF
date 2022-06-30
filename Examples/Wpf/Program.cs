using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

using Nevron.Nov.Barcode;
using Nevron.Nov.Chart;
using Nevron.Nov.Diagram;
using Nevron.Nov.Grid;
using Nevron.Nov.Schedule;
using Nevron.Nov.Text;
using Nevron.Nov.Windows;

namespace Nevron.Nov.Examples.Wpf
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			try
			{
				// install Nevron Open Vision for WPF
				NNovApplicationInstaller.Install(
					NTextModule.Instance,
					NChartModule.Instance,
					NDiagramModule.Instance,
					NScheduleModule.Instance,
					NGridModule.Instance,
                    NBarcodeModule.Instance);

                // show the main window
                Window window = new Window();
                window.Title = "Nevron Open Vision Examples for WPF";
                window.WindowState = WindowState.Maximized;

                // load icon from stream
                using (Stream stream = typeof(Program).Assembly.GetManifestResourceStream("Nevron.Nov.Examples.Wpf.Resources.NevronOpenVision.ico"))
                {
                    // Decode the icon from the stream and set the first frame to the BitmapSource
                    BitmapDecoder decoder = IconBitmapDecoder.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.None);
                    BitmapSource source = decoder.Frames[1];

                    // set image source
                    window.Icon = source;
                }

                // place a NOV UI Element that contains an NExampleContent widget
                window.Content = new NNovWidgetHost<NExamplesContent>();

                // show the window
                Application app = new Application();
				app.Run(window);
			}
			catch (Exception ex)
			{
				NTrace.WriteException("Exception in Main", ex);
			}
		}
	}
}