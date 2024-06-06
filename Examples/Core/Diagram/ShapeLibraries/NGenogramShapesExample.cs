using Nevron.Nov.Diagram;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.IO;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NGenogramShapesExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NGenogramShapesExample()
		{

		}

		/// <summary>
		/// Static constructor
		/// </summary>
		static NGenogramShapesExample()
		{
			NGenogramShapesExampleSchema = NSchema.Create(typeof(NGenogramShapesExample), NExampleBaseSchema);
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
			return @"<p>This example shows the genogram shapes located in the ""Family Tree\Genogram Shapes.nlb"" shape library.</p>";
		}

		private void InitDiagram(NDrawingDocument drawingDocument)
		{
			NDrawing drawing = drawingDocument.Content;
			NPage activePage = drawing.ActivePage;

			// Hide grid and ports
			drawing.ScreenVisibility.ShowGrid = false;
			drawing.ScreenVisibility.ShowPorts = false;

			// Load the library and create all shapes from it
			NFile libraryFile = NApplication.ResourcesFolder.GetFile(NPath.Current.Combine(
					"ShapeLibraries", "Family Tree", "Genogram Shapes.nlb"));
			NLibraryDocument.FromFileAsync(libraryFile).Then(
				libraryDocument =>
				{
					NLibrary library = libraryDocument.Content;
					int row = 0, col = 0;
					double cellWidth = 240;
					double cellHeight = 150;

					for (int i = 0; i < library.Items.Count; i++, col++)
					{
						NShape shape = library.CreateShape(i);
						shape.HorizontalPlacement = ENHorizontalPlacement.Center;
						shape.VerticalPlacement = ENVerticalPlacement.Center;

						NTextBlock textBlock = shape.GetFirstDescendant<NTextBlock>();

						if (textBlock == null ||
							i == (int)ENGenogramShape.Male ||
							i == (int)ENGenogramShape.Female ||
							i == (int)ENGenogramShape.Pet ||
							i == (int)ENGenogramShape.UnknownGender)
						{
							textBlock = (NTextBlock)shape.TextBlock;
						}

						textBlock.Text = shape.Name;

						activePage.Items.Add(shape);

						if (col >= 4)
						{
							row++;
							col = 0;
						}

						NPoint beginPoint = new NPoint(50 + col * cellWidth, 50 + row * cellHeight);
						if (shape.ShapeType == ENShapeType.Shape1D)
						{
							NPoint endPoint = beginPoint + new NPoint(cellWidth - 50, cellHeight - 50);

							shape.SetBeginPoint(beginPoint);
							shape.SetEndPoint(endPoint);
						}
						else
						{
							textBlock.SetFx(NTextBlock.PinYProperty, "$Parent.Height + Height + 10");
							textBlock.ResizeMode = ENTextBlockResizeMode.TextSize;
							shape.SetBounds(beginPoint.X, beginPoint.Y, shape.Width, shape.Height);
						}
					}

					// size page to content
					activePage.Layout.ContentPadding = new NMargins(50);
					activePage.SizeToContent();
				}
			);
		}

		#endregion

		#region Fields

		private NDrawingView m_DrawingView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NGenogramShapesExample.
		/// </summary>
		public static readonly NSchema NGenogramShapesExampleSchema;

		#endregion

		#region Nested Types

		/// <summary>
		/// Enumerates the genogram shapes.
		/// </summary>
		public enum ENGenogramShape
		{
			/// <summary>
			/// Male.
			/// </summary>
			Male,
			/// <summary>
			/// Female.
			/// </summary>
			Female,
			/// <summary>
			/// Unknown.
			/// </summary>
			UnknownGender,
			/// <summary>
			/// Pet.
			/// </summary>
			Pet,
			/// <summary>
			/// Pregnancy
			/// </summary>
			Pregnancy,
			/// <summary>
			/// Marriage
			/// </summary>
			Marriage,
			/// <summary>
			/// Engagement
			/// </summary>
			Engagement,
			/// <summary>
			/// Engagement and separation
			/// </summary>
			EngagementAndSeparation,
			/// <summary>
			/// Engagement and cohabitation
			/// </summary>
			EngagementAndCohabitation,
			/// <summary>
			/// Commited
			/// </summary>
			Commited,
			/// <summary>
			/// Casual relationship
			/// </summary>
			CasualRelationship,
			/// <summary>
			/// Casual relationship and separation
			/// </summary>
			CasualRelationshipAndSeparation,
			/// <summary>
			/// Temporary relationship
			/// </summary>
			TemporaryRelationship,
			/// <summary>
			/// Love affair
			/// </summary>
			LoveAffair,
			/// <summary>
			/// Separation in fact.
			/// </summary>
			SeparationInFact,
			/// <summary>
			/// Legal separation
			/// </summary>
			LegalSeparation,
			/// <summary>
			/// Legal cohabitaion
			/// </summary>
			LegalCohabitaion,
			/// <summary>
			/// Legal cohabitaion and separation
			/// </summary>
			LegalCohabitaionAndSeparation,
			/// <summary>
			/// Cohabitaion
			/// </summary>
			Cohabitaion,
			/// <summary>
			/// Non-sentimental cohabitaion
			/// </summary>
			NonSentimentalCohabitaion,
			/// <summary>
			/// Non-sentimental cohabitaion and separation
			/// </summary>
			NonSentimentalCohabitaionAndSeparation,
			/// <summary>
			/// Cohabitaion and separation
			/// </summary>
			CohabitaionAndSeparation,
			/// <summary>
			/// Cohabitaion and legal separation
			/// </summary>
			CohabitaionAndLegalSeparation,
			/// <summary>
			/// Divorce
			/// </summary>
			Divorce,
			/// <summary>
			/// Nullity
			/// </summary>
			Nullity,
			/// <summary>
			/// Indifferent
			/// </summary>
			Indifferent,
			/// <summary>
			/// Distant
			/// </summary>
			Distant,
			/// <summary>
			/// Harmony
			/// </summary>
			Harmony,
			/// <summary>
			/// Friendship
			/// </summary>
			Friendship,
			/// <summary>
			/// Discord
			/// </summary>
			Discord,
			/// <summary>
			/// Hate
			/// </summary>
			Hate,
			/// <summary>
			/// Fused
			/// </summary>
			Fused,
			/// <summary>
			/// Cutoff
			/// </summary>
			Cutoff,
			/// <summary>
			/// Love
			/// </summary>
			Love,
			/// <summary>
			/// In Love
			/// </summary>
			InLove,
			/// <summary>
			/// Focused On
			/// </summary>
			FocusedOn,
			/// <summary>
			/// Fan
			/// </summary>
			Fan,
			/// <summary>
			/// Limerence
			/// </summary>
			Limerence,
			/// <summary>
			/// Neglect
			/// </summary>
			Neglect,
			/// <summary>
			/// Manipulative
			/// </summary>
			Manipulative,
			/// <summary>
			/// Controlling
			/// </summary>
			Controlling,
			/// <summary>
			/// Hostile
			/// </summary>
			Hostile,
			/// <summary>
			/// Distant - Hostile
			/// </summary>
			DistantHostile,
			/// <summary>
			/// Violence
			/// </summary>
			Violence,
			/// <summary>
			/// Distant - Violence
			/// </summary>
			DistantViolence,
			/// <summary>
			/// Abuse
			/// </summary>
			Abuse,
			/// <summary>
			/// Phisical abuse
			/// </summary>
			PhisicalAbuse,
			/// <summary>
			/// Emotional abuse
			/// </summary>
			EmotionalAbuse,
			/// <summary>
			/// Close - Hosile
			/// </summary>
			CloseHostile,
			/// <summary>
			/// Fused - Hostile
			/// </summary>
			FusedHostile,
			/// <summary>
			/// Close - Violence
			/// </summary>
			CloseViolence,
			/// <summary>
			/// Fused - Violence
			/// </summary>
			FusedViolence,
			/// <summary>
			/// Sexual Abuse
			/// </summary>
			SexualAbuse,
			/// <summary>
			/// BestFriends
			/// </summary>
			BestFriends,
			/// <summary>
			/// Distrust
			/// </summary>
			Distrust
		}

		#endregion
	}
}