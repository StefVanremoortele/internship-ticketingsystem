using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Quartz;
using Ticketingsystem.Core;
using Ticketingsystem.Domain.Helpers;
using Ticketingsystem.Domain.Interfaces.Services;
using Ticketingsystem.Domain.Models;
using Ticketingsystem.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ticketingsystem.Controllers
{
    /// <summary>
    /// The controller that handles all requests concerning tickets
    /// </summary>
    [EnableCors("CorsPolicy")]
    public class TicketsController : Controller
    {
        private readonly ITicketService _ticketService;
        private readonly IUserService _userService;
        private readonly IEventService _eventService;
        private readonly IOrderService _orderService;
        private readonly IHubContext<StockHub> _hubContext;
        private readonly IScheduler _scheduler;


        /// <summary>
        /// The ticketsController constructor
        /// </summary>
        public TicketsController(ITicketService ticketService, IUserService userService, 
                        IEventService eventService, IOrderService orderService, 
                        IHubContext<StockHub> hubcontext, IScheduler scheduler)
        {
            _ticketService = ticketService;
            _userService = userService;
            _eventService = eventService;
            _orderService = orderService;
            _hubContext = hubcontext;
            _scheduler = scheduler; 
        }



        /// <summary>
        /// Gets a specific Ticket.
        /// </summary>
        /// <param name="ticketId">The ticket ID</param>   
        /// <returns>a specific ticket</returns>
        [Authorize(Policy = "administratorPolicy")]
        [HttpGet("api/tickets/{ticketId}", Name = "GetTicket")]
        [ProducesResponseType(typeof(DTO.Tickets.Ticket), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTicket(int ticketId)
        {
            RepositoryActionResult<Ticket> result = await _ticketService.GetTicket(ticketId);

            if (result.Status == RepositoryActionStatus.Error)
                return StatusCode(StatusCodes.Status500InternalServerError);

            if (result.Status == RepositoryActionStatus.NotFound)
                return StatusCode(StatusCodes.Status404NotFound);

            var entities = Mapper.Map<DTO.Tickets.Ticket>(result.Entity);
            return StatusCode(StatusCodes.Status200OK, entities);
        }


        /// <summary>
        /// Gets all tickets for a specific event by page
        /// </summary>
        /// <param name="eventId">The event ID</param>
        /// <param name="ticketsResourceParameters">Optional parameters</param>
        /// <returns>http statuscode</returns>
        [AllowAnonymous]
        [HttpGet("api/events/{eventId}/ticketsByPage", Name = "GetTicketsForEventByPage")]
        [ProducesResponseType(typeof(IEnumerable<DTO.Tickets.Ticket>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTicketsForEventByPage(int eventId, TicketsResourceParameters ticketsResourceParameters)
        {
            RepositoryActionResult<IEnumerable<Ticket>> result = await _ticketService.GetTicketsFromEvent(eventId, ticketsResourceParameters);

            if (result.Status == RepositoryActionStatus.Error)
                return StatusCode(StatusCodes.Status500InternalServerError);

            if (result.Status == RepositoryActionStatus.NotFound)
                return StatusCode(StatusCodes.Status404NotFound);

            var entities = Mapper.Map<IEnumerable<DTO.Tickets.Ticket>>(result.Entity);
            return StatusCode(StatusCodes.Status200OK, entities);
        }


        /// <summary>
        /// Gets all tickets for a specific event.
        /// </summary>
        /// <param name="eventId">The event ID</param>
        /// <returns>http statuscode</returns>
        [AllowAnonymous]
        [HttpGet("api/events/{eventId}/tickets", Name = "GetTicketsForEvent")]
        [ProducesResponseType(typeof(IEnumerable<DTO.Tickets.Ticket>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTicketsForEvent(int eventId)
        {
            RepositoryActionResult<IEnumerable<Ticket>> result = await _ticketService.GetAllTicketsFromEvent(eventId);

            if (result.Status == RepositoryActionStatus.Error)
                return StatusCode(StatusCodes.Status500InternalServerError);

            if (result.Status == RepositoryActionStatus.NotFound)
                return StatusCode(StatusCodes.Status404NotFound);

            var entities = Mapper.Map<IEnumerable<DTO.Tickets.Ticket>>(result.Entity);
            return StatusCode(StatusCodes.Status200OK, entities);
        }


        /// <summary>
        /// Gets history of a specific Ticket.
        /// </summary>
        /// <param name = "ticketId" > The ticket ID</param>   
        /// <returns>a specific ticket</returns>
        [Authorize(Policy = "customerPolicy")]
        [HttpGet("api/tickets/{ticketId}/history", Name = "GetTicketHistory")]
        [ProducesResponseType(typeof(IEnumerable<DTO.TicketHistory.TicketHistory>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHistory(int ticketId)
        {
            RepositoryActionResult<IEnumerable<TicketHistory>> result = await _ticketService.GetTicketHistory(ticketId);

            if (result.Status == RepositoryActionStatus.Error)
                return StatusCode(StatusCodes.Status500InternalServerError);

            if (result.Status == RepositoryActionStatus.NotFound)
                return StatusCode(StatusCodes.Status404NotFound);

            var entities = Mapper.Map<IEnumerable<DTO.TicketHistory.TicketHistory>>(result.Entity);
            return StatusCode(StatusCodes.Status200OK, entities);
        }


        /// <summary>
        /// Gets all ticket from a specific order.
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <returns>http statuscode</returns>
        [Authorize(Policy = "customerPolicy")]
        [HttpGet("api/account/orders/{orderId}/tickets", Name = "GetTicketsFromOrder")]
        [ProducesResponseType(typeof(IEnumerable<DTO.Tickets.Ticket>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTicketsFromOrder(int orderId)
        {
            RepositoryActionResult<IEnumerable<Ticket>> result = await _ticketService.GetTicketsFromOrder(orderId);

            if (result.Status == RepositoryActionStatus.Error)
                return StatusCode(StatusCodes.Status500InternalServerError);

            if (result.Status == RepositoryActionStatus.NotFound)
                return StatusCode(StatusCodes.Status404NotFound);

            var entities = Mapper.Map<IEnumerable<DTO.Tickets.Ticket>>(result.Entity);
            return StatusCode(StatusCodes.Status200OK, entities);
        }


        /// <summary>
        /// Gets the amount of tickets from a specific specific
        /// </summary>
        /// <param name="eventId">The event ID</param>
        /// <returns>http statuscode</returns>
        [AllowAnonymous]
        [HttpGet("api/events/{eventId}/tickets/stock", Name = "GetAmountOfTickets")]
        [ProducesResponseType(typeof(IEnumerable<DTO.Stock.Stock>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetStock(int eventId)
        {
            RepositoryActionResult<IEnumerable<Stock>> categoriesResult = await _ticketService.GetAvailableStock(eventId);

            var stock = Mapper.Map<IEnumerable<DTO.Stock.Stock>>(categoriesResult.Entity);

            return StatusCode(StatusCodes.Status200OK, stock);
        }



        /// <summary>
        /// Gets all bought tickets from a specific user.
        /// </summary>
        /// <returns>http statuscode</returns>
        [Authorize(Policy = "customerPolicy")]
        [HttpGet("api/account/tickets", Name = "GetBoughtTicketsFromUser")]
        [ProducesResponseType(typeof(IEnumerable<DTO.Tickets.BoughtTicket>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBoughtTickets()
        {
            var userId = User.FindFirst("sub").Value;

            // this should be redundant due to authorize attribute (todo: check to remove this)
            if (userId == null)
                return StatusCode(StatusCodes.Status400BadRequest, null);

            RepositoryActionResult<IEnumerable<Ticket>> boughtTicketsResult = await _ticketService.GetBoughtTicketsFromUser(userId);

            if (boughtTicketsResult.Status == RepositoryActionStatus.Error)
                return StatusCode(StatusCodes.Status500InternalServerError);

            if (boughtTicketsResult.Status == RepositoryActionStatus.NotFound)
                return StatusCode(StatusCodes.Status404NotFound);


            return StatusCode(StatusCodes.Status200OK,
                Mapper.Map<IEnumerable<DTO.Tickets.BoughtTicket>>(boughtTicketsResult.Entity));
        }


        /// <summary>
        /// Reserves tickets with a specific type
        /// </summary>
        /// <param name="eventId">The event ID</param>
        /// <param name="ticketType">The ticket type</param>
        /// <param name="amount">Amount of tickets to reserve</param>
        /// <returns>http statuscode</returns>
        /// 
        [Authorize(Policy = "customerPolicy")]
        [TrackUsage("Ticket", Constants.General.ApiName, "Reservation")]
        [HttpGet("api/events/{eventId}/tickets/reserve", Name = "ReserveTicket")]
        [ProducesResponseType(typeof(ICollection<DTO.Tickets.Ticket>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Reserve(int eventId, [FromQuery] string ticketType, [FromQuery] int amount = 1)
        {
            try
            {
                var userId = User.FindFirst("sub").Value;

                // this should be redundant due to authorize attribute (todo: check to remove this)
                if (userId == null)
                    return StatusCode(StatusCodes.Status400BadRequest, null);

                TicketType type;
                switch (ticketType.ToLower())
                {
                    case "early_bird":
                        type = TicketType.EARLY_BIRD;
                        break;
                    case "promo":
                        type = TicketType.PROMO;
                        break;
                    case "vip":
                        type = TicketType.VIP;
                        break;
                    case "group":
                        type = TicketType.GROUP;
                        break;
                    default:
                        return StatusCode(StatusCodes.Status418ImATeapot);
                }
                RepositoryActionResult<Event> eventResult = await _eventService.GetEvent(eventId);

                RepositoryActionResult<Ticket> ticketReservationResult = null;
                ICollection<Ticket> reservedTickets = new List<Ticket>();

                for (int orderedTicketsAmount = 0; orderedTicketsAmount < amount; orderedTicketsAmount++)
                {
                    ticketReservationResult = await _ticketService.ProcessReservation(userId, eventId, type);

                    // As soon as we get an error we cancel the reservation -- The tickets that are reserved until now are fully processed
                    if (ticketReservationResult.Status == RepositoryActionStatus.PartialContent)
                    {
                        if (orderedTicketsAmount > 0)
                            return StatusCode(StatusCodes.Status206PartialContent, "Order limit reached - We managed to reserve " + orderedTicketsAmount + " more tickets for you");

                        return StatusCode(StatusCodes.Status206PartialContent, "Order limit reached");
                    }

                    reservedTickets.Add(ticketReservationResult.Entity);
                }

                if (ticketReservationResult.Status == RepositoryActionStatus.Error)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Something bad happened with the database");


                int ticketsInStock = await _ticketService.GetAmountLeft(eventId, type);
                await _hubContext.Clients.Group(eventResult.Entity.Name + '-' + ticketType).SendAsync("StockUpdate", ticketsInStock.ToString(), ticketType);

                return StatusCode(StatusCodes.Status200OK, Mapper.Map<ICollection<DTO.Tickets.Ticket>>(reservedTickets));
            }
            catch (Exception ex)
            {
                //WebHelper.LogWebError("Ticketing", Constant.General.ApiName, ex, HttpContext);
                return null;
            }

        }

        /// <summary>
        /// Cancels a specific ticket from an order
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <param name="ticketId">The ticket ID</param>
        /// <returns>http statuscode</returns>
        [Authorize(Policy = "customerPolicy")]
        [TrackUsage("Ticket", Constants.General.ApiName, "Cancellation")]
        [HttpGet("api/account/cart/tickets/{ticketId}/cancel", Name = "CancelTicket")]
        [ProducesResponseType(typeof(IEnumerable<DTO.Tickets.Ticket>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CancelTicket(int ticketId)
        {
            var userId = User.FindFirst("sub").Value;

            // this should be redundant due to authorize attribute (todo: check to remove this)
            if (userId == null)
                return StatusCode(StatusCodes.Status400BadRequest, null);
            
            RepositoryActionResult<Ticket> result = await _orderService.CancelTicketFromOrder(userId, ticketId);

            if (result.Status == RepositoryActionStatus.Error)
                return StatusCode(StatusCodes.Status500InternalServerError);

            var canceledTicket = Mapper.Map<DTO.Tickets.Ticket>(result.Entity);
            return StatusCode(StatusCodes.Status200OK, canceledTicket);
        }


        /// <summary>
        /// Gets all ticket categories
        /// </summary>
        /// <returns>http statuscode</returns>
        [Authorize(Policy = "administratorPolicy")]
        [HttpGet("api/tickets/categories", Name = "GetTicketCategories")]
        [ProducesResponseType(typeof(IEnumerable<DTO.TicketCategories.TicketCategory>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            RepositoryActionResult<IEnumerable<TicketCategory>> result = await _ticketService.GetAllTicketCategories();

            if (result.Status == RepositoryActionStatus.Error)
            {
                WebHelper.LogWebError("Tickets", Constants.General.ApiName, result.Exception, HttpContext);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            if (result.Status == RepositoryActionStatus.NotFound)
                return StatusCode(StatusCodes.Status404NotFound);

            var entities = Mapper.Map<IEnumerable<DTO.TicketCategories.TicketCategory>>(result.Entity);
            return StatusCode(StatusCodes.Status200OK, entities);
        }
    }
}
