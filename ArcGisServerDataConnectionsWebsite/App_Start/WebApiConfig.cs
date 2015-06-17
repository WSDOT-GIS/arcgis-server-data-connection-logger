using ArcGisServerDataConnectionsWebsite.Formatters;
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

            ////config.Routes.MapHttpRoute(
            ////    name: "DefaultApi",
            ////    routeTemplate: "api/{controller}/{id}",
            ////    defaults: new { id = RouteParameter.Optional }
            ////);
        }
    }
}
