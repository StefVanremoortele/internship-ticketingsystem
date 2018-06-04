using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ticketingsystem.Domain.Helpers;
using Ticketingsystem.Domain.Interfaces.Repository;
using Ticketingsystem.Domain.Models;

namespace Ticketingsystem.DAL.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private ApplicationDbContext _ctx => (ApplicationDbContext)_context;

        public UserRepository(ApplicationDbContext context) : base(context)
        { }
        
        public async Task<bool> UserExists(string userId)
        {
            try
            {
                return await _ctx.Users.FindAsync(userId) != null;

            }
            catch (Exception ex)
            {
                //WebHelper.LogWebError("Tickets", Constants.General.ApiName, ex, null);
                throw new Exception("", ex);
            }
        }

        public async Task<RepositoryActionResult<User>> GetUserById(string userId)
        {
            try
            {
                User user = await _ctx.Users.FirstOrDefaultAsync(u => u.UserId == userId);
                if (user == null)
                    return new RepositoryActionResult<User>(user, RepositoryActionStatus.NotFound);
                else
                    return new RepositoryActionResult<User>(user, RepositoryActionStatus.Ok);
            }
            catch (Exception ex)
            {

                return new RepositoryActionResult<User>(null, RepositoryActionStatus.Error, ex);
            }
        }
        
    }
}
