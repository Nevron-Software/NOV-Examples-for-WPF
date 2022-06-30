using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Text;
using Nevron.Nov.UI;
using Nevron.Nov.DataStructures;

namespace Nevron.Nov.Examples.Text
{
	/// <summary>
	/// The example demonstrates how to programmatically create inline elements with different formatting
	/// </summary>
	public class NWidgetInlinesExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public NWidgetInlinesExample()
		{
		}
		/// <summary>
		/// 
		/// </summary>
		static NWidgetInlinesExample()
		{
			NWidgetInlinesExampleSchema = NSchema.Create(typeof(NWidgetInlinesExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create the rich text
			NRichTextViewWithRibbon richTextWithRibbon = new NRichTextViewWithRibbon();
			m_RichText = richTextWithRibbon.View;
			m_RichText.AcceptsTab = true;
			m_RichText.Content.Sections.Clear();

			// Populate the rich text
			PopulateRichText();

			return richTextWithRibbon;
		}
		protected override NWidget CreateExampleControls()
		{
			return null;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>This example demonstrates how to use widgets in order to create ""HTML like"" interfaces. The example also demonstrates how to use style sheets.</p>
<p>Press the ""Show Prev Book"" and ""Show Next Buttons"" buttons to browse through the available books.</p>
<p>Press the ""Add to Cart"" button to add the currently selected book to the shopping cart.</p>
<p>Press the ""Delete"" button to remove a book from the shopping cart.</p>
<p>Use the combo box to select the quantity of books to purchase.</p>
";
		}

		private void PopulateRichText()
		{
			m_Books = new NBookInfoList();
			m_CurrentBookIndex = 0;

			NSection section = new NSection();
			m_RichText.Content.Sections.Add(section);
			m_RichText.Document.StyleSheets.Add(CreateShoppingCartStyleSheet());

			{
				NTable navigationTable = new NTable(1, 2);

				NTableCell cell0 = navigationTable.Rows[0].Cells[0];

				NButton showPrevBookButton = new NButton("Show Prev Book");
				showPrevBookButton.Click += new Function<NEventArgs>(OnShowPrevBookButtonClick);

				cell0.Blocks.Clear();
				cell0.Blocks.Add(CreateWidgetParagraph(showPrevBookButton));

				NTableCell cell1 = navigationTable.Rows[0].Cells[1];

				NButton showNextBookButton = new NButton("Show Next Book");
				showNextBookButton.Click += new Function<NEventArgs>(OnShowNextBookButtonClick);

				cell1.Blocks.Clear();
				cell1.Blocks.Add(CreateWidgetParagraph(showNextBookButton));

				section.Blocks.Add(navigationTable);
			}

			m_BookInfoPlaceHolder = new NGroupBlock();
			section.Blocks.Add(m_BookInfoPlaceHolder);

			{
				m_BookInfoPlaceHolder.Blocks.Add(CreateBookContent(m_Books[0]));
			}

			{
				m_ShoppingCartPlaceHolder = new NGroupBlock();
				AddEmptyShoppingCartText();

				section.Blocks.Add(m_ShoppingCartPlaceHolder);
			}
		}

		#endregion

		#region Implementation

		/// <summary>
		/// 
		/// </summary>
		void AddEmptyShoppingCartText()
		{
			m_ShoppingCartPlaceHolder.Blocks.Clear();
			m_ShoppingCartPlaceHolder.Blocks.Add(new NParagraph("Shopping Cart Is Empty"));
		}
		/// <summary>
		/// Creates a style sheet for the shopping cart table
		/// </summary>
		/// <returns></returns>
		NStyleSheet CreateShoppingCartStyleSheet()
		{
			NStyleSheet styleSheet = new NStyleSheet();

			NRule rule = new NRule();

			for (int i = 0; i < 3; i++)
			{
				NSelectorBuilder sb = rule.GetSelectorBuilder();
				sb.Start();

				sb.Type(NTableCell.NTableCellSchema);

				// in case of the first or last row selector -> must not be last cell
				if (i == 0 || i == 2)
				{
					sb.StartInvertedConditions();
					sb.LastChild();
					sb.EndInvertedConditions();
				}

				// descendant of table row
				sb.DescendantOf();
				sb.Type(NTableRow.NTableRowSchema);

				switch (i)
				{
					case 0:
						// descendant of first row
						sb.FirstChild();
						break;

					case 1:
						// middle cells
						sb.StartInvertedConditions();
						sb.FirstChild();
						sb.LastChild();
						sb.EndInvertedConditions();
						break;

					case 2:
						// descendant of last row
						sb.LastChild();
						break;
				}

				// descendant of table
				sb.DescendantOf();
				sb.Type(NTable.NTableSchema);
				sb.ValueEquals(NTable.TagProperty, "ShoppingCart");

				sb.End();
			}

			rule.Declarations.Add(new NValueDeclaration<NMargins>(NTableCell.BorderThicknessProperty, new NMargins(1)));
			rule.Declarations.Add(new NValueDeclaration<NBorder>(NTableCell.BorderProperty, NBorder.CreateFilledBorder(NColor.Black)));

			styleSheet.Add(rule);

			return styleSheet;
		}
		/// <summary>
		/// Creates a table based on the book info
		/// </summary>
		/// <param name="bookInfo"></param>
		/// <returns></returns>
		Nevron.Nov.Text.NBlock CreateBookContent(NBookInfo bookInfo)
		{
			NTable table = new NTable(4, 2);

			// Create the image
			NTableCell imageTableCell = table.Rows[0].Cells[0];
			imageTableCell.RowSpan = int.MaxValue;
			imageTableCell.Blocks.Clear();
			imageTableCell.Blocks.Add(CreateImageParagraph(bookInfo.Image));

			NTableCell titleTableCell = table.Rows[0].Cells[1];
			titleTableCell.Blocks.Clear();
			titleTableCell.Blocks.Add(CreateTitleParagraph(bookInfo.Name));

			NTableCell descriptionTableCell = table.Rows[1].Cells[1];
			descriptionTableCell.Blocks.Clear();
			descriptionTableCell.Blocks.Add(CreateDescriptionParagraph(bookInfo.Description));

			NTableCell authorTableCell = table.Rows[2].Cells[1];
			authorTableCell.Blocks.Clear();
			authorTableCell.Blocks.Add(CreateAuthorParagraph(bookInfo.Author));

			NTableCell addToCartTableCell = table.Rows[3].Cells[1];
			addToCartTableCell.RowSpan = int.MaxValue;
			addToCartTableCell.Blocks.Clear();

			NButton addToCartButton = new NButton("Add To Cart");
			addToCartButton.Click += new Function<NEventArgs>(OnAddTableRow);
			addToCartTableCell.VerticalAlignment = ENVAlign.Bottom;
			addToCartTableCell.Blocks.Add(CreateWidgetParagraph(addToCartButton));

			return table;
		}
		/// <summary>
		/// Loads the current book info in the book place holder group
		/// </summary>
		void LoadBookInfo()
		{
			m_BookInfoPlaceHolder.Blocks.Clear();
			m_BookInfoPlaceHolder.Blocks.Add(CreateBookContent(m_Books[m_CurrentBookIndex]));
		}
		/// <summary>
		/// Creates the author paragraph
		/// </summary>
		/// <param name="author"></param>
		/// <returns></returns>
		NParagraph CreateAuthorParagraph(string author)
		{
			return new NParagraph("Author: " + author);
		}
		/// <summary>
		/// Creates teh description paragraph
		/// </summary>
		/// <param name="title"></param>
		/// <returns></returns>
		NParagraph CreateDescriptionParagraph(string title)
		{
			NParagraph paragraph = new NParagraph(title);

			paragraph.BackgroundFill = new NColorFill(NColor.WhiteSmoke);

			return paragraph;
		}
		/// <summary>
		/// Creates the title paragraph
		/// </summary>
		/// <param name="title"></param>
		/// <returns></returns>
		NParagraph CreateTitleParagraph(string title)
		{
			NParagraph paragraph = new NParagraph(title);

			paragraph.FontSize = 18;
			paragraph.Fill = new NColorFill(NColor.RoyalBlue);

			return paragraph;
		}
		/// <summary>
		/// Creates the book image paragraph
		/// </summary>
		/// <param name="image"></param>
		/// <returns></returns>
		NParagraph CreateImageParagraph(NImage image)
		{
			NParagraph paragraph = new NParagraph();

			NImageInline inline = new NImageInline();
			inline.Image = (NImage)image.DeepClone();
			inline.PreferredWidth = new NMultiLength(ENMultiLengthUnit.Dip, 200);
			inline.PreferredHeight = new NMultiLength(ENMultiLengthUnit.Dip, 280);
			paragraph.Inlines.Add(inline);

			return paragraph;
		}
		/// <summary>
		/// Creates a paragraph that contains a widget
		/// </summary>
		/// <param name="widget"></param>
		/// <returns></returns>
		NParagraph CreateWidgetParagraph(NWidget widget)
		{
			NParagraph paragraph = new NParagraph();

			NWidgetInline inline = new NWidgetInline();
			inline.Content = widget;
			paragraph.Inlines.Add(inline);

			return paragraph;
		}
		/// <summary>
		/// Creates a combo that shows the 
		/// </summary>
		/// <returns></returns>
		NComboBox CreateQuantityCombo()
		{
			NComboBox combo = new NComboBox();

			for (int i = 0; i < 9; i++)
			{
				combo.Items.Add(new NComboBoxItem((i + 1).ToString()));
			}

			combo.SelectedIndex = 0;
			combo.SelectedIndexChanged +=new Function<NValueChangeEventArgs>(OnQuantityComboSelectedIndexChanged);

			return combo;
		}
		/// <summary>
		/// Adds a book row to the shopping cart
		/// </summary>
		void AddBookRow()
		{
			NBookInfo bookInfo = m_Books[m_CurrentBookIndex];

			NTableRow bookRow = new NTableRow();
			bookRow.Tag = bookInfo;

			NTableCell nameCell = new NTableCell();
			nameCell.Blocks.Add(new NParagraph(bookInfo.Name));
			bookRow.Cells.Add(nameCell);

			NTableCell quantityCell = new NTableCell();
			quantityCell.Blocks.Add(CreateWidgetParagraph(CreateQuantityCombo()));
			bookRow.Cells.Add(quantityCell);

			NTableCell priceCell = new NTableCell();
			priceCell.Blocks.Add(new NParagraph(bookInfo.Price.ToString()));
			bookRow.Cells.Add(priceCell);

			NTableCell totalCell = new NTableCell();
			totalCell.Blocks.Add(new NParagraph());
			bookRow.Cells.Add(totalCell);

			NTableCell deleteCell = new NTableCell();
			NButton deleteRowButton = new NButton("Delete");
			deleteRowButton.Click += new Function<NEventArgs>(OnDeleteRowButtonClick);
			deleteCell.Blocks.Add(CreateWidgetParagraph(deleteRowButton));
			bookRow.Cells.Add(deleteCell);

			m_CartTable.Rows.Insert(m_CartTable.Rows.Count - 1, bookRow);
		}
		/// <summary>
		/// Adds a total row to the shopping cart
		/// </summary>
		void AddTotalRow()
		{
			NTableRow totalRow = m_CartTable.Rows.CreateNewRow();

			NTableCell totalCell = totalRow.Cells[0];
			totalCell.Blocks.Clear();
			totalCell.ColSpan = 3;
			totalCell.Blocks.Add(new NParagraph("Grand Total:"));

			m_CartTable.Rows.Add(totalRow);
		}
		/// <summary>
		/// Updates the total values in the shopping cart
		/// </summary>
		void UpdateTotals()
		{
			if (m_CartTable == null || m_CartTable.Columns.Count != 5)
				return;

			double grandTotal = 0;

			// sum all book info price * quantity
			for (int i = 0; i < m_CartTable.Rows.Count; i++)
			{
				NTableRow row = m_CartTable.Rows[i];
				NBookInfo bookInfo = row.Tag as NBookInfo;

				if (bookInfo != null)
				{
					NVFlowBlockCollection<Nevron.Nov.Text.NBlock> blocks = row.Cells[1].Blocks;

					NComboBox combo = (NComboBox)blocks.GetFirstDescendant(new NInstanceOfSchemaFilter(NComboBox.NComboBoxSchema));

					if (combo != null)
					{
						double total = (combo.SelectedIndex + 1) * bookInfo.Price;

						row.Cells[3].Blocks.Clear();
						row.Cells[3].Blocks.Add(new NParagraph(total.ToString()));

						grandTotal += total;
					}
				}
			}

			NTableCell grandTotalCell = m_CartTable.Rows[m_CartTable.Rows.Count - 1].Cells[3];
			grandTotalCell.Blocks.Clear();
			grandTotalCell.Blocks.Add(new NParagraph(grandTotal.ToString()));
		}

		#endregion

		#region Event Handlers

		/// <summary>
		/// Called when a new row must be added to the shopping cart
		/// </summary>
		/// <param name="arg"></param>
		void OnAddTableRow(NEventArgs arg)
		{
			if (m_CartTable == null)
			{
				m_CartTable = new NTable(1, 5);
				m_CartTable.Tag = "ShoppingCart";
				m_CartTable.AllowSpacingBetweenCells = false;

				m_ShoppingCartPlaceHolder.Blocks.Clear();
				m_ShoppingCartPlaceHolder.Blocks.Add(m_CartTable);

				NTableCell nameCell = m_CartTable.Rows[0].Cells[0];
				nameCell.Blocks.Clear();
				nameCell.Blocks.Add(new NParagraph("Name"));

				NTableCell quantity = m_CartTable.Rows[0].Cells[1];
				quantity.Blocks.Clear();
				quantity.Blocks.Add(new NParagraph("Quantity"));

				NTableCell priceCell = m_CartTable.Rows[0].Cells[2];
				priceCell.Blocks.Clear();
				priceCell.Blocks.Add(new NParagraph("Price"));

				NTableCell totalCell = m_CartTable.Rows[0].Cells[3];
				totalCell.Blocks.Clear();
				totalCell.Blocks.Add(new NParagraph("Total"));

				NTableCell deleteCell = m_CartTable.Rows[0].Cells[4];
				deleteCell.Blocks.Clear();

				AddTotalRow();
			}

			AddBookRow();

			UpdateTotals();
		}
		/// <summary>
		/// Called when a row must be deleted from the shopping cart
		/// </summary>
		/// <param name="arg"></param>
		void OnDeleteRowButtonClick(NEventArgs arg)
		{
			NTableRow tableRow = (NTableRow)arg.TargetNode.GetFirstAncestor(NTableRow.NTableRowSchema);

			m_CartTable.Rows.Remove(tableRow);

			if (m_CartTable.Rows.Count == 2)
			{
				m_CartTable.Rows.RemoveAt(m_CartTable.Rows.Count - 1);
				m_CartTable = null;

				AddEmptyShoppingCartText();
			}

			UpdateTotals();
		}
		/// <summary>
		/// Called when the quantity combo for a row has changed
		/// </summary>
		/// <param name="arg"></param>
		void OnQuantityComboSelectedIndexChanged(NEventArgs arg)
		{
			UpdateTotals();
		}
		/// <summary>
		/// Called to load the next book in the list
		/// </summary>
		/// <param name="arg"></param>
		void OnShowNextBookButtonClick(NEventArgs arg)
		{
			if (m_CurrentBookIndex >= m_Books.Count - 1)
			{
				return;
			}

			m_CurrentBookIndex++;
			LoadBookInfo();
		}
		/// <summary>
		/// Called to load the prev book in the list
		/// </summary>
		/// <param name="arg"></param>
		void OnShowPrevBookButtonClick(NEventArgs arg)
		{
			if (m_CurrentBookIndex <= 0)
			{
				return;
			}

			m_CurrentBookIndex--;
			LoadBookInfo();
		}

		#endregion

		#region Fields

		private NRichTextView m_RichText;
		private NBookInfoList m_Books;
		private NGroupBlock m_BookInfoPlaceHolder;
		private NGroupBlock m_ShoppingCartPlaceHolder;
		private NTable m_CartTable;
		private int m_CurrentBookIndex;

		#endregion

		#region Schema

		public static readonly NSchema NWidgetInlinesExampleSchema;

		#endregion

		#region Nested Types

		private class NBookInfo : INDeeplyCloneable
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
			public NBookInfo(string name, string author, string description, NImage image, double price)
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
			public NBookInfo(NBookInfo bookInfo)
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
				return new NBookInfo(this);
			}

			#endregion
		}

		private class NBookInfoList : NList<NBookInfo>
		{
			public NBookInfoList()
			{
				Add(new NBookInfo("The Name Of The Wind", "Patrick Rothfuss", "This is the riveting first-person narrative of Kvothe, a young man who grows to be one of the most notorious magicians his world has ever seen. From his childhood in a troupe of traveling players, to years spent as a near-feral orphan in a crime-riddled city, to his daringly brazen yet successful bid to enter a legendary school of magic, The Name of the Wind is a masterpiece that transports readers into the body and mind of a wizard.", NResources.Image_Books_NameOfTheWind_jpg, 12.90));
				Add(new NBookInfo("Lord of Ohe Rings", "J.R.R. Tolkien", "In ancient times the Rings of Power were crafted by the Elven-smiths, and Sauron, the Dark Lord, forged the One Ring, filling it with his own power so that he could rule all others. But the One Ring was taken from him, and though he sought it throughout Middle-earth, it remained lost to him. After many ages it fell by chance into the hands of the hobbit Bilbo Baggins.", NResources.Image_Books_LordOfTheRings_jpg, 13.99));
				Add(new NBookInfo("A Game Of Thrones", "George R.R. Martin", "Long ago, in a time forgotten, a preternatural event threw the seasons out of balance. In a land where summers can last decades and winters a lifetime, trouble is brewing. The cold is returning, and in the frozen wastes to the north of Winterfell, sinister and supernatural forces are massing beyond the kingdom’s protective Wall. At the center of the conflict lie the Starks of Winterfell, a family as harsh and unyielding as the land they were born to.", NResources.Image_Books_AGameOfThrones_jpg, 12.79));
				Add(new NBookInfo("The Way Of Kings", "Brandon Sanderson", "Roshar is a world of stone and storms. Uncanny tempests of incredible power sweep across the rocky terrain so frequently that they have shaped ecology and civilization alike. Animals hide in shells, trees pull in branches, and grass retracts into the soilless ground. Cities are built only where the topography offers shelter.", NResources.Image_Books_TheWayOfKings_jpg, 7.38));
				Add(new NBookInfo("Mistborn", "Brandon Sanderson", "For a thousand years the ash fell and no flowers bloomed. For a thousand years the Skaa slaved in misery and lived in fear. For a thousand years the Lord Ruler, the 'Sliver of Infinity' reigned with absolute power and ultimate terror, divinely invincible. Then, when hope was so long lost that not even its memory remained, a terribly scarred, heart-broken half-Skaa rediscovered it in the depths of the Lord Ruler’s most hellish prison. Kelsier 'snapped' and found in himself the powers of a Mistborn. A brilliant thief and natural leader, he turned his talents to the ultimate caper, with the Lord Ruler himself as the mark. ", NResources.Image_Books_Mistborn_jpg, 6.38));
			}
		}

		#endregion
	}
}