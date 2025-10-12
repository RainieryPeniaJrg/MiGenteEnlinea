using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Domain.Entities.Seguridad;

namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuración de Entity Framework para la entidad Permiso
/// </summary>
public class PermisoConfiguration : IEntityTypeConfiguration<Permiso>
{
    public void Configure(EntityTypeBuilder<Permiso> builder)
    {
        // Nombre de tabla
        builder.ToTable("Permisos");

        // Primary Key
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        // UserId
        builder.Property(p => p.UserId)
            .IsRequired()
            .HasMaxLength(450)
            .HasColumnName("userID")
            .IsUnicode(false);

        // Atributos (flags de permisos)
        builder.Property(p => p.Atributos)
            .IsRequired()
            .HasColumnName("atributos")
            .HasDefaultValue(0);

        // Ignorar propiedades calculadas
        builder.Ignore(p => p.DescripcionPermisos);

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
        builder.HasIndex(p => p.UserId)
            .HasDatabaseName("IX_Permisos_UserId");

        builder.HasIndex(p => p.Atributos)
            .HasDatabaseName("IX_Permisos_Atributos");

        // Índice único: un solo registro de permisos por usuario
        builder.HasIndex(p => p.UserId)
            .IsUnique()
            .HasDatabaseName("UX_Permisos_UserId");
    }
}
