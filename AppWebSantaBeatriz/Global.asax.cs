using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AppWebSantaBeatriz
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }
        protected void Application_BeginRequest()
        {
            if (!Context.Request.IsSecureConnection
                && !Context.Request.IsLocal // to avoid switching to https when local testing
                )
            {
                // Only insert an "s" to the "http:", and avoid replacing wrongly http: in the url parameters
                Response.Redirect(Context.Request.Url.ToString().Insert(4, "s"));
            }
        }
    }
}
