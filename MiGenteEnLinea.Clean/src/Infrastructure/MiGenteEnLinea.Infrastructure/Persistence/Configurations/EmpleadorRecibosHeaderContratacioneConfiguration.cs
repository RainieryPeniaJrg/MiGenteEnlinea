using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Domain.Entities.Pagos;

namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuración de Entity Framework para EmpleadorRecibosHeaderContratacione
/// Mapea al modelo de dominio con la tabla legacy "Empleador_Recibos_Header_Contrataciones"
/// </summary>
public class EmpleadorRecibosHeaderContratacioneConfiguration : IEntityTypeConfiguration<EmpleadorRecibosHeaderContratacione>
{
    public void Configure(EntityTypeBuilder<EmpleadorRecibosHeaderContratacione> builder)
    {
        // Tabla
        builder.ToTable("Empleador_Recibos_Header_Contrataciones");

        // Clave primaria
        builder.HasKey(e => e.PagoId);
        builder.Property(e => e.PagoId)
            .HasColumnName("pagoID")
            .ValueGeneratedOnAdd();

        // Propiedades
        builder.Property(e => e.UserId)
            .IsRequired()
            .HasMaxLength(250) // Debe coincidir con Credenciales.userID
            .IsUnicode(false)
            .HasColumnName("userID");

        builder.Property(e => e.ContratacionId)
            .HasColumnName("contratacionID");

        builder.Property(e => e.FechaRegistro)
            .HasColumnType("datetime")
            .HasColumnName("fechaRegistro");

        builder.Property(e => e.FechaPago)
            .HasColumnType("datetime")
            .HasColumnName("fechaPago");

        builder.Property(e => e.ConceptoPago)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("conceptoPago");

        builder.Property(e => e.Tipo)
            .HasColumnName("tipo");

        // Índices
        builder.HasIndex(e => e.UserId)
            .HasDatabaseName("IX_EmpleadorRecibosHeader_UserId");

        builder.HasIndex(e => e.ContratacionId)
            .HasDatabaseName("IX_EmpleadorRecibosHeader_ContratacionId");

        builder.HasIndex(e => e.FechaPago)
            .HasDatabaseName("IX_EmpleadorRecibosHeader_FechaPago");

        builder.HasIndex(e => new { e.UserId, e.FechaPago })
            .HasDatabaseName("IX_EmpleadorRecibosHeader_UserId_FechaPago");

        // ===========================
        // RELACIONES
        // ===========================
        
        // ✅ RELACIÓN: EmpleadorRecibosHeaderContratacione → EmpleadorRecibosDetalleContratacione (1:N)
        // Un header de recibo puede tener múltiples líneas de detalle
        builder.HasMany<EmpleadorRecibosDetalleContratacione>()
            .WithOne()
            .HasForeignKey(d => d.PagoId)
            .HasPrincipalKey(h => h.PagoId)
            .HasConstraintName("FK_Empleador_Recibos_Detalle_Contrataciones_Empleador_Recibos_Header_Contrataciones")
            .OnDelete(DeleteBehavior.Cascade); // Si se borra header, se borran detalles
    }
}
