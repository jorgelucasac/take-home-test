using FluentAssertions;
using Fundo.Application.Features.Commands.CreateLoan;
using Fundo.Application.Features.Shared;
using Fundo.Application.Results;
using Fundo.Services.Tests.Integration.Fixtures;
using Xunit;

namespace Fundo.Services.Tests.Integration.Fundo.WebApi.DependencyInjection
{
    public class DependencyInjectionTests(SqlServerContainerFixture db) : IntegrationTestBase(db)
    {
        [Fact]
        public void ApplicationServices_PipelineBehaviorRegistered_ShouldWork()
        {
            Factory.Services.GetService(typeof(MediatR.IPipelineBehavior<CreateLoanCommand, Result<LoanResponse>>)).Should().NotBeNull();
        }
    }
}