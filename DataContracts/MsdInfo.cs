using System.Collections.Generic;

namespace Wsdot.ArcGis.Server.Reporting.DataContracts
{
    /// <summary>
    /// Information about a Map Service Definition (MSD) file.
    /// </summary>
    public class MsdInfo
    {
        /// <summary>
        /// Path to the file, relative to <see cref="ServerInfo.Directory"/>.
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// List of connections used by layers in the MSD file.
        /// </summary>
        public IEnumerable<DataConnectionInfo> Connections { get; set; }
    }
}
