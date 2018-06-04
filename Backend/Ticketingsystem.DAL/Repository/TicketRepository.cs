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
    public class TicketRepository : Repository<Ticket>, ITicketRepository
    {
        private ApplicationDbContext _ctx => (ApplicationDbContext)_context;

        public TicketRepository(ApplicationDbContext context) : base(context)
        {
        }


        public async Task<bool> ChangeTicketStateAndSave(int ticketId, TicketStatus status)
        {
            try
            {
                Ticket availableTicket = await _ctx.Tickets.FirstOrDefaultAsync(t => t.TicketId == ticketId);
                if (availableTicket.TicketStatus == status)
                    return false;

                availableTicket.TicketStatus = status;

                return await _ctx.SaveChangesAsync() == 0;
            }
            catch (Exception ex)
            {
                //WebHelper.LogWebError("Tickets", Constants.General.ApiName, ex, null);
                //throw new Exception("", ex);
                return false;
            }
            
        }

        public async Task<bool> IsModified(Ticket ticket)
        {
            Ticket ticketFromDb = await _ctx.Tickets.AsNoTracking().FirstOrDefaultAsync(t => t.TicketId == ticket.TicketId);

            return ticket.LastModified < ticketFromDb.LastModified;
        }

        public async Task<RepositoryActionResult<Ticket>> GetAvailableTicket(int eventId, TicketType type)
        {
            try
            {
                Ticket availableTicket = await _ctx.Tickets
                    .AsNoTracking()
                    //.Include(t => t.TicketCategory)
                    .Where(t => t.TicketStatus == TicketStatus.AVAILABLE && t.EventId == eventId && t.TicketCategory.Type == type)
                    .OrderBy(r => Guid.NewGuid()) // randomly take 5
                    .FirstOrDefaultAsync();

                return new RepositoryActionResult<Ticket>(availableTicket, RepositoryActionStatus.Ok);

            }
            catch (Exception ex)
            {
                //WebHelper.LogWebError("Tickets", Constants.General.ApiName, ex, null);
                //throw new Exception("", ex);
                return new RepositoryActionResult<Ticket>(null, RepositoryActionStatus.Error);
            }
        }


        public async Task<RepositoryActionResult<Ticket>> GetTicketFromUser(string userId, int ticketId)
        {
            try
            {
                Ticket ticket = await _ctx.Tickets
                    .Include(t => t.TicketCategory)
                    .FirstOrDefaultAsync(t => t.Order.UserId == userId);

                if (ticket == null)
                    return new RepositoryActionResult<Ticket>(ticket, RepositoryActionStatus.NotFound);
                else
                    return new RepositoryActionResult<Ticket>(ticket, RepositoryActionStatus.Ok);
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<Ticket>(null, RepositoryActionStatus.Error, ex);
            }
        }

        public async Task<RepositoryActionResult<IEnumerable<Ticket>>> GetTicketsFromUser(string userId)
        {
            try
            {
                IEnumerable<Ticket> tickets = await _ctx.Tickets
                .Include(t => t.TicketCategory)
                .Where(t => t.Order.UserId == userId)
                .OrderBy(t => t.LastModified)
                .ToListAsync();

                if (tickets == null)
                    return new RepositoryActionResult<IEnumerable<Ticket>>(tickets, RepositoryActionStatus.NotFound);
                else
                    return new RepositoryActionResult<IEnumerable<Ticket>>(tickets, RepositoryActionStatus.Ok);
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<IEnumerable<Ticket>>(null, RepositoryActionStatus.Error, ex);
            }
        }
        
        public async Task<RepositoryActionResult<IEnumerable<Ticket>>> GetTicketsFromOrder(int orderId)
        {            
            try
            {
                IEnumerable<Ticket> tickets = await _ctx.Tickets
                    .Include(t => t.TicketCategory)
                    .Where(t => t.OrderId == orderId)
                    .OrderBy(t => t.LastModified)
                    .ToListAsync();

                if (tickets == null)
                    return new RepositoryActionResult<IEnumerable<Ticket>>(tickets, RepositoryActionStatus.NotFound);
                else
                    return new RepositoryActionResult<IEnumerable<Ticket>>(tickets, RepositoryActionStatus.Ok);
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<IEnumerable<Ticket>>(null, RepositoryActionStatus.Error, ex);
            }
        }
        
        public async Task<RepositoryActionResult<IEnumerable<Ticket>>> GetAllTickets()
        {
            try
            {
                IEnumerable<Ticket> tickets = await _ctx.Tickets
                    .Include(t => t.TicketCategory)
                    .OrderBy(a => a.TicketId)
                    .ToListAsync();

                if (tickets == null)
                    return new RepositoryActionResult<IEnumerable<Ticket>>(tickets, RepositoryActionStatus.NotFound);
                else
                    return new RepositoryActionResult<IEnumerable<Ticket>>(tickets, RepositoryActionStatus.Ok);

            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<IEnumerable<Ticket>>(null, RepositoryActionStatus.Error, ex);
            }
        }
        

        public async Task<RepositoryActionResult<IEnumerable<Ticket>>> GetBoughtTickets(string userId)
        {
            try
            {
                IEnumerable<Ticket> tickets = await _ctx.Tickets
                    .Include(t => t.TicketCategory)
                    .Where(t => t.Order.UserId == userId && t.Order.OrderState == OrderState.COMPLETE)
                    .OrderBy(t => t.LastModified)
                    .ToListAsync();

                if (tickets == null)
                    return new RepositoryActionResult<IEnumerable<Ticket>>(tickets, RepositoryActionStatus.NotFound);
                else
                    return new RepositoryActionResult<IEnumerable<Ticket>>(tickets, RepositoryActionStatus.Ok);
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<IEnumerable<Ticket>>(null, RepositoryActionStatus.Error, ex);
            }
        }

        public async Task<RepositoryActionResult<IEnumerable<Ticket>>> GetTickets(TicketsResourceParameters ticketsResourceParameters)
        {
            try
            {
                IEnumerable<Ticket> tickets = await _ctx.Tickets
                    .Include(t => t.TicketCategory)
                    .OrderBy(a => a.TicketId)
                    .Skip(ticketsResourceParameters.PageSize
                    * (ticketsResourceParameters.PageNumber - 1))
                    .Take(ticketsResourceParameters.PageSize)
                    .ToListAsync();

                if (tickets == null)
                    return new RepositoryActionResult<IEnumerable<Ticket>>(tickets, RepositoryActionStatus.NotFound);
                else
                    return new RepositoryActionResult<IEnumerable<Ticket>>(tickets, RepositoryActionStatus.Ok);
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<IEnumerable<Ticket>>(null, RepositoryActionStatus.Error, ex);
            }
        }

        public async Task<RepositoryActionResult<IEnumerable<Ticket>>> GetTicketsFromEvent(int eventId)
        {
            try
            {
                IEnumerable<Ticket> tickets = await _ctx.Tickets
                    .Include(t => t.TicketCategory)
                    .Where(t => t.EventId == eventId)
                    .OrderBy(t => t.LastModified)
                    .ToListAsync();
                
                if (tickets == null)
                    return new RepositoryActionResult<IEnumerable<Ticket>>(tickets, RepositoryActionStatus.NotFound);
                else
                    return new RepositoryActionResult<IEnumerable<Ticket>>(tickets, RepositoryActionStatus.Ok);
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<IEnumerable<Ticket>>(null, RepositoryActionStatus.Error, ex);
            }
        }
        
        public async Task<RepositoryActionResult<IEnumerable<Ticket>>> GetTicketsFromEvent(TicketsResourceParameters ticketsResourceParameters, int eventId)
        {
            try
            {
                IEnumerable<Ticket> tickets = await _ctx.Tickets
                    .Where(t => t.EventId == eventId)
                    .Include(t => t.TicketCategory)
                    .OrderBy(a => a.TicketId)
                    .Skip(ticketsResourceParameters.PageSize
                    * (ticketsResourceParameters.PageNumber - 1))
                    .Take(ticketsResourceParameters.PageSize)
                    .ToListAsync();

                if (tickets == null)
                    return new RepositoryActionResult<IEnumerable<Ticket>>(tickets, RepositoryActionStatus.NotFound);
                else
                    return new RepositoryActionResult<IEnumerable<Ticket>>(tickets, RepositoryActionStatus.Ok);
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<IEnumerable<Ticket>>(null, RepositoryActionStatus.Error, ex);
            }
        }

        public async Task<RepositoryActionResult<IEnumerable<Ticket>>> GetAvailableTickets(int eventId, TicketType ticketType, int amount)
        {
            try
            {
                IEnumerable<Ticket> tickets = await _ctx.Tickets
                    .Include(t => t.TicketCategory)
                    .Where(t => t.EventId == eventId && t.TicketCategory.Type == ticketType && t.TicketStatus == TicketStatus.AVAILABLE)
                    .OrderBy(r => Guid.NewGuid()) // randomly take 5
                    .Take(amount) 
                    .ToListAsync();

                if (tickets == null)
                    return new RepositoryActionResult<IEnumerable<Ticket>>(tickets, RepositoryActionStatus.NotFound);
                else
                    return new RepositoryActionResult<IEnumerable<Ticket>>(tickets, RepositoryActionStatus.Ok);
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<IEnumerable<Ticket>>(null, RepositoryActionStatus.Error, ex);
            }
        }
        
        public async Task<RepositoryActionResult<IEnumerable<Stock>>> GetAvailableStock(int eventId)
        {
            try
            {
                var details = await _ctx.Tickets
                                           .Where(ticket => ticket.EventId == eventId)
                                           .Join(_ctx.TicketCategory, // the source table of the inner join
                                              ticket => ticket.TicketCategoryId,        // Select the primary key (the first part of the "on" clause in an sql "join" statement)
                                              cat => cat.TicketCategoryId,   // Select the foreign key (the second part of the "on" clause)
                                              (ticket, cat) => new { Ticket = ticket, Cat = cat }) // selection
                                            .GroupBy(x => new { x.Ticket.TicketStatus, x.Cat.Type, x.Cat.Price })
                                            .Select(t => new Stock { TicketStatus = t.Key.TicketStatus, TicketType = t.Key.Type, Price = t.Key.Price ,Amount = t.Count() })
                                            .OrderBy(t => t.TicketType)
                                           .ToListAsync();    // where statement


                return new RepositoryActionResult<IEnumerable<Stock>>(details, RepositoryActionStatus.Ok);

            }
            catch (Exception)
            {
                return new RepositoryActionResult<IEnumerable<Stock>>(null, RepositoryActionStatus.Error);
            }
        }

        public async Task<int> GetAvailableTicketsAmount(int eventId, TicketType type)
        {
            return await (from x in _ctx.Tickets where x.EventId == eventId && x.TicketStatus == TicketStatus.AVAILABLE && x.TicketCategory.Type == type select x).CountAsync();
        }
    }
}
