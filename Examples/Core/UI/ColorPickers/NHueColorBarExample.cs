using Nevron.Nov.Dom;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;
using Nevron.Nov.Editors;
using Nevron.Nov.DataStructures;

namespace Nevron.Nov.Examples.UI
{
	public class NHueColorBarExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NHueColorBarExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NHueColorBarExample()
		{
			NHueColorBarExampleSchema = NSchema.Create(typeof(NHueColorBarExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_HueColorBar = new NHueColorBar();
			m_HueColorBar.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_HueColorBar.VerticalPlacement = ENVerticalPlacement.Top;
			m_HueColorBar.SelectedValueChanged += new Function<NValueChangeEventArgs>(OnHueColorBarSelectedValueChanged);
			return m_HueColorBar;
		}
		protected override NWidget CreateExampleControls()
		{
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_HueColorBar).CreatePropertyEditors(
				m_HueColorBar,
				NHueColorBar.UpdateWhileDraggingProperty,
				NHueColorBar.SelectedValueProperty,
				NHueColorBar.OrientationProperty,
				NHueColorBar.ValueSelectorExtendPercentProperty
			);

			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.Last;
			stack.FitMode = ENStackFitMode.Last;

			for (int i = 0, editorsCount = editors.Count; i < editorsCount; i++)
			{
				stack.Add(editors[i]);
			}

			// Create an events log
			m_EventsLog = new NExampleEventsLog();
			stack.Add(m_EventsLog);

			return new NUniSizeBoxGroup(stack);
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create and use a Hue Color Bar. The Hue Color Bar is a color bar that lets the user select
	the hue component of a color. You can control its appearance and behavior using the controls to the right.
</p>
";
		}

		#endregion

		#region Event Handlers

		private void OnHueColorBarSelectedValueChanged(NValueChangeEventArgs args)
		{
			m_EventsLog.LogEvent("Selected Hue: " + args.NewValue.ToString());
		}

		#endregion

		#region Fields

		private NHueColorBar m_HueColorBar;
		private NExampleEventsLog m_EventsLog;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NHueColorBarExample.
		/// </summary>
		public static readonly NSchema NHueColorBarExampleSchema;

		#endregion
	}
}