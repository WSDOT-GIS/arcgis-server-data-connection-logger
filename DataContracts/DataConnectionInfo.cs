using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace DataContracts
{
    /// <summary>
    /// Represents a data connection.
    /// </summary>
    public class DataConnectionInfo
    {
        /// <summary>
        /// Connection string
        /// </summary>
        public ConnectionInfo ConnectionInfo { get; set; }
        public string DataSet { get; set; }
        public string LayerName { get; set; }

        /// <summary>
        /// Creates a new instance of this class.
        /// </summary>
        public DataConnectionInfo()
        {

        }

        /// <summary>
        /// Creates a new instance of this class using an XML Document.
        /// </summary>
        /// <param name="xDoc">Layer XML document from within an Map Service Definition (MSD) archive.</param>
        public DataConnectionInfo(XContainer xDoc)
        {
            var dataConnection = xDoc.Descendants("DataConnection").FirstOrDefault();
            var nameElement = xDoc.Descendants("Name").FirstOrDefault();
            this.LayerName = nameElement != null ? nameElement.Value : null;
            var passwordRe = new Regex(@"(\w?PASSWORD)=[^=;]+", RegexOptions.IgnoreCase);
            if (dataConnection != null)
            {
                var csElement = dataConnection.Element("WorkspaceConnectionString");
                var dsElement = dataConnection.Element("Dataset");
                var wsElement = dataConnection.Element("WorkspaceFactory");

                var connectionInfo = csElement != null || wsElement != null ? new ConnectionInfo
                {
                    ConnectionString = csElement != null ? new ConnectionString(csElement.Value) : null,
                    WorkspaceFactory = wsElement != null ? wsElement.Value : null
                } : null;

                //this.ConnectionString = csElement != null ? csElement.Value : null;
                this.DataSet = dsElement != null ? dsElement.Value : null;
                this.ConnectionInfo = connectionInfo;
            }
        }
    }
}
