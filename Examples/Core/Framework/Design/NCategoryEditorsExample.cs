using Nevron.Nov.Editors;
using Nevron.Nov.Dom;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Framework
{
	public class NCategoryEditorsExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NCategoryEditorsExample()
		{
			m_Node = new NStyleNode();
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NCategoryEditorsExample()
		{
			NCategoryEditorsExampleSchema = NSchema.Create(typeof(NCategoryEditorsExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NStackPanel stack = new NStackPanel();
			stack.HorizontalPlacement = ENHorizontalPlacement.Left;
			stack.VerticalPlacement = ENVerticalPlacement.Top;
			
			NDesigner[] designers = NStyleNode.Designers;
			for (int i = 0, count = designers.Length; i < count; i++)
			{
				NDesigner designer = designers[i];
				NButton button = new NButton(designer.ToString());

				stack.Add(button);
				button.Tag = designer;
				button.Click += new Function<NEventArgs>(OnButtonClick);
			}

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
	This example demonstrates how to use category editor templates in node designers to achieve different designer layouts.
</p>
";
		}

		#endregion

		#region Event Handlers

		private void OnButtonClick(NEventArgs args)
		{
			NButton button = args.TargetNode as NButton;
			if (button == null)
				return;

			NDesigner designer = (NDesigner)button.Tag;
			NEditor editor = designer.CreateInstanceEditor(m_Node);
			NEditorWindow window = NApplication.CreateTopLevelWindow<NEditorWindow>();

			window.Editor = editor;
			window.Modal = false;
			window.Open();
		}

		#endregion

		#region Fields

		private NStyleNode m_Node;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NCategoryEditorsExample.
		/// </summary>
		public static readonly NSchema NCategoryEditorsExampleSchema;

		#endregion
	}
}