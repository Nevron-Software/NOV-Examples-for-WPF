using System;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.DataStructures;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
    public class NTransformContentExample : NExampleBase
    {
        #region Constructors

        public NTransformContentExample()
        {
        }
        static NTransformContentExample()
        {
            NTransformContentExampleSchema = NSchema.Create(typeof(NTransformContentExample), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            // create a dummy button
            NButton button = new NButton("I can be transformed");

            m_TransformContent = new NTransformContent(button);
            m_TransformContent.BorderThickness = new NMargins(1);
            m_TransformContent.Border = NBorder.CreateFilledBorder(NColor.Red);
            return m_TransformContent;
        }
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();
            
            // Transform Properties
            {
                NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_TransformContent).CreatePropertyEditors(
                    m_TransformContent,
                    NTransformContent.ScaleXProperty,
                    NTransformContent.ScaleYProperty,
                    NTransformContent.AngleProperty,
                    NTransformContent.StretchAtRightAnglesProperty,
                    NTransformContent.HorizontalPlacementProperty,
                    NTransformContent.VerticalPlacementProperty
                );

                NStackPanel propertiesStack = new NStackPanel();
                for (int i = 0; i < editors.Count; i++)
                {
                    propertiesStack.Add(editors[i]);
                }

                stack.Add(new NGroupBox("Transform Content Properties", propertiesStack));
            }

            return stack;
        }
        protected override string GetExampleDescription()
        {
            return @"
<p>
	This example demonstrates how to create and use the NTransformContent widget. 
    This widget allows you to aggregate another widget and apply arbitrary rotation and scaling on the content.
</p>
";
        }

        #endregion

        #region Fields

        NTransformContent m_TransformContent;

        #endregion

        #region Schema

        public static readonly NSchema NTransformContentExampleSchema;

        #endregion

        #region Nested Types - NDynamicContentTooltip

        /// <summary>
        /// A tooltip that shows as content the current date and time
        /// </summary>
        public class NDynamicContentTooltip : NTooltip
        {
            #region Constructors

            public NDynamicContentTooltip()
            {

            }
            static NDynamicContentTooltip()
            {
                NDynamicContentTooltipSchema = NSchema.Create(typeof(NDynamicContentTooltip), NTooltip.NTooltipSchema);
            }

            #endregion

            #region Overrides - GetContent()

            public override NWidget GetContent()
            {
                DateTime now = DateTime.Now;
                return new NLabel("I was shown at: " + now.ToString("T"));
            }

            #endregion

            #region Schema

            public static readonly NSchema NDynamicContentTooltipSchema;

            #endregion
        }

        #endregion
    }
}