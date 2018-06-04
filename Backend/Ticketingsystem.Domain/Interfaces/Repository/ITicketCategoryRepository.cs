using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ticketingsystem.Domain.Helpers;
using Ticketingsystem.Domain.Models;

namespace Ticketingsystem.Domain.Interfaces.Repository
{
    public interface ITicketCategoryRepository : IRepository<TicketCategory>
    {
        Task<RepositoryActionResult<IEnumerable<TicketCategory>>> GetAllTicketCategories();
    }
}
