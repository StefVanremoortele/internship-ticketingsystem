using System;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Ticketingsystem.Core;
using Ticketingsystem.Utilities;

namespace Ticketingsystem.Helpers
{
    public static class QuartzExtensions
    {
        public static void UseQuartz(this IApplicationBuilder app)
        {
            app.ApplicationServices.GetService<IScheduler>();
        }

        public static void AddQuartz(this IServiceCollection services, params Type[] jobs)
        {
            var properties = new NameValueCollection
            {
                // json serialization is the one supported under .NET Core (binary isn't)
                ["quartz.serializer.type"] = "json",

                // the following setup of job store is just for example and it didn't change from v2
                ["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz",
                ["quartz.jobStore.useProperties"] = "false",
                ["quartz.jobStore.dataSource"] = "default",
                ["quartz.jobStore.tablePrefix"] = "QRTZ_",
                ["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.SqlServerDelegate, Quartz",
                ["quartz.dataSource.default.provider"] = "SqlServer", // SqlServer-41 is the new provider for .NET Core
                ["quartz.dataSource.default.connectionString"] = "Server=clyde542\\SQLEXPRESS;Database=QuartzDb;Initial Catalog=QuartzDb;Trusted_Connection=True;MultipleActiveResultSets=true"

            };

            services.AddSingleton<IJobFactory, JobFactory>();
            services.Add(jobs.Select(jobType => new ServiceDescriptor(jobType, jobType, ServiceLifetime.Singleton)));

            services.AddTransient(provider =>
            {
                var schedulerFactory = new StdSchedulerFactory();
                var scheduler = schedulerFactory.GetScheduler().Result;
                scheduler.JobFactory = provider.GetService<IJobFactory>();
                scheduler.Start().GetAwaiter();
                return scheduler;
            });

        }
    }
}
