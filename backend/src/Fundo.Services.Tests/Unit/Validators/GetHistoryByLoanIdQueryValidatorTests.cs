using FluentAssertions;
using Fundo.Application.Features.Queries.GetHistoryByLoanId;
using Xunit;

namespace Fundo.Services.Tests.Unit.Validators
{
    public class GetHistoryByLoanIdQueryValidatorTests
    {
        private readonly GetHistoryByLoanIdQueryValidator _validator;

        public GetHistoryByLoanIdQueryValidatorTests()
        {
            _validator = new GetHistoryByLoanIdQueryValidator();
        }

        [Fact]
        public void Validate_ShouldReturnError_WhenLoanIdIsLessThanOrEqualToZero()
        {
            // Arrange
            var query = new GetHistoryByLoanIdQuery(0);
            // Act
            var result = _validator.Validate(query);
            // Assert
            result.IsValid.Should().BeFalse();
            Assert.False(result.IsValid);
            result.Errors.Count.Should().Be(1);
            result.Errors.Should().ContainSingle(e => e.PropertyName == "LoanId" && e.ErrorMessage == "LoanId must be greater than 0.");
        }

        [Fact]
        public void Validate_ShouldBeValid_WhenLoanIdIsGreaterThanZero()
        {
            // Arrange
            var query = new GetHistoryByLoanIdQuery(1);
            // Act
            var result = _validator.Validate(query);
            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Validate_ShouldReturnError_WhenPaginationIsInvalid()
        {
            // Arrange
            var query = new GetHistoryByLoanIdQuery(1, 0, -5);
            // Act
            var result = _validator.Validate(query);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Pagination.PageNumber" && e.ErrorMessage.Contains("PageNumber must be greater than 0."));
            result.Errors.Should().Contain(e => e.PropertyName == "Pagination.PageSize" && e.ErrorMessage.Contains("PageSize must be greater than 0."));
        }

        [Fact]
        public void Validate_ShouldBeValid_WhenPaginationIsValid()
        {
            // Arrange
            var query = new GetHistoryByLoanIdQuery(1, 2, 50);
            // Act
            var result = _validator.Validate(query);
            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Validate_ShouldReturnMultipleErrors_WhenBothLoanIdAndPaginationAreInvalid()
        {
            // Arrange
            var query = new GetHistoryByLoanIdQuery(0, -1, 200);
            // Act
            var result = _validator.Validate(query);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "LoanId" && e.ErrorMessage == "LoanId must be greater than 0.");
            result.Errors.Should().Contain(e => e.PropertyName == "Pagination.PageNumber" && e.ErrorMessage.Contains("PageNumber must be greater than 0."));
            result.Errors.Should().Contain(e => e.PropertyName == "Pagination.PageSize" && e.ErrorMessage.Contains("PageSize must be less than or equal to 100."));
        }
    }
}