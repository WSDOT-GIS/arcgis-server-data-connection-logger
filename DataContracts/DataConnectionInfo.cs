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
        public string ConnectionString { get; set; }
        public string DataSet { get; set; }
        public string WorkspaceFactory { get; set; }
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
            const string replacement = "$1=[Omitted]";
            if (dataConnection != null)
            {
                var csElement = dataConnection.Element("WorkspaceConnectionString");
                var dsElement = dataConnection.Element("Dataset");
                var wsElement = dataConnection.Element("WorkspaceFactory");

                this.ConnectionString = csElement != null ? passwordRe.Replace(csElement.Value, replacement) : null;
                //this.ConnectionString = csElement != null ? csElement.Value : null;
                this.DataSet = dsElement != null ? dsElement.Value : null;
                this.WorkspaceFactory = wsElement != null ? wsElement.Value : null;
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
    }
}
