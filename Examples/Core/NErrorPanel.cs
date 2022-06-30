using Nevron.Nov.Dom;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples
{
	public class NErrorPanel : NContentHolder
	{
		public NErrorPanel(string error)
		{
			Content = new NLabel(error);
		}
		static NErrorPanel()
		{
			NErrorPanelSchema = NSchema.Create(typeof(NErrorPanel), NContentHolder.NContentHolderSchema);
		}

		public static readonly NSchema NErrorPanelSchema;
	}
}