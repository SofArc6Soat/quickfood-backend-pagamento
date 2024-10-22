using Infra.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Infra.Mappings
{
    [ExcludeFromCodeCoverage]
    public class PagamentoMapping : IEntityTypeConfiguration<PagamentoDb>
    {
        public void Configure(EntityTypeBuilder<PagamentoDb> builder)
        {
            builder.ToTable("Pagamentos", "dbo");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Valor)
                   .HasColumnType("decimal(18,2)")
                   .HasPrecision(2);

            builder.Property(c => c.Status)
                   .IsRequired()
                   .HasColumnType("varchar(20)");

            builder.Property(c => c.QrCodePix)
                   .IsRequired()
                   .HasColumnType("varchar(100)");
        }
    }
}
