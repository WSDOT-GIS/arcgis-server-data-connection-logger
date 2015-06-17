using System.Collections.Generic;

namespace DataContracts
{
    public class MsdInfo
    {
        public string Path { get; set; }
        public IEnumerable<DataConnectionInfo> Connections { get; set; }
    }
}
