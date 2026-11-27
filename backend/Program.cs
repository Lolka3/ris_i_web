using backend.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using backend.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException(
        "Connection string 'Default' is required. Configure it with user-secrets or an environment variable.");

builder.Logging.AddFilter("Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware", LogLevel.None);
builder.Services.AddControllers();
builder.Services.AddSingleton<IHealthService, HealthService>();
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddScoped<RetakeRequestService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        var logger = context.RequestServices
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger("UnhandledRequest");
        logger.LogError(
            "Unhandled request failed; method={Method}; path={Path}; error={ErrorType}",
            context.Request.Method,
            context.Request.Path,
            exception?.GetType().Name ?? "Exception");

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Не удалось обработать запрос",
            Type = "about:blank"
        };
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/problem+json; charset=utf-8";
        var responseBody = JsonSerializer.SerializeToUtf8Bytes(
            problemDetails,
            new JsonSerializerOptions(JsonSerializerDefaults.Web));
        await context.Response.Body.WriteAsync(responseBody);
    });
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/health", () => Results.Ok(new { status = "Healthy" }));

app.MapGet("/info", () => Results.Ok(new 
{
    application = "Retake Requests API",
    environment = app.Environment.EnvironmentName,
    version = "ЛР10"
}));

app.Run();
