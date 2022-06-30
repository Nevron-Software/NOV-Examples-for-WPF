using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Layout;
using Nevron.Nov.Text;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NMaskedEnumDropDownExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NMaskedEnumDropDownExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NMaskedEnumDropDownExample()
		{
			NMaskedEnumDropDownExampleSchema = NSchema.Create(typeof(NMaskedEnumDropDownExample), NExampleBaseSchema);
		}

		#endregion

		#region Protected Overrides

		protected override NWidget CreateExampleContent()
		{
			NMaskedEnumDropDown dropDown = new NMaskedEnumDropDown();
			dropDown.VerticalPlacement = ENVerticalPlacement.Top;
			dropDown.HorizontalPlacement = ENHorizontalPlacement.Left;
			dropDown.ColumnCount = 2;
			dropDown.EnumType = NDomType.FromType(typeof(ENTableStyleOptions));
			dropDown.Initialize();
			dropDown.EnumValue = ENTableStyleOptions.FirstRow;

			return dropDown;
		}
		protected override NWidget CreateExampleControls()
		{
			return null;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to used the masked (flag) enum drop down widget to select enum flags.
</p>
";
		}

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NMaskedEnumDropDownExample.
		/// </summary>
		public static readonly NSchema NMaskedEnumDropDownExampleSchema;

		#endregion
	}
}
