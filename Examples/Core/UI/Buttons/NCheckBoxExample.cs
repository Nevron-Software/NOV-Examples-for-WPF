using Nevron.Nov.DataStructures;
using Nevron.Nov.Editors;
using Nevron.Nov.Dom;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NCheckBoxExample : NExampleBase
	{
		#region Constructors

		public NCheckBoxExample()
		{
		}
		static NCheckBoxExample()
		{
			NCheckBoxExampleSchema = NSchema.Create(typeof(NCheckBoxExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_CheckBox = new NCheckBox("Check Box");
			m_CheckBox.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_CheckBox.VerticalPlacement = ENVerticalPlacement.Top;
			m_CheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnCheckedChanged);
			return m_CheckBox;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.Last;
			stack.FitMode = ENStackFitMode.Last;

			// properties
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_CheckBox).CreatePropertyEditors(
				m_CheckBox,
				NCheckBox.EnabledProperty,
				NCheckBox.CheckedProperty,
				NCheckBox.IndeterminateProperty,
				NCheckBox.SymbolContentRelationProperty);

			NStackPanel propertiesStack = new NStackPanel();
			for (int i = 0; i < editors.Count; i++)
			{
				propertiesStack.Add(editors[i]);
			}

			stack.Add(new NGroupBox("Properties", new NUniSizeBoxGroup(propertiesStack)));

			// events log
			m_EventsLog = new NExampleEventsLog();
			stack.Add(m_EventsLog);

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates NOV check boxes. Use the controls to the right to enable/disable the check box and to change its state and direction.
</p>
";
		}

		#endregion

		#region Event Handlers

		private void OnCheckedChanged(NValueChangeEventArgs args)
		{
			NCheckBox checkBox = (NCheckBox)args.TargetNode;
			m_EventsLog.LogEvent("Check box " + (checkBox.Checked ? " checked" : " unchecked"));
		}

		#endregion

		#region Fields

		NCheckBox m_CheckBox;
		NExampleEventsLog m_EventsLog;

		#endregion

		#region Schema

		public static readonly NSchema NCheckBoxExampleSchema;

		#endregion
	}
}