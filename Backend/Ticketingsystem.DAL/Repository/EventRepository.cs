    
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
    public class EventRepository : Repository<Event>, IEventRepository
    {
        private ApplicationDbContext _ctx => (ApplicationDbContext)_context;

        public EventRepository(ApplicationDbContext context) : base(context)
        { }
        
        

        public async Task<RepositoryActionResult<IEnumerable<Event>>> GetEvents(ResourceParameters eventsResourceParameters)
        {
            try
            {
                IEnumerable<Event> eventsFromDb = await _ctx.Events
                
                    .OrderBy(e => e.Start)
                    .Skip(eventsResourceParameters.PageSize
                    * (eventsResourceParameters.PageNumber - 1))
                    .Take(eventsResourceParameters.PageSize)
                    .ToListAsync();        
            
                if (eventsFromDb == null)
                    return new RepositoryActionResult<IEnumerable<Event>>(eventsFromDb, RepositoryActionStatus.NotFound);
                else
                    return new RepositoryActionResult<IEnumerable<Event>>(eventsFromDb, RepositoryActionStatus.Ok);            

            }
            catch (Exception)
            {
                return new RepositoryActionResult<IEnumerable<Event>>(null, RepositoryActionStatus.Error);
            }
        }
    }
}
