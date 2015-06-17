
namespace DataContracts
{
    public class FlattenedItem
    {
        public string Directory { get; set; }

        public string ConnectionStringServer { get; set; }

        public string MsdPath { get; set; }

        public string ConnectionStringInstance { get; set; }

        public string ConnectionStringDatabase { get; set; }

        public string ConnectionStringVersion { get; set; }

        public string ConnectionStringAuthenticationMode { get; set; }

        public string ConnectionStringDBClient { get; set; }

        public string ConnectionStringServerInstance { get; set; }

        public string ConnectionStringUser { get; set; }

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

        public string DataSet { get; set; }
        public string LayerName { get; set; }
    }
}
