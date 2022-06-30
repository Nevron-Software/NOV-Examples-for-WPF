using System;
using Nevron.Nov.Grid;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.DataStructures;
using Nevron.Nov.Data;
using Nevron.Nov.Dom;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Grid
{
    public class NColumnsConditionalFormattingExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NColumnsConditionalFormattingExample()
        {
        }
        /// <summary>
        /// Static constructor.
        /// </summary>
        static NColumnsConditionalFormattingExample()
        {
            NColumnsConditionalFormattingExampleSchema = NSchema.Create(typeof(NColumnsConditionalFormattingExample), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            // create a view and get its grid
            NTableGridView view = new NTableGridView();
            NTableGrid grid = view.Grid;

            // bind the grid to the PersonsOrders data source
            grid.DataSource = NDummyDataSource.CreatePersonsOrdersDataSource();

            // Formatting Rule 1 = applied to the Product Name column.
            // make the Aptax, Joykix, Zun Zimtam,  Dingtincof cell backgrounds LightCoral, with a bold Font
            {
                // create the formatting rule and add it in the "Product Name" column
                NColumn column = grid.Columns.GetColumnByFieldName("Product Name");

                NFormattingRule formattingRule = new NFormattingRule();
                column.FormattingRules.Add(formattingRule);

                // row condition
                NOrGroupRowCondition orCondition = new NOrGroupRowCondition();
                orCondition.Add(new NOperatorRowCondition(new NFieldRowValue("Product Name"), ENRowConditionOperator.Equals, "Aptax"));
                orCondition.Add(new NOperatorRowCondition(new NFieldRowValue("Product Name"), ENRowConditionOperator.Equals, "Joykix"));
                orCondition.Add(new NOperatorRowCondition(new NFieldRowValue("Product Name"), ENRowConditionOperator.Equals, "Zun Zimtam"));
                orCondition.Add(new NOperatorRowCondition(new NFieldRowValue("Product Name"), ENRowConditionOperator.Equals, "Dingtincof"));
                formattingRule.RowCondition = orCondition;

                // LightCoral background fill declaration
                NBackgroundFillDeclaration backgroundFillDeclaration = new NBackgroundFillDeclaration();
                backgroundFillDeclaration.Mode = ENFillDeclarationMode.Uniform;
                backgroundFillDeclaration.UniformFill = new NColorFill(NColor.LightCoral);
                formattingRule.Declarations.Add(backgroundFillDeclaration);

                // Bold font style declaration
                NFontStyleDeclaration fontStyleDeclaration = new NFontStyleDeclaration();
                fontStyleDeclaration.FontStyle = ENFontStyle.Bold;
                formattingRule.Declarations.Add(fontStyleDeclaration);
            }

            // Formatting Rule 2 = applied to the Product Name column.
            // make the Aptax and Joykix cell backgrounds LightCoral, with a bold Font
            {
                // create the formatting rule and add it in the "Product Name" column
                NColumn column = grid.Columns.GetColumnByFieldName("Price");
                NFormattingRule formattingRule = new NFormattingRule();
                column.FormattingRules.Add(formattingRule);

                // row condition
                formattingRule.RowCondition = new NTrueRowCondition();

                // get price field min and max
                object minPrice, maxPrice;
                int priceFieldIndex = grid.DataSource.GetFieldIndex("Price");
                grid.DataSource.TryGetMin(priceFieldIndex, out minPrice);
                grid.DataSource.TryGetMax(priceFieldIndex, out maxPrice);

                // make a graident fill declaration 
                NBackgroundFillDeclaration backgroundFillDeclaration = new NBackgroundFillDeclaration();
                backgroundFillDeclaration.Mode = ENFillDeclarationMode.TwoColorGradient;
                backgroundFillDeclaration.MinimumValue = Convert.ToDouble(minPrice);
                backgroundFillDeclaration.MaximumValue = Convert.ToDouble(maxPrice);
                backgroundFillDeclaration.BeginColor = NColor.Green;
                backgroundFillDeclaration.EndColor = NColor.Red;
                formattingRule.Declarations.Add(backgroundFillDeclaration);
            }

            return view;
        }

        protected override NWidget CreateExampleControls()
        {
            return null;
        }
        protected override string GetExampleDescription()
        {
            return @"
<p>
    Demonstrates the conditional column formatting.
</p>
<p>
    Conditional formatting changes the default formatting of column cells, when a certain condition is met.
    NOV Grid for .NET provides strong support for authoring complex cell conditions.
</p>
<p>
    Besides static fill rules, NOV Grid for .NET also supports gradient background and text fill declarations, 
    that can be defined as a two color or three color gradient. 
</p>
<p>
    In this example <b>Price</b> background uses a two-color gradient background fill.
    The <b>Product Name</b> is has different background fill and font style applied to certain products (<b>Aptax</b>,<b>Joykix</b>,<b>Zun Zimtam</b> and <b>Dingtincof</b>).
</p>
";
        }

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NColumnsConditionalFormattingExample.
        /// </summary>
        public static readonly NSchema NColumnsConditionalFormattingExampleSchema;

        #endregion
    }
}