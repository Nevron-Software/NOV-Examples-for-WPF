using System;

namespace Nevron.Nov.Examples
{
	internal class NCsSourceCodeParser : NSourceCodeParser
	{
		#region Properties - Overrides

		public override string UsingKeyword
		{
			get
			{
				return "using";
			}
		}
		public override string OverrideKeyword
		{
			get
			{
				return "override";
			}
		}
		public override string StatementTerminator
		{
			get
			{
				return ";";
			}
		}

		public override string NamespaceKeyword
		{
			get
			{
				return "namespace";
			}
		}
		public override string RegionBeginKeyword
		{
			get
			{
				return "#region";
			}
		}
		public override string RegionEndKeyword
		{
			get
			{
				return "#endregion";
			}
		}
		public override string RegionNamePrefix
		{
			get
			{
				return String.Empty;
			}
		}
		public override string RegionNameSuffix
		{
			get
			{
				return String.Empty;
			}
		}

		#endregion
	}
}