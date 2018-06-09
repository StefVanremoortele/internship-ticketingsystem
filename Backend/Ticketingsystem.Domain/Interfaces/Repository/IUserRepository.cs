using System.Collections.Generic;
using System.Threading.Tasks;
using Ticketingsystem.Domain.Helpers;
using Ticketingsystem.Domain.Models;

namespace Ticketingsystem.Domain.Interfaces.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> UserExists(string userId);

        Task<RepositoryActionResult<User>> GetUserById(string userId);
        Task<RepositoryActionResult<IEnumerable<UserType>>> GetAllUserRoles();
    }
}
