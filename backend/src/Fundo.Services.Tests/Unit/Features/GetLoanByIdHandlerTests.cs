using FluentAssertions;
using Fundo.Application.Features.Queries.GetLoanById;
using Fundo.Application.Repositories;
using Fundo.Application.Results;
using Fundo.Domain.Entities;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Fundo.Services.Tests.Unit.Features;

public class GetLoanByIdHandlerTests
{
    private readonly Mock<ILoanRepository> _loanRepositoryMock;
    private readonly GetLoanByIdHandler _handler;

    public GetLoanByIdHandlerTests()
    {
        _loanRepositoryMock = new Mock<ILoanRepository>();
        _handler = new GetLoanByIdHandler(_loanRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_LoanExists_ReturnsSuccessResult()
    {
        // Arrange
        var loan = new Loan(1000, 500, "John Doe");
        var loanId = loan.Id;
        _loanRepositoryMock.Setup(repo => repo.GetByIdAsync(loanId, CancellationToken.None))
            .ReturnsAsync(loan);
        var query = new GetLoanByIdQuery(loanId);
        // Act
        var result = await _handler.Handle(query, CancellationToken.None);
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Id.Should().Be(loanId);
        result.Value!.Amount.Should().Be(loan.Amount);
        result.Value!.CurrentBalance.Should().Be(loan.CurrentBalance);
        result.Value!.ApplicantName.Should().Be(loan.ApplicantName);
        result.Value!.Status.Should().Be(loan.Status.ToString());
        _loanRepositoryMock.Verify(repo => repo.GetByIdAsync(loanId, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_LoanDoesNotExist_ReturnsNotFoundError()
    {
        // Arrange
        var loanId = Random.Shared.Next(1, 1000);
        _loanRepositoryMock.Setup(repo => repo.GetByIdAsync(loanId, CancellationToken.None))
            .ReturnsAsync((Loan?)null);
        var query = new GetLoanByIdQuery(loanId);
        // Act
        var result = await _handler.Handle(query, CancellationToken.None);
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error!.Message.Should().Be("Loan not found.");
        result.Error.Type.Should().Be(ErrorType.NotFound);
        _loanRepositoryMock.Verify(repo => repo.GetByIdAsync(loanId, CancellationToken.None), Times.Once);
    }
}