using Nevron.Nov.Dom;
using Nevron.Nov.UI;
using Nevron.Nov.DataStructures;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;

namespace Nevron.Nov.Examples.Framework
{
	/// <summary>
	/// The PNG images displayed in this example are created by Willem van Schaik (willem@schaik.com).
	/// For more information: http://schaik.com/
	/// </summary>
	public class NPngDecoderExample : NExampleBase
	{
		#region Constructors

		public NPngDecoderExample()
		{
		}
		static NPngDecoderExample()
		{
			NPngDecoderExampleSchema = NSchema.Create(typeof(NPngDecoderExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			string[] colHeadings = new string[]
			{
				"Image",
				"Description",
				"Decoded with NOV Decoders",
				"Decoded with Native Decoders"
			};

			int colCount = colHeadings.Length;

			NTableFlowPanel table = new NTableFlowPanel();
			table.HorizontalPlacement = ENHorizontalPlacement.Left;
			table.VerticalPlacement = ENVerticalPlacement.Top;
			table.Padding = new NMargins(30);
			table.HorizontalSpacing = 30;
			table.VerticalSpacing = 30;
			table.MaxOrdinal = colCount;

			NList<string> imageNames = NImageDecodingExampleHelper.GetImageNames("PngSuite", "png");
			NMap<string, string> descriptions = NImageDecodingExampleHelper.LoadDescriptions(NResources.String_PngSuite_txt);

			for (int i = 0; i < colCount; i++)
			{
				NLabel label = new NLabel(colHeadings[i]);
				label.Font = new NFont(NFontDescriptor.DefaultSansFamilyName, 9, ENFontStyle.Bold);
				table.Add(label);
			}

			int rowCount = imageNames.Count;
			for (int i = 0; i < rowCount; i++)
			{
				string resourceName = imageNames[i];
				string description = NImageDecodingExampleHelper.GetImageDescription(descriptions, resourceName);

				NLabel nameLabel = new NLabel(resourceName);
				nameLabel.MaxWidth = 200;

				NLabel descriptionLabel = new NLabel(description);
				descriptionLabel.MaxWidth = 200;
				descriptionLabel.TextWrapMode = ENTextWrapMode.WordWrap;

				NImage novImage = NImageDecodingExampleHelper.LoadImage(resourceName, ENCodecPreference.OnlyNOV);
				NImageBox novImageBox = new NImageBox(novImage);
				novImageBox.ImageMapping = new NAlignTextureMapping(ENHorizontalAlignment.Center, ENVerticalAlignment.Center);

				NImage nativeImage = NImageDecodingExampleHelper.LoadImage(resourceName, ENCodecPreference.PreferNative);
				NImageBox nativeImageBox = new NImageBox(nativeImage);
				nativeImageBox.ImageMapping = new NAlignTextureMapping(ENHorizontalAlignment.Center, ENVerticalAlignment.Center);

				table.Add(nameLabel);
				table.Add(descriptionLabel);
				table.Add(novImageBox);
				table.Add(nativeImageBox);
			}

			// The table must be scrollable
			NScrollContent scroll = new NScrollContent();
			scroll.Content = table;
			return scroll;
		}
		protected override NWidget CreateExampleControls()
		{
			return null;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates NOV's PNG image decoding capabilities.
</p>
";
		}

		#endregion

		#region Schema

		public static readonly NSchema NPngDecoderExampleSchema;

		#endregion
	}
}