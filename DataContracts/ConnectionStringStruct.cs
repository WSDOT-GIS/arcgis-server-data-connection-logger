using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    [DataContract]
    public class ConnectionString
    {
        private string _server;
        private string _instance;
        private string _database;
        private string _version;
        private string _authenticationMode;
        private string _dbClient;
        private string _serverInstance;
        private string _user;
        private string _dbConnectionProperties;
        private string _originalString;

        [DataMember]
        public string Server
        {
            get { return _server; }
        }

        [DataMember]
        public string Instance
        {
            get { return _instance; }
        }

        [DataMember]
        public string Database
        {
            get { return _database; }
        }

        [DataMember]
        public string Version
        {
            get { return _version; }
        }

        [DataMember]
        public string AuthenticationMode
        {
            get { return _authenticationMode; }
        }

        [DataMember]
        public string DBClient
        {
            get { return _dbClient; }
        }

        [DataMember]
        public string ServerInstance
        {
            get { return _serverInstance; }
        }

        [DataMember]
        public string User
        {
            get { return _user; }
        }

        [DataMember]
        public string DBConnectionProperties
        {
            get { return _dbConnectionProperties; }
        }


        public ConnectionString(string connectionString)
        {
            this._originalString = connectionString;
            var dict = GetConnectionStringParts(connectionString);

            this._server = dict.Keys.Contains("SERVER") ? dict["SERVER"] : null;
            this._instance = dict.Keys.Contains("INSTANCE") ? dict["INSTANCE"] : null;
            this._database = dict.Keys.Contains("DATABASE") ? dict["DATABASE"] : null;
            this._version = dict.Keys.Contains("VERSION") ? dict["VERSION"] : null;
            this._authenticationMode = dict.Keys.Contains("AUTHENTICATION_MODE") ? dict["AUTHENTICATION_MODE"] : null;
            this._dbClient = dict.Keys.Contains("DBCLIENT") ? dict["DBCLIENT"] : null;
            this._serverInstance = dict.Keys.Contains("SERVERINSTANCE") ? dict["SERVERINSTANCE"] : null;
            this._user = dict.Keys.Contains("USER") ? dict["USER"] : null;
            this._dbConnectionProperties = dict.Keys.Contains("DB_CONNECTION_PROPERTIES") ? dict["DB_CONNECTION_PROPERTIES"] : null;

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
