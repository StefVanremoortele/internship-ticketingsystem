using System.Collections.Generic;
using System.Threading.Tasks;
using Ticketingsystem.Domain.Helpers;
using Ticketingsystem.Domain.Interfaces.Services;
using Ticketingsystem.Domain.Models;

namespace Ticketingsystem.Domain.Interfaces.Repository
{
    public interface IEventRepository : IRepository<Event>
    {
        Task<RepositoryActionResult<IEnumerable<Event>>> GetEvents(ResourceParameters eventsResourceParameters);
    }
}