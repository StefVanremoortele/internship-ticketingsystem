using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl.Matchers;
using Ticketingsystem.DAL.Core;
using Ticketingsystem.DAL.Helpers;
using Ticketingsystem.Domain.Interfaces.Services;
using Ticketingsystem.Domain.Models;

namespace Ticketingsystem.DAL.Services
{
    public class QuartzService : IQuartzService
    {
        private readonly IScheduler _quartzScheduler;
        private readonly IEventService _eventService;

        public QuartzService(IScheduler scheduler, IEventService eventService)
        {
            _quartzScheduler = scheduler;
            _eventService = eventService;
        }

        public void Queue(string userId)
        {

            // Add Cart Expiring Job
            IJobDetail job = JobBuilder.Create<CartExpireJob>()
            .WithIdentity("cart" + userId, "carts")
            .Build();

            //Trigger the job to run after 15 minutes
            ITrigger trigger = TriggerBuilder.Create()
               .WithIdentity("cartTrigger" + userId, "carts")
               //.StartNow()
               .StartAt(DateBuilder.FutureDate(5, IntervalUnit.Second))
               .Build();

            //job.JobDataMap.Put("count", "1");
            
            _quartzScheduler.ScheduleJob(job, trigger);

            ////CartExpireJobListener myJobListener = new CartExpireJobListener(cart.UserId);

            //_quartzScheduler.ListenerManager.AddJobListener(myJobListener, KeyMatcher<JobKey>.KeyEquals(new JobKey("CartJob_" + cart.OrderId)));
            
        }
    }
}
