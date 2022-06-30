using System;

namespace Nevron.Nov.Examples
{
	internal class NVbSourceCodeParser : NSourceCodeParser
	{
		#region Properties - Overrides

		public override string UsingKeyword
		{
			get
			{
				return "Imports";
			}
		}
		public override string OverrideKeyword
		{
			get
			{
				return "Overrides";
			}
		}
		public override string StatementTerminator
		{
			get
			{
				return String.Empty;
			}
		}

		public override string NamespaceKeyword
		{
			get
			{
				return "Namespace";
			}
		}
		public override string RegionBeginKeyword
		{
			get
			{
				return "#Region";
			}
		}
		public override string RegionEndKeyword
		{
			get
			{
				return "#End Region";
			}
		}
		public override string RegionNamePrefix
		{
			get
			{
				return "\"";
			}
		}
		public override string RegionNameSuffix
		{
			get
			{
				return "\"";
			}
		}

		#endregion

		#region Internal Overrides

		/// <summary>
		/// Gets the namespace or class start index in the given source code string.
		/// </summary>
		/// <param name="sourceCode"></param>
		/// <returns></returns>
		internal override int GetClassStartIndex(string sourceCode)
		{
			int index = base.GetClassStartIndex(sourceCode);
			if (index == -1)
			{
				for (int i = 0; i < ClassStartKeywords.Length; i++)
				{
					index = sourceCode.IndexOf(ClassStartKeywords[i]);
					if (index != -1)
						break;
				}
			}

			return index;
		}

		#endregion

		#region Constants

		private static readonly string[] ClassStartKeywords = new string[]
		{
			"Public Class",
			"Friend Class",
			"Class"
		};

		#endregion
	}
}