namespace Fundo.Application.Handlers.Commands.CreateLoan;

public record CreateLoanResponse(Guid Id, decimal Amount, decimal CurrentBalance, string ApplicantName, string Status);