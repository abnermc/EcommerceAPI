using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Persistence.Configurations;

public class DetallePedidoConfiguration : IEntityTypeConfiguration<DetallePedido>
{
    public void Configure(EntityTypeBuilder<DetallePedido> builder)
    {
        builder.ToTable("detalles_pedido");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasColumnName("id");

        builder.Property(d => d.PedidoId)
            .HasColumnName("pedido_id")
            .IsRequired();

        builder.Property(d => d.ProductoId)
            .HasColumnName("producto_id")
            .IsRequired();

        builder.Property(d => d.NombreProductoSnapshot)
            .HasColumnName("nombre_producto_snapshot")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(d => d.Cantidad)
            .HasColumnName("cantidad")
            .IsRequired();

        builder.Property(d => d.PrecioUnitarioCongelado)
            .HasColumnName("precio_unitario_congelado")
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.Property(d => d.Subtotal)
            .HasColumnName("subtotal")
            .HasColumnType("numeric(18,2)")
            .IsRequired();
    }
}