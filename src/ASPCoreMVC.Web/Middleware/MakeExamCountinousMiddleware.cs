using ASPCoreMVC.TCUEnglish.ExamLogs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPCoreMVC.Web.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class MakeExamCountinousMiddleware
    {
        private readonly RequestDelegate _next;

        public MakeExamCountinousMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IExamLogService examLogService)
        {
            var guid = examLogService.GetLastExamNotFinished();
            if (guid != null && guid != Guid.Empty)
            {
                var destinationUri = $"/exams/re-work/{guid}";
                if (!httpContext.Request.Path.StartsWithSegments("/exams", StringComparison.OrdinalIgnoreCase))
                    httpContext.Response.Redirect(destinationUri);
            }
            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MakeExamCountinousMiddlewareExtension
    {
        public static IApplicationBuilder UseExamContinousMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MakeExamCountinousMiddleware>();
        }
    }
}
