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
    public class NAutomaticDataColumnsExamle : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NAutomaticDataColumnsExamle()
        {
        }
        /// <summary>
        /// Static constructor.
        /// </summary>
        static NAutomaticDataColumnsExamle()
        {
            NAutomaticDataColumnsExamleSchema = NSchema.Create(typeof(NAutomaticDataColumnsExamle), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            m_GridView = new NTableGridView();
            NTableGrid grid = m_GridView.Grid;

            NItem[] items = new NItem[]{
                new NItem(NResources.Image_CountryFlags_ad_png, NColor.Navy, new NColorFill(NColor.Moccasin), new NStroke(NColor.AntiqueWhite)),
                new NItem(NResources.Image_CountryFlags_ae_png, NColor.Olive, new NStockGradientFill(NColor.Violet, NColor.WhiteSmoke), new NStroke(NColor.Bisque)),
                new NItem(NResources.Image_CountryFlags_af_png, NColor.OldLace, new NHatchFill(ENHatchStyle.DiagonalBrick, NColor.Yellow, NColor.Red), new NStroke(NColor.DarkCyan)),
                new NItem(NResources.Image_CountryFlags_ag_png, NColor.Plum, new NImageFill(NResources.Image__16x16_Birthday_png), new NStroke(NColor.DimGray)),
                new NItem(NResources.Image_CountryFlags_ai_png, NColor.Peru,  new NStockGradientFill(ENGradientStyle.FromCenter, ENGradientVariant.Variant1, NColor.Wheat, NColor.DarkGoldenrod), new NStroke(NColor.CadetBlue))
            };

            // bind the grid to the data source
            grid.DataSource = new NDataSource(new NGenericIListDataTable<NItem>(items));
            grid.AutoCreateColumn += grid_AutoCreateColumn;
            
            return m_GridView;
        }

        void grid_AutoCreateColumn(NAutoCreateColumnEventArgs arg)
        {
            // get the data column which was automatically created
            NDataColumn dataColumn = arg.DataColumn;
        }

        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();
            return stack;
        }
        protected override string GetExampleDescription()
        {
            return @"
<p>
    Demonstrates automatically created data columns.
</p>
<p>
    Data columns are columns, which obtain and edit data from the data source.
</p>
<p>
    When the grid is bound to a data source, it will automatically create data columns for all data source fields, if the grid <b>AutoCreateColumns</b> property is true.</br>
    During this process it will also raise the <b>AutoCreateColumn</b> column event.
</p>
<p>
</p>";
        }

        #endregion

        #region Fields

        NTableGridView m_GridView;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NAutomaticDataColumnsExamle.
        /// </summary>
        public static readonly NSchema NAutomaticDataColumnsExamleSchema;

        #endregion

        public enum ENDummyEnum
        {
            EnumValue1,
            EnumValue2,
            EnumValue3
        }

        public class NItem
        {
            public NItem(NImage image, NColor color, NFill fill, NStroke stroke)
            {
                Image = image;
                Color = color;
                Fill = fill;
                Stroke = stroke;
            }

            public bool BooleanValue { get; set; }
            public byte ByteValue { get; set; }
            
            public ushort UInt16Value { get; set; }
            public uint UInt32Value { get; set; }
            public ulong UInt64Value { get; set; }

            public short Int16Value { get; set; }
            public int Int32Value { get; set; }
            public long Int64Value { get; set; }

            public float SingleValue { get; set; }
            public double DoubleValue { get; set; }
            public decimal DecimalValue { get; set; }

            public DateTime DateTimeValue { get; set; }
            public string StringValue { get; set; }
            public ENDummyEnum EnumValue { get; set; }

            public NImage Image { get; set; }
            public NColor Color { get; set; }
            public NFill Fill { get; set; }
            public NStroke Stroke { get; set; }
        }
    }
}