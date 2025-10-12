using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Domain.Entities.Catalogos;

namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuración de Entity Framework Core para la entidad Servicio.
/// Mapea la entidad de dominio a la tabla legacy "Servicios".
/// </summary>
public sealed class ServicioConfiguration : IEntityTypeConfiguration<Servicio>
{
    public void Configure(EntityTypeBuilder<Servicio> builder)
    {
        // Nombre de tabla legacy
        builder.ToTable("Servicios");

        // Clave primaria
        builder.HasKey(s => s.ServicioId);
        builder.Property(s => s.ServicioId)
            .HasColumnName("servicioID")
            .ValueGeneratedOnAdd();

        // Propiedades
        builder.Property(s => s.Descripcion)
            .IsRequired()
            .HasMaxLength(250)
            .HasColumnName("descripcion")
            .IsUnicode(false);

        builder.Property(s => s.UserId)
            .HasMaxLength(250)
            .HasColumnName("userID")
            .IsUnicode(false);

        builder.Property(s => s.Activo)
            .IsRequired()
            .HasColumnName("activo")
            .HasDefaultValue(true);

        builder.Property(s => s.Orden)
            .IsRequired()
            .HasColumnName("orden")
            .HasDefaultValue(999);

        builder.Property(s => s.Categoria)
            .HasMaxLength(100)
            .HasColumnName("categoria")
            .IsUnicode(false);

        builder.Property(s => s.Icono)
            .HasMaxLength(50)
            .HasColumnName("icono")
            .IsUnicode(false);

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
        builder.HasIndex(s => s.Descripcion)
            .HasDatabaseName("IX_Servicios_Descripcion");

        builder.HasIndex(s => s.Activo)
            .HasDatabaseName("IX_Servicios_Activo");

        builder.HasIndex(s => new { s.Categoria, s.Orden })
            .HasDatabaseName("IX_Servicios_Categoria_Orden");
    }
}
