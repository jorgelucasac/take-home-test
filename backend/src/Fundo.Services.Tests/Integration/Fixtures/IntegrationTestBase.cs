using Fundo.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Fundo.Services.Tests.Integration.Fixtures;

[Collection(nameof(DatabaseCollection))]
public abstract class IntegrationTestBase(SqlServerContainerFixture db) : IAsyncLifetime
{
    public HttpClient Client;
    private readonly SqlServerContainerFixture _db = db;
    protected CustomWebApplicationFactory Factory;
    protected AppDbContext TestDbContext;

    public Task InitializeAsync()
    {
        Factory = new CustomWebApplicationFactory(_db.ConnectionString);
        TestDbContext = Factory.Services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
        Client = Factory.CreateClient();
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        TestDbContext?.Dispose();
        Client.Dispose();
        Factory.Dispose();
        return Task.CompletedTask;
    }
}