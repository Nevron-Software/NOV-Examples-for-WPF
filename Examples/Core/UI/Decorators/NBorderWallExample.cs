using System;

using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NBorderWallExample : NExampleBase
    {
        #region Constructors

        public NBorderWallExample()
        {
        }
        static NBorderWallExample()
        {
            NBorderWallExampleSchema = NSchema.Create(typeof(NBorderWallExample), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            // create a tab control for the different border walls
            NTab tab = new NTab();

            NTabPage boxBorderPage = new NTabPage("Box Border");
            tab.TabPages.Add(boxBorderPage);

            NTabPage crossBorderPage = new NTabPage("Cross Border");
            tab.TabPages.Add(crossBorderPage);

            NTabPage openBorderPage = new NTabPage("Opened Border");
            tab.TabPages.Add(openBorderPage);

            // create the three elements that demonstrate the border walls
            m_BoxBorderElement = new NCustomBorderWallWidget();
            boxBorderPage.Content = m_BoxBorderElement;
            m_BoxBorderElement.BorderWallType = ENCustomBorderWallType.Rectangle;

            m_CrossBorderElement = new NCustomBorderWallWidget();
            crossBorderPage.Content = m_CrossBorderElement;
            m_CrossBorderElement.BorderWallType = ENCustomBorderWallType.Cross;

            m_OpenedBorderElement = new NCustomBorderWallWidget();
            openBorderPage.Content = m_OpenedBorderElement;
            m_OpenedBorderElement.BorderWallType = ENCustomBorderWallType.Opened;

            // init the custom border elements
            NCustomBorderWallWidget[] elements = GetCustomBorderElements();
            NUIThemeColorMap colors = new NUIThemeColorMap(ENUIThemeScheme.WindowsClassic);
            for (int i = 0; i < elements.Length; i++)
            {
                elements[i].BorderThickness = new NMargins(2);
                elements[i].Border = NBorder.CreateRaised3DBorder(colors);
                elements[i].Margins = new NMargins(10);
            }

            return tab;
        }
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();
            
            // create the predefined borders combo and populate it
            stack.Add(CreateLabel("Apply Predefined Border:"));
            m_PredefinedBorderCombo = new NComboBox();
            stack.Add(m_PredefinedBorderCombo);

            m_PredefinedBorderCombo.Items.Add(new NComboBoxItem("3D Raised Border"));
            m_PredefinedBorderCombo.Items.Add(new NComboBoxItem("3D Sunken Border"));
            m_PredefinedBorderCombo.Items.Add(new NComboBoxItem("Filled Border"));
            m_PredefinedBorderCombo.Items.Add(new NComboBoxItem("Filled Border with Outlines"));
            m_PredefinedBorderCombo.SelectedIndex = 0;
            m_PredefinedBorderCombo.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnPredefinedBorderComboSelectedIndexChanged);

            // create the combos for the border thickness
            stack.Add(CreateLabel("Border Thickness:"));
            m_BorderThicknessCombo = CreateBorderSideThicknessCombo();
            stack.Add(m_BorderThicknessCombo);

            stack.Add(CreateLabel("Left Side Thickness:"));
            m_LeftSideThicknessCombo = CreateBorderSideThicknessCombo();
            stack.Add(m_LeftSideThicknessCombo);

            stack.Add(CreateLabel("Right Side Thickness:"));
            m_RightSideThicknessCombo = CreateBorderSideThicknessCombo();
            stack.Add(m_RightSideThicknessCombo);

            stack.Add(CreateLabel("Top Side Thickness:"));
            m_TopSideThicknessCombo = CreateBorderSideThicknessCombo();
            stack.Add(m_TopSideThicknessCombo);

            stack.Add(CreateLabel("Bottom Side Thickness:"));
            m_BottomSideThicknessCombo = CreateBorderSideThicknessCombo();
            stack.Add(m_BottomSideThicknessCombo);

            stack.Add(CreateLabel("Inner Corner Radius:"));
            m_InnerRadiusCombo = CreateCornerRadiusCombo();
            stack.Add(m_InnerRadiusCombo);

            stack.Add(CreateLabel("Outer Corner Radius:"));
            m_OuterRadiusCombo = CreateCornerRadiusCombo();
            stack.Add(m_OuterRadiusCombo);

            return stack;
        }
        protected override string GetExampleDescription()
        {
            return @"
<p>
This example demonstrates how to create different types of borders and apply them to widgets. 
It also demonstrates how to override the border wall of a widget and provide a custom one.
Using the controls to the right	you can change the type and appearance of the generated borders.
</p>
";
        }

        #endregion

        #region Event Handlers

        void OnPredefinedBorderComboSelectedIndexChanged(NValueChangeEventArgs args)
        {
            if (m_PredefinedBorderCombo.SelectedIndex == -1)
                return;

            double innerRadius = m_InnerRadiusCombo.SelectedIndex;
            double outerRadius = m_OuterRadiusCombo.SelectedIndex;

            // apply a predefined border
            NCustomBorderWallWidget[] elements = GetCustomBorderElements();
            for (int i = 0; i < elements.Length; i++)
            {
                NBorder border = null;
                switch (m_PredefinedBorderCombo.SelectedIndex)
                {
                    case 0: // 3D Raised Border
                        border = NBorder.CreateRaised3DBorder(new NUIThemeColorMap(ENUIThemeScheme.WindowsClassic));
                        break;
                    case 1: // 3D Sunken Border
                        border = NBorder.CreateSunken3DBorder(new NUIThemeColorMap(ENUIThemeScheme.WindowsClassic));
                        break;
                    case 2: // Filled Border
                        border = NBorder.CreateFilledBorder(NColor.Red);
                        break;
                    case 3: // Filled Border with Outlines
                        border = NBorder.CreateFilledBorder(new NColorFill(NColor.Blue), new NStroke(1, NColor.Black), new NStroke(1, NColor.Black));
                        break;
                }

                border.SetRadiuses(innerRadius, outerRadius);
                elements[i].Border = border;
            }
        }
        void OnSideThicknessComboSelectedIndexChanged(NValueChangeEventArgs args)
        {
            NComboBox combo = args.TargetNode as NComboBox;
            double sideThickness = combo.SelectedIndex;

            NMargins bt = m_BoxBorderElement.BorderThickness;

            if (combo == m_BorderThicknessCombo)
            {
                bt = new NMargins(sideThickness);
            }
            else if (combo == m_LeftSideThicknessCombo)
            {
                bt.Left = sideThickness;
            }
            else if (combo == m_RightSideThicknessCombo)
            {
                bt.Right = sideThickness;
            }
            else if (combo == m_TopSideThicknessCombo)
            {
                bt.Top = sideThickness;
            }
            else if (combo == m_BottomSideThicknessCombo)
            {
                bt.Bottom = sideThickness;
            }

            NCustomBorderWallWidget[] elements = GetCustomBorderElements();
            for (int i = 0; i < elements.Length; i++)
            {
                elements[i].BorderThickness = bt;
            }
        }
        void OnCornerRadiusComboSelectedIndexChanged(NValueChangeEventArgs args)
        {
            OnPredefinedBorderComboSelectedIndexChanged(null);
        }

        #endregion

        #region Implementation

        NLabel CreateLabel(string text)
        {
            NLabel label = new NLabel(text);
            label.HorizontalPlacement = ENHorizontalPlacement.Left;
            return label;
        }
        NComboBox CreateBorderSideThicknessCombo()
        {
            NComboBox combo = new NComboBox();

            for (int i = 0; i < 30; i++)
            {
                combo.Items.Add(new NComboBoxItem(i.ToString() + " dip"));
            }
            combo.SelectedIndex = 2;
            combo.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnSideThicknessComboSelectedIndexChanged);
            return combo;
        }
        NComboBox CreateCornerRadiusCombo()
        {
            NComboBox combo = new NComboBox();

            for (int i = 0; i < 30; i++)
            {
                combo.Items.Add(new NComboBoxItem(i.ToString() + " dip"));
            }
            combo.SelectedIndex = 0;
            combo.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnCornerRadiusComboSelectedIndexChanged);
            return combo;
        }
        NCustomBorderWallWidget[] GetCustomBorderElements()
        {
            return new NCustomBorderWallWidget[] { m_BoxBorderElement, m_CrossBorderElement, m_OpenedBorderElement };
        }

        #endregion

        #region Fields

        NCustomBorderWallWidget m_BoxBorderElement;
        NCustomBorderWallWidget m_CrossBorderElement;
        NCustomBorderWallWidget m_OpenedBorderElement;

        NComboBox m_PredefinedBorderCombo;
        NComboBox m_BorderThicknessCombo;
        NComboBox m_LeftSideThicknessCombo;
        NComboBox m_RightSideThicknessCombo;
        NComboBox m_TopSideThicknessCombo;
        NComboBox m_BottomSideThicknessCombo;
        NComboBox m_InnerRadiusCombo;
        NComboBox m_OuterRadiusCombo;

        #endregion

        #region Schema

        public static readonly NSchema NBorderWallExampleSchema;

        #endregion

        #region Nested Types

        /// <summary>
        /// Enumerates the demonstrated border walls
        /// </summary>
        public enum ENCustomBorderWallType
        {
            Rectangle,
            Cross,
            Opened
        }
        /// <summary>
        /// A widget that overrides the default border wall of widgets 
        /// to demonstrate all possible corners and to demonstrate opened walls.
        /// </summary>
        public class NCustomBorderWallWidget : NWidget
        {
            #region Constructors

            public NCustomBorderWallWidget()
            {

            }
            static NCustomBorderWallWidget()
            {
                NCustomBorderWallWidgetSchema = NSchema.Create(typeof(NCustomBorderWallWidget), NWidget.NWidgetSchema);
                BorderWallTypeProperty = NCustomBorderWallWidgetSchema.AddSlot("BorderWallType", NDomType.FromType(typeof(ENCustomBorderWallType)), ENCustomBorderWallType.Rectangle);
            }

            #endregion

            #region Properties

            public ENCustomBorderWallType BorderWallType
            {
                get
                {
                    return (ENCustomBorderWallType)GetValue(BorderWallTypeProperty);
                }
                set
                {
                    SetValue(BorderWallTypeProperty, value);
                }
            }

            #endregion

            #region Protected Overrides - Border Wall

            protected override NBorderWall CreateBorderWall(NPaintVisitor visitor)
            {
                switch (BorderWallType)
                {
                    case ENCustomBorderWallType.Rectangle:
                        return base.CreateBorderWall(visitor);

                    case ENCustomBorderWallType.Cross:
                        return CreateCrossBorderWall();

                    case ENCustomBorderWallType.Opened:
                        return CreateOpenedBorderWall();

                    default:
                        throw new Exception("New ENCustomBorderWallType?");
                }
            }

            #endregion

            #region Implementation - Border Wall

            private NBorderWall CreateCrossBorderWall()
            {
                NRectangle r0 = GetContentEdge();

                NMargins bt = BorderThickness;
                bt.Left = Math.Min(bt.Left, r0.Width / 6);
                bt.Right = Math.Min(bt.Right, r0.Width / 6);
                bt.Top = Math.Min(bt.Top, r0.Height / 6);
                bt.Bottom = Math.Min(bt.Bottom, r0.Height / 6);

                NRectangle r1 = bt.GetInnerRect(r0);

                NRectangle r2 = NRectangle.FromLTRB(
                    r1.X + r1.Width / 3 - (bt.Left / 2),
                    r1.Y + r1.Height / 3 - (bt.Top / 2),
                    r1.Right - r1.Width / 3 + (bt.Right / 2),
                    r1.Bottom - r1.Height / 3 + (bt.Bottom / 2));

                NRectangle r3 = bt.GetInnerRect(r2);

                double x0 = r0.X;
                double x1 = r1.X;
                double x2 = r2.X;
                double x3 = r3.X;
                double x4 = r3.Right;
                double x5 = r2.Right;
                double x6 = r1.Right;
                double x7 = r0.Right;

                double y0 = r0.Y;
                double y1 = r1.Y;
                double y2 = r2.Y;
                double y3 = r3.Y;
                double y4 = r3.Bottom;
                double y5 = r2.Bottom;
                double y6 = r1.Bottom;
                double y7 = r0.Bottom;

                NBorderWall wall = new NBorderWall(true);

                wall.AddLeftTopCorner(new NRectangle(x2, y0, bt.Left, bt.Top));
                wall.AddTopSide(new NRectangle(x3, y0, x4 - x3, bt.Top));
                wall.AddTopRightCorner(new NRectangle(x4, y0, bt.Right, bt.Top));
                wall.AddRightSide(new NRectangle(x4, y1, bt.Right, y2 - y1));
                wall.AddRightTopCorner(new NRectangle(x4, y2, bt.Right, bt.Top));
                wall.AddTopSide(new NRectangle(x5, y2, x6 - x5, bt.Top));
                wall.AddTopRightCorner(new NRectangle(x6, y2, bt.Right, bt.Top));
                wall.AddRightSide(new NRectangle(x6, y3, bt.Right, y4 - y3));
                wall.AddRightBottomCorner(new NRectangle(x6, y4, bt.Right, bt.Bottom));
                wall.AddBottomSide(new NRectangle(x5, y4, x6 - x5, bt.Bottom));
                wall.AddBottomRightCorner(new NRectangle(x4, y4, bt.Right, bt.Bottom));
                wall.AddRightSide(new NRectangle(x4, y5, bt.Right, y6 - y5));
                wall.AddRightBottomCorner(new NRectangle(x4, y6, bt.Right, bt.Bottom));
                wall.AddBottomSide(new NRectangle(x3, y6, x4 - x3, bt.Bottom));
                wall.AddBottomLeftCorner(new NRectangle(x2, y6, bt.Left, bt.Bottom));
                wall.AddLeftSide(new NRectangle(x2, y5, bt.Left, y6 - y5));
                wall.AddLeftBottomCorner(new NRectangle(x2, y4, bt.Left, bt.Bottom));
                wall.AddBottomSide(new NRectangle(x1, y4, x2 - x1, bt.Bottom));
                wall.AddBottomLeftCorner(new NRectangle(x0, y4, bt.Left, bt.Bottom));
                wall.AddLeftSide(new NRectangle(x0, y3, bt.Left, y4 - y3));
                wall.AddLeftTopCorner(new NRectangle(x0, y2, bt.Left, bt.Top));
                wall.AddTopSide(new NRectangle(x1, y2, x2 - x1, bt.Top));
                wall.AddTopLeftCorner(new NRectangle(x2, y2, bt.Left, bt.Top));
                wall.AddLeftSide(new NRectangle(x2, y1, bt.Left, y2 - y1));

                return wall;
            }
            private NBorderWall CreateOpenedBorderWall()
            {
                NRectangle outer = GetBorderEdge();
                NRectangle inner = GetContentEdge();

                NBorderWall wall = new NBorderWall(false);

                double leftSide = inner.X - outer.X;
                double topSide = inner.Y - outer.Y;
                double rightSide = outer.Right - inner.Right;
                double bottomSide = outer.Bottom - inner.Bottom;

                double topClipStart = inner.X + inner.Width / 3;
                double topClipEnd = inner.Right - inner.Width / 3;
                
                wall.AddTopSide(new NRectangle(topClipEnd, outer.Y, inner.Right - topClipEnd, topSide));
                wall.AddTopRightCorner(new NRectangle(inner.Right, outer.Y, rightSide, topSide));
                wall.AddRightSide(new NRectangle(inner.Right, inner.Y, rightSide, inner.Height));
                wall.AddRightBottomCorner(new NRectangle(inner.Right, inner.Bottom, rightSide, bottomSide));
                wall.AddBottomSide(new NRectangle(inner.X, inner.Bottom, inner.Width, bottomSide));
                wall.AddBottomLeftCorner(new NRectangle(outer.X, inner.Bottom, leftSide, bottomSide));
                wall.AddLeftSide(new NRectangle(outer.X, inner.Y, leftSide, inner.Height));
                wall.AddLeftTopCorner(new NRectangle(outer.X, outer.Y, leftSide, topSide));
                wall.AddTopSide(new NRectangle(inner.X, outer.Y, topClipStart - inner.X, topSide));

                return wall;

            }

            #endregion

            #region Schema

            public static readonly NSchema NCustomBorderWallWidgetSchema;
            public static readonly NProperty BorderWallTypeProperty;

            #endregion
        }

        #endregion
    }
}
