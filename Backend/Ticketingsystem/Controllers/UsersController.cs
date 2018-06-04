using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using Ticketingsystem.Core;
using Ticketingsystem.DAL.Core;
using Ticketingsystem.DAL.Utilities;
using Ticketingsystem.Domain.Helpers;
using Ticketingsystem.Domain.Interfaces.Services;
using Ticketingsystem.Domain.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ticketingsystem.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IScheduler _quartzScheduler;

        public UsersController(IUserService userService, IScheduler scheduler)
        {
            _userService = userService;
            _quartzScheduler = scheduler;

        }


        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>http statuscode</returns> 
        [AllowAnonymous]
        [HttpGet("api/quartzTest", Name = "testquartz")]
        [ProducesResponseType(typeof(IEnumerable<DTO.Users.User>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> TestQuartz()
        {
            QuartzServicesUtilities.StartJob<CartExpireJob>(_quartzScheduler, new TimeSpan(0, 0, 0, 10), "20");
            return Ok();
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>http statuscode</returns> 
        [Authorize(Policy = "administratorPolicy")]
        [HttpGet("api/users", Name = "GetUsers")]
        [ProducesResponseType(typeof(IEnumerable<DTO.Users.User>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            RepositoryActionResult<IEnumerable<User>> result = await _userService.GetAllUsers();

            if (result.Status == RepositoryActionStatus.Error)
                return StatusCode(StatusCodes.Status500InternalServerError);

            if (result.Status == RepositoryActionStatus.NotFound)
                return StatusCode(StatusCodes.Status404NotFound);

            var entities = Mapper.Map<IEnumerable<DTO.Users.User>>(result.Entity);
            return StatusCode(StatusCodes.Status200OK, entities);
        }


        /// <summary>
        /// Gets a specific user.
        /// </summary>
        /// <param name="userId">The user ID</param> 
        /// <returns>a specific user</returns> 
        [Authorize(Policy = "customerPolicy")]
        [HttpGet("api/users/{userId}", Name = "GetUser")]
        [ProducesResponseType(typeof(DTO.Users.User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(string userId)
        {
            var userIdClaim = User.FindFirst("sub").Value;
            var hasAdminClaim = User.HasClaim("role", "administrator");
            
            if (userId != userIdClaim && !hasAdminClaim) // administrators can request every user
                return StatusCode(StatusCodes.Status400BadRequest, null);

            RepositoryActionResult<User> result = await _userService.GetUser(userId);

            if (result.Status == RepositoryActionStatus.Error)
                return StatusCode(StatusCodes.Status500InternalServerError);

            if (result.Status == RepositoryActionStatus.NotFound)
                return StatusCode(StatusCodes.Status404NotFound);

            var entities = Mapper.Map<DTO.Users.User>(result.Entity);
            return StatusCode(StatusCodes.Status200OK, entities);
        }
    }
}
