using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcSigesoft
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               "Sigesoft", "Sigesoft/",
               new { controller = "Generals", action = "Index" },
               new[] { "SigesoftWebUI.Controllers" }
           );

            routes.MapRoute(
                "PruebaRapidaCovid19", "PruebaRapidaCovid19/",
                new { controller = "PruebaRapidaCovid19", action = "Index" },
                new[] { "SigesoftWebUI.Controllers" }
            );


            routes.MapRoute(
              "Covid19", "Sigesoft/",
              new { controller = "Covid19", action = "Index" },
              new[] { "SigesoftWebUI.Controllers" }
          );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "AccessSystem", action = "Login", id = UrlParameter.Optional }
            );
        }
    }
}