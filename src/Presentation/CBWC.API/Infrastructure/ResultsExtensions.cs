using System.Net.Mime;

namespace CBWC.API.Infrastructure;

static class ResultsExtensions
{
    public static IResult StatusCodeWithResponse(
        this IResultExtensions resultExtensions,
        int statusCode,
        object response)
    {
        ArgumentNullException.ThrowIfNull(resultExtensions);
        return new StatusCodeResult(statusCode, response);
    }
}

class StatusCodeResult(int statusCode, object response) : IResult
{
    private readonly int _statusCode = statusCode;
    private readonly object _response = response;

    public Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.ContentType = MediaTypeNames.Application.Json;
        httpContext.Response.StatusCode = _statusCode;
        return httpContext.Response.WriteAsJsonAsync(_response);
    }
}
