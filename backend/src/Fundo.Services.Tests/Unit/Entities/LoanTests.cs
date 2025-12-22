using FluentAssertions;
using Fundo.Domain.Entities;
using System;
using Xunit;

namespace Fundo.Services.Tests.Unit.Entities
{
    public class LoanTests
    {
        public LoanTests()
        {
        }

        [Fact]
        public void CreatingLoan_WithValidData_ShouldSucceed()
        {
            // Arrange
            decimal amount = 1000m;
            decimal currentBalance = 500m;
            string applicantName = "John Doe";
            var status = Domain.Enums.LoanStatus.Active;
            // Act
            var loan = new Loan(amount, currentBalance, applicantName, status);
            // Assert
            loan.Amount.Should().Be(amount);
            loan.CurrentBalance.Should().Be(currentBalance);
            loan.ApplicantName.Should().Be(applicantName);
            loan.Status.Should().Be(status);
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
            var status = Domain.Enums.LoanStatus.Active;
            // Act
            Action act = () => new Loan(amount, currentBalance, applicantName, status);
            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Applicant name cannot be null or empty.*");
        }

        [Fact]
        public void CreatingLoan_WithNonPositiveAmount_ShouldThrowException()
        {
            // Arrange
            decimal amount = 0m;
            decimal currentBalance = 500m;
            string applicantName = "John Doe";
            var status = Domain.Enums.LoanStatus.Active;
            // Act
            Action act = () => new Loan(amount, currentBalance, applicantName, status);
            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("Amount must be greater than zero.*");
        }

        [Fact]
        public void CreatingLoan_WithNegativeCurrentBalance_ShouldThrowException()
        {
            // Arrange
            decimal amount = 1000m;
            decimal currentBalance = -100m;
            string applicantName = "John Doe";
            var status = Domain.Enums.LoanStatus.Active;
            // Act
            Action act = () => new Loan(amount, currentBalance, applicantName, status);
            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("Current balance cannot be negative.*");
        }
    }
}