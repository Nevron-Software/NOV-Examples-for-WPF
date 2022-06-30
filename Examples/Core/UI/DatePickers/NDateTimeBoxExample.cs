using System;
using System.Collections.Generic;
using System.Globalization;

using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NDateTimeBoxExample : NExampleBase
	{
		#region Constructors

		public NDateTimeBoxExample()
		{
		}
		static NDateTimeBoxExample()
		{
			NDateTimeBoxExampleSchema = NSchema.Create(typeof(NDateTimeBoxExample), NExampleBase.NExampleBaseSchema);

			// Fill the list of cultures
			string[] cultureNames = new string[] { "en-US", "en-GB", "fr-FR", "de-DE", "es-ES", "ru-RU", "zh-CN", "ja-JP",
				"it-IT", "hi-IN", "ar-AE", "he-IL", "id-ID", "ko-KR", "pt-BR", "sv-SE", "tr-TR", "pt-BR", "bg-BG", "ro-RO",
				"pl-PL", "nl-NL", "cs-CZ" };
			Cultures = new NList<CultureInfo>();

			for (int i = 0, count = cultureNames.Length; i < count; i++)
			{
				CultureInfo cultureInfo;
				try
				{
					cultureInfo = new CultureInfo(cultureNames[i]);
				}
				catch
				{
					cultureInfo = null;
				}

				if (cultureInfo != null && Cultures.Contains(cultureInfo) == false)
				{
					Cultures.Add(cultureInfo);
				}
			}

			// Sort the cultures by their English name
			Cultures.Sort(new NCultureNameComparer());
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_DateTimeBox = new NDateTimeBox();
			m_DateTimeBox.VerticalPlacement = ENVerticalPlacement.Top;
			m_DateTimeBox.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_DateTimeBox.SelectedDateChanged += new Function<NValueChangeEventArgs>(OnDateTimeBoxSelectedColorChanged);

			return m_DateTimeBox;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.Last;
			stack.FitMode = ENStackFitMode.Last;

			NGroupBox groupBox = new NGroupBox("Culture:");
			stack.Add(groupBox);
			groupBox.Margins = new NMargins(0, 0, 0, 10);

			// add the cultures combo box
			int selectedIndex = -1;
			NComboBox combo = new NComboBox();
			for (int i = 0, count = Cultures.Count; i < count; i++)
			{
				CultureInfo culture = Cultures[i];
				NComboBoxItem item = new NComboBoxItem(culture.EnglishName);
				item.Tag = culture.Name;
				combo.Items.Add(item);

				if (culture.Name == m_DateTimeBox.CultureName)
				{
					selectedIndex = i;
				}
			}

			groupBox.Content = combo;
			combo.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnCultureComboSelectedIndexChanged);
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_DateTimeBox).CreatePropertyEditors(
				m_DateTimeBox,
				NDateTimeBox.EnabledProperty,
				NDateTimeBox.HighlightTodayProperty,
				NDateTimeBox.HasTodayButtonProperty,
				NDateTimeBox.ModeProperty,
				NDateTimeBox.FormatProperty,
				NDateTimeBox.SelectedDateProperty
			);

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
	This example demonstrates how to create and configure a date time box. Using the controls on the right you can
	modify various aspects of the date time box and its drop down calendar's appearance and behavior. NOV date time
	box is fully localizable, you can test how it looks for different cultures by selecting one from the cultures
	combo box on the right.
</p>
";
		}

		#endregion

		#region Event Handlers

		private void OnDateTimeBoxSelectedColorChanged(NValueChangeEventArgs args)
		{
			DateTime newDate = (DateTime)args.NewValue;
			m_EventsLog.LogEvent(newDate.ToString());
		}
		private void OnCultureComboSelectedIndexChanged(NValueChangeEventArgs args)
		{
			NComboBox combo = args.TargetNode as NComboBox;

			NComboBoxItem selectedItem = combo.SelectedItem;
			if (selectedItem == null)
				return;

			m_DateTimeBox.CultureName = (string)selectedItem.Tag;
		}

		#endregion

		#region Fields

		private NExampleEventsLog m_EventsLog;
		private NDateTimeBox m_DateTimeBox;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NDateTimeBoxExample.
		/// </summary>
		public static readonly NSchema NDateTimeBoxExampleSchema;

		#endregion

		#region Constants

		private static readonly NList<CultureInfo> Cultures;

		#endregion

		#region Nested Types

		private class NCultureNameComparer : IComparer<CultureInfo>
		{
			public int Compare(CultureInfo culture1, CultureInfo culture2)
			{
				return culture1.EnglishName.CompareTo(culture2.EnglishName);
			}
		}

		#endregion
	}
}