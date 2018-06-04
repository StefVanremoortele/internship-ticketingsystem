using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ticketingsystem.Constants;
using Ticketingsystem.IdentityServer.Data;
using Ticketingsystem.IdentityServer.Models;

namespace Ticketingsystem.IdentityServer
{
    public class SeedData
    {
        public static void EnsureSeedData(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                context.Database.Migrate();

                var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleMgr= scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();


                // Add the Admin role to the database
                IdentityResult roleResult;
                bool adminRoleExists = roleMgr.RoleExistsAsync("Admin").Result;
                if (!adminRoleExists)
                {
                    roleResult = roleMgr.CreateAsync(new IdentityRole("Admin")).Result;
                    if (roleResult.Succeeded)
                    {
                        "Admin role added".ConsoleGreen();
                    }
                }
              

                // Adding user "Daniyar"
                var daniyar = userMgr.FindByNameAsync("daniyar").Result;
                if (daniyar == null)
                {
                    daniyar = new ApplicationUser
                    {
                        UserName = "daniyar.talipov@realdolmen.com",
                        NormalizedUserName = "daniyar.talipov@realdolmen.com",
                        Email = "daniyar.talipov@realdolmen.com",          
                        NormalizedEmail = "daniyar.talipov@realdolmen.com".ToUpper()
                    };
                    var result = userMgr.CreateAsync(daniyar, "Pass123$").Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddToRoleAsync(daniyar, "Admin").Result;

                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddClaimsAsync(daniyar, new Claim[]{
                        new Claim(JwtClaimTypes.Name, "Daniyar Talipov"),
                        new Claim(JwtClaimTypes.GivenName, "Daniyar"),
                        new Claim(JwtClaimTypes.FamilyName, "Talipov"),
                        new Claim(JwtClaimTypes.Email, "daniyar.talipov@realdolmen.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://realdolmen.com"),
                        new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'Gaston Crommenlaan 4', 'locality': 'Gent', 'postal_code': 9050, 'country': 'Belgium' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                        new Claim("role", "administrator"),
                        new Claim("role", "customer")
                     }).Result;

                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                    "Daniyar created".ConsoleGreen();
                }
                else
                {
                    "Daniyar already exists".ConsoleYellow();
                }



                // Adding user "Arno"
                var arno = userMgr.FindByNameAsync("arno").Result;
                if (arno == null)
                {
                    arno = new ApplicationUser
                    {
                        UserName = "arno.abraham@realdolmen.com",
                        NormalizedUserName = "arno.abraham@realdolmen.com",
                        Email = "arno.abraham@realdolmen.com",
                        NormalizedEmail = "arno.abraham@realdolmen.com".ToUpper()
                    };
                    var result = userMgr.CreateAsync(arno, "Pass123$").Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddToRoleAsync(arno, "Admin").Result;

                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddClaimsAsync(arno, new Claim[]{
                        new Claim(JwtClaimTypes.Name, "Arno Abraham"),
                        new Claim(JwtClaimTypes.GivenName, "Arno"),
                        new Claim(JwtClaimTypes.FamilyName, "Abraham"),
                        new Claim(JwtClaimTypes.Email, "arno.abraham@realdolmen.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://realdolmen.com"),
                        new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'Gaston Crommenlaan 4', 'locality': 'Gent', 'postal_code': 9050, 'country': 'Belgium' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                        new Claim("role", "administrator"),
                        new Claim("role", "customer")
                    }).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                    "Arno created".ConsoleGreen();
                }
                else
                {
                    "Arno already exists".ConsoleYellow();
                }



                // Adding user "Stef"
                var stef = userMgr.FindByNameAsync("stef").Result;
                if (stef == null)
                {
                    stef = new ApplicationUser
                    {
                        UserName = "stef.vanremoortele@realdolmen.com",
                        NormalizedUserName = "stef.vanremoortele@realdolmen.com",
                        Email = "stef.vanremoortele@realdolmen.com",
                        NormalizedEmail = "stef.vanremoortele@realdolmen.com".ToUpper()
                    };
                    var result = userMgr.CreateAsync(stef, "Pass123$").Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddToRolesAsync(stef, new string[]{ "Admin" }).Result;

                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }


                    result = userMgr.AddClaimsAsync(stef, new Claim[]{
                        new Claim(JwtClaimTypes.Name, "Stef Vanremoortele"),
                        new Claim(JwtClaimTypes.GivenName, "Stef"),
                        new Claim(JwtClaimTypes.FamilyName, "Vanremoortele"),
                        new Claim(JwtClaimTypes.Email, "stef.vanremoortele@realdolmen.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://realdolmen.com"),
                        new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'Gaston Crommenlaan 4', 'locality': 'Gent', 'postal_code': 9050, 'country': 'Belgium' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                        new Claim("role", "administrator"),
                    }).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                    "Stef created".ConsoleGreen();
                }
                else
                {
                    "Stef already exists".ConsoleYellow();
                }


                // Adding a customer
                var customer = userMgr.FindByNameAsync("customer").Result;
                if (customer == null)
                {
                    customer = new ApplicationUser
                    {
                        UserName = "customer@realdolmen.com",
                        NormalizedUserName = "customer@realdolmen.com",
                        Email = "customer@realdolmen.com",
                        NormalizedEmail = "customer@realdolmen.com".ToUpper()
                    };
                    var result = userMgr.CreateAsync(customer, "Pass123$").Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddClaimsAsync(customer, new Claim[]{
                        new Claim(JwtClaimTypes.Name, "customer"),
                        new Claim(JwtClaimTypes.GivenName, "customer"),
                        new Claim(JwtClaimTypes.FamilyName, "customer"),
                        new Claim(JwtClaimTypes.Email, "customer@realdolmen.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://realdolmen.com"),
                        new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'Gaston Crommenlaan 4', 'locality': 'Gent', 'postal_code': 9050, 'country': 'Belgium' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                        new Claim("role", "customer")
                    }).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                    "customer created".ConsoleGreen();
                }
                else
                {
                    "customer already exists".ConsoleYellow();
                }

                // adding an administrator
                var administrator = userMgr.FindByNameAsync("administrator").Result;
                if (administrator == null)
                {
                    administrator = new ApplicationUser
                    {
                        UserName = "administrator@realdolmen.com",
                        NormalizedUserName = "administrator@realdolmen.com",
                        Email = "administrator@realdolmen.com",
                        NormalizedEmail = "administrator@realdolmen.com".ToUpper()
                    };
                    var result = userMgr.CreateAsync(administrator, "Pass123$").Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddToRolesAsync(customer, new string[] { "Admin" }).Result;

                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddClaimsAsync(administrator, new Claim[]{
                        new Claim(JwtClaimTypes.Name, "administrator"),
                        new Claim(JwtClaimTypes.GivenName, "administrator"),
                        new Claim(JwtClaimTypes.FamilyName, "administrator"),
                        new Claim(JwtClaimTypes.Email, "administrator@realdolmen.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://realdolmen.com"),
                        new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'Gaston Crommenlaan 4', 'locality': 'Gent', 'postal_code': 9050, 'country': 'Belgium' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                        new Claim("role", "administrator"),
                    }).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                    "administrator created".ConsoleGreen();
                }
                else
                {
                    "administrator already exists".ConsoleYellow();
                }
            }
        }
    }
}
