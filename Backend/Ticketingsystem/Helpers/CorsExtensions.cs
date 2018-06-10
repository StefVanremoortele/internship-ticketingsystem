using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Ticketingsystem.Helpers
{
    public static class CorsExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .WithOrigins(/*Constants.General.MvcClient_URI,*/ Constants.General.AngularClient_URI)
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .AllowAnyHeader());
            });
        }
    }
}
