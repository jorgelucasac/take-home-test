using FluentAssertions;
using Fundo.Domain.Entities;
using Fundo.Domain.Enums;
using Fundo.Domain.Exceptions;
using System;
using Xunit;

namespace Fundo.Services.Tests.Unit.Entities;

public class LoanTests
{
    [Fact]
    public void CreatingLoan_WithBalanceGreatThenZero_ShouldBeActive()
    {
        // Arrange
        decimal amount = 1000m;
        decimal currentBalance = 500m;
        string applicantName = "John Doe";

        // Act
        var loan = new Loan(amount, currentBalance, applicantName);
        // Assert
        loan.Amount.Should().Be(amount);
        loan.CurrentBalance.Should().Be(currentBalance);
        loan.ApplicantName.Should().Be(applicantName);
        loan.Status.Should().Be(LoanStatus.Active);
        loan.Id.Should().NotBeEmpty();
        loan.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, precision: TimeSpan.FromMilliseconds(1000));
    }

    [Fact]
    public void CreatingLoan_WithZeroBalance_ShouldBePaid()
    {
        // Arrange
        decimal amount = 1000m;
        decimal currentBalance = 0m;
        string applicantName = "John Doe";
        // Act
        var loan = new Loan(amount, currentBalance, applicantName);
        // Assert
        loan.Amount.Should().Be(amount);
        loan.CurrentBalance.Should().Be(currentBalance);
        loan.ApplicantName.Should().Be(applicantName);
        loan.Status.Should().Be(LoanStatus.Paid);
        loan.Id.Should().NotBeEmpty();
        loan.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, precision: TimeSpan.FromMilliseconds(1000));
    }

    [Fact]
    public void CreatingLoan_WithNullApplicantName_ShouldThrowException()
    {
        // Arrange
        decimal amount = 1000m;
        decimal currentBalance = 500m;
        string applicantName = null;
        // Act
        Action act = () => new Loan(amount, currentBalance, applicantName);
        // Assert
        act.Should().Throw<DomainArgumentException>()
            .WithMessage("Applicant name cannot be null or empty.*");
    }

    [Fact]
    public void CreatingLoan_WithInvalidApplicantName_ShouldThrowException()
    {
        // Arrange
        decimal amount = 1000m;
        decimal currentBalance = 500m;
        string applicantName = "";
        // Act
        Action act = () => new Loan(amount, currentBalance, applicantName);
        // Assert
        act.Should().Throw<DomainArgumentException>()
            .WithMessage("Applicant name cannot be null or empty.*");
    }

    [Fact]
    public void CreatingLoan_WithNonPositiveAmount_ShouldThrowException()
    {
        // Arrange
        decimal amount = 0m;
        decimal currentBalance = 500m;
        string applicantName = "John Doe";
        // Act
        Action act = () => new Loan(amount, currentBalance, applicantName);
        // Assert
        act.Should().Throw<DomainArgumentException>()
            .WithMessage("Amount must be greater than zero.*");
    }

    [Fact]
    public void CreatingLoan_WithNegativeCurrentBalance_ShouldThrowException()
    {
        // Arrange
        decimal amount = 1000m;
        decimal currentBalance = -100m;
        string applicantName = "John Doe";
        // Act
        Action act = () => new Loan(amount, currentBalance, applicantName);
        // Assert
        act.Should().Throw<DomainArgumentException>()
            .WithMessage("Current balance cannot be negative.*");
    }

    [Fact]
    public void CreatingLoan_WithCurrentBalanceGreaterThanAmount_ShouldThrowException()
    {
        // Arrange
        decimal amount = 1000m;
        decimal currentBalance = 1500m;
        string applicantName = "John Doe";
        // Act
        Action act = () => new Loan(amount, currentBalance, applicantName);
        // Assert
        act.Should().Throw<DomainArgumentException>()
            .WithMessage("Current balance cannot be greater than amount.*");
    }

    [Fact]
    public void ApplyPayment_WithValidAmount_ShouldReduceBalance()
    {
        // Arrange
        decimal amount = 1000m;
        decimal currentBalance = 500m;
        string applicantName = "John Doe";
        var loan = new Loan(amount, currentBalance, applicantName);
        decimal paymentAmount = 200m;
        // Act
        loan.ApplyPayment(paymentAmount);
        // Assert
        loan.CurrentBalance.Should().Be(currentBalance - paymentAmount);
        loan.Status.Should().Be(LoanStatus.Active);
    }

    [Fact]
    public void ApplyPayment_WithAmountEqualToCurrentBalance_ShouldSetStatusToPaid()
    {
        // Arrange
        decimal amount = 1000m;
        decimal currentBalance = 500m;
        string applicantName = "John Doe";
        var loan = new Loan(amount, currentBalance, applicantName);
        decimal paymentAmount = 500m;
        // Act
        loan.ApplyPayment(paymentAmount);
        // Assert
        loan.CurrentBalance.Should().Be(0m);
        loan.Status.Should().Be(LoanStatus.Paid);
    }

    [Fact]
    public void ApplyPayment_WithNonPositiveAmount_ShouldThrowException()
    {
        // Arrange
        decimal amount = 1000m;
        decimal currentBalance = 500m;
        string applicantName = "John Doe";
        var loan = new Loan(amount, currentBalance, applicantName);
        decimal paymentAmount = 0m;
        // Act
        Action act = () => loan.ApplyPayment(paymentAmount);
        // Assert
        act.Should().Throw<DomainArgumentException>()
            .WithMessage("Payment amount must be greater than zero.*");
    }

    [Fact]
    public void ApplyPayment_WithAmountGreaterThanCurrentBalance_ShouldThrowException()
    {
        // Arrange
        decimal amount = 1000m;
        decimal currentBalance = 500m;
        string applicantName = "John Doe";
        var loan = new Loan(amount, currentBalance, applicantName);
        decimal paymentAmount = 600m;
        // Act
        Action act = () => loan.ApplyPayment(paymentAmount);
        // Assert
        act.Should().Throw<DomainArgumentException>()
            .WithMessage("Payment cannot be greater than current balance.*");
    }

    [Fact]
    public void CreatingLoan_ShouldInitializeIdAndCreatedAt()
    {
        // Arrange
        decimal amount = 1000m;
        decimal currentBalance = 500m;
        string applicantName = "John Doe";
        // Act
        var loan = new Loan(amount, currentBalance, applicantName);
        // Assert
        loan.Id.Should().NotBeEmpty();
        loan.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, precision: TimeSpan.FromMilliseconds(1000));
    }
}