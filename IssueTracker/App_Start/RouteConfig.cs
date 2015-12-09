using System.Web.Mvc;
using System.Web.Routing;

namespace IssueTracker
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Root",
                url: "",
                defaults: new { culture = Abstractions.CultureHelper.CurrentCulture, controller = "Home", action = "RedirectToIndex" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{culture}/{controller}/{action}/{id}",
                defaults: new { culture = Abstractions.CultureHelper.CurrentCulture, controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
