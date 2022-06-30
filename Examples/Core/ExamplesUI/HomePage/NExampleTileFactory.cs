using Nevron.Nov.UI;

namespace Nevron.Nov.Examples
{
	internal class NExampleTileFactory : NWidgetFactory<NKeyValuePair<string, NWidget>>
	{
		public override string GetString(NKeyValuePair<string, NWidget> item)
		{
			return item.Key;
		}
		public override NWidget CreateWidget(NKeyValuePair<string, NWidget> item)
		{
			return (NWidget)item.Value.DeepClone();
		}
	}
}