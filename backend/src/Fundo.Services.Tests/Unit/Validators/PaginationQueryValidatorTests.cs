using FluentAssertions;
using Fundo.Application.Pagination;
using Xunit;

namespace Fundo.Services.Tests.Unit.Validators
{
    public class PaginationQueryValidatorTests
    {
        private readonly PaginationQueryValidator _validator;

        public PaginationQueryValidatorTests()
        {
            _validator = new PaginationQueryValidator();
        }

        [Fact]
        public void Validate_ShouldReturnError_WhenPageNumberIsLessThanOrEqualToZero()
        {
            // Arrange
            var query = new PaginationQuery(0, 10);
            // Act
            var result = _validator.Validate(query);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Errors[0].PropertyName.Should().Be("PageNumber");
            result.Errors[0].ErrorMessage.Should().Be("PageNumber must be greater than 0.");
        }

        [Fact]
        public void Validate_ShouldReturnError_WhenPageSizeIsLessThanOrEqualToZero()
        {
            // Arrange
            var query = new PaginationQuery(1, 0);
            // Act
            var result = _validator.Validate(query);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Errors[0].PropertyName.Should().Be("PageSize");
            result.Errors[0].ErrorMessage.Should().Be("PageSize must be greater than 0.");
        }

        [Fact]
        public void Validate_ShouldReturnError_WhenPageSizeIsGreaterThan100()
        {
            // Arrange
            var query = new PaginationQuery(1, 101);
            // Act
            var result = _validator.Validate(query);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Errors[0].PropertyName.Should().Be("PageSize");
            result.Errors[0].ErrorMessage.Should().Be("PageSize must be less than or equal to 100.");
        }

        [Fact]
        public void Validate_ShouldBeValid_WhenPageNumberAndPageSizeAreValid()
        {
            // Arrange
            var query = new PaginationQuery(1, 50);
            // Act
            var result = _validator.Validate(query);
            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Validate_ShouldReturnMultipleErrors_WhenBothPageNumberAndPageSizeAreInvalid()
        {
            // Arrange
            var query = new PaginationQuery(0, 150);
            // Act
            var result = _validator.Validate(query);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(2);
            result.Errors.Should().Contain(e => e.PropertyName == "PageNumber" && e.ErrorMessage == "PageNumber must be greater than 0.");
            result.Errors.Should().Contain(e => e.PropertyName == "PageSize" && e.ErrorMessage == "PageSize must be less than or equal to 100.");
        }
    }
}