using System;

using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Framework
{
	public class NNodeDesignersExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NNodeDesignersExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NNodeDesignersExample()
		{
			NNodeDesignersExampleSchema = NSchema.Create(typeof(NNodeDesignersExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NStackPanel stack = new NStackPanel();
			stack.HorizontalPlacement = ENHorizontalPlacement.Left;
			
			// Reset the style node counter
			NStyleNode.Counter = 1;

			// Create the show designer buttons
			stack.Add(CreateShowDesignerButton(new NSimpleNode()));
			stack.Add(CreateShowDesignerButton(new NStyleNode()));
			stack.Add(CreateShowDesignerButton(CreateStyleNodesTree()));
			stack.Add(CreateShowDesignerButton(CreateStyleNodesList()));

			return stack;
		}
		protected override NWidget CreateExampleControls()
		{
			return null;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to show the designer for a given Nevron DOM node.
</p>
";
		}

		#endregion

		#region Implementation

		private NButton CreateShowDesignerButton(NNode node)
		{
			NButton button = new NButton(NDesigner.GetDesigner(node).ToString());
			button.Tag = node;
			button.Click += OnShowDesignerClick;
			return button;
		}
		private NStyleNodeCollectionList CreateStyleNodesList()
		{
			NStyleNodeCollectionList collection = new NStyleNodeCollectionList();
			collection.Add(CreateStyleNode(NColor.Red));
			collection.Add(CreateStyleNode(NColor.Green));
			collection.Add(CreateStyleNode(NColor.Blue));
			return collection;
		}
		private NStyleNodeCollectionTree CreateStyleNodesTree()
		{
			NStyleNodeCollectionTree collection = new NStyleNodeCollectionTree();
			collection.Add(CreateStyleNode(NColor.Red));
			collection.Add(CreateStyleNode(NColor.Green));
			collection.Add(CreateStyleNode(NColor.Blue));
			return collection;
		}
		private NStyleNode CreateStyleNode(NColor color)
		{
			NStyleNode styleNode = new NStyleNode();
			styleNode.ColorFill = new NColorFill(color);
			return styleNode;
		}

		#endregion

		#region Event Handlers

		private void OnShowDesignerClick(NEventArgs args)
		{
			try
			{
				NButton button = (NButton)args.TargetNode;
				NNode node = (NNode)button.Tag;
				NEditorWindow editorWindow = NEditorWindow.CreateForInstance(
                    node, 
                    null,
                    button.DisplayWindow, 
                    null);

				if (node is NStyleNodeCollectionTree)
				{
					editorWindow.PreferredSize = new NSize(500, 360);
				}

				editorWindow.Open();
			}
			catch (Exception ex)
			{
				NTrace.WriteException("OnShowDesignerClick failed.", ex);
			}
		}

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NNodeDesignersExample.
		/// </summary>
		public static readonly NSchema NNodeDesignersExampleSchema;

		#endregion
	}
}