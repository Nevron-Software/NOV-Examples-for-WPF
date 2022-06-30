using System;

using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;
using Nevron.Nov.Wmf;
using Nevron.Nov.Xml;

namespace Nevron.Nov.Examples
{
	/// <summary>
	/// The welcome panel of the examples application, which includes a NOV description
	/// and an icon for each of the NOV components.
	/// </summary>
	public class NWelcomePanel : NStackPanel
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NWelcomePanel()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NWelcomePanel()
		{
			NWelcomePanelSchema = NSchema.Create(typeof(NWelcomePanel), NStackPanel.NStackPanelSchema);

			// Modify some default values
			VerticalSpacingProperty.SetDefaultValue(NWelcomePanelSchema, LaneSpacing);		
		}

		#endregion@

		#region Properties - Private

		private NEmfDecompressor EmfDecompressor
		{
			get
			{
				NHomePage homePage = (NHomePage)GetFirstAncestor(NHomePage.NHomePageSchema);
				return homePage.m_EmfDecompressor;
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Initializes the panel and creates its content.
		/// </summary>
		public void Initialize(NXmlElement root)
		{
			Clear();

			Add(CreateHeaderLabel());
			Add(CreateProductGroupsStack(root));
			Add(CreateFooterLabel());
			Add(CreatePlatformsStack());
		}

		#endregion

		#region Implementation

		/// <summary>
		/// Creates the NOV description label placed in the header.
		/// </summary>
		/// <returns></returns>
		private NLabel CreateHeaderLabel()
		{
			NColor transparentWhite = new NColor(NColor.White, 0);
			NColor textColor = NHomePage.HeaderColor;
			NColor transparentTextColor = new NColor(textColor, 0);

            NLabel headerLabel = new NLabel("Leading User Interface components for Blazor, WinForms, WPF and Xamarin.Mac");

			// Background and border
			headerLabel.BackgroundFill = new NLinearGradientFill(NAngle.Zero, new NColor[] {
				transparentWhite, NColor.White, NColor.White, transparentWhite
			});

			NBorder border = new NBorder();
			border.TopSide = new NBorderSide(new NLinearGradientFill(NAngle.Zero, new NColor[] {
				transparentTextColor, textColor, textColor, transparentTextColor }));
			border.BottomSide = new NBorderSide(new NLinearGradientFill(NAngle.Zero, new NColor[] {
				transparentTextColor, textColor, transparentTextColor }));
			headerLabel.Border = border;
			headerLabel.BorderThickness = new NMargins(0, 1, 0, 1);

			// Text style
			headerLabel.TextFill = new NColorFill(textColor);
			headerLabel.Font = new NFont(NFontDescriptor.DefaultSansFamilyName, HeaderFontSize);

			// Box model
			headerLabel.Padding = new NMargins(0, LaneSpacing);
			headerLabel.HorizontalPlacement = ENHorizontalPlacement.Center;

			return headerLabel;
		}
		/// <summary>
		/// Creates the product groups stack panel.
		/// </summary>
		/// <param name="root"></param>
		/// <returns></returns>
		private NStackPanel CreateProductGroupsStack(NXmlElement root)
		{
			NMap<string, NStackPanel> stackMap = new NMap<string, NStackPanel>();

			// Create the main stack
			NStackPanel mainStack = new NStackPanel();
			mainStack.Direction = ENHVDirection.LeftToRight;
			mainStack.Margins = new NMargins(0, LaneSpacing * 2);

			// Create a stack panel for each license groups and add it to the main stack
			int count = root.ChildrenCount;
			for (int i = 0; i < count; i++)
			{
                NXmlElement categoryElement = root.GetChildAt(i) as NXmlElement;
                if (categoryElement == null)
                    continue;

				string license = categoryElement.GetAttributeValue("license");

				NStackPanel licenseGroupStack;
				if (!stackMap.TryGet(license, out licenseGroupStack))
				{
					// A stack panel for the license group not found, so create one
					licenseGroupStack = CreateProductGroupStack();
					stackMap.Add(license, licenseGroupStack);

					// Create a stack for the current group and its name
					NStackPanel stack = new NStackPanel();
					stack.Direction = ENHVDirection.TopToBottom;

					// 1. Add the license group stack
					stack.Add(licenseGroupStack);

					// 2. Add the bracket
					NColor color = NColor.Parse(categoryElement.GetAttributeValue("color"));
					NWidget bracket = CreateLicenseGroupBracket(color);
					stack.Add(bracket);

					// 3. Add the label
					NLabel label = new NLabel(license);
					label.HorizontalPlacement = ENHorizontalPlacement.Center;
					label.TextFill = new NColorFill(color);
					label.Font = new NFont(NFontDescriptor.DefaultSansFamilyName, InfoFontSize);
					stack.Add(label);

					mainStack.Add(stack);
				}

				// Create an image box for the current category
				NImageBox imageBox = CreateImageBox(categoryElement);
				licenseGroupStack.Add(imageBox);
			}

			return mainStack;
		}
        private NImageBox CreateImageBox(NXmlElement categoryElement)
		{
			string emfName = categoryElement.GetAttributeValue("namespace") + ".emf";

			// Get the metafile
			byte[] metaImage = EmfDecompressor.GetMetaImage(emfName);

			// Create an image box for it
			NComponentImageBox imageBox = new NComponentImageBox();
            imageBox.Image = new NImage(new NBytesImageSource(metaImage));
			imageBox.Status = categoryElement.GetAttributeValue("status");
			imageBox.Tag = categoryElement.GetAttributeValue("name");

			return imageBox;
		}

		/// <summary>
		/// Creates the bracket of a license group.
		/// </summary>
		/// <param name="color"></param>
		/// <returns></returns>
		private NWidget CreateLicenseGroupBracket(NColor color)
		{
			NWidget bracket = new NWidget();
			bracket.BackgroundFill = new NStockGradientFill(ENGradientStyle.Horizontal,
				ENGradientVariant.Variant1, new NColor(NColor.White, 128), NColor.White);
			bracket.Border = NBorder.CreateFilledBorder(color);
			bracket.BorderThickness = new NMargins(1, 0, 1, 1);
			bracket.PreferredHeight = IconSpacing / 2;
			bracket.Padding = new NMargins(0, NDesign.VerticalSpacing);
			bracket.Margins = new NMargins(IconSpacing / 4, 0);

			return bracket;
		}
		/// <summary>
		/// Creates the footer label.
		/// </summary>
		/// <returns></returns>
		private NLabel CreateFooterLabel()
		{
			NLabel label = new NLabel("All Components and Examples run from a Single Codebase" +
				Environment.NewLine + "on the following platforms:");
			label.HorizontalPlacement = ENHorizontalPlacement.Center;
			label.TextAlignment = ENContentAlignment.MiddleCenter;
			label.Font = new NFont(NFontDescriptor.DefaultSansFamilyName, InfoFontSize);
			label.TextFill = new NColorFill(NColor.DimGray);
			label.Padding = new NMargins(IconSpacing * 3, 0);

			// Border
			NBorder border = new NBorder();
			border.TopSide = new NBorderSide(new NLinearGradientFill(NAngle.Zero,
				NColor.Transparent, NColor.Gray, NColor.Transparent));
			label.Border = border;
			label.BorderThickness = new NMargins(0, 1, 0, 0);
			
			return label;
		}
		/// <summary>
		/// Creates the platforms stack panel, placed in the footer.
		/// </summary>
		/// <returns></returns>
		private NStackPanel CreatePlatformsStack()
		{
			NStackPanel stack = new NStackPanel();
			stack.Direction = ENHVDirection.LeftToRight;
			stack.HorizontalPlacement = ENHorizontalPlacement.Center;
			stack.HorizontalSpacing = IconSpacing;

			INIterator<NKeyValuePair<string, byte[]>> iter = EmfDecompressor.GetImageIterator();
			while (iter.MoveNext())
			{
				string name = iter.Current.Key;
				if (name.StartsWith("Platform", StringComparison.Ordinal))
				{
					// Add an image box with the current metafile image
                    NImageBox imageBox = new NImageBox(new NBytesImageSource(iter.Current.Value));
					imageBox.Tag = name;

					int index = GetPlatformIndex(stack, name);
					stack.Insert(index, imageBox);
				}
			}

			return stack;
		}

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NWelcomePanel.
		/// </summary>
		public static readonly NSchema NWelcomePanelSchema;

		#endregion

		#region Static Methods

		/// <summary>
		/// Creates a new products group stack panel.
		/// </summary>
		/// <returns></returns>
		private static NStackPanel CreateProductGroupStack()
		{
			NStackPanel stack = new NStackPanel();
			stack.BackgroundFill = new NLinearGradientFill(new NAngle(90),
				NColor.Transparent, NColor.Transparent, new NColor(NColor.White, 128));
			stack.Direction = ENHVDirection.LeftToRight;
			stack.HorizontalSpacing = IconSpacing;
			stack.Margins = new NMargins(IconSpacing / 2, 0, IconSpacing / 2, 0);

			return stack;
		}
		private static int GetPlatformIndex(NStackPanel stack, string name)
		{
			int i, count = stack.Count;
			for (i = 0; i < count; i++)
			{
				if (String.CompareOrdinal((string)stack[i].Tag, name) > 0)
					break;
			}

			return i;
		}

		#endregion

		#region Constants

		private const double IconSpacing = 20;
		private const double LaneSpacing = 20;
		private const double HeaderFontSize = 20;
		private const double InfoFontSize = 16;

		#endregion
	}
}