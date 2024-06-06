using System.Globalization;

using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NImageBoxDpiScalingExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NImageBoxDpiScalingExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NImageBoxDpiScalingExample()
		{
			NImageBoxDpiScalingExampleSchema = NSchema.Create(typeof(NImageBoxDpiScalingExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NStackPanel stack = new NStackPanel();
			stack.Direction = ENHVDirection.LeftToRight;
			stack.HorizontalPlacement = ENHorizontalPlacement.Center;
			stack.VerticalPlacement = ENVerticalPlacement.Center;

			NImage printIcon = NResources.Image_Print_ico;
			NDpiAwareImageSource iconImageSource = new NDpiAwareImageSource(
				printIcon.ImageSource, // Windows icon (ICO file)
				new NSizeI(32, 32) // Default size - i.e. desired image size at 96 DPI (100% screen scaling)
			);
			stack.Add(CreatePairBox("DPI Aware Image", new NImageBox(iconImageSource)));

			// Create a raster from the image at index 3 in the print icon, which has a size of 32x32 pixels
			NRaster raster = printIcon.ImageSource.CreateFrameRaster(3);
			stack.Add(CreatePairBox("DPI Unaware Image", new NImageBox(NImage.FromRaster(raster))));

			return new NUniSizeBoxGroup(stack);
		}
		protected override NWidget CreateExampleControls()
		{
			m_EventsLog = new NExampleEventsLog();
			return m_EventsLog;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates the features of the Nevron icon box widget. This is a widget that can show a different image based
	on the current DPI scaling. Change the screen scaling in your OS to see how the image box dynamically loads an image of different
	size. Compare that image to the 16x16 pixel image in the image box and you will see that the image in the icon box is not blurred
	like the one in the image box.
</p>
";
		}

		#endregion

		#region Event Handlers

		protected override void OnRegistered()
		{
			base.OnRegistered();

			m_EventsLog.LogEvent("Current DPI: " + GetResolution().ToString("N0", CultureInfo.InvariantCulture));
		}

        protected override void OnResolutionChanged()
		{
			base.OnResolutionChanged();

			m_EventsLog.LogEvent("DPI Changed to " + GetResolution().ToString("N0", CultureInfo.InvariantCulture));
		}

		#endregion

		#region Fields

		private NExampleEventsLog m_EventsLog = new NExampleEventsLog();

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NImageBoxDpiScalingExample.
		/// </summary>
		public static readonly NSchema NImageBoxDpiScalingExampleSchema;

		#endregion

		#region Static Methods

		private static NPairBox CreatePairBox(string text, NWidget widget)
		{
			NPairBox pairBox = new NPairBox(text, widget, ENPairBoxRelation.Box1AboveBox2, true);
			pairBox.Border = NBorder.CreateFilledBorder(NColor.DarkGray);
			pairBox.BorderThickness = new NMargins(1);
			pairBox.Padding = new NMargins(NDesign.HorizontalSpacing / 2, NDesign.VerticalSpacing / 2);

			return pairBox;
		}

		#endregion
	}
}