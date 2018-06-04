using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ticketingsystem.Domain.Helpers;
using Ticketingsystem.Domain.Models;

namespace Ticketingsystem.Domain.Interfaces.Services
{
    public interface IAccountService
    {
        Task<Boolean> GetRoles(int eventId);

    }
}
