using System;
using System.IO;

using Nevron.Nov.Compression;
using Nevron.Nov.DataStructures;
using Nevron.Nov.IO;

namespace Nevron.Nov.Examples
{
	internal class NSourceCodeDecompressor : INZipDecompressor
	{
		#region Constructors

		public NSourceCodeDecompressor(Type exampleType, ENProgrammingLanguage language)
		{
			m_ExampleNamespace = exampleType.Namespace;
			m_ExampleFileName = exampleType.Name +
				(language == ENProgrammingLanguage.CSharp ? ".cs" : ".vb");
			m_Language = language;
			m_Items = new NList<NZipItem>();
		}

		#endregion

		#region INZipDecompressor

		public bool Filter(NZipItem item)
		{
			string fileName = NPath.Current.GetFileName(item.Name);
			return String.Equals(fileName, m_ExampleFileName, StringComparison.OrdinalIgnoreCase);
		}
		public void OnItemDecompressed(NZipItem item)
		{
			m_Items.Add(item);
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Gets the source code stream.
		/// </summary>
		/// <returns></returns>
		public MemoryStream GetSourceCodeStream()
		{
			if (m_Items.Count == 0)
				return null;
			else if (m_Items.Count == 1)
				return (MemoryStream)m_Items[0].Stream;

			// Analyze the example names and returns the best matching one
			NZipItem bestItem = null;
			int bestScore = -1;
			string[] tokens = m_ExampleNamespace.Split('.');

			for (int i = 0; i < m_Items.Count; i++)
			{
				NZipItem item = m_Items[i];

				// Find how many tokens from the namespace are present in the current file name
				string dirName = NPath.Current.GetParentFolderPath(m_Items[i].Name);
				int score = 0;

				for (int j = 0; j < tokens.Length; j++)
				{
					if (dirName.Contains(tokens[j]))
					{
						score++;
					}
				}

				if (score > bestScore)
				{
					// The current item is a better match, so store it
					bestItem = item;
					bestScore = score;
				}
			}

			return (MemoryStream)bestItem.Stream;
		}

		#endregion

		#region Fields

		private string m_ExampleNamespace;
		private string m_ExampleFileName;
		private NList<NZipItem> m_Items;
		private ENProgrammingLanguage m_Language;

		#endregion
	}
}