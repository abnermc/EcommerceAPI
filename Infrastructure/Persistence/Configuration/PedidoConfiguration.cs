using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Persistence.Configurations;

public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
{
    public void Configure(EntityTypeBuilder<Pedido> builder)
    {
        builder.ToTable("pedidos");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id");

        builder.Property(p => p.ClienteId)
            .HasColumnName("cliente_id")
            .IsRequired();

        builder.Property(p => p.Estado)
            .HasColumnName("estado")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(p => p.Total)
            .HasColumnName("total")
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.Property(p => p.DireccionEntrega)
            .HasColumnName("direccion_entrega")
            .HasMaxLength(500);

        builder.Property(p => p.FechaCreacion)
            .HasColumnName("fecha_creacion");

        builder.Property(p => p.FechaConfirmacion)
            .HasColumnName("fecha_confirmacion");

        // Relación con DetallePedido
        builder.HasMany(p => p.Detalles)
            .WithOne()
            .HasForeignKey(d => d.PedidoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Cliente>()
            .WithMany()
            .HasForeignKey(p => p.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(p => p.ClienteId)
            .HasDatabaseName("ix_pedidos_cliente_id");

        builder.HasIndex(p => p.Estado)
            .HasDatabaseName("ix_pedidos_estado");
    }
}