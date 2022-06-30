using System.Windows;
using Nevron.Nov.Windows;

namespace Nevron.Nov.Examples.Wpf
{
	/// <summary>
	/// Interaction logic for NMainWindow.xaml
	/// </summary>
	public partial class NMainWindow : Window
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public NMainWindow()
		{
			InitializeComponent();
			Title = "Nevron Open Vision Examples for WPF";
			WindowState = WindowState.Maximized;

            // place a NOV UI Element that contains an NExampleContent widget
            Content = new NNOVWidgetHost<NExamplesContent>();
		}
	}
}