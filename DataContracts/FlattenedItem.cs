
namespace Wsdot.ArcGis.Server.Reporting.DataContracts
{
    /// <summary>
    /// Represents a map service layer in a flattened form, suitable for tabular output.
    /// </summary>
    public class FlattenedItem
    {
        /// <summary>
        /// Directory containing the MSD file.
        /// </summary>
        public string Directory { get; set; }

        /// <summary>
        /// The server portion of the connection string.
        /// </summary>
        public string ConnectionStringServer { get; set; }

        /// <summary>
        /// Path to the MSD file, relative to <see cref="FlattenedItem.Directory"/>.
        /// </summary>
        public string MsdPath { get; set; }

        /// <summary>
        /// Instance portion of the connection string.
        /// </summary>
        public string ConnectionStringInstance { get; set; }

        /// <summary>
        /// Database portion of the connection string.
        /// </summary>
        public string ConnectionStringDatabase { get; set; }

        /// <summary>
        /// Version portion of the connection string.
        /// </summary>
        public string ConnectionStringVersion { get; set; }

        /// <summary>
        /// Authentication Mode portion of the connection string.
        /// </summary>
        public string ConnectionStringAuthenticationMode { get; set; }

        /// <summary>
        /// DBClient portion of the connection string.
        /// </summary>
        public string ConnectionStringDBClient { get; set; }


        /// <summary>
        /// Server Instance portion of the connection string.
        /// </summary>
        public string ConnectionStringServerInstance { get; set; }

        /// <summary>
        /// User portion of the connection string.
        /// </summary>
        public string ConnectionStringUser { get; set; }

        /// <summary>
        /// DBConnectionProperties portion of the connection string.
        /// </summary>
        public string ConnectionStringDBConnectionProperties { get; set; }

        /// <summary>
        /// Workspace factory
        /// <example>
        /// <list type="bullet">
        /// <item>SDE</item>
        /// <item>Sql</item>
        /// <item>FileGDB</item>
        /// </list>
        /// </example>
        /// </summary>
        public string WorkspaceFactory { get; set; }

        /// <summary>
        /// The dataset that is the source for the data layer.
        /// </summary>
        public string DataSet { get; set; }

        /// <summary>
        /// The name of the map service layer.
        /// </summary>
        public string LayerName { get; set; }
    }
}
