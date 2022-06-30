using System;
using Nevron.Nov.Grid;
using Nevron.Nov.Graphics;
using Nevron.Nov.Data;
using Nevron.Nov.Dom;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Grid
{
    public class NFieldGroupingExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NFieldGroupingExample()
        {
        }
        /// <summary>
        /// Static constructor.
        /// </summary>
        static NFieldGroupingExample()
        {
            NFieldGroupingExampleSchema = NSchema.Create(typeof(NFieldGroupingExample), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            // create a view and get its grid
            NTableGridView view = new NTableGridView();
            NTableGrid grid = view.Grid;

            // customize the grid
            grid.AllowEdit = false;

            // create the dummy persons data source - we will use it to obtain person names from person ids from it.
            m_PersonsDataSource = NDummyDataSource.CreatePersonsDataSource();

            // bind to data source, but exclude the "PersonId" field from binding 
            grid.AutoCreateColumn += delegate(NAutoCreateColumnEventArgs arg)
            {
                if (arg.FieldInfo.Name == "PersonId")
                {
                    arg.DataColumn = null;
                }
            };
            grid.DataSource = NDummyDataSource.CreatePersonsOrdersDataSource();

            // create a grouping rule that groups by the PersonId field
            NGroupingRule groupingRule = new NGroupingRule();
            groupingRule.RowValue = new NFieldRowValue("PersonId");

            // create a custom grouping header named "Person"
            groupingRule.CreateGroupingHeaderContentDelegate = delegate(NGroupingRule theGroupingRule)
            {
                return new NLabel("Person");
            };

            // create custom group row cells that display the person Name and number of orders 
            groupingRule.CreateGroupRowCellsDelegate = delegate(NGroupingRuleCreateGroupRowCellsArgs arg)
            {
                // get the person id from the row for which we create row cells.
                int personId = (int)arg.GroupRow.GroupValue;

                // get the person name that corresponds to that person id.
                int idField = m_PersonsDataSource.GetFieldIndex("Id");
                NRecordset rs = m_PersonsDataSource.GetOrCreateIndex(idField).GetRecordsForValue(personId);
                string personName = (string)m_PersonsDataSource.GetValue(rs[0], "Name");

                // create the group row cells
                NGroupRowCell personNameCell = new NGroupRowCell(personName);
                personNameCell.EndXPosition.Mode = ENSpanCellEndXPositionMode.NextCellBeginX;

                NGroupRowCell ordersCountCell = new NGroupRowCell("Orders Count:" + arg.GroupRow.Recordset.Count);
                ordersCountCell.EndXPosition.Mode = ENSpanCellEndXPositionMode.RowEndX;
                ordersCountCell.BeginXPosition.Mode = ENSpanCellBeginXPositionMode.AnchorToEndX;

                return new NGroupRowCell[] { personNameCell, ordersCountCell };
            };
            grid.GroupingRules.Add(groupingRule);

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
    Demonstrates fields grouping, custom group header content and custom group row cells.
</p>
<p>
    In this example we group the <b>PersonSales</b> data source by the <b>PersonId</b> field, which is intentionally hidden from display in the grid.
</p>
<p>
    Because the <b>PersonId</b> field is not very informative for the user, we create custom GroupRow cells, that pull the person <b>Name</b> by <b>Id</b> from the <b>Persons</b> data source.
</p>
";
        }

        #endregion

        #region Fields

        NDataSource m_PersonsDataSource;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NFieldGroupingExample.
        /// </summary>
        public static readonly NSchema NFieldGroupingExampleSchema;

        #endregion
    }
}