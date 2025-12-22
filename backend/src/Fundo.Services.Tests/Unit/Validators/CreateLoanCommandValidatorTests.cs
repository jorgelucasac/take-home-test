using FluentAssertions;
using Fundo.Application.Features.Commands.CreateLoan;
using Xunit;

namespace Fundo.Services.Tests.Unit.Validators;

public class CreateLoanCommandValidatorTests
{
    private readonly CreateLoanCommandValidator _validator;

    public CreateLoanCommandValidatorTests()
    {
        _validator = new CreateLoanCommandValidator();
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenAmountIsNotGreaterThanZero()
    {
        // Arrange
        var command = new CreateLoanCommand(0m, 5m, "Joe Doe");
        // Act
        var result = _validator.Validate(command);
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Amount" && e.ErrorMessage == "Amount must be > 0.");
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenCurrentBalanceIsNegative()
    {
        // Arrange
        var command = new CreateLoanCommand(100m, -10m, "Joe Doe");
        // Act
        var result = _validator.Validate(command);
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "CurrentBalance" && e.ErrorMessage == "CurrentBalance must be >= 0.");
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenCurrentBalanceExceedsAmount()
    {
        // Arrange
        var command = new CreateLoanCommand(100m, 150m, "Joe Doe");
        // Act
        var result = _validator.Validate(command);
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "CurrentBalance" && e.ErrorMessage == "CurrentBalance cannot exceed Amount.");
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenApplicantNameIsEmpty()
    {
        // Arrange
        var command = new CreateLoanCommand(100m, 50m, "");
        // Act
        var result = _validator.Validate(command);
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "ApplicantName" && e.ErrorMessage == "ApplicantName is required.");
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenApplicantNameExceedsMaxLength()
    {
        // Arrange
        var longName = new string('A', 201);
        var command = new CreateLoanCommand(100m, 50m, longName);
        // Act
        var result = _validator.Validate(command);
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "ApplicantName" && e.ErrorMessage == "ApplicantName max length is 200.");
    }

    [Fact]
    public void Validate_ShouldBeValid_WhenCommandIsCorrect()
    {
        // Arrange
        var command = new CreateLoanCommand(1000m, 500m, "Jane Doe");
        // Act
        var result = _validator.Validate(command);
        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}