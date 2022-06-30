using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NNamedColorPickerExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NNamedColorPickerExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NNamedColorPickerExample()
		{
			NNamedColorPickerExampleSchema = NSchema.Create(typeof(NNamedColorPickerExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_NamedColorPicker = new NNamedColorPicker();
			m_NamedColorPicker.PreferredWidth = 300;
			m_NamedColorPicker.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_NamedColorPicker.VerticalPlacement = ENVerticalPlacement.Fit;
			m_NamedColorPicker.SelectedColorChanged += OnNamedColorPickerSelectedColorChanged;

			return m_NamedColorPicker;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.Last;
			stack.FitMode = ENStackFitMode.Last;

			// add come property editors
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_NamedColorPicker).CreatePropertyEditors(
				m_NamedColorPicker,
				NNamedColorPicker.EnabledProperty,
				NNamedColorPicker.HorizontalPlacementProperty,
				NNamedColorPicker.VerticalPlacementProperty
			);

			for (int i = 0; i < editors.Count; i++)
			{
				stack.Add(editors[i]);
			}

			// create the events log
			m_EventsLog = new NExampleEventsLog();
			stack.Add(m_EventsLog);

			return new NUniSizeBoxGroup(stack);
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create and use a NOV named color picker.
</p>
";
		}

		#endregion

		#region Event Handlers

		private void OnNamedColorPickerSelectedColorChanged(NColor arg)
		{
			m_EventsLog.LogEvent(NColor.GetNameOrHex(arg));
		}

		#endregion

		#region Fields

		private NNamedColorPicker m_NamedColorPicker;
		private NExampleEventsLog m_EventsLog;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NNamedColorPickerExample.
		/// </summary>
		public static readonly NSchema NNamedColorPickerExampleSchema;

		#endregion
	}
}