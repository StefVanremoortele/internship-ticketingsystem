using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ticketingsystem.Helpers
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection ConfigureAuthentication(this IServiceCollection services)
        {
            services
                .AddAuthorization(options =>
                {
                    options.AddPolicy("administratorPolicy", adminPolicy =>
                    {
                        adminPolicy.RequireAuthenticatedUser();
                        adminPolicy.RequireClaim("role", "Administrator");
                        adminPolicy.RequireScope(Constants.General.ApiName);
                    });
                    options.AddPolicy("customerPolicy", userPolicy =>
                    {
                        userPolicy.RequireAuthenticatedUser();
                        userPolicy.RequireClaim("role", "customer");
                        userPolicy.RequireScope(Constants.General.ApiName);
                    });
                })
                .AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    // === FOR DEMO ONLY
                    options.RequireHttpsMetadata = false;
                    // SET THIS TO true IN PRODUCTION!

                    options.Authority = Constants.General.Authority_URI;
                    options.ApiName = Constants.General.ApiName;
                    options.ApiSecret = Constants.General.ApiSecret;
                });
            return services;
        }
    }
}
