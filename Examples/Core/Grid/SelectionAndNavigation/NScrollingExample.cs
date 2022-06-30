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
    public class NScrollingExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NScrollingExample()
        {
        }
        /// <summary>
        /// Static constructor.
        /// </summary>
        static NScrollingExample()
        {
            NScrollingExampleSchema = NSchema.Create(typeof(NScrollingExample), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            m_TableView = new NTableGridView();

            // create a dummy data source with many columns to demonstrate horizontal scrolling
            NMemoryDataTable dataTable = new NMemoryDataTable(new NFieldInfo[] { 
                // person info
                new NFieldInfo("Name-0", typeof(String)),
                new NFieldInfo("Gender-1", typeof(ENGender)),
                new NFieldInfo("Birthday-2", typeof(DateTime)),
                new NFieldInfo("Phone-3", typeof(String)),
                new NFieldInfo("Email-4", typeof(String)),
                // address info
                new NFieldInfo("Country-5", typeof(ENCountry)),
                new NFieldInfo("City-6", typeof(String)),
                new NFieldInfo("Address-7", typeof(String)),
                // product info
                new NFieldInfo("Product Name-8", typeof(String)),
                new NFieldInfo("Product Price-9", typeof(Double)),
                new NFieldInfo("Product Quantity-10", typeof(Int32)),
            });

            for (int i = 0; i < 1000; i++)
            {
                NDummyDataSource.NPersonInfo personInfo = NDummyDataSource.RandomPersonInfo();
                NDummyDataSource.NAddressInfo addressInfo = NDummyDataSource.RandomAddressInfo();
                NDummyDataSource.NProductInfo productInfo = NDummyDataSource.RandomProductInfo();

                dataTable.AddRow(
                    // person info
                    personInfo.Name,
                    personInfo.Gender,
                    personInfo.Birthday,
                    personInfo.Phone,
                    personInfo.Email,
                    // address
                    addressInfo.Country,
                    addressInfo.City,
                    addressInfo.Address,
                    // product
                    productInfo.Name,
                    productInfo.Price,
                    NDummyDataSource.RandomInt32(1, 100)
                    ); 
            }

            m_TableView.Grid.DataSource = new NDataSource(dataTable);
            return m_TableView;
        }
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();
            
            // create the horizontal scrolling properties
            {
                NStackPanel hstack = new NStackPanel();

                NDesigner designer = NDesigner.GetDesigner(NTableGrid.NTableGridSchema);
                NList<NPropertyEditor> editors = designer.CreatePropertyEditors(
                    m_TableView.Grid,
                    NTableGrid.HScrollModeProperty,
                    NTableGrid.IntegralHScrollProperty,
                    NTableGrid.SmallHScrollChangeProperty);

                for (int i = 0; i < editors.Count; i++)
                {
                    hstack.Add(editors[i]);
                }

                NGroupBox hgroup = new NGroupBox("Horizontal Scrolling", hstack);
                stack.Add(new NUniSizeBoxGroup(hgroup));
            }

            // create the vertical scrolling properties
            {
                NStackPanel vstack = new NStackPanel();

                NDesigner designer = NDesigner.GetDesigner(NTableGrid.NTableGridSchema);
                NList<NPropertyEditor> editors = designer.CreatePropertyEditors(
                    m_TableView.Grid,
                    NTableGrid.VScrollModeProperty,
                    NTableGrid.IntegralVScrollProperty,
                    NTableGrid.SmallVScrollChangeProperty);

                for (int i = 0; i < editors.Count; i++)
                {
                    vstack.Add(editors[i]);
                }

                NGroupBox vgroup = new NGroupBox("Vertical Scrolling", vstack);
                stack.Add(new NUniSizeBoxGroup(vgroup));
            }

            return stack;
        }
        protected override string GetExampleDescription()
        {
            return @"
<p>
    Demonstrates the properties that control the grid horizontal and vertical scrolling behavior.
</p>
<p>
    Note that the grid supports integral and pixel-wise scrolling in both the horizontal and vertical dimensions.
</p>
";
        }

        #endregion

        #region Fields

        NTableGridView m_TableView;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NScrollingExample.
        /// </summary>
        public static readonly NSchema NScrollingExampleSchema;

        #endregion
    }
}