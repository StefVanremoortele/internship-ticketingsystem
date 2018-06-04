using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ticketingsystem.Domain.Helpers;
using Ticketingsystem.Domain.Models;

namespace Ticketingsystem.Domain.Interfaces.Repository
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        Task<bool> ChangeTicketStateAndSave(int ticketId, TicketStatus status);
        Task<bool> IsModified(Ticket ticket);

        Task<RepositoryActionResult<Ticket>> GetTicketFromUser(string userId, int ticketId);
        Task<RepositoryActionResult<Ticket>> GetAvailableTicket(int eventId, TicketType type);

        Task<RepositoryActionResult<IEnumerable<Ticket>>> GetTicketsFromUser(string userId);
        Task<RepositoryActionResult<IEnumerable<Ticket>>> GetTicketsFromOrder(int orderId);
        Task<RepositoryActionResult<IEnumerable<Ticket>>> GetBoughtTickets(string userId);
        Task<RepositoryActionResult<IEnumerable<Ticket>>> GetTickets(TicketsResourceParameters ticketsResourceParameters);
        Task<RepositoryActionResult<IEnumerable<Ticket>>> GetTicketsFromEvent(int eventId);
        Task<RepositoryActionResult<IEnumerable<Ticket>>> GetTicketsFromEvent(TicketsResourceParameters ticketsResourceParameters, int eventId);
        Task<RepositoryActionResult<IEnumerable<Ticket>>> GetAvailableTickets(int eventId, TicketType ticketType, int amount);
        Task<RepositoryActionResult<IEnumerable<Stock>>> GetAvailableStock(int eventId);

        Task<int> GetAvailableTicketsAmount(int eventId, TicketType type);
    }
}
