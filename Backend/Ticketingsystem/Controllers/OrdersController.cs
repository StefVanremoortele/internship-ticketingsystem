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
using Ticketingsystem.Domain.Helpers;
using Ticketingsystem.Domain.Interfaces.Services;
using Ticketingsystem.Domain.Models;
using Ticketingsystem.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ticketingsystem.Controllers
{
    /// <summary>
    /// The controller that handles all requests concerning orders
    /// </summary>
    [EnableCors("CorsPolicy")]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;

        /// <summary>
        /// The ordersController constructor
        /// </summary>
        public OrdersController(IOrderService orderService, IUserService userService)
        {
            _orderService = orderService;
            _userService = userService;
        }


        /// <summary> 
        /// Gets the current Cart from a user
        /// </summary>
        /// <returns>http statuscode</returns>
        [Authorize(Policy = "customerPolicy")]
        [HttpGet("api/account/cart", Name = "GetCart")]
        [ProducesResponseType(typeof(DTO.Carts.Cart), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCart()
        {
            var userId = User.FindFirst("sub").Value;

            // this should be redundant due to authorize attribute (todo: check to remove this)
            if (userId == null)
                return StatusCode(StatusCodes.Status400BadRequest, null);


            RepositoryActionResult<Order> result = await _orderService.GetCart(userId);

            if (result.Status == RepositoryActionStatus.Error)
                return StatusCode(StatusCodes.Status500InternalServerError);

            if (result.Status == RepositoryActionStatus.NotFound)
            {
                RepositoryActionResult<Order> lastCartResult = await _orderService.GetLastOrder(userId);
                var lastCart = Mapper.Map<DTO.Carts.Cart>(lastCartResult.Entity);
                return StatusCode(StatusCodes.Status404NotFound, lastCart);
            }

            var cart = Mapper.Map<DTO.Carts.Cart>(result.Entity);
            
            return StatusCode(StatusCodes.Status200OK, cart);
        }




        /// <summary>
        /// Gets all order history from a user
        /// </summary>
        /// <returns>http statuscode</returns>  
        [Authorize(Policy = "customerPolicy")]
        [HttpGet("api/account/orders/history", Name = "GetOrderHistory")]
        [ProducesResponseType(typeof(IEnumerable<DTO.Orders.Order>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrderHistory()
        {
            var userId = User.FindFirst("sub").Value;

            // this should be redundant due to authorize attribute (todo: check to remove this)
            if (userId == null)
                return StatusCode(StatusCodes.Status400BadRequest, null);

            RepositoryActionResult<IEnumerable<Order>> result = await _orderService.GetAllOrderHistory(userId);

            if (result.Status == RepositoryActionStatus.Error)
                return StatusCode(StatusCodes.Status500InternalServerError);
            
            if (result.Status == RepositoryActionStatus.NotFound)
                return StatusCode(StatusCodes.Status404NotFound);
            
            var orders = Mapper.Map<IEnumerable<DTO.Orders.Order>>(result.Entity);
            return StatusCode(StatusCodes.Status200OK, orders);
        }

        /// <summary>
        /// Gets all Order history from a user
        /// </summary>
        /// <returns>all order history</returns>
        [AllowAnonymous]
        [HttpGet("api/account/orders/historyByPage", Name = "GetOrderHistoryByPage")]
        [ProducesResponseType(typeof(DTO.Orders.PagedOrder), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrderHistoryByPage(ResourceParameters eventsResourceParemeters)
        {
            var userId = User.FindFirst("sub").Value;

            // this should be redundant due to authorize attribute (todo: check to remove this)
            if (userId == null)
                return StatusCode(StatusCodes.Status400BadRequest, null);

            RepositoryActionResult<IEnumerable<Order>> orderHistoryResult = await _orderService.GetAllOrderHistory(userId, eventsResourceParemeters);

            if (orderHistoryResult.Status == RepositoryActionStatus.Error)
                return StatusCode(StatusCodes.Status500InternalServerError);

            if (orderHistoryResult.Status == RepositoryActionStatus.NotFound)
                return StatusCode(StatusCodes.Status404NotFound);

            DTO.Orders.PagedOrder results = new DTO.Orders.PagedOrder();

            results.Orders = Mapper.Map<IEnumerable<DTO.Orders.Order>>(orderHistoryResult.Entity);
            results.Total_count = await _orderService.GetOrderHistoryTotalCount(userId);

            return StatusCode(StatusCodes.Status200OK, results);
        }

        /// <summary>
        /// Completes an order
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <returns>http statuscode</returns>  
        [Authorize(Policy = "customerPolicy")]
        [HttpGet("api/account/orders/{orderId}/complete", Name = "CompleteOrder")]
        [ProducesResponseType(typeof(DTO.Orders.Order), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status304NotModified)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [TrackUsage("Order", Constants.General.ApiName, "Completion")]
        public async Task<IActionResult> Complete(int orderId)
        {
            var userId = User.FindFirst("sub").Value;

            // this should be redundant due to authorize attribute (todo: check to remove this)
            if (userId == null)
                return StatusCode(StatusCodes.Status400BadRequest, null);

            RepositoryActionResult<Order> result = await _orderService.CompleteOrder(userId, orderId);

            if (result.Status == RepositoryActionStatus.Error)
                return StatusCode(StatusCodes.Status500InternalServerError);
            
            if (result.Status == RepositoryActionStatus.NotFound)
                return StatusCode(StatusCodes.Status404NotFound);

            if (result.Status == RepositoryActionStatus.NothingModified)
                return StatusCode(StatusCodes.Status304NotModified);
            
            var orders = Mapper.Map<DTO.Orders.Order>(result.Entity);
            return StatusCode(StatusCodes.Status200OK, orders);
        }


        /// <summary>
        /// Cancels an order
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <returns>http statuscode</returns>  
        [Authorize(Policy = "customerPolicy")]
        [HttpGet("api/account/cart/cancel", Name = "CancelCart")]
        [ProducesResponseType(typeof(DTO.Orders.Order), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [TrackUsage("Order", Constants.General.ApiName, "Cancellation")]
        public async Task<IActionResult> Cancel(int orderId)
        {
            var userId = User.FindFirst("sub").Value;

            // this should be redundant due to authorize attribute (todo: check to remove this)
            if (userId == null)
                return StatusCode(StatusCodes.Status400BadRequest, null);

            RepositoryActionResult<Order> result = await _orderService.CancelCart(userId);

            if (result.Status == RepositoryActionStatus.Error)
                return StatusCode(StatusCodes.Status500InternalServerError);

            if (result.Status == RepositoryActionStatus.NotFound)
                return StatusCode(StatusCodes.Status404NotFound);

            if (result.Status == RepositoryActionStatus.NothingModified)
                return StatusCode(StatusCodes.Status304NotModified);

            var orders = Mapper.Map<DTO.Orders.Order>(result.Entity);
            return StatusCode(StatusCodes.Status200OK, orders);
        }
    }
}
