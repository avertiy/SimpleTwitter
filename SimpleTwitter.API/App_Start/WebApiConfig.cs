using System;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Web.OData.Routing;
using Microsoft.OData.Edm;
using Newtonsoft.Json;
using SimpleTwitter.API.Models;

namespace SimpleTwitter.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            ODataRoute route = config.MapODataServiceRoute("ODataRoute", "odata", GetEdmModel());
            
            config.Formatters.Add(new BrowserJsonFormatter());
            
            // Web API configuration and services
            //config.MapODataServiceRoute("odata", null, GetEdmModel());//, new DefaultODataBatchHandler(GlobalConfiguration.DefaultServer)
            config.EnsureInitialized();
        }
        private static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder
            {
                Namespace = "SimpleTwitter.API.OData",
                ContainerName = "DefaultContainer"
            };
            builder.EntitySet<UserModel>("twitterapi").EntityType.HasKey(u => u.UserName);

            var edmModel = builder.GetEdmModel();

            return edmModel;
        }
    }
    public class BrowserJsonFormatter : JsonMediaTypeFormatter
    {
        public BrowserJsonFormatter()
        {
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            this.SerializerSettings.Formatting = Formatting.Indented;
        }

        public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
        {
            base.SetDefaultContentHeaders(type, headers, mediaType);
            headers.ContentType = new MediaTypeHeaderValue("application/json");
        }
    }
}
