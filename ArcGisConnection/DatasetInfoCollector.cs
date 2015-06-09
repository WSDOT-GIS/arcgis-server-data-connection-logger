using DataContracts;
using System;
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

        /// Enumerates through Map Service Definition (MSD) files' connection information.
        /// </summary>
        /// <param name="rootDir">
        /// The directory containing MSD files. 
        /// <example>C:\arcgisserver\directories\arcgissystem\arcgisinput</example>
        /// <example>\\myserver\sharename$\arcgisserver\directories\arcgissystem\arcgisinput</example>
        /// </param>
        /// <returns>Returns an enumeration of <see cref="DataConnectionInfo"/> objects.</returns>
        public static IEnumerable<IEnumerable<IEnumerable<DataConnectionInfo>>> GetDatasetInfos(this IEnumerable<DirectoryInfo> rootDirs)
        {
            ////var output = rootDirs.Select(dir => new { key = dir.FullName, value = dir.GetMsds().GetDatasetInfos() }).ToDictionary;

            foreach (var dir in rootDirs)
            {
                yield return dir.GetMsds().GetDatasetInfos();
            }
        }

        /// <summary>
        /// Writes a Markdown report of datasets in use by MSDs in the given directories.
        /// </summary>
        /// <param name="dirs"></param>
        /// <param name="textWriter"></param>
        public static void WriteMarkdown(this IEnumerable<DirectoryInfo> dirs, TextWriter textWriter)
        {
            textWriter.WriteLine("Datasets in use on {0}", DateTimeOffset.Now);
            textWriter.WriteLine("===========================================");
            textWriter.WriteLine();

            foreach (var dir in dirs)
            {

                textWriter.WriteLine(dir.FullName);
                textWriter.WriteLine("--------------------------------");
                textWriter.WriteLine();

                var dsGroups = from d in dir.GetDatasetInfos().SelectMany(d => d)
                               where !string.IsNullOrWhiteSpace(d.ConnectionString)
                               group d by d.ConnectionString;

                foreach (var g in dsGroups)
                {
                    textWriter.WriteLine("### `{0}` ###", g.Key);
                    textWriter.WriteLine();

                    var dict = g.First().GetConnectionStringParts();
                    foreach (var kvp in dict)
                    {
                        textWriter.WriteLine("* `{0}`:\t`{1}`", kvp.Key, kvp.Value);
                    }

                    textWriter.WriteLine("#### Tables ####\n");

                    foreach (var dataSet in g.Select(ds => ds.DataSet).OrderBy(ds => ds).Distinct())
                    {
                        textWriter.WriteLine("* `{0}`", dataSet);
                    }
                    textWriter.WriteLine();
                }
            }
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
                    DataConnectionInfo dcInfo = xml.GetDataConnectionInfo(file);
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
        public static DataConnectionInfo GetDataConnectionInfo(this ZipArchiveEntry xmlEntry, FileInfo fileInfo)
        {
            DataConnectionInfo dcInfo = null;
            using (var xStream = xmlEntry.Open())
            using (var xReader = XmlReader.Create(xStream))
            {
                var xDoc = XDocument.Load(xReader);
                dcInfo = new DataConnectionInfo(xDoc, fileInfo.FullName);
            }
            return dcInfo;
        }
    }
}
