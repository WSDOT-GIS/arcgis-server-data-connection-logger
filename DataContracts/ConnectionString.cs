using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Wsdot.ArcGis.Server.Reporting.DataContracts
{
    /// <summary>
    /// Represents a database connection string.
    /// </summary>
    [DataContract]
    public class ConnectionString: IComparable, IComparable<ConnectionString>
    {
        /// <summary>
        /// Server
        /// </summary>
        [DataMember(IsRequired=false, EmitDefaultValue=false)]
        public string Server { get; set; }

        /// <summary>
        /// Instance
        /// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string Instance { get; set; }

        /// <summary>
        /// Database
        /// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string Database { get; set; }

        /// <summary>
        /// Version
        /// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string Version { get; set; }

        /// <summary>
        /// Authentication Mode.
        /// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string AuthenticationMode { get; set; }

        /// <summary>
        /// Database Client.
        /// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string DBClient { get; set; }

        /// <summary>
        /// Server Instance.
        /// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string ServerInstance { get; set; }

        /// <summary>
        /// User
        /// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string User { get; set; }

        /// <summary>
        /// DB Connection Properties
        /// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string DBConnectionProperties { get; set; }

        private string _originalString;

        /// <summary>
        /// Creates a new <see cref="ConnectionString"/>
        /// </summary>
        public ConnectionString()
        {

        }

        /// <summary>
        /// Creates a new <see cref="ConnectionString"/>, parsed from an input connection string.
        /// </summary>
        /// <param name="connectionString">A connection string.</param>
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
        /// <returns>A dictionary of connection string parameters, or null if there is no connection string.</returns>
        public static Dictionary<string, string> GetConnectionStringParts(string connectionString)
        {
            Dictionary<string, string> output = null;
            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                output = connectionString.Split(';').Select(s => s.Split('=')).ToDictionary(k => k.ElementAt(0), v => v.ElementAt(1));
            }

            return output;
        }

        /// <summary>
        /// Determines if this object is equal to another.
        /// </summary>
        /// <param name="obj">Another object.</param>
        /// <returns>Returns <see langword="true"/> if they are equal, <see langword="false"/> otherwise.</returns>
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

        /// <summary>
        /// Determines if two <see cref="ConnectionString"/> values are equal.
        /// </summary>
        /// <param name="csA"></param>
        /// <param name="csB"></param>
        /// <returns>Returns <see langword="true"/> if they are equal, <see langword="false"/> otherwise.</returns>
        public static bool operator ==(ConnectionString csA, ConnectionString csB)
        {
            object 
                objA = (object)csA, 
                objB = (object)csB;

            if (objA == null && objB == null)
            {
                return true;
            }
            else if (objA != null && objB != null)
            {
                return csA.Equals(csB);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        ///  Determines if two <see cref="ConnectionString"/> values are not equal.
        /// </summary>
        /// <param name="csA"></param>
        /// <param name="csB"></param>
        /// <returns>Returns <see langword="false"/> if they are equal, <see langword="true"/> otherwise.</returns>
        public static bool operator !=(ConnectionString csA, ConnectionString csB)
        {
            return !(csA == csB);
        }

        /// <summary>
        /// Returns the original connection string <see cref="string"/> that this object represents.
        /// </summary>
        /// <returns>The original connection string.</returns>
        public override string ToString()
        {
            return this._originalString;
        }

        /// <summary>
        /// Returns a hash code for this object.
        /// </summary>
        /// <returns>An integer value.</returns>
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


        /// <summary>
        /// Compares this object to another.
        /// </summary>
        /// <param name="obj">Another object.</param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if (obj == null) {
                return 1;
            }
            else if (typeof(ConnectionString).IsInstanceOfType(obj))
            {
                var other = (ConnectionString)obj;
                return this.CompareTo(other);

            }
            else if (typeof(IComparable).IsInstanceOfType(obj))
            {
                return ((IComparable)obj).CompareTo(this);
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// Compares this <see cref="ConnectionString"/> to another.
        /// </summary>
        /// <param name="other">Another <see cref="ConnectionString"/></param>
        /// <returns>An integer used for sorting.</returns>
        public int CompareTo(ConnectionString other)
        {
            if (other == null)
            {
                return 1;
            }
            int output = 0;
            output += string.Compare(this.Server, other.Server);
            output += string.Compare(this.Instance, other.Instance);
            output += string.Compare(this.Database, other.Database);
            output += string.Compare(this.Version, other.Version);
            output += string.Compare(this.AuthenticationMode, other.AuthenticationMode);
            output += string.Compare(this.DBClient, other.DBClient);
            output += string.Compare(this.ServerInstance, other.ServerInstance);
            output += string.Compare(this.User, other.User);
            output += string.Compare(this.DBConnectionProperties, other.DBConnectionProperties);
            return output;
        }
    }
}
