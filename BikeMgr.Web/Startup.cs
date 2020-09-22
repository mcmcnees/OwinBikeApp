using IdentityModel.Client;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BikeMgrWeb
{
    public class Startup
    {
        private static string ClientUri = ConfigurationManager.AppSettings["BikeWeb.Uri"];
        private static string IdServBaseUri = ConfigurationManager.AppSettings["BikeAuth.Uri"];
        private static string UserInfoEndpoint = IdServBaseUri + @"/connect/userinfo";
        private static string TokenEndpoint = IdServBaseUri + @"/connect/token";

        public void Configuration(IAppBuilder app)
        {
            app.UseKentorOwinCookieSaver();

            AntiForgeryConfig.UniqueClaimTypeIdentifier = "sub";
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            app.UseResourceAuthorization(new AuthorizationManager());

            app.UseCookieAuthentication(new CookieAuthenticationOptions { AuthenticationType = "Cookies" });

            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = "bike_mgr_id",
                    Authority = IdServBaseUri,
                    RedirectUri = ClientUri + "/",
                    //PostLogoutRedirectUri = ClientUri + "/",
                    ResponseType = "code id_token token",
                    Scope = "openid profile email roles offline_access",
                    TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "name",
                        RoleClaimType = "role"
                    },
                    SignInAsAuthenticationType = "Cookies",
                    Notifications =
                        new OpenIdConnectAuthenticationNotifications
                        {
                            AuthorizationCodeReceived = async n =>
                            {
                                var userInfoClient = new UserInfoClient(new Uri(UserInfoEndpoint), n.ProtocolMessage.AccessToken);
                                var userInfoResponse = await userInfoClient.GetAsync();

                                var identity = new ClaimsIdentity(n.AuthenticationTicket.Identity.AuthenticationType, "given_name", "role");
                                identity.AddClaims(userInfoResponse.GetClaimsIdentity().Claims);

                                // Handle null custom claims
                                if (identity.Claims.FirstOrDefault(x => x.Type == "given_name") == null)
                                    identity.AddClaim(new Claim("given_name", ""));
                                if (identity.Claims.FirstOrDefault(x => x.Type == "family_name") == null)
                                    identity.AddClaim(new Claim("family_name", ""));

                                var tokenClient = new TokenClient(TokenEndpoint, "bike_mgr_id", "9461e50df!6bc4e9091362ad7$773_c8");
                                var response = await tokenClient.RequestAuthorizationCodeAsync(n.Code, n.RedirectUri);

                                identity.AddClaim(new Claim("access_token", response.AccessToken));
                                identity.AddClaim(new Claim("expires_at", DateTime.UtcNow.AddSeconds(response.ExpiresIn).ToLocalTime().ToString(CultureInfo.InvariantCulture)));
                                identity.AddClaim(new Claim("refresh_token", response.RefreshToken));
                                identity.AddClaim(new Claim("id_token", n.ProtocolMessage.IdToken));

                                n.AuthenticationTicket = new AuthenticationTicket(
                                    identity,
                                    n.AuthenticationTicket.Properties);
                            },
                            RedirectToIdentityProvider = n =>
                            {
                                if (n.ProtocolMessage.RequestType == OpenIdConnectRequestType.LogoutRequest)
                                {
                                    var idTokenHint = n.OwinContext.Authentication.User.FindFirst("id_token").Value;
                                    n.ProtocolMessage.IdTokenHint = idTokenHint;
                                }

                                return Task.FromResult(0);
                            }
                        }
                });
        }

    }
}