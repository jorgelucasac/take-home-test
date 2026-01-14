using FluentAssertions;
using Fundo.Application.Features.Queries.GetHistoryByLoanId;
using Fundo.Application.Pagination;
using Fundo.Application.Repositories;
using Fundo.Domain.Entities;
using Fundo.Domain.Enums;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Fundo.Services.Tests.Unit.Features
{
    public class GetHistoryByLoanIdHandlerTests
    {
        private readonly Mock<ILoanHistoryRepository> _loanHistoryRepositoryMock;
        private GetHistoryByLoanIdHandler _handler;

        public GetHistoryByLoanIdHandlerTests()
        {
            _loanHistoryRepositoryMock = new Mock<ILoanHistoryRepository>();
            _handler = new GetHistoryByLoanIdHandler(_loanHistoryRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyResponse_WhenNoHistoriesExist()
        {
            // Arrange
            var loanId = Random.Shared.Next(1, 1000);
            var query = new GetHistoryByLoanIdQuery(loanId);
            _loanHistoryRepositoryMock
                .Setup(repo => repo.GetByLoanIdPagedAsync(loanId, query.Pagination, CancellationToken.None))
                .ReturnsAsync(PaginatedResponse<LoanHistory>.Empty(query.Pagination.PageNumber, query.Pagination.PageSize));
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);
            // Assert

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Items.Should().BeEmpty();
            result.Value.PageNumber.Should().Be(query.Pagination.PageNumber);
            result.Value.PageSize.Should().Be(query.Pagination.PageSize);
            _loanHistoryRepositoryMock.Verify(repo => repo.GetByLoanIdPagedAsync(loanId, query.Pagination, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnHistories_WhenHistoriesExist()
        {
            // Arrange
            var loanId = Random.Shared.Next(1, 1000);
            var query = new GetHistoryByLoanIdQuery(loanId);
            var histories = new[]
            {
                new LoanHistory(Random.Shared.Next(1, 1000), Random.Shared.Next(1, 1000), 5000m, LoanStatus.Active),
            };
            var paginatedResponse = PaginatedResponse<LoanHistory>.Create(histories, query.Pagination.PageNumber, query.Pagination.PageSize, histories.Length);
            _loanHistoryRepositoryMock
                .Setup(repo => repo.GetByLoanIdPagedAsync(loanId, query.Pagination, CancellationToken.None))
                .ReturnsAsync(paginatedResponse);
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);
            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Items.Should().HaveCount(histories.Length);
            result.Value.PageNumber.Should().Be(query.Pagination.PageNumber);
            result.Value.PageSize.Should().Be(query.Pagination.PageSize);
            _loanHistoryRepositoryMock.Verify(repo => repo.GetByLoanIdPagedAsync(loanId, query.Pagination, CancellationToken.None), Times.Once);
        }
    }
}