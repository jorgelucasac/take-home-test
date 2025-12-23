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

    public Task InitializeAsync()
    {
        Factory = new CustomWebApplicationFactory(_db.ConnectionString);
        Client = Factory.CreateClient();
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        Client.Dispose();
        Factory.Dispose();
        return Task.CompletedTask;
    }
}