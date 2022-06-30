

using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;
using System;
using System.Text;
using Nevron.Nov.TrueType;

namespace Nevron.Nov.Examples.Framework
{
	/// <summary>
	/// The example demonstrates how to paint text at location
	/// </summary>
	public class NInstallingFontsExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NInstallingFontsExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NInstallingFontsExample()
		{
			NInstallingFontsExampleSchema = NSchema.Create(typeof(NInstallingFontsExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NOTResourceInstalledFont font = NResources.Font_LiberationMonoBold_ttf;
			NOTFontDescriptor descriptor = font.InstalledFonts[0].Descriptor;
			m_FontDescriptor = new NFontDescriptor(descriptor.m_FamilyName, descriptor.m_FontVariant);
			NApplication.FontService.InstalledFontsMap.InstallFont(font);

			// Create a table panel to hold the canvases and the labels
			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.Last;
			stack.FitMode = ENStackFitMode.Last;

			NCanvas canvas = new NCanvas();
			stack.Add(canvas);

			canvas.PrePaint += new Function<NCanvasPaintEventArgs>(OnCanvasPrePaint);
			canvas.BackgroundFill = new NColorFill(NColor.White);

			return stack;
		}
		protected override NWidget CreateExampleControls()
		{
			return null;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
The example demonstrates how to install fonts.
</p>
";
		}		

		#endregion

		#region Event Handlers

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		private void OnCanvasPrePaint(NCanvasPaintEventArgs args)
		{
			NCanvas canvas = args.TargetNode as NCanvas;
			if (canvas == null)
				return;

			NPaintVisitor paintVisitor = args.PaintVisitor;

			NRectangle contentEge = canvas.GetContentEdge();

			// create the text bounds
			double width = contentEge.Width * 0.5;
			double height = contentEge.Height * 0.5;
			NPoint center = contentEge.Center;

			NRectangle textBounds = new NRectangle(center.X - width / 2.0, center.Y - height / 2.0, width, height);

			// create the settings
			NPaintTextRectSettings settings = new NPaintTextRectSettings();
			settings.SingleLine = false;
			settings.WrapMode = ENTextWrapMode.WordWrap;
			settings.HorzAlign = ENTextHorzAlign.Center;
			settings.VertAlign = ENTextVertAlign.Center;

			// create the text
			StringBuilder builder = new StringBuilder();
			builder.AppendLine("This text is displayed using Liberation Fonts!");
			builder.AppendLine("distributed under the SIL Open Font License (OFL)");

			// paint the bounding box
			paintVisitor.ClearStyles();
			paintVisitor.SetFill(NColor.LightBlue);
			paintVisitor.PaintRectangle(textBounds);

			// init font and fill
			paintVisitor.SetFill(NColor.Black);
			ENFontStyle fontStyle =  NFontFaceDescriptor.FontVariantToFontStyle(m_FontDescriptor.FontVariant);
			paintVisitor.SetFont(new NFont(m_FontDescriptor.FamilyName, 10, fontStyle));

			// paint the text
			paintVisitor.PaintString(textBounds, builder.ToString(), ref settings);
		}

		#endregion

		#region Fonts

		NFontDescriptor m_FontDescriptor;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NInstallingFontsExample.
		/// </summary>
		public static readonly NSchema NInstallingFontsExampleSchema;

		#endregion
	}
}