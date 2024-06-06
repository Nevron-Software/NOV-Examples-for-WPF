using System;

using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Text;

namespace Nevron.Nov.Examples.Framework
{
	public class NSimpleNode : NNode
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NSimpleNode()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NSimpleNode()
		{
			NSimpleNodeSchema = NSchema.Create(typeof(NSimpleNode), NNode.NNodeSchema);

			// Properties
			BooleanValueProperty = NSimpleNodeSchema.AddSlot("BooleanValue", NDomType.Boolean, defaultBoolean);
			IntegerValueProperty = NSimpleNodeSchema.AddSlot("IntegerValue", NDomType.Int32, defaultInteger);
			LongValueProperty = NSimpleNodeSchema.AddSlot("LongValue", NDomType.Int64, defaultLong);
			UnsignedIntegerValueProperty = NSimpleNodeSchema.AddSlot("UnsignedIntegerValue", NDomType.UInt32, defaultUnsignedInteger);
			SingleValueProperty = NSimpleNodeSchema.AddSlot("SingleValue", NDomType.Single, defaultSingle);
			DoubleValueProperty = NSimpleNodeSchema.AddSlot("DoubleValue", NDomType.Double, defaultDouble);
			SpecifiedDoubleValueProperty = NSimpleNodeSchema.AddSlot("SpecifiedDoubleValue", NDomType.Double, defaultSpecifiedDouble);
			ComboBoxEnumValueProperty = NSimpleNodeSchema.AddSlot("ComboBoxEnum", typeof(ENSampleEnum), defaultComboBoxEnum);
			ComboBoxMaskedEnumProperty = NSimpleNodeSchema.AddSlot("ComboBoxMaskedEnum", typeof(ENSampleEnum), DefaultComboBoxMaskedEnum);
			HRadioGroupEnumProperty = NSimpleNodeSchema.AddSlot("HRadioGroupEnum", typeof(ENSampleEnum), defaultHRadioGroupEnum);
			VRadioGroupEnumProperty = NSimpleNodeSchema.AddSlot("VRadioGroupEnum", typeof(ENSampleEnum), defaultVRadioGroupEnum);

			AngleProperty = NSimpleNodeSchema.AddSlot("Angle", NDomType.NAngle, defaultAngle);
			ColorProperty = NSimpleNodeSchema.AddSlot("Color", NDomType.NColor, defaultColor);
			AdvancedColorProperty = NSimpleNodeSchema.AddSlot("AdvancedColor", NDomType.NColor, defaultAdvancedColor);
			LengthProperty = NSimpleNodeSchema.AddSlot("Length", NDomType.NLength, defaultLength);
			MarginsProperty = NSimpleNodeSchema.AddSlot("Margins", NDomType.NMargins, defaultMargins);
			PointProperty = NSimpleNodeSchema.AddSlot("Point", NDomType.NPoint, defaultPoint);
			RectangleProperty = NSimpleNodeSchema.AddSlot("Rectangle", NDomType.NRectangle, defaultRectangle);
			SizeProperty = NSimpleNodeSchema.AddSlot("Size", NDomType.NSize, defaultSize);
			MultiLengthProperty = NSimpleNodeSchema.AddSlot("MultiLength", typeof(NMultiLength), defaultMultiLength);

			// Designer
			NSimpleNodeSchema.SetMetaUnit(new NDesignerMetaUnit(typeof(NSimpleNodeDesigner)));
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the value of the Boolean property.
		/// </summary>
		public bool BooleanValue
		{
			get
			{
				return (bool)GetValue(BooleanValueProperty);
			}
			set
			{
				SetValue(BooleanValueProperty, value);
			}
		}
		/// <summary>
		/// Gets or sets the value of the Integer property.
		/// </summary>
		public int IntegerValue
		{
			get
			{
				return (int)GetValue(IntegerValueProperty);
			}
			set
			{
				SetValue(IntegerValueProperty, value);
			}
		}
		/// <summary>
		/// Gets or sets the value of the Long property.
		/// </summary>
		public long LongValue
		{
			get
			{
				return (long)GetValue(LongValueProperty);
			}
			set
			{
				SetValue(LongValueProperty, value);
			}
		}
		/// <summary>
		/// Gets or sets the value of the Single property.
		/// </summary>
		public float SingleValue
		{
			get
			{
				return (float)GetValue(SingleValueProperty);
			}
			set
			{
				SetValue(SingleValueProperty, value);
			}
		}
		/// <summary>
		/// Gets or sets the value of the UnsignedInteger property.
		/// </summary>
		public uint UnsignedIntegerValue
		{
			get
			{
				return (uint)GetValue(UnsignedIntegerValueProperty);
			}
			set
			{
				SetValue(UnsignedIntegerValueProperty, value);
			}
		}
		/// <summary>
		/// Gets or sets the value of the double property.
		/// </summary>
		public double DoubleValue
		{
			get
			{
				return (double)GetValue(DoubleValueProperty);
			}
			set
			{
				SetValue(DoubleValueProperty, value);
			}
		}
		/// <summary>
		/// Gets or sets the value of the SpecifiedDouble property.
		/// </summary>
		public double SpecifiedDoubleValue
		{
			get
			{
				return (double)GetValue(SpecifiedDoubleValueProperty);
			}
			set
			{
				SetValue(SpecifiedDoubleValueProperty, value);
			}
		}
		/// <summary>
		/// Gets or sets the value of the ComboBoxEnum property.
		/// </summary>
		public ENSampleEnum ComboBoxEnum
		{
			get
			{
				return (ENSampleEnum)GetValue(ComboBoxEnumValueProperty);
			}
			set
			{
				SetValue(ComboBoxEnumValueProperty, value);
			}
		}
		/// <summary>
		/// Gets or sets the value of the ComboBoxMaskedEnumValue property.
		/// </summary>
		public ENSampleEnum ComboBoxMaskedEnum
		{
			get
			{
				return (ENSampleEnum)GetValue(ComboBoxMaskedEnumProperty);
			}
			set
			{
				SetValue(ComboBoxMaskedEnumProperty, value);
			}
		}
		/// <summary>
		/// Gets or sets the value of the HRadioGroupEnum property.
		/// </summary>
		public ENSampleEnum HRadioGroupEnum
		{
			get
			{
				return (ENSampleEnum)GetValue(HRadioGroupEnumProperty);
			}
			set
			{
				SetValue(HRadioGroupEnumProperty, value);
			}
		}
		/// <summary>
		/// Gets or sets the value of the VRadioGroupEnum property.
		/// </summary>
		public ENSampleEnum VRadioGroupEnum
		{
			get
			{
				return (ENSampleEnum)GetValue(VRadioGroupEnumProperty);
			}
			set
			{
				SetValue(VRadioGroupEnumProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of the Angle property.
		/// </summary>
		public NAngle Angle
		{
			get
			{
				return (NAngle)GetValue(AngleProperty);
			}
			set
			{
				SetValue(AngleProperty, value);
			}
		}
		/// <summary>
		/// Gets or sets the value of the color property.
		/// </summary>
		public NColor Color
		{
			get
			{
				return (NColor)GetValue(ColorProperty);
			}
			set
			{
				SetValue(ColorProperty, value);
			}
		}
		/// <summary>
		/// Gets or sets the value of the AdvancedColor property.
		/// </summary>
		public NColor AdvancedColor
		{
			get
			{
				return (NColor)GetValue(AdvancedColorProperty);
			}
			set
			{
				SetValue(AdvancedColorProperty, value);
			}
		}
		/// <summary>
		/// Gets or sets the value of the Length property.
		/// </summary>
		public NLength Length
		{
			get
			{
				return (NLength)GetValue(LengthProperty);
			}
			set
			{
				SetValue(LengthProperty, value);
			}
		}
		/// <summary>
		/// Gets or sets the value of the Point property.
		/// </summary>
		public NPoint Point
		{
			get
			{
				return (NPoint)GetValue(PointProperty);
			}
			set
			{
				SetValue(PointProperty, value);
			}
		}
		/// <summary>
		/// Gets or sets the value of the size property.
		/// </summary>
		public NSize Size
		{
			get
			{
				return (NSize)GetValue(SizeProperty);
			}
			set
			{
				SetValue(SizeProperty, value);
			}
		}
		/// <summary>
		/// Gets or sets the value of the rectangle property.
		/// </summary>
		public NRectangle Rectangle
		{
			get
			{
				return (NRectangle)GetValue(RectangleProperty);
			}
			set
			{
				SetValue(RectangleProperty, value);
			}
		}
		/// <summary>
		/// Gets or sets the value of the Margins property.
		/// </summary>
		public NMargins Margins
		{
			get
			{
				return (NMargins)GetValue(MarginsProperty);
			}
			set
			{
				SetValue(MarginsProperty, value);
			}
		}
		/// <summary>
		/// Gets or sets the value of the MultiLength property.
		/// </summary>
		public NMultiLength MultiLength
		{
			get
			{
				return (NMultiLength)GetValue(MultiLengthProperty);
			}
			set
			{
				SetValue(MultiLengthProperty, value);
			}
		}

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NSimpleNode.
		/// </summary>
		public static readonly NSchema NSimpleNodeSchema;
		/// <summary>
		/// Reference to the Angle property.
		/// </summary>
		public static readonly NProperty AngleProperty;
		/// <summary>
		/// Reference to the Boolean property.
		/// </summary>
		public static readonly NProperty BooleanValueProperty;
		/// <summary>
		/// Reference to the Integer property.
		/// </summary>
		public static readonly NProperty IntegerValueProperty;
		/// <summary>
		/// Reference to the Long property.
		/// </summary>
		public static readonly NProperty LongValueProperty;
		/// <summary>
		/// Reference to the UnsignedInteger property.
		/// </summary>
		public static readonly NProperty UnsignedIntegerValueProperty;
		/// <summary>
		/// Reference to the Single property.
		/// </summary>
		public static readonly NProperty SingleValueProperty;
		/// <summary>
		/// Reference to the Double property.
		/// </summary>
		public static readonly NProperty DoubleValueProperty;
		/// <summary>
		/// Reference to the SpecifiedDouble property.
		/// </summary>
		public static readonly NProperty SpecifiedDoubleValueProperty;
		/// <summary>
		/// Reference to the Color property.
		/// </summary>
		public static readonly NProperty ColorProperty;
		/// <summary>
		/// Reference to the AdvancedColor property.
		/// </summary>
		public static readonly NProperty AdvancedColorProperty;
		/// <summary>
		/// Reference to the Point property.
		/// </summary>
		public static readonly NProperty PointProperty;
		/// <summary>
		/// Reference to the ComboBoxEnum property.
		/// </summary>
		public static readonly NProperty ComboBoxEnumValueProperty;
		/// <summary>
		/// Reference to the ComboBoxMaskedEnum property.
		/// </summary>
		public static readonly NProperty ComboBoxMaskedEnumProperty;
		/// <summary>
		/// Reference to the HRadioGroupEnum property.
		/// </summary>
		public static readonly NProperty HRadioGroupEnumProperty;
		/// <summary>
		/// Reference to the VRadioGroupEnum property.
		/// </summary>
		public static readonly NProperty VRadioGroupEnumProperty;
		/// <summary>
		/// Reference to the Length property.
		/// </summary>
		public static readonly NProperty LengthProperty;
		/// <summary>
		/// Reference to the Size property.
		/// </summary>
		public static readonly NProperty SizeProperty;
		/// <summary>
		/// Reference to the Margins property.
		/// </summary>
		public static readonly NProperty MarginsProperty;
		/// <summary>
		/// Reference to the Rectangle property.
		/// </summary>
		public static readonly NProperty RectangleProperty;
		/// <summary>
		/// Reference to the MultiLength property.
		/// </summary>
		public static readonly NProperty MultiLengthProperty;

		#endregion

		#region Default Values

		private const bool defaultBoolean = true;
		private const int defaultInteger = 0;
		private const long defaultLong = 0;
		private const uint defaultUnsignedInteger = 0;
		private const float defaultSingle = 0;
		private const double defaultDouble = 0;
		private const double defaultSpecifiedDouble = Double.NaN;
		private const ENSampleEnum defaultComboBoxEnum = ENSampleEnum.Option1;
		private const ENSampleEnum DefaultComboBoxMaskedEnum = ENSampleEnum.Option1 | ENSampleEnum.Option2;
		private const ENSampleEnum defaultHRadioGroupEnum = ENSampleEnum.Option1;
		private const ENSampleEnum defaultVRadioGroupEnum = ENSampleEnum.Option1;

		private static readonly NAngle defaultAngle = NAngle.Zero;
		private static readonly NColor defaultColor = NColor.White;
		private static readonly NColor defaultAdvancedColor = NColor.Black;
		private static readonly NLength defaultLength = NLength.Zero;
		private static readonly NMargins defaultMargins = NMargins.Zero;
		private static readonly NPoint defaultPoint = NPoint.Zero;
		private static readonly NRectangle defaultRectangle = NRectangle.Zero;
		private static readonly NSize defaultSize = NSize.Zero;
		private static readonly NMultiLength defaultMultiLength = NMultiLength.NewFixed(0);

		#endregion

		#region Nested Types

		[Flags]
		public enum ENSampleEnum
		{
			None = 0,
			Option1 = 1,
			Option2 = 2,
			Option3 = 4,
			Option4 = 8
		}

		#endregion

		#region Designer

		/// <summary>
		/// Designer for NSimpleNode.
		/// </summary>
		public class NSimpleNodeDesigner : NDesigner
		{
			/// <summary>
			/// Default constructor.
			/// </summary>
			public NSimpleNodeDesigner()
			{
				// Categories
				SetPropertyCategory(AdvancedColorProperty, ColorsCategory);
				SetPropertyCategory(ColorProperty, ColorsCategory);

				SetPropertyCategory(ComboBoxEnumValueProperty, EnumsCategory);
				SetPropertyCategory(ComboBoxMaskedEnumProperty, EnumsCategory);
				SetPropertyCategory(HRadioGroupEnumProperty, EnumsCategory);
				SetPropertyCategory(VRadioGroupEnumProperty, EnumsCategory);

				// Category Editors
				SetCategoryEditor(NLocalizedString.Empty, NTabCategoryEditor.HeadersTopTemplate);

				// Property Editors
				SetPropertyEditor(ComboBoxMaskedEnumProperty, NMaskedEnumPropertyEditor.DropDownTemplate);
				SetPropertyEditor(SpecifiedDoubleValueProperty, NSpecifiedDoublePropertyEditor.ZeroTemplate);
				SetPropertyEditor(AdvancedColorProperty, NColorPropertyEditor.AdvancedTemplate);
				SetPropertyEditor(HRadioGroupEnumProperty, NEnumPropertyEditor.HorizontalRadioGroupTemplate);
				SetPropertyEditor(VRadioGroupEnumProperty, NEnumPropertyEditor.VerticalRadioGroupTemplate);
			}

			private static readonly NLocalizedString ColorsCategory = new NLocalizedString("Colors");
			private static readonly NLocalizedString EnumsCategory = new NLocalizedString("Enums");
		}

		#endregion
	}
}