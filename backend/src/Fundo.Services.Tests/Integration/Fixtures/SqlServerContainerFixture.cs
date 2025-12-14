using DotNet.Testcontainers.Builders;
using System;
using System.Threading.Tasks;
using Testcontainers.MsSql;
using Xunit;

namespace Fundo.Services.Tests.Integration.Fixtures;

public sealed class SqlServerContainerFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _container;

    public string ConnectionString => _container.GetConnectionString();

    public SqlServerContainerFixture()
    {
        _container = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPassword("Dev@123456")
            //.WithEnvironment("ACCEPT_EULA", "Y")
            .WithCleanUp(true)
            .WithPortBinding(1434, true)
            .WithWaitStrategy(
                Wait.ForUnixContainer()
                    .UntilPortIsAvailable(1434)
            )
            .Build();
    }

    public async Task InitializeAsync()
    {
        try
        {
            await _container.StartAsync();
        }
        catch
        {
            var logs = await _container.GetLogsAsync();
            throw new Exception($"SQL Server container failed to start.\n\nLOGS:\n{logs}");
        }
    }

    public Task DisposeAsync() => _container.DisposeAsync().AsTask();
}