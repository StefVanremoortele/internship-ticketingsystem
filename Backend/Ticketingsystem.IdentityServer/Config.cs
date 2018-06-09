using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Ticketingsystem.IdentityServer
{
    public static class Config
    {
        // scopes define the resources in your system
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource {
                    Name = "role",
                    UserClaims = new[] { JwtClaimTypes.Role}
                }
            };
        }


        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource {
                    Name = Constants.General.ApiName,
                    DisplayName = Constants.General.ApiName,
                    Description = "Ticketingsystem API Access",
                    UserClaims = new List<string> {"role"},
                    ApiSecrets = new List<Secret> {new Secret("secret".Sha256())},
                    Scopes = new List<Scope> {
                        new Scope(Constants.General.ApiName),
                        //new Scope(Constants.SCOPE_WRITE),
                        //new Scope(Constants.SCOPE_READ)
                    },
                }
            };
        }        

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {
                new Client
                {
                    ClientId = Constants.General.ConsoleClientId,
                    ClientName = "Console Client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    RequireConsent = false,

                    ClientSecrets =
                    {
                        new Secret(Constants.General.ConsoleClientSecret.Sha256())
                    },
                    AllowedScopes = { Constants.General.ApiName, Constants.General.SCOPE_READ }
                },

                new Client
                {
                    ClientId = Constants.General.MvcClientId,
                    ClientUri = Constants.General.MvcClient_URI,
                    ClientName = "MVC Client",
                    
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

                    ClientSecrets =
                    {
                        new Secret(Constants.General.MvcClientSecret.Sha256())
                    },

                    RedirectUris = { Constants.General.MvcClient_URI + "/signin-oidc" },

                    PostLogoutRedirectUris = { Constants.General.MvcClient_URI + "/signout-callback-oidc" },


                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        Constants.General.ApiName,
                        //Constants.SCOPE_WRITE,
                        //Constants.SCOPE_READ
                    },
                    ClientClaimsPrefix = "",
                    AllowOfflineAccess = true,
                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true
                },

                new Client
                {
                    ClientId = Constants.General.AngularClientId,
                    ClientUri = Constants.General.AngularClient_URI,
                    ClientName = "Angular Client",
                    AllowedGrantTypes = GrantTypes.Implicit,

                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { Constants.General.AngularClient_URI + "/signin-callback.html" },
                    PostLogoutRedirectUris = { Constants.General.AngularClient_URI },
                    AllowedCorsOrigins = { Constants.General.AngularClient_URI },

                    ClientClaimsPrefix = "",
                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true,

                    AccessTokenLifetime = 100000,

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        Constants.General.ApiName,
                        //Constants.General.SCOPE_WRITE,
                        //Constants.General.SCOPE_READ
                    },
                    AllowOfflineAccess = true,
                },

                new Client
                {
                    ClientId = "swaggerui",
                    ClientName = "Swagger",
                    AllowedGrantTypes = GrantTypes.Implicit,


                    ClientSecrets =
                    {
                        new Secret(Constants.General.AngularClientSecret.Sha256())
                    },
                    AllowedScopes = {
                        Constants.General.ApiName,
                        //Constants.General.SCOPE_READ,
                        //Constants.General.SCOPE_WRITE
                    }
                },
            };
        }
        
    }
}
