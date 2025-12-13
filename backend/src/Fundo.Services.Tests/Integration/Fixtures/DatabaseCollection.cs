using Xunit;

namespace Fundo.Services.Tests.Integration.Fixtures;

[CollectionDefinition(nameof(DatabaseCollection))]
public class DatabaseCollection : ICollectionFixture<SqlServerContainerFixture>
{
}