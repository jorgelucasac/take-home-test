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
        var query = new GetLoanByIdQuery(0);
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
        int loandId = Random.Shared.Next(1, 1000);
        var query = new GetLoanByIdQuery(loandId);
        // Act
        var result = _validator.Validate(query);
        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}