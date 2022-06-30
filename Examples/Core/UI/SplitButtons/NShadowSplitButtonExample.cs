using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NShadowSplitButtonExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NShadowSplitButtonExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NShadowSplitButtonExample()
		{
			NShadowSplitButtonExampleSchema = NSchema.Create(typeof(NShadowSplitButtonExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_ShadowSplitButton = new NShadowSplitButton();
			m_ShadowSplitButton.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_ShadowSplitButton.VerticalPlacement = ENVerticalPlacement.Top;
			m_ShadowSplitButton.SelectedValueChanged += OnShadowSplitButtonSelectedValueChanged;
			return m_ShadowSplitButton;
		}
		protected override NWidget CreateExampleControls()
		{
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_ShadowSplitButton).CreatePropertyEditors(
				m_ShadowSplitButton,
				NShadowSplitButton.EnabledProperty,
				NShadowSplitButton.HorizontalPlacementProperty,
				NShadowSplitButton.VerticalPlacementProperty,
				NShadowSplitButton.DropDownButtonPositionProperty,
				NShadowSplitButton.HasAutomaticButtonProperty,
				NShadowSplitButton.HasNoneButtonProperty,
				NShadowSplitButton.HasMoreOptionsButtonProperty
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
	This example demonstrates how to create and use shadow split buttons. Split buttons are drop down edits,
	whose item slot is filled with an action button, which generates a <b>Click</b> event on behalf of the
	split button. The shadow split button's drop down content provides the user with a convenient way to quickly
	select a shadow.
</p>
";
		}

		#endregion

		#region Event Handlers

		private void OnShadowSplitButtonSelectedValueChanged(NValueChangeEventArgs arg)
		{
            NAutomaticValue<NShadow> selectedValue = (NAutomaticValue<NShadow>)arg.NewValue;
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

			m_EventsLog.LogEvent("Selected shadow: " + str);
		}

		#endregion

		#region Fields

		private NExampleEventsLog m_EventsLog;
		private NShadowSplitButton m_ShadowSplitButton;

		#endregion

		#region Schema

		public static readonly NSchema NShadowSplitButtonExampleSchema;

		#endregion
	}
}