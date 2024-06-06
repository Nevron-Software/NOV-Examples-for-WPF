using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Framework
{
    public class NMetafileDecoderExample : NExampleBase
    {
        #region Constructors

        public NMetafileDecoderExample()
        {
        }
        static NMetafileDecoderExample()
        {
            NMetafileDecoderExampleSchema = NSchema.Create(typeof(NMetafileDecoderExample), NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            // Show the metafile image in an image box
            m_ImageBox = new NImageBox(NResources.Image_FishBowl_wmf);
            m_ImageBox.HorizontalPlacement = ENHorizontalPlacement.Fit;
            m_ImageBox.VerticalPlacement = ENVerticalPlacement.Fit;
            m_ImageBox.SetBorder(1, NColor.Red);

            return m_ImageBox;
        }
        protected override NWidget CreateExampleControls()
        {
            NList<NPropertyEditor> propertyEditors = NDesigner.GetDesigner(m_ImageBox).CreatePropertyEditors(m_ImageBox,
                NImageBox.HorizontalPlacementProperty,
                NImageBox.VerticalPlacementProperty);

            NStackPanel stack = new NStackPanel();
            for (int i = 0; i < propertyEditors.Count; i++)
            {
                stack.Add(propertyEditors[i]);
            }

            return new NUniSizeBoxGroup(stack);
        }
        protected override string GetExampleDescription()
        {
            return @"
<p>
	This example demonstrates NOV's Windows Metafile (WMF) and Enhanced Metafile (EMF) image decoding capabilities.
</p>
";
        }

        #endregion

        #region Fields

        private NImageBox m_ImageBox;

        #endregion

        #region Schema

        public static readonly NSchema NMetafileDecoderExampleSchema;

        #endregion
    }
}