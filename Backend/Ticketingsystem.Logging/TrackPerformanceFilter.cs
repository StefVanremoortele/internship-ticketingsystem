using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ticketingsystem.Logging
{
    public class TrackPerformanceFilter : IAsyncActionFilter
    {
        private PerfTracker _tracker;
        private string _subject, _layer;
        public TrackPerformanceFilter(string subject, string layer)
        {
            _subject = subject;
            _layer = layer;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _tracker?.Stop();
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var request = context.HttpContext.Request;
            var activity = $"{request.Path}-{request.Method}";

            var dict = new Dictionary<string, object>();
            foreach (var key in context.RouteData.Values?.Keys)
                dict.Add($"RouteData-{key}", (string)context.RouteData.Values[key]);

            var details = WebHelper.GetWebLogDetail(_subject, _layer, activity,
                context.HttpContext, dict);

            _tracker = new PerfTracker(details);

            await next();


            _tracker?.Stop();
        }
    }
}
