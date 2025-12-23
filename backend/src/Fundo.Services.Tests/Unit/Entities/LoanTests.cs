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
    public void CreatingLoan_WithValidData_ShouldSucceed()
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
}