using FluentAssertions;
using Fundo.Application.Handlers.Commands.CreateLoan;
using Fundo.Application.Handlers.Shared;
using Fundo.Services.Tests.Integration.Fixtures;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Fundo.Services.Tests.Integration.Fundo.Applications.WebApi.Controllers
{
    [Collection(nameof(DatabaseCollection))]
    public class LoanManagementControllerTests : IAsyncLifetime
    {
        private HttpClient _client;
        private readonly SqlServerContainerFixture _db;
        private CustomWebApplicationFactory _factory = default!;

        public LoanManagementControllerTests(SqlServerContainerFixture db)
        {
            _db = db;
        }

        [Fact]
        public async Task GetBalances_ShouldReturnExpectedResult()
        {
            var response = await _client.GetAsync("/loans");
            var result = await response.Content.ReadFromJsonAsync<List<LoanResponse>>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
        }

        [Fact]
        public async Task CreateLoan_Then_GetById_ShouldWork()
        {
            var create = new CreateLoanCommand(1500m, 500m, "Maria Silva");

            var createdResp = await _client.PostAsJsonAsync("/loans", create);
            Assert.Equal(HttpStatusCode.Created, createdResp.StatusCode);

            var created = await createdResp.Content.ReadFromJsonAsync<LoanResponse>();
            Assert.NotNull(created);

            var getResp = await _client.GetAsync($"/loans/{created!.Id}");
            Assert.Equal(HttpStatusCode.OK, getResp.StatusCode);
        }

        public Task InitializeAsync()
        {
            _factory = new CustomWebApplicationFactory(_db.ConnectionString);
            _client = _factory.CreateClient();
            return Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            _client.Dispose();
            _factory.Dispose();
            return Task.CompletedTask;
        }
    }
}