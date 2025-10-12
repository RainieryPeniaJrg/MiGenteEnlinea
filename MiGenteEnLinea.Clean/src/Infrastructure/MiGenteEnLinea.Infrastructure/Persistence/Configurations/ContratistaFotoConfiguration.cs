using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Domain.Entities.Contratistas;

namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuración de Entity Framework Core para la entidad ContratistaFoto.
/// Mapea la entidad de dominio a la tabla legacy "Contratistas_Fotos".
/// </summary>
public sealed class ContratistaFotoConfiguration : IEntityTypeConfiguration<ContratistaFoto>
{
    public void Configure(EntityTypeBuilder<ContratistaFoto> builder)
    {
        // Nombre de tabla legacy
        builder.ToTable("Contratistas_Fotos");

        // Clave primaria
        builder.HasKey(cf => cf.ImagenId);
        builder.Property(cf => cf.ImagenId)
            .HasColumnName("imagenID")
            .ValueGeneratedOnAdd();

        // Propiedades
        builder.Property(cf => cf.ContratistaId)
            .IsRequired()
            .HasColumnName("contratistaID");

        builder.Property(cf => cf.ImagenUrl)
            .IsRequired()
            .HasMaxLength(250)
            .HasColumnName("imagenURL")
            .IsUnicode(false);

        builder.Property(cf => cf.TipoFoto)
            .HasMaxLength(50)
            .HasColumnName("tipo_foto")
            .IsUnicode(false);

        builder.Property(cf => cf.Descripcion)
            .HasMaxLength(500)
            .HasColumnName("descripcion")
            .IsUnicode(false);

        builder.Property(cf => cf.Orden)
            .IsRequired()
            .HasColumnName("orden")
            .HasDefaultValue(999);

        builder.Property(cf => cf.Activa)
            .IsRequired()
            .HasColumnName("activa")
            .HasDefaultValue(true);

        builder.Property(cf => cf.EsPrincipal)
            .IsRequired()
            .HasColumnName("es_principal")
            .HasDefaultValue(false);

        builder.Property(cf => cf.Tags)
            .HasMaxLength(200)
            .HasColumnName("tags")
            .IsUnicode(false);

        builder.Property(cf => cf.FechaTrabajo)
            .HasColumnName("fecha_trabajo")
            .HasColumnType("datetime");

        // Campos de auditoría (heredados de AggregateRoot → AuditableEntity)
        builder.Property(cf => cf.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired(false);

        builder.Property(cf => cf.CreatedBy)
            .HasColumnName("created_by")
            .HasMaxLength(250)
            .IsRequired(false);

        builder.Property(cf => cf.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired(false);

        builder.Property(cf => cf.UpdatedBy)
            .HasColumnName("updated_by")
            .HasMaxLength(250)
            .IsRequired(false);

        // Ignorar la colección de eventos de dominio
        builder.Ignore(cf => cf.Events);

        // Relación con Contratista (muchos a uno)
        // Nota: La configuración de navegación se maneja en ContratistaConfiguration

        // Índices para optimizar consultas
        builder.HasIndex(cf => cf.ContratistaId)
            .HasDatabaseName("IX_ContratistasFotos_ContratistaId");

        builder.HasIndex(cf => cf.Activa)
            .HasDatabaseName("IX_ContratistasFotos_Activa");

        builder.HasIndex(cf => cf.EsPrincipal)
            .HasDatabaseName("IX_ContratistasFotos_EsPrincipal");

        builder.HasIndex(cf => new { cf.ContratistaId, cf.Activa, cf.Orden })
            .HasDatabaseName("IX_ContratistasFotos_Contratista_Activa_Orden");

        builder.HasIndex(cf => new { cf.ContratistaId, cf.EsPrincipal })
            .HasDatabaseName("IX_ContratistasFotos_Contratista_Principal")
            .IsUnique()
            .HasFilter("[es_principal] = 1"); // Solo una foto principal por contratista
    }
}
