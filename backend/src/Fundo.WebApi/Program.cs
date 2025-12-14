using Fundo.Application.DependencyInjections;
using Fundo.Infrastructure.Persistence.DependencyInjections;
using Fundo.Infrastructure.Persistence.Seed;
using Fundo.WebApi.Extensions;
using Fundo.WebApi.Extensions.Swagger;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerCustom();
builder.Services.AddPersistence(builder.Configuration, builder.Environment.IsDevelopment());
builder.Services.AddApplication();
builder.Host.AddSerilog();

builder.Services.AddCorsPolicy("AngularDev");
var app = builder.Build();

app.UseSwaggerCustom();

app.UseCors("AngularDev");
app.UseCustomMiddleware();
app.UseSerilogRequestLogging();
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