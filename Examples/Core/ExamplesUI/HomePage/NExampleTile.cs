using System;

using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples
{
	public class NExampleTile : NPairBox
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NExampleTile()
		{
			m_Status = defaultStatus;
		}
		/// <summary>
		/// Initializing constructor.
		/// </summary>
		/// <param name="icon"></param>
		/// <param name="title"></param>
		public NExampleTile(NImage icon, string title)
			: base(icon, title)
		{
			m_Status = defaultStatus;
			Box2.VerticalPlacement = ENVerticalPlacement.Center;
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NExampleTile()
		{
			NExampleTileSchema = NSchema.Create(typeof(NExampleTile), NPairBox.NPairBoxSchema);

			// Properties
			StatusProperty = NExampleTileSchema.AddMember("Status", NDomType.String, defaultStatus,
				delegate(NNode o) { return ((NExampleTile)o).m_Status; },
				delegate(NNode o, object v) { ((NExampleTile)o).m_Status = (string)v; });

			// Modify some default values
			SpacingProperty.SetDefaultValue(NExampleTileSchema, 5.0);
		}

		#endregion

		#region Properties - Public

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

		#region Properties - Internal

		private NHomePage HomePage
		{
			get
			{
				NHomePage homePage = (NHomePage)GetFirstAncestor(NHomePage.NHomePageSchema);
				if (homePage != null)
					return homePage;

				NExamplesContent examplesContent = (NExamplesContent)GetFirstAncestor(NExamplesContent.NExamplesContentSchema);
				return examplesContent.m_HomePage;
			}
		}

		#endregion

		#region Protected Overrides - Paint

		/// <summary>
		/// Performs the element post-children custom paint. Overriden to paint the status
		/// of this example tile (if it has one) in its bottom-left corner.
		/// </summary>
		/// <param name="visitor"></param>
		protected override void OnPostPaint(NPaintVisitor visitor)
		{
			base.OnPostPaint(visitor);

			if (String.IsNullOrEmpty(m_Status))
				return;

			// Paint a new label in the bottom left corner
			NRectangle bounds = GetContentEdge();

			NFont font = new NFont(FontName, 5.0, ENFontStyle.Regular);
			font.RasterizationMode = ENFontRasterizationMode.Aliased;
			NSize textSize = font.MeasureString(m_Status, this);
			NRectangle textRect = new NRectangle(bounds.X - 1, bounds.Bottom - textSize.Height,
				textSize.Width + 3, textSize.Height);

			// Paint the text background
			NColor color = HomePage.GetStatusColor(m_Status);
			visitor.SetFill(color);
			visitor.PaintRectangle(textRect);

			// Paint the text
			visitor.SetFill(NColor.White);
			visitor.SetFont(font);

			NPoint location = textRect.Location;
			NPaintTextPointSettings settings = new NPaintTextPointSettings();
			visitor.PaintString(location, m_Status, ref settings);
		}

		#endregion

		#region Fields

		private string m_Status;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NExampleTile.
		/// </summary>
		public static readonly NSchema NExampleTileSchema;
		/// <summary>
		/// Reference to the Status property.
		/// </summary>
		public static readonly NProperty StatusProperty;

		#endregion

		#region Default Values

		private const string defaultStatus = null;

		#endregion

		#region Constants

		internal const string FontName = "Arial";

		#endregion
	}
}