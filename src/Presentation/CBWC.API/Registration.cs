using Asp.Versioning;
using CBWC.API.Infrastructure;
using Correlate.DependencyInjection;

namespace CBWC.API;

public static class Registration
{
    #region Api Versioning

    public static void AddApiVersionService(this IServiceCollection services)
    {
        services.AddApiVersioning(
                 options =>
                 {
                     options.DefaultApiVersion = new ApiVersion(1);
                     options.AssumeDefaultVersionWhenUnspecified = true;
                     options.ReportApiVersions = true;
                 })
             .AddApiExplorer(
                 options => options.SubstituteApiVersionInUrl = true);
    }

    #endregion

    #region Global Exception Handler

    public static void AddGlobalExceptionService(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
    }

    #endregion

    #region Security Headers

    public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app)
    {
        return app.Use(
            async (context, next) =>
            {
                //Must register the event before calling the next middleware
                context.Response.OnStarting(
                    () =>
                    {
                        //https://cheatsheetseries.owasp.org/cheatsheets/HTTP_Headers_Cheat_Sheet.html
                        context.Response.Headers.XFrameOptions = "DENY";
                        context.Response.Headers.XXSSProtection = "0";
                        context.Response.Headers.XContentTypeOptions = "nosniff";
                        context.Response.Headers.ContentSecurityPolicy =
                            "default-src 'self';"; //might be meaningless in APIs https://cheatsheetseries.owasp.org/cheatsheets/HTTP_Headers_Cheat_Sheet.html#content-security-policy-csp
                        context.Response.Headers["Referer-Policy"] = "strict-origin-when-cross-origin";
                        return Task.CompletedTask;
                    });

                await next(context);
            });
    }


    #endregion

    #region Correlation

    public static IHostApplicationBuilder ConfigureCorrelate(this IHostApplicationBuilder builder)
    {
        builder.Services.AddCorrelate(
            options =>
            {
                options.RequestHeaders = ["X-Correlation-ID"];
                options.IncludeInResponse = true;
            });
        return builder;
    }

    #endregion

}
