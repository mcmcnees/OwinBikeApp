using BikeMgr.API.Helpers;
using BikeMgr.API.Loggers;
using BikeMgrAPI.Filters;
using IdentityServer3.AccessTokenValidation;
using Microsoft.Owin;
using Owin;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

[assembly: OwinStartup(typeof(BikeMgr.API.Startup))]
namespace BikeMgr.API
{
    public class Startup
    {
        public static HttpConfiguration HttpConfiguration { get; set; } = new HttpConfiguration();

        public void Configuration(IAppBuilder app)
        {
            var config = Startup.HttpConfiguration;
            config.DependencyResolver = new UnityDependencyResolver(UnityHelpers.GetConfiguredContainer());
            ConfigureWebApi(app, config);
        }

        private static void ConfigureWebApi(IAppBuilder app, HttpConfiguration config)
        {
            config.Services.Add(typeof(IExceptionLogger), new UnhandledExceptionLogger());

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            ConfigureAuthorization(app);

            config.MapHttpAttributeRoutes();

            config.Filters.Add(new AppExceptionFilter());

            config.Routes.MapHttpRoute(
                name: "BikeMgr.API",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            app.UseWebApi(config);
        }

        private static void ConfigureAuthorization(IAppBuilder app)
        {
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = ConfigurationManager.AppSettings["TokenAuthority"],
                ClientId = "bike_mgr_id",
                ClientSecret = ConfigurationManager.AppSettings["ApiSecret"],
            });
        }
    }
}