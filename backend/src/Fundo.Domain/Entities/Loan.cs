using Fundo.Domain.Enums;

namespace Fundo.Domain.Entities
{
    public class Loan : BaseEntity
    {
        public decimal Amount { get; set; }
        public decimal CurrentBalance { get; set; }
        public string ApplicantName { get; set; }
        public LoanStatus Status { get; set; }

        public Loan(decimal amount, decimal currentBalance, string applicantName, LoanStatus status) : base()
        {
            if (string.IsNullOrWhiteSpace(applicantName))
            {
                throw new ArgumentException("Applicant name cannot be null or empty.", nameof(applicantName));
            }

            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");
            }

            if (currentBalance < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(currentBalance), "Current balance cannot be negative.");
            }

            Amount = amount;
            CurrentBalance = currentBalance;
            ApplicantName = applicantName;
            Status = status;
        }
    }
}