using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ArcGisConnection;
using DataContracts;
using WebApi.OutputCache.V2;

namespace ArcGisServerDataConnectionsWebsite.Controllers
{
    public class ConnectionsController : ApiController
    {
        [Route("api/connections")]
        [CacheOutput(ServerTimeSpan=24*60*60)]
        public Dictionary<string,Dictionary<string,IEnumerable<DataContracts.DataConnectionInfo>>> GetConnections()
        {
            var dirListString = ConfigurationManager.AppSettings["directories"];
            var dirs = from s in dirListString.Split(';') 
                       select new DirectoryInfo(s);

            ////var datasetInfos = dirs.GetDatasetInfos();
            ////return datasetInfos.SelectMany(s => s);

            var output = (from d in dirs
                         select new { key = d.FullName, value = d.GetDatasetInfos().Where(k => k.Count() > 0).ToDictionary(k => k.First().Msd, v => v) }).ToDictionary(k => k.key, v => v.value);

            return output;

            //ArcGisConnection.DatasetInfoCollector.GetDatasetInfos()
        }
    }
}