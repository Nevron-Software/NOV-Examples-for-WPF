using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NScrollContentExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NScrollContentExample()
		{
			m_ScrollContent = null;
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NScrollContentExample()
		{
			NScrollContentExampleSchema = NSchema.Create(typeof(NScrollContentExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_ScrollContent = new NScrollContent();
			m_ScrollContent.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_ScrollContent.VerticalPlacement = ENVerticalPlacement.Top;
			m_ScrollContent.SetBorder(1, NColor.Red);
            m_ScrollContent.PreferredSize = new NSize(300, 250);

			// Create a table with some buttons
			NTableFlowPanel table = new NTableFlowPanel();
			table.MaxOrdinal = 10;
			for (int i = 1; i <= 150; i++)
			{
				table.Add(new NButton("Button " + i.ToString()));
			}

			m_ScrollContent.Content = table;
			return m_ScrollContent;
		}
		protected override NWidget CreateExampleControls()
		{
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_ScrollContent).CreatePropertyEditors(
				m_ScrollContent,
				NScrollContent.EnabledProperty,
				NScrollContent.HorizontalPlacementProperty,
				NScrollContent.VerticalPlacementProperty);

			NStackPanel stack = new NStackPanel();
			
			for (int i = 0, count = editors.Count; i < count; i++)
			{
				stack.Add(editors[i]);
			}

			return new NUniSizeBoxGroup(stack);
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to use the scroll content widget. The scroll content is a widget,
	which contains a single other widget, and allows for its scrolling. It measures to fit the
	contained element without scrollbars, but if this is not possible, the scroll content element
	will display scrollbars. The contained element is always sized to its desired size.
</p>
";
		}

		#endregion

		#region Fields

		private NScrollContent m_ScrollContent;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NScrollContentExample.
		/// </summary>
		public static readonly NSchema NScrollContentExampleSchema;

		#endregion
	}
}