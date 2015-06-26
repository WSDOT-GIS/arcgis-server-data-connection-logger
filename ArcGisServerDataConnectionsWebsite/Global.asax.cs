#pragma warning disable 1591
using System.Web.Http;

namespace Wsdot.ArcGis.Server.Reporting
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}

#pragma warning restore 1591
