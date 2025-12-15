using CBWC.API;
using CBWC.API.App;
using CBWC.API.App.EndPoints;
using CBWC.API.Infrastructure.Swagger;
using CBWC.Application;
using CBWC.Infrastructure.Persistence;
using Correlate.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json.Serialization;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options => options.AddServerHeader = false);

builder.ConfigureCorrelate();
builder.AddFluentValidationEndpointFilter();
builder.AddSwagger(EndpointGroupNames.All);
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()));

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiVersionService();
builder.Services.ConfigureHttpJsonOptions(
     options => options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.Configure<JsonOptions>(
    options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddProblemDetails();

builder.Services.AddGlobalExceptionService();
builder.Services.AddApplicationServicesForApi(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler();
app.UseCors();
app.UseDeveloperExceptionPage();

app.UseSecurityHeaders();
app.UseCorrelate();
//app.UseAuthentication();
//app.UseAuthorization();

app.MapMemberEndpoints();

app.UseSwagger();
app.UseSwaggerUI(x => x.ConfigureSwaggerUi(EndpointGroupNames.All));

await app.RunAsync();
