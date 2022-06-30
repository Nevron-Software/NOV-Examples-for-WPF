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
    public class NExtendedColumnEditorsExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NExtendedColumnEditorsExample()
        {
        }
        /// <summary>
        /// Static constructor.
        /// </summary>
        static NExtendedColumnEditorsExample()
        {
            NExtendedColumnEditorsExampleSchema = NSchema.Create(typeof(NExtendedColumnEditorsExample), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            // create a view and get its grid
            NTableGridView view = new NTableGridView();
            NTableGrid grid = view.Grid;

            grid.AllowEdit = true;

            // create persons order data source
            NDataSource personOrders = NDummyDataSource.CreatePersonsOrdersDataSource();

            // get the min and max price. We will use it in the progress bars.
            object min, max;
            personOrders.TryGetMin("Price", out min);
            personOrders.TryGetMax("Price", out max);

            grid.AutoCreateColumn += delegate(NAutoCreateColumnEventArgs args)
            {
                if (args.FieldInfo.Name == "Price")
                {
                    // create a progress bar column format for the Price field
                    NSliderColumnEditor sliderColumnEditor = new NSliderColumnEditor();
                    args.DataColumn.Editor = sliderColumnEditor;
                    args.DataColumn.WidthMode = ENColumnWidthMode.Fixed;
                    args.DataColumn.FixedWidth = 150;
                }
            };

            grid.DataSource = personOrders;
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
    Demonstrates the extended column editors. 
</p>
<p>
    Extended column editors are editors, which are not automatically assigned to data columns during data binding.
    Instead it is up to the user to assign these editors to specific columns.
</p>
<p>
    In this example we have assigned the <b>NSliderColumnEditor</b> to the <b>Price</b> column.
</p>
";
        }

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NExtendedColumnEditorsExample.
        /// </summary>
        public static readonly NSchema NExtendedColumnEditorsExampleSchema;

        #endregion
    }
}
