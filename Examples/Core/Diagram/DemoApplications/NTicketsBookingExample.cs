using System;

using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Themes;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Text;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NTicketsBookingExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NTicketsBookingExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NTicketsBookingExample()
		{
			NTicketsBookingExampleSchema = NSchema.Create(typeof(NTicketsBookingExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_DrawingView = new NDrawingView();

            // hide grid and ports
            m_DrawingView.Content.ScreenVisibility.ShowGrid = false;
            m_DrawingView.Content.ScreenVisibility.ShowPorts = false;

			// no history for this example
			m_DrawingView.Document.HistoryService.Stop();

			// create styling
			NStyleSheet styleSheet = CreateStyles();
			m_DrawingView.Document.StyleSheets.Add(styleSheet);

			NPage page = m_DrawingView.ActivePage;

			m_FreeSeats = 0;
			m_BookedSeats = 0;

			page.Items.Add(CreateTitleShape("Tickets Booking"));
			page.Items.Add(CreateAirplaneShape());

			NGroup infoShape = CreateInfoShape();
			page.Items.Add(infoShape);
			UpdateTexts();
			infoShape.UpdateBounds();

			page.SizeToContent();

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
	This example demonstrates how to create a tickets booking application using NOV Diagram for .NET. When the user clicks on a shape (seat)
    its user class is changed in order to mark it as booked. The example also demonstrates the usage of protections, which helps us
    prevent the user from moving, resizing and deleting of shapes thus leaving him the only possible interaction he should be allowed to do -
    to click on shapes.
</p>";
		}

		#endregion

		#region Implementation

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private NStyleSheet CreateStyles()
		{
			NStyleSheet styleSheet = new NStyleSheet();

			// Create the free seats style
			{
				// Shape appearance
				NRule rule = styleSheet.CreateRule(
					sb =>
					{
						sb.Type(NGeometry.NGeometrySchema);
						sb.ChildOf();
						sb.Type(NShape.NShapeSchema);
						sb.UserClass(UserClassFree);
					}
				);

				rule.AddValueDeclaration(NGeometry.FillProperty, new NColorFill(NColor.LemonChiffon));

				// Shape cursor
				rule = styleSheet.CreateRule(
					sb =>
					{
						sb.Type(NShape.NShapeSchema);
						sb.UserClass(UserClassFree);
					}
				);

				rule.AddValueDeclaration(NShape.CursorProperty, new NCursor(ENPredefinedCursor.Hand));
			}

			// Create the booked seats style
			{
				NRule rule = styleSheet.CreateRule(
					sb =>
					{
						sb.Type(NGeometry.NGeometrySchema);
						sb.ChildOf();
						sb.Type(NShape.NShapeSchema);
						sb.UserClass(UserClassBooked);
					}
				);

				rule.AddValueDeclaration(NGeometry.FillProperty, new NHatchFill(ENHatchStyle.LightUpwardDiagonal, NColor.DarkRed, NColor.LemonChiffon));
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
		/// Creates the Airplane shape.
		/// </summary>
		/// <returns></returns>
		private NGroup CreateAirplaneShape()
		{
			// Constants
			const double SeatsDistance = 5.1;
			NPoint Line1Left = new NPoint(300, 182);
			NPoint Line1Right = new NPoint(588, 182);
			NPoint Line2Left = new NPoint(292, 257);

			// Get the airplane seats image from the resources
			NImage image = NResources.Image_Artistic_AirplaneSeats_png;

			// Create a group
			NGroup group = new NGroup();
			group.SetProtectionMask(ENDiagramItemOperationMask.All);
			group.SelectionMode = ENGroupSelectionMode.GroupOnly;
			group.Geometry.AddRelative(new NDrawRectangle(0, 0, 1, 1));
			group.Geometry.Fill = new NImageFill(image);
			group.SetBounds(0, 100, image.Width, image.Height);

			// Create the seat shapes
			double distance = SeatsDistance;
			NPoint startPoint = Line1Left;

			for (double y = Line1Left.Y; y < 309; y += SeatHeight)
			{
				if (y > 223 && y < Line2Left.Y)
				{
					y = Line2Left.Y;
					startPoint = Line2Left;
				}

				if (y >= Line2Left.Y)
					distance = 6.1;

				for (double x = startPoint.X; x < 970; x += SeatWidth + distance)
				{
					if (x > 460 && x < Line1Right.X)
					{
						x = Line1Right.X;
						distance = SeatsDistance;
					}

					m_FreeSeats++;

					NShape seatShape = CreateSeatShape(x, y);
					seatShape.UserClass = UserClassFree;
					seatShape.MouseDown += OnSeatShapeMouseDown;
					group.Shapes.Add(seatShape);
				}
			}

			return group;
		}
		private NShape CreateSeatShape(double x, double y)
		{
			NShape seatShape = new NShape();
			seatShape.Geometry.AddRelative(new NDrawRectangle(0, 0, 1, 1));
			seatShape.SetBounds(x, y, SeatWidth, SeatHeight);

			return seatShape;
		}
		/// <summary>
		/// Creates the Info shape.
		/// </summary>
		/// <returns></returns>
		private NGroup CreateInfoShape()
		{
			NGroup infoGroup = new NGroup();
			infoGroup.SetProtectionMask(ENDiagramItemOperationMask.All);
			infoGroup.SelectionMode = ENGroupSelectionMode.GroupOnly;

			NShape freeSeatShape = CreateSeatShape(2, 2);
			freeSeatShape.UserClass = UserClassFree;
			infoGroup.Shapes.Add(freeSeatShape);

			NShape bookedSeatShape = CreateSeatShape(2, 25);
			bookedSeatShape.UserClass = UserClassBooked;
			infoGroup.Shapes.Add(bookedSeatShape);

			m_FreeSeatsShape = new NShape();
			m_FreeSeatsShape.SetBounds(30, 2, 150, 20);
			m_FreeSeatsShape.TextBlock.HorizontalAlignment = ENAlign.Left;
			infoGroup.Shapes.Add(m_FreeSeatsShape);

			m_BookedSeatsShape = new NShape();
			m_BookedSeatsShape.SetBounds(30, 25, 150, 20);
			m_BookedSeatsShape.TextBlock.HorizontalAlignment = ENAlign.Left;
			infoGroup.Shapes.Add(m_BookedSeatsShape);

			m_RevenueShape = new NShape();
			m_RevenueShape.SetBounds(30, 48, 200, 20);
			m_RevenueShape.TextBlock.HorizontalAlignment = ENAlign.Left;
			m_RevenueShape.TextBlock.FontStyleBold = true;
			m_RevenueShape.TextBlock.Fill = new NColorFill(NColor.MediumBlue);
			infoGroup.Shapes.Add(m_RevenueShape);

			return infoGroup;
		}
		/// <summary>
		/// 
		/// </summary>
		private void UpdateTexts()
		{
			m_FreeSeatsShape.Text = String.Format(FormatStringFree, m_FreeSeats);
			m_BookedSeatsShape.Text = String.Format(FormatStringBooked, m_BookedSeats);
			m_RevenueShape.Text = String.Format(FormatStringRevenue, m_BookedSeats * SeatPrice);
		}

		#endregion

		#region Event Handlers

		private void OnSeatShapeMouseDown(NMouseButtonEventArgs arg)
		{
			if (arg.Cancel || arg.Button != ENMouseButtons.Left)
				return;

			NShape shape = (NShape)arg.CurrentTargetNode;
			if (shape.UserClass == UserClassFree)
			{
				m_FreeSeats--;
				m_BookedSeats++;
				shape.UserClass = UserClassBooked;
			}
			else
			{
				m_FreeSeats++;
				m_BookedSeats--;
				shape.UserClass = UserClassFree;
			}

			UpdateTexts();
		}

		#endregion

		#region Fields

		private NDrawingView m_DrawingView;
		private int m_FreeSeats;
		private int m_BookedSeats;

		private NShape m_FreeSeatsShape;
		private NShape m_BookedSeatsShape;
		private NShape m_RevenueShape;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NTicketsBookingExample.
		/// </summary>
		public static readonly NSchema NTicketsBookingExampleSchema;

		#endregion

		#region Constants

		private const double SeatWidth = 20;
		private const double SeatHeight = 19;
		private const double SeatPrice = 50;

		private const string UserClassFree = "FreeSeat";
		private const string UserClassBooked = "BookedSeat";

		private const string FormatStringFree = "Free seats: {0}";
		private const string FormatStringBooked = "Booked seats: {0}";
		private const string FormatStringRevenue = "Total revenue: {0:C0}";

		#endregion
	}
}