using Fundo.Domain.Enums;

namespace Fundo.Domain.Entities;

public class LoanHistory : BaseEntity
{
    public LoanHistory(Guid loanId, decimal currentBalance, LoanStatus status)
    {
        LoanId = loanId;
        CurrentBalance = currentBalance;
        Status = status;
    }

    public Guid LoanId { get; set; }
    public decimal CurrentBalance { get; private set; }
    public LoanStatus Status { get; set; }
    public Loan? Loan { get; set; }
}