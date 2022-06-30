using Nevron.Nov.Xml;

namespace Nevron.Nov.Examples
{
	internal class NExampleTileInfo : INDeeplyCloneable
	{
		public NExampleTileInfo(NXmlElement xmlElement)
		{
			XmlElement = xmlElement;
		}

		public object DeepClone()
		{
			return new NExampleTileInfo(XmlElement);
		}

		public readonly NXmlElement XmlElement;
	}
}