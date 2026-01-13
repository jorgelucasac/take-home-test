using Fundo.Domain.Enums;
using Fundo.Domain.Exceptions;

namespace Fundo.Domain.Entities;

public class Loan : BaseEntity
{
    public decimal Amount { get; private set; }
    public decimal CurrentBalance { get; private set; }
    public string ApplicantName { get; private set; }
    public LoanStatus Status { get; set; }
    public ICollection<LoanHistory> Histories { get; private set; }

    public Loan(decimal amount, decimal currentBalance, string applicantName) : base()
    {
        if (string.IsNullOrWhiteSpace(applicantName))
        {
            throw new DomainArgumentException("Applicant name cannot be null or empty.", nameof(applicantName));
        }

        if (amount <= 0)
        {
            throw new DomainArgumentException("Amount must be greater than zero.", nameof(amount));
        }

        if (currentBalance < 0)
        {
            throw new DomainArgumentException("Current balance cannot be negative.", nameof(currentBalance));
        }

        if (currentBalance > amount)
        {
            throw new DomainArgumentException("Current balance cannot be greater than amount.", nameof(currentBalance));
        }

        Amount = amount;
        CurrentBalance = currentBalance;
        ApplicantName = applicantName;
        Status = currentBalance == 0 ? LoanStatus.Paid : LoanStatus.Active;
        Histories = new List<LoanHistory>();
    }

    public void ApplyPayment(decimal paymentAmount)
    {
        if (paymentAmount <= 0)
        {
            throw new DomainArgumentException("Payment amount must be greater than zero.", nameof(paymentAmount));
        }
        if (paymentAmount > CurrentBalance)
        {
            throw new DomainArgumentException("Payment cannot be greater than current balance.", nameof(paymentAmount));
        }
        CurrentBalance -= paymentAmount;
        if (CurrentBalance == 0)
        {
            Status = LoanStatus.Paid;
        }
    }
}