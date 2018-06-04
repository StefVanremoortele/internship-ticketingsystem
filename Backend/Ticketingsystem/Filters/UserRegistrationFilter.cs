using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ticketingsystem.Domain.Interfaces.Services;
using Ticketingsystem.Domain.Models;

namespace Ticketingsystem.Filters
{

    public class UserRegistrationFilter : IAsyncAuthorizationFilter
    {

        /// <inheritdoc />
        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            try
            {
                // get the userService
                var _userService = (IUserService)context.HttpContext.RequestServices.GetService(typeof(IUserService));
                //var _accountService = (IAccountService)context.HttpContext.RequestServices.GetService(typeof(IAccountService));

                // get the sub claim
                var idClaim = context.HttpContext.User.FindFirst("sub");

                if (idClaim == null)
                {
                    return Task.CompletedTask;
                }
                // register user
                var userResult = _userService.GetUser(idClaim.Value).Result;
                var emailClaim = context.HttpContext.User.FindFirst("email");
                var hasAdminClaim = context.HttpContext.User.IsInRole("administrator");

                if (userResult.Entity == null)
                {
                    _userService.RegisterUser(idClaim.Value, /*emailClaim.Value,*/
                        hasAdminClaim ? UserType.ADMINISTRATOR : UserType.CUSTOMER);
                }

                return Task.CompletedTask;

                //return Task.CompletedTask;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
