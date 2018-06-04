using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using Ticketingsystem.Domain.Helpers;
using Ticketingsystem.Domain.Interfaces.Services;
using Ticketingsystem.Domain.Models;
using Ticketingsystem.DTO;
using Ticketingsystem.Logging;


namespace Ticketingsystem.Controllers
{

    /// <summary>
    /// The controller that handles all requests concerning events
    /// </summary>
    [Route("api/[controller]")]
    public class EventsController : Controller
    {
        private readonly IEventService _eventService;
        //private readonly IScheduler _scheduler;


        /// <summary>
        /// The eventsController constructor
        /// </summary>
        public EventsController(IEventService eventService /*IScheduler scheduler*/)
        { 
            _eventService = eventService;
        }
        
        /// <summary>
        /// Gets all Events.
        /// </summary>
        /// <returns>all events</returns>
        [AllowAnonymous]
        [HttpGet("byPage", Name = "GetEventsByPage")]
        [ProducesResponseType(typeof(IEnumerable<DTO.Events.Event>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEventsByPage(ResourceParameters eventsResourceParemeters)
        {
            RepositoryActionResult<IEnumerable<Event>> result = await _eventService.GetAllEvents(eventsResourceParemeters);

            if (result.Status == RepositoryActionStatus.Error)
                return StatusCode(StatusCodes.Status500InternalServerError);

            if (result.Status == RepositoryActionStatus.NotFound)
                return StatusCode(StatusCodes.Status404NotFound, "No events available at the moment");


            var events = Mapper.Map<IEnumerable<DTO.Events.Event>>(result.Entity);
            return StatusCode(StatusCodes.Status200OK, events);
        }


        /// <summary>
        /// Gets all Events.
        /// </summary>
        /// <returns>all events</returns>
        [AllowAnonymous]
        [HttpGet(Name = "GetEvents")]
        [ProducesResponseType(typeof(IEnumerable<DTO.Events.Event>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {

            RepositoryActionResult<IEnumerable<Event>> result = await _eventService.GetAllEvents();

            if (result.Status == RepositoryActionStatus.Error)
            {
                WebHelper.LogWebError("Events", Constants.General.ApiName, result.Exception, HttpContext);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            if (result.Status == RepositoryActionStatus.NotFound)
                return StatusCode(StatusCodes.Status404NotFound, "No events available at the moment");

            var events = Mapper.Map<IEnumerable<DTO.Events.Event>>(result.Entity);
            return StatusCode(StatusCodes.Status200OK, events);
        }


        /// <summary>
        /// Gets a specific Event.
        /// </summary>
        /// <param name="eventId">The event ID</param>   
        /// <returns>a specific event</returns>
        [AllowAnonymous]
        [HttpGet("{eventId}", Name = "GetEvent")]
        [ProducesResponseType(typeof(DTO.Events.Event), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int eventId)
        {
            RepositoryActionResult<Event> result = await _eventService.GetEvent(eventId);

            if (result.Status == RepositoryActionStatus.Error)
                return StatusCode(StatusCodes.Status500InternalServerError);

            if (result.Status == RepositoryActionStatus.NotFound)
                return StatusCode(StatusCodes.Status404NotFound);

            var eventToReturn = Mapper.Map<Ticketingsystem.DTO.Events.Event>(result.Entity);
            return CreatedAtRoute("GetEvent", new { eventId = eventToReturn.EventId }, eventToReturn);
        }


        /// <summary>
        /// Creates a new Event.
        /// </summary>
        /// <param name="eventToCreate">The new Event object</param>
        /// <returns>http statuscode</returns>
        [Authorize(Policy = "administratorPolicy")]
        [HttpPost(Name = "CreateEvent")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] DTO.Events.EventForCreation eventToCreate)
        {
            if (eventToCreate == null)
                return StatusCode(StatusCodes.Status400BadRequest);

            var eventEntity = Mapper.Map<Event>(eventToCreate);
            RepositoryActionResult<Event> result = await _eventService.CreateEvent(eventEntity);

            if (result.Status == RepositoryActionStatus.Error)
            {
                WebHelper.LogWebError("Events", Constants.General.ApiName, result.Exception, HttpContext);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
            var eventToReturn = Mapper.Map<DTO.Events.Event>(eventEntity);
            return CreatedAtRoute("GetEvent", new { eventId = eventEntity.EventId }, eventToReturn);
        }
    }
}
