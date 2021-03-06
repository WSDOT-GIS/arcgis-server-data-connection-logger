﻿using System;

namespace Wsdot.ArcGis.Server.Reporting.DataContracts
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

        /// <summary>
        /// Determines if this object is equal to another.
        /// </summary>
        /// <param name="obj">Another object.</param>
        /// <returns>Returns <see langword="true"/> if they are equal, <see langword="false"/> otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (obj != null && typeof(ConnectionInfo).IsInstanceOfType(obj))
            {
                var other = (ConnectionInfo)obj;
                return this.ConnectionString == other.ConnectionString && this.WorkspaceFactory == other.WorkspaceFactory;
            }
            return base.Equals(obj);
        }

        /// <summary>
        /// Gets a hash code for this object.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (this.WorkspaceFactory != null ? this.WorkspaceFactory.GetHashCode() : 0) ^ (this.ConnectionString != null ? this.ConnectionString.GetHashCode() : 0);
        }

        /// <summary>
        /// Compares this <see cref="ConnectionInfo"/> to another object.
        /// </summary>
        /// <param name="obj">An object.</param>
        /// <returns>Returns an integer for sorting.</returns>
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

        /// <summary>
        /// Compares this <see cref="ConnectionInfo"/> to another.
        /// </summary>
        /// <param name="other">A <see cref="ConnectionInfo"/></param>
        /// <returns>An integer for sorting.</returns>
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
