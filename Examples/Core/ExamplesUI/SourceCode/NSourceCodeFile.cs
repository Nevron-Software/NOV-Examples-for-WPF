using System;

using Nevron.Nov.DataStructures;
using Nevron.Nov.Graphics;

namespace Nevron.Nov.Examples
{
	/// <summary>
	/// Represents a source code file.
	/// </summary>
	internal class NSourceCodeFile
	{
		#region Constructors

		public NSourceCodeFile(string sourceCode, NSourceCodeParser parser, NList<string> usings, NMap<string, string> regions)
		{
			m_SourceCode = sourceCode;
			m_SourceCodeParser = parser;
			m_Usings = usings;
			m_Regions = regions;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the using declarations.
		/// </summary>
		public NList<string> Usings
		{
			get
			{
				return m_Usings;
			}
		}
		/// <summary>
		/// Gets the regions that should be exported.
		/// </summary>
		public NMap<string, string> Regions
		{
			get
			{
				return m_Regions;
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Updates usings and regions code and returns the resulting source code string.
		/// </summary>
		/// <returns>The source code with updated "usings" and regions.</returns>
		public string GetSourceCode()
		{
			string codeStr = m_SourceCode;

			// Update usings
			int namespaceIndex = m_SourceCodeParser.GetClassStartIndex(codeStr);
			codeStr = String.Join(Environment.NewLine, m_Usings.ToArray()) +
				Environment.NewLine + Environment.NewLine +
				codeStr.Substring(namespaceIndex);

			// Regions are already updated, so return the source code string
			return codeStr;
		}
		/// <summary>
		/// Updates the code of the region with the given name.
		/// </summary>
		/// <param name="regionName">The region to update.</param>
		/// <param name="newRegionCode">The new source code of the region.</param>
		public void ReplaceRegion(string regionName, string newRegionCode)
		{
			if (!m_Regions.Contains(regionName))
				return;

			// Update the regions map
			m_Regions.Set(regionName, newRegionCode);

			// Update the source code string
			NRangeI regionBounds = m_SourceCodeParser.GetRegionBounds(regionName, m_SourceCode);
			double length = regionBounds.End - regionBounds.Begin;
			if (length <= 0)
				return;

			m_SourceCode = m_SourceCode.Substring(0, regionBounds.Begin) + newRegionCode +
				m_SourceCode.Substring(regionBounds.End);
		}

		#endregion

		#region Fields

		private string m_SourceCode;
		private NSourceCodeParser m_SourceCodeParser;
		private NList<string> m_Usings;
		private NMap<string, string> m_Regions;

		#endregion
	}
}