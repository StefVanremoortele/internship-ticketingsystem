using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ticketingsystem.Domain.Helpers;
using Ticketingsystem.Domain.Models;

namespace Ticketingsystem.Domain.Interfaces.Services
{
    public interface IOrderService
    {
        Task<int> GetOrderHistoryTotalCount(string userId);

        Task<RepositoryActionResult<Order>> GetCart(string userId);
        Task<RepositoryActionResult<Order>> GetLastOrder(string userId);
        Task<RepositoryActionResult<Order>> CompleteOrder(string userId, int orderId);        
        Task<RepositoryActionResult<Order>> CancelCart(string userId);        
        Task<RepositoryActionResult<Order>> ExpireOrder(string userId);

        Task<RepositoryActionResult<Ticket>> CancelTicketFromOrder(string userId, int ticketId);

        Task<RepositoryActionResult<IEnumerable<Order>>> GetAllOrderHistory(string userId);
        Task<RepositoryActionResult<IEnumerable<Order>>> GetAllOrderHistory(string userId, ResourceParameters parameters);
    }
}
