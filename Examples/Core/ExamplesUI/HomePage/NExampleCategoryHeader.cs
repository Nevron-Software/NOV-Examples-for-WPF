using System;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;
using Nevron.Nov.Xml;

namespace Nevron.Nov.Examples
{
	/// <summary>
	/// Represents the header of an example category.
	/// </summary>
	public class NExampleCategoryHeader : NPairBox
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NExampleCategoryHeader()
		{
		}
		/// <summary>
		/// Initializing constructor.
		/// </summary>
		/// <param name="content"></param>
		public NExampleCategoryHeader(string text)
		{
			Box1 = CreateHomeButton();

			NLabel label = new NLabel(text);
			label.Cursor = new NCursor(ENPredefinedCursor.Hand);
			label.HorizontalPlacement = ENHorizontalPlacement.Center;
			label.VerticalPlacement = ENVerticalPlacement.Center;
			label.MouseUp += OnLabelMouseUp;
			Box2 = label;
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NExampleCategoryHeader()
		{
			NExampleCategoryHeaderSchema = NSchema.Create(typeof(NExampleCategoryHeader), NPairBox.NPairBoxSchema);

			// Properties
			StatusProperty = NExampleCategoryHeaderSchema.AddMember("Status", NDomType.String, defaultStatus,
				delegate(NNode o) { return ((NExampleCategoryHeader)o).m_Status; },
				delegate(NNode o, object v) { ((NExampleCategoryHeader)o).m_Status = (string)v; });

			// Constants
			StatusFont = new NFont(NExampleTile.FontName, 5.0, ENFontStyle.Regular);
			StatusFont.RasterizationMode = ENFontRasterizationMode.Aliased;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets/Sets the status of this example tile, e.g. "NEW", "BETA", "UPD", etc.
		/// </summary>
		public string Status
		{
			get
			{
				return m_Status;
			}
			set
			{
				SetValue(StatusProperty, value);
			}
		}

		#endregion

		#region Protected Overrides - Measure

		/// <summary>
		/// Gets the padding of the element.
		/// </summary>
		/// <returns></returns>
		public override NMargins GetPadding()
		{
			NMargins padding = base.GetPadding();

			string status = Status;
			if (String.IsNullOrEmpty(status) == false)
			{
				NSize statusTextSize = StatusFont.MeasureString(Status, this);
				padding.Right += statusTextSize.Width + StatusLeftPadding + StatusRightPadding;
			}

			return padding;
		}

		#endregion

		#region Protected Overrides - Paint

		/// <summary>
		/// Performs the element post-children custom paint. Overriden to paint the status
		/// of this category header's group (if it has one) in the top-right corner of the header.
		/// </summary>
		/// <param name="visitor"></param>
		protected override void OnPostPaint(NPaintVisitor visitor)
		{
			base.OnPostPaint(visitor);

			if (String.IsNullOrEmpty(m_Status))
				return;

			// Determine the text bounds
			NRectangle bounds = GetContentEdge();
			NSize textSize = Font.MeasureString(((NLabel)Box2).Text, this);
			NRectangle textBounds = NRectangle.FromCenterAndSize(bounds.Center, textSize.Width, textSize.Height);
			textBounds.X += Box1.Width / 2;

			// Calculate a rectangle for the status text located to the right of the text rectangle
			textSize = StatusFont.MeasureString(m_Status, this);
			NRectangle textRect = new NRectangle(
				textBounds.Right + StatusLeftPadding, 
				textBounds.Top,
				textSize.Width + StatusRightPadding, 
				textSize.Height);

			// Paint the text background
			NHomePage homePage = (NHomePage)GetFirstAncestor(NHomePage.NHomePageSchema);
			NColor color = homePage.GetStatusColor(m_Status);
			visitor.SetFill(color);
			visitor.PaintRectangle(textRect);

			// Paint the text
			visitor.SetFill(NColor.White);
			visitor.SetFont(StatusFont);

			NPoint location = textRect.Location;
			NPaintTextPointSettings settings = new NPaintTextPointSettings();
			visitor.PaintString(location, m_Status, ref settings);
		}

		#endregion

		#region Implementation

		private NButton CreateHomeButton()
		{
			NButton button = new NButton(NResources.Image_ExamplesUI_Icons_Home_png);
			button.Cursor = new NCursor(ENPredefinedCursor.Hand);
			button.FocusDecorator = null;
			button.Click += OnButtonClick;

			return button;
		}

		#endregion

		#region Event Handlers

		private void OnButtonClick(NEventArgs arg)
		{
			if (arg.Cancel || arg.EventPhase != ENEventPhase.AtTarget)
				return;

			NSingleVisiblePanel panel = (NSingleVisiblePanel)GetFirstAncestor(NSingleVisiblePanel.NSingleVisiblePanelSchema);
			panel.VisibleIndex = 0;
		}
		private void OnLabelMouseUp(NMouseButtonEventArgs arg)
		{
			if (arg.Cancel || arg.EventPhase != ENEventPhase.AtTarget)
				return;

			NExamplesContent examplesContent = (NExamplesContent)GetFirstAncestor(NExamplesContent.NExamplesContentSchema);
			examplesContent.NavigateToExample((NXmlElement)ParentNode.Tag);
		}

		#endregion

		#region Fields

		private string m_Status;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NExampleCategoryHeader.
		/// </summary>
		public static readonly NSchema NExampleCategoryHeaderSchema;
		/// <summary>
		/// Reference to the Status property.
		/// </summary>
		public static readonly NProperty StatusProperty;

		#endregion

		#region Default Values

		private const string defaultStatus = null;

		#endregion

		#region Constants

		private static readonly NFont StatusFont;
		private const double StatusLeftPadding = 3;
		private const double StatusRightPadding = 4;

		#endregion
	}
}