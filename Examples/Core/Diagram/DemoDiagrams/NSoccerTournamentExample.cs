using Nevron.Nov.DataStructures;
using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Layout;
using Nevron.Nov.Diagram.Themes;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.Text;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NSoccerTournamentExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NSoccerTournamentExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NSoccerTournamentExample()
		{
			NSoccerTournamentExampleSchema = NSchema.Create(typeof(NSoccerTournamentExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create a simple drawing
			NDrawingViewWithRibbon drawingViewWithRibbon = new NDrawingViewWithRibbon();
			m_DrawingView = drawingViewWithRibbon.View;

            // hide grid and ports
            m_DrawingView.Content.ScreenVisibility.ShowGrid = false;
            m_DrawingView.Content.ScreenVisibility.ShowPorts = false;

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
			return @"<p>Demonstrates how to create a soccer tournament scoreboard using the following NOV Diagram features:</p>
<ul>
	<li>Theme-based colors for the title - change the page theme from the ""Design"" tab to see how the style of the title will change, too.</li>
	<li>Table blocks - used for the match shapes.</li>
	<li>Layered Graph Layout - used to arrange the shapes.</li>
</ul>
";
		}

		private void InitDiagram(NDrawingDocument drawingDocument)
		{
			NPage page = drawingDocument.Content.ActivePage;

			// Create a title shape
			NShape titleShape = CreateTitleShape("FIFA World Cup 2022");
			page.Items.Add(titleShape);

			// Create the match shapes
			NShape[] roundOf16Shapes = CreateMatchShapes(page, RoundOf16);
			NShape[] quarterFinalsShapes = CreateMatchShapes(page, QuarterFinals);
			NShape[] semiFinalsShapes = CreateMatchShapes(page, SemiFinals);

			NShape finalShape = CreateMatchShape(Final);
			page.Items.Add(finalShape);

			// Connect the shapes
			for (int i = 0; i < quarterFinalsShapes.Length; i++)
			{
				ConnectShapes(roundOf16Shapes[i * 2], roundOf16Shapes[i * 2 + 1], quarterFinalsShapes[i]);
			}

			ConnectShapes(quarterFinalsShapes[0], quarterFinalsShapes[1], semiFinalsShapes[0]);
			ConnectShapes(quarterFinalsShapes[2], quarterFinalsShapes[3], semiFinalsShapes[1]);

			ConnectShapes(semiFinalsShapes[0], semiFinalsShapes[1], finalShape);

			// Arrange the shapes using a Layered Graph Layout
			NLayeredGraphLayout layout = new NLayeredGraphLayout();
			layout.Direction = ENHVDirection.LeftToRight;
			layout.UseSingleBus = true;
			layout.PlugSpacing.Mode = ENPlugSpacingMode.None;
			layout.RegionLayout.VerticalSpacing = 25; // Controls the spacing between the title and the diagram

			NList<NShape> shapes = page.GetShapes(false, NDiagramFilters.ShapeType2D);
			layout.Arrange(shapes.CastAll<object>(), new NDrawingLayoutContext(page));

			page.SizeToContent();
		}

		#endregion

		#region Implementation

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

		private NShape[] CreateMatchShapes(NPage page, Match[] matches)
		{
			NShape[] matchShapes = new NShape[matches.Length];
			for (int i = 0; i < matches.Length; i++)
			{
				NShape matchShape = CreateMatchShape(matches[i]);
				page.Items.Add(matchShape);
				matchShapes[i] = matchShape;
			}

			return matchShapes;
		}
		private NShape CreateMatchShape(Match match)
		{
			NShape shape = new NShape();
			shape.SetBounds(0, 0, MatchShapeSize.Width, MatchShapeSize.Height);

			// Create a table block
			NTableBlock tableBlock = new NTableBlock(2, 2, NBorder.CreateFilledBorder(MatchBorderColor), MatchBorderThickness);
			tableBlock.PortsDistributionMode = ENPortsDistributionMode.None;
			tableBlock.ResizeMode = ENTableBlockResizeMode.FitToShape;
			shape.TextBlock = tableBlock;

			// Configure the table block content
			NTableBlockContent table = tableBlock.Content;
			table.AllowSpacingBetweenCells = false;
			table.Columns[1].PreferredWidth = NMultiLength.NewFixed(MatchShapeSize.Width / 4);

			// Add the match info to the table
			table.Rows[0].Cells[0].Blocks[0] = new NParagraph(match.Team1);
			table.Rows[0].Cells[1].Blocks[0] = new NParagraph(match.Score1String);
			table.Rows[1].Cells[0].Blocks[0] = new NParagraph(match.Team2);
			table.Rows[1].Cells[1].Blocks[0] = new NParagraph(match.Score2String);

			// Apply center vertical alignment to all cells
			for (int i = 0; i < table.Rows.Count; i++)
			{
				for (int j = 0; j < table.Columns.Count; j++)
				{
					table.Rows[i].Cells[j].VerticalAlignment = ENVAlign.Center;
				}
			}

			// Make the winner's row bold
			table.Rows[match.WinnerTeamIndex - 1].FontStyleBold = true;

			return shape;
		}

		private void ConnectShapes(NShape matchShape1, NShape matchShape2, NShape nextMatchShape)
		{
			NPage page = matchShape1.OwnerPage;

			NRoutableConnector con1 = new NRoutableConnector();
			page.Items.Add(con1);
			con1.GlueBeginToShape(matchShape1);
			con1.GlueEndToShape(nextMatchShape);

			NRoutableConnector con2 = new NRoutableConnector();
			page.Items.Add(con2);
			con2.GlueBeginToShape(matchShape2);
			con2.GlueEndToShape(nextMatchShape);
		}

		#endregion

		#region Fields

		private NDrawingView m_DrawingView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NSoccerTournamentExample.
		/// </summary>
		public static readonly NSchema NSoccerTournamentExampleSchema;

		#endregion

		#region Constants - Data

		private Match[] RoundOf16 = new Match[]
		{
			new Match("Netherlands", 3, "United States", 1),
			new Match("Argentina", 2, "Australia", 1),
			new Match("Japan", 1, 1, "Croatia", 1, 3),
			new Match("Brazil", 4, "South Korea", 1),
			new Match("England", 3, "Senegal", 0),
			new Match("France", 3, "Poland", 1),
			new Match("Morocco", 0, 3, "Spain", 0, 0),
			new Match("Portugal", 6, "Switzerland", 1)
		};

		private Match[] QuarterFinals = new Match[]
		{
			new Match("Netherlands", 2, 3, "Argentina", 2, 4),
			new Match("Croatia", 1, 4, "Brazil", 1, 2),
			new Match("England", 1, "France", 2),
			new Match("Morocco", 1, "Portugal", 0)
		};

		private Match[] SemiFinals = new Match[]
		{
			new Match("Argentina", 3, "Croatia", 0),
			new Match("France", 2, "Morocco", 0)
		};

		private Match ThirdPlace = new Match("Croatia", 2, "Morocco", 1);

		private Match Final = new Match("Argentina", 3, 4, "France", 3, 2);

		#endregion

		#region Constants - Appearance

		private static readonly NSize MatchShapeSize = new NSize(160, 60);
		private static readonly NColor MatchBorderColor = NColor.DimGray;
		private static readonly NMargins MatchBorderThickness = new NMargins(1);

		#endregion

		#region Nested Types

		private class Match
		{
			public Match(string team1, int goals1, string team2, int goals2)
				: this (team1, goals1, -1, team2, goals2, -1)
			{
			}
			public Match(string team1, int goals1, int penalty1, string team2, int goals2, int penalty2)
			{
				Team1 = team1;
				m_Goals1 = goals1;
				m_Penalty1 = penalty1;

				Team2 = team2;
				m_Goals2 = goals2;
				m_Penalty2 = penalty2;
			}

			/// <summary>
			/// Returns the index of the winner team - 1 or 2.
			/// </summary>
			public int WinnerTeamIndex
			{
				get
				{
					if (m_Goals1 > m_Goals2)
						return 1;
					else if (m_Goals1 < m_Goals2)
						return 2;
					else
						return m_Penalty1 > m_Penalty2 ? 1 : 2;
				}
			}
			public string Score1String
			{
				get
				{
					string score = m_Goals1.ToString();
					if (m_Penalty1 != -1)
					{
						score += $" ({m_Penalty1})";
					}

					return score;
				}
			}
			public string Score2String
			{
				get
				{
					string score = m_Goals2.ToString();
					if (m_Penalty2 != -1)
					{
						score += $" ({m_Penalty2})";
					}

					return score;
				}
			}

			public string Team1;
			public string Team2;

			private int m_Goals1;
			private int m_Goals2;
			private int m_Penalty1;
			private int m_Penalty2;
		}

		#endregion
	}
}