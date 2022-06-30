using System;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;
using Nevron.Nov.Wmf;

namespace Nevron.Nov.Examples
{
	/// <summary>
	/// Represents the image box for a NOV component.
	/// </summary>
	public class NComponentImageBox : NImageBox
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NComponentImageBox()
		{
			m_Status = defaultStatus;
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NComponentImageBox()
		{
            NComponentImageBoxSchema = NSchema.Create(typeof(NComponentImageBox), NImageBox.NImageBoxSchema);

			// Properties
			StatusProperty = NComponentImageBoxSchema.AddMember("Status", NDomType.String, defaultStatus,
				delegate(NNode o) { return ((NComponentImageBox)o).m_Status; },
				delegate(NNode o, object v) { ((NComponentImageBox)o).m_Status = (string)v; });
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets/Sets the status of the component this image box is representing.
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

		#region Protected Overrides - Paint

		protected override void OnPostPaint(NPaintVisitor visitor)
		{
			base.OnPostPaint(visitor);

			if (String.IsNullOrEmpty(m_Status))
				return;

			visitor.ClearStyles();
			visitor.SetFont(Font);

			// Determine status bounds
			NRectangle contentArea = GetContentEdge();
			double length = NMath.Max(contentArea.Width, contentArea.Height) * 0.3;
			NRectangle statusBounds = new NRectangle(contentArea.Right - length - 5,
				contentArea.Top + 5, length, length);

			// Fill the status bounds with a circle
			NHomePage homePage = (NHomePage)GetFirstAncestor(NHomePage.NHomePageSchema);
			NColor color = homePage.GetStatusColor(m_Status);
			visitor.SetFill(new NColor(color, 160));
			visitor.PaintEllipse(statusBounds);

			// Create text paint settings
			NPaintTextRectSettings settings = new NPaintTextRectSettings();
			settings.HorzAlign = ENTextHorzAlign.Center;
			settings.VertAlign = ENTextVertAlign.Center;

			// Paint the status text in the top right corner
			visitor.SetFill(NColor.White);
			visitor.PaintString(statusBounds, m_Status, ref settings);
		}

		#endregion

		#region Protected Overrides - Mouse Events

		protected override void OnMouseDown(NMouseButtonEventArgs args)
		{
			base.OnMouseDown(args);

			Border = NBorder.CreateFilledBorder(new NColor(32, 32, 32));
		}
		protected override void OnMouseUp(NMouseButtonEventArgs args)
		{
			base.OnMouseUp(args);

			ClearLocalValue(BorderProperty);
		}
		protected override void OnMouseOut(NMouseOverChangeEventArgs args)
		{
			base.OnMouseOut(args);

			ClearLocalValue(BorderProperty);
		}

		#endregion

		#region Fields

		private string m_Status;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NComponentImageBox.
		/// </summary>
		public static readonly NSchema NComponentImageBoxSchema;
		/// <summary>
		/// Reference to the Status property.
		/// </summary>
		public static readonly NProperty StatusProperty;

		#endregion

		#region Default Values

		private const string defaultStatus = null;

		#endregion
	}
}