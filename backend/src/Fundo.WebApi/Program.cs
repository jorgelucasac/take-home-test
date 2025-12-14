using Fundo.Application.DependencyInjections;
using Fundo.Infrastructure.Persistence.DependencyInjections;
using Fundo.Infrastructure.Persistence.Seed;
using Fundo.WebApi.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddPersistence(builder.Configuration, builder.Environment.IsDevelopment());
builder.Services.AddApplication();

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        //.ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Application", "Fundo.WebApi")
        .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();

//app.UseAuthorization();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.MapControllers();

try
{
    await LoanSeed.EnsureSeededAsync(app.Services);
    await app.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"Unhandled WebApi exception: {ex.Message}");
}
finally
{
    Console.WriteLine("Application shutting down.");
}

public partial class Program
{ }