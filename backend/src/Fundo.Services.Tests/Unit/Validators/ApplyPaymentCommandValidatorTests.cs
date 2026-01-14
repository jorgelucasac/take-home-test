using FluentAssertions;
using Fundo.Application.Features.Commands.ApplyPayment;
using System;
using Xunit;

namespace Fundo.Services.Tests.Unit.Validators;

public class ApplyPaymentCommandValidatorTests
{
    private readonly ApplyPaymentCommandValidator _validator;

    public ApplyPaymentCommandValidatorTests()
    {
        _validator = new ApplyPaymentCommandValidator();
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenLoanIdIsEmpty()
    {
        // Arrange
        var command = new ApplyPaymentCommand(0, 100m);
        // Act
        var result = _validator.Validate(command);
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Id" && e.ErrorMessage == "Loan ID must be greater than zero.");
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenAmountIsNotGreaterThanZero()
    {
        // Arrange
        int loandId = Random.Shared.Next(1, 1000);
        var command = new ApplyPaymentCommand(loandId, 0m);
        // Act
        var result = _validator.Validate(command);
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Amount" && e.ErrorMessage == "Payment amount must be greater than zero.");
    }

    [Fact]
    public void Validate_ShouldBeValid_WhenCommandIsCorrect()
    {
        // Arrange
        int loandId = Random.Shared.Next(1, 1000);
        var command = new ApplyPaymentCommand(loandId, 150m);
        // Act
        var result = _validator.Validate(command);
        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}