using System.Web.Mvc;
using System.Web.Routing;

namespace Doe.Ls.ProjectTemplate.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Test",
                url: "Test/{action}/{id}",
                defaults: new { controller = "Test", action = "Help", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Pages",
                url: "Page/{id}",
                defaults: new { controller = "AppPage", action = "Page" }
            );

            Associations(routes);

            routes.MapRoute(
                name: "PagesByName",
                url: "PageByName",
                defaults: new { controller = "AppPage", action = "PageByName" }
            );

            routes.MapRoute(
              name: "Assets",
              url: "File/{id}",
              defaults: new { controller = "Asset", action = "File" }
          );
            routes.MapRoute(
            name: "Thumbnails",
            url: "Thumbnail/{id}",
            defaults: new { controller = "Asset", action = "Thumbnail" }
        );
            routes.MapRoute(
                name: "Events",
                url: "Events/{action}/{id}",
                defaults: new { controller = "EventHeader", action= "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        private static void Associations(RouteCollection routes)
        {
            routes.MapRoute(
                name: "Hunter",
                url: "Hunter",
                defaults: new {controller = "Association", action = "AssociationLandingPage", id = 2}
                );
            routes.MapRoute(
                name: "NorthCoast",
                url: "NorthCoast",
                defaults: new { controller = "Association", action = "AssociationLandingPage", id = 3 }
                );
            routes.MapRoute(
                name: "NorthWest",
                url: "NorthWest",
                defaults: new { controller = "Association", action = "AssociationLandingPage", id = 4 }
                );
            routes.MapRoute(
                name: "Riverina",
                url: "Riverina",
                defaults: new { controller = "Association", action = "AssociationLandingPage", id = 5 }
                );
            routes.MapRoute(
                name: "SouthCoast",
                url: "SouthCoast",
                defaults: new { controller = "Association", action = "AssociationLandingPage", id = 6 }
                );
            routes.MapRoute(
                name: "SydneyEast",
                url: "SydneyEast",
                defaults: new { controller = "Association", action = "AssociationLandingPage", id = 7 }
                );
            routes.MapRoute(
                name: "SydneyNorth",
                url: "SydneyNorth",
                defaults: new { controller = "Association", action = "AssociationLandingPage", id = 8 }
                );
            routes.MapRoute(
                name: "SydneySouthWest",
                url: "SydneySouthWest",
                defaults: new { controller = "Association", action = "AssociationLandingPage", id = 9 }
                );
            routes.MapRoute(
                name: "SydneyWest",
                url: "SydneyWest",
                defaults: new { controller = "Association", action = "AssociationLandingPage", id = 10 }
                );
            routes.MapRoute(
                name: "Western",
                url: "Western",
                defaults: new { controller = "Association", action = "AssociationLandingPage", id = 11 }
                );
            routes.MapRoute(
                name: "NSWPSSA",
                url: "NSWPSSA",
                defaults: new { controller = "Association", action = "AssociationLandingPage", id = 14 }
                );
            routes.MapRoute(
                name: "NSWCHSSA",
                url: "NSWCHSSA",
                defaults: new { controller = "Association", action = "AssociationLandingPage", id = 13 }
                );
        }
    }
}