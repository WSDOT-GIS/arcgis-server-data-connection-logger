using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace DataContracts
{
    /// <summary>
    /// Represents a data connection.
    /// </summary>
    public class ConnectionInfo
    {
        /// <summary>
        /// Connection string
        /// </summary>
        public ConnectionString ConnectionString { get; set; }

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
        /// Creates a new instance of this class.
        /// </summary>
        public ConnectionInfo()
        {

        }

        public override bool Equals(object obj)
        {
            if (obj != null && typeof(ConnectionInfo).IsInstanceOfType(obj))
            {
                var other = (ConnectionInfo)obj;
                return this.ConnectionString == other.ConnectionString && this.WorkspaceFactory == other.WorkspaceFactory;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return (this.WorkspaceFactory != null ? this.WorkspaceFactory.GetHashCode() : 0) ^ (this.ConnectionString != null ? this.ConnectionString.GetHashCode() : 0);
        }
    }
}
