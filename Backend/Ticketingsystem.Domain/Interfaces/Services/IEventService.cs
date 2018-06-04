using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ticketingsystem.Domain.Helpers;
using Ticketingsystem.Domain.Models;

namespace Ticketingsystem.Domain.Interfaces.Services
{
    public interface IEventService
    {
        Task<RepositoryActionResult<Event>> GetEvent(int eventId);

        Task<RepositoryActionResult<IEnumerable<Event>>> GetAllEvents(ResourceParameters eventsResourceParameters);
        Task<RepositoryActionResult<IEnumerable<Event>>> GetAllEvents();

        Task<RepositoryActionResult<Event>> CreateEvent(Event Event);        
    }
}
