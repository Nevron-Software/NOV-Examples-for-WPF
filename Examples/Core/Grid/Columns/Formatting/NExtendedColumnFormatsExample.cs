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
    public class NExtendedColumnFormatsExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NExtendedColumnFormatsExample()
        {
        }
        /// <summary>
        /// Static constructor.
        /// </summary>
        static NExtendedColumnFormatsExample()
        {
            NExtendedColumnFormatsExampleSchema = NSchema.Create(typeof(NExtendedColumnFormatsExample), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            // create a view and get its grid
            NTableGridView view = new NTableGridView();
            NTableGrid grid = view.Grid;

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
                    NProgressBarColumnFormat progressBarColumnFormat = new NProgressBarColumnFormat();
                    progressBarColumnFormat.Minimum = Convert.ToDouble(min);
                    progressBarColumnFormat.Maximum = Convert.ToDouble(max);
                    args.DataColumn.Format = progressBarColumnFormat;
                }
            };

            grid.DataSource = NDummyDataSource.CreatePersonsOrdersDataSource();
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
    Demonstrates the extended column formats.
</p>
<p>
    Extended column formats are such column formats that the grid does not use by default. 
    It is up to the developer to manually assign the extended column format to specific columns, as the grid will not automatically assign them.
</p>
<p>
    In this example the Price column is displayed by the <b>NProgressBarColumnFormat</b>, that is an extended column format.
<p>
";
        }

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NExtendedColumnFormatsExample.
        /// </summary>
        public static readonly NSchema NExtendedColumnFormatsExampleSchema;

        #endregion
    }
}