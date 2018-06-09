using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ticketingsystem.Domain.Helpers;
using Ticketingsystem.Domain.Interfaces;
using Ticketingsystem.Domain.Interfaces.Services;
using Ticketingsystem.Domain.Models;
using Ticketingsystem.Logging;

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

        public async Task<RepositoryActionResult<IEnumerable<UserType>>> GetAllUserRoles()
        {
            return await _unitOfWork.Users.GetAllUserRoles();
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

        public async Task<bool> UserExists(string id)
        {
            return await _unitOfWork.Users.UserExists(id);
        }

        public bool UpdateUser(User userToUpdate)
        {
            try
            {
                _unitOfWork.Users.Update(userToUpdate);

                var res =  _unitOfWork.SaveChanges();

                return res;
            }
            catch (Exception ex)
            {
                WebHelper.LogWebError("Users", "Ticketingsystem_DAL", ex, null);
                throw ex;
            }
        }
    }


}
