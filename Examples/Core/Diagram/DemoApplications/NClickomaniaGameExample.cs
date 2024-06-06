using System;

using Nevron.Nov.DataStructures;
using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Themes;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Text;
using Nevron.Nov.TrueType;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NClickomaniaGameExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NClickomaniaGameExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NClickomaniaGameExample()
		{
			NClickomaniaGameExampleSchema = NSchema.Create(typeof(NClickomaniaGameExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_DrawingView = new NDrawingView();
			m_DrawingView.Document.HistoryService.Pause();

            // hide grid and ports
            m_DrawingView.Content.ScreenVisibility.ShowGrid = false;
            m_DrawingView.Content.ScreenVisibility.ShowPorts = false;

            NStyleSheet styleSheet = CreateStyles();
			m_DrawingView.Document.StyleSheets.Add(styleSheet);

			NPage page = m_DrawingView.ActivePage;

			page.Items.Add(CreateTitleShape("Clickomania"));
			page.Items.Add(CreateInfoShape());
			page.Items.Add(CreateBoardShape());

			page.SizeToContent();

			m_CellsToClear = new NList<NPointI>();
			m_Score = 0;
			UpdateInfo();

			m_Timer = new NTimer(ClearCellsDelay);
			m_Timer.Tick += OnTimerTick;

			return m_DrawingView;
		}
		protected override NWidget CreateExampleControls()
		{
			return null;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	The example demonstrates how to use table blocks and how to handle events in NOV Diagram.
	We've implemented the popular Clickomania game using a table block and table cell
	mouse down events.
</p>
<p>
    Clickomania is a one-player game (puzzle) with the following rules. The board is a
    rectangular grid (this is where the table block support comes in handy). Initially the board
    is full of square blocks each colored one of k colors. A group is a collection of
    squares connected along edges that all have the same color. At any step, the player
    can click any group of size at least two. This move causes those blocks to disappear,
    and any blocks stacked above them fall straight down as far as they can. This falling
    is similar to Tetris, but each column falls independently. In particular, we never leave
    an internal hole. One final twist on the rules is that, if an entire column becomes empty
    of blocks, then the left and right components are slid together to remove the vertical hole. 
</p>";
		}

		#endregion

		#region Implementation - Shapes

		/// <summary>
		/// Creates the cell styles.
		/// </summary>
		/// <returns></returns>
		private NStyleSheet CreateStyles()
		{
			NStyleSheet styleSheet = new NStyleSheet();

			// Create the default cell style
			{
				NRule rule = styleSheet.CreateRule(
				sb =>
				{
					sb.Type(NTableCell.NTableCellSchema);
				});

				rule.AddValueDeclaration(NTextElement.BackgroundFillProperty, new NColorFill(NColor.White));
				rule.AddValueDeclaration(NTableCell.VerticalAlignmentProperty, ENVAlign.Center);
			}

			// Create the cell classes
			for (int i = 0; i < CellClasses.Length; i++)
			{
				string className = CellClasses[i];
				NRule rule = styleSheet.CreateRule(
					sb =>
					{
						sb.Type(NTableCell.NTableCellSchema);
						sb.UserClass(className);
					});

				NColor color = NColor.Parse(className);
				NRadialGradientFill gradient = new NRadialGradientFill();
				gradient.GradientStops.Add(new NGradientStop(0, color));
				gradient.GradientStops.Add(new NGradientStop(0.8, color.Darken()));
				gradient.GradientStops.Add(new NGradientStop(1, NColor.White));
				rule.AddValueDeclaration(NTextElement.BackgroundFillProperty, gradient);
			}

			// Add the highlighted cell classes
			{
				NRule rule = styleSheet.CreateRule(
					sb =>
					{
						sb.Type(NTableCell.NTableCellSchema);
						sb.UserClass(HighlightedClassName);
					});

				rule.AddValueDeclaration(NTextElement.BackgroundFillProperty, new NHatchFill(ENHatchStyle.DiagonalCross, NColor.DarkRed, NColor.White));
			}

			return styleSheet;
		}

		/// <summary>
		/// Creates the Title shape.
		/// </summary>
		/// <param name="title"></param>
		/// <returns></returns>
		private NShape CreateTitleShape(string title)
		{
			NDrawingTheme theme = NDrawingTheme.MyDrawNature;

			NShape titleShape = new NShape();
			titleShape.SetBounds(0, 0, 500, 50);
			titleShape.Text = title;
			titleShape.SetProtectionMask(ENDiagramItemOperationMask.All);

			NTextBlock titleTextBlock = (NTextBlock)titleShape.TextBlock;
			titleTextBlock.ResizeMode = ENTextBlockResizeMode.ShapeSize;
			titleTextBlock.FontSize = 28;
			titleTextBlock.FontStyleBold = true;

			// Set theme-based colors to the title text, so that it changes when the user changes the theme
			NColor strokeColor = theme.ColorPalette.Variants[0][0];
			strokeColor.Tag = new NThemeVariantColorInfo(0);
			titleTextBlock.Stroke = new NStroke(strokeColor);

			NColor fillColor = theme.ColorPalette.Variants[0][4];
			fillColor.Tag = new NThemeVariantColorInfo(4);
			titleTextBlock.Fill = new NColorFill(fillColor);

			// Alternatively, you can also use fixed colors (uncomment the 2 lines below)
			//titleTextBlock.Stroke = new NStroke(NColor.DarkBlue);
			//titleTextBlock.Fill = new NColorFill(NColor.LightBlue);

			// Set an expression to center the title horizontally in the page
			titleShape.SetFx(NShape.PinXProperty, new NFormulaFx("$ParentSheet.X + $ParentSheet.Width / 2", true));

			return titleShape;
		}
		/// <summary>
		/// Creates the Board shape.
		/// </summary>
		/// <returns></returns>
		private NShape CreateBoardShape()
		{
			NShape boardShape = new NShape();
			boardShape.SetBounds(200, 100, 600, 600);
			boardShape.SetProtectionMask(ENDiagramItemOperationMask.All);

			NTableBlock tableBlock = new NTableBlock();
			tableBlock.ResizeMode = ENTableBlockResizeMode.FitToShape;
			tableBlock.PortsDistributionMode = ENPortsDistributionMode.None;
			boardShape.TextBlock = tableBlock;

			m_Table = new NTableBlockContent(BoardSize.Height, BoardSize.Width,
				NBorder.CreateFilledBorder(TableBorderColor), TableBorderThickness);
			m_Table.AllowSpacingBetweenCells = false;
			tableBlock.Content = m_Table;

			// Create the cells
			Random random = new Random();
			for (int i = 0; i < m_Table.Rows.Count; i++)
			{
				NTableRow row = m_Table.Rows[i];
				for (int j = 0; j < m_Table.Columns.Count; j++)
				{
					NTableCell cell = row.Cells[j];
					cell.UserClass = CellClasses[random.Next(CellClasses.Length)];
					cell.MouseDown += OnCellMouseDown;
				}
			}

			return boardShape;
		}
		/// <summary>
		/// Creates the info shape.
		/// </summary>
		/// <returns></returns>
		private NShape CreateInfoShape()
		{
			NShape infoShape = new NShape();
			infoShape.SetBounds(0, 240, 120, 240);
			infoShape.SetProtectionMask(ENDiagramItemOperationMask.All);

			NTableBlock tableBlock = new NTableBlock();
			tableBlock.ResizeMode = ENTableBlockResizeMode.FitToShape;
			tableBlock.PortsDistributionMode = ENPortsDistributionMode.None;
			infoShape.TextBlock = tableBlock;

			m_InfoTable = new NTableBlockContent(CellClasses.Length + 2, 2,
				NBorder.CreateFilledBorder(TableBorderColor), TableBorderThickness);
			m_InfoTable.AllowSpacingBetweenCells = false;
			m_InfoTable.FontSize = 16;
			tableBlock.Content = m_InfoTable;

			m_InfoTable.Columns[0].PreferredWidth = NMultiLength.NewFixed(40);

			m_InfoTable.Rows[0].Cells[0].Blocks[0] = new NParagraph("Cells:");
			m_InfoTable.Rows[0].Cells[0].ColSpan = 2;

			for (int i = 0; i < CellClasses.Length; i++)
			{
				m_InfoTable.Rows[i + 1].Cells[0].UserClass = CellClasses[i];
			}

			m_InfoTable.Rows[CellClasses.Length + 1].Cells[0].Blocks[0] = new NParagraph("Score: 0");
			m_InfoTable.Rows[CellClasses.Length + 1].Cells[0].ColSpan = 2;

			return infoShape;
		}

		#endregion

		#region Implementation - Game

		/// <summary>
		/// Gets the UserClass of the cell at the given address.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		private string GetCellClass(int x, int y)
		{
			return m_Table.Rows[y].Cells[x].UserClass;
		}

		/// <summary>
		/// Checks if a column is empty from the given row index to the top.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		private bool IsEmptyToTop(int x, int y)
		{
			for (int i = y; i >= 0; i--)
			{
				if (!String.IsNullOrEmpty(GetCellClass(x, i)))
					return false;
			}

			return true;
		}
		/// <summary>
		/// Applies top to bottom and right to left gravity forces to the table.
		/// </summary>
		private void ApplyGravity()
		{
			int i, j, z;
			int rowCount = m_Table.Rows.Count;
			int columnCount = m_Table.Columns.Count;

			// Top to bottom gravity force
			for (j = 0; j < columnCount; j++)
			{
				for (i = rowCount - 1; i > 0; i--)
				{
					if (String.IsNullOrEmpty(GetCellClass(j, i)))
					{   
						// Shift the column down by 1 cell
						if (IsEmptyToTop(j, i - 1))
							break;

						for (z = i; z > 0; z--)
						{
							m_Table.Rows[z].Cells[j].UserClass = m_Table.Rows[z - 1].Cells[j].UserClass;
						}

						m_Table.Rows[0].Cells[j].ClearLocalValue(UserClassProperty);
						i++;
					}
				}
			}

			// Right to left gravity force
			for (j = columnCount - 2; j >= 0; j--)
			{
				if (IsEmptyToTop(j, rowCount - 1))
				{
					// Shift columns to the left
					for (i = 0; i < rowCount; i++)
					{
						for (z = j; z < columnCount - 1; z++)
						{
							m_Table.Rows[i].Cells[z].UserClass = m_Table.Rows[i].Cells[z + 1].UserClass;
						}

						m_Table.Rows[i].Cells[z].ClearLocalValue(UserClassProperty);
					}
				}
			}
		}
		/// <summary>
		/// Returns true if all cells are cleared.
		/// </summary>
		/// <returns></returns>
		private bool AllClear()
		{
			int rowCount = m_Table.Rows.Count;
			int columnCount = m_Table.Columns.Count;

			for (int i = rowCount - 1; i >= 0; i--)
			{
				for (int j = 0; j < columnCount; j++)
				{
					if (!String.IsNullOrEmpty(GetCellClass(j, i)))
						return false;
				}
			}

			return true;
		}
		/// <summary>
		/// Returns true if there are no more regions to remove.
		/// </summary>
		/// <returns></returns>
		private bool GameOver()
		{
			int rowCount = m_Table.Rows.Count;
			int columnCount = m_Table.Columns.Count;
			NTableCell cell;

			for (int i = rowCount - 1; i >= 0; i--)
			{
				for (int j = 0; j < columnCount; j++)
				{
					cell = m_Table.Rows[i].Cells[j];
					if (!String.IsNullOrEmpty(cell.UserClass))
					{
						if (Test(j, i, cell.UserClass))
							return false;
					}
				}
			}

			return true;
		}

		/// <summary>
		/// Tests if a region of at least 2 cells with the same color is clicked.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="color"></param>
		/// <returns></returns>
		private bool Test(int x, int y, string color)
		{
			if (x > 0 && GetCellClass(x - 1, y) == color)
				return true;

			if (x < m_Table.Columns.Count - 1 && GetCellClass(x + 1, y) == color)
				return true;

			if (y > 0 && GetCellClass(x, y - 1) == color)
				return true;

			if (y < m_Table.Rows.Count - 1 && GetCellClass(x, y + 1) == color)
				return true;

			return false;
		}
		/// <summary>
		/// Accumulates all cells of the same color adjacent to the given cell.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="color"></param>
		/// <param name="cellAddresses"></param>
		private void HighlightCellsOfSameColor(int x, int y, string color, NList<NPointI> cellAddresses)
		{
			NPointI address = new NPointI(x, y);
			cellAddresses.AddNoDuplicates(address);
			m_Table.Rows[y].Cells[x].UserClass = HighlightedClassName;

			if (x > 0 && GetCellClass(x - 1, y) == color)
				HighlightCellsOfSameColor(x - 1, y, color, cellAddresses);

			if (x < m_Table.Columns.Count - 1 && GetCellClass(x + 1, y) == color)
				HighlightCellsOfSameColor(x + 1, y, color, cellAddresses);

			if (y > 0 && GetCellClass(x, y - 1) == color)
				HighlightCellsOfSameColor(x, y - 1, color, cellAddresses);

			if (y < m_Table.Rows.Count - 1 && GetCellClass(x, y + 1) == color)
				HighlightCellsOfSameColor(x, y + 1, color, cellAddresses);
		}

		/// <summary>
		/// Clears the given cells.
		/// </summary>
		/// <param name="cellAddresses"></param>
		private void ClearCells(NList<NPointI> cellAddresses)
		{
			// Clear the adjacent cells of the same color
			for (int i = 0; i < cellAddresses.Count; i++)
			{
				m_Table.Rows[cellAddresses[i].Y].Cells[cellAddresses[i].X].ClearLocalValue(UserClassProperty);
			}

			int n = cellAddresses.Count - 1;
			m_Score += n * n;

			ApplyGravity();

			bool gameOver = false;
			if (AllClear())
			{
				// All cells were cleared
				m_Score *= 2;
				gameOver = true;
			}
			else if (GameOver())
			{
				// No more cells can be cleared
				gameOver = true;
			}

			UpdateInfo(gameOver);
		}

		/// <summary>
		/// Updates the cells of the info table.
		/// </summary>
		/// <param name="gameOver"></param>
		private void UpdateInfo(bool gameOver = false)
		{
			int rowCount = m_Table.Rows.Count;
			int columnCount = m_Table.Columns.Count;

			// Update the cell count
			int[] cellCount = new int[CellClasses.Length];
			for (int i = rowCount - 1; i >= 0; i--)
			{
				for (int j = 0; j < columnCount; j++)
				{
					string color = GetCellClass(j, i);
					int index = Array.IndexOf(CellClasses, color);
					if (index != -1)
					{
						cellCount[index]++;
					}
				}
			}

			for (int i = 0; i < cellCount.Length; i++)
			{
				m_InfoTable.Rows[i + 1].Cells[1].Blocks[0] = new NParagraph(cellCount[i].ToString());
			}

			// Update the score
			m_InfoTable.Rows[CellClasses.Length + 1].Cells[0].Blocks[0] = new NParagraph($"Score: {m_Score}");

			if (gameOver)
			{
				NShape boardShape = m_Table.GetFirstAncestor<NShape>();

				NShape gameOverShape = CreateTitleShape("Game Over");
				gameOverShape.Geometry.AddRelative(new NDrawRectangle(0, 0, 1, 1));
				gameOverShape.Geometry.Fill = new NColorFill(new NColor(0xA0FFFFFF));
				gameOverShape.SetProtectionMask(ENDiagramItemOperationMask.All);

				boardShape.OwnerPage.Items.Add(gameOverShape);
				gameOverShape.PinX = boardShape.PinX;
				gameOverShape.PinY = boardShape.PinY;
			}
		}

		#endregion

		#region Event Handlers

		private void OnCellMouseDown(NMouseButtonEventArgs arg)
		{
			if (m_CellsToClear.Count > 0)
				return;

			if (arg.Cancel || arg.Button != ENMouseButtons.Left)
				return;

			// Get the clicked cell and mark the event as handled
			NTableCell cell = (NTableCell)arg.CurrentTargetNode;
			arg.Cancel = true;

			if (String.IsNullOrEmpty(cell.UserClass))
				return;

			int x = cell.Column.GetAggregationInfo().Index;
			int y = cell.Row.GetAggregationInfo().Index;
			if (!Test(x, y, cell.UserClass))
				return;

			// Highlight the adjacent cells of same color
			HighlightCellsOfSameColor(x, y, cell.UserClass, m_CellsToClear);
			m_Timer.Start();
		}

		private void OnTimerTick()
		{
			m_Timer.Stop();

			ClearCells(m_CellsToClear);

			m_CellsToClear.Clear();
		}

		#endregion

		#region Fields

		private NDrawingView m_DrawingView;
		private NTableBlockContent m_Table;
		private NTableBlockContent m_InfoTable;
		private NList<NPointI> m_CellsToClear;
		private int m_Score;
		private NTimer m_Timer;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NClickomaniaGameExample.
		/// </summary>
		public static readonly NSchema NClickomaniaGameExampleSchema;

		#endregion

		#region Constants

		private static readonly NSizeI BoardSize = new NSizeI(15, 15);
		private static readonly NColor TableBorderColor = NColor.DimGray;
		private static readonly NMargins TableBorderThickness = new NMargins(1);

		private const int ClearCellsDelay = 1000;
		private const string HighlightedClassName = "Highlighted";

		private static readonly string[] CellClasses = new string[]
		{
			"Red",
			"Blue",
			"Green",
			"Gold"
		};

		#endregion
	}
}