using Correlate;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Diagnostics;

namespace CBWC.API.Infrastructure;

public class GlobalExceptionHandler(
    ICorrelationContextAccessor correlationContextAccessor) : IExceptionHandler
{
    private readonly ICorrelationContextAccessor _correlationContextAccessor = correlationContextAccessor;
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var exceptionTelemetry = new ExceptionTelemetry(exception);

        var correlationId = _correlationContextAccessor?.CorrelationContext?.CorrelationId;

        if (exceptionTelemetry is ISupportProperties supportProperties)
        {
            if (!string.IsNullOrEmpty(correlationId))
            {
                supportProperties.Properties["CorrelationId"] = correlationId;
            }

            supportProperties.Properties["RequestUrl"] = httpContext.Request.Path;
        }

        await httpContext.Response.WriteAsJsonAsync(Results.Problem("An unhandled exception occurred"), cancellationToken);

        return true;
    }
}
