using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ticketingsystem.Logging
{
    public class TrackUsageAttribute : ActionFilterAttribute
    {
        private string _subject, _layer, _activityName;

        public TrackUsageAttribute(string subject, string layer, string activityName)
        {
            _subject = subject;
            _layer = layer;
            _activityName = activityName;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var dict = new Dictionary<string, object>();
            foreach (var key in context.RouteData.Values?.Keys)
                dict.Add($"RouteData-{key}", (string)context.RouteData.Values[key]);

            WebHelper.LogWebUsage(_subject, _layer, _activityName, context.HttpContext, dict);
        }
    }
}
