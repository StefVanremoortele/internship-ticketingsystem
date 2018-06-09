using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketingsystem.Domain.Helpers;
using Ticketingsystem.Domain.Interfaces;
using Ticketingsystem.Domain.Interfaces.Services;
using Ticketingsystem.Domain.Models;

namespace Ticketingsystem.DAL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        private void CreateTicketHistory(Order order, Ticket ticket, TicketAction ticketAction, DateTime timestamp)
        {
            TicketHistory history = new TicketHistory { Order = order, Ticket = ticket, TicketAction = ticketAction, Timestamp = timestamp };
            _unitOfWork.TicketHistory.AddAsync(history);
        }

        public async Task<RepositoryActionResult<Order>> GetCart(string userId)
        {
            return await _unitOfWork.Orders.GetCart(userId);
        }

        public async Task<RepositoryActionResult<Order>> GetLastOrder(string userId)
        {
            return await _unitOfWork.Orders.GetLastOrder(userId);
        }

        public async Task<RepositoryActionResult<Order>> CompleteOrder(string userId, int orderId)
        {
            // get the cart
            RepositoryActionResult<Order> cartResult = await _unitOfWork.Orders.GetAsync(orderId);
            if (cartResult.Status == RepositoryActionStatus.Error)
                return new RepositoryActionResult<Order>(null, RepositoryActionStatus.Error);

            // change orderstate and timestamp if this order is a cart
            Order order = cartResult.Entity;
            if (order.OrderState != OrderState.PENDING)
                return new RepositoryActionResult<Order>(null, RepositoryActionStatus.BadRequest);

           
            // check if the cart belongs to the requesting user
            if (order.UserId != userId) // TODO: implement userManager
                return new RepositoryActionResult<Order>(null, RepositoryActionStatus.BadRequest);

            order.OrderState = OrderState.COMPLETE;
            order.DateClosed = DateTime.Now.ToUniversalTime();


            // change ticketstatus and timestamp
            RepositoryActionResult<IEnumerable<Ticket>> ticketsResult = await _unitOfWork.Tickets.GetTicketsFromOrder(order.OrderId);
            IEnumerable<Ticket> tickets = ticketsResult.Entity;            

            // change ticket properties and create history
            foreach (var ticket in tickets)
            {
                ticket.TicketStatus = TicketStatus.SOLD;
                ticket.LastModified = DateTime.Now.ToUniversalTime();
                CreateTicketHistory(order, ticket, TicketAction.SOLD, ticket.LastModified);
            }
            

            // save changes
            var saveResult = await _unitOfWork.SaveChangesAsync();
            if (saveResult)
                return new RepositoryActionResult<Order>(order, RepositoryActionStatus.Ok);
            else
                return new RepositoryActionResult<Order>(null, RepositoryActionStatus.Error);
        }

        public async Task<RepositoryActionResult<Order>> CancelCart(string userId)
        {
            var cancelation_timestamp = DateTime.Now.ToUniversalTime();

            // get the cart and change orderstate and timestamp
            RepositoryActionResult<Order> cartResult = await _unitOfWork.Orders.GetCart(userId);

            // change orderstate and timestamp if this order is a cart
            Order order = cartResult.Entity;
            if (order == null || order.OrderState != OrderState.PENDING)
                return new RepositoryActionResult<Order>(null, RepositoryActionStatus.BadRequest);

            order.OrderState = OrderState.CANCELED;
            order.DateClosed = cancelation_timestamp;


            // change ticketstatus and timestamp
            RepositoryActionResult<IEnumerable<Ticket>> ticketsResult = await _unitOfWork.Tickets.GetTicketsFromOrder(order.OrderId);
            IEnumerable<Ticket> tickets = ticketsResult.Entity;

            // change ticket properties and log history
            foreach (var ticket in tickets)
            {
                ticket.TicketStatus = TicketStatus.AVAILABLE;
                ticket.Order = null;
                ticket.LastModified = cancelation_timestamp;
                CreateTicketHistory(order, ticket, TicketAction.RELEASED, cancelation_timestamp);
            }
            
            // save changes
            var saveResult = await _unitOfWork.SaveChangesAsync();
            if (saveResult)
                return new RepositoryActionResult<Order>(order, RepositoryActionStatus.Ok);
            else
                return new RepositoryActionResult<Order>(null, RepositoryActionStatus.Error);
        }
        public async Task<RepositoryActionResult<Order>> ExpireOrder(string userId)
        {

            RepositoryActionResult<User> userResult = await _unitOfWork.Users.GetUserById(userId);
            User user = userResult.Entity;

            RepositoryActionResult<Order> cartResult = await _unitOfWork.Orders.GetCart(user.UserId);

            // change orderstate and timestamp if this order is a cart
            Order order = cartResult.Entity;
            if (order == null || order.OrderState != OrderState.PENDING)
                return new RepositoryActionResult<Order>(null, RepositoryActionStatus.BadRequest);

            order.OrderState = OrderState.EXPIRED;
            order.DateClosed = DateTime.Now.ToUniversalTime();


            // change ticketstatus and timestamp
            RepositoryActionResult<IEnumerable<Ticket>> ticketsResult = await _unitOfWork.Tickets.GetTicketsFromOrder(order.OrderId);
            IEnumerable<Ticket> tickets = ticketsResult.Entity;

            // change ticket properties and log history
            foreach (var ticket in tickets)
            {
                ticket.TicketStatus = TicketStatus.AVAILABLE;
                ticket.Order = null;
                ticket.LastModified = DateTime.Now.ToUniversalTime();
                CreateTicketHistory(order, ticket, TicketAction.RELEASED, DateTime.Now.ToUniversalTime());
            }

            // save changes
            var saveResult = await _unitOfWork.SaveChangesAsync();
            if (saveResult)
                return new RepositoryActionResult<Order>(order, RepositoryActionStatus.Ok);
            else
                return new RepositoryActionResult<Order>(null, RepositoryActionStatus.Error);
        }

        public Task<RepositoryActionResult<IEnumerable<Order>>> GetAllOrders()
        {
            return _unitOfWork.Orders.GetAllAsync();
        }
        public async Task<RepositoryActionResult<IEnumerable<Order>>> GetAllOrderHistory(string userId)
        {
            return await _unitOfWork.Orders.GetAllOrderHistoryFromUser(userId);
        }
        public async Task<RepositoryActionResult<Ticket>> CancelTicketFromOrder(string userId, int ticketId)
        {
            var cancelation_timestamp = DateTime.Now.ToUniversalTime();
            //check if the user exists
            RepositoryActionResult<User> result = await _unitOfWork.Users.GetUserById(userId);
            User user = result.Entity;
            if (user == null)
                return new RepositoryActionResult<Ticket>(null, RepositoryActionStatus.BadRequest);

            // return an error if the user doesn't have a Cart
            RepositoryActionResult<Order> orderResult = await _unitOfWork.Orders.GetCart(userId);
            Order cart = orderResult.Entity;

            if (cart == null || cart.OrderState != OrderState.PENDING)
                return new RepositoryActionResult<Ticket>(null, RepositoryActionStatus.BadRequest);

            // return not found if the ticket isn't in the cart
            Ticket ticketFromCart = cart.Tickets.FirstOrDefault(t => t.TicketId == ticketId);
            if (ticketFromCart == null)
                return new RepositoryActionResult<Ticket>(null, RepositoryActionStatus.BadRequest);

            // remove the ticket from the cart
            cart.Tickets.Remove(ticketFromCart);

            // cancel the order if it's empty
            if (!cart.Tickets.Any())
            {
                cart.OrderState = OrderState.CANCELED;
                cart.DateClosed = cancelation_timestamp;
            }

            // set ticket available
            ticketFromCart.TicketStatus = TicketStatus.AVAILABLE;
            ticketFromCart.LastModified = cancelation_timestamp;

            // create history
            CreateTicketHistory(cart, ticketFromCart, TicketAction.RELEASED, cancelation_timestamp);
            // check for sql errors
            if (await _unitOfWork.SaveChangesAsync())
                return new RepositoryActionResult<Ticket>(ticketFromCart, RepositoryActionStatus.Ok);
            else
                return new RepositoryActionResult<Ticket>(ticketFromCart, RepositoryActionStatus.Error);
        }


        public async Task<RepositoryActionResult<IEnumerable<Order>>> GetAllOrderHistory(string userId, ResourceParameters parameters)
        {
            return await _unitOfWork.Orders.GetAllOrderHistoryFromUser(userId, parameters);
        }
        public async Task<int> GetOrderHistoryTotalCount(string userId)
        {
            return await _unitOfWork.Orders.GetOrderHistoryTotalCount(userId);
        }

    }
}
