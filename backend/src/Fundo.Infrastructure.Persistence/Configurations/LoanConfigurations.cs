using Fundo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fundo.Infrastructure.Persistence.Configurations;

internal class LoanConfiguration : IEntityTypeConfiguration<Loan>
{
    public void Configure(EntityTypeBuilder<Loan> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.CreatedAt)
               .IsRequired();

        builder.Property(l => l.Amount)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        builder.Property(l => l.CurrentBalance)
                 .IsRequired()
                 .HasColumnType("decimal(18,2)");

        builder.Property(l => l.ApplicantName)
                 .IsRequired()
                 .HasMaxLength(200);

        builder.Property(l => l.Status)
                .IsRequired();

        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.CreatedAt);
    }
}