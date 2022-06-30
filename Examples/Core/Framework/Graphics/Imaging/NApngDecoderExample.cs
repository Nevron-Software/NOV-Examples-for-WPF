using System;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Framework
{
	public class NApngDecoderExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NApngDecoderExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NApngDecoderExample()
		{
			NApngDecoderExampleSchema = NSchema.Create(typeof(NApngDecoderExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NStackPanel stack = new NStackPanel();
			stack.HorizontalPlacement = ENHorizontalPlacement.Left;

			NImage bouncingBeachBall = AnimateImage(NResources.Image_AnimatedPNGs_BouncingBeachBall_png);
			stack.Add(NPairBox.Create("Bouncing beach ball:", new NImageBox(bouncingBeachBall)));

			NImage smiley = AnimateImage(NResources.Image_AnimatedPNGs_Smiley_png);
			stack.Add(NPairBox.Create("Smiley:", new NImageBox(smiley)));

			return new NUniSizeBoxGroup(stack);
		}
		protected override NWidget CreateExampleControls()
		{
			return null;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates NOV's animated PNG (APNG) image decoding capabilities and the built-in support for animated PNGs.
</p>
";
		}

		#endregion

		#region Implementation

		private NImage AnimateImage(NImage image)
		{
			NEncodedImageSource encodedImageSource = (NEncodedImageSource)image.ImageSource;
			encodedImageSource.AnimateFrames = true;
			return image;
		}

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NApngDecoderExample.
		/// </summary>
		public static readonly NSchema NApngDecoderExampleSchema;

		#endregion
	}
}