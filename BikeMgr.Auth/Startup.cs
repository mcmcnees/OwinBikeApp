using BikeMgrAuth.IdentityServer;
using IdentityServer3.Core;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Logging;
using Microsoft.Owin;
using Owin;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;
using System.Web.Helpers;

[assembly: OwinStartup(typeof(BikeMgrAuth.Startup))]
namespace BikeMgrAuth
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .WriteTo.Trace()
               .CreateLogger();

            AntiForgeryConfig.UniqueClaimTypeIdentifier = Constants.ClaimTypes.Subject;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap = new Dictionary<string, string>();

            app.Map("", idsrvApp =>
            {
                var idSvrFactory = Factory.Configure();
                idSvrFactory.ConfigureUserService("AspID");

                var options = new IdentityServerOptions
                {
                    SiteName = "Bike Manager",
                    SigningCertificate = LoadCertificate(),
                    Factory = idSvrFactory,
                    RequireSsl = true
                    //,AuthenticationOptions = new AuthenticationOptions { EnablePostSignOutAutoRedirect = true }
                };

                idsrvApp.UseIdentityServer(options);
            });


        }

        X509Certificate2 LoadCertificate()
        {
            return new X509Certificate2(
                string.Format(@"{0}\bin\identityServer\localIDCert.pfx", AppDomain.CurrentDomain.BaseDirectory), ConfigurationManager.AppSettings["CertPassword"]);
        }
    }
}