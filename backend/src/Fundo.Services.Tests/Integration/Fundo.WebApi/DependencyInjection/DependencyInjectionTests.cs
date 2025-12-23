using FluentAssertions;
using Fundo.Application.Behaviors;
using Fundo.Application.Features.Commands.CreateLoan;
using Fundo.Application.Features.Shared;
using Fundo.Application.Results;
using Fundo.Services.Tests.Integration.Fixtures;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fundo.Services.Tests.Integration.Fundo.WebApi.DependencyInjection
{
    public class DependencyInjectionTests(SqlServerContainerFixture db) : IntegrationTestBase(db)
    {
        [Fact]
        public void ApplicationServices_PipelineBehaviorRegistered_ShouldWork()
        {
            var behaviors = Factory.Services.CreateScope()
                .ServiceProvider
                .GetServices<IPipelineBehavior<CreateLoanCommand, Result<LoanResponse>>>();

            behaviors.Should().NotBeNull();
            behaviors.Should().Contain(b => b.GetType().IsGenericType
                && b.GetType().GetGenericTypeDefinition() == typeof(ValidationBehavior<,>), because: "ValidationBehavior should be registered");
            behaviors.Should().HaveCount(5);
        }
    }
}