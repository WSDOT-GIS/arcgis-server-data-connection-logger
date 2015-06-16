using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    public class ConnectionString
    {

        public string Server { get; set; }

        public string Instance { get; set; }

        public string Database { get; set; }

        public string Version { get; set; }

        public string AuthenticationMode { get; set; }

        public string DBClient { get; set; }

        public string ServerInstance { get; set; }

        public string User { get; set; }

        public string DBConnectionProperties { get; set; }

        private string _originalString;

        public ConnectionString()
        {

        }

        public ConnectionString(string connectionString)
        {
            this._originalString = connectionString;
            var dict = GetConnectionStringParts(connectionString);

            this.Server = dict.Keys.Contains("SERVER") ? dict["SERVER"] : null;
            this.Instance = dict.Keys.Contains("INSTANCE") ? dict["INSTANCE"] : null;
            this.Database = dict.Keys.Contains("DATABASE") ? dict["DATABASE"] : null;
            this.Version = dict.Keys.Contains("VERSION") ? dict["VERSION"] : null;
            this.AuthenticationMode = dict.Keys.Contains("AUTHENTICATION_MODE") ? dict["AUTHENTICATION_MODE"] : null;
            this.DBClient = dict.Keys.Contains("DBCLIENT") ? dict["DBCLIENT"] : null;
            this.ServerInstance = dict.Keys.Contains("SERVERINSTANCE") ? dict["SERVERINSTANCE"] : null;
            this.User = dict.Keys.Contains("USER") ? dict["USER"] : null;
            this.DBConnectionProperties = dict.Keys.Contains("DB_CONNECTION_PROPERTIES") ? dict["DB_CONNECTION_PROPERTIES"] : null;

        }

        /// <summary>
        /// Splits the parts of a connection string into a <see cref="Dictionary&lt;K,V&gt;"/>
        /// </summary>
        /// <returns>A dictionary of connection string parameters, or null if there is no <see cref="DataConnectionInfo.ConnectionString"/>.</returns>
        public static Dictionary<string, string> GetConnectionStringParts(string connectionString)
        {
            Dictionary<string, string> output = null;
            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                output = connectionString.Split(';').Select(s => s.Split('=')).ToDictionary(k => k.ElementAt(0), v => v.ElementAt(1));
            }

            return output;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj.GetType() == typeof(ConnectionString))
            {
                var other = (ConnectionString)obj;
                return 
                    this.AuthenticationMode == other.AuthenticationMode &&
                    this.Database == other.Database &&
                    this.DBClient == other.DBClient &&
                    this.DBConnectionProperties == other.DBConnectionProperties &&
                    this.Instance == other.Instance &&
                    this.Server == other.Server &&
                    this.ServerInstance == other.ServerInstance &&
                    this.User == other.User &&
                    this.Version == other.Version;
                
            }
            return base.Equals(obj);
        }

        public static bool operator ==(ConnectionString csA, ConnectionString csB)
        {
            return csA.Equals(csB);
        }

        public static bool operator !=(ConnectionString csA, ConnectionString csB)
        {
            return !csA.Equals(csB);
        }

        public override string ToString()
        {
            return this._originalString;
        }

        public override int GetHashCode()
        {
            int hashCode = 0;
            if (this.AuthenticationMode != null)
            {
                hashCode += this.AuthenticationMode.GetHashCode();
            }

            if (this.Database != null)
            {
                hashCode += this.Database.GetHashCode();
            }

            if (this.DBClient != null)
            {
                hashCode += this.DBClient.GetHashCode();
            }

            if (this.DBConnectionProperties != null)
            {
                hashCode += this.DBConnectionProperties.GetHashCode();
            }

            if (this.Instance != null)
            {
                hashCode += this.Instance.GetHashCode();
            }

            if (this.Server != null)
            {
                hashCode += this.Server.GetHashCode();
            }

            if (this.ServerInstance != null)
            {
                hashCode += this.ServerInstance.GetHashCode();
            }

            if (this.User != null)
            {
                hashCode += this.User.GetHashCode();
            }

            if (this.Version != null)
            {
                hashCode += this.Version.GetHashCode();
            }

            return hashCode;
        }


    }
}
