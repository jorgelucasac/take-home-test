using FluentAssertions;
using Fundo.Domain.Entities;
using Fundo.Domain.Enums;
using Fundo.Domain.Exceptions;
using System;
using Xunit;

namespace Fundo.Services.Tests.Unit.Entities
{
    public class LoanHistoryTests
    {
        [Fact]
        public void CreateLoanHistory_WithValidParameters_ShouldCreateInstance()
        {
            // Arrange
            int loanId = Random.Shared.Next(1, 1000);
            decimal currentBalance = 5000m;
            decimal paymentAmount = 1000m;
            LoanStatus status = LoanStatus.Active;
            // Act
            var loanHistory = new LoanHistory(loanId, currentBalance, paymentAmount, status);
            // Assert
            loanHistory.LoanId.Should().Be(loanId);
            loanHistory.CurrentBalance.Should().Be(currentBalance);
            loanHistory.PaymentAmount.Should().Be(paymentAmount);
            loanHistory.Status.Should().Be(status);
            loanHistory.Id.Should().Be(0);
            loanHistory.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-1000)]
        public void CreateLoanHistory_WithInvalidAmount_ShouldThrowException(int amount)
        {
            // Arrange & Act
            Action act = () => new LoanHistory(
                Random.Shared.Next(1, 1000),
                1000m,
                amount,
               LoanStatus.Active);

            // Assert
            act.Should().Throw<DomainArgumentException>()
                .WithMessage("paymentAmount must be greater than zero.*");
        }
    }
}