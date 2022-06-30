using System;
using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NHexagonColorPickerExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NHexagonColorPickerExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NHexagonColorPickerExample()
		{
			NHexagonColorPickerExampleSchema = NSchema.Create(typeof(NHexagonColorPickerExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_HexagonColorPicker = new NHexagonColorPicker();
			m_HexagonColorPicker.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_HexagonColorPicker.VerticalPlacement = ENVerticalPlacement.Top;
			m_HexagonColorPicker.SelectedIndexChanged += OnHexagonColorPickerSelectedIndexChanged;

			return m_HexagonColorPicker;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.Last;
			stack.FitMode = ENStackFitMode.Last;

			// Add some property editors
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_HexagonColorPicker).CreatePropertyEditors(
				m_HexagonColorPicker,
				NHexagonColorPicker.EnabledProperty,
				NHexagonColorPicker.SelectedIndexProperty,
				NHexagonColorPicker.HorizontalPlacementProperty,
				NHexagonColorPicker.VerticalPlacementProperty
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
	This example demonstrates how to create a hexagon color picker. The hexagon color picker is color picker
	that lets the user pick a color from a set of standard colors. The selected color can be get or set
	through the <b>SelectedColor</b> property of the picker.
</p>
<p>
	The desired size of the picker is determined by the desired size of a cell, which is controlled through
	the <b>CellSideLength</b> property. If the picker is larger or smaller than its desired size, its cells
	are scaled automatically to fill the available area.
</p>
";
		}

		#endregion

		#region Event Handlers

		private void OnHexagonColorPickerSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			NHexagonColorPicker colorPicker = (NHexagonColorPicker)arg.TargetNode;
			NColor selectedColor = colorPicker.SelectedColor;
			m_EventsLog.LogEvent(selectedColor.GetHEX().ToUpper());
		}

		#endregion

		#region Fields

		private NHexagonColorPicker m_HexagonColorPicker;
		private NExampleEventsLog m_EventsLog;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NHexagonColorPickerExample.
		/// </summary>
		public static readonly NSchema NHexagonColorPickerExampleSchema;

		#endregion
	}
}