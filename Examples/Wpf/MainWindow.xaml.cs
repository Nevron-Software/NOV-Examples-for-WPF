using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

using Nevron.Nov.Windows;

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
            Content = new NNovWidgetHost<NExamplesContent>();
        }
    }
}