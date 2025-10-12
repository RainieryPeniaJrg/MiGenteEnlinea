using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Domain.Entities.Pagos;

namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuración de Entity Framework para EmpleadorRecibosDetalleContratacione
/// Mapea al modelo de dominio con la tabla legacy "Empleador_Recibos_Detalle_Contrataciones"
/// </summary>
public class EmpleadorRecibosDetalleContratacioneConfiguration : IEntityTypeConfiguration<EmpleadorRecibosDetalleContratacione>
{
    public void Configure(EntityTypeBuilder<EmpleadorRecibosDetalleContratacione> builder)
    {
        // Tabla
        builder.ToTable("Empleador_Recibos_Detalle_Contrataciones");

        // Clave primaria
        builder.HasKey(d => d.DetalleId);
        builder.Property(d => d.DetalleId)
            .HasColumnName("detalleID")
            .ValueGeneratedOnAdd();

        // Propiedades
        builder.Property(d => d.PagoId)
            .HasColumnName("pagoID");

        builder.Property(d => d.Concepto)
            .HasMaxLength(90)
            .IsUnicode(false)
            .HasColumnName("Concepto");

        builder.Property(d => d.Monto)
            .HasColumnType("decimal(10, 2)")
            .HasColumnName("Monto");

        // Índices
        builder.HasIndex(d => d.PagoId)
            .HasDatabaseName("IX_EmpleadorRecibosDetalle_PagoId");

        builder.HasIndex(d => d.Monto)
            .HasDatabaseName("IX_EmpleadorRecibosDetalle_Monto");

        // Relaciones - se configurará la relación con el header cuando sea necesario
        // builder.HasOne<EmpleadorRecibosHeaderContratacione>()
        //     .WithMany()
        //     .HasForeignKey(d => d.PagoId)
        //     .OnDelete(DeleteBehavior.Cascade);
    }
}
