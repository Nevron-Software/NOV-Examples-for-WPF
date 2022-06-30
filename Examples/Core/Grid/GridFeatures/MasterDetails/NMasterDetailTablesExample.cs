using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Grid;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Grid
{
    public class NMasterDetailTablesExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NMasterDetailTablesExample()
        {
        }
        /// <summary>
        /// Static constructor.
        /// </summary>
        static NMasterDetailTablesExample()
        {
            NMasterDetailTablesExampleSchema = NSchema.Create(typeof(NMasterDetailTablesExample), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            // create a view and get its grid
            m_View = new NTableGridView();
            NTableGrid grid = m_View.Grid;

            // bind the grid to the data source
            grid.DataSource = NDummyDataSource.CreatePersonsDataSource();

            // configure the master grid
            grid.AllowEdit = false;

            // assign some icons to the columns
            for (int i = 0; i < grid.Columns.Count; i++)
            {
                NDataColumn dataColumn = grid.Columns[i] as NDataColumn;
                if (dataColumn == null)
                    continue;

                NImage image = null;
                switch (dataColumn.FieldName)
                {
                    case "Name":
                        image = NResources.Image__16x16_Contacts_png;
                        break;
                    case "Gender":
                        image = NResources.Image__16x16_Gender_png;
                        break;
                    case "Birthday":
                        image = NResources.Image__16x16_Birthday_png;
                        break;
                    case "Country":
                        image = NResources.Image__16x16_Globe_png;
                        break;
                    case "Phone":
                        image = NResources.Image__16x16_Phone_png;
                        break;
                    case "Email":
                        image = NResources.Image__16x16_Mail_png;
                        break;
                    default:
                        continue;
                }

                // NOTE: The CreateHeaderContentDelegate is invoked whenever the Title changes or the UpdateHeaderContent() is called.
                // you can use this event to create custom column header content
                dataColumn.CreateHeaderContentDelegate = delegate(NColumn theColumn)
                {
                    NPairBox pairBox = new NPairBox(image, dataColumn.Title, ENPairBoxRelation.Box1BeforeBox2);
                    pairBox.Spacing = 2;
                    return pairBox;
                };
                dataColumn.UpdateHeaderContent();
            }

            // get the grid master details
            NMasterDetails masterDetails = grid.MasterDetails;

            // creater the table grid detail. 
            // NOTE: It shows information from the sales data source. the details are bound using field binding
            NTableGridDetail detail = new NTableGridDetail();
            masterDetails.Details.Add(detail);
            detail.DataSource = NDummyDataSource.CreatePersonsOrdersDataSource();

            // configure the details grid
            detail.GridView.Grid.AllowEdit = false;

            NRelationMasterBinding masterBinding = new NRelationMasterBinding();
            masterBinding.Relations.Add(new NRelation("Id", "PersonId"));
            detail.MasterBinding = masterBinding;

            return m_View;
        }
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();

            NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_View.Grid).CreatePropertyEditors(m_View.Grid, 
                NGrid.FrozenRowsProperty,
                NGrid.IntegralVScrollProperty);

            for (int i = 0; i < editors.Count; i++)
            {
                stack.Add(editors[i]);
            }
            
            return stack;
        }
        protected override string GetExampleDescription()
        {
            return @"
<p>
    Demonstrates how to show other tables data that is related to the master table rows.
</p>
<p>
    <b>NTableGridDetail</b> and <b>NTreeGridDetail</b> are master-details that can display a table or tree grid that display information from a slave data source.
</p>
<p>
    In this example we have created an <b>NTableGridDetail</b> detail that display information about each specific person orders.
    The master grid shows the <b>Persons</b> data source.
    The detail for each person are extracted from the <b>PersonOrders</b> data source and displayed as a table grid again.
</p>
";
        }

        #endregion

        #region Fields

        NTableGridView m_View;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NMasterDetailsExample.
        /// </summary>
        public static readonly NSchema NMasterDetailTablesExampleSchema;

        #endregion
    }
}