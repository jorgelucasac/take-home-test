using Fundo.Domain.Enums;
using Fundo.Domain.Exceptions;

namespace Fundo.Domain.Entities;

public class LoanHistory : BaseEntity<int>
{
    public LoanHistory(Guid loanId, decimal currentBalance, decimal paymentAmount, LoanStatus status)
    {
        if (paymentAmount <= 0)
        {
            throw new DomainArgumentException("paymentAmount must be greater than zero.", nameof(paymentAmount));
        }

        LoanId = loanId;
        CurrentBalance = currentBalance;
        Status = status;
        PaymentAmount = paymentAmount;
    }

    public decimal PaymentAmount { get; set; }
    public Guid LoanId { get; set; }
    public decimal CurrentBalance { get; private set; }
    public LoanStatus Status { get; set; }
    public Loan? Loan { get; set; }
}