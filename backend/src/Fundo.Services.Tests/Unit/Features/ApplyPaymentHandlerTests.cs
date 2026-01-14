using FluentAssertions;
using Fundo.Application.Features.Commands.ApplyPayment;
using Fundo.Domain.Entities;
using Fundo.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Fundo.Services.Tests.Unit.Features;

public class ApplyPaymentHandlerTests
{
    private readonly ApplyPaymentHandler _handler;
    private readonly Mock<ILoanRepository> _loanRepositoryMock;
    private readonly Mock<ILoanHistoryRepository> _loanHistorRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<ApplyPaymentHandler>> _loggerMock;

    public ApplyPaymentHandlerTests()
    {
        _loanRepositoryMock = new Mock<ILoanRepository>();
        _loanHistorRepositoryMock = new Mock<ILoanHistoryRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<ApplyPaymentHandler>>();
        _handler = new ApplyPaymentHandler(
            _loanRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object,
            _loanHistorRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidPayment_AppliesPaymentSuccessfully()
    {
        // Arrange
        var loan = new Loan(1000, 500, "John Doe");
        var command = new ApplyPaymentCommand(loan.Id, 200);
        _loanRepositoryMock
            .Setup(repo => repo.GetByIdForUpdateAsync(command.Id, CancellationToken.None))
            .ReturnsAsync(loan);

        _loanHistorRepositoryMock
            .Setup(x => x.AddAsync(It.Is<LoanHistory>(e =>
            e.LoanId == loan.Id
            && e.Status == loan.Status
            && e.CurrentBalance == loan.CurrentBalance
            && e.PaymentAmount == command.Amount)
            , CancellationToken.None))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        result.IsSuccess.Should().BeTrue();
        loan.CurrentBalance.Should().Be(300);
        loan.Status.Should().Be(Domain.Enums.LoanStatus.Active);
        _unitOfWorkMock.Verify(uow => uow.CommitAsync(CancellationToken.None), Times.Once);
        _loanRepositoryMock.Verify(repo => repo.GetByIdForUpdateAsync(command.Id, CancellationToken.None), Times.Once);
        _loanHistorRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<LoanHistory>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_LoanNotFound_ReturnsNotFoundError()
    {
        // Arrange
        int id = Random.Shared.Next(1, 1000);
        var command = new ApplyPaymentCommand(id, 100);
        _loanRepositoryMock
            .Setup(repo => repo.GetByIdForUpdateAsync(command.Id, CancellationToken.None))
            .ReturnsAsync((Loan?)null);
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error!.Message.Should().Be("Loan not found.");
        _loanRepositoryMock.Verify(repo => repo.GetByIdForUpdateAsync(command.Id, CancellationToken.None), Times.Once);
        _loanHistorRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<LoanHistory>(), CancellationToken.None), Times.Never);
    }

    [Fact]
    public async Task Handle_PaymentGreaterThanBalance_ReturnsFailureError()
    {
        // Arrange
        var loan = new Loan(1000, 300, "Jane Doe");
        var command = new ApplyPaymentCommand(loan.Id, 400);
        _loanRepositoryMock
            .Setup(repo => repo.GetByIdForUpdateAsync(command.Id, CancellationToken.None))
            .ReturnsAsync(loan);
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error!.Message.Should().Be("Payment cannot be greater than current balance.");
        _loanRepositoryMock.Verify(repo => repo.GetByIdForUpdateAsync(command.Id, CancellationToken.None), Times.Once);
        _loanHistorRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<LoanHistory>(), CancellationToken.None), Times.Never);
    }

    [Fact]
    public async Task Handle_LoanAlreadyPaid_ReturnsFailureError()
    {
        // Arrange
        var loan = new Loan(1000, 1000, "Alice Smith");
        loan.ApplyPayment(1000); // Mark loan as paid

        var command = new ApplyPaymentCommand(loan.Id, 100);
        _loanRepositoryMock
            .Setup(repo => repo.GetByIdForUpdateAsync(command.Id, CancellationToken.None))
            .ReturnsAsync(loan);
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error!.Message.Should().Be("Loan is already paid.");
        _loanRepositoryMock.Verify(repo => repo.GetByIdForUpdateAsync(command.Id, CancellationToken.None), Times.Once);
        _loanHistorRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<LoanHistory>(), CancellationToken.None), Times.Never);
    }
}