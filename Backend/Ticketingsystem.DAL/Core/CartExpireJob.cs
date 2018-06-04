using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using Ticketingsystem.Domain.Helpers;
using Ticketingsystem.Domain.Interfaces.Services;
using Ticketingsystem.Domain.Models;

namespace Ticketingsystem.DAL.Core
{
    public class CartExpireJob : IJob
    {
        private readonly ILogger<CartExpireJob> _log;
        private readonly IOrderService _orderService;

        private readonly JobConfiguration _configuration;

        public string userId;

        public CartExpireJob(IOptions<JobConfiguration> configuration, ILogger<CartExpireJob> log, IOrderService orderService)
        {
            _log = log;
            _configuration = configuration.Value;
            _orderService = orderService;

        }


        private async Task Execute()
        {
            try
            {

                RepositoryActionResult<Order> order = await _orderService.ExpireOrder(userId);

                var test = 0;


            }
            catch (Exception ex)
            {
                _log.LogError(null, ex, "An error occurred during execution of scheduled job");
            }
        }

        public Task Execute(IJobExecutionContext context)
        {
            userId = (string)context.JobDetail.JobDataMap.Get("userId");
            return Task.Run(Execute);
        }
    }
}
