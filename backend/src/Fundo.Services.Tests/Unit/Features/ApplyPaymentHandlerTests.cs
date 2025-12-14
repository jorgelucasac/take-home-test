using FluentAssertions;
using Fundo.Application.Features.Commands.ApplyPayment;
using Fundo.Domain.Entities;
using Fundo.Domain.Enums;
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
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<ApplyPaymentHandler>> _loggerMock;

    public ApplyPaymentHandlerTests()
    {
        _loanRepositoryMock = new Mock<ILoanRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<ApplyPaymentHandler>>();
        _handler = new ApplyPaymentHandler(_loanRepositoryMock.Object, _unitOfWorkMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ValidPayment_AppliesPaymentSuccessfully()
    {
        // Arrange
        var loan = new Loan(1000, 500, "John Doe", LoanStatus.Active);
        var command = new ApplyPaymentCommand(loan.Id, 200);
        _loanRepositoryMock
            .Setup(repo => repo.GetByIdForUpdateAsync(command.Id, CancellationToken.None))
            .ReturnsAsync(loan);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        result.IsSuccess.Should().BeTrue();
        loan.CurrentBalance.Should().Be(300);
        _unitOfWorkMock.Verify(uow => uow.CommitAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_LoanNotFound_ReturnsNotFoundError()
    {
        // Arrange
        var command = new ApplyPaymentCommand(Guid.NewGuid(), 100);
        _loanRepositoryMock
            .Setup(repo => repo.GetByIdForUpdateAsync(command.Id, CancellationToken.None))
            .ReturnsAsync((Loan?)null);
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error!.Message.Should().Be("Loan not found.");
    }

    [Fact]
    public async Task Handle_PaymentGreaterThanBalance_ReturnsFailureError()
    {
        // Arrange
        var loan = new Loan(1000, 300, "Jane Doe", LoanStatus.Active);
        var command = new ApplyPaymentCommand(loan.Id, 400);
        _loanRepositoryMock
            .Setup(repo => repo.GetByIdForUpdateAsync(command.Id, CancellationToken.None))
            .ReturnsAsync(loan);
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error!.Message.Should().Be("Payment cannot be greater than current balance.");
    }

    [Fact]
    public async Task Handle_LoanAlreadyPaid_ReturnsFailureError()
    {
        // Arrange
        var loan = new Loan(1000, 0, "Alice Smith", LoanStatus.Paid);
        var command = new ApplyPaymentCommand(loan.Id, 100);
        _loanRepositoryMock
            .Setup(repo => repo.GetByIdForUpdateAsync(command.Id, CancellationToken.None))
            .ReturnsAsync(loan);
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error!.Message.Should().Be("Loan is already paid.");
    }
}