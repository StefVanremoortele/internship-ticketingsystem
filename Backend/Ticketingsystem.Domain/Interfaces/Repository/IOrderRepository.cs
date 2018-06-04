using System.Collections.Generic;
using System.Threading.Tasks;
using Ticketingsystem.Domain.Helpers;
using Ticketingsystem.Domain.Models;

namespace Ticketingsystem.Domain.Interfaces.Repository
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<RepositoryActionResult<Order>> GetCart(string userId);
        Task<RepositoryActionResult<Order>> GetLastOrder(string userId);
        Task<RepositoryActionResult<IEnumerable<Order>>> GetAllOrderHistoryFromUser(string userId);
        Task<RepositoryActionResult<IEnumerable<Order>>> GetAllOrderHistoryFromUser(string userId, ResourceParameters parameters);
        Task<int> GetOrderHistoryTotalCount(string userId);

    }
}
