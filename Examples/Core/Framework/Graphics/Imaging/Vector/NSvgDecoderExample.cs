using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Framework
{
    public class NSvgDecoderExample : NExampleBase
    {
        #region Constructors

        public NSvgDecoderExample()
        {
        }
        static NSvgDecoderExample()
        {
            NSvgDecoderExampleSchema = NSchema.Create(typeof(NSvgDecoderExample), NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            // Show the SVG image in an image box
            m_ImageBox = new NImageBox(NResources.Image_SVG_Tiger_svg);
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
	This example demonstrates NOV's Scalable Vector Graphics (SVG) image decoding capabilities.
</p>
";
        }

        #endregion

        #region Fields

        private NImageBox m_ImageBox;

        #endregion

        #region Schema

        public static readonly NSchema NSvgDecoderExampleSchema;

        #endregion
    }
}