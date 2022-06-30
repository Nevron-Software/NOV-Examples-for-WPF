using Nevron.Nov.Dom;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NGroupBoxExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NGroupBoxExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NGroupBoxExample()
		{
			NGroupBoxExampleSchema = NSchema.Create(typeof(NGroupBoxExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_ContentPanel = new NStackPanel();
			m_ContentPanel.VerticalSpacing = 3;

			// Create the first group box
			NGroupBox groupBox1 = new NGroupBox("Group Box 1");
			m_ContentPanel.Add(groupBox1);

			NButton button = new NButton("Button inside group box");
			groupBox1.Content = button;

			// Create the second group box
			NGroupBox groupBox2 = new NGroupBox("Group Box 2 - Centered Header");
			groupBox2.Header.HorizontalPlacement = ENHorizontalPlacement.Center;
			m_ContentPanel.Add(groupBox2);

			NStackPanel stack1 = new NStackPanel();
			groupBox2.Content = stack1;

			stack1.Add(new NLabel("Label 1 in stack"));
			stack1.Add(new NLabel("Label 2 in stack"));
			stack1.Add(new NLabel("Label 3 in stack"));

            // Create the third group box
            NGroupBox groupBox3 = new NGroupBox("Group Box 3 - Expandable");
            groupBox3.Expandable = true;
            groupBox3.Content = new NImageBox(NResources.Image_Artistic_FishBowl_jpg);
            m_ContentPanel.Add(groupBox3);

			return m_ContentPanel;
		}
		protected override NWidget CreateExampleControls()
		{
			NCheckBox checkBox = new NCheckBox("Enabled", true);
			checkBox.VerticalPlacement = ENVerticalPlacement.Top;
			checkBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnEnabledCheckBoxCheckedChanged);

			return checkBox;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create a group box. The group box is a widget that consists of 2 widgets –
	<b>Header</b> and <b>Content</b>.
</p>
";
		}

		#endregion

		#region Event Handlers

		private void OnEnabledCheckBoxCheckedChanged(NValueChangeEventArgs args)
		{
			m_ContentPanel.Enabled = (bool)args.NewValue;
		}

		#endregion

		#region Fields

		private NStackPanel m_ContentPanel; 

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NGroupBoxExample.
		/// </summary>
		public static readonly NSchema NGroupBoxExampleSchema;

		#endregion
	}
}