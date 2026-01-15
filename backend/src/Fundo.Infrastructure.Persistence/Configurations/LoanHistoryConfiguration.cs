using Fundo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fundo.Infrastructure.Persistence.Configurations;

internal class LoanHistoryConfiguration : IEntityTypeConfiguration<LoanHistory>
{
    public void Configure(EntityTypeBuilder<LoanHistory> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.LoanId)
            .IsRequired();

        builder.Property(x => x.CurrentBalance)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(x => x.PaymentAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(l => l.Status)
               .IsRequired();

        builder.Property(x => x.LoanId)
            .IsRequired();

        builder.HasIndex(x => new { x.LoanId, x.Id });

        builder.HasOne(x => x.Loan)
            .WithMany(x => x.Histories)
            .HasForeignKey(x => x.LoanId);
    }
}