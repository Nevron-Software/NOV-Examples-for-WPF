using System;
using System.Reflection;

using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Formats;
using Nevron.Nov.Dom;
using Nevron.Nov.IO;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
    public class NCauseAndEffectShapesExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NCauseAndEffectShapesExample()
        {
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static NCauseAndEffectShapesExample()
        {
            NCauseAndEffectShapesExampleSchema = NSchema.Create(typeof(NCauseAndEffectShapesExample), NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            NDrawingViewWithRibbon drawingViewWithRibbon = new NDrawingViewWithRibbon();
            m_DrawingView = drawingViewWithRibbon.View;

            // Load the "Cause and Effect" shape library
            NFile libraryFile = NApplication.ResourcesFolder.GetFile(NPath.Current.Combine(
                "ShapeLibraries", "Business", "Cause and Effect Shapes.nlb"));
            NLibraryFormat.NevronBinary.LoadFromFileAsync(libraryFile).Then(
                delegate (NLibraryDocument libraryDocument)
                {
                    // "Cause and Effect" shape library loaded successfully, so create a "Cause and Effect" diagram
                    m_CauseAndEffectLibrary = libraryDocument.Content;
                    InitDiagram(m_DrawingView.Document);
                    m_DrawingView.Document.HistoryService.Resume();
                },
                delegate (Exception ex)
                {
                    // Failed to load the "Cause and Effect" shape library
                    m_DrawingView.Document.HistoryService.Resume();
                }
            );

            return drawingViewWithRibbon;
        }
        protected override NWidget CreateExampleControls()
        {
            return null;
        }
        protected override string GetExampleDescription()
        {
            return @"<p>This example shows the cause and effect shapes located in the ""Business\Cause and Effect Shapes.nlb"" shape library.</p>";
        }

        #endregion

        #region Implementation

        private NShape CreateShape(ENCauseAndEffectShape causeAndEffectShape)
        {
            NLibraryItem libraryItem = m_CauseAndEffectLibrary.Items[(int)causeAndEffectShape];
            NShape shape = (NShape)libraryItem.Items[0];
            return (NShape)shape.DeepClone();
        }
        private void InitDiagram(NDrawingDocument drawingDocument)
        {
            NPage page = drawingDocument.Content.ActivePage;
            m_PendingShapesForPortCreation = 0;

            // Create some Cause and Effect shapes, setting the number of desired ports of each shape to its "Tag" property.
            // Ports will be created when the code-behind of the shapes has been compiled.

            // Create an Effect shape
            m_EffectShape = CreateShape(ENCauseAndEffectShape.Effect);
            m_EffectShape.Text = "Desired Effect";
            m_EffectShape.PinX = 700;
            m_EffectShape.PinY = 250;
            m_EffectShape.Controls[0].X = -600;
            SetNumberOfPortsToCreate(m_EffectShape, 4);
            page.Items.Add(m_EffectShape);

            // Craete 4 Category shapes
            m_CategoryShape1 = CreateShape(ENCauseAndEffectShape.Category1);
            m_CategoryShape1.Text = "Category 1";
            m_CategoryShape1.PinX = 70;
            m_CategoryShape1.PinY = 100;
            page.Items.Add(m_CategoryShape1);

            m_CategoryShape2 = CreateShape(ENCauseAndEffectShape.Category1);
            m_CategoryShape2.Text = "Category 2";
            m_CategoryShape2.PinX = 340;
            m_CategoryShape2.PinY = 100;
            SetNumberOfPortsToCreate(m_CategoryShape2, 1);
            page.Items.Add(m_CategoryShape2);

            m_CategoryShape3 = CreateShape(ENCauseAndEffectShape.Category2);
            m_CategoryShape3.Text = "Category 3";
            m_CategoryShape3.PinX = 200;
            m_CategoryShape3.PinY = 400;
            SetNumberOfPortsToCreate(m_CategoryShape3, 1);
            page.Items.Add(m_CategoryShape3);

            m_CategoryShape4 = CreateShape(ENCauseAndEffectShape.Category2);
            m_CategoryShape4.Text = "Category 4";
            m_CategoryShape4.PinX = 470;
            m_CategoryShape4.PinY = 400;
            page.Items.Add(m_CategoryShape4);

            // Create 2 Cause shapes
            m_CauseShape1 = CreateShape(ENCauseAndEffectShape.PrimaryCause2);
            m_CauseShape1.Text = "Cause 1";
            m_CauseShape1.PinX = 500;
            m_CauseShape1.PinY = 175;
            page.Items.Add(m_CauseShape1);

            m_CauseShape2 = CreateShape(ENCauseAndEffectShape.PrimaryCause1);
            m_CauseShape2.Text = "Cause 2";
            m_CauseShape2.PinX = 120;
            m_CauseShape2.PinY = 325;
            page.Items.Add(m_CauseShape2);
        }
        private void SetNumberOfPortsToCreate(NShape shape, int numberOfPortsToCreate)
        {
            shape.Tag = numberOfPortsToCreate; // Denotes the desired number of ports to create
            shape.ShapeCodeBehindChanged += OnShapeCodeBehindChanged;
            m_PendingShapesForPortCreation++;
        }
        private void ConnectShapes()
        {
            m_CategoryShape1.Controls[0].GlueToPort(m_EffectShape.Ports[3]);
            m_CategoryShape2.Controls[0].GlueToPort(m_EffectShape.Ports[1]);
            m_CategoryShape3.Controls[0].GlueToPort(m_EffectShape.Ports[2]);
            m_CategoryShape4.Controls[0].GlueToPort(m_EffectShape.Ports[0]);

            m_CauseShape1.Controls[0].GlueToPort(m_CategoryShape2.Ports[0]);
            m_CauseShape2.Controls[0].GlueToPort(m_CategoryShape3.Ports[0]);
        }

        #endregion

        #region Event Handlers

        private void OnShapeCodeBehindChanged(NShapeCodeBehind codeBehind)
        {
            // Get the current cause and effect shape
            NShape shape = codeBehind.Object;
            int numberOfPortsToCreate = (int)shape.Tag;

            // Get the "AddPort" method of the shape code-behind via reflection
            MethodInfo addPortMethod = codeBehind.GetType().GetMethod("AddPort");

            // Invoke the "AddPort" method to create the desired number of ports
            for (int i = 0; i < numberOfPortsToCreate; i++)
            {
                addPortMethod.Invoke(codeBehind, null);
            }

            m_PendingShapesForPortCreation--;
            if (m_PendingShapesForPortCreation == 0)
            {
                // No more shapes are pending for port creation, so connect shapes to ports
                ConnectShapes();
            }
        }

        #endregion

        #region Fields

        private NLibrary m_CauseAndEffectLibrary;
        private NDrawingView m_DrawingView;
        private int m_PendingShapesForPortCreation;

        // Shapes
        private NShape m_EffectShape;
        private NShape m_CategoryShape1;
        private NShape m_CategoryShape2;
        private NShape m_CategoryShape3;
        private NShape m_CategoryShape4;
        private NShape m_CauseShape1;
        private NShape m_CauseShape2;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NCauseAndEffectShapesExample.
        /// </summary>
        public static readonly NSchema NCauseAndEffectShapesExampleSchema;

        #endregion

        #region Nested Types

        /// <summary>
        /// Cause and effect shapes.
        /// </summary>
        public enum ENCauseAndEffectShape
        {
            /// <summary>
            /// Effect
            /// </summary>
            Effect,
            /// <summary>
            /// Category 1
            /// </summary>
            Category1,
            /// <summary>
            /// Category 2
            /// </summary>
            Category2,
            /// <summary>
            /// Primary cause 1
            /// </summary>
            PrimaryCause1,
            /// <summary>
            /// Primary cause 2
            /// </summary>
            PrimaryCause2,
            /// <summary>
            /// Secondary cause 1
            /// </summary>
            SecondaryCause1,
            /// <summary>
            /// Secondary cause 2
            /// </summary>
            SecondaryCause2,
            /// <summary>
            /// Secondary cause 3
            /// </summary>
            SecondaryCause3,
            /// <summary>
            /// Secondary cause 4
            /// </summary>
            SecondaryCause4,
            /// <summary>
            /// Secondary cause 5
            /// </summary>
            SecondaryCause5,
            /// <summary>
            /// Secondary cause 6
            /// </summary>
            SecondaryCause6
        }

        #endregion
    }
}