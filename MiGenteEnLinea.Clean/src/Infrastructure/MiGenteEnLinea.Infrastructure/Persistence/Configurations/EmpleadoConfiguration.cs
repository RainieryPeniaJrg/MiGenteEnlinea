using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Domain.Entities.Empleados;
using MiGenteEnLinea.Domain.Entities.Nominas;

namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuración de Entity Framework Core para la entidad Empleado.
/// Mapea a la tabla legacy "Empleados".
/// </summary>
public sealed class EmpleadoConfiguration : IEntityTypeConfiguration<Empleado>
{
    public void Configure(EntityTypeBuilder<Empleado> builder)
    {
        // Tabla
        builder.ToTable("Empleados");

        // Primary Key
        builder.HasKey(e => e.EmpleadoId);
        builder.Property(e => e.EmpleadoId)
            .HasColumnName("empleadoID")
            .ValueGeneratedOnAdd();

        // Propiedades requeridas
        builder.Property(e => e.UserId)
            .IsRequired()
            .HasColumnName("userID")
            .HasMaxLength(250) // Debe coincidir con Credenciales.userID
            .IsUnicode(false);

        builder.Property(e => e.Identificacion)
            .IsRequired()
            .HasColumnName("identificacion")
            .HasMaxLength(20)
            .IsUnicode(false);

        builder.Property(e => e.Nombre)
            .IsRequired()
            .HasColumnName("Nombre")
            .HasMaxLength(50)
            .IsUnicode(false);

        builder.Property(e => e.Apellido)
            .IsRequired()
            .HasColumnName("Apellido")
            .HasMaxLength(50)
            .IsUnicode(false);

        builder.Property(e => e.Salario)
            .IsRequired()
            .HasColumnName("salario")
            .HasColumnType("decimal(10, 2)");

        builder.Property(e => e.PeriodoPago)
            .IsRequired()
            .HasColumnName("periodoPago");

        builder.Property(e => e.Activo)
            .IsRequired()
            .HasColumnName("Activo")
            .HasDefaultValue(true);

        builder.Property(e => e.TieneContrato)
            .IsRequired()
            .HasColumnName("contrato")
            .HasDefaultValue(false);

        builder.Property(e => e.InscritoTss)
            .IsRequired()
            .HasColumnName("tss")
            .HasDefaultValue(false);

        // Propiedades opcionales
        builder.Property(e => e.FechaRegistro)
            .IsRequired()
            .HasColumnName("fechaRegistro")
            .HasColumnType("datetime");

        builder.Property(e => e.FechaInicio)
            .HasColumnName("fechaInicio");

        builder.Property(e => e.Alias)
            .HasColumnName("alias")
            .HasMaxLength(20)
            .IsUnicode(false);

        builder.Property(e => e.Nacimiento)
            .HasColumnName("nacimiento");

        builder.Property(e => e.EstadoCivil)
            .HasColumnName("estadoCivil");

        builder.Property(e => e.Direccion)
            .HasColumnName("direccion")
            .HasMaxLength(250)
            .IsUnicode(false);

        builder.Property(e => e.Provincia)
            .HasColumnName("provincia")
            .HasMaxLength(50)
            .IsUnicode(false);

        builder.Property(e => e.Municipio)
            .HasColumnName("municipio")
            .HasMaxLength(20)
            .IsUnicode(false);

        builder.Property(e => e.Telefono1)
            .HasColumnName("telefono1")
            .HasMaxLength(20)
            .IsUnicode(false);

        builder.Property(e => e.Telefono2)
            .HasColumnName("telefono2")
            .HasMaxLength(20)
            .IsUnicode(false);

        builder.Property(e => e.Posicion)
            .HasColumnName("posicion")
            .HasMaxLength(50)
            .IsUnicode(false);

        builder.Property(e => e.ContactoEmergencia)
            .HasColumnName("contactoEmergencia")
            .HasMaxLength(50)
            .IsUnicode(false);

        builder.Property(e => e.TelefonoEmergencia)
            .HasColumnName("telefonoEmergencia")
            .HasMaxLength(20)
            .IsUnicode(false);

        // Remuneraciones extras
        builder.Property(e => e.RemuneracionExtra1)
            .HasColumnName("remuneracionExtra1")
            .HasMaxLength(100)
            .IsUnicode(false);

        builder.Property(e => e.MontoExtra1)
            .HasColumnName("montoExtra1")
            .HasColumnType("decimal(10, 2)");

        builder.Property(e => e.RemuneracionExtra2)
            .HasColumnName("remuneracionExtra2")
            .HasMaxLength(100)
            .IsUnicode(false);

        builder.Property(e => e.MontoExtra2)
            .HasColumnName("montoExtra2")
            .HasColumnType("decimal(10, 2)");

        builder.Property(e => e.RemuneracionExtra3)
            .HasColumnName("remuneracionExtra3")
            .HasMaxLength(100)
            .IsUnicode(false);

        builder.Property(e => e.MontoExtra3)
            .HasColumnName("montoExtra3")
            .HasColumnType("decimal(10, 2)");

        // Campos de baja
        builder.Property(e => e.FechaSalida)
            .HasColumnName("fechaSalida")
            .HasColumnType("datetime");

        builder.Property(e => e.MotivoBaja)
            .HasColumnName("motivoBaja")
            .HasMaxLength(20)
            .IsUnicode(false);

        builder.Property(e => e.Prestaciones)
            .HasColumnName("prestaciones")
            .HasColumnType("decimal(10, 2)");

        builder.Property(e => e.Foto)
            .HasColumnName("foto")
            .IsUnicode(false);

        builder.Property(e => e.DiasPago)
            .HasColumnName("diasPago");

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
            .HasDatabaseName("IX_Empleados_UserId");

        builder.HasIndex(e => e.Identificacion)
            .HasDatabaseName("IX_Empleados_Identificacion");

        builder.HasIndex(e => e.Activo)
            .HasDatabaseName("IX_Empleados_Activo");

        builder.HasIndex(e => new { e.UserId, e.Activo })
            .HasDatabaseName("IX_Empleados_UserId_Activo");

        // ===========================
        // RELACIONES
        // ===========================
        
        // ✅ RELACIÓN: Empleado → ReciboHeader (1:N)
        // Un empleado puede tener múltiples recibos de nómina
        // Nota: Sin propiedades de navegación (DDD puro)
        builder.HasMany<ReciboHeader>()
            .WithOne()
            .HasForeignKey(r => r.EmpleadoId)
            .HasPrincipalKey(e => e.EmpleadoId)
            .HasConstraintName("FK_Empleador_Recibos_Header_Empleados")
            .OnDelete(DeleteBehavior.Restrict); // No borrar empleado si tiene recibos

        // Ignorar propiedades calculadas y eventos
        builder.Ignore(e => e.NombreCompleto);
        builder.Ignore(e => e.Events);
    }
}
