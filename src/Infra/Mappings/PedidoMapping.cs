using Infra.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Infra.Mappings
{
    [ExcludeFromCodeCoverage]
    public class PedidoMapping : IEntityTypeConfiguration<PedidoDb>
    {
        public void Configure(EntityTypeBuilder<PedidoDb> builder)
        {
            builder.ToTable("Pedidos", "dbo");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.ValorTotal)
                   .HasColumnType("decimal(18,2)")
                   .HasPrecision(2);
        }
    }
}
