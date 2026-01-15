using Fundo.Domain.Entities;

namespace Fundo.Application.Features.Shared;

public class LoanHistoryResponse
{
    public LoanHistoryResponse(int id, decimal paymentAmount, int loanId, decimal currentBalance, string status)
    {
        Id = id;
        PaymentAmount = paymentAmount;
        LoanId = loanId;
        CurrentBalance = currentBalance;
        Status = status;
    }

    public int Id { get; set; }
    public decimal PaymentAmount { get; set; }
    public int LoanId { get; set; }
    public decimal CurrentBalance { get; private set; }
    public string Status { get; set; }

    public static LoanHistoryResponse FromEntity(LoanHistory history)
    {
        return new LoanHistoryResponse(
            history.Id,
            history.PaymentAmount,
            history.LoanId,
            history.CurrentBalance,
            history.Status.ToString()
        );
    }
}