using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AppWebSantaBeatriz
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
          name: "Auto",
          url: "ServicioAutomotriz",
          defaults: new { controller = "ServicioAutomotriz", action = "Home", id = UrlParameter.Optional }
          );
            routes.MapRoute(
             name: "Servicios",
             url: "ServiciosIndustriales",
             defaults: new { controller = "ServiciosIndustriales", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
             name: "Electricidad",
             url: "Electricidad",
             defaults: new { controller = "ServiciosIndustriales", action = "Electricidad", id = UrlParameter.Optional }
            );
            routes.MapRoute(
            name: "Mecanica",
            url: "Mecanica",
            defaults: new { controller = "ServiciosIndustriales", action = "Mecanica", id = UrlParameter.Optional }
           );
            routes.MapRoute(
            name: "Estructuras",
            url: "Estructuras",
            defaults: new { controller = "ServiciosIndustriales", action = "Estructuras", id = UrlParameter.Optional }
           );
            routes.MapRoute(
            name: "Hidraulica",
            url: "Hidraulica",
            defaults: new { controller = "ServiciosIndustriales", action = "Hidraulica", id = UrlParameter.Optional }
           );
            routes.MapRoute(
             name: "Admin Taller",
             url: "Auto",
             defaults: new { controller = "AdminServAuto", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
              name: "Administración",
              url: "Admin",
              defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
          );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "ServicioAutomotriz", action = "Home", id = UrlParameter.Optional }
            );
         
        }
    }
}
