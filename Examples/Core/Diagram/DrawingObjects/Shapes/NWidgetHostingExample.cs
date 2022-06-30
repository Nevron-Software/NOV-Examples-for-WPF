using Nevron.Nov.DataStructures;
using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NWidgetHostingExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NWidgetHostingExample()
        {
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static NWidgetHostingExample()
        {
            NWidgetHostingExampleSchema = NSchema.Create(typeof(NWidgetHostingExample), NExampleBaseSchema);
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
            return @"
<p>
    This example demonstrates the ability to embed any NOV UI Widget in shapes. This allows the creation of interactive
    diagrams that are incredibly rich on interaction features.
</p>
";
        }

        private void InitDiagram(NDrawingDocument drawingDocument)
        {
            // create the books list
            CreateBooksList();

            // create the shopping cart
            m_ShoppingCart = new NShoppingCart();

            NDrawing drawing = drawingDocument.Content;
            NPage activePage = drawing.ActivePage;

            // hide the grid
            drawing.ScreenVisibility.ShowGrid = false;

            // create a shape which hosts a widget
            CreateBookStore(activePage);
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Creates the list of books through which the user can browse
        /// </summary>
        private void CreateBooksList()
        {
            m_Books = new NList<NBook>();

            m_Books.Add(new NBook(
                "The Name Of The Wind",
                "Patrick Rothfuss",
                "This is the riveting first-person narrative of Kvothe, a young man who grows to be one of the most notorious magicians his world has ever seen. From his childhood in a troupe of traveling players, to years spent as a near-feral orphan in a crime-riddled city, to his daringly brazen yet successful bid to enter a legendary school of magic, The Name of the Wind is a masterpiece that transports readers into the body and mind of a wizard.",
                NResources.Image_Books_NameOfTheWind_jpg,
                12.90));
            m_Books.Add(new NBook(
                "Lord of Ohe Rings",
                "J.R.R. Tolkien",
                "In ancient times the Rings of Power were crafted by the Elven-smiths, and Sauron, the Dark Lord, forged the One Ring, filling it with his own power so that he could rule all others. But the One Ring was taken from him, and though he sought it throughout Middle-earth, it remained lost to him. After many ages it fell by chance into the hands of the hobbit Bilbo Baggins.",
                NResources.Image_Books_LordOfTheRings_jpg,
                13.99));
            m_Books.Add(new NBook(
                "A Game Of Thrones",
                "George R.R. Martin",
                "Long ago, in a time forgotten, a preternatural event threw the seasons out of balance. In a land where summers can last decades and winters a lifetime, trouble is brewing. The cold is returning, and in the frozen wastes to the north of Winterfell, sinister and supernatural forces are massing beyond the kingdom’s protective Wall. At the center of the conflict lie the Starks of Winterfell, a family as harsh and unyielding as the land they were born to.",
                NResources.Image_Books_AGameOfThrones_jpg,
                12.79));
            m_Books.Add(new NBook(
                "The Way Of Kings",
                "Brandon Sanderson",
                "Roshar is a world of stone and storms. Uncanny tempests of incredible power sweep across the rocky terrain so frequently that they have shaped ecology and civilization alike. Animals hide in shells, trees pull in branches, and grass retracts into the soilless ground. Cities are built only where the topography offers shelter.",
                NResources.Image_Books_TheWayOfKings_jpg,
                7.38));
            m_Books.Add(new NBook(
                "Mistborn",
                "Brandon Sanderson",
                "For a thousand years the ash fell and no flowers bloomed. For a thousand years the Skaa slaved in misery and lived in fear. For a thousand years the Lord Ruler, the 'Sliver of Infinity' reigned with absolute power and ultimate terror, divinely invincible. Then, when hope was so long lost that not even its memory remained, a terribly scarred, heart-broken half-Skaa rediscovered it in the depths of the Lord Ruler’s most hellish prison. Kelsier 'snapped' and found in himself the powers of a Mistborn. A brilliant thief and natural leader, he turned his talents to the ultimate caper, with the Lord Ruler himself as the mark. ",
                NResources.Image_Books_Mistborn_jpg,
                6.38));
        }
        /// <summary>
        /// Gets the min Width and Height of all book images
        /// </summary>
        /// <returns></returns>
        NSize GetMinBookImageSize()
        {
            NSize size = new NSize(double.MaxValue, double.MaxValue);
            for (int i = 0; i < m_Books.Count; i++)
            {
                NBook book = m_Books[i];
                if (book.Image.Width < size.Width)
                {
                    size.Width = book.Image.Width;
                }

                if (book.Image.Height < size.Height)
                {
                    size.Height = book.Image.Height;
                }
            }

            return size;
        }
        /// <summary>
        /// Creates the book store interface
        /// </summary>
        /// <param name="activePage"></param>
        void CreateBookStore(NPage activePage)
        {
            const double x1 = 50;
            const double x2 = x1 + 200;
            const double x3 = x2 + 50;
            const double x4 = x3 + 400;

            const double y1 = 50;
            const double y2 = y1 + 50;
            const double y3 = y2 + 50;
            const double y4 = y3 + 20;
            const double y5 = y4 + 200;
            const double y6 = y5 + 20;
            const double y7 = y6 + 50;

            // prev button
            NShape prevButtonShape = CreateButtonShape("Show Prev Book");
            SetLeftTop(prevButtonShape, new NPoint(x1, y1));
            ((NButton)prevButtonShape.Widget).Click += delegate(NEventArgs args)
            {
                LoadBook(m_nSelectedBook - 1);
            };
            activePage.Items.Add(prevButtonShape);

            // next button
            NShape nextButtonShape = CreateButtonShape("Show Next Book");
            SetRightTop(nextButtonShape, new NPoint(x2, y1));
            ((NButton)nextButtonShape.Widget).Click += delegate(NEventArgs args)
            {
                LoadBook(m_nSelectedBook + 1);
            };
            activePage.Items.Add(nextButtonShape);

            // add to cart
            NShape addToCartButton = CreateButtonShape("Add to Cart");
            SetRightTop(addToCartButton, new NPoint(x2, y6));
            ((NButton)addToCartButton.Widget).Click += delegate(NEventArgs args)
            {
                m_ShoppingCart.AddItem(m_Books[m_nSelectedBook], this);
            };
            activePage.Items.Add(addToCartButton);

            // create selected book shapes
            NBasicShapeFactory basicShapes = new NBasicShapeFactory();

            // selected image
            m_SelectedBookImage = basicShapes.CreateShape(ENBasicShape.Rectangle);
            SetLeftTop(m_SelectedBookImage, new NPoint(x1, y2));
            NSize minBookSize = GetMinBookImageSize();
            m_SelectedBookImage.Width = x2 - x1;
            m_SelectedBookImage.Height = y5 - y2; 
            activePage.Items.Add(m_SelectedBookImage);

            // selected title
            m_SelectedBookTitle = basicShapes.CreateShape(ENBasicShape.Text);
            m_SelectedBookTitle.TextBlock.InitXForm(ENTextBlockXForm.ShapeBox);
            m_SelectedBookTitle.TextBlock.FontSize = 25;
            m_SelectedBookTitle.TextBlock.Fill = new NColorFill(NColor.DarkBlue);
            SetLeftTop(m_SelectedBookTitle, new NPoint(x3, y2));
            m_SelectedBookTitle.Width = x4 - x3;
            m_SelectedBookTitle.Height = y3 - y2; 
            activePage.Items.Add(m_SelectedBookTitle);

            // selected description
            m_SelectedBookDescription = basicShapes.CreateShape(ENBasicShape.Text);
            m_SelectedBookDescription.TextBlock.InitXForm(ENTextBlockXForm.ShapeBox);
            SetLeftTop(m_SelectedBookDescription, new NPoint(x3, y4));
            m_SelectedBookDescription.Width = x4 - x3;
            m_SelectedBookDescription.Height = y5 - y4;
            activePage.Items.Add(m_SelectedBookDescription);

            // load the first book
            LoadBook(0);

            // create the shape that hosts the shopping cart widget
            NShape shoppingCartShape = new NShape();
            shoppingCartShape.Init2DShape();
            m_ShoppingCartWidget = new NContentHolder();
            m_ShoppingCartWidget.Content = m_ShoppingCart.CreateWidget(this);
            shoppingCartShape.Widget = m_ShoppingCartWidget;
            SetLeftTop(shoppingCartShape, new NPoint(x1, y7));
            BindSizeToDesiredSize(shoppingCartShape);
            activePage.Items.Add(shoppingCartShape);
        }
        /// <summary>
        /// Loads a book with the specified index
        /// </summary>
        /// <param name="index"></param>
        void LoadBook(int index)
        {
            m_nSelectedBook = NMath.Clamp(0, m_Books.Count - 1, index);
            NBook book = m_Books[m_nSelectedBook];

            m_SelectedBookImage.Geometry.Fill = new NImageFill((NImage)book.Image.DeepClone());
            m_SelectedBookTitle.Text = book.Name;
            m_SelectedBookDescription.Text = book.Description;
        }
        /// <summary>
        /// Creates a shape
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        NShape CreateButtonShape(string text)
        {
            NShape buttonShape = new NShape();
            buttonShape.Init2DShape();

            // make a button and place it in the shape
            NButton button = new NButton(text);
            buttonShape.Widget = button;

            // bind size to button desired size 
            BindSizeToDesiredSize(buttonShape);

            return buttonShape;
        }
        /// <summary>
        /// Sets the left top corner of a shape
        /// </summary>
        /// <param name="shape"></param>
        /// <param name="location"></param>
        void SetLeftTop(NShape shape, NPoint location)
        {
            // align local pin to left/top corner so pin point can change the location 
            shape.LocPinX = 0;
            shape.LocPinY = 0;
            shape.LocPinRelative = true;

            // set the pin point
            shape.SetPinPoint(location);
        }
        /// <summary>
        /// Sets the right top corner of a shape
        /// </summary>
        /// <param name="shape"></param>
        /// <param name="location"></param>
        void SetRightTop(NShape shape, NPoint location)
        {
            // align local pin to left/top corner so pin point can change the location 
            shape.LocPinX = 1;
            shape.LocPinY = 0;
            shape.LocPinRelative = true;

            // set the pin point
            shape.SetPinPoint(location);
        }
        /// <summary>
        /// Binds the size of the shape to the embedded widget desired size
        /// </summary>
        /// <param name="shape"></param>
        void BindSizeToDesiredSize(NShape shape)
        {
            NWidget widget = shape.Widget;

            // bind shape width to button desired width
            NBindingFx bx = new NBindingFx(widget, NButton.DesiredWidthProperty);
            bx.Guard = true;
            shape.SetFx(NShape.WidthProperty, bx);

            // bind shape height to button desired height
            NBindingFx by = new NBindingFx(widget, NButton.DesiredHeightProperty);
            by.Guard = true;
            shape.SetFx(NShape.HeightProperty, by);

            shape.AllowResizeX = false;
            shape.AllowRotate = false;
            shape.AllowResizeY = false;
        }

        #endregion

        #region Fields

        private NDrawingView m_DrawingView;

        private int m_nSelectedBook = 0;
        private NShape m_SelectedBookImage;
        private NShape m_SelectedBookTitle;
        private NShape m_SelectedBookDescription;
        private NList<NBook> m_Books;
        private NShoppingCart m_ShoppingCart;
        private NContentHolder m_ShoppingCartWidget;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NWidgetHostingExample.
        /// </summary>
        public static readonly NSchema NWidgetHostingExampleSchema;

        #endregion

        #region Nested Types

        class NBook : INDeeplyCloneable
        {
            #region Constructors

            /// <summary>
            /// Initializer contructor
            /// </summary>
            /// <param name="name"></param>
            /// <param name="author"></param>
            /// <param name="description"></param>
            /// <param name="image"></param>
            /// <param name="price"></param>
            public NBook(string name, string author, string description, NImage image, double price)
            {
                Name = name;
                Author = author;
                Description = description;
                Image = image;
                Price = price;
            }
            /// <summary>
            /// Copy constructor
            /// </summary>
            /// <param name="bookInfo"></param>
            public NBook(NBook bookInfo)
            {
                Name = bookInfo.Name;
                Author = bookInfo.Author;
                Description = bookInfo.Description;
                Image = (NImage)bookInfo.Image.DeepClone();
                Price = bookInfo.Price;
            }

            #endregion

            #region Fields

            public readonly string Name;
            public readonly string Author;
            public readonly string Description;
            public readonly NImage Image;
            public readonly double Price;

            #endregion

            #region INDeeplyCloneable

            public object DeepClone()
            {
                return new NBook(this);
            }

            #endregion
        }

        class NShoppingCartItem
        {
            public NShoppingCartItem()
            {
            }

            public string Name;
            public int Quantity;
            public double Price;
            public double GetTotal()
            {
                return Quantity * Price;
            }
        }

        class NShoppingCart
        {
            NList<NShoppingCartItem> Items = new NList<NShoppingCartItem>();

            /// <summary>
            /// Adds a book to the shopping cart.
            /// </summary>
            /// <param name="book"></param>
            /// <param name="example"></param>
            public void AddItem(NBook book, NWidgetHostingExample example)
            {
                // try find an item with the same name. if such exists -> incerase quantity and rebuild shopping cart widget
                for (int i = 0; i < Items.Count; i++)
                {
                    if (Items[i].Name == book.Name)
                    {
                        Items[i].Quantity++;
                        example.m_ShoppingCartWidget.Content = CreateWidget(example);
                        return;
                    }
                }

                // add new item and rebuild shopping cart widget
                NShoppingCartItem item = new NShoppingCartItem();
                item.Name = book.Name;
                item.Price = book.Price;
                item.Quantity = 1;
                Items.Add(item);
                example.m_ShoppingCartWidget.Content = CreateWidget(example);
            }

            public void DeleteItem(NShoppingCartItem item, NWidgetHostingExample example)
            {
                // remove the item and rebuild the shopping cart
                Items.Remove(item);
                example.m_ShoppingCartWidget.Content = CreateWidget(example);
            }

            /// <summary>
            /// Gets the grand total of items
            /// </summary>
            /// <returns></returns>
            public double GetGrandTotal()
            {
                double grandTotal = 0;
                for (int i = 0; i < Items.Count; i++)
                {
                    grandTotal += Items[i].GetTotal();
                }
                return grandTotal;
            }

            /// <summary>
            /// Creates the widget that represents the current state of the shopping cart.
            /// </summary>
            /// <param name="example"></param>
            /// <returns></returns>
            public NWidget CreateWidget(NWidgetHostingExample example)
            {
                if (Items.Count == 0)
                {
                    return new NLabel("The Shopping Cart is Empty");
                }

                const double spacing = 10;
                
                // create the grand total label
                NLabel grandTotalLabel = new NLabel(GetGrandTotal().ToString("0.00"));

                // items table
                NTableFlowPanel tableFlowPanel = new NTableFlowPanel();
                tableFlowPanel.HorizontalSpacing = spacing;
                tableFlowPanel.VerticalSpacing = spacing;
                tableFlowPanel.MaxOrdinal = 5;

                // add headers
                tableFlowPanel.Add(new NLabel("Name"));
                tableFlowPanel.Add(new NLabel("Quantity"));
                tableFlowPanel.Add(new NLabel("Price"));
                tableFlowPanel.Add(new NLabel("Total"));
                tableFlowPanel.Add(new NLabel());

                for (int i = 0; i < Items.Count; i++)
                {
                    NShoppingCartItem item = Items[i];

                    // name
                    NLabel nameLabel = new NLabel(item.Name);
                    NLabel priceLabel = new NLabel(item.Price.ToString("0.00"));
                    NLabel totalLabel = new NLabel(item.GetTotal().ToString("0.00"));

                    // quantity
                    NNumericUpDown quantityNud = new NNumericUpDown();
                    quantityNud.Value = item.Quantity;
                    quantityNud.DecimalPlaces = 0;
                    quantityNud.Minimum = 0;
                    quantityNud.ValueChanged += delegate(NValueChangeEventArgs args) { 
                        item.Quantity = (int)((NNumericUpDown)args.TargetNode).Value;
                        totalLabel.Text = item.GetTotal().ToString("0.00");
                        grandTotalLabel.Text = GetGrandTotal().ToString("0.00");
                    };

                    NButton deleteButton = new NButton("Delete");
                    deleteButton.Click += delegate(NEventArgs args){
                        DeleteItem(item, example);
                    };

                    tableFlowPanel.Add(nameLabel);
                    tableFlowPanel.Add(quantityNud);
                    tableFlowPanel.Add(priceLabel);
                    tableFlowPanel.Add(totalLabel);
                    tableFlowPanel.Add(deleteButton);
                }

                // add grand total
                tableFlowPanel.Add(new NLabel("Grand Total"));
                tableFlowPanel.Add(new NLabel());
                tableFlowPanel.Add(new NLabel());
                tableFlowPanel.Add(grandTotalLabel);
                tableFlowPanel.Add(new NLabel());

                return tableFlowPanel;
            }
        }
        
        #endregion
    }
}