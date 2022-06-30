using Nevron.Nov.Data;
using Nevron.Nov.Dom;
using Nevron.Nov.Grid;
using Nevron.Nov.UI;
using System;

namespace Nevron.Nov.Examples.Grid
{
    public class NNullValuesExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NNullValuesExample()
        {
        }
        /// <summary>
        /// Static constructor.
        /// </summary>
        static NNullValuesExample()
        {
            NNullValuesExampleSchema = NSchema.Create(typeof(NNullValuesExample), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
			/*            // create a memory data table that supports null values
						NMemoryDataTable table = new NMemoryDataTable();
						table.AddField(new NFieldInfo("Name", typeof(string), true));
						table.AddField(new NFieldInfo("Birthday", typeof(DateTime), true));
						table.AddField(new NFieldInfo("Country", typeof(ENCountry), true));
						table.AddField(new NFieldInfo("Email", typeof(string), true));

						Random rnd = new Random();
						for (int i = 0; i < NDummyDataSource.PersonInfos.Length; i++)
						{
							NDummyDataSource.NPersonInfo personInfo = NDummyDataSource.PersonInfos[i];

							bool nullName = (rnd.Next(8) == 1);
							bool nullBirthday = (rnd.Next(8) == 2);
							bool nullCountry = (rnd.Next(8) == 3);
							bool nullEmail = (rnd.Next(8) == 4);

							table.AddRow(
								(nullName? null: personInfo.Name),                  // name
								(nullBirthday? null: (object)personInfo.Birthday),  // birthday
								(nullCountry ? null : (object)personInfo.Country),  // country
								(nullEmail? null: personInfo.Email));               // email
						}*/

			// create a memory data table that supports null values
			NMemoryDataTable table = new NMemoryDataTable();
			table.AddField(new NFieldInfo("Name", typeof(string), true));

			Random rnd = new Random();
			for (int i = 0; i < 1; i++)
			{
				NDummyDataSource.NPersonInfo personInfo = NDummyDataSource.PersonInfos[i];

				bool nullName = (rnd.Next(8) == 1);
				bool nullBirthday = (rnd.Next(8) == 2);
				bool nullCountry = (rnd.Next(8) == 3);
				bool nullEmail = (rnd.Next(8) == 4);

				table.AddRow((nullName ? null : personInfo.Name));
			}


			m_TableView = new NTableGridView();
            m_TableView.Grid.DataSource = new NDataSource(table);
            m_TableView.Grid.AllowEdit = true;
            return m_TableView;
        }
        protected override NWidget CreateExampleControls()
        {
            return null;
        }
        protected override string GetExampleDescription()
        {
            return @"
<p>
    Demonstrates the grid support for null values. Note that the grid also supports editing of null values. 
</p>
";
        }

        #endregion

        #region Fields

        NTableGridView m_TableView;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NNullValuesExample.
        /// </summary>
        public static readonly NSchema NNullValuesExampleSchema;

        #endregion
    }
}