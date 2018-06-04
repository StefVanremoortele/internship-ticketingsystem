using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Ticketingsystem.Logging.Middleware
{
    public static class CustomExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(
            this IApplicationBuilder builder, string subject, string layer,
            string errorHandlingPath)
        {
            return builder.UseMiddleware<CustomExceptionHandlerMiddleware>
            (subject, layer, Options.Create(new ExceptionHandlerOptions
            {
                ExceptionHandlingPath = new PathString(errorHandlingPath)
            }));
        }
    }
}
