using Nevron.Nov.Dom;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples
{
	internal class NErrorPanel : NContentHolder
	{
		public NErrorPanel(string error)
		{
			Content = new NLabel(error);
		}
		static NErrorPanel()
		{
			NErrorPanelSchema = NSchema.Create(typeof(NErrorPanel), NContentHolderSchema);
		}

		public static readonly NSchema NErrorPanelSchema;
	}
}