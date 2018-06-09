using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ticketingsystem.Domain.Helpers;
using Ticketingsystem.Domain.Models;

namespace Ticketingsystem.Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<bool> UserExists(string id);
        Task<RepositoryActionResult<User>> GetUser(string userId);

        Task<RepositoryActionResult<IEnumerable<User>>> GetAllUsers();
        Task<RepositoryActionResult<IEnumerable<UserType>>> GetAllUserRoles();


        Task<User> RegisterUser(string userId, UserType userType);

        bool UpdateUser(User userToUpdate);
    }
}
