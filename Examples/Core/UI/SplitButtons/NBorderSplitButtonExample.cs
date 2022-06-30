using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NBorderSplitButtonExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NBorderSplitButtonExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NBorderSplitButtonExample()
		{
			NBorderSplitButtonExampleSchema = NSchema.Create(typeof(NBorderSplitButtonExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_BorderSplitButton = new NBorderSplitButton();
			m_BorderSplitButton.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_BorderSplitButton.VerticalPlacement = ENVerticalPlacement.Top;
			m_BorderSplitButton.SelectedValueChanged += OnBorderSplitButtonSelectedValueChanged;
			return m_BorderSplitButton;
		}
		protected override NWidget CreateExampleControls()
		{
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_BorderSplitButton).CreatePropertyEditors(
				m_BorderSplitButton,
				NBorderSplitButton.EnabledProperty,
				NBorderSplitButton.HorizontalPlacementProperty,
				NBorderSplitButton.VerticalPlacementProperty,
				NBorderSplitButton.DropDownButtonPositionProperty,
				NBorderSplitButton.HasAutomaticButtonProperty,
				NBorderSplitButton.HasNoneButtonProperty,
				NBorderSplitButton.HasMoreOptionsButtonProperty
			);

			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.Last;
			stack.FitMode = ENStackFitMode.Last;

			for (int i = 0; i < editors.Count; i++)
			{
				stack.Add(editors[i]);
			}

			m_EventsLog = new NExampleEventsLog();
			stack.Add(m_EventsLog);

			return new NUniSizeBoxGroup(stack);
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create and use border split buttons. Split buttons are drop down edits,
	whose item slot is filled with an action button, which generates a <b>Click</b> event on behalf of the
	split button. The border split button's drop down content provides the user with a convenient way to quickly
	select a border and its thickness.
</p>
";
		}

		#endregion

		#region Event Handlers

		private void OnBorderSplitButtonSelectedValueChanged(NValueChangeEventArgs arg)
		{
            NAutomaticValue<NBorder> selectedValue = (NAutomaticValue<NBorder>)arg.NewValue;
			string str;
			if (selectedValue.Automatic)
			{
				str = "Automatic";
			}
			else if (selectedValue.Value == null)
			{
				str = "None";
			}
			else
			{
				str = selectedValue.Value.ToString();
			}

			m_EventsLog.LogEvent("Selected border: " + str);
		}

		#endregion

		#region Fields

		private NExampleEventsLog m_EventsLog;
		private NBorderSplitButton m_BorderSplitButton;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NBorderSplitButtonExample.
		/// </summary>
		public static readonly NSchema NBorderSplitButtonExampleSchema;

		#endregion
	}
}