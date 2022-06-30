using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NCommandingExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NCommandingExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NCommandingExample()
		{
			NCommandingExampleSchema = NSchema.Create(typeof(NCommandingExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleControls()
		{
			return null;
		}
		protected override NWidget CreateExampleContent()
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

		#region Schema

		/// <summary>
		/// Schema associated with NCommandingExample.
		/// </summary>
		public static readonly NSchema NCommandingExampleSchema;

		#endregion

		public class MyCommandableWidget : NWidget
		{
			public MyCommandableWidget()
			{
				// TODO: initialize commander here
			}
			static MyCommandableWidget()
			{
				MyCommandableWidgetSchema = NSchema.Create(typeof(MyCommandableWidget), NWidget.NWidgetSchema);
				// create a command that is associated with the Ctrl+T shortcut
				MyActionCommand = NCommand.Create(typeof(MyCommandableWidget), "MyActionCommand", "Sets a contstant text", new NShortcut(new NKey(ENKeyCode.T), ENModifierKeys.Control));
				MyToggleCommand = NCommand.Create(typeof(MyCommandableWidget), "MyToggleCommand", "Toggles the text fill", new NShortcut(new NKey(ENKeyCode.R), ENModifierKeys.Control));
			}

			public static readonly NSchema MyCommandableWidgetSchema;
			public static readonly NCommand MyActionCommand;
			public static readonly NCommand MyToggleCommand;
		}
	}
}