using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NContentHolderExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NContentHolderExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NContentHolderExample()
		{
			NContentHolderExampleSchema = NSchema.Create(typeof(NContentHolderExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NLabel label = new NLabel("Content Holder");
			label.HorizontalPlacement = ENHorizontalPlacement.Center;
			label.VerticalPlacement = ENVerticalPlacement.Center;

			m_ContentHolder = new NContentHolder();
			m_ContentHolder.Content = label;
			m_ContentHolder.SetBorder(1, NColor.Red);

			return m_ContentHolder;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();

			NList<NPropertyEditor> propertyEditors = NDesigner.GetDesigner(m_ContentHolder).CreatePropertyEditors(
				m_ContentHolder,
				NWidget.HorizontalPlacementProperty,
				NWidget.VerticalPlacementProperty
			);

			for (int i = 0; i < propertyEditors.Count; i++)
			{
				stack.Add(propertyEditors[i]);
			}

			NButton toggleLoaderButton = new NButton("Show Loader");
			toggleLoaderButton.Click += ToggleLoaderButton_Click;
			stack.Add(toggleLoaderButton);

			return new NUniSizeBoxGroup(stack);
		}
		protected override string GetExampleDescription()
		{
			return "<p>The content holder is a widget that can host another widget and provides support for showing a loader.</p>";
		}

		#endregion

		#region Implementation

		private void ToggleLoaderButton_Click(NEventArgs arg)
		{
			NButton button = (NButton)arg.CurrentTargetNode;
			NLabel label = (NLabel)button.Content;

			if (m_ContentHolder.Enabled)
			{
				m_ContentHolder.ShowLoader();
				label.Text = "Hide Loader";
			}
			else
			{
				m_ContentHolder.HideLoader();
				label.Text = "Show Loader";
			}
		}

		#endregion

		#region Fields

		private NContentHolder m_ContentHolder;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NContentHolderExample.
		/// </summary>
		public static readonly NSchema NContentHolderExampleSchema;

		#endregion
	}
}