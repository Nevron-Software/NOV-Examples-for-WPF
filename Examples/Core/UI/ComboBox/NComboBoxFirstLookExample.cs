using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NComboBoxFirstLookExample : NExampleBase
	{
		#region Constructors

		public NComboBoxFirstLookExample()
		{
		}
		static NComboBoxFirstLookExample()
		{
			NComboBoxFirstLookExampleSchema = NSchema.Create(typeof(NComboBoxFirstLookExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// create the combo box
			m_ComboBox = new NComboBox();
			m_ComboBox.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_ComboBox.VerticalPlacement = ENVerticalPlacement.Top;
			m_ComboBox.DropDownStyle = ENComboBoxStyle.DropDownList;			

			// add a few items
			for (int i = 0; i < 10; i++)
			{
				m_ComboBox.Items.Add(new NComboBoxItem("Item " + i.ToString()));
			}

			// select the first item
			m_ComboBox.SelectedIndex = 0;

			// hook combo box selection events
			m_ComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnComboBoxSelectedIndexChanged);

			return m_ComboBox;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.Last;
			stack.FitMode = ENStackFitMode.Last;

			NStackPanel propertiesStack = new NStackPanel();
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_ComboBox).CreatePropertyEditors(
				m_ComboBox,
				NComboBox.EnabledProperty,
				NComboBox.HorizontalPlacementProperty,
				NComboBox.VerticalPlacementProperty,
				NComboBox.DropDownButtonPositionProperty,
				NComboBox.SelectedIndexProperty,
				NComboBox.DropDownStyleProperty,
                NComboBox.WheelNavigationModeProperty
            );

			for (int i = 0; i < editors.Count; i++)
			{
				propertiesStack.Add(editors[i]);
			}

			stack.Add(new NGroupBox("Properties", new NUniSizeBoxGroup(propertiesStack)));

			// create the events list box
			m_EventsLog = new NExampleEventsLog();
			stack.Add(m_EventsLog);

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create a simple combo box with text only items. You can use the controls
	on the right to modify various properties of the combo box.
</p>
";
		}

		#endregion

		#region Event Handlers

		private void OnComboBoxSelectedIndexChanged(NValueChangeEventArgs args)
		{
			NComboBox comboBox = (NComboBox)args.TargetNode;
			m_EventsLog.LogEvent("Selected Index: " + comboBox.SelectedIndex.ToString());
		}
		private void OnShowDesignerButtonClicked(NEventArgs args)
		{
			NEditor editor = NDesigner.GetDesigner(m_ComboBox).CreateInstanceEditor(m_ComboBox);
			NEditorWindow window = NApplication.CreateTopLevelWindow<NEditorWindow>();
			window.Editor = editor;
			window.Open();
		}

		#endregion

		#region Fields

		private NExampleEventsLog m_EventsLog;
		private NComboBox m_ComboBox;

		#endregion

		#region Schema

		public static readonly NSchema NComboBoxFirstLookExampleSchema;

		#endregion
	}
}