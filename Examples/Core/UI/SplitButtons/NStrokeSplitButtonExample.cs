using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NStrokeSplitButtonExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NStrokeSplitButtonExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NStrokeSplitButtonExample()
		{
			NStrokeSplitButtonExampleSchema = NSchema.Create(typeof(NStrokeSplitButtonExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_StrokeSplitButton = new NStrokeSplitButton();
			m_StrokeSplitButton.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_StrokeSplitButton.VerticalPlacement = ENVerticalPlacement.Top;
			m_StrokeSplitButton.SelectedValueChanged += OnStrokeSplitButtonSelectedValueChanged;
			return m_StrokeSplitButton;
		}
		protected override NWidget CreateExampleControls()
		{
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_StrokeSplitButton).CreatePropertyEditors(
				m_StrokeSplitButton,
				NStrokeSplitButton.EnabledProperty,
				NStrokeSplitButton.HorizontalPlacementProperty,
				NStrokeSplitButton.VerticalPlacementProperty,
				NStrokeSplitButton.DropDownButtonPositionProperty,
				NStrokeSplitButton.HasAutomaticButtonProperty,
				NStrokeSplitButton.HasNoneButtonProperty,
				NStrokeSplitButton.HasMoreOptionsButtonProperty
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
	This example demonstrates how to create and use stroke split buttons. Split buttons are drop down edits,
	whose item slot is filled with an action button, which generates a <b>Click</b> event on behalf of the
	split button. The stroke split button drop down content provides the user with a convenient way to quickly
	select a stroke.
</p>
";
		}

		#endregion

		#region Event Handlers

		private void OnStrokeSplitButtonSelectedValueChanged(NValueChangeEventArgs arg)
		{
            NAutomaticValue<NStroke> selectedValue = (NAutomaticValue<NStroke>)arg.NewValue;
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

			m_EventsLog.LogEvent("Selected stroke: " + str);
		}

		#endregion

		#region Fields

		private NExampleEventsLog m_EventsLog;
		private NStrokeSplitButton m_StrokeSplitButton;

		#endregion

		#region Schema

		public static readonly NSchema NStrokeSplitButtonExampleSchema;

		#endregion
	}
}