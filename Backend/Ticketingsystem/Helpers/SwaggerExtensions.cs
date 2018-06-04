using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Ticketingsystem.Filters;

namespace Ticketingsystem.Helpers
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = Constants.General.ApiName + " on " + Environment.MachineName,
                    Description = "Internship project: Ticketing System",
                    Contact = new Contact
                    {
                        Name = "Stef Vanremoortele",
                        Email = "stef.vanremoortele@student.howest.be",
                        Url = "http://www.howest.be"
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "Ticketingsystem.xml");
                c.IncludeXmlComments(xmlPath);

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                c.DocumentFilter<SecurityRequirementsDocumentFilter>();
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger()
                 .UseSwaggerUI(c => {
                     c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ticketingsystem API V1");
                     c.OAuthClientSecret("secret");
                     c.OAuthAppName(Constants.General.ApiName);
                     c.OAuthClientId("swaggerui");
                     c.OAuthAdditionalQueryStringParams(new { nonce = Guid.NewGuid().ToString() });

                 });
            return app;
        }
    }
}
