using Fundo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fundo.Infrastructure.Persistence.Configurations;

internal class LoanConfiguration : BaseEntityConfiguration<Loan>
{
    public override void Configure(EntityTypeBuilder<Loan> builder)
    {
        base.Configure(builder);

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