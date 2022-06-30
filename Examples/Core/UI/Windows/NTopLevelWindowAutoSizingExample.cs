using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NTopLevelWindowAutoSizingExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NTopLevelWindowAutoSizingExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NTopLevelWindowAutoSizingExample()
		{
			NTopLevelWindowAutoSizingExampleSchema = NSchema.Create(typeof(NTopLevelWindowAutoSizingExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NStackPanel stack = new NStackPanel();
			
			// Create the example's content
			NButton openYAutoSizeWindowButton = new NButton("Open Y auto sizable Window...");
			openYAutoSizeWindowButton.HorizontalPlacement = ENHorizontalPlacement.Left;
			openYAutoSizeWindowButton.VerticalPlacement = ENVerticalPlacement.Top;
			openYAutoSizeWindowButton.Click += new Function<NEventArgs>(OnOpenYAutoSizeWindowButtonClick);
			stack.Add(openYAutoSizeWindowButton);

			NButton openXAutoSizeWindowButton = new NButton("Open X auto sizable Window...");
			openXAutoSizeWindowButton.HorizontalPlacement = ENHorizontalPlacement.Left;
			openXAutoSizeWindowButton.VerticalPlacement = ENVerticalPlacement.Top;
			openXAutoSizeWindowButton.Click += new Function<NEventArgs>(OnOpenXAutoSizeWindowButtonClick);
			stack.Add(openXAutoSizeWindowButton);

			NButton openAutoSizeWindowButton = new NButton("Open X and Y auto sizable and auto centered Window...");
			openAutoSizeWindowButton.HorizontalPlacement = ENHorizontalPlacement.Left;
			openAutoSizeWindowButton.VerticalPlacement = ENVerticalPlacement.Top;
			openAutoSizeWindowButton.Click += new Function<NEventArgs>(OnOpenAutoSizeWindowButtonClick);
			stack.Add(openAutoSizeWindowButton);

			return stack;
		}
		protected override NWidget CreateExampleControls()
		{
			return null;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
This example demonstrates how to create auto sizable and auto centered windows with expressions.
</p>
";
		}

		#endregion

		#region Event Handlers

		private void OnOpenXAutoSizeWindowButtonClick(NEventArgs arg)
		{
			NTopLevelWindow window = new NTopLevelWindow();
			window.Modal = true;

			// allow the user to resize the Y window dimension
			window.AllowYResize = true;

			// bind the window Width to the desired width of the window
			NBindingFx bindingFx = new NBindingFx(window, NTopLevelWindow.DesiredWidthProperty);
			bindingFx.Guard = true;
			window.SetFx(NTopLevelWindow.WidthProperty, bindingFx);

			// create a wrap flow panel with Y direction
			NWrapFlowPanel wrapPanel = new NWrapFlowPanel();
			wrapPanel.Direction = ENHVDirection.TopToBottom;
			window.Content = wrapPanel;

			for (int i = 0; i < 10; i++)
			{
				wrapPanel.Add(new NButton("Button" + i));
			}

			// open the window
			DisplayWindow.Windows.Add(window);
			window.Open();
		}
		private void OnOpenYAutoSizeWindowButtonClick(NEventArgs arg)
		{
			NTopLevelWindow window = new NTopLevelWindow();
			window.Modal = true;

			// allow the user to resize the X window dimension
			window.AllowXResize = true;

			// bind the window Height to the desired height of the window
			NBindingFx bindingFx = new NBindingFx(window, NTopLevelWindow.DesiredHeightProperty);
			bindingFx.Guard = true;
			window.SetFx(NTopLevelWindow.HeightProperty, bindingFx);

			// create a wrap flow panel (by default flows from left to right)
			NWrapFlowPanel wrapPanel = new NWrapFlowPanel();
			window.Content = wrapPanel;

			for (int i = 0; i < 10; i++)
			{
				wrapPanel.Add(new NButton("Button" + i));
			}

			// open the window
			DisplayWindow.Windows.Add(window);
			window.Open();
		}
		private void OnOpenAutoSizeWindowButtonClick(NEventArgs arg)
		{
			NTopLevelWindow window = new NTopLevelWindow();
			window.Modal = true;

			// open the window in the center of its parent, 
			window.StartPosition = ENWindowStartPosition.CenterOwnerWindow;

			// implement auto width and height sizing
			{
				// bind the window Width to the DefaultWidth of the window
				NBindingFx widthBindingFx = new NBindingFx(window, NTopLevelWindow.DefaultWidthProperty);
				widthBindingFx.Guard = true;
				window.SetFx(NTopLevelWindow.WidthProperty, widthBindingFx);

				// bind the window Height to the DefaultHeight of the window
				NBindingFx heightBindingFx = new NBindingFx(window, NTopLevelWindow.DesiredHeightProperty);
				heightBindingFx.Guard = true;
				window.SetFx(NTopLevelWindow.HeightProperty, heightBindingFx);
			}

			// implement auto center 
			{
				// scratch X and Y define the window center 
				// they are implemented by simply calculating the center X and Y via formulas
				window.SetFx(NScratchPropertyEx.XPropertyEx, "X+Width/2");
				window.SetFx(NScratchPropertyEx.YPropertyEx, "Y+Height/2");

				// now that we have an automatic center, we need to write expressions that define the X and Y from that center. 
				// These are cyclic expressions - CenterX depends on X, and X depends on CenterX.
				// The expressions that are assigned to X and Y are guarded and permeable. 
				//    guard is needed because X and Y are updated when the user moves the window around.
				//    permeable is needed to allow the X and Y values to change when the user moves the window around.
				// When the the X and Y values change -> center changes -> X and Y expressions are triggered but they produce the same X and Y results and the cycle ends.
				// When the Width and Height change -> center changes -> X and Y expression are triggered but they produce the same X and Y results and the cycle ends.
				NFormulaFx xfx = new NFormulaFx(NScratchPropertyEx.XPropertyEx.Name + "-Width/2");
				xfx.Guard = true;
				xfx.Permeable = true;
				window.SetFx(NTopLevelWindow.XProperty, xfx);

				NFormulaFx yfx = new NFormulaFx(NScratchPropertyEx.YPropertyEx.Name + "-Height/2");
				yfx.Guard = true;
				yfx.Permeable = true;
				window.SetFx(NTopLevelWindow.YProperty, yfx);
			}

			// create a dummy tab that sizes to the currently selected page,
			// and add two pages with different sizes to the tab.
			NTab tab = new NTab();
			window.Content = tab;
			tab.SizeToSelectedPage = true;

			NTabPage page1 = new NTabPage("Small Content");
			NButton btn = new NButton("I am small");
			page1.Content = btn;
			tab.TabPages.Add(page1);

			NTabPage page2 = new NTabPage("Large Content");
			NButton btn2 = new NButton("I am LARGE");
			btn2.PreferredSize = new NSize(200, 200);
			page2.Content = btn2;
			tab.TabPages.Add(page2);

			// open the window
			DisplayWindow.Windows.Add(window);
			window.Open();
		}
		
		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NTopLevelWindowAutoSizingExample.
		/// </summary>
		public static readonly NSchema NTopLevelWindowAutoSizingExampleSchema;

		#endregion
	}
}