using System.IO;
using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Text;
using Nevron.Nov.Text.Data;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
    public class NMailMergeExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NMailMergeExample()
        {
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static NMailMergeExample()
        {
            NMailMergeExampleSchema = NSchema.Create(typeof(NMailMergeExample), NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            // Create a simple drawing
            NDrawingViewWithRibbon drawingViewWithRibbon = new NDrawingViewWithRibbon();
            m_DrawingView = drawingViewWithRibbon.View;

            m_DrawingView.Document.HistoryService.Pause();
            try
            {
                InitDiagram(m_DrawingView.Document);
            }
            finally
            {
                m_DrawingView.Document.HistoryService.Resume();
            }

            return drawingViewWithRibbon;
        }
        protected override NWidget CreateExampleControls()
        {
            return null;
        }
        protected override string GetExampleDescription()
        {
            NDataUri previewMailMergeUri = NDataUri.FromImage(NResources.Image_Documentation_PreviewResults_png);
            return @"
<p>
	This example demonstrates how to use the mail merge functionality of the Nevron Diagram control.
</p>
<p>
	Click the <b>Preview Mail Merge</b> button (&nbsp;<img src=""" + previewMailMergeUri.ToString() +
    @""" />&nbsp;) from the <b>Mailings</b> ribbon tab to see the values for the currently selected
    mail merge record. When ready click the <b>Merge & Save</b> button to save all merged documents to a file.
</p>
<p>
	The <b>Merge & Save</b> button saves each of the individual documents result of the mail
	merge operation to a folder.	
</p>
";
        }

        private void InitDiagram(NDrawingDocument drawingDocument)
        {
            NDrawing drawing = drawingDocument.Content;
            NPage activePage = drawing.ActivePage;

            // Hide the grid and the ports
            drawing.ScreenVisibility.ShowGrid = false;
            drawing.ScreenVisibility.ShowPorts = false;

            // Create a shape factory
            NBasicShapeFactory basicShapeFactory = new NBasicShapeFactory();
            basicShapeFactory.DefaultSize = new NSize(100, 100);

            // Create the Name shape
            NShape nameShape = basicShapeFactory.CreateShape(ENBasicShape.Rectangle);
            nameShape.Width = 150;
            nameShape.PinX = activePage.Width / 2;
            nameShape.PinY = 100;

            NParagraph paragraph = new NParagraph();
            paragraph.Inlines.Add(new NFieldInline(new NMailMergePredefinedFieldValue(ENMailMergeDataField.FirstName)));
            paragraph.Inlines.Add(new NTextInline(" "));
            paragraph.Inlines.Add(new NFieldInline(new NMailMergePredefinedFieldValue(ENMailMergeDataField.LastName)));

            nameShape.GetTextBlock().Content.Blocks.Clear();
            nameShape.GetTextBlock().Content.Blocks.Add(paragraph);
            activePage.Items.Add(nameShape);

            // Create the City shape
            NShape cityShape = basicShapeFactory.CreateShape(ENBasicShape.SixPointStar);
            cityShape.PinX = nameShape.PinX - 150;
            cityShape.PinY = nameShape.PinY + 200;

            paragraph = new NParagraph();
            paragraph.Inlines.Add(new NFieldInline(new NMailMergePredefinedFieldValue(ENMailMergeDataField.City)));

            cityShape.GetTextBlock().Content.Blocks.Clear();
            cityShape.GetTextBlock().Content.Blocks.Add(paragraph);
            activePage.Items.Add(cityShape);

            // Create the Birth Date shape
            NShape birthDateShape = basicShapeFactory.CreateShape(ENBasicShape.Circle);
            birthDateShape.PinX = nameShape.PinX + 150;
            birthDateShape.PinY = cityShape.PinY;

            paragraph = new NParagraph();
            paragraph.Inlines.Add(new NFieldInline(new NMailMergeSourceFieldValue("BirthDate")));

            birthDateShape.GetTextBlock().Content.Blocks.Clear();
            birthDateShape.GetTextBlock().Content.Blocks.Add(paragraph);
            activePage.Items.Add(birthDateShape);

            // Connect the shapes
            NRoutableConnector connector = new NRoutableConnector();
            connector.Text = "City";
            connector.TextBlock.BackgroundFill = new NColorFill(NColor.White);
            connector.GlueBeginToNearestPort(nameShape);
            connector.GlueEndToNearestPort(cityShape);
            activePage.Items.Add(connector);

            connector = new NRoutableConnector();
            connector.Text = "Birth Date";
            connector.TextBlock.BackgroundFill = new NColorFill(NColor.White);
            connector.GlueBeginToNearestPort(nameShape);
            connector.GlueEndToNearestPort(birthDateShape);
            activePage.Items.Add(connector);

            // Load a mail merge data source from resource
            Stream stream = NResources.Instance.GetResourceStream("RSTR_Employees_csv");
            NMailMergeDataSource dataSource = NDataSourceFormat.Csv.LoadFromStream(stream, new NDataSourceLoadSettings(null, null, true));

            // Set some field mappings
            NMailMergeFieldMap fieldMap = new NMailMergeFieldMap();

            fieldMap.Set(ENMailMergeDataField.CourtesyTitle, "TitleOfCourtesy");
            fieldMap.Set(ENMailMergeDataField.FirstName, "FirstName");
            fieldMap.Set(ENMailMergeDataField.LastName, "LastName");
            fieldMap.Set(ENMailMergeDataField.City, "City");

            // Configure the drawing's mail merge
            drawing.MailMerge.DataSource = dataSource;
        }

        #endregion

        #region Fields

        private NDrawingView m_DrawingView;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NMailMergeExample.
        /// </summary>
        public static readonly NSchema NMailMergeExampleSchema;

        #endregion
    }
}
