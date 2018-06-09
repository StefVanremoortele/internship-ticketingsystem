
using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ticketingsystem.Domain.Helpers;
using Ticketingsystem.Domain.Interfaces.Services;
using Ticketingsystem.Domain.Models;

namespace Ticketingsystem.Controllers
{
    public class AccountController : Controller
    {

        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;

        }


        /// <summary>
        /// Updates a user.
        /// </summary>
        /// <param name="userToUpdate">The user to update object</param>
        /// <returns>http statuscode</returns>
        [Authorize]
        [HttpPut("api/account/profile/update", Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] DTO.Users.UserForUpdate userToUpdate)
        {
            try
            {
                
                var userIdClaim = User.FindFirst("sub").Value;

                if (userToUpdate == null || userToUpdate.UserId != userIdClaim)
                    return StatusCode(StatusCodes.Status400BadRequest);
                
                RepositoryActionResult<User> oldUserResult = await _userService.GetUser(userIdClaim);
                User oldUser = oldUserResult.Entity;

                var newUser = Mapper.Map(userToUpdate, oldUser);
                newUser.UpdatedDate = DateTime.UtcNow;

                var updatedUserResult = _userService.UpdateUser(newUser);

                if (!updatedUserResult)
                    return StatusCode(StatusCodes.Status500InternalServerError);

                var userToReturn = Mapper.Map<DTO.Users.UserForUpdate>(oldUser);
                return Ok(userToReturn);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}