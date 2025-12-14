using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Fundo.Services.Tests.Integration.Fixtures;

[Collection(nameof(DatabaseCollection))]
public abstract class BaseControllerTest(SqlServerContainerFixture db) : IAsyncLifetime
{
    public HttpClient Client;
    private readonly SqlServerContainerFixture _db = db;
    private CustomWebApplicationFactory _factory;

    public Task InitializeAsync()
    {
        _factory = new CustomWebApplicationFactory(_db.ConnectionString);
        Client = _factory.CreateClient();
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        Client.Dispose();
        _factory.Dispose();
        return Task.CompletedTask;
    }
}