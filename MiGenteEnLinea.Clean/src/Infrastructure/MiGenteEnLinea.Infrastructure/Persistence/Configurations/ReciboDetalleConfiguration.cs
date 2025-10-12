using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Domain.Entities.Nominas;

namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuración de Entity Framework Core para la entidad ReciboDetalle.
/// Mapea a la tabla legacy "Empleador_Recibos_Detalle".
/// </summary>
public sealed class ReciboDetalleConfiguration : IEntityTypeConfiguration<ReciboDetalle>
{
    public void Configure(EntityTypeBuilder<ReciboDetalle> builder)
    {
        // Tabla
        builder.ToTable("Empleador_Recibos_Detalle");

        // Primary Key
        builder.HasKey(d => d.DetalleId);
        builder.Property(d => d.DetalleId)
            .HasColumnName("detalleID")
            .ValueGeneratedOnAdd();

        // Propiedades requeridas
        builder.Property(d => d.PagoId)
            .IsRequired()
            .HasColumnName("pagoID");

        builder.Property(d => d.Concepto)
            .IsRequired()
            .HasColumnName("Concepto")
            .HasMaxLength(90)
            .IsUnicode(false);

        builder.Property(d => d.Monto)
            .IsRequired()
            .HasColumnName("Monto")
            .HasColumnType("decimal(10, 2)");

        builder.Property(d => d.TipoConcepto)
            .IsRequired()
            .HasColumnName("tipo_concepto")
            .HasDefaultValue(1);

        builder.Property(d => d.Orden)
            .HasColumnName("orden");

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
        builder.HasIndex(d => d.PagoId)
            .HasDatabaseName("IX_ReciboDetalle_PagoId");

        builder.HasIndex(d => d.TipoConcepto)
            .HasDatabaseName("IX_ReciboDetalle_TipoConcepto");

        builder.HasIndex(d => new { d.PagoId, d.Orden })
            .HasDatabaseName("IX_ReciboDetalle_PagoId_Orden");

        // Relaciones
        // La relación con ReciboHeader se configurará en ReciboHeaderConfiguration
    }
}
