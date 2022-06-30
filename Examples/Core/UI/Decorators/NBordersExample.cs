using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
    public class NBordersExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NBordersExample()
        {
        }
        /// <summary>
        /// Static constructor.
        /// </summary>
        static NBordersExample()
        {
            NBordersExampleSchema = NSchema.Create(typeof(NBordersExample), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            // Create a table layout panel
            NTableFlowPanel table = new NTableFlowPanel();
            table.Padding = new NMargins(10);
            table.BackgroundFill = new NColorFill(NColor.White);
            table.MaxOrdinal = 3;
            table.HorizontalSpacing = 10;
            table.VerticalSpacing = 10;
            table.ColFillMode = ENStackFillMode.Equal;
            table.ColFitMode = ENStackFitMode.Equal;
            table.RowFitMode = ENStackFitMode.Equal;
            table.RowFillMode = ENStackFillMode.None;
            table.UniformWidths = ENUniformSize.Max;
            table.UniformHeights = ENUniformSize.Max;

            // add some predefined borders
            // 3D Borders
            NUIThemeColorMap map = new NUIThemeColorMap(ENUIThemeScheme.WindowsClassic);
            table.Add(CreateBorderedWidget("3D Border",
                NBorder.Create3DBorder(NColor.Green.Lighten().Lighten(), NColor.Green.Lighten(), NColor.Green.Darken(), NColor.Green)
            ));
            table.Add(CreateBorderedWidget("Raised Border (using Theme Colors)", 
                NBorder.CreateRaised3DBorder(map)
            ));
            table.Add(CreateBorderedWidget("Sunken Border (using Theme Colors)",
                NBorder.CreateSunken3DBorder(map)
            ));
            
            // Filled Borders
            table.Add(CreateBorderedWidget("Solid Color", 
                NBorder.CreateFilledBorder(NColor.Red)
            ));
            table.Add(CreateBorderedWidget("Solid Color With Rounded Corners", 
                NBorder.CreateFilledBorder(NColor.Blue, 10, 13)
            ));
            table.Add(CreateBorderedWidget("Gradient Filling With Outline",
                NBorder.CreateFilledBorder(NFill.CreatePredefined(ENPredefinedFillPattern.GradientVertical, NColor.Red, NColor.Blue), new NStroke(1, NColor.Green), new NStroke(1, NColor.Green))
                ));

            // Outer Outline Borders
            table.Add(CreateBorderedWidget("Outer Outline Border",
                NBorder.CreateOuterOutlineBorder(new NStroke(1, NColor.Red, ENDashStyle.Dash))
            ));
            table.Add(CreateBorderedWidget("Outer Outline Border with Rounding",
                NBorder.CreateOuterOutlineBorder(new NStroke(1, NColor.Red, ENDashStyle.Dash), 10)
            ));
            table.Add(CreateBorderedWidget("Outer Outline Border",
                NBorder.CreateOuterOutlineBorder(new NStroke(1, NColor.Red, ENDashStyle.Dash))
            ));
            table.Add(CreateBorderedWidget("Outer Outline Border with Rounding",
                NBorder.CreateOuterOutlineBorder(new NStroke(1, NColor.Red, ENDashStyle.Dash), 10)
            ));

            // Inner Outline Borders
            table.Add(CreateBorderedWidget("Inner Outline Border",
                NBorder.CreateInnerOutlineBorder(new NStroke(1, NColor.Red, ENDashStyle.Dash))
            ));
            table.Add(CreateBorderedWidget("Inner Outline Border with Rounding",
                NBorder.CreateInnerOutlineBorder(new NStroke(1, NColor.Green, ENDashStyle.Dash), 10)
            ));
            table.Add(CreateBorderedWidget("Inner Outline Border",
                NBorder.CreateInnerOutlineBorder(new NStroke(1, NColor.Green, ENDashStyle.Dash))
            ));
            table.Add(CreateBorderedWidget("Inner Outline Border with Rounding",
                NBorder.CreateInnerOutlineBorder(new NStroke(1, NColor.Green, ENDashStyle.Dash), 10)
            ));

            // Double border
            table.Add(CreateBorderedWidget("Double Border",
                NBorder.CreateDoubleBorder(NColor.Blue)
            ));

            table.Add(CreateBorderedWidget("Double Border with Two Colors",
                NBorder.CreateDoubleBorder(NColor.Blue, NColor.Red)
            ));

            table.Add(CreateBorderedWidget("Double Border with Two Colors and Rounding",
                NBorder.CreateDoubleBorder(NColor.Blue, NColor.Red, 10,  12)
            ));

            // Two color borders
            table.Add(CreateBorderedWidget("Two Colors Border",
                NBorder.CreateTwoColorBorder(NColor.Blue, NColor.Red)
            ));

            table.Add(CreateBorderedWidget("Two Colors Border with Rounding",
                NBorder.CreateTwoColorBorder(NColor.Blue, NColor.Red, 10, 12)
            ));

            // Three color borders
            table.Add(CreateBorderedWidget("Three Colors Border",
                NBorder.CreateThreeColorBorder(NColor.Red, NColor.Green, NColor.Blue)
            ));

            table.Add(CreateBorderedWidget("Three Colors Border with Rounding",
                NBorder.CreateThreeColorBorder(NColor.Red, NColor.Green, NColor.Blue, 10, 12)
            ));

            return table;
        }
        protected override NWidget CreateExampleControls()
        {
            return null;
        }
        protected override string GetExampleDescription()
        {
            return @"
<p>
	This example demonstrates some of the static methods of NBorder that help you quickly create commonly used types of borders.
</p>
";
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Creates a simple label that demonstrates the specified border.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="border"></param>
        /// <returns></returns>
        private NWidget CreateBorderedWidget(string text, NBorder border)
        {
            NLabel label = new NLabel(text);
            
            label.HorizontalPlacement = ENHorizontalPlacement.Fit;
            label.VerticalPlacement = ENVerticalPlacement.Fit;
            label.Padding = new NMargins(10);

            label.Border = border;
            label.BorderThickness = new NMargins(5);

            return label;
        }

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NBordersExample.
        /// </summary>
        public static readonly NSchema NBordersExampleSchema;

        #endregion
    }
}
