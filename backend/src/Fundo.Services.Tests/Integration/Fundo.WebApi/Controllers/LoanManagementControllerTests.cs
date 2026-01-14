using FluentAssertions;
using Fundo.Application.Features.Commands.CreateLoan;
using Fundo.Application.Features.Shared;
using Fundo.Services.Tests.Integration.Fixtures;
using Fundo.Services.Tests.Integration.Responses;
using Fundo.WebApi.Transport.Rerquest;
using Fundo.WebApi.Transport.Response;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Fundo.Services.Tests.Integration.Fundo.WebApi.Controllers;

public class LoanManagementControllerTests(SqlServerContainerFixture db) : IntegrationTestBase(db)
{
    [Fact]
    public async Task GetLoans_ShouldReturnExpectedResult()
    {
        var response = await Client.GetAsync("/loans");
        var result = await response.Content.ReadFromJsonAsync<List<LoanResponse>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CreateLoan_Then_GetById_ShouldWork()
    {
        var create = new CreateLoanCommand(1500m, 500m, "Maria Silva");

        var createdResp = await Client.PostAsJsonAsync("/loans", create);
        createdResp.StatusCode.Should().Be(HttpStatusCode.Created);

        var created = await createdResp.Content.ReadFromJsonAsync<LoanResponse>();
        created.Should().NotBeNull();

        var getResp = await Client.GetAsync($"/loans/{created!.Id}");
        getResp.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateLoan_WithInvalidData_ShouldReturnBadRequest()
    {
        var create = new CreateLoanCommand(-100m, 50m, "");
        var response = await Client.PostAsJsonAsync("/loans", create);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetLoan_ByNonExistentId_ShouldReturnNotFound()
    {
        var response = await Client.GetAsync($"/loans/{int.MaxValue}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateLoan_Then_PostPayment_ShouldWork()
    {
        var create = new CreateLoanCommand(1500m, 500m, "Maria Silva");

        var createdResp = await Client.PostAsJsonAsync("/loans", create);
        createdResp.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await createdResp.Content.ReadFromJsonAsync<LoanResponse>();
        created.Should().NotBeNull();

        var payment = new PaymentRequest(500m);
        var paymentResp = await Client.PostAsJsonAsync($"/loans/{created!.Id}/payment", payment);
        paymentResp.StatusCode.Should().Be(HttpStatusCode.OK);

        var updated = await paymentResp.Content.ReadFromJsonAsync<LoanResponse>();
        updated.Should().NotBeNull();
        updated!.CurrentBalance.Should().Be(created.CurrentBalance - payment.Amount);
    }

    [Fact]
    public async Task CreateLoan_Then_PostPayment_ShouldCreateHistory()
    {
        var create = new CreateLoanCommand(1500m, 500m, "Maria Silva");

        var createdResp = await Client.PostAsJsonAsync("/loans", create);
        createdResp.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await createdResp.Content.ReadFromJsonAsync<LoanResponse>();
        created.Should().NotBeNull();

        var payment = new PaymentRequest(500m);
        var paymentResp = await Client.PostAsJsonAsync($"/loans/{created!.Id}/payment", payment);
        paymentResp.StatusCode.Should().Be(HttpStatusCode.OK);

        var historiesResp = await Client.GetAsync($"/loans/{created.Id}/histories");
        historiesResp.StatusCode.Should().Be(HttpStatusCode.OK);
        var historiesResult = await historiesResp.Content.ReadFromJsonAsync<TestPaginatedResponse<LoanHistoryResponse>>();

        var updated = await paymentResp.Content.ReadFromJsonAsync<LoanResponse>();
        updated.Should().NotBeNull();
        updated!.CurrentBalance.Should().Be(created.CurrentBalance - payment.Amount);
        historiesResult.Should().NotBeNull();
        historiesResult.TotalItems.Should().Be(1);
        historiesResult.Items.Should().ContainSingle(h =>
            h.LoanId == created.Id &&
            h.PaymentAmount == payment.Amount);
    }

    [Fact]
    public async Task PostPayment_WithInvalidData_ShouldReturnBadRequest()
    {
        var create = new CreateLoanCommand(1500m, 500m, "Maria Silva");

        var createdResp = await Client.PostAsJsonAsync("/loans", create);
        createdResp.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await createdResp.Content.ReadFromJsonAsync<LoanResponse>();
        created.Should().NotBeNull();

        var payment = new PaymentRequest(-200);
        var paymentResp = await Client.PostAsJsonAsync($"/loans/{created!.Id}/payment", payment);
        paymentResp.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errorResponse = await paymentResp.Content.ReadFromJsonAsync<ErrorResponse>();
        errorResponse.Should().NotBeNull();
        errorResponse.Details.Should().ContainSingle(d => d.Key == "Amount");
    }
}