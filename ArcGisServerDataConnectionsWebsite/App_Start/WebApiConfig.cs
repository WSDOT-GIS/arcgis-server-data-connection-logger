#pragma warning disable 1591
using Microsoft.AspNet.WebApi.MessageHandlers.Compression;
using Microsoft.AspNet.WebApi.MessageHandlers.Compression.Compressors;
using System.Web.Http;
using Wsdot.ArcGis.Server.Reporting.Formatters;

namespace Wsdot.ArcGis.Server.Reporting
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();
            // Add CSV output for FlattenedItem enumerations.
            config.Formatters.Insert(0, new FlattenedItemCsvFormatter());
            // Add ability to compress output.
            GlobalConfiguration.Configuration.MessageHandlers.Insert(0, new ServerCompressionHandler(new GZipCompressor(), new DeflateCompressor()));
        }
    }
}

#pragma warning restore 1591
