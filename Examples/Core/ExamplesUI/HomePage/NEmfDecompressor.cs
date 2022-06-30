using System;
using System.IO;
using Nevron.Nov.Compression;
using Nevron.Nov.DataStructures;
using Nevron.Nov.Wmf;
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
            m_ImageMap = new NMap<string, byte[]>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets an iterator, which iterates through all metafile images.
        /// </summary>
        /// <returns></returns>
        public INIterator<NKeyValuePair<string, byte[]>> GetImageIterator()
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
            byte[] bytes = NStreamHelpers.ReadToEnd(zipItem.Stream);
            m_ImageMap.Add(zipItem.Name, bytes);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the meta image with the given name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public byte[] GetMetaImage(string name)
        {
            return m_ImageMap[name];
        }

        #endregion

        #region Fields

        private NMap<string, byte[]> m_ImageMap;

        #endregion
    }
}