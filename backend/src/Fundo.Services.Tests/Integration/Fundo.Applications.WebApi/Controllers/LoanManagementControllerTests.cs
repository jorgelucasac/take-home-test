using FluentAssertions;
using Fundo.Application.Features.Commands.CreateLoan;
using Fundo.Application.Features.Shared;
using Fundo.Services.Tests.Integration.Fixtures;
using Fundo.WebApi.Transport.Rerquest;
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
        public async Task GetLoans_ShouldReturnExpectedResult()
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
            createdResp.StatusCode.Should().Be(HttpStatusCode.Created);

            var created = await createdResp.Content.ReadFromJsonAsync<LoanResponse>();
            created.Should().NotBeNull();

            var getResp = await _client.GetAsync($"/loans/{created!.Id}");
            getResp.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task CreateLoan_WithInvalidData_ShouldReturnBadRequest()
        {
            var create = new CreateLoanCommand(-100m, 50m, "");
            var response = await _client.PostAsJsonAsync("/loans", create);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetLoan_ByNonExistentId_ShouldReturnNotFound()
        {
            var response = await _client.GetAsync($"/loans/{int.MaxValue}");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateLoan_Then_PostPayment_ShouldWork()
        {
            var create = new CreateLoanCommand(1500m, 500m, "Maria Silva");

            var createdResp = await _client.PostAsJsonAsync("/loans", create);
            createdResp.StatusCode.Should().Be(HttpStatusCode.Created);
            var created = await createdResp.Content.ReadFromJsonAsync<LoanResponse>();
            created.Should().NotBeNull();

            var payment = new PaymentRequest(500m);
            var paymentResp = await _client.PostAsJsonAsync($"/loans/{created!.Id}/payment", payment);
            paymentResp.StatusCode.Should().Be(HttpStatusCode.OK);

            var updated = await paymentResp.Content.ReadFromJsonAsync<LoanResponse>();
            updated.Should().NotBeNull();
            updated!.CurrentBalance.Should().Be(created.CurrentBalance - payment.Amount);
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