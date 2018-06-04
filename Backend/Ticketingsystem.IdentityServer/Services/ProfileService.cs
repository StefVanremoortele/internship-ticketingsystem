using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ticketingsystem.IdentityServer.Models;

namespace Ticketingsystem.IdentityServer.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            context.IssuedClaims.AddRange(context.Subject.Claims);

            var claim = context.IssuedClaims.Find(cl => cl.Type == "name");

            var user = _userManager.FindByEmailAsync(claim.Value).Result;

            if (user == null)
            {
                context.IssuedClaims.Add(new Claim("role", "customer"));
            }
            else
            {
                var userClaims = _userManager.GetClaimsAsync(user).Result;
                context.IssuedClaims.AddRange(userClaims);
            }

            return Task.FromResult(0);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.FromResult(0);
        }
    }
}
