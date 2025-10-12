using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Domain.Entities.Nominas;

namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuración de Entity Framework Core para la entidad DeduccionTss.
/// Mapea a la tabla legacy "Deducciones_TSS".
/// </summary>
public sealed class DeduccionTssConfiguration : IEntityTypeConfiguration<DeduccionTss>
{
    public void Configure(EntityTypeBuilder<DeduccionTss> builder)
    {
        // Tabla
        builder.ToTable("Deducciones_TSS");

        // Primary Key
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        // Propiedades
        builder.Property(d => d.Descripcion)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("descripcion")
            .IsUnicode(false);

        builder.Property(d => d.Porcentaje)
            .IsRequired()
            .HasColumnName("porcentaje")
            .HasColumnType("decimal(5, 2)");

        builder.Property(d => d.Activa)
            .IsRequired()
            .HasColumnName("activa")
            .HasDefaultValue(true);

        builder.Property(d => d.TopeSalarial)
            .HasColumnName("tope_salarial")
            .HasColumnType("decimal(12, 2)");

        // Campos de auditoría (heredados de AuditableEntity)
        builder.Property(d => d.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired(false);

        builder.Property(d => d.CreatedBy)
            .HasColumnName("created_by")
            .HasMaxLength(100)
            .IsUnicode(false)
            .IsRequired(false);

        builder.Property(d => d.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired(false);

        builder.Property(d => d.UpdatedBy)
            .HasColumnName("updated_by")
            .HasMaxLength(100)
            .IsUnicode(false)
            .IsRequired(false);

        // Índices
        builder.HasIndex(d => d.Descripcion)
            .HasDatabaseName("IX_DeduccionesTss_Descripcion");

        builder.HasIndex(d => d.Activa)
            .HasDatabaseName("IX_DeduccionesTss_Activa");
    }
}
