namespace Fundo.Application.Features.Shared;

public record LoanResponse(int Id, decimal Amount, decimal CurrentBalance, string ApplicantName, string Status);