using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Domain.Entities.Empleados;
using MiGenteEnLinea.Domain.Entities.Contrataciones;
using MiGenteEnLinea.Domain.Entities.Pagos;

namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuración de Entity Framework Core para la entidad EmpleadoTemporal.
/// Mapea a la tabla legacy "Empleados_Temporales".
/// </summary>
public sealed class EmpleadoTemporalConfiguration : IEntityTypeConfiguration<EmpleadoTemporal>
{
    public void Configure(EntityTypeBuilder<EmpleadoTemporal> builder)
    {
        // Tabla
        builder.ToTable("Empleados_Temporales");

        // Primary Key
        builder.HasKey(e => e.ContratacionId);
        builder.Property(e => e.ContratacionId)
            .HasColumnName("contratacionID")
            .ValueGeneratedOnAdd();

        // Propiedades requeridas
        builder.Property(e => e.UserId)
            .IsRequired()
            .HasColumnName("userID")
            .HasMaxLength(250) // Debe coincidir con Credenciales.userID
            .IsUnicode(false);

        builder.Property(e => e.FechaRegistro)
            .IsRequired()
            .HasColumnName("fechaRegistro")
            .HasColumnType("datetime");

        builder.Property(e => e.Tipo)
            .IsRequired()
            .HasColumnName("tipo");

        builder.Property(e => e.Activo)
            .IsRequired()
            .HasColumnName("activo")
            .HasDefaultValue(true);

        // Datos Persona Jurídica
        builder.Property(e => e.NombreComercial)
            .HasColumnName("nombreComercial")
            .HasMaxLength(50)
            .IsUnicode(false);

        builder.Property(e => e.Rnc)
            .HasColumnName("rnc")
            .HasMaxLength(20)
            .IsUnicode(false);

        builder.Property(e => e.NombreRepresentante)
            .HasColumnName("nombreRepresentante")
            .HasMaxLength(50)
            .IsUnicode(false);

        builder.Property(e => e.CedulaRepresentante)
            .HasColumnName("cedulaRepresentante")
            .HasMaxLength(20)
            .IsUnicode(false);

        // Datos Persona Física
        builder.Property(e => e.Identificacion)
            .HasColumnName("identificacion")
            .HasMaxLength(20)
            .IsUnicode(false);

        builder.Property(e => e.Nombre)
            .HasColumnName("nombre")
            .HasMaxLength(50)
            .IsUnicode(false);

        builder.Property(e => e.Apellido)
            .HasColumnName("apellido")
            .HasMaxLength(50)
            .IsUnicode(false);

        builder.Property(e => e.Alias)
            .HasColumnName("alias")
            .HasMaxLength(20)
            .IsUnicode(false);

        // Datos Comunes
        builder.Property(e => e.Direccion)
            .HasColumnName("direccion")
            .HasMaxLength(250)
            .IsUnicode(false);

        builder.Property(e => e.Provincia)
            .HasColumnName("provincia")
            .HasMaxLength(20)
            .IsUnicode(false);

        builder.Property(e => e.Municipio)
            .HasColumnName("municipio")
            .HasMaxLength(20)
            .IsUnicode(false);

        builder.Property(e => e.Telefono1)
            .HasColumnName("telefono1")
            .HasMaxLength(18)
            .IsUnicode(false);

        builder.Property(e => e.Telefono2)
            .HasColumnName("telefono2")
            .HasMaxLength(18)
            .IsUnicode(false);

        builder.Property(e => e.Foto)
            .HasColumnName("foto")
            .IsUnicode(false);

        // Campos de auditoría (heredados de AggregateRoot/AuditableEntity)
        builder.Property(e => e.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired(false);

        builder.Property(e => e.CreatedBy)
            .HasColumnName("created_by")
            .HasMaxLength(100)
            .IsUnicode(false)
            .IsRequired(false);

        builder.Property(e => e.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired(false);

        builder.Property(e => e.UpdatedBy)
            .HasColumnName("updated_by")
            .HasMaxLength(100)
            .IsUnicode(false)
            .IsRequired(false);

        // Índices
        builder.HasIndex(e => e.UserId)
            .HasDatabaseName("IX_EmpleadosTemporales_UserId");

        builder.HasIndex(e => e.Tipo)
            .HasDatabaseName("IX_EmpleadosTemporales_Tipo");

        builder.HasIndex(e => e.Activo)
            .HasDatabaseName("IX_EmpleadosTemporales_Activo");

        builder.HasIndex(e => e.Identificacion)
            .HasDatabaseName("IX_EmpleadosTemporales_Identificacion");

        builder.HasIndex(e => e.Rnc)
            .HasDatabaseName("IX_EmpleadosTemporales_Rnc");

        builder.HasIndex(e => new { e.UserId, e.Activo })
            .HasDatabaseName("IX_EmpleadosTemporales_UserId_Activo");

        // ===========================
        // RELACIONES
        // ===========================
        
        // ✅ RELACIÓN 1: EmpleadoTemporal → DetalleContratacion (1:N)
        // Una contratación temporal puede tener múltiples detalles de trabajo
        builder.HasMany<DetalleContratacion>()
            .WithOne()
            .HasForeignKey(d => d.ContratacionId)
            .HasPrincipalKey(e => e.ContratacionId)
            .HasConstraintName("FK_DetalleContrataciones_EmpleadosTemporales")
            .OnDelete(DeleteBehavior.Restrict); // No borrar empleado si tiene detalles

        // ✅ RELACIÓN 2: EmpleadoTemporal → EmpleadorRecibosHeaderContratacione (1:N)
        // Una contratación temporal puede tener múltiples pagos/recibos
        builder.HasMany<EmpleadorRecibosHeaderContratacione>()
            .WithOne()
            .HasForeignKey(r => r.ContratacionId)
            .HasPrincipalKey(e => e.ContratacionId)
            .HasConstraintName("FK_Empleador_Recibos_Header_Contrataciones_EmpleadosTemporales")
            .OnDelete(DeleteBehavior.Restrict); // No borrar empleado si tiene pagos

        // Ignorar eventos
        builder.Ignore(e => e.Events);
    }
}
