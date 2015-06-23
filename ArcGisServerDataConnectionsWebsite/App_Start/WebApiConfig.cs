using ArcGisServerDataConnectionsWebsite.Formatters;
using Microsoft.AspNet.WebApi.MessageHandlers.Compression;
using Microsoft.AspNet.WebApi.MessageHandlers.Compression.Compressors;
using System.Web.Http;

namespace ArcGisServerDataConnectionsWebsite
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Formatters.Add(new FlattenedItemCsvFormatter());
            GlobalConfiguration.Configuration.MessageHandlers.Insert(0, new ServerCompressionHandler(new GZipCompressor(), new DeflateCompressor()));

            ////config.Routes.MapHttpRoute(
            ////    name: "DefaultApi",
            ////    routeTemplate: "api/{controller}/{id}",
            ////    defaults: new { id = RouteParameter.Optional }
            ////);
        }
    }
}
