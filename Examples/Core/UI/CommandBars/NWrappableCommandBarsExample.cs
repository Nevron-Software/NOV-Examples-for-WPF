using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NWrappableCommandBarsExample : NExampleBase
	{		
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NWrappableCommandBarsExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NWrappableCommandBarsExample()
		{
			NWrappableCommandBarsExampleSchema = NSchema.Create(typeof(NWrappableCommandBarsExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NCommandBarManager manager = new NCommandBarManager();

			// create two lanes
			NCommandBarLane lane0 = new NCommandBarLane();
			manager.TopDock.Add(lane0);

			// create some toolbars in the second lane
			for (int i = 0; i < 10; i++)
			{
				NToolBar toolBar = new NToolBar();
				lane0.Add(toolBar);
				toolBar.Text = "Bar" + i.ToString();

				for (int j = 0; j < 8; j++)
				{
					string name = "BTN " + i.ToString() + "." + j.ToString();

					NWidget item;
					if (j == 2)
					{
						item = new NColorBox();
					}
					else if (j == 3)
					{
						NMenuSplitButton msb = new NMenuSplitButton();
						msb.ActionButton.Content = NWidget.FromObject("Send/Receive");
						msb.Menu.Items.Add(new NMenuItem("Send Receive All"));
						msb.Menu.Items.Add(new NMenuItem("Send All"));
						msb.Menu.Items.Add(new NMenuItem("Receive All"));
						item = msb;
					}
					else if (j == 4)
					{
						NComboBox comboBox = new NComboBox();
						comboBox.Items.Add(new NComboBoxItem("Item 1"));
						comboBox.Items.Add(new NComboBoxItem("Item 2"));
						comboBox.Items.Add(new NComboBoxItem("Item 3"));
						comboBox.Items.Add(new NComboBoxItem("Item 4"));
						item = comboBox;
					}
					else
					{
						item = new NButton(name);
					}

					NCommandBar.SetText(item, name);
					toolBar.Items.Add(item);

					if (j == 2 || j == 6)
					{
						toolBar.Items.Add(new NCommandBarSeparator());
					}
				}

				if (i == 2)
				{
					toolBar.Wrappable = true;
				}
			}

			manager.Content = new NLabel("Content Goes Here");
			manager.Content.AllowFocus = true;
			manager.Content.MouseDown += new Function<NMouseButtonEventArgs>(OnContentMouseDown);
			manager.Content.Border = NBorder.CreateFilledBorder(NColor.Black);
			manager.Content.BackgroundFill = new NColorFill(NColor.White);
			manager.Content.BorderThickness = new NMargins(1);
			manager.Content.GotFocus += new Function<NFocusChangeEventArgs>(OnContentGotFocus);
			manager.Content.LostFocus += new Function<NFocusChangeEventArgs>(OnContentLostFocus);

			return manager;
		}
		protected override NWidget CreateExampleControls()
		{
			return null;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates multiline and wrappable toolbars.
</p>
";
		}

		#endregion
		
		#region Event Handlers

		private void OnContentLostFocus(NFocusChangeEventArgs args)
		{
			(args.TargetNode as NLabel).Border = NBorder.CreateFilledBorder(NColor.Black);
		}
		private void OnContentGotFocus(NFocusChangeEventArgs args)
		{
			(args.TargetNode as NLabel).Border = NBorder.CreateFilledBorder(NColor.Red);
		}
		private void OnContentMouseDown(NMouseButtonEventArgs args)
		{
			(args.TargetNode as NLabel).Focus();
		}

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NWrappableCommandBarsExample.
		/// </summary>
		public static readonly NSchema NWrappableCommandBarsExampleSchema;

		#endregion
	}
}
