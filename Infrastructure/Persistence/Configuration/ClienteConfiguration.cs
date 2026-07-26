using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("clientes");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("id");

            builder.Property(c => c.Nombres)
                .HasColumnName("nombres")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.Apellidos)
                .HasColumnName("apellidos")
                .HasMaxLength(100)
                .IsRequired();

            builder.Ignore(c => c.NombreCompleto);

            builder.Property(c => c.Email)
                .HasColumnName("email")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(c => c.PasswordHash)
                .HasColumnName("password_hash")
                .IsRequired();

            builder.Property(c => c.Telefono)
            .HasColumnName("telefono")
            .HasMaxLength(20);

            builder.Property(c => c.Activo)
                .HasColumnName("activo")
                .HasDefaultValue(true);

            builder.Property(c => c.FechaRegistro)
                .HasColumnName("fecha_registro");

            builder.HasIndex(c => c.Email)
                .IsUnique()
                .HasDatabaseName("ix_clientes_email");
        }
    }
}
