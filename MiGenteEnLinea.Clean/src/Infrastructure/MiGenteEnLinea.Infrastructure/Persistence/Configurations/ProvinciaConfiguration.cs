using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Domain.Entities.Catalogos;

namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuración de Entity Framework para la entidad Provincia
/// </summary>
public class ProvinciaConfiguration : IEntityTypeConfiguration<Provincia>
{
    public void Configure(EntityTypeBuilder<Provincia> builder)
    {
        // Nombre de tabla
        builder.ToTable("Provincias");

        // Primary Key
        builder.HasKey(p => p.ProvinciaId);
        builder.Property(p => p.ProvinciaId)
            .HasColumnName("provinciaID")
            .ValueGeneratedOnAdd();

        // Nombre
        builder.Property(p => p.Nombre)
            .HasMaxLength(50)
            .HasColumnName("nombre")
            .IsUnicode(false);

        // Mapeo de campos de auditoría heredados
        builder.Property(p => p.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("datetime");

        builder.Property(p => p.CreatedBy)
            .HasColumnName("created_by")
            .HasMaxLength(450)
            .IsUnicode(false);

        builder.Property(p => p.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("datetime");

        builder.Property(p => p.UpdatedBy)
            .HasColumnName("updated_by")
            .HasMaxLength(450)
            .IsUnicode(false);

        // Ignorar colección de eventos de dominio
        builder.Ignore(p => p.Events);

        // Índices para optimización de consultas
        builder.HasIndex(p => p.Nombre)
            .HasDatabaseName("IX_Provincias_Nombre");

        // Índice único: no puede haber provincias con el mismo nombre
        builder.HasIndex(p => p.Nombre)
            .IsUnique()
            .HasDatabaseName("UX_Provincias_Nombre");
    }
}
