using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace ContactMgmtAPI.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        public RequestDelegate _requestDelegate;
        private readonly Logger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware(RequestDelegate requestDelegate, Logger<ExceptionHandlingMiddleware> logger)
        {
            _requestDelegate = requestDelegate;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {

                //var ex = httpContext.Features.Get<IExceptionHandlerFeature>();
                //if (ex != null)
                //{
                //    HandleException(httpContext, );
                //}

                await _requestDelegate(httpContext);

            }
            catch (Exception ex)
            {
                HandleException(httpContext, ex);
            }
        }
        private Task HandleException(HttpContext httpContext, Exception ex)
        {
            _logger.LogError("HandleException: " + ex.Message + " - " + ex.StackTrace);
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return httpContext.Response.WriteAsync(ex.Message);
        }
    }
}
