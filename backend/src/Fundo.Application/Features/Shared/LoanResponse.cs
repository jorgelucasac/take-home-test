namespace Fundo.Application.Features.Shared;

public record LoanResponse(Guid Id, decimal Amount, decimal CurrentBalance, string ApplicantName, string Status);