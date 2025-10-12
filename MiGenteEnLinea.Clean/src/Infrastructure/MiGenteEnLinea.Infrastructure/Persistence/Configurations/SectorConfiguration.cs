using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Domain.Entities.Catalogos;

namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuración de Entity Framework Core para la entidad Sector.
/// Mapea la entidad de dominio a la tabla legacy "Sectores".
/// </summary>
public sealed class SectorConfiguration : IEntityTypeConfiguration<Sector>
{
    public void Configure(EntityTypeBuilder<Sector> builder)
    {
        // Nombre de tabla legacy
        builder.ToTable("Sectores");

        // Clave primaria
        builder.HasKey(s => s.SectorId);
        builder.Property(s => s.SectorId)
            .HasColumnName("sectorID")
            .ValueGeneratedOnAdd();

        // Propiedades
        builder.Property(s => s.Nombre)
            .IsRequired()
            .HasMaxLength(60)
            .HasColumnName("sector") // Nota: columna se llama "sector" en legacy
            .IsUnicode(false);

        builder.Property(s => s.Codigo)
            .HasMaxLength(10)
            .HasColumnName("codigo")
            .IsUnicode(false);

        builder.Property(s => s.Descripcion)
            .HasMaxLength(500)
            .HasColumnName("descripcion")
            .IsUnicode(false);

        builder.Property(s => s.Grupo)
            .HasMaxLength(100)
            .HasColumnName("grupo")
            .IsUnicode(false);

        builder.Property(s => s.Activo)
            .IsRequired()
            .HasColumnName("activo")
            .HasDefaultValue(true);

        builder.Property(s => s.Orden)
            .IsRequired()
            .HasColumnName("orden")
            .HasDefaultValue(999);

        // Campos de auditoría (heredados de AggregateRoot → AuditableEntity)
        builder.Property(s => s.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired(false);

        builder.Property(s => s.CreatedBy)
            .HasColumnName("created_by")
            .HasMaxLength(250)
            .IsRequired(false);

        builder.Property(s => s.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired(false);

        builder.Property(s => s.UpdatedBy)
            .HasColumnName("updated_by")
            .HasMaxLength(250)
            .IsRequired(false);

        // Ignorar la colección de eventos de dominio
        builder.Ignore(s => s.Events);

        // Índices para optimizar consultas
        builder.HasIndex(s => s.Nombre)
            .HasDatabaseName("IX_Sectores_Nombre");

        builder.HasIndex(s => s.Codigo)
            .HasDatabaseName("IX_Sectores_Codigo");

        builder.HasIndex(s => s.Activo)
            .HasDatabaseName("IX_Sectores_Activo");

        builder.HasIndex(s => new { s.Grupo, s.Orden })
            .HasDatabaseName("IX_Sectores_Grupo_Orden");
    }
}
