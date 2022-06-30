using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NTopLevelWindowPropertiesExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NTopLevelWindowPropertiesExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NTopLevelWindowPropertiesExample()
		{
			NTopLevelWindowPropertiesExampleSchema = NSchema.Create(typeof(NTopLevelWindowPropertiesExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create and initialize a top level window
			m_Window = new NTopLevelWindow();
			m_Window.Title = "Top Level Window";
			m_Window.RemoveFromParentOnClose = true;
			m_Window.AllowXResize = true;
			m_Window.AllowYResize = true;
			m_Window.PreferredSize = new NSize(300, 300);
			m_Window.QueryManualStartPosition += new Function<NEventArgs>(OnWindowQueryManualStartPosition);
			m_Window.Closed += new Function<NEventArgs>(OnWindowClosed);

			// Create the top level window's content
			NStackPanel stack = new NStackPanel();
			stack.FitMode = ENStackFitMode.First;
			stack.FillMode = ENStackFillMode.First;

			NLabel label = new NLabel("This is a top level window.");
			label.HorizontalPlacement = ENHorizontalPlacement.Center;
			label.VerticalPlacement = ENVerticalPlacement.Center;
			stack.Add(label);

			NButton closeButton = new NButton("Close");
			closeButton.HorizontalPlacement = ENHorizontalPlacement.Center;
			closeButton.Click += new Function<NEventArgs>(OnCloseButtonClick);
			stack.Add(closeButton);
			m_Window.Content = stack;

			// Create example content
			m_SettingsStack = new NStackPanel();
			m_SettingsStack.HorizontalPlacement = ENHorizontalPlacement.Left;

			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_Window).CreatePropertyEditors(m_Window,
				NTopLevelWindow.TitleProperty,
				NTopLevelWindow.StartPositionProperty,
				NTopLevelWindow.XProperty,
				NTopLevelWindow.YProperty,
                NStylePropertyEx.ExtendedLookPropertyEx,

				NTopLevelWindow.ModalProperty,
				NTopLevelWindow.ShowInTaskbarProperty,
				NTopLevelWindow.ShowTitleBarProperty,
				NTopLevelWindow.ShowControlBoxProperty,

				NTopLevelWindow.AllowMinimizeProperty,
				NTopLevelWindow.AllowMaximizeProperty,
				NTopLevelWindow.AllowXResizeProperty,
				NTopLevelWindow.AllowYResizeProperty
			);

            // Change the text of the extended look property editor
            label = (NLabel)editors[4].GetFirstDescendant(new NInstanceOfSchemaFilter(NLabel.NLabelSchema));
            label.Text = "Extended Look:";

            // Add the created property editors to the stack
			for (int i = 0, count = editors.Count; i < count; i++)
			{
				m_SettingsStack.Add(editors[i]);
			}

			// Create a button that opens the window
			NButton openWindowButton = new NButton("Open Window...");
            openWindowButton.Content.HorizontalPlacement = ENHorizontalPlacement.Center;
			openWindowButton.Click += new Function<NEventArgs>(OnOpenWindowButtonClick);
			m_SettingsStack.Add(openWindowButton);

			return new NUniSizeBoxGroup(m_SettingsStack);
		}	
		protected override NWidget CreateExampleControls()
		{
			return null;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create, configure and open top level windows. Use the controls
	above to configure the properties of the top level window and then click ""Open Window...""
	button to show it.
</p>
";
		}
		protected internal override void OnClosing()
		{
			base.OnClosing();

			m_Window.Close();
		}

		#endregion

		#region Event Handlers

		private void OnOpenWindowButtonClick(NEventArgs args)
		{
			DisplayWindow.Windows.Add(m_Window);
			m_Window.Open();
			m_SettingsStack.Enabled = false;
		}
		private void OnCloseButtonClick(NEventArgs args)
		{
			m_Window.Close();
		}
		private void OnWindowQueryManualStartPosition(NEventArgs args)
		{
			// Get the top level window which queries for position
			NTopLevelWindow window = (NTopLevelWindow)args.TargetNode;

			// Set the top level window bounds (in DIPs)
			window.Bounds = new NRectangle(window.X, window.Y, window.DefaultWidth, window.DefaultHeight);
		}
		private void OnWindowClosed(NEventArgs args)
		{
			m_SettingsStack.Enabled = true;
		}

		#endregion

		#region Fields

		private NTopLevelWindow m_Window;
		private NStackPanel m_SettingsStack;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NTopLevelWindowPropertiesExample.
		/// </summary>
		public static readonly NSchema NTopLevelWindowPropertiesExampleSchema;

		#endregion
	}
}