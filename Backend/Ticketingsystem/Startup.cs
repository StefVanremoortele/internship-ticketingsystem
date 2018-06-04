using System;
using System.Diagnostics;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Ticketingsystem.App_Start;
using Ticketingsystem.Core;
using Ticketingsystem.DAL;
using Ticketingsystem.DAL.Core;
using Ticketingsystem.DAL.Services;
using Ticketingsystem.Domain.Interfaces;
using Ticketingsystem.Domain.Interfaces.Services;
using Ticketingsystem.Filters;
using Ticketingsystem.Helpers;
using Ticketingsystem.Logging;

namespace Ticketingsystem
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }


        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //SQL Server
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"], b => b.MigrationsAssembly("Ticketingsystem.DAL"));
            });


            var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole("customer")
                .Build();

            services.AddMvc(options =>
            {
                options.Filters.Add(new TrackPerformanceFilter("Tickets", Constants.General.ApiName));
                options.Filters.Add(typeof(UserRegistrationFilter));
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            services
                .AddAuthorization(options =>
                {
                    options.AddPolicy("administratorPolicy", adminPolicy =>
                    {
                        adminPolicy.RequireAuthenticatedUser();
                        adminPolicy.RequireClaim("role", "Administrator");
                    });
                    options.AddPolicy("customerPolicy", userPolicy =>
                    {
                        userPolicy.RequireAuthenticatedUser();
                        userPolicy.RequireClaim("role", "customer");
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

            // OLD DI SYSTEM
            // Services
            services.AddScoped<IUnitOfWork, HttpUnitOfWork>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<IEventService, EventService>()
                .AddScoped<IOrderService, OrderService>()
                .AddScoped<ITicketService, TicketService>()
                //.AddScoped<IQuartzService, QuartzService>()
                .AddScoped<IDatabaseInitializer, DbInitializer>();

            services.AddSwaggerDocumentation();
            services.ConfigureCors();
            services.AddSignalR();
            services.AddQuartz( typeof(CartExpireJob));


            return services.BuildServiceProvider();

            // autofac DI
            //var builder = new ContainerBuilder();
            //builder.RegisterModule<DefaultModule>();
            //builder.Populate(services);

            //var container = builder.Build();
            //return new AutofacServiceProvider(container);
        }

        public void Configure(IApplicationBuilder app,
                                IHostingEnvironment env,
                                IServiceProvider serviceProvider)
        {
            app.UseQuartz();
            app.UseCors("CorsPolicy");
            app.UseExceptionHandler(eApp =>
            {
                eApp.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";

                    var errorCtx = context.Features.Get<IExceptionHandlerFeature>();
                    if (errorCtx != null)
                    {
                        var ex = errorCtx.Error;
                        WebHelper.LogWebError("Exception", Constants.General.ApiName, ex, context);

                        var errorId = Activity.Current?.Id ?? context.TraceIdentifier;
                        var jsonResponse = JsonConvert.SerializeObject(new CustomErrorResponse
                        {
                            ErrorId = errorId,
                            Message = "An error occurred in the api. If you need assistance please contact our support with the given errorId."
                        });
                        await context.Response.WriteAsync(jsonResponse, Encoding.UTF8);
                    }
                });
            });

            app.UseSignalR(routes =>
            {
                routes.MapHub<StockHub>("/coolmessages");

            });

            AutomapperConfiguration.ConfigureProfiles();

            app.UseSwaggerDocumentation()
                .UseStatusCodePages()
                .UseAuthentication()
                .UseMvc();
        }
    }
}
