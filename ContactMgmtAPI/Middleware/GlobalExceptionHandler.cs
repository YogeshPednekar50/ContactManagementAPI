using Microsoft.AspNetCore.Diagnostics;

namespace ContactMgmtAPI.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly Logger<GlobalExceptionHandler> _logger;
        public GlobalExceptionHandler( Logger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError("Unhandled error occured!", exception);
            await httpContext.Response.WriteAsync(exception.Message, cancellationToken);

            return true;

        }
    }
}
