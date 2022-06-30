using System;
using Nevron.Nov.Data;
using Nevron.Nov.Dom;
using Nevron.Nov.Grid;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Grid
{
    public class NCustomGroupingExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NCustomGroupingExample()
        {
        }
        /// <summary>
        /// Static constructor.
        /// </summary>
        static NCustomGroupingExample()
        {
            NCustomGroupingExampleSchema = NSchema.Create(typeof(NCustomGroupingExample), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            // store current date time
            m_Now = DateTime.Now;

            // create a view and get its grid
            NTableGridView view = new NTableGridView();
            NTableGrid grid = view.Grid;

            // bind the grid to the data source
            grid.DataSource = CreateMailDataSource();

            // create a grouping rule with a custom row value.
            // NOTE: The RowValue associated with each grouping rule, returns an object for each row of the data source.
            // the rows in the data source are grouped according to that object.
            // The NCustomRowValue provides a delegate that help you return a custom object object for each data source row.
            // In our example the NCustomRowValue returns a member of the ENMailGroup enumeraiton for each record, depending on its Received row value.
            NCustomRowValue<ENMailGroup> customRowValue = new NCustomRowValue<ENMailGroup>();
            customRowValue.Description = "Received";
            customRowValue.GetRowValueDelegate = GetRowValueDelegate;

            // NOTE: The NGroupingRule provides the following events:
            // CreateGroupRowCells - raised when the grid needs to create the cells of the group row.
            // CreateGroupingHeaderContent - raised when the grid needs to create a grouping header content for the grouping in the groupings panel.
            NGroupingRule groupingRule = new NGroupingRule();
            groupingRule.RowValue = customRowValue;
            groupingRule.CreateGroupRowCellsDelegate = delegate(NGroupingRuleCreateGroupRowCellsArgs arg)
            {
                int groupValue = Convert.ToInt32(arg.GroupRow.GroupValue);
                string text = NStringHelpers.InsertSpacesBeforeUppersAndDigits(((ENMailGroup)groupValue).ToString());
                return new NGroupRowCell[] { new NGroupRowCell(text) };
            };

            groupingRule.CreateGroupingHeaderContentDelegate = delegate(NGroupingRule theGroupingRule)
            {
                return new NLabel("Received");
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
    Demonstrates custom grouping, custom group header content and custom group row cells.
</p>
<p>
    In this example we are grouping fictional emails by the <b>Received</b> field. 
    However, since the condition for grouping is complex, we are using the <b>NCustomRowValue</b> to provide a custom row grouping condition.
</p>
<p>
    The example also demonstrates how to create custom GroupRow cells. Since the email group values are instances of the ENMailGroup enumeration, 
    the <b>CreateGroupRowCellsDelegate</b> is handled to provide a string representation for them.
</p>
<p>
    The example also demonstrates how to override the <b>CreateGroupingHeaderContentDelegate</b> to provide a custom grouping rule header content. 
    In example we have created, Received label serves as header content for the custom grouping rule.
</p>
";
        }

        #endregion

        #region Custom Row Value Provider Delegates

        /// <summary>
        /// Delegate for getting a row value.
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        NNullable<ENMailGroup> GetRowValueDelegate(NCustomRowValueGetRowValueArgs<ENMailGroup> args)
        {
            DateTime received = (DateTime)args.DataSource.GetValue(args.Row, "Received");

            DayOfWeek dayOfWeek = m_Now.DayOfWeek;
            DateTime todayStart = new DateTime(m_Now.Year, m_Now.Month, m_Now.Day, 0, 0, 0);
            DateTime yesterdayStart = todayStart - new TimeSpan(1, 0, 0, 0);

            // today
            if (received >= todayStart)
                return ENMailGroup.Today;

            // yesterday
            if (received >= yesterdayStart && received < todayStart)
                return ENMailGroup.Yesterday;

            // check weeks
            {
                DateTime lastWeekEnd = todayStart - new TimeSpan(((int)dayOfWeek - 1), 0, 0, 0);
                if (received < lastWeekEnd)
                {
                    DateTime lastWeekStart = lastWeekEnd - new TimeSpan(7, 0, 0, 0);
                    if (received >= lastWeekStart && received < lastWeekEnd)
                        return ENMailGroup.LastWeek;

                    DateTime twoWeekAgoStart = lastWeekStart - new TimeSpan(7, 0, 0, 0);
                    if (received >= twoWeekAgoStart && received < lastWeekStart)
                        return ENMailGroup.TwoWeeksAgo;

                    DateTime threeWeeksAgoStart = twoWeekAgoStart - new TimeSpan(7, 0, 0, 0);
                    if (received >= threeWeeksAgoStart && received < twoWeekAgoStart)
                        return ENMailGroup.ThreeWeeksAgo;
                }
            }

            // check days of week
            {
                DateTime dayOfWeekStart = todayStart;
                DateTime dayOfWeekEnd = todayStart + new TimeSpan(24, 0, 0);
                while (true)
                {
                    if (received >= dayOfWeekStart && received < dayOfWeekEnd)
                    {
                        switch (dayOfWeek)
                        {
                            case DayOfWeek.Friday:
                                return ENMailGroup.Friday;

                            case DayOfWeek.Monday:
                                return ENMailGroup.Monday;

                            case DayOfWeek.Saturday:
                                return ENMailGroup.Saturday;

                            case DayOfWeek.Sunday:
                                return ENMailGroup.Sunday;

                            case DayOfWeek.Thursday:
                                return ENMailGroup.Thursday;

                            case DayOfWeek.Tuesday:
                                return ENMailGroup.Tuesday;

                            case DayOfWeek.Wednesday:
                                return ENMailGroup.Wednesday;

                            default:
                                throw new Exception("New DayOfWeek?");
                        }
                    }

                    dayOfWeek = dayOfWeek - 1;
                    dayOfWeekStart = dayOfWeekStart - new TimeSpan(24, 0, 0);
                    dayOfWeekEnd = dayOfWeekEnd - new TimeSpan(24, 0, 0);

                    if (dayOfWeek == DayOfWeek.Sunday)
                        break;
                }
            }

            // check months
            DateTime lastMonthEnd = new DateTime(m_Now.Year, m_Now.Month, 1, 0, 0, 0);
            DateTime lastMonthStart = lastMonthEnd.AddMonths(-1);
            if (received >= lastMonthStart && received < lastMonthEnd)
                return ENMailGroup.LastMonth;

            DateTime twoMonthsAgoStart = lastMonthStart - new TimeSpan(7, 0, 0, 0);
            if (received >= twoMonthsAgoStart && received < lastMonthStart)
                return ENMailGroup.TwoMonthsAgo;

            DateTime threeMonthsAgoStart = twoMonthsAgoStart - new TimeSpan(7, 0, 0, 0);
            if (received >= threeMonthsAgoStart && received < twoMonthsAgoStart)
                return ENMailGroup.ThreeMonthsAgo;

            return ENMailGroup.Older;
        }

        #endregion
     
        #region Mail Data Source

        /// <summary>
        /// Creates a fictional data source that represents received e-mails.
        /// </summary>
        /// <returns></returns>
        private NDataSource CreateMailDataSource()
        {
            // create a a dummy data table that represents a simple organization.
            NMemoryDataTable dataTable = new NMemoryDataTable(new NFieldInfo[]{
                new NFieldInfo("From", typeof(String)),
                new NFieldInfo("Subject", typeof(String)),
                new NFieldInfo("Received", typeof(DateTime)),
                new NFieldInfo("Size", typeof(String)),
            });

            string[] subjects = new string[]
            {
                "VIVACOM BILL",
                "SharePoint Users",
                "USB Sticks",
                "Garden Conference",
                ".NET Core and .NET Native",
                "Hackers Attack",
                "Week in Review",
                "Big Data Analytics",
                "Encryption Compromise",
                "Grid Issues",
                "DSC SOT BILL",
                "Data Security Bulletin",
                "How Cybercriminals use Facebook",
                "Empowering Users Success",
                "Boost your Income",
                "The AMISH way to motivate",
                "Daily news",
            };

            Random rnd = new Random();

            for (int i = 0; i < 600; i++)
            {
                string name = NDummyDataSource.RandomPersonInfo().Name;
                string subject = subjects[rnd.Next(subjects.Length)];
                DateTime received = m_Now - new TimeSpan(rnd.Next(60), rnd.Next(24), rnd.Next(60), 0);
                string size = (10 + rnd.Next(100)).ToString() + " KB";

                dataTable.AddRow(name, subject, received, size);
            }

            return new NDataSource(dataTable);
        }

        #endregion

        #region Fields

        DateTime m_Now;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NCustomGroupingExample.
        /// </summary>
        public static readonly NSchema NCustomGroupingExampleSchema;

        #endregion

        #region MailGroup

        public enum ENMailGroup
        {
            Today,
            Yesterday,
            Sunday,
            Saturday,
            Friday,
            Thursday,
            Wednesday,
            Tuesday,
            Monday,
            LastWeek,
            TwoWeeksAgo,
            ThreeWeeksAgo,
            LastMonth,
            TwoMonthsAgo,
            ThreeMonthsAgo,
            Older
        }


        #endregion
    }
}