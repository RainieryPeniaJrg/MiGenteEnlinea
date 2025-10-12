using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Domain.Entities.Empleados;

namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuración de Entity Framework Core para la entidad EmpleadoNota.
/// Mapea a la tabla legacy "Empleados_Notas".
/// </summary>
public sealed class EmpleadoNotaConfiguration : IEntityTypeConfiguration<EmpleadoNota>
{
    public void Configure(EntityTypeBuilder<EmpleadoNota> builder)
    {
        // Tabla
        builder.ToTable("Empleados_Notas");

        // Primary Key
        builder.HasKey(n => n.NotaId);
        builder.Property(n => n.NotaId)
            .HasColumnName("notaID")
            .ValueGeneratedOnAdd();

        // Propiedades requeridas
        builder.Property(n => n.UserId)
            .IsRequired()
            .HasColumnName("userID")
            .HasMaxLength(150)
            .IsUnicode(false);

        builder.Property(n => n.EmpleadoId)
            .IsRequired()
            .HasColumnName("empleadoID");

        builder.Property(n => n.Fecha)
            .IsRequired()
            .HasColumnName("fecha")
            .HasColumnType("datetime");

        builder.Property(n => n.Nota)
            .IsRequired()
            .HasColumnName("nota")
            .HasMaxLength(250)
            .IsUnicode(false);

        builder.Property(n => n.Eliminada)
            .IsRequired()
            .HasColumnName("eliminada")
            .HasDefaultValue(false);

        // Campos de auditoría (heredados de AuditableEntity)
        builder.Property(n => n.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired(false);

        builder.Property(n => n.CreatedBy)
            .HasColumnName("created_by")
            .HasMaxLength(100)
            .IsUnicode(false)
            .IsRequired(false);

        builder.Property(n => n.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired(false);

        builder.Property(n => n.UpdatedBy)
            .HasColumnName("updated_by")
            .HasMaxLength(100)
            .IsUnicode(false)
            .IsRequired(false);

        // Índices
        builder.HasIndex(n => n.EmpleadoId)
            .HasDatabaseName("IX_EmpleadosNotas_EmpleadoId");

        builder.HasIndex(n => n.UserId)
            .HasDatabaseName("IX_EmpleadosNotas_UserId");

        builder.HasIndex(n => n.Fecha)
            .HasDatabaseName("IX_EmpleadosNotas_Fecha");

        builder.HasIndex(n => n.Eliminada)
            .HasDatabaseName("IX_EmpleadosNotas_Eliminada");

        builder.HasIndex(n => new { n.EmpleadoId, n.Eliminada })
            .HasDatabaseName("IX_EmpleadosNotas_EmpleadoId_Eliminada");
    }
}
