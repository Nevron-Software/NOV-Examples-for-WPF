using System;
using System.Collections.Generic;
using System.Text;

using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;
using Nevron.Nov.UI.ThemeBuilder;

namespace Nevron.Nov.Examples.UI
{
	public class NThemeBuilderExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NThemeBuilderExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NThemeBuilderExample()
		{
			NThemeBuilderExampleSchema = NSchema.Create(typeof(NThemeBuilderExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NButton showThemeBuilderButton = new NButton("Open NOV Theme Builder");
			showThemeBuilderButton.Margins = new NMargins(NDesign.HorizontalSpacing, NDesign.VerticalSpacing);
			showThemeBuilderButton.HorizontalPlacement = ENHorizontalPlacement.Left;
			showThemeBuilderButton.VerticalPlacement = ENVerticalPlacement.Top;
			showThemeBuilderButton.Click += OnShowThemeBuilderButtonClick;
			NStylePropertyEx.SetRelativeFontSize(showThemeBuilderButton, ENRelativeFontSize.Large);

			return showThemeBuilderButton;
		}
		protected override NWidget CreateExampleControls()
		{
			return null;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>This example demonstrates how to open the NOV Theme Builder via code. Click the <b>Open NOV Theme Builder</b> button
to show the theme builder in a new window.</p>";
		}

		#endregion

		#region Event Handlers

		private void OnShowThemeBuilderButtonClick(NEventArgs arg)
		{
			// Create a Theme Builder widget
			NThemeBuilderWidget themeBuilderWidget = new NThemeBuilderWidget();

			// Create and open a top level window with the Theme Builder widget
			NTopLevelWindow window = NApplication.CreateTopLevelWindow(DisplayWindow);
			window.StartPosition = ENWindowStartPosition.CenterScreen;
			window.PreferredSize = new NSize(1000, 700);
			window.SetupApplicationWindow("NOV Theme Builder");
			window.Modal = true;
			window.Content = themeBuilderWidget;

			// Open the window and show the Theme Builder start up dialog
			window.Open();
			themeBuilderWidget.ShowStartUpDialog();
		}

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NThemeBuilderExample.
		/// </summary>
		public static readonly NSchema NThemeBuilderExampleSchema;

		#endregion
	}
}
