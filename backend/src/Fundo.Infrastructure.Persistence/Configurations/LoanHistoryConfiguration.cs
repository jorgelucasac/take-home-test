using Fundo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fundo.Infrastructure.Persistence.Configurations;

internal class LoanHistoryConfiguration : BaseEntityConfiguration<LoanHistory>
{
    public override void Configure(EntityTypeBuilder<LoanHistory> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.LoanId)
            .IsRequired();

        builder.Property(x => x.CurrentBalance)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(l => l.Status)
               .IsRequired();

        builder.Property(x => x.LoanId)
            .IsRequired();

        builder.HasOne(x => x.Loan)
            .WithMany(x => x.Histories)
            .HasForeignKey(x => x.LoanId);
    }
}