using System;
using System.Globalization;

using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Framework
{
	public class NValueTypeEditorsExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NValueTypeEditorsExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NValueTypeEditorsExample()
		{
			NValueTypeEditorsExampleSchema = NSchema.Create(typeof(NValueTypeEditorsExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NTab tab = new NTab();
			m_SimpleNode = new NSimpleNode();

			// Primitive types
			tab.TabPages.Add(CreateBooleanPage());
			tab.TabPages.Add(CreateInt32Page());
			tab.TabPages.Add(CreateInt64Page());
			tab.TabPages.Add(CreateUInt32Page());
			tab.TabPages.Add(CreateSinglePage());
			tab.TabPages.Add(CreateDoublePage());
			tab.TabPages.Add(CreateEnumPage());

			// Nevron types
			tab.TabPages.Add(CreateAnglePage());
			tab.TabPages.Add(CreateColorPage());
			tab.TabPages.Add(CreateGraphicsCorePage());
			tab.TabPages.Add(CreateTextPage());

			return tab;
		}
		protected override NWidget CreateExampleControls()
		{
			return null;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates the value type property editors. Select the tab page for the value types
    you are interested in and your will see their property editors.
</p>
";
		}

		#endregion

		#region Implementation - Primitive Types

		private NTabPage CreateBooleanPage()
		{
			NTabPage tabPage = new NTabPage("Boolean");
			NStackPanel stack = new NStackPanel();
			tabPage.Content = stack;
			stack.HorizontalPlacement = ENHorizontalPlacement.Left;

			NGroupBox groupBox = new NGroupBox("Default");
			stack.Add(groupBox);

			NBooleanPropertyEditor editor = (NBooleanPropertyEditor)CreateEditor(NSimpleNode.BooleanValueProperty);
			groupBox.Content = editor;

			return tabPage;
		}
		private NTabPage CreateInt32Page()
		{
			NTabPage page = new NTabPage("Integer");

			NStackPanel stack = new NStackPanel();
			page.Content = stack;
			stack.HorizontalPlacement = ENHorizontalPlacement.Left;

			stack.Add(CreateSample("Default", NSimpleNode.IntegerValueProperty, Double.NaN, Double.NaN, Double.NaN, -1));
			stack.Add(CreateSample("Example 1", NSimpleNode.IntegerValueProperty, 2, -10, 10, -1));
			stack.Add(CreateSample("Example 2", NSimpleNode.IntegerValueProperty, 10, 200, 300, -1));		

			return page;
		}
		private NTabPage CreateInt64Page()
		{
			NTabPage page = new NTabPage("Long");

			NStackPanel stack = new NStackPanel();
			page.Content = stack;
			stack.HorizontalPlacement = ENHorizontalPlacement.Left;

			stack.Add(CreateSample("Default", NSimpleNode.LongValueProperty, Double.NaN, Double.NaN, Double.NaN, -1));
			stack.Add(CreateSample("Example 1", NSimpleNode.LongValueProperty, 2, -10, 10, -1));
			stack.Add(CreateSample("Example 2", NSimpleNode.LongValueProperty, 10, 200, 300, -1));

			return page;
		}
		private NTabPage CreateUInt32Page()
		{
			NTabPage page = new NTabPage("Unsigned Integer");

			NStackPanel stack = new NStackPanel();
			page.Content = stack;
			stack.HorizontalPlacement = ENHorizontalPlacement.Left;

			stack.Add(CreateSample("Default", NSimpleNode.UnsignedIntegerValueProperty, Double.NaN, Double.NaN, Double.NaN, -1));
			stack.Add(CreateSample("Example 1", NSimpleNode.UnsignedIntegerValueProperty, 2, 0, 10, -1));
			stack.Add(CreateSample("Example 2", NSimpleNode.UnsignedIntegerValueProperty, 10, 200, 300, -1));

			return page;
		}
		private NTabPage CreateSinglePage()
		{
			NTabPage page = new NTabPage("Single");

			NStackPanel stack = new NStackPanel();
			page.Content = stack;
			stack.HorizontalPlacement = ENHorizontalPlacement.Left;

			stack.Add(CreateSample("Default", NSimpleNode.SingleValueProperty, Double.NaN, Double.NaN, Double.NaN, -1));
			stack.Add(CreateSample("Example 1", NSimpleNode.SingleValueProperty, 2, -10, 10, 0));
			stack.Add(CreateSample("Example 2", NSimpleNode.SingleValueProperty, 0.2, -1, 1, 1));
			stack.Add(CreateSample("Example 3", NSimpleNode.SingleValueProperty, 0.03, 4, 5, 2));

			return page;
		}
		private NTabPage CreateDoublePage()
		{
			NTabPage page = new NTabPage("Double");

			NStackPanel stack = new NStackPanel();
			page.Content = stack;
			stack.HorizontalPlacement = ENHorizontalPlacement.Left;

			stack.Add(CreateSample("Default", NSimpleNode.DoubleValueProperty, Double.NaN, Double.NaN, Double.NaN, -1));
			stack.Add(CreateSample("Example 1", NSimpleNode.DoubleValueProperty, 2, -10, 10, 0));
			stack.Add(CreateSample("Example 2", NSimpleNode.DoubleValueProperty, 0.2, -1, 1, 1));
			stack.Add(CreateSample("Example 3", NSimpleNode.DoubleValueProperty, 0.03, 4, 5, 2));
			stack.Add(CreateSample("Specified Double", NSimpleNode.SpecifiedDoubleValueProperty, 1, Double.MinValue, Double.MaxValue, 2));

			return page;
		}
		private NTabPage CreateEnumPage()
		{
			NTabPage page = new NTabPage("Enum");
			NStackPanel stack = new NStackPanel();
			page.Content = stack;
			stack.HorizontalPlacement = ENHorizontalPlacement.Left;

			NList<NPropertyEditor> editors = SimpleNodeDesigner.CreatePropertyEditors(m_SimpleNode,
				NSimpleNode.ComboBoxEnumValueProperty,
				NSimpleNode.HRadioGroupEnumProperty,
				NSimpleNode.VRadioGroupEnumProperty);

			NGroupBox groupBox = new NGroupBox(editors[0].EditedProperty.ToString());
			stack.Add(groupBox);
			groupBox.Content = editors[0];

			for (int i = 1, count = editors.Count; i < count; i++)
			{
				NPropertyEditor editor = editors[i];
				stack.Add(editor);
			}

			return page;
		}

		private NGroupBox CreateSample(string title, NProperty property, double step, double min, double max, int decimalPlaces)
		{
			NGroupBox groupBox = new NGroupBox(title);

			NStackPanel stack = new NStackPanel();
			groupBox.Content = stack;
			stack.VerticalSpacing = 10;

			NStackPanel propertyStack = new NStackPanel();
			stack.Add(new NUniSizeBoxGroup(propertyStack));
			propertyStack.HorizontalPlacement = ENHorizontalPlacement.Left;

			NNumberPropertyEditor editor = CreateEditor(property, step, min, max, decimalPlaces);
			propertyStack.Add(new NPairBox("Step = ", editor.Step, true));
			propertyStack.Add(new NPairBox("Minimum = ", editor.Minimum, true));
			propertyStack.Add(new NPairBox("Maximum = ", editor.Maximum, true));
			if (editor is NFloatingNumberPropertyEditor)
			{
				propertyStack.Add(new NPairBox("Decimal Places = ", ((NFloatingNumberPropertyEditor)editor).DecimalPlaces, true));
			}

			for (int i = 0, count = propertyStack.Count; i < count; i++)
			{
				NPairBox pairBox = (NPairBox)propertyStack[i];
				NUniSizeBox box1 = (NUniSizeBox)pairBox.Box1;
				box1.Content.HorizontalPlacement = ENHorizontalPlacement.Right;
			}

			stack.Add(editor);

			return groupBox;
		}
		private NNumberPropertyEditor CreateEditor(NProperty property, double step, double min, double max, int decimalPlaces)
		{
			NNumberPropertyEditor editor = (NNumberPropertyEditor)CreateEditor(property);
			NSimpleNode node = (NSimpleNode)editor.EditedNode;
			if (Double.IsNaN(step) == false)
			{
				editor.Step = step;
			}

			if (Double.IsNaN(min) == false)
			{
				editor.Minimum = min;
			}

			if (Double.IsNaN(max) == false)
			{
				editor.Maximum = max;
			}

			if (decimalPlaces != -1 && editor is NFloatingNumberPropertyEditor)
			{
				((NFloatingNumberPropertyEditor)editor).DecimalPlaces = decimalPlaces;
			}

			// Ensure the value is in the range [min, max]
			double value = Convert.ToDouble(node.GetValue(property));
			if (value < min)
			{
				node.SetValue(property, Convert.ChangeType(editor.Minimum, property.DomType.CLRType, CultureInfo.InvariantCulture));
			}
			if (value > max)
			{
				node.SetValue(property, Convert.ChangeType(editor.Maximum, property.DomType.CLRType, CultureInfo.InvariantCulture));
			}

			return editor;
		}

		#endregion

		#region Implementation - Nevron Types

		private NTabPage CreateAnglePage()
		{
			NTabPage page = new NTabPage("Angle");
			NStackPanel stack = CreateStackPanel();
			page.Content = stack;

			NGroupBox groupBox = new NGroupBox("Default");
			stack.Add(groupBox);

			NAnglePropertyEditor editor = (NAnglePropertyEditor)CreateEditor(NSimpleNode.AngleProperty);
			groupBox.Content = editor;

			return page;
		}
		private NTabPage CreateColorPage()
		{
			NTabPage page = new NTabPage("Color");
			NStackPanel stack = CreateStackPanel();
			page.Content = stack;

			// Create a default (drop down) color editor
			NGroupBox groupBox = new NGroupBox("Default (drop down)");
			stack.Add(groupBox);

			NColorPropertyEditor editor = (NColorPropertyEditor)CreateEditor(NSimpleNode.ColorProperty);
			groupBox.Content = editor;

			// Create an advanced color editor
			editor = (NColorPropertyEditor)CreateEditor(NSimpleNode.AdvancedColorProperty);
			stack.Add(editor);

			return page;
		}
		private NTabPage CreateGraphicsCorePage()
		{
			NTabPage page = new NTabPage("Graphics Core");
			NStackPanel stack = CreateStackPanel();
			page.Content = new NUniSizeBoxGroup(stack);

			NList<NPropertyEditor> editors = SimpleNodeDesigner.CreatePropertyEditors(m_SimpleNode,
				NSimpleNode.PointProperty,
				NSimpleNode.SizeProperty,
				NSimpleNode.RectangleProperty,
				NSimpleNode.MarginsProperty);

			for (int i = 0, count = editors.Count; i < count; i++)
			{
				stack.Add(editors[i]);
			}

			return page;
		}
		private NTabPage CreateTextPage()
		{
			NTabPage page = new NTabPage("Text");
			NStackPanel stack = CreateStackPanel();
			page.Content = new NUniSizeBoxGroup(stack);

			stack.Add(SimpleNodeDesigner.CreatePropertyEditor(m_SimpleNode, NSimpleNode.MultiLengthProperty));

			return page;
		}

		#endregion

		#region Implementation

		private NPropertyEditor CreateEditor(NProperty property)
		{
			return SimpleNodeDesigner.CreatePropertyEditor(m_SimpleNode, property);
		}
		private NStackPanel CreateStackPanel()
		{
			NStackPanel stack = new NStackPanel();
			stack.HorizontalPlacement = ENHorizontalPlacement.Left;
			return stack;
		}

		#endregion

		#region Fields

		private NSimpleNode m_SimpleNode;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NValueTypeEditorsExample.
		/// </summary>
		public static readonly NSchema NValueTypeEditorsExampleSchema;

		#endregion

		#region Constants

		private static readonly NDesigner SimpleNodeDesigner = NDesigner.GetDesigner(NSimpleNode.NSimpleNodeSchema);

		#endregion
	}
}