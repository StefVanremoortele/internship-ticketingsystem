using System;
using Quartz;
using Quartz.Simpl;
using Quartz.Spi;

namespace Ticketingsystem.DAL.Utilities
{
    public class JobFactory : PropertySettingJobFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public JobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var jobDetail = bundle.JobDetail;

            var job = (IJob)_serviceProvider.GetService(jobDetail.JobType);
            return job;
        }

        public override void ReturnJob(IJob job) { }
    }
}
