// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ticketingsystem.IdentityServer.Models;

namespace Ticketingsystem.IdentityServer.Quickstart.Home
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IHostingEnvironment _environment;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(IIdentityServerInteractionService interaction, IHostingEnvironment environment, UserManager<ApplicationUser> userManager)
        {
            _interaction = interaction;
            _environment = environment;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            if (_environment.IsDevelopment())
            {
                // only show in development
                return View();
            }
            return View();
            //return NotFound();
        }

        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { Error = new ErrorMessage{Error = "error"}});
        //}

        /// <summary>
        /// Shows the error page
        /// </summary>
        public async Task<IActionResult> Error(string errorId)
        {
            var vm = new ErrorViewModel();

            // retrieve error details from identityserver
            var message = await _interaction.GetErrorContextAsync(errorId);
            if (message != null)
            {
                vm.Error = message;

                if (!_environment.IsDevelopment())
                {
                    // only show in development
                    message.ErrorDescription = null;
                }
            }

            return View("Error", vm);
        }

        public async Task<IActionResult> GetAdminClaim()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return View("NoExternalAdmins");
            }

            var userCLaims = await _userManager.GetClaimsAsync(user);


            var claim = new Claim("role", "administrator");
            if (userCLaims.Contains(claim))
            {
                await Error("1337");
            }
            var addClaimResult = await _userManager.AddClaimAsync(user, claim);
            return View(addClaimResult);
        }

        public async Task<IActionResult> GetEmployeeClaim()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return View();

            var userCLaims = await _userManager.GetClaimsAsync(user);
            var claim = new Claim("role", "customer");
            if (userCLaims.Contains(claim))
            {
                await Error("1337");
            }
            var addClaimResult = await _userManager.AddClaimAsync(user, claim);
            return View(addClaimResult);
        }

    }
}