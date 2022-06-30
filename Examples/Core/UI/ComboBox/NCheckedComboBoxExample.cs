using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NCheckedComboBoxExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NCheckedComboBoxExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NCheckedComboBoxExample()
		{
			NCheckedComboBoxExampleSchema = NSchema.Create(typeof(NCheckedComboBoxExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NStackPanel stack = new NStackPanel();
			stack.VerticalPlacement = ENVerticalPlacement.Top;
			stack.HorizontalPlacement = ENHorizontalPlacement.Left;
			stack.Padding = new NMargins(5);

			NLabel headerLabel = new NLabel("Geography Test");
			headerLabel.HorizontalPlacement = ENHorizontalPlacement.Center;
			NStylePropertyEx.SetRelativeFontSize(headerLabel, ENRelativeFontSize.XXLarge);  // was huge
			stack.Add(headerLabel);

			NLabel contentLabel = new NLabel("Place a check on the countries located in Europe and select the one whose capital is Berlin:");
			NStylePropertyEx.SetRelativeFontSize(contentLabel, ENRelativeFontSize.Large); // was medium
			stack.Add(contentLabel);

			// Create a combo box with check boxes
			NCheckedComboBox comboBox = new NCheckedComboBox();
			comboBox.HorizontalPlacement = ENHorizontalPlacement.Left;

			comboBox.AddCheckBoxItem("Argentina", false);
			comboBox.AddCheckBoxItem("Bulgaria", true);
			comboBox.AddCheckBoxItem("Canada", false);
			comboBox.AddCheckBoxItem("Germany", true);
			comboBox.AddCheckBoxItem("Japan", false);
			comboBox.AddCheckBoxItem("Mexico", false);
			comboBox.AddCheckBoxItem("Spain", true);
			comboBox.AddCheckBoxItem("USA", false);

			stack.Add(comboBox);

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
	This example demonstrates how to create a class that inherits from NComboBox and has check boxes in the combo box items.
</p>
";
		}

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NCheckedComboBoxExample.
		/// </summary>
		public static readonly NSchema NCheckedComboBoxExampleSchema;

		#endregion
	}
}