using ArcGisConnection;
using DataContracts;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Http;
using WebApi.OutputCache.V2;

namespace ArcGisServerDataConnectionsWebsite.Controllers
{
    public class ConnectionsController : ApiController
    {
        [Route("api/connections")]
        [CacheOutput(ServerTimeSpan=24*60*60)]
        public IEnumerable<ServerInfo> GetConnections()
        {
            var dirListString = ConfigurationManager.AppSettings["directories"];
            var dirs = from s in dirListString.Split(';') 
                       select new DirectoryInfo(s);

            var output = dirs.GetServerInfos();

            return output;
        }

        [Route("api/flattened")]
        [CacheOutput(ServerTimeSpan = 24 * 60 * 60)]
        public IEnumerable<FlattenedItem> GetFlattenedMsdData()
        {
            var dirListString = ConfigurationManager.AppSettings["directories"];
            var dirs = from s in dirListString.Split(';') 
                       select new DirectoryInfo(s);
            return dirs.GetFlattenedOutput();
        }
    }
}