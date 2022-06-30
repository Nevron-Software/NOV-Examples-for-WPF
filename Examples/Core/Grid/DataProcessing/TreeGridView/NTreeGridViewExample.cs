using System;
using Nevron.Nov.Data;
using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Grid;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Grid
{
    public class NTreeGridViewExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NTreeGridViewExample()
        {
        }
        /// <summary>
        /// Static constructor.
        /// </summary>
        static NTreeGridViewExample()
        {
            NTreeGridViewExampleSchema = NSchema.Create(typeof(NTreeGridViewExample), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            // create a hieararchical data table that represents a simple organization.
            NMemoryDataTable dataTable = new NMemoryDataTable(new NFieldInfo[]{
                new NFieldInfo("Id", typeof(Int32)),
                new NFieldInfo("ParentId", typeof(Int32)),
                new NFieldInfo("Name", typeof(String)),
                new NFieldInfo("Job Title", typeof(ENJobTitle)),
                new NFieldInfo("Company", typeof(string)),
            });

            int i = 0;

            // company 1
            dataTable.AddRow(0, -1, NDummyDataSource.PersonInfos[i++].Name, ENJobTitle.President, NDummyDataSource.CompanyNames[0]);
            dataTable.AddRow(1, 0, NDummyDataSource.PersonInfos[i++].Name, ENJobTitle.VicePresident, NDummyDataSource.CompanyNames[0]);
            dataTable.AddRow(2, 1, NDummyDataSource.PersonInfos[i++].Name, ENJobTitle.SalesManager, NDummyDataSource.CompanyNames[0]);
            dataTable.AddRow(3, 2, NDummyDataSource.PersonInfos[i++].Name, ENJobTitle.SalesRepresentative, NDummyDataSource.CompanyNames[0]);
            dataTable.AddRow(4, 2, NDummyDataSource.PersonInfos[i++].Name, ENJobTitle.SalesRepresentative, NDummyDataSource.CompanyNames[0]);
            dataTable.AddRow(5, 1, NDummyDataSource.PersonInfos[i++].Name, ENJobTitle.LeadDevelop, NDummyDataSource.CompanyNames[0]);
            dataTable.AddRow(6, 5, NDummyDataSource.PersonInfos[i++].Name, ENJobTitle.SeniorDeveloper, NDummyDataSource.CompanyNames[0]);
            dataTable.AddRow(7, 5, NDummyDataSource.PersonInfos[i++].Name, ENJobTitle.SeniorDeveloper, NDummyDataSource.CompanyNames[0]);

            // company 2
            dataTable.AddRow(8, -1, NDummyDataSource.PersonInfos[i++].Name, ENJobTitle.President, NDummyDataSource.CompanyNames[1]);
            dataTable.AddRow(9, 8, NDummyDataSource.PersonInfos[i++].Name, ENJobTitle.VicePresident, NDummyDataSource.CompanyNames[1]);
            dataTable.AddRow(10, 9, NDummyDataSource.PersonInfos[i++].Name, ENJobTitle.SalesManager, NDummyDataSource.CompanyNames[1]);
            dataTable.AddRow(11, 10, NDummyDataSource.PersonInfos[i++].Name, ENJobTitle.SalesRepresentative, NDummyDataSource.CompanyNames[1]);
            dataTable.AddRow(12, 10, NDummyDataSource.PersonInfos[i++].Name, ENJobTitle.SalesRepresentative, NDummyDataSource.CompanyNames[1]);
            dataTable.AddRow(13, 10, NDummyDataSource.PersonInfos[i++].Name, ENJobTitle.SalesRepresentative, NDummyDataSource.CompanyNames[1]);
            dataTable.AddRow(14, 9, NDummyDataSource.PersonInfos[i++].Name, ENJobTitle.LeadDevelop, NDummyDataSource.CompanyNames[1]);
            dataTable.AddRow(15, 14, NDummyDataSource.PersonInfos[i++].Name, ENJobTitle.SeniorDeveloper, NDummyDataSource.CompanyNames[1]);
            dataTable.AddRow(16, 14, NDummyDataSource.PersonInfos[i++].Name, ENJobTitle.SeniorDeveloper, NDummyDataSource.CompanyNames[1]);
            dataTable.AddRow(17, 14, NDummyDataSource.PersonInfos[i++].Name, ENJobTitle.SeniorDeveloper, NDummyDataSource.CompanyNames[1]);

            // create a tree grid view
            // records are identified by the Id field.
            // the parent of each record is specified by the ParentId field.
            m_TreeGridView = new NTreeGridView();
            m_TreeGridView.Grid.IdFieldName = "Id";
            m_TreeGridView.Grid.ParentIdFieldName = "ParentId";
            m_TreeGridView.Grid.DataSource = new NDataSource(dataTable);

            return m_TreeGridView;
        }
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();

            NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_TreeGridView.Grid).CreatePropertyEditors(m_TreeGridView.Grid,
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
    Demonstrates the <b>NTreeGridView</b> and <b>NTreeGrid</b> 
</p>
<p>
    The <b>NTreeGridView</b> represents a grid that displays hierarchical data from the data source.
</p>
<p>
The hierarchy is encoded in the data source with the help of two service fields:
    <br/>
    <b>Id</b> - uniquely identifies the records in the data source.
    <br/>
    <b>ParentId</b> - identifies the parent record of a specific record by Id.
</p>
<p>
Usually the service fields of the data source are not displayed by the <b>Tree Grid</b> as is the case in this example.
</p>
";
        }

        #endregion

        #region Fields

        NTreeGridView m_TreeGridView;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NSelfReferencingExample.
        /// </summary>
        public static readonly NSchema NTreeGridViewExampleSchema;

        #endregion
    }
}