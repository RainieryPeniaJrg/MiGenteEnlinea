using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Domain.ReadModels;

namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations.ReadModels;

/// <summary>
/// Configuración de Entity Framework para VistaEmpleado
/// Mapea al read model con la vista legacy "VEmpleados"
/// </summary>
public class VistaEmpleadoConfiguration : IEntityTypeConfiguration<VistaEmpleado>
{
    public void Configure(EntityTypeBuilder<VistaEmpleado> builder)
    {
        builder.ToView("VEmpleados");
        builder.HasNoKey();

        builder.Property(v => v.EmpleadoId).HasColumnName("empleadoID");
        builder.Property(v => v.UserId).HasColumnName("userID").IsUnicode(false);
        builder.Property(v => v.FechaRegistro).HasColumnName("fechaRegistro").HasColumnType("datetime");
        builder.Property(v => v.FechaInicio).HasColumnName("fechaInicio");
        builder.Property(v => v.Identificacion).HasColumnName("identificacion").HasMaxLength(20).IsUnicode(false);
        builder.Property(v => v.Nombre).HasMaxLength(101).IsUnicode(false);
        builder.Property(v => v.Nacimiento).HasColumnName("nacimiento");
        builder.Property(v => v.Direccion).HasColumnName("direccion").HasMaxLength(250).IsUnicode(false);
        builder.Property(v => v.Telefono1).HasColumnName("telefono1").HasMaxLength(20).IsUnicode(false);
        builder.Property(v => v.Telefono2).HasColumnName("telefono2").HasMaxLength(20).IsUnicode(false);
        builder.Property(v => v.Salario).HasColumnName("salario").HasColumnType("decimal(10, 2)");
        builder.Property(v => v.PeriodoPago).HasColumnName("periodoPago");
        builder.Property(v => v.Contrato).HasColumnName("contrato");
        builder.Property(v => v.Activo);
        builder.Property(v => v.Alias).HasColumnName("alias").HasMaxLength(20).IsUnicode(false);
        builder.Property(v => v.EstadoCivil).HasColumnName("estadoCivil");
        builder.Property(v => v.Provincia).HasColumnName("provincia").HasMaxLength(50).IsUnicode(false);
        builder.Property(v => v.Municipio).HasColumnName("municipio").HasMaxLength(20).IsUnicode(false);
        builder.Property(v => v.Posicion).HasColumnName("posicion").HasMaxLength(50).IsUnicode(false);
        builder.Property(v => v.ContactoEmergencia).HasColumnName("contactoEmergencia").HasMaxLength(50).IsUnicode(false);
        builder.Property(v => v.TelefonoEmergencia).HasColumnName("telefonoEmergencia").HasMaxLength(20).IsUnicode(false);
        builder.Property(v => v.RemuneracionExtra1).HasColumnName("remuneracionExtra1").HasMaxLength(100).IsUnicode(false);
        builder.Property(v => v.MontoExtra1).HasColumnName("montoExtra1").HasColumnType("decimal(10, 2)");
        builder.Property(v => v.RemuneracionExtra2).HasColumnName("remuneracionExtra2").HasMaxLength(100).IsUnicode(false);
        builder.Property(v => v.MontoExtra2).HasColumnName("montoExtra2").HasColumnType("decimal(10, 2)");
        builder.Property(v => v.RemuneracionExtra3).HasColumnName("remuneracionExtra3").HasMaxLength(100).IsUnicode(false);
        builder.Property(v => v.MontoExtra3).HasColumnName("montoExtra3").HasColumnType("decimal(10, 2)");
        builder.Property(v => v.Tss).HasColumnName("tss");
    }
}
