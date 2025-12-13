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
            Amount = amount;
            CurrentBalance = currentBalance;
            ApplicantName = applicantName;
            Status = status;
        }
    }
}