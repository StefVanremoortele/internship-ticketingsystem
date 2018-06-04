using System;
using Quartz;
using Ticketingsystem.DAL.Core;

namespace Ticketingsystem.DAL.Utilities
{
    public class QuartzServicesUtilities
    {
        public static void StartJob<TJob>(IScheduler scheduler, TimeSpan runInterval, string userId)
            where TJob : IJob
        {
            var jobName = typeof(TJob).FullName; 

            // Add Cart Expiring Job
            IJobDetail job = JobBuilder.Create<CartExpireJob>()
                .WithIdentity("cart" + userId, "carts")
                .Build();

            //Trigger the job to run after 15 minutes
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("cartTrigger" + userId, "carts")
                .StartAt(DateBuilder.FutureDate(runInterval.Minutes, IntervalUnit.Minute))
                .Build();

            job.JobDataMap.Add("userId", userId);

            scheduler.ScheduleJob(job, trigger);
        }
    }
}
