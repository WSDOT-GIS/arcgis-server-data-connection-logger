using ArcGisConnection;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Web.Http;
using WebApi.OutputCache.V2;

namespace ArcGisServerDataConnectionsWebsite.Controllers
{
    /// <summary>
    /// Controller for returning ArcGIS Server map service connection information.
    /// </summary>
    public class ConnectionsController : ApiController
    {
        /// <summary>
        /// Lists servers, map services, and associated data connections.
        /// </summary>
        /// <returns>An enumeration of <see cref="ServerInfo"/>.</returns>
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

        /// <summary>
        /// Returns information about map services on ArcGIS Servers in a flattened, tabular data format.
        /// </summary>
        /// <returns>A table of information about ArcGIS Server map services.</returns>
        [Route("api/flattened")]
        [CacheOutput(ServerTimeSpan = 24 * 60 * 60)]
        public HttpResponseMessage GetFlattenedMsdData()
        {
            var dirListString = ConfigurationManager.AppSettings["directories"];
            var dirs = from s in dirListString.Split(';') 
                       select new DirectoryInfo(s);
            var output = dirs.GetFlattenedOutput();

            var responseContent = new ObjectContent<IEnumerable<FlattenedItem>>(output, new Formatters.FlattenedItemCsvFormatter(), "text/csv");


            var response = new HttpResponseMessage() {
                Content = responseContent,
            };

            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName="MapServiceData.csv"
            };

            


            return response;
        }
    }
}