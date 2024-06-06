using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

using Nevron.Nov.Graphics;
using Nevron.Nov.Windows;
using Nevron.Nov.Windows.Wpf;

namespace Nevron.Nov.Examples.Wpf
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

            Title = "Nevron Open Vision Examples for WPF";
            WindowState = WindowState.Maximized;

            // Load icon from stream
            using (Stream stream = GetType().Assembly.GetManifestResourceStream("Nevron.Nov.Examples.Wpf.Resources.NevronOpenVision.ico"))
            {
                // Decode the icon from the stream and set the first frame to the BitmapSource
                BitmapDecoder decoder = BitmapDecoder.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.None);
                BitmapSource source = decoder.Frames[1];

                // set image source
                Icon = source;
            }

            // Place a NOV UI Element that contains an NExampleContent widget
            NExamplesContent examplesContent = new NExamplesContent();
            examplesContent.LinkProcessor = new NWpfExampleLinkProcessor();
            Content = new NNovWidgetHost<NExamplesContent>(examplesContent);
		}
	}
}