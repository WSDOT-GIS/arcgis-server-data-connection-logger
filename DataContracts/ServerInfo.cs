using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    public class ServerInfo
    {
        public string Directory { get; set; }
        public IEnumerable<MsdInfo> MsdInfos { get; set; }
    }
}
