using System.Collections.Generic;
using System.Linq;
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
        public string ConnectionString { get; set; }
        public string DataSet { get; set; }
        public string WorkspaceFactory { get; set; }

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
            if (dataConnection != null)
            {
                var csElement = dataConnection.Element("WorkspaceConnectionString");
                var dsElement = dataConnection.Element("Dataset");
                var wsElement = dataConnection.Element("WorkspaceFactory");

                ConnectionString = csElement != null ? csElement.Value : null;
                DataSet = dsElement != null ? dsElement.Value : null;
                WorkspaceFactory = wsElement != null ? wsElement.Value : null;
            }
        }

        /// <summary>
        /// Splits the parts of a connection string into a <see cref="Dictionary&lt;K,V&gt;"/>
        /// </summary>
        /// <returns>A dictionary of connection string parameters, or null if there is no <see cref="DataConnectionInfo.ConnectionString"/>.</returns>
        public Dictionary<string, string> GetConnectionStringParts()
        {
            Dictionary<string, string> output = null;
            if (!string.IsNullOrWhiteSpace(ConnectionString)) {
                output = ConnectionString.Split(';').Select(s => s.Split('=')).ToDictionary(k => k.ElementAt(0), v => v.ElementAt(1));
            }

            return output;
        }

        public override string ToString()
        {
            return string.Format("{0}\t{1}\t{2}", WorkspaceFactory, ConnectionString != null ? ConnectionString : null, DataSet);
        }
    }
}
