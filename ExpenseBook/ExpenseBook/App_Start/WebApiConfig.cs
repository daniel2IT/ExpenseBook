using System.Web.Http;
using System.Web.Http.Cors;

namespace ExpenseBook
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configure NijectResolver
             config.DependencyResolver = new NinjectResolver();
            // Web API configuration and services
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));

            // Enable CORS for the Angular App
            var cors = new EnableCorsAttribute("http://localhost:4200", "*", "*");
            config.EnableCors(cors);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
