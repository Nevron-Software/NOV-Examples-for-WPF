using System;

using Nevron.Nov.Compression;
using Nevron.Nov.DataStructures;
using Nevron.Nov.Graphics;
using Nevron.Nov.IO;

namespace Nevron.Nov.Examples
{
	/// <summary>
	/// Decompresses EMF image files from a ZIP archive.
	/// </summary>
	internal class NEmfDecompressor : INZipDecompressor
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NEmfDecompressor()
        {
            m_ImageMap = new NMap<string, NImage>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets an iterator, which iterates through all metafile images.
        /// </summary>
        /// <returns></returns>
        public INIterator<NKeyValuePair<string, NImage>> GetImageIterator()
        {
            return m_ImageMap.GetIterator();
        }

        #endregion

        #region INZipDecompressor

        public bool Filter(NZipItem item)
        {
            return item.Name.EndsWith(".emf", StringComparison.OrdinalIgnoreCase);
        }
        public void OnItemDecompressed(NZipItem zipItem)
        {
			NImage image = NImage.FromStream(zipItem.Stream);
			string imageName = NPath.Current.GetFileNameWithoutExtension(zipItem.Name);
            m_ImageMap.Add(imageName, image);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the meta image with the given name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public NImage GetMetaImage(string name)
        {
            return (NImage)m_ImageMap[name].DeepClone();
        }

        #endregion

        #region Fields

		/// <summary>
		/// A map with image names (without the extension) and images.
		/// </summary>
        internal NMap<string, NImage> m_ImageMap;

        #endregion
    }
}