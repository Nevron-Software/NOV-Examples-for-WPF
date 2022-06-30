using System;

using Nevron.Nov.Editors;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Framework
{
	public class NStyleNode : NNode
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NStyleNode()
		{
			m_sName = "Style Node " + (Counter++).ToString();
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NStyleNode()
		{
			defaultLinearGradientFill = new NLinearGradientFill();
			defaultLinearGradientFill.GradientStops.Add(new NGradientStop(0.0f, NColor.Red));
			defaultLinearGradientFill.GradientStops.Add(new NGradientStop(0.5f, NColor.Yellow));
			defaultLinearGradientFill.GradientStops.Add(new NGradientStop(1.0f, NColor.Indigo));

			defaultRadialGradientFill = new NRadialGradientFill();
			defaultRadialGradientFill.GradientStops.Add(new NGradientStop(0.0f, NColor.Red));
			defaultRadialGradientFill.GradientStops.Add(new NGradientStop(0.5f, NColor.Yellow));
			defaultRadialGradientFill.GradientStops.Add(new NGradientStop(1.0f, NColor.Indigo));

			defaultAdvancedGradientFill = new NAdvancedGradientFill();
			defaultAdvancedGradientFill.Points.Add(new NAdvancedGradientPoint(NColor.Red, NAngle.Zero, 0, 0, 1, ENAdvancedGradientPointShape.Circle));
			defaultAdvancedGradientFill.Points.Add(new NAdvancedGradientPoint(NColor.Blue, NAngle.Zero, 1, 1, 1, ENAdvancedGradientPointShape.Circle));

			NStyleNodeSchema = NSchema.Create(typeof(NStyleNode), NNode.NNodeSchema);

			// Properties - fill
			FillProperty = NStyleNodeSchema.AddSlot("Fill", typeof(NFill), defaultFill);
			ColorFillProperty = NStyleNodeSchema.AddSlot("ColorFill", typeof(NColorFill), defaultColorFill);
			StockGradientFillProperty = NStyleNodeSchema.AddSlot("StockGradientFill", typeof(NStockGradientFill), defaultStockGradientFill);
			LinearGradientFillProperty = NStyleNodeSchema.AddSlot("LinearGradientFill", typeof(NLinearGradientFill), defaultLinearGradientFill);
			RadialGradientFillProperty = NStyleNodeSchema.AddSlot("RadialGradientFill", typeof(NRadialGradientFill), defaultRadialGradientFill);
			AdvancedGradientFillProperty = NStyleNodeSchema.AddSlot("AdvancedGradientFill", typeof(NAdvancedGradientFill), defaultAdvancedGradientFill);
			HatchFillProperty = NStyleNodeSchema.AddSlot("HatchFill", typeof(NHatchFill), defaultHatchFill);
			ImageFillProperty = NStyleNodeSchema.AddSlot("ImageFill", typeof(NImageFill), defaultImageFill);

			// Broperties - border
			BorderProperty = NStyleNodeSchema.AddSlot("Border", typeof(NBorder), defaultBorder);

			// Broperties - stroke
			StrokeProperty = NStyleNodeSchema.AddSlot("Stroke", typeof(NStroke), defaultStroke);

			// Properties - font
			FontProperty = NStyleNodeSchema.AddSlot("Font", typeof(NFont), defaultFont);

			// Constants
			Designers = new NDesigner[] {
				new NStyleNodeHStackDesigner(),
				new NStyleNodeVStackDesigner(),
				new NStyleNodeTabDesigner(),
				new NStyleNodeMixedDesigner()
			};
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the value of the Fill property.
		/// </summary>
		public NFill Fill
		{
			get
			{
				return (NFill)GetValue(FillProperty);
			}
			set
			{
				SetValue(FillProperty, value);
			}
		}
		/// <summary>
		/// Gets or sets the color fill style of the node.
		/// </summary>
		public NColorFill ColorFill
		{
			get
			{
				return (NColorFill)GetValue(ColorFillProperty);
			}
			set
			{
				SetValue(ColorFillProperty, value);
			}
		}
		/// <summary>
		/// Gets or sets the value of the TwoColorGradientFill property.
		/// </summary>
		public NStockGradientFill StockGradientFill
		{
			get
			{
				return (NStockGradientFill)GetValue(StockGradientFillProperty);
			}
			set
			{
				SetValue(StockGradientFillProperty, value);
			}
		}
		/// <summary>
		/// Gets or sets the value of the LinearGradientFill property.
		/// </summary>
		public NLinearGradientFill LinearGradientFill
		{
			get
			{
				return (NLinearGradientFill)GetValue(LinearGradientFillProperty);
			}
			set
			{
				SetValue(LinearGradientFillProperty, value);
			}
		}
		/// <summary>
		/// Gets or sets the value of the TwoColorGradientFill property.
		/// </summary>
		public NRadialGradientFill RadialGradientFill
		{
			get
			{
				return (NRadialGradientFill)GetValue(RadialGradientFillProperty);
			}
			set
			{
				SetValue(RadialGradientFillProperty, value);
			}
		}
		/// <summary>
		/// Gets or sets the value of the AdvancedGradientFill property.
		/// </summary>
		public NAdvancedGradientFill AdvancedGradientFill
		{
			get
			{
				return (NAdvancedGradientFill)GetValue(AdvancedGradientFillProperty);
			}
			set
			{
				SetValue(AdvancedGradientFillProperty, value);
			}
		}
		/// <summary>
		/// Gets or sets the value of the HatchFill property.
		/// </summary>
		public NHatchFill HatchFill
		{
			get
			{
				return (NHatchFill)GetValue(HatchFillProperty);
			}
			set
			{
				SetValue(HatchFillProperty, value);
			}
		}
		/// <summary>
		/// Gets or sets the value of the ImageFill property.
		/// </summary>
		public NImageFill ImageFill
		{
			get
			{
				return (NImageFill)GetValue(ImageFillProperty);
			}
			set
			{
				SetValue(ImageFillProperty, value);
			}
		}
		/// <summary>
		/// Gets or sets the border of the node.
		/// </summary>
		public NBorder Border
		{
			get
			{
				return (NBorder)GetValue(BorderProperty);
			}
			set
			{
				SetValue(BorderProperty, value);
			}
		}
		/// <summary>
		/// Gtes/Sets the stroke style of the node.
		/// </summary>
		public NStroke Stroke
		{
			get
			{
				return (NStroke)GetValue(StrokeProperty);
			}
			set
			{
				SetValue(StrokeProperty, value);
			}
		}
		/// <summary>
		/// Gets or sets the value of the Font property.
		/// </summary>
		public NFont Font
		{
			get
			{
				return (NFont)GetValue(FontProperty);
			}
			set
			{
				SetValue(FontProperty, value);
			}
		}

		#endregion

		#region Overrides

		public override string ToString()
		{
			return m_sName;
		}

		#endregion

		#region Fields

		internal string m_sName;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NStyleNode.
		/// </summary>
		public static readonly NSchema NStyleNodeSchema;
		/// <summary>
		/// Reference to the Fill property.
		/// </summary>
		public static readonly NProperty FillProperty;
		/// <summary>
		/// Reference to the ColorFill property.
		/// </summary>
		public static readonly NProperty ColorFillProperty;
		/// <summary>
		/// Reference to the HatchFill property.
		/// </summary>
		public static readonly NProperty HatchFillProperty;
		/// <summary>
		/// Reference to the TwoColorGradientFill property.
		/// </summary>
		public static readonly NProperty StockGradientFillProperty;
		/// <summary>
		/// Reference to the LinearGradientFill property.
		/// </summary>
		public static readonly NProperty LinearGradientFillProperty;
		/// <summary>
		/// Reference to the RadialGradientFill property.
		/// </summary>
		public static readonly NProperty RadialGradientFillProperty;
		/// <summary>
		/// Reference to the AdvancedGradientFill property.
		/// </summary>
		public static readonly NProperty AdvancedGradientFillProperty;
		/// <summary>
		/// Reference to the ImageFill property.
		/// </summary>
		public static readonly NProperty ImageFillProperty;
		/// <summary>
		/// Reference to the Border property.
		/// </summary>
		public static readonly NProperty BorderProperty;
		/// <summary>
		/// Reference to the Stroke property.
		/// </summary>
		public static readonly NProperty StrokeProperty;
		/// <summary>
		/// Reference to the Font property.
		/// </summary>
		public static readonly NProperty FontProperty;

		#endregion

		#region Static

		public static int Counter = 1;

		#endregion

		#region Constants

		private static readonly NBorder defaultBorder = NBorder.CreateFilledBorder(NColor.Black);
		private static readonly NFill defaultFill = null;
		private static readonly NColorFill defaultColorFill = new NColorFill(NColor.MediumBlue);
		private static readonly NStockGradientFill defaultStockGradientFill = new NStockGradientFill(ENGradientStyle.FromCenter, ENGradientVariant.Variant1, NColor.Black, NColor.White);
		private static readonly NLinearGradientFill defaultLinearGradientFill;
		private static readonly NRadialGradientFill defaultRadialGradientFill;
		private static readonly NAdvancedGradientFill defaultAdvancedGradientFill;
		private static readonly NHatchFill defaultHatchFill = new NHatchFill(ENHatchStyle.LightHorizontal, NColor.Black, NColor.White);
		private static readonly NImageFill defaultImageFill = new NImageFill();
		private static readonly NStroke defaultStroke = new NStroke();
		private static readonly NFont defaultFont = new NFont(NFontDescriptor.DefaultSansFamilyName, 10, ENFontStyle.Regular);

		public static readonly NDesigner[] Designers;

		#endregion

		#region Designers

		/// <summary>
		/// Designer for NStyleNode.
		/// </summary>
		public abstract class NStyleNodeDesigner : NDesigner
		{
			/// <summary>
			/// Default constructor.
			/// </summary>
			public NStyleNodeDesigner()
			{
				Schema = NStyleNodeSchema;
				m_Name = "Style Node Editor";

				SetPropertyCategory(FillProperty, FillStylesCategory);
				SetPropertyCategory(ColorFillProperty, FillStylesCategory);
				SetPropertyCategory(HatchFillProperty, FillStylesCategory);
				SetPropertyCategory(StockGradientFillProperty, FillStylesCategory);
				SetPropertyCategory(LinearGradientFillProperty, FillStylesCategory);
				SetPropertyCategory(RadialGradientFillProperty, FillStylesCategory);
				SetPropertyCategory(AdvancedGradientFillProperty, FillStylesCategory);
				SetPropertyCategory(ImageFillProperty, FillStylesCategory);

				SetPropertyCategory(BorderProperty, StrokeStylesCategory);
				SetPropertyCategory(StrokeProperty, StrokeStylesCategory);

				SetPropertyCategory(FontProperty, TextStylesCategory);				
			}

			public override string ToString()
			{
				return m_Name;
			}

			protected string m_Name;

			protected static readonly NLocalizedString FillStylesCategory = new NLocalizedString("Fill Styles");
			protected static readonly NLocalizedString StrokeStylesCategory = new NLocalizedString("Stroke Styles");
			protected static readonly NLocalizedString TextStylesCategory = new NLocalizedString("Text Styles");
		}

		public class NStyleNodeHStackDesigner : NStyleNodeDesigner
		{
			public NStyleNodeHStackDesigner()
			{
				m_Name = "Horizontal Stack Category Editor";
				SetCategoryEditor(NLocalizedString.Empty, NStackCategoryEditor.HorizontalEmbedChildEditorsTemplate);
			}
		}

		public class NStyleNodeVStackDesigner : NStyleNodeDesigner
		{
			public NStyleNodeVStackDesigner()
			{
				m_Name = "Vertical Stack Category Editor";
				SetCategoryEditor(NLocalizedString.Empty, NStackCategoryEditor.VerticalEmbedChildEditorsTemplate);
			}
		}

		public class NStyleNodeTabDesigner : NStyleNodeDesigner
		{
			public NStyleNodeTabDesigner()
			{
				m_Name = "Tab Category Editor";
				SetCategoryEditor(NLocalizedString.Empty, NTabCategoryEditor.HeadersTopTemplate);
			}
		}

		public class NStyleNodeMixedDesigner : NStyleNodeDesigner
		{
			public NStyleNodeMixedDesigner()
			{
				m_Name = "Mixed Category Editor";

				SetCategoryEditor(NLocalizedString.Empty, NStackCategoryEditor.VerticalEmbedChildEditorsTemplate);
				SetCategoryEditor(StrokeStylesCategory, NTabCategoryEditor.HeadersTopTemplate);
			}
		}

		#endregion
	}
}