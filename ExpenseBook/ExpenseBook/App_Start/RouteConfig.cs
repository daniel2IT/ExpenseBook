using System.Web.Http;
using System.Web.Mvc;

namespace ExpenseBook
{
    public class RouteConfig
    {
        public static void RegisterRoutes(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = UrlParameter.Optional }
            );
        }
    }
}
