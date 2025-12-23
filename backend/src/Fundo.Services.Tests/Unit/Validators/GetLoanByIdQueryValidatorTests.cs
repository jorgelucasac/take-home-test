using FluentAssertions;
using Fundo.Application.Features.Queries.GetLoanById;
using System;
using Xunit;

namespace Fundo.Services.Tests.Unit.Validators;

public class GetLoanByIdQueryValidatorTests
{
    private readonly GetLoanByIdQueryValidator _validator;

    public GetLoanByIdQueryValidatorTests()
    {
        _validator = new GetLoanByIdQueryValidator();
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenIdIsEmpty()
    {
        // Arrange
        var query = new GetLoanByIdQuery(Guid.Empty);
        // Act
        var result = _validator.Validate(query);
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().HaveCount(1);
        result.Errors.Should().Contain(e => e.PropertyName == "Id" && e.ErrorMessage == "Loan ID must be provided.");
    }

    [Fact]
    public void Validate_ShouldPass_WhenIdIsValid()
    {
        // Arrange
        var query = new GetLoanByIdQuery(Guid.NewGuid());
        // Act
        var result = _validator.Validate(query);
        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}