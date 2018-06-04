using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;

namespace Ticketingsystem.Logging
{
    public static class WebHelper
    {
        public static void LogWebUsage(string subject, string layer, string activityName,
            HttpContext context, Dictionary<string, object> additionalInfo = null)
        {
            var details = GetWebLogDetail(subject, layer, activityName, context, additionalInfo);
            Logger.WriteUsage(details);
        }

        public static void LogWebDiagnostic(string subject, string layer, string message,
            HttpContext context, Dictionary<string, object> diagnosticInfo = null)
        {
            var details = GetWebLogDetail(subject, layer, message, context, diagnosticInfo);
            Logger.WriteDiagnostic(details);
        }
        public static void LogWebError(string subject, string layer, Exception ex,
            HttpContext context)
        {
            var details = GetWebLogDetail(subject, layer, null, context, null);
            details.Exception = ex;

            Logger.WriteError(details);
        }

        public static LogDetail GetWebLogDetail(string subject, string layer,
            string activityName, HttpContext context,
            Dictionary<string, object> additionalInfo = null)
        {
            var detail = new LogDetail
            {
                Subject = subject,
                Layer = layer,
                Message = activityName,
                Hostname = Environment.MachineName,
                CorrelationId = Activity.Current?.Id ?? context.TraceIdentifier,
                AdditionalInfo = additionalInfo ?? new Dictionary<string, object>()
            };

            GetUserData(detail, context);
            GetRequestData(detail, context);
            // Session data??
            // Cookie data??

            return detail;
        }

        private static void GetRequestData(LogDetail detail, HttpContext context)
        {
            var request = context.Request;
            if (request != null)
            {
                detail.Location = request.Path;

                detail.AdditionalInfo.Add("UserAgent", request.Headers["User-Agent"]);
                // non en-US preferences here??
                detail.AdditionalInfo.Add("Languages", request.Headers["Accept-Language"]);

                var qdict = Microsoft.AspNetCore.WebUtilities
                    .QueryHelpers.ParseQuery(request.QueryString.ToString());
                foreach (var key in qdict.Keys)
                {
                    detail.AdditionalInfo.Add($"QueryString-{key}", qdict[key]);
                }
            }
        }

        private static void GetUserData(LogDetail detail, HttpContext context)
        {
            var userId = "";
            var userName = "";
            var user = context.User;  // ClaimsPrincipal.Current is not sufficient
            if (user != null)
            {
                var i = 1; // i included in dictionary key to ensure uniqueness
                foreach (var claim in user.Claims)
                {
                    if (claim.Type == ClaimTypes.NameIdentifier)
                        userId = claim.Value;
                    else if (claim.Type == "name")
                        userName = claim.Value;
                    else
                        // example dictionary key: UserClaim-4-role 
                        detail.AdditionalInfo.Add($"UserClaim-{i++}-{claim.Type}", claim.Value);
                }
            }
            detail.UserId = userId;
            detail.UserName = userName;
        }

    }
}
