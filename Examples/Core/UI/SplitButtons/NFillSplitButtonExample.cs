using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NFillSplitButtonExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NFillSplitButtonExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NFillSplitButtonExample()
		{
			NFillSplitButtonExampleSchema = NSchema.Create(typeof(NFillSplitButtonExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_FillSplitButton = new NFillSplitButton();
			m_FillSplitButton.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_FillSplitButton.VerticalPlacement = ENVerticalPlacement.Top;
			m_FillSplitButton.SelectedValueChanged += OnFillSplitButtonSelectedValueChanged;
			return m_FillSplitButton;
		}
		protected override NWidget CreateExampleControls()
		{
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_FillSplitButton).CreatePropertyEditors(
				m_FillSplitButton,
				NFillSplitButton.EnabledProperty,
				NFillSplitButton.HorizontalPlacementProperty,
				NFillSplitButton.VerticalPlacementProperty,
				NFillSplitButton.DropDownButtonPositionProperty,
				NFillSplitButton.HasAutomaticButtonProperty,
				NFillSplitButton.HasNoneButtonProperty,
				NFillSplitButton.HasMoreOptionsButtonProperty
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
	This example demonstrates how to create and use fill split buttons. Split buttons are drop down edits,
	whose item slot is filled with an action button, which generates a <b>Click</b> event on behalf of the
	split button. The fill split button's drop down content provides the user with a convenient way to quickly
	select a fill.
</p>
";
		}

		#endregion

		#region Event Handlers

		private void OnFillSplitButtonSelectedValueChanged(NValueChangeEventArgs arg)
		{
            NAutomaticValue<NFill> selectedValue = (NAutomaticValue<NFill>)arg.NewValue;
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

			m_EventsLog.LogEvent("Selected fill: " + str);
		}

		#endregion

		#region Fields

		private NExampleEventsLog m_EventsLog;
		private NFillSplitButton m_FillSplitButton;

		#endregion

		#region Schema

		public static readonly NSchema NFillSplitButtonExampleSchema;

		#endregion
	}
}