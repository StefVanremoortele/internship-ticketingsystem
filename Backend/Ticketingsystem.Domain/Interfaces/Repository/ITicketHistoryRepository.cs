using System.Collections.Generic;
using System.Threading.Tasks;
using Ticketingsystem.Domain.Helpers;
using Ticketingsystem.Domain.Models;

namespace Ticketingsystem.Domain.Interfaces.Repository
{
    public interface ITicketHistoryRepository : IRepository<TicketHistory>
    {
        Task<RepositoryActionResult<IEnumerable<TicketHistory>>> GetHistoryAsync(int ticketId);
    }
}
