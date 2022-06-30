using Nevron.Nov.Dom;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NFontNameComboBoxExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NFontNameComboBoxExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NFontNameComboBoxExample()
		{
			NFontNameComboBoxExampleSchema = NSchema.Create(typeof(NFontNameComboBoxExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Overrides - Example

		protected override NWidget CreateExampleContent()
		{
			// create the combo box
			m_FontNameComboBox = new NFontNameThumbnailComboBox();

			m_FontNameComboBox.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_FontNameComboBox.VerticalPlacement = ENVerticalPlacement.Top;
			m_FontNameComboBox.DropDownStyle = ENComboBoxStyle.DropDownList;

			// select the first item
			m_FontNameComboBox.SelectedIndex = 0;
			m_FontNameComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnFontNameChanged);

			return m_FontNameComboBox;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			
			// Create the commands
			NLabel selectFontNameTitle = new NLabel("Selected Font Name:");
			stack.Add(selectFontNameTitle);

			m_SelectFontName = new NLabel("");
			stack.Add(m_SelectFontName);

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to use the built-in font name combo box, which allows the user to select a font name from the list of available fonts.
</p>
";
		}

		#endregion

		#region Event Handlers

		private void OnFontNameChanged(NValueChangeEventArgs args)
		{
			NFontNameThumbnailComboBox comboBox = (NFontNameThumbnailComboBox)args.TargetNode;
			m_SelectFontName.Text = comboBox.SelectedFontName;
		}

		#endregion

		#region Fields

		private NFontNameThumbnailComboBox m_FontNameComboBox;
		private NLabel m_SelectFontName;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NFontNameComboBoxExample.
		/// </summary>
		public static readonly NSchema NFontNameComboBoxExampleSchema;

		#endregion
	}
}