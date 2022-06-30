using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Grid;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Grid
{
    public class NAlternatingRowsExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NAlternatingRowsExample()
        {
        }
        /// <summary>
        /// Static constructor.
        /// </summary>
        static NAlternatingRowsExample()
        {
            NAlternatingRowsExampleSchema = NSchema.Create(typeof(NAlternatingRowsExample), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            m_GridView = new NTableGridView();
            m_GridView.Grid.DataSource = NDummyDataSource.CreatePersonsOrdersDataSource();
            m_GridView.Grid.AlternatingRows = true;
            return m_GridView;
        }
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();
			
            NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_GridView.Grid).CreatePropertyEditors(m_GridView.Grid, 
                NGrid.AlternatingRowsProperty,
                NGrid.AlternatingRowsIntervalProperty,
                NGrid.AlternatingRowsLengthProperty,
                NGrid.AlternatingRowBackgroundFillProperty);

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
    Demonstrates <b>Alternating Rows</b>.
</p>
<p>
    Use the controls on the right side to alter the properties that affect the <b>Alternating Rows</b> feature.
</p>
";
        }

        #endregion

        #region Fields

        NTableGridView m_GridView;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NAlternatingRowsExample.
        /// </summary>
        public static readonly NSchema NAlternatingRowsExampleSchema;

        #endregion
    }
}