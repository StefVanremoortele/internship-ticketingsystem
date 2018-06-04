using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ticketingsystem.Domain.Helpers;
using Ticketingsystem.Domain.Interfaces.Repository;
using Ticketingsystem.Domain.Models;

namespace Ticketingsystem.DAL.Repository
{
    public class TicketHistoryRepository : Repository<TicketHistory>, ITicketHistoryRepository
    {
        private ApplicationDbContext _ctx => (ApplicationDbContext)_context;

        public TicketHistoryRepository(ApplicationDbContext context) : base(context)
        {
                
        }
        

        public async Task<RepositoryActionResult<IEnumerable<TicketHistory>>> GetHistoryAsync(int ticketId)
        {
            try
            {
                IEnumerable<TicketHistory> ticketHistoryFromDb = await _ctx.TicketHistory
                .Where(th => th.TicketId == ticketId)
                .OrderBy(th => th.Timestamp)
                .ThenBy(th => th.TicketId)
                .ToListAsync();

                if (ticketHistoryFromDb == null)
                    return new RepositoryActionResult<IEnumerable<TicketHistory>>(ticketHistoryFromDb, RepositoryActionStatus.NotFound);
                else
                    return new RepositoryActionResult<IEnumerable<TicketHistory>>(ticketHistoryFromDb, RepositoryActionStatus.Ok);
            }
            catch (Exception)
            {

                return new RepositoryActionResult<IEnumerable<TicketHistory>>(null, RepositoryActionStatus.Error);
            }      
        }
    }
}
