using System.Collections.Generic;

namespace Wsdot.ArcGis.Server.Reporting.DataContracts
{
    /// <summary>
    /// Contains info about an ArcGIS Server.
    /// </summary>
    public class ServerInfo
    {
        /// <summary>
        /// The directory where Map Service Definitions (MSD) are contained.
        /// </summary>
        public string Directory { get; set; }
        /// <summary>
        /// List of <see cref="MsdInfo"/> objects for the MSD files on the server
        /// </summary>
        public IEnumerable<MsdInfo> MsdInfos { get; set; }
    }
}
