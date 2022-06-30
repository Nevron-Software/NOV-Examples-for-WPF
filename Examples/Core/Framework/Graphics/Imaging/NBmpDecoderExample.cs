using Nevron.Nov.Dom;
using Nevron.Nov.UI;
using Nevron.Nov.Layout;
using Nevron.Nov.Graphics;
using Nevron.Nov.DataStructures;
using System.IO;

namespace Nevron.Nov.Examples.Framework
{
	/// <summary>
	/// The BMP images displayed in this example are created by Jason Summers (jason1@pobox.com).
	/// For more information: http://entropymine.com/jason/bmpsuite/
	/// </summary>
	public class NBmpDecoderExample : NExampleBase
	{
		#region Constructors

		public NBmpDecoderExample()
		{
		}
		static NBmpDecoderExample()
		{
			NBmpDecoderExampleSchema = NSchema.Create(typeof(NBmpDecoderExample), NExampleBase.NExampleBaseSchema);
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

			NList<string> imageNames = NImageDecodingExampleHelper.GetImageNames("BmpSuite", "bmp");
			NMap<string, string> descriptions = NImageDecodingExampleHelper.LoadDescriptions(NResources.String_BmpSuite_txt);

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
	This example demonstrates NOV's BMP image decoding capabilities.
</p>
";
		}

		#endregion

		#region Schema

		public static readonly NSchema NBmpDecoderExampleSchema;

		#endregion
	}

	static class NImageDecodingExampleHelper
	{
		internal static NList<string> GetImageNames(string suiteName, string extension)
		{
			string[] names = NResources.Instance.GetResourceNames();
			NList<string> resources = new NList<string>();

			for (int i = 0, count = names.Length; i < count; i++)
			{
				if (names[i].EndsWith(extension) && names[i].Contains(suiteName))
				{
					resources.Add(names[i]);
				}
			}

			return resources;
		}
		internal static NMap<string, string> LoadDescriptions(string descriptionTextFileContent)
		{
			NMap<string, string> descriptions = new NMap<string, string>();

			using (StringReader reader = new StringReader(descriptionTextFileContent))
			{
				string line;

				while ((line = reader.ReadLine()) != null)
				{
					int dashIndex = line.IndexOf("-", 0);
					if (dashIndex > 0)
					{
						string name = line.Remove(dashIndex).Trim();
						string description = line.Remove(0, dashIndex + 1).Trim();
						descriptions.Add(name, description);
					}
				}
			}

			return descriptions;
		}
		internal static string ResourceNameToFileName(string resourceName)
		{
			int index = resourceName.LastIndexOf('_');
			index = resourceName.LastIndexOf('_', index - 1);
			return resourceName.Substring(index + 1).Replace('_', '.').ToLower();
		}
		internal static string GetImageDescription(NMap<string, string> descriptions, string resourceName)
		{
			string desc;

			if (descriptions.TryGet(ResourceNameToFileName(resourceName), out desc))
				return desc;

			return string.Empty;
		}
		internal static NImage LoadImage(string resourceName, ENCodecPreference decoderPref)
		{
			NEmbeddedResource resource = NResources.Instance.GetResource(resourceName);
			NImageData imageData = new NImageData(resource.Data);

			try
			{
				NRaster raster = imageData.Decode(decoderPref);
				return new NImage(raster);
			}
			catch
			{
				return NResources.Image_ErrorImage_png;
			}
		}
	}
}