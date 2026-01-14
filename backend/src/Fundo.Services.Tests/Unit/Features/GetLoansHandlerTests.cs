using FluentAssertions;
using Fundo.Application.Features.Queries.GetLoans;
using Fundo.Domain.Entities;
using Fundo.Domain.Repositories;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Fundo.Services.Tests.Unit.Features;

public class GetLoansHandlerTests
{
    private readonly Mock<ILoanRepository> _loanRepositoryMock;
    private readonly GetLoansHandler _handler;

    public GetLoansHandlerTests()
    {
        _loanRepositoryMock = new Mock<ILoanRepository>();
        _handler = new GetLoansHandler(_loanRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_CallsGetAllAsyncOnRepository()
    {
        // Arrange
        _loanRepositoryMock
            .Setup(repo => repo.GetAllAsync(CancellationToken.None))
            .ReturnsAsync(new List<Loan>());
        var query = new GetLoansQuery();
        // Act
        var result = await _handler.Handle(query, CancellationToken.None);
        // Assert
        _loanRepositoryMock.Verify(repo => repo.GetAllAsync(CancellationToken.None), Times.Once);
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ReturnsLoansFromRepository()
    {
        // Arrange
        var loans = new List<Loan>
        {
            new(2000, 1500, "Bob")
        };
        _loanRepositoryMock
            .Setup(repo => repo.GetAllAsync(CancellationToken.None))
            .ReturnsAsync(loans);
        var query = new GetLoansQuery();
        // Act
        var result = await _handler.Handle(query, CancellationToken.None);
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Count().Should().Be(1);
        result.Value.Should().AllSatisfy(loanResponse =>
        {
            var correspondingLoan = loans.FirstOrDefault(l => l.Id == loanResponse.Id);
            loanResponse.Amount.Should().Be(correspondingLoan.Amount);
            loanResponse.CurrentBalance.Should().Be(correspondingLoan.CurrentBalance);
            loanResponse.ApplicantName.Should().Be(correspondingLoan.ApplicantName);
            loanResponse.Status.Should().Be(correspondingLoan.Status.ToString());
        });
        _loanRepositoryMock.Verify(repo => repo.GetAllAsync(CancellationToken.None), Times.Once);
    }
}