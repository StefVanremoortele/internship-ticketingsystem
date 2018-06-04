using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Ticketingsystem.DAL.Core;
using Ticketingsystem.DAL.Utilities;
using Ticketingsystem.Domain.Helpers;
using Ticketingsystem.Domain.Interfaces;
using Ticketingsystem.Domain.Interfaces.Services;
using Ticketingsystem.Domain.Models;

namespace Ticketingsystem.DAL.Services
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IQuartzService _quartzService;
        private readonly IScheduler _quartzScheduler;

        public EventService(IUnitOfWork unitOfWork /*IQuartzService quartzService*/, IScheduler scheduler)
        {
            _unitOfWork = unitOfWork;
            //_quartzService = quartzService;
            _quartzScheduler = scheduler;
        }

        public async Task<RepositoryActionResult<Event>> GetEvent(int eventId)
        {
            return await _unitOfWork.Events.GetAsync(eventId);
        }

        public async Task<RepositoryActionResult<IEnumerable<Event>>> GetAllEvents()
        {
            //QuartzServicesUtilities.StartJob<CartExpireJob>(_quartzScheduler, new TimeSpan(0, 0, 0, 10), "20");

            return await _unitOfWork.Events.GetAllAsync();
        }

        public async Task<RepositoryActionResult<IEnumerable<Event>>> GetAllEvents(ResourceParameters eventsResourceParameters)
        {
            return await _unitOfWork.Events.GetEvents(eventsResourceParameters);
        }        

        public async Task<RepositoryActionResult<Event>> CreateEvent(Event eventToCreate)
        {
            _unitOfWork.Events.AddAsync(eventToCreate);
                                  
            if (await _unitOfWork.SaveChangesAsync())
                return new RepositoryActionResult<Event>(eventToCreate, RepositoryActionStatus.Ok);            
            else
                return new RepositoryActionResult<Event>(eventToCreate, RepositoryActionStatus.Error);            
        }
    }
}
