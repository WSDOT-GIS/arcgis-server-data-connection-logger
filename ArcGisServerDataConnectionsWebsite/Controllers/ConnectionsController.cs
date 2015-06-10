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

namespace ArcGisServerDataConnectionsWebsite.Controllers
{
    public class ConnectionsController : ApiController
    {
        [Route("api/connections")]
        public IEnumerable<IEnumerable<DataConnectionInfo>> GetConnections()
        {
            var dirListString = ConfigurationManager.AppSettings["directories"];
            var dirs = from s in dirListString.Split(';') 
                       select new DirectoryInfo(s);

            var datasetInfos = dirs.GetDatasetInfos();
            return datasetInfos.SelectMany(s => s);

            //ArcGisConnection.DatasetInfoCollector.GetDatasetInfos()
        }
    }
}
