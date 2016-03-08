using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using GasPrice.Api.Filters;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Owin;

namespace GasPrice.Api
{
    public partial class Startup
    {

        private static void ConfigWebApi(IAppBuilder app)
        {
            var config = GlobalConfiguration.Configuration;

            // Enables us to call the Web API from domains other than the ones the API responds to
            RegisterRoutes(config);
            RegisterFormatters(config);
            RegisterAuth(config);

            app.UseWebApi(config);
            config.EnsureInitialized();
        }

        private static void RegisterRoutes(HttpConfiguration config)
        {

            config.MapHttpAttributeRoutes(new CustomDirectRouteProvider());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional}
                );
        }

        private static void RegisterFormatters(HttpConfiguration config)
        {
            // Remove default XML handler
            var matches = config.Formatters
                .Where(
                    f =>
                        f.SupportedMediaTypes.Any(
                            m => m.MediaType.ToString(CultureInfo.InvariantCulture) == "application/xml" ||
                                 m.MediaType.ToString(CultureInfo.InvariantCulture) == "text/xml")).ToList();

            foreach (var match in matches) { 
                config.Formatters.Remove(match);
            }
            // Need to disable validation since it currently fails on DbGeography
            //config.Services.Clear(typeof (ModelValidatorProvider));
            //config.Filters.Add(new ModelStateValidatorAttribute());
            // Create Json.Net formatter serializing DateTime using the ISO 8601 format
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateParseHandling = DateParseHandling.DateTime,
                DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None,
                Converters = new List<JsonConverter>
                {
                    new IsoDateTimeConverter() // { DateTimeFormat = "yyyy-MM-dd HH:mm:ss"},
                    // If need to auto-convert enum porperties to it's string value
                    //new StringEnumConverter()
                }
            };

            config.Formatters.JsonFormatter.SerializerSettings = settings;
        }

        /// <summary>
        /// Registers the web API authentication.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public static void RegisterAuth(HttpConfiguration config)
        {
            //All routes forbidden by default
            config.Filters.Add(new AuthorizeAttribute());
            config.Filters.Add(new ExceptionAttribute());

            //Use only bearer token authentication
            //SuppressDefaultHostAuthentication will register a message handler and set the current principal
            //to anonymous, so no host principal will get passed to Web API. It also suppress default challenges
            //from OWIN middleware
            config.SuppressDefaultHostAuthentication();

            //HostAuthenticationFilter  behaviour  is te opposite. It will set the principal from specified OWIN authentication middleware
            //In this application, it is the Bearer token middleware, it will also send a challenge to specified middleware when it sees a 401 response.
            //Since this authentication filter is set as global filter, it will apply to all Web APIs so the result is Web API only sees the 
            //authentication principal from the bearer token middleware and any 401 response will add a bearer challenge.
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
           
        }
    }

    public class CustomDirectRouteProvider : DefaultDirectRouteProvider
    {
        protected override IReadOnlyList<IDirectRouteFactory>
        GetActionRouteFactories(HttpActionDescriptor actionDescriptor)
        {
            return actionDescriptor.GetCustomAttributes<IDirectRouteFactory>(true);
        }
    }
}

