using Nevron.Nov.DataStructures;
using Nevron.Nov.Editors;
using Nevron.Nov.Dom;

namespace Nevron.Nov.Examples.Framework
{
	public abstract class NStyleNodeCollectionBase : NNodeCollection<NStyleNode>
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NStyleNodeCollectionBase()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NStyleNodeCollectionBase()
		{
			NStyleNodeCollectionBaseSchema = NSchema.Create(typeof(NStyleNodeCollectionBase), NNodeCollection<NStyleNode>.NNodeCollectionSchema);
		}

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NStyleNodeCollectionBase.
		/// </summary>
		public static readonly NSchema NStyleNodeCollectionBaseSchema;

		#endregion
	}

	public class NStyleNodeCollectionList : NStyleNodeCollectionBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NStyleNodeCollectionList()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NStyleNodeCollectionList()
		{
			NStyleNodeCollectionListSchema = NSchema.Create(typeof(NStyleNodeCollectionList), NStyleNodeCollectionBase.NStyleNodeCollectionBaseSchema);

			// Designer
			NStyleNodeCollectionListSchema.SetMetaUnit(new NDesignerMetaUnit(typeof(NStyleNodeCollectionListDesigner)));
		}

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NStyleNodeCollectionList.
		/// </summary>
		public static readonly NSchema NStyleNodeCollectionListSchema;

		#endregion

		#region Designer

		/// <summary>
		/// Designer for NStyleNodeCollectionList.
		/// </summary>
		public class NStyleNodeCollectionListDesigner : NDesigner
		{
            public NStyleNodeCollectionListDesigner()
            {
				HierarchyEmbeddableEditor = NChildrenHierarchyEditor.ListBoxTemplate;
            }
		}

		#endregion
	}

	public class NStyleNodeCollectionTree : NStyleNodeCollectionBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NStyleNodeCollectionTree()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NStyleNodeCollectionTree()
		{
			NStyleNodeCollectionTreeSchema = NSchema.Create(typeof(NStyleNodeCollectionTree), NStyleNodeCollectionBase.NStyleNodeCollectionBaseSchema);

			// Designer
			NStyleNodeCollectionTreeSchema.SetMetaUnit(new NDesignerMetaUnit(typeof(NStyleNodeCollectionTreeDesigner)));
		}

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NStyleNodeCollectionTree.
		/// </summary>
		public static readonly NSchema NStyleNodeCollectionTreeSchema;

		#endregion

		#region Designer

		/// <summary>
		/// Designer for NStyleNodeCollectionTree.
		/// </summary>
		public class NStyleNodeCollectionTreeDesigner : NDesigner
		{
		}

		#endregion
	}
}