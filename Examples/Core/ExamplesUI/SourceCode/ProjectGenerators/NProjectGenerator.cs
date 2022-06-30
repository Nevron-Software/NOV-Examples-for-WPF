using System;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;

using Nevron.Nov.Compression;
using Nevron.Nov.DataStructures;
using Nevron.Nov.IO;
using Nevron.Nov.Text;
using Nevron.Nov.Xml;

namespace Nevron.Nov.Examples
{
	internal abstract class NProjectGenerator
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		protected NProjectGenerator()
		{
			m_SourceCodeParser = CreateSourceCodeParser();
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NProjectGenerator()
		{
			// Create a list of the NOV assemblies
			NovAssemblies = new NList<NAssemblyInfo>();

			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			for (int i = 0; i < assemblies.Length; i++)
			{
				Assembly assembly = assemblies[i];
				string assemblyName = assembly.GetName().Name;

				if (assemblyName.StartsWith("Nevron.Nov.", StringComparison.OrdinalIgnoreCase) &&
					!assemblyName.StartsWith("Nevron.Nov.Examples.", StringComparison.OrdinalIgnoreCase))
				{
					// This is a NOV assembly
					NovAssemblies.Add(new NAssemblyInfo(assembly));
				}
			}

			// Determine the target framework of the entry assembly
			TargetFrameworkAttribute targetFramework = Assembly.GetEntryAssembly().GetCustomAttribute<TargetFrameworkAttribute>();
			IsNet50orNewer = targetFramework != null && targetFramework.FrameworkName != null &&
				targetFramework.FrameworkName.StartsWith(".NETCoreApp", StringComparison.OrdinalIgnoreCase);
		}

		#endregion

		#region Properties - Must Override

		/// <summary>
		/// Gets the source file extension.
		/// </summary>
		public abstract string SourceFileExtension
		{
			get;
		}

		/// <summary>
		/// Gets the programming language this generator generates projects in.
		/// </summary>
		protected abstract ENProgrammingLanguage Language
		{
			get;
		}
		/// <summary>
		/// Gets the project file extension.
		/// </summary>
		protected abstract string ProjectFileExtension
		{
			get;
		}

		/// <summary>
		/// Gets an embedded resource that contains the zipped project template.
		/// </summary>
		protected abstract NEmbeddedResource ProjectTemplate
		{
			get;
		}
		/// <summary>
		/// Gets the name of the main source code file.
		/// </summary>
		protected abstract string MainSourceCodeFileName
		{
			get;
		}

		#endregion

		#region Public Methods - Generate

		/// <summary>
		/// Generates a ZIP archive with a sample project for the given NOV example into the specified folder.
		/// </summary>
		/// <param name="folder"></param>
		/// <param name="example"></param>
		public byte[] Generate(NExampleBase example)
		{
			// 1. Decompress the project template
			NZipDecompressor decompressor = new NZipDecompressor();
			using (MemoryStream inputStream = new MemoryStream(ProjectTemplate.Data))
			{
				NCompression.DecompressZip(inputStream, decompressor);
			}

			// 2. Modify the project
			NList<NZipItem> items = decompressor.Items;
			if (items.Count > 0)
			{
				string templateName = NPath.Current.GetParentFolderPath(items[0].Name);
				templateName = templateName.Remove(templateName.Length - 1);
				NList<NZipItem> newItems = new NList<NZipItem>();

				for (int i = 0; i < items.Count; i++)
				{
					// Process the ZIP item and update its stream
					NZipItem item = items[i];
					ProcessItem(item, example, newItems);

					// Update the name of the ZIP item
					string exampleName = GetExampleName(example);
					item.Name = item.Name.Replace(templateName, exampleName);
				}

				if (newItems.Count > 0)
				{
					// Add the newly generated items to the list of items
					items.AddRange(newItems);
				}
			}

			// Add platform specific assemblies
			AddPlatformAssemblies(items, example);

			// Add NOV assemblies
			AddNovAssemblies(items, example);

			// 3. Compress the modified project back to a ZIP archive
			NProjectZipCompressor compressor = new NProjectZipCompressor(items);

			using (MemoryStream outputStream = new MemoryStream())
			{
				NCompression.CompressZip(outputStream, ENCompressionLevel.BestSpeed, compressor);
				return outputStream.ToArray();
			}
		}

		#endregion

		#region Protected - Must Override

		protected abstract NSourceCodeParser CreateSourceCodeParser();

		#endregion

		#region Protected Overridable - Assemblies

		protected virtual void AddPlatformAssemblies(NList<NZipItem> items, NExampleBase example)
		{
		}

		#endregion

		#region Protected Overridable - Project Items Processing

		protected virtual void ProcessItem(NZipItem item, NExampleBase example, NList<NZipItem> newItems)
		{
			const StringComparison StrComparison = StringComparison.OrdinalIgnoreCase;

			string extension = NPath.Current.GetExtension(item.Name);
			if (String.Equals(extension, SourceFileExtension, StrComparison))
			{
				if (item.Name == MainSourceCodeFileName)
				{
					// Load the code of the main source code file and modify it
					// by adding the example's code
					string mainFileCode = ReadSourceCode(item.Stream);
					mainFileCode = ProcessMainSourceCodeFile(mainFileCode, example);

					// Update the stream
					item.Stream = SaveToMemoryStream(mainFileCode);
				}
			}
			else if (String.Equals(extension, ProjectFileExtension, StrComparison))
			{
				// Load and modify the project file
				NXmlDocument xmlDocument = NXmlDocument.LoadFromStream(item.Stream);
				ProcessProjectFile(xmlDocument, example, newItems);

				// Save the project file back to the ZIP item
				item.Stream = SaveToMemoryStream(xmlDocument);
			}
			else if (String.Equals(extension, "sln", StrComparison))
			{
				// Load and modify the solution file
				string solutionXml = NStreamHelpers.ReadToEndAsString(item.Stream);
				solutionXml = ProcessSolutionFile(solutionXml, example);

				// Save the solution file back to the ZIP item
				item.Stream = SaveToMemoryStream(solutionXml);
			}
		}
		protected virtual void ProcessProjectFile(NXmlDocument xmlDocument, NExampleBase example, NList<NZipItem> newItems)
		{
			// Update assembly name
			string exampleName = GetExampleName(example);
			NXmlElement assemblyNameElement = xmlDocument.GetFirstDescendant("AssemblyName") as NXmlElement;
			if (assemblyNameElement != null)
			{
				assemblyNameElement.InnerText = exampleName;
			}

			// Update the project references to points to the "References" sub folder
			NList<NXmlNode> referenceNodes = xmlDocument.GetDescendants("Reference");
			if (referenceNodes.Count == 0)
				return;

			for (int i = 0; i < referenceNodes.Count; i++)
			{
				NXmlElement referenceElement = referenceNodes[i] as NXmlElement;
				if (referenceElement == null)
					continue;

				// Get the hint path child of the current Reference element
				NXmlElement hintPathElement = referenceElement.GetFirstChild("HintPath") as NXmlElement;
				if (hintPathElement == null)
					continue;

				// Get the reference name and hint path
				string referenceName = referenceElement.GetAttributeValue("Include");
				string oldHintPath = hintPathElement.InnerText;

				string newHintPath;
				if (ShouldUpdateHintPath(referenceName, oldHintPath, out newHintPath))
				{
					// Update the hint path
					hintPathElement.InnerText = newHintPath;
				}
			}
		}
		protected virtual string ProcessSolutionFile(string solutionXml, NExampleBase example)
		{
			// Find the template project name
			int endIndex = solutionXml.IndexOf("proj\"");
			if (endIndex == -1)
				return solutionXml;

			endIndex = solutionXml.LastIndexOf('.', endIndex);
			if (endIndex == -1)
				return solutionXml;

			int startIndex = solutionXml.LastIndexOf('"', endIndex);
			if (startIndex == -1)
				return solutionXml;

			// Replace the template project name with the example name
			string templateProjectName = solutionXml.Substring(startIndex + 1, endIndex - startIndex - 1);
			return solutionXml.Replace(templateProjectName, GetExampleName(example));
		}

		/// <summary>
		/// Indicates whether the given reference hint path should be updated.
		/// </summary>
		/// <param name="referenceName">The reference name.</param>
		/// <param name="oldHintPath">The old hint path.</param>
		/// <param name="newHintPath">The new hint path.</param>
		/// <returns>True if the reference hint path should be updated.</returns>
		protected virtual bool ShouldUpdateHintPath(string referenceName, string oldHintPath, out string newHintPath)
		{
			if (referenceName == null || !referenceName.StartsWith("Nevron.Nov."))
			{
				newHintPath = null;
				return false;
			}
			else
			{
				newHintPath = @"References\" + referenceName + ".dll";
				return true;
			}
		}
		/// <summary>
		/// Extracts the source code of the given example.
		/// </summary>
		/// <param name="example"></param>
		/// <returns></returns>
		protected virtual NSourceCodeFile ExtractExampleSourceCode(NExampleBase example)
		{
			// Get the current example's source code
			Stream sourceCodeStream = NExampleBase.GetExampleSourceCodeStream(example.Schema, Language);
			string sourceCodeStr = ReadSourceCode(sourceCodeStream);

			// Parse the example's source code
			return m_SourceCodeParser.Parse(sourceCodeStr);
		}
		/// <summary>
		/// Processes the main source code file's code.
		/// </summary>
		/// <param name="mainFileCodeStr"></param>
		/// <param name="example"></param>
		protected virtual string ProcessMainSourceCodeFile(string mainFileCodeStr, NExampleBase example)
		{
			// Parse the example's source code
			NSourceCodeFile exampleCode = ExtractExampleSourceCode(example);

			// Update the example's window title
			mainFileCodeStr = mainFileCodeStr.Replace(TitlePlaceholder, example.GetType().Name);

			// Parse the form's source code
			NSourceCodeFile mainFileCode = m_SourceCodeParser.Parse(mainFileCodeStr);

			// Add example's usings to the form's code
			mainFileCode.Usings.AddRangeNoDuplicates(exampleCode.Usings);

			// Add the example namespace as a using
			mainFileCode.Usings.Add(m_SourceCodeParser.UsingKeyword + " " + example.GetType().Namespace + m_SourceCodeParser.StatementTerminator);

			// Sort the usings
			mainFileCode.Usings.Sort();

			// Replace some of the form's regions with those of the example
			var iter = exampleCode.Regions.GetIterator();
			while (iter.MoveNext())
			{
				string regionName = iter.Current.Key;
				string regionCodeStr = iter.Current.Value;

				// Process the region code
				regionCodeStr = ProcessRegionCode(regionName, regionCodeStr);

				// Replace the form's region code with the one from the example
				mainFileCode.ReplaceRegion(regionName, regionCodeStr);
			}

			return mainFileCode.GetSourceCode();
		}
		/// <summary>
		/// Processes the source code of the given region.
		/// </summary>
		/// <param name="regionName"></param>
		/// <param name="regionCodeStr"></param>
		/// <returns></returns>
		protected virtual string ProcessRegionCode(string regionName, string regionCodeStr)
		{
			if (regionName != "Nested Types")
			{
				// Remove the "override" keyword
				regionCodeStr = regionCodeStr.Replace(m_SourceCodeParser.OverrideKeyword + " ", String.Empty);
			}

			// Replace "NResources" with "Nevron.Nov.Examples.NResources"
			regionCodeStr = regionCodeStr.Replace(" NResources.", " Nevron.Nov.Examples.NResources.");
			regionCodeStr = regionCodeStr.Replace("\tNResources.", "\tNevron.Nov.Examples.NResources.");
			regionCodeStr = regionCodeStr.Replace("(NResources.", "(Nevron.Nov.Examples.NResources.");

			return regionCodeStr;
		}

		#endregion

		#region Implementation

		private void AddNovAssemblies(NList<NZipItem> items, NExampleBase example)
		{
			string exampleName = GetExampleName(example);

			for (int i = 0; i < NovAssemblies.Count; i++)
			{
				NAssemblyInfo novAssembly = NovAssemblies[i];
				string path = exampleName + "/References/" + novAssembly.Name + ".dll";
				items.Add(new NZipItem(path, new MemoryStream(novAssembly.Data)));
			}
		}

		#endregion

		#region Static Methods

		protected static string ReadSourceCode(Stream stream)
		{
			int byteIndex = 0;
			byte[] data = NStreamHelpers.ReadToEnd(stream);

			if (data.Length > 3 && data[0] == 0xEF && data[1] == 0xBB && data[2] == 0xBF)
			{
				// The data starts with a BOM
				byteIndex = 3;
			}

			return NEncoding.UTF8.GetString(data, byteIndex, data.Length - byteIndex);
		}
		protected static string GetExampleName(NExampleBase example)
		{
			return example.Schema.DomType.CLRType.Name;
		}
		protected static MemoryStream SaveToMemoryStream(NXmlDocument xmlDocument)
		{
			MemoryStream memoryStream = new MemoryStream();
			xmlDocument.SaveToStream(memoryStream);
			memoryStream.Position = 0;
			return memoryStream;
		}
		protected static MemoryStream SaveToMemoryStream(string text)
		{
			return new MemoryStream(NEncoding.UTF8.GetBytes(text));
		}

		#endregion

		#region Fields

		private NSourceCodeParser m_SourceCodeParser;

		#endregion

		#region Constants

		private const string TitlePlaceholder = "{Title}";
		private static readonly NList<NAssemblyInfo> NovAssemblies;

		/// <summary>
		/// Indicates whether the entry assembly targets .NET Framework 5.0 or newer.
		/// </summary>
		protected static readonly bool IsNet50orNewer;

		#endregion

		#region Nested Types

		internal class NAssemblyInfo
		{
			#region Constructors

			public NAssemblyInfo(Assembly assembly)
			{
				Name = assembly.GetName().Name;
				Data = GetAssemblyBytes(assembly);
			}

			#endregion

			#region Fields

			public readonly string Name;
			public readonly byte[] Data;

			#endregion

			#region Static Methods

			private static byte[] GetAssemblyBytes(Assembly assembly)
			{
				byte[] assemblyBytes = null;

				// Try get the "Assembly.GetRawBytes" method via reflection
				MethodInfo methodGetRawBytes = assembly.GetType().GetMethod("GetRawBytes",
					BindingFlags.Instance | BindingFlags.NonPublic);

				if (methodGetRawBytes != null)
				{
					// The "Assembly.GetRawBytes" method found, so call it to get the bytes of the assembly
					object result = methodGetRawBytes.Invoke(assembly, null);
					assemblyBytes = (byte[])result;
				}
				else if (!String.IsNullOrEmpty(assembly.Location))
				{
					// The "Assembly.GetRawBytes" method not found, so read the assembly bytes from disk
					assemblyBytes = System.IO.File.ReadAllBytes(assembly.Location);
				}
				else
				{
					NDebug.Assert(false, "Neighter 'Assembly.GetRawBytes', nor 'Assembly.Location' are available");
				}

				return assemblyBytes;
			}

			#endregion
		}

		internal class NProjectZipCompressor : INZipCompressor
		{
			#region Constructors

			/// <summary>
			/// Initializing constructor.
			/// </summary>
			/// <param name="items"></param>
			public NProjectZipCompressor(NList<NZipItem> items)
			{
				m_Items = items;
			}

			#endregion

			#region INZipCompressor

			public INIterable<NZipItem> GetItemsToCompress()
			{
				return m_Items;
			}
			public void OnItemCompressed(NZipItem item)
			{
				// Close the compressed stream
				item.Stream.Dispose();
			}

			#endregion

			#region Fields

			private NList<NZipItem> m_Items;

			#endregion
		}

		#endregion
	}
}