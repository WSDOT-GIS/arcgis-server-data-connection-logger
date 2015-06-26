using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Wsdot.ArcGis.Server.Reporting.DataContracts
{
    /// <summary>
    /// Represents a data connection.
    /// </summary>
    public class DataConnectionInfo: IComparable, IComparable<DataConnectionInfo>, IEquatable<DataConnectionInfo>
    {
        /// <summary>
        /// Connection string
        /// </summary>
        public ConnectionInfo ConnectionInfo { get; set; }
        /// <summary>
        /// Dataset that is the source for the map service layer.
        /// </summary>
        public string DataSet { get; set; }
        /// <summary>
        /// Name of the map service layer.
        /// </summary>
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

        /// <summary>
        /// Compares the <see cref="DataConnectionInfo"/> to another <see cref="object"/>.
        /// </summary>
        /// <param name="obj">An object.</param>
        /// <returns><see cref="IComparable.CompareTo(object)"/></returns>
        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }
            else
            {
                var objType = obj.GetType();
                if (typeof(DataConnectionInfo).IsAssignableFrom(objType))
                {
                    var other = (DataConnectionInfo)obj;
                    return this.CompareTo(other);
                }
                else if (typeof(IComparable).IsAssignableFrom(objType))
                {
                    var other = (IComparable)obj;
                    return other.CompareTo(this);
                }
                else
                {
                    return 1;
                }
            }
        }

        /// <summary>
        /// Compares the current <see cref="DataConnectionInfo"/> with another <see cref="DataConnectionInfo"/>.
        /// </summary>
        /// <param name="other">Another <see cref="DataConnectionInfo"/></param>
        /// <returns>0 if they are equal, 1 if this one's greater, -1 if the other's greater.</returns>
        public int CompareTo(DataConnectionInfo other)
        {
            int output = 0;
            // Compare connection info
            if (this.ConnectionInfo != null)
            {
                output += this.ConnectionInfo.CompareTo(other.ConnectionInfo);
            }
            else if (other.ConnectionInfo != null)
            {
                output -= other.ConnectionInfo.CompareTo(this.ConnectionInfo);
            }

            // Compare dataset
            if (this.DataSet != null && other.DataSet != null)
            {
                output += string.Compare(this.DataSet, other.DataSet, StringComparison.InvariantCultureIgnoreCase);
            }
            else
            {
                output += this.DataSet == null ? -1 : 1;
            }

            // Compare layer name
            if (this.LayerName != null && other.LayerName != null)
            {
                output += string.Compare(this.LayerName, other.LayerName, StringComparison.InvariantCultureIgnoreCase);
            }
            else
            {
                output += this.LayerName == null ? -1 : 1;
            }

            return output;
        }

        /// <summary>
        /// Determines if this <see cref="DataConnectionInfo"/> is equal to another.
        /// </summary>
        /// <param name="other">Another <see cref="DataConnectionInfo"/></param>
        /// <returns>Returns <see langword="true"/> if they are equal, <see langword="false"/> otherwise.</returns>
        public bool Equals(DataConnectionInfo other)
        {
            return this.CompareTo(other) == 0;
        }

        /// <summary>
        /// Determines if this <see cref="DataConnectionInfo"/> is equal to another <see cref="object"/>.
        /// </summary>
        /// <param name="obj">Another object</param>
        /// <returns>Returns <see langword="true"/> if they are equal, <see langword="false"/> otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (obj != null && typeof(DataConnectionInfo).IsAssignableFrom(obj.GetType())) {
                return this.Equals((DataConnectionInfo)obj);
            }
            return base.Equals(obj);
        }

        /// <summary>
        /// Gets a hash code for this object.
        /// </summary>
        /// <returns>An integer.</returns>
        public override int GetHashCode()
        {
            return (this.ConnectionInfo != null ? this.ConnectionInfo.GetHashCode() : 0)
                ^ (this.DataSet != null ? this.DataSet.GetHashCode() : 0)
                ^ (this.LayerName != null ? this.LayerName.GetHashCode() : 0);
        }

        /// <summary>
        /// Determines if two <see cref="DataConnectionInfo"/> values are equivalent.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>Returns <see langword="true"/> if they are equal, <see langword="false"/> otherwise.</returns>
        public static bool operator == (DataConnectionInfo a, DataConnectionInfo b) 
        {
            bool aIsNull = object.Equals(a, null), bIsNull = object.Equals(b, null);
            if (aIsNull && bIsNull)
            {
                return true;
            }
            else if (aIsNull || bIsNull)
            {
                return false;
            }
            else
            {
                return a.CompareTo(b) == 0;
            }
        }


        /// <summary>
        /// Determines if two <see cref="DataConnectionInfo"/> values are not equivalent.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>Returns <see langword="false"/> if they are equal, <see langword="true"/> otherwise.</returns>
        public static bool operator !=(DataConnectionInfo a, DataConnectionInfo b)
        {
            return !(a == b);
        }
    }
}
