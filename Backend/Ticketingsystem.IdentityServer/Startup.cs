using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer4;
using Microsoft.IdentityModel.Tokens;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Http;
using System.Text;
using Microsoft.AspNetCore.Diagnostics;
using System.Diagnostics;
using IdentityServer4.Services;
using Newtonsoft.Json;
using Ticketingsystem.IdentityServer.Data;
using Ticketingsystem.IdentityServer.Extensions;
using Ticketingsystem.IdentityServer.Models;
using Ticketingsystem.IdentityServer.Services;

namespace Ticketingsystem.IdentityServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }    

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services
                .AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy",
                        builder => builder
                            .WithOrigins(Constants.General.MvcClient_URI, Constants.General.AngularClient_URI, Constants.General.API_URI)
                            .AllowAnyMethod()
                            .AllowCredentials()
                            .AllowAnyHeader());
                });

            services.AddMvc();

            // SQL SERVER DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("AzureConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer()
                .AddOperationalStore(options =>
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(Configuration.GetConnectionString("AzureConnection"), sqlOptions => sqlOptions.MigrationsAssembly("Ticketingsystem.IdentityServer")))
                .AddConfigurationStore(options =>
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(Configuration.GetConnectionString("AzureConnection"), sqlOptions => sqlOptions.MigrationsAssembly("Ticketingsystem.IdentityServer")))
                .AddAspNetIdentity<ApplicationUser>()
                .AddDeveloperSigningCredential()
                .AddProfileService<ProfileService>();
            

            services.AddAuthentication()
                .AddGoogle("Google", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                    options.ClientId = "728729069402-mola8pccg8gsf19bu2g38e7qf79rl8bh.apps.googleusercontent.com";
                    options.ClientSecret = "bYG6Gpb_Nx4zG6lght5ZrSzh";
                })
                .AddOpenIdConnect("oidc", "OpenID Connect", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.SignOutScheme = IdentityServerConstants.SignoutScheme;

                    options.Authority = "https://demo.identityserver.io/";
                    options.ClientId = "implicit";

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "role",
                        RoleClaimType = "customer"
                    };
                });

            //services.AddTransient<IProfileService, ProfileService>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddDirectoryBrowser();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            
            app.InitIdentityConfig();

            app.UseCors("CorsPolicy");
            app.UseIdentityServer();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
