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
    public class NCustomMasterDetailsExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NCustomMasterDetailsExample()
        {
        }
        /// <summary>
        /// Static constructor.
        /// </summary>
        static NCustomMasterDetailsExample()
        {
            NCustomMasterDetailsExampleSchema = NSchema.Create(typeof(NCustomMasterDetailsExample), NExampleBase.NExampleBaseSchema);
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
                    NPairBox pairBox = new NPairBox(image, theColumn.Title, ENPairBoxRelation.Box1BeforeBox2);
                    pairBox.Spacing = 2;
                    return pairBox;
                };
                dataColumn.UpdateHeaderContent();
            }

            // create the custom detail that creates a widget displaying information about the row.
            // NOTE: The widget is created by the OnCustomDetailCreateWidget event handler.
            NMasterDetails masterDetails = grid.MasterDetails;

            NCustomDetail customDetail = new NCustomDetail();
            masterDetails.Details.Add(customDetail);

            customDetail.CreateWidgetDelegate = delegate(NCustomDetailCreateWidgetArgs arg)
            {
                // get information about the data source row
                string name = (string)arg.DataSource.GetValue(arg.RowIndex, "Name");
                ENGender gender = (ENGender)arg.DataSource.GetValue(arg.RowIndex, "Gender");
                DateTime birthday = (DateTime)arg.DataSource.GetValue(arg.RowIndex, "Birthday");
                ENCountry country = (ENCountry)arg.DataSource.GetValue(arg.RowIndex, "Country");
                string phone = (string)arg.DataSource.GetValue(arg.RowIndex, "Phone");
                string email = (string)arg.DataSource.GetValue(arg.RowIndex, "Email");

                // display the information as a widget
                NPairBox namePair = new NPairBox("Name:", name);
                NPairBox genderPair = new NPairBox("Gender:", gender.ToString());
                NPairBox birthdayPair = new NPairBox("Birthday:", birthday.ToString());
                NPairBox countryPair = new NPairBox("Country:", country.ToString());
                NPairBox phonePair = new NPairBox("Phone:", phone.ToString());
                NPairBox emailPair = new NPairBox("Email:", email.ToString());

                NImageBox image = new NImageBox();
                switch (gender)
                {
                    case ENGender.Male:
                        image.Image = NResources.Image__256x256_MaleIcon_jpg;
                        break;

                    case ENGender.Female:
                        image.Image = NResources.Image__256x256_FemaleIcon_jpg;
                        break;

                    default:
                        break;
                }

                NStackPanel infoStack = new NStackPanel();
                infoStack.VerticalSpacing = 2.0d;
                infoStack.Add(namePair);
                infoStack.Add(genderPair);
                infoStack.Add(birthdayPair);
                infoStack.Add(countryPair);
                infoStack.Add(phonePair);
                infoStack.Add(emailPair);

                NDockPanel dock = new NDockPanel();
                dock.Add(image, ENDockArea.Left);
                dock.Add(infoStack, ENDockArea.Center);

                // assign the widget to the event arguments.
                return dock;
            };

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
    Demonstrates how to implement custom master-details. Expand the master table rows to see details about each person.
</p>
<p>
    Custom master details are widgets that you can create in response to an event. 
    The event holds information about the data source and the row index for which a widget is needed.
    In this scenario of master-details it is up to the user to provide the widget.
</p>
";
        }

        #endregion

        #region Fields

        NTableGridView m_View;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NCustomMasterDetailsExample.
        /// </summary>
        public static readonly NSchema NCustomMasterDetailsExampleSchema;

        #endregion
    }
}