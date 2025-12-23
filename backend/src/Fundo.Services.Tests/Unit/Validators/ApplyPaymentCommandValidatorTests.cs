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
        var command = new ApplyPaymentCommand(Guid.Empty, 100m);
        // Act
        var result = _validator.Validate(command);
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Id" && e.ErrorMessage == "Loan ID must be provided.");
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenAmountIsNotGreaterThanZero()
    {
        // Arrange
        var command = new ApplyPaymentCommand(Guid.NewGuid(), 0m);
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
        var command = new ApplyPaymentCommand(Guid.NewGuid(), 150m);
        // Act
        var result = _validator.Validate(command);
        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}