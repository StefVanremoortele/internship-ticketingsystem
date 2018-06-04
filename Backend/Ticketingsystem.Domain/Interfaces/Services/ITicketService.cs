using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketingsystem.Domain.Helpers;
using Ticketingsystem.Domain.Models;

namespace Ticketingsystem.Domain.Interfaces.Services
{
    public interface ITicketService
    {
        Task<RepositoryActionResult<Ticket>> GetTicket(int ticketId); 
        Task<RepositoryActionResult<Ticket>> ProcessReservation(string userId, int eventId, TicketType ticketType);

        Task<RepositoryActionResult<IEnumerable<Ticket>>> GetTicketsFromOrder(int orderId);
        Task<RepositoryActionResult<IEnumerable<Ticket>>> GetBoughtTicketsFromUser(string userId);
        Task<RepositoryActionResult<IEnumerable<Ticket>>> GetTicketsFromEvent(int eventId, TicketsResourceParameters ticketsResourceParameters);
        Task<RepositoryActionResult<IEnumerable<Ticket>>> GetAllTicketsFromEvent(int eventId);
        Task<RepositoryActionResult<IEnumerable<TicketHistory>>> GetTicketHistory(int ticketId);
        Task<RepositoryActionResult<IEnumerable<TicketCategory>>> GetAllTicketCategories();
        Task<RepositoryActionResult<IEnumerable<Stock>>> GetAvailableStock(int eventId);        
        Task<int> GetAmountLeft(int eventId, TicketType type);

    }
}
