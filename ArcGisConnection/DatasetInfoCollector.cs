using DataContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
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
        /// Examines all MSDs in a directory and provides output in a flattened format.
        /// </summary>
        /// <param name="msdDirectories">One or more directories containing MSD files.</param>
        /// <returns>Returns <see cref="FlattenedItem"/> objects.</returns>
        public static IEnumerable<FlattenedItem> GetFlattenedOutput(this IEnumerable<DirectoryInfo> msdDirectories)
        {
            foreach (DirectoryInfo dir in msdDirectories)
            {
                ServerInfo serverInfo = dir.GetServerInfo();
                foreach (MsdInfo msdInfo in serverInfo.MsdInfos)
                {
                    foreach (DataConnectionInfo connectionInfo in msdInfo.Connections)
                    {
                        var outItem = new FlattenedItem
                        {
                            DataSet = connectionInfo.DataSet,
                            Directory = serverInfo.Directory,
                            LayerName = connectionInfo.LayerName,
                            MsdPath = msdInfo.Path,
                            WorkspaceFactory = connectionInfo.ConnectionInfo.WorkspaceFactory
                        };
                        if (connectionInfo.ConnectionInfo != null && connectionInfo.ConnectionInfo.ConnectionString != null)
                        {
                            var cString = connectionInfo.ConnectionInfo.ConnectionString;
                            outItem.ConnectionStringAuthenticationMode = cString.AuthenticationMode;
                            outItem.ConnectionStringDatabase = cString.Database;
                            outItem.ConnectionStringDBClient = cString.DBClient;
                            outItem.ConnectionStringDBConnectionProperties = cString.DBConnectionProperties;
                            outItem.ConnectionStringInstance = cString.Instance;
                            outItem.ConnectionStringServer = cString.Server;
                            outItem.ConnectionStringServerInstance = cString.ServerInstance;
                            outItem.ConnectionStringUser = cString.User;
                            outItem.ConnectionStringVersion = cString.Version;
                        }
                        yield return outItem;
                    }
                }
            }
        }

        /// <summary>
        /// Enumerates through Map Service Definition (MSD) files.
        /// </summary>
        /// <param name="rootDir">
        /// The directory containing MSD files. 
        /// <example>C:\arcgisserver\directories\arcgissystem\arcgisinput</example>
        /// <example>\\myserver\sharename$\arcgisserver\directories\arcgissystem\arcgisinput</example>
        /// </param>
        /// <returns>Returns an enumeration of <see cref="FileInfo"/> objects.</returns>
        public static ServerInfo GetServerInfo(this DirectoryInfo rootDir)
        {
            return new ServerInfo
            {
                Directory = rootDir.FullName,
                MsdInfos = from msd in rootDir.EnumerateFiles("*.msd", SearchOption.AllDirectories)
                           select new MsdInfo { Path = msd.Name, Connections = msd.GetDatasetInfo() }
            };
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
        public static IEnumerable<ServerInfo> GetServerInfos(this IEnumerable<DirectoryInfo> rootDirs)
        {
            ////var output = rootDirs.Select(dir => new { key = dir.FullName, value = dir.GetMsds().GetDatasetInfos() }).ToDictionary;

            foreach (var dir in rootDirs)
            {
                //yield return dir.GetMsds().GetDatasetInfos();
                yield return dir.GetServerInfo();
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

            var serverInfos = dirs.GetServerInfos();

            foreach (var server in serverInfos)
            {

                textWriter.WriteLine(server.Directory);
                textWriter.WriteLine("--------------------------------");
                textWriter.WriteLine();

                foreach (var msd in server.MsdInfos)
                {
                    textWriter.WriteLine("### `{0}` ###", Path.GetFileName(msd.Path));

                    var groups = from c in msd.Connections
                                         where c.ConnectionInfo.WorkspaceFactory == "SDE"
                                         group c by c.ConnectionInfo.ConnectionString;


                    textWriter.WriteLine("#### SDE Connections ####");


                    foreach (var g in groups)
                    {
                        textWriter.WriteLine("##### Connection String #####");

                        var csDict = ConnectionString.GetConnectionStringParts(g.First().ConnectionInfo.ConnectionString.ToString());

                        foreach (var kvp in csDict)
                        {
                            textWriter.WriteLine("* `{0}`:\t`{1}`", kvp.Key, kvp.Value);
                        }

                        textWriter.WriteLine("##### Datasets #####");

                        var dataSets = (from c in g
                                           select c.DataSet).Distinct();

                        foreach (var ds in dataSets)
                        {
                            textWriter.WriteLine("* {0}", ds);
                        }
                    }
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
            // Regex matches file path with a '/' character and ending in ".xml".
            var inFolderRe = new Regex(@"^[^\/]+\/[^\/]+\.xml$");
            using (var fileStream = file.OpenRead())
            using (var zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Read))
            {
                var xmls = from f in zipArchive.Entries 
                           where inFolderRe.IsMatch(f.FullName) 
                           select f;

                foreach (var xml in xmls)
                {
                    DataConnectionInfo dcInfo = xml.GetDataConnectionInfo(file);
                    if (dcInfo != null && !string.IsNullOrWhiteSpace(dcInfo.DataSet))
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
            var xStream = xmlEntry.Open();
            using (var xReader = XmlReader.Create(xStream))
            {
                var xDoc = XDocument.Load(xReader);
                dcInfo = new DataConnectionInfo(xDoc);
            }
            return dcInfo;
        }
    }
}
