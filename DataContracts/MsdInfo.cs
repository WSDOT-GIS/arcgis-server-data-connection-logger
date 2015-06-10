using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    public class MsdInfo
    {
        public string Path { get; set; }
        public IEnumerable<DataConnectionInfo> Connections { get; set; }
    }
}
