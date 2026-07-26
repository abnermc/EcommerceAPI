using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Persistence.Configurations;

public class CatalogoLecturaConfiguration : IEntityTypeConfiguration<CatalogoLectura>
{
    public void Configure(EntityTypeBuilder<CatalogoLectura> builder)
    {
        builder.ToTable("catalogo_lectura");

        // ProductoId es la PK — no tiene Id propio
        // porque es una copia identificada por el Id del producto en Backoffice
        builder.HasKey(c => c.ProductoId);

        builder.Property(c => c.ProductoId)
            .HasColumnName("producto_id");

        builder.Property(c => c.Sku)
            .HasColumnName("sku")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.Nombre)
            .HasColumnName("nombre")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(c => c.Descripcion)
            .HasColumnName("descripcion")
            .HasMaxLength(1000);

        builder.Property(c => c.PrecioActual)
            .HasColumnName("precio_actual")
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.Property(c => c.CantidadDisponibleEstimada)
            .HasColumnName("cantidad_disponible_estimada")
            .IsRequired();

        builder.Property(c => c.Activo)
            .HasColumnName("activo")
            .HasDefaultValue(true);

        builder.Property(c => c.FechaSincronizacion)
            .HasColumnName("fecha_sincronizacion");

        builder.HasIndex(c => c.Sku)
            .HasDatabaseName("ix_catalogo_sku");

        builder.HasIndex(c => c.Activo)
            .HasDatabaseName("ix_catalogo_activo");
    }
}
