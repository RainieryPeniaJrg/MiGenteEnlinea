using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Domain.Entities.Contratistas;

namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuración de Entity Framework Core para la entidad ContratistaServicio.
/// Mapea la entidad de dominio a la tabla legacy "Contratistas_Servicios".
/// </summary>
public sealed class ContratistaServicioConfiguration : IEntityTypeConfiguration<ContratistaServicio>
{
    public void Configure(EntityTypeBuilder<ContratistaServicio> builder)
    {
        // Nombre de tabla legacy
        builder.ToTable("Contratistas_Servicios");

        // Clave primaria
        builder.HasKey(cs => cs.ServicioId);
        builder.Property(cs => cs.ServicioId)
            .HasColumnName("servicioID")
            .ValueGeneratedOnAdd();

        // Propiedades
        builder.Property(cs => cs.ContratistaId)
            .IsRequired()
            .HasColumnName("contratistaID");

        builder.Property(cs => cs.DetalleServicio)
            .IsRequired()
            .HasMaxLength(250)
            .HasColumnName("detalleServicio")
            .IsUnicode(false);

        builder.Property(cs => cs.Activo)
            .IsRequired()
            .HasColumnName("activo")
            .HasDefaultValue(true);

        builder.Property(cs => cs.AniosExperiencia)
            .HasColumnName("anios_experiencia");

        builder.Property(cs => cs.TarifaBase)
            .HasMaxLength(100)
            .HasColumnName("tarifa_base")
            .IsUnicode(false);

        builder.Property(cs => cs.Orden)
            .IsRequired()
            .HasColumnName("orden")
            .HasDefaultValue(999);

        builder.Property(cs => cs.Certificaciones)
            .HasMaxLength(500)
            .HasColumnName("certificaciones")
            .IsUnicode(false);

        // Campos de auditoría (heredados de AggregateRoot → AuditableEntity)
        builder.Property(cs => cs.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired(false);

        builder.Property(cs => cs.CreatedBy)
            .HasColumnName("created_by")
            .HasMaxLength(250)
            .IsRequired(false);

        builder.Property(cs => cs.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired(false);

        builder.Property(cs => cs.UpdatedBy)
            .HasColumnName("updated_by")
            .HasMaxLength(250)
            .IsRequired(false);

        // Ignorar la colección de eventos de dominio
        builder.Ignore(cs => cs.Events);

        // Relación con Contratista (muchos a uno)
        // Nota: La configuración de navegación se maneja en ContratistaConfiguration

        // Índices para optimizar consultas
        builder.HasIndex(cs => cs.ContratistaId)
            .HasDatabaseName("IX_ContratistasServicios_ContratistaId");

        builder.HasIndex(cs => cs.Activo)
            .HasDatabaseName("IX_ContratistasServicios_Activo");

        builder.HasIndex(cs => new { cs.ContratistaId, cs.Activo, cs.Orden })
            .HasDatabaseName("IX_ContratistasServicios_Contratista_Activo_Orden");
    }
}
