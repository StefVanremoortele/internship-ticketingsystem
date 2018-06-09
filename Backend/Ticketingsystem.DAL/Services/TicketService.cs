using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketingsystem.DAL.Core;
using Ticketingsystem.DAL.Helpers;
using Ticketingsystem.DAL.Utilities;
using Ticketingsystem.Domain.Helpers;
using Ticketingsystem.Domain.Interfaces;
using Ticketingsystem.Domain.Interfaces.Services;
using Ticketingsystem.Domain.Models;


namespace Ticketingsystem.DAL.Services
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderService _orderService;
        private readonly IScheduler _quartzScheduler;

        public TicketService(IUnitOfWork unitOfWork, IOrderService orderService, IScheduler scheduler)
        {
            _unitOfWork = unitOfWork;
            _orderService = orderService;
            _quartzScheduler = scheduler;
        }
        
        private void AddTicketHistory(Order order, Ticket ticket, TicketAction ticketAction, DateTime timestamp)
        {
            TicketHistory history = new TicketHistory { Order = order, Ticket = ticket, TicketAction = ticketAction, Timestamp = timestamp };
            _unitOfWork.TicketHistory.AddAsync(history);
        }

        public async Task<RepositoryActionResult<Ticket>> GetTicket(int ticketId)
        {
            return await _unitOfWork.Tickets.GetAsync(ticketId);
        }

        public async Task<RepositoryActionResult<IEnumerable<Ticket>>> GetTicketsFromOrder(int orderId)
        {
            return await _unitOfWork.Tickets.GetTicketsFromOrder(orderId);
        }
        public async Task<RepositoryActionResult<IEnumerable<Ticket>>> GetBoughtTicketsFromUser(string userId)
        {
            RepositoryActionResult<User> result = await _unitOfWork.Users.GetUserById(userId);
            if (result.Entity == null)
                return new RepositoryActionResult<IEnumerable<Ticket>>(null, RepositoryActionStatus.NotFound);

            return await _unitOfWork.Tickets.GetBoughtTickets(userId);
        }
        public async Task<RepositoryActionResult<IEnumerable<Ticket>>> GetAllTicketsFromEvent(int eventId)
        {
            return await _unitOfWork.Tickets.GetTicketsFromEvent(eventId);
        }
        public async Task<RepositoryActionResult<IEnumerable<Ticket>>> GetTicketsFromEvent(int eventId, TicketsResourceParameters ticketsResourceParameters)
        {
            return await _unitOfWork.Tickets.GetTicketsFromEvent(ticketsResourceParameters, eventId);
        }
        public async Task<RepositoryActionResult<IEnumerable<TicketCategory>>> GetAllTicketCategories()
        {
            return await _unitOfWork.TicketCategory.GetAllAsync();
        }
        public async Task<RepositoryActionResult<IEnumerable<Stock>>> GetAvailableStock(int eventId)
        {
            return await _unitOfWork.Tickets.GetAvailableStock(eventId);
        }
        public async Task<RepositoryActionResult<IEnumerable<TicketHistory>>> GetTicketHistory(int ticketId)
        {
            return await _unitOfWork.TicketHistory.GetHistoryAsync(ticketId);
        }   
        public async Task<RepositoryActionResult<Ticket>> ProcessReservation(string userId, int eventId, TicketType ticketType)
        {
            // get request timestamp
            DateTime request_timestamp = DateTime.Now.ToUniversalTime();
            // get a random available ticket (UnTracked)
            RepositoryActionResult<Ticket> untrackedTicketResult = await _unitOfWork.Tickets.GetAvailableTicket(eventId, ticketType);
            Ticket untrackedTicket = untrackedTicketResult.Entity;
            // if the ticketStatus
            if (untrackedTicket.TicketStatus == TicketStatus.RESERVED || untrackedTicket.Order != null || untrackedTicket.LastModified > request_timestamp)
                return new RepositoryActionResult<Ticket>(null, RepositoryActionStatus.ConcurrencyConflict);

            // set status to RESERVED and save changes before continuing
            await _unitOfWork.Tickets.ChangeTicketStateAndSave(untrackedTicket.TicketId, TicketStatus.RESERVED);

            // if the user doesn't have a Cart, make a new one
            RepositoryActionResult<Order> orderResult = await _unitOfWork.Orders.GetCart(userId);
            Order cart = orderResult.Entity;
            if (cart == null)
            {
                cart = new Order { UserId = userId };
                _unitOfWork.Orders.Add(cart);

                QuartzServicesUtilities.StartJob<CartExpireJob>(_quartzScheduler, new TimeSpan(0, 0, 15, 0), userId);
            }


            // LIMIT REACHED
            if (!(cart.Tickets.Count() < 10))
            {
                await _unitOfWork.Tickets.ChangeTicketStateAndSave(untrackedTicket.TicketId, TicketStatus.AVAILABLE);
                return new RepositoryActionResult<Ticket>(null, RepositoryActionStatus.PartialContent);
            }

            // get
            RepositoryActionResult<Ticket> trackedTicketResult = await _unitOfWork.Tickets.GetAsync(untrackedTicket.TicketId);
            Ticket reservedTicket = trackedTicketResult.Entity;
            reservedTicket.LastModified = request_timestamp;

            // compare timestamp one more time
            // -- if it is newer than request timestamp, then somehow we didn't catch that RESERVED signal
            if (await _unitOfWork.Tickets.IsModified(untrackedTicket))
                return new RepositoryActionResult<Ticket>(reservedTicket, RepositoryActionStatus.ConcurrencyConflict);
            
            cart.Tickets.Add(reservedTicket);           

            // check for sql errors
            if (await _unitOfWork.SaveChangesAsync())
                return new RepositoryActionResult<Ticket>(reservedTicket, RepositoryActionStatus.Ok);
            else
                return new RepositoryActionResult<Ticket>(reservedTicket, RepositoryActionStatus.Error);
        }

        public async Task<int> GetAmountLeft(int eventId, TicketType type)
        {
            return await _unitOfWork.Tickets.GetAvailableTicketsAmount(eventId, type);
        }
        
    }
}
