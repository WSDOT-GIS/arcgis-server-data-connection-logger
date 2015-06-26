using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace DataContracts
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

        public bool Equals(DataConnectionInfo other)
        {
            return this.CompareTo(other) == 0;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && typeof(DataConnectionInfo).IsAssignableFrom(obj.GetType())) {
                return this.Equals((DataConnectionInfo)obj);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return (this.ConnectionInfo != null ? this.ConnectionInfo.GetHashCode() : 0)
                ^ (this.DataSet != null ? this.DataSet.GetHashCode() : 0)
                ^ (this.LayerName != null ? this.LayerName.GetHashCode() : 0);
        }

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

        public static bool operator !=(DataConnectionInfo a, DataConnectionInfo b)
        {
            return !(a == b);
        }
    }
}
