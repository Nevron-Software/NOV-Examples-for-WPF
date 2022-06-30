using Nevron.Nov.Dom;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;
using Nevron.Nov.Editors;
using Nevron.Nov.DataStructures;

namespace Nevron.Nov.Examples.UI
{
	public class NLuminanceColorBarExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NLuminanceColorBarExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NLuminanceColorBarExample()
		{
			NLuminanceColorBarExampleSchema = NSchema.Create(typeof(NLuminanceColorBarExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_LuminanceColorBar = new NLuminanceColorBar();
			m_LuminanceColorBar.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_LuminanceColorBar.VerticalPlacement = ENVerticalPlacement.Top;
			m_LuminanceColorBar.SelectedValueChanged += new Function<NValueChangeEventArgs>(OnLuminanceColorBarSelectedValueChanged);
			return m_LuminanceColorBar;
		}
		protected override NWidget CreateExampleControls()
		{
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_LuminanceColorBar).CreatePropertyEditors(
				m_LuminanceColorBar,
				NLuminanceColorBar.UpdateWhileDraggingProperty,
				NLuminanceColorBar.BaseColorProperty,
				NLuminanceColorBar.SelectedValueProperty,
				NLuminanceColorBar.OrientationProperty,
				NLuminanceColorBar.ValueSelectorExtendPercentProperty
			);

			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.Last;
			stack.FitMode = ENStackFitMode.Last;

			for (int i = 0, editorsCount = editors.Count; i < editorsCount; i++)
			{
				stack.Add(editors[i]);
			}

			// Modify the properties of the selected value property editor
			NSinglePropertyEditor selectedValueEditor = (NSinglePropertyEditor)editors[2];
			selectedValueEditor.Minimum = 0;
			selectedValueEditor.Maximum = 1;
			selectedValueEditor.Step = 0.01;

			// Create an events log
			m_EventsLog = new NExampleEventsLog();
			stack.Add(m_EventsLog);

			return new NUniSizeBoxGroup(stack);
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create a luminance color bar. The luminance color bar allows the user to select darker or lighter variants of a given base color.
	He can modify the luminance of the <b>BaseColor</b> value by dragging a color selector. The currently selected luminance is stored in the
	<b>SelectedValue</b> property and can be in the range [0,1].
</p>
";
		}

		#endregion

		#region Event Handlers

		private void OnLuminanceColorBarSelectedValueChanged(NValueChangeEventArgs args)
		{
			m_EventsLog.LogEvent("Selected Luminance: " + args.NewValue.ToString());
		}

		#endregion

		#region Fields

		private NLuminanceColorBar m_LuminanceColorBar;
		private NExampleEventsLog m_EventsLog;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NLuminanceColorBarExample.
		/// </summary>
		public static readonly NSchema NLuminanceColorBarExampleSchema;

		#endregion
	}
}