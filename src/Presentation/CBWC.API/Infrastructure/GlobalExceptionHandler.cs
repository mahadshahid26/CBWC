using Correlate;
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
        var correlationId = _correlationContextAccessor?.CorrelationContext?.CorrelationId;

        await httpContext.Response.WriteAsJsonAsync(Results.Problem("An unhandled exception occurred"), cancellationToken);

        return true;
    }
}
