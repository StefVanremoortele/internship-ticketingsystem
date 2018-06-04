using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ticketingsystem.Domain.Helpers;
using Ticketingsystem.Domain.Interfaces;
using Ticketingsystem.Domain.Interfaces.Services;
using Ticketingsystem.Domain.Models;

namespace Ticketingsystem.DAL.Services
{

    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork, IServiceProvider provider)
        {
            _unitOfWork = unitOfWork;
        }
        
        
        public async Task<RepositoryActionResult<User>> GetUser(string userId)
        {
            return await _unitOfWork.Users.GetUserById(userId);
        }

        public async Task<RepositoryActionResult<IEnumerable<User>>> GetAllUsers()
        {
            return await _unitOfWork.Users.GetAllAsync();
        }

        public async Task<User> RegisterUser(string userId, UserType userType)
        {
            try
            {
                User user = new User
                {
                    UserId = userId,
                    //Email = email,
                    UserType = userType,
                    CreatedDate = DateTime.UtcNow
                };
                _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<bool> UserExists(string id)
        {
            return _unitOfWork.Users.UserExists(id);
        }
    }
}
