using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace CBWC.API.Infrastructure.Swagger;

public static class SwaggerSetupHelper
{
    public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder, string[]? endpointGroupNames = null)
    {
        endpointGroupNames ??= [];

        builder.Services.AddSwaggerGen(o =>
        {
            o.SwaggerDoc("all", new OpenApiInfo
            {
                Title = "All endpoints",
                Version = "v1"
            });

            foreach (var group in endpointGroupNames)
            {
                if (string.IsNullOrWhiteSpace(group))
                    continue;

                var name = group.ToLower();
                o.SwaggerDoc(name, new OpenApiInfo
                {
                    Title = group,
                    Version = "v1"
                });
            }

            o.DocInclusionPredicate((docName, apiDesc) =>
            {
                if (string.Equals(docName, "all", StringComparison.OrdinalIgnoreCase))
                    return true;

                if (!string.IsNullOrEmpty(apiDesc.GroupName) && string.Equals(apiDesc.GroupName, docName, StringComparison.OrdinalIgnoreCase))
                    return true;

                var relativePath = apiDesc.RelativePath ?? string.Empty;
                if (!string.IsNullOrEmpty(relativePath) && relativePath.IndexOf($"/{docName}", StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;

                return false;
            });

            o.ExampleFilters();
        });

        builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

        builder.Services.AddFluentValidationRulesToSwagger();

        return builder;
    }

    public static void ConfigureSwaggerUi(
        this SwaggerUIOptions options, string[]? endpointGroupNames = null)
    {
        endpointGroupNames ??= [];

        options.DisplayOperationId();

        options.SwaggerEndpoint($"/swagger/all/swagger.json", "All endpoints");

        // Expose group-specific documents too
        foreach (var group in endpointGroupNames)
        {
            if (string.IsNullOrWhiteSpace(group))
                continue;

            var name = group.ToLower();
            options.SwaggerEndpoint($"/swagger/{name}/swagger.json", group);
        }
    }
}
