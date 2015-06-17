using System;

namespace DataContracts
{
    /// <summary>
    /// Represents a data connection.
    /// </summary>
    public class ConnectionInfo: IComparable, IComparable<ConnectionInfo>
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

        public int CompareTo(object obj)
        {
            if (typeof(ConnectionInfo).IsInstanceOfType(obj))
            {
                var other = (ConnectionInfo)obj;
                return this.CompareTo(other);
            }
            else if (typeof(IComparable).IsInstanceOfType(obj))
            {
                var other = (IComparable)obj;
                return this.CompareTo(other);
            }
            else
            {
                throw new NotImplementedException("The other object does not implement IComparable.");
            }
        }

        public int CompareTo(ConnectionInfo other)
        {
            if (other == null)
            {
                return 1;
            }

            int output = 0;
            
            if (this.ConnectionString != null && other.ConnectionString != null)
            {
                output += this.ConnectionString.CompareTo(other.ConnectionString);
            }
            else if (this.ConnectionString == null)
            {
                output -= 1;
            }
            else
            {
                output += 1;
            }

            if (this.WorkspaceFactory != null && other.ConnectionString != null)
            {
                output += string.Compare(this.WorkspaceFactory, other.WorkspaceFactory);
            }
            else if (this.WorkspaceFactory == null)
            {
                output -= 1;
            }
            else
            {
                output += 1;
            }

            return output;
        }
    }
}
