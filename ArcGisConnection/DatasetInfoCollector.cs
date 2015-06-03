using DataContracts;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace ArcGisConnection
{
    /// <summary>
    /// Static class that provides functions for gathering information about datasets in use by ArcGIS Server.
    /// </summary>
    public static class DatasetInfoCollector
    {
        /// <summary>
        /// Enumerates through Map Service Definition (MSD) files.
        /// </summary>
        /// <param name="rootDir">
        /// The directory containing MSD files. 
        /// <example>C:\arcgisserver\directories\arcgissystem\arcgisinput</example>
        /// <example>\\myserver\sharename$\arcgisserver\directories\arcgissystem\arcgisinput</example>
        /// </param>
        /// <returns>Returns an enumeration of <see cref="FileInfo"/> objects.</returns>
        private static IEnumerable<FileInfo> GetMsds(this DirectoryInfo rootDir)
        {
            var msds = rootDir.EnumerateFiles("*.msd", SearchOption.AllDirectories);
            return msds;
        }

        /// <summary>
        /// Enumerates through Map Service Definition (MSD) files' connection information.
        /// </summary>
        /// <param name="files">MSD files</param>
        /// <returns>Returns an enumeration of <see cref="DataConnectionInfo"/> objects.</returns>
        public static IEnumerable<IEnumerable<DataConnectionInfo>> GetDatasetInfos(this IEnumerable<FileInfo> files)
        {
            foreach (var msd in files)
            {
                var datasetInfos = msd.GetDatasetInfo();
                yield return datasetInfos;
            }
        }

        /// <summary>
        /// Enumerates through Map Service Definition (MSD) files' connection information.
        /// </summary>
        /// <param name="rootDir">
        /// The directory containing MSD files. 
        /// <example>C:\arcgisserver\directories\arcgissystem\arcgisinput</example>
        /// <example>\\myserver\sharename$\arcgisserver\directories\arcgissystem\arcgisinput</example>
        /// </param>
        /// <returns>Returns an enumeration of <see cref="DataConnectionInfo"/> objects.</returns>
        public static IEnumerable<IEnumerable<DataConnectionInfo>> GetDatasetInfos(this DirectoryInfo rootDir)
        {
            return GetMsds(rootDir).GetDatasetInfos();
        }

        /// <summary>
        /// Gets connection info from a Map Service Definition (MSD) file.
        /// </summary>
        /// <param name="file">Map Service Definition (MSD) file</param>
        /// <returns>Returns an enumeration of <see cref="DataConnectionInfo"/> objects.</returns>
        public static IEnumerable<DataConnectionInfo> GetDatasetInfo(this FileInfo file)
        {
            using (var fileStream = file.OpenRead())
            using (var zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Read))
            {
                var xmls = zipArchive.Entries.Where(f => f.FullName.StartsWith("layers"));
                foreach (var xml in xmls)
                {
                    DataConnectionInfo dcInfo = xml.GetDataConnectionInfo();
                    if (dcInfo != null)
                    {
                        yield return dcInfo;
                    }
                }
            }
        }

        /// <summary>
        /// Gets connection info from an XML file.
        /// </summary>
        /// <param name="xmlEntry">XML file zip entry from an MSD file.</param>
        /// <returns>Returns a <see cref="DataConnectionInfo"/></returns>
        public static DataConnectionInfo GetDataConnectionInfo(this ZipArchiveEntry xmlEntry)
        {
            DataConnectionInfo dcInfo = null;
            using (var xStream = xmlEntry.Open())
            using (var xReader = XmlReader.Create(xStream))
            {
                var xDoc = XDocument.Load(xReader);
                dcInfo = new DataConnectionInfo(xDoc);
            }
            return dcInfo;
        }
    }
}
