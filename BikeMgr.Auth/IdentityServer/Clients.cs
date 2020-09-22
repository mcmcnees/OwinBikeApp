using IdentityServer3.Core;
using IdentityServer3.Core.Models;
using System.Collections.Generic;
using System.Configuration;

namespace BikeMgrAuth.IdentityServer
{
    public class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new[]
            {
                new Client
                {
                    ClientName = "Bike Manager",
                    ClientId = "bike_mgr_id",
                    ClientSecrets = new List<Secret> {
                        new Secret(ConfigurationManager.AppSettings["WebSecret"].Sha256())
                    },
                    Enabled = true,
                    Flow = Flows.Hybrid,
                    RequireConsent = false,
                    //AllowRememberConsent = true,
                    RedirectUris = new List<string>
                    {
                        ConfigurationManager.AppSettings["BikeWeb.Uri"]
                    },
                    //PostLogoutRedirectUris = new List<string> 
                    //{
                    //    "https://localhost:44304/"
                    //},
                    AllowedScopes = new List<string>
                    {
                        Constants.StandardScopes.OpenId,
                        Constants.StandardScopes.Profile,
                        Constants.StandardScopes.Email,
                        Constants.StandardScopes.Roles,
                        Constants.StandardScopes.OfflineAccess
                    },
                    RefreshTokenUsage = TokenUsage.ReUse,
                    AccessTokenType = AccessTokenType.Jwt,
                    AccessTokenLifetime = 900,
                    IdentityTokenLifetime = 900,
                }
            };
        }
    }
}