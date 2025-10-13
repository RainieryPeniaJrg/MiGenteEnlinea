using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Domain.Entities.Pagos;

namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuración de Entity Framework Core para la entidad Venta.
/// </summary>
public class VentaConfiguration : IEntityTypeConfiguration<Venta>
{
    public void Configure(EntityTypeBuilder<Venta> builder)
    {
        builder.ToTable("Ventas");

        // Primary Key
        builder.HasKey(v => v.VentaId);
        builder.Property(v => v.VentaId)
            .HasColumnName("ventaID")
            .ValueGeneratedOnAdd();

        // Properties
        builder.Property(v => v.UserId)
            .IsRequired()
            .HasColumnName("userID")
            .HasMaxLength(250) // Debe coincidir con Credenciales.userID
            .IsUnicode(false);

        builder.Property(v => v.FechaTransaccion)
            .IsRequired()
            .HasColumnName("fecha")
            .HasColumnType("datetime");

        builder.Property(v => v.MetodoPago)
            .IsRequired()
            .HasColumnName("metodo");

        builder.Property(v => v.PlanId)
            .IsRequired()
            .HasColumnName("planID");

        builder.Property(v => v.Precio)
            .IsRequired()
            .HasColumnType("decimal(18, 2)")
            .HasColumnName("precio");

        builder.Property(v => v.Comentario)
            .HasColumnName("comentario")
            .IsUnicode(false)
            .IsRequired(false);

        builder.Property(v => v.IdTransaccion)
            .HasMaxLength(100)
            .HasColumnName("idTransaccion")
            .IsUnicode(false)
            .IsRequired(false);

        builder.Property(v => v.IdempotencyKey)
            .HasMaxLength(100)
            .HasColumnName("idempotencyKey")
            .IsUnicode(false)
            .IsRequired(false);

        builder.Property(v => v.UltimosDigitosTarjeta)
            .HasMaxLength(20)
            .HasColumnName("card")
            .IsUnicode(false)
            .IsRequired(false);

        builder.Property(v => v.DireccionIp)
            .HasMaxLength(20)
            .HasColumnName("ip")
            .IsUnicode(false)
            .IsRequired(false);

        builder.Property(v => v.Estado)
            .IsRequired()
            .HasColumnName("estado")
            .HasDefaultValue(1); // Pendiente

        // Audit Fields (nuevos campos, nullable para compatibilidad con datos existentes)
        builder.Property(v => v.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired(false);

        builder.Property(v => v.CreatedBy)
            .HasColumnName("created_by")
            .HasMaxLength(450)
            .IsRequired(false);

        builder.Property(v => v.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired(false);

        builder.Property(v => v.UpdatedBy)
            .HasColumnName("updated_by")
            .HasMaxLength(450)
            .IsRequired(false);

        // Indexes
        builder.HasIndex(v => v.UserId)
            .HasDatabaseName("IX_Ventas_UserId");

        builder.HasIndex(v => v.PlanId)
            .HasDatabaseName("IX_Ventas_PlanId");

        builder.HasIndex(v => v.IdTransaccion)
            .HasDatabaseName("IX_Ventas_IdTransaccion");

        builder.HasIndex(v => v.IdempotencyKey)
            .IsUnique()
            .HasDatabaseName("IX_Ventas_IdempotencyKey");

        builder.HasIndex(v => v.Estado)
            .HasDatabaseName("IX_Ventas_Estado");

        builder.HasIndex(v => v.FechaTransaccion)
            .HasDatabaseName("IX_Ventas_FechaTransaccion");

        builder.HasIndex(v => new { v.UserId, v.FechaTransaccion })
            .HasDatabaseName("IX_Ventas_UserId_Fecha");

        // Ignore domain events
        builder.Ignore(v => v.Events);
    }
}
