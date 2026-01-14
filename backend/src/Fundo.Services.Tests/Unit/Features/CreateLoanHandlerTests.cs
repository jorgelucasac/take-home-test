using FluentAssertions;
using Fundo.Application.Features.Commands.CreateLoan;
using Fundo.Application.Repositories;
using Fundo.Domain.Entities;
using Fundo.Domain.Enums;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Fundo.Services.Tests.Unit.Features;

public class CreateLoanHandlerTests
{
    private readonly CreateLoanHandler _handler;
    private readonly Mock<ILoanRepository> _loanRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public CreateLoanHandlerTests()
    {
        _loanRepositoryMock = new Mock<ILoanRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new CreateLoanHandler(_loanRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_BalanceGreaterThanZero_CreatesActiveLoanSuccessfully()
    {
        // Arrange
        var command = new CreateLoanCommand(1000, 1000, "Jane Doe");
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Amount.Should().Be(command.Amount);
        result.Value.CurrentBalance.Should().Be(command.CurrentBalance);
        result.Value.ApplicantName.Should().Be(command.ApplicantName);
        result.Value.Status.Should().Be(LoanStatus.Active.ToString());

        _loanRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Loan>(x =>
        x.Amount == command.Amount
       && x.CurrentBalance == command.CurrentBalance
       && x.ApplicantName == command.ApplicantName
       && x.Status == LoanStatus.Active), CancellationToken.None), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.CommitAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_ZeroBalance_CreatesPaidLoanSuccessfully()
    {
        // Arrange
        var command = new CreateLoanCommand(1000, 0, "John Smith");
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Amount.Should().Be(command.Amount);
        result.Value.CurrentBalance.Should().Be(command.CurrentBalance);
        result.Value.ApplicantName.Should().Be(command.ApplicantName);
        result.Value.Status.Should().Be(LoanStatus.Paid.ToString());
        _loanRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Loan>(x =>
        x.Amount == command.Amount
       && x.CurrentBalance == command.CurrentBalance
       && x.ApplicantName == command.ApplicantName
       && x.Status == LoanStatus.Paid), CancellationToken.None), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.CommitAsync(CancellationToken.None), Times.Once);
    }
}