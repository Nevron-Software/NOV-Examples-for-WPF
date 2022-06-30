using System;
using System.IO;

using Nevron.Nov.DataStructures;
using Nevron.Nov.Graphics;

namespace Nevron.Nov.Examples
{
	/// <summary>
	/// Parses the source code of a NOV example. Used in the exporting process of
	/// a NOV example to a Visual Studio solution.
	/// </summary>
	internal abstract class NSourceCodeParser
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		protected NSourceCodeParser()
		{
		}

		#endregion

		#region Properties - Must Override

		public abstract string UsingKeyword
		{
			get;
		}
		public abstract string NamespaceKeyword
		{
			get;
		}
		public abstract string OverrideKeyword
		{
			get;
		}
		public abstract string RegionBeginKeyword
		{
			get;
		}
		public abstract string RegionEndKeyword
		{
			get;
		}
		public abstract string StatementTerminator
		{
			get;
		}
		public abstract string RegionNamePrefix
		{
			get;
		}
		public abstract string RegionNameSuffix
		{
			get;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Parses the given source code and populates this object's properties.
		/// </summary>
		/// <param name="sourceCode"></param>
		public NSourceCodeFile Parse(string sourceCode)
		{
			// Extract and append the "using directives"
			NList<string> usings = ExtractUsingDirectives(sourceCode);

			// Extract some regions from the source code file
			NMap<string, string> regions = new NMap<string, string>(RegionsToExtract.Length);
			for (int i = 0; i < RegionsToExtract.Length; i++)
			{
				string regionName = RegionsToExtract[i];
				string regionCode = ExtractRegionCode(regionName, sourceCode);

				if (!String.IsNullOrEmpty(regionCode))
				{
					regions.Set(regionName, regionCode);
				}
			}

			return new NSourceCodeFile(sourceCode, this, usings, regions);
		}

		#endregion

		#region Internal Methods

		/// <summary>
		/// Gets the namespace or class start index in the given source code string.
		/// </summary>
		/// <param name="sourceCode"></param>
		/// <returns></returns>
		internal virtual int GetClassStartIndex(string sourceCode)
		{
			return sourceCode.IndexOf(NamespaceKeyword + " ");
		}
		/// <summary>
		/// Gets the start and end index of the region with the given name in the specified source code string
		/// including the "#region Name" and "#endregion" directives.
		/// </summary>
		/// <param name="regionName">The region to search for.</param>
		/// <param name="sourceCode">The source code string to search in.</param>
		/// <returns></returns>
		internal NRangeI GetRegionBounds(string regionName, string sourceCode)
		{
			string BeginRegionString = RegionBeginKeyword + " ";

			// Determine the begin region index in the source code string
			int index = 0;
			int startIndex;
			string encapsulatedRegionName = RegionNamePrefix + regionName + RegionNameSuffix;

			while ((startIndex = sourceCode.IndexOf(BeginRegionString + encapsulatedRegionName, index)) != -1)
			{
				// We have found a region that matches the desired region name
				index = startIndex + BeginRegionString.Length + encapsulatedRegionName.Length;

				// Exit the loop if a new line follows the region name
				char nextChar = sourceCode[index + 1];
				if (nextChar == '\n' || nextChar == '\r')
					break;
			}

			if (startIndex == -1)
				return NRangeI.Zero;

			int prevNewLineIndex = sourceCode.LastIndexOf('\n', startIndex);
			if (prevNewLineIndex != -1)
			{
				startIndex = prevNewLineIndex + 1;
			}

			// Determine the end region index in the source code string
			int regionLevel = 0;
			int loopEndIndex = sourceCode.Length - RegionEndKeyword.Length;
			int endIndex = -1;

			for (; index < loopEndIndex; index++)
			{
				if (sourceCode[index] != '#')
					continue;

				if (ContainsSubstring(sourceCode, BeginRegionString, index))
				{
					// A "#region Name" directive - increase the current region level
					regionLevel++;
				}
				else if (ContainsSubstring(sourceCode, RegionEndKeyword, index))
				{
					if (regionLevel > 0)
					{
						// End of a nested region - so decrease the current region level
						regionLevel--;
					}
					else
					{
						// End of the region - stop the loop
						endIndex = index;
						break;
					}
				}
			}

			if (endIndex == -1)
			{
				NDebug.Assert(false, $"\"{RegionEndKeyword}\" directive not found for region \"{regionName}\"");
				return NRangeI.Zero;
			}

			endIndex += RegionEndKeyword.Length;

			return new NRangeI(startIndex, endIndex);
		}

		#endregion

		#region Implementation

		/// <summary>
		/// Gets a list with the "using" directives of the given source code.
		/// </summary>
		/// <param name="sourceCode"></param>
		/// <returns></returns>
		private NList<string> ExtractUsingDirectives(string sourceCode)
		{
			NList<string> usingDirectives = new NList<string>();
			string usingKeyword = UsingKeyword + " ";
			string namespaceKeyword = NamespaceKeyword + " ";

			using (StringReader reader = new StringReader(sourceCode))
			{
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					if (line.StartsWith(usingKeyword, StringComparison.Ordinal))
					{
						usingDirectives.Add(line.Trim());
					}
					else if (line.StartsWith(namespaceKeyword, StringComparison.Ordinal))
					{
						break;
					}
				}
			}

			return usingDirectives;
		}
		/// <summary>
		/// Gets the code of the given region including the "#region Name" and "#endregion" directives.
		/// Returns an empty string if such region was not found in the given source code.
		/// </summary>
		/// <param name="regionName"></param>
		/// <param name="sourceCode"></param>
		/// <returns></returns>
		private string ExtractRegionCode(string regionName, string sourceCode)
		{
			NRangeI regionBounds = GetRegionBounds(regionName, sourceCode);
			double length = regionBounds.End - regionBounds.Begin;
			if (length <= 0)
				return String.Empty;

			return sourceCode.Substring(regionBounds.Begin, regionBounds.GetLength());
		}

		#endregion

		#region Static Methods

		/// <summary>
		/// Checks whenter the given string contains the given substring at the specified position.
		/// </summary>
		/// <param name="str"></param>
		/// <param name="substr"></param>
		/// <returns></returns>
		private static bool ContainsSubstring(string str, string substr, int index)
		{
			if (index + substr.Length >= str.Length)
				return false;

			for (int i = 0; i < substr.Length; i++)
			{
				if (str[index + i] != substr[i])
					return false;
			}

			return true;
		}

		#endregion

		#region Constants

		/// <summary>
		/// The names of the regions that should be extracted from the example's source code.
		/// </summary>
		private static readonly string[] RegionsToExtract = new string[] {
			"Example",
			"Implementation",
			"Event Handlers",
			"Fields",
			"Static Methods",
			"Constants",
			"Nested Types"
		};

		#endregion
	}
}