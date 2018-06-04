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
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private ApplicationDbContext _ctx => (ApplicationDbContext)_context;

        public OrderRepository(ApplicationDbContext context) : base(context)
        { }

        public void AddTicketToOrder(int orderId, Ticket ticketToAdd)
        {
            var orderFromDB = _ctx.Orders.Find(orderId);
            if (orderFromDB != null)
            {
                orderFromDB.Tickets.Add(ticketToAdd);
            }
        }
        

        public async Task<RepositoryActionResult<Order>> GetCart(string userId)
        {
            try
            {
                Order orderFromDb = await _ctx.Orders
                        .Include(o => o.Tickets)
                        .ThenInclude(o => o.TicketCategory)
                        .Include(o => o.Tickets)
                        .ThenInclude(o => o.Event)  
                        .FirstOrDefaultAsync(o => o.UserId == userId && o.OrderState == OrderState.PENDING);
                

                if (orderFromDb == null)
                    return new RepositoryActionResult<Order>(orderFromDb, RepositoryActionStatus.NotFound);
                else
                    return new RepositoryActionResult<Order>(orderFromDb, RepositoryActionStatus.Ok);

            }
            catch (Exception)
            {
                return new RepositoryActionResult<Order>(null, RepositoryActionStatus.Error);
            }
        }

        public async Task<RepositoryActionResult<Order>> GetLastOrder(string userId)
        {
            try
            {
                Order orderFromDb = await _ctx.Orders
                    .Include(o => o.Tickets)
                    .ThenInclude(o => o.TicketCategory)
                    .OrderBy( o => o.DateOpened)
                    .LastOrDefaultAsync(o => o.UserId == userId);


                if (orderFromDb == null)
                    return new RepositoryActionResult<Order>(orderFromDb, RepositoryActionStatus.NotFound);
                else
                    return new RepositoryActionResult<Order>(orderFromDb, RepositoryActionStatus.Ok);

            }
            catch (Exception)
            {
                return new RepositoryActionResult<Order>(null, RepositoryActionStatus.Error);
            }
        }

        public async Task<RepositoryActionResult<IEnumerable<Order>>> GetAllOrderHistoryFromUser(string userId)
        {
            try
            {
                IEnumerable<Order> orderFromDb = await _ctx.Orders
                .Where(o => o.UserId == userId && o.OrderState != OrderState.PENDING)
                .OrderBy(o => o.DateOpened)
                .AsNoTracking()
                .ToListAsync();

                if (orderFromDb == null)
                    return new RepositoryActionResult<IEnumerable<Order>>(orderFromDb, RepositoryActionStatus.NotFound);
                else
                    return new RepositoryActionResult<IEnumerable<Order>>(orderFromDb, RepositoryActionStatus.Ok);
            }
            catch (Exception)
            {
                return new RepositoryActionResult<IEnumerable<Order>>(null, RepositoryActionStatus.Error);
            }            
        }


        public async Task<RepositoryActionResult<IEnumerable<Order>>> GetAllOrderHistoryFromUser(string userId, ResourceParameters parameters)
        {
            IEnumerable<Order> orderHistory = await _ctx.Orders
                .Where(o => o.UserId == userId)
                .OrderBy(e => e.OrderId)
                .Skip(parameters.PageSize
                * (parameters.PageNumber - 1))
                .Take(parameters.PageSize)
                .ToListAsync();

            if (orderHistory == null)
                return new RepositoryActionResult<IEnumerable<Order>>(orderHistory, RepositoryActionStatus.NotFound);
            else
                return new RepositoryActionResult<IEnumerable<Order>>(orderHistory, RepositoryActionStatus.Ok);
        }

        public async Task<int> GetOrderHistoryTotalCount(string userId)
        {
            return await _ctx.Orders
                .Where(o => o.UserId == userId)
                .CountAsync();
        }
        
    }
}