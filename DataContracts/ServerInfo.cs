using System.Collections.Generic;

namespace DataContracts
{
    public class ServerInfo
    {
        public string Directory { get; set; }
        public IEnumerable<MsdInfo> MsdInfos { get; set; }
    }
}
