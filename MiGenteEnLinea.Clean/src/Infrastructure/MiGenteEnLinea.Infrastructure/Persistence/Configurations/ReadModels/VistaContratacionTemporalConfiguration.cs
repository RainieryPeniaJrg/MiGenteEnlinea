using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Domain.ReadModels;

namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations.ReadModels;

/// <summary>
/// Configuración de Entity Framework para VistaContratacionTemporal
/// Mapea al read model con la vista legacy "VContratacionesTemporales"
/// </summary>
public class VistaContratacionTemporalConfiguration : IEntityTypeConfiguration<VistaContratacionTemporal>
{
    public void Configure(EntityTypeBuilder<VistaContratacionTemporal> builder)
    {
        builder.ToView("VContratacionesTemporales");
        builder.HasNoKey();

        builder.Property(v => v.ContratacionId).HasColumnName("contratacionID");
        builder.Property(v => v.UserId).HasColumnName("userID").HasMaxLength(100).IsUnicode(false);
        builder.Property(v => v.FechaRegistro).HasColumnName("fechaRegistro").HasColumnType("datetime");
        builder.Property(v => v.Tipo).HasColumnName("tipo");
        builder.Property(v => v.NombreComercial).HasColumnName("nombreComercial").HasMaxLength(50).IsUnicode(false);
        builder.Property(v => v.Rnc).HasColumnName("rnc").HasMaxLength(20).IsUnicode(false);
        builder.Property(v => v.Identificacion).HasColumnName("identificacion").HasMaxLength(20).IsUnicode(false);
        builder.Property(v => v.Nombre).HasColumnName("nombre").HasMaxLength(50).IsUnicode(false);
        builder.Property(v => v.Apellido).HasColumnName("apellido").HasMaxLength(50).IsUnicode(false);
        builder.Property(v => v.Alias).HasColumnName("alias").HasMaxLength(20).IsUnicode(false);
        builder.Property(v => v.Direccion).HasColumnName("direccion").HasMaxLength(250).IsUnicode(false);
        builder.Property(v => v.Provincia).HasColumnName("provincia").HasMaxLength(20).IsUnicode(false);
        builder.Property(v => v.Municipio).HasColumnName("municipio").HasMaxLength(20).IsUnicode(false);
        builder.Property(v => v.Telefono1).HasColumnName("telefono1").HasMaxLength(18).IsUnicode(false);
        builder.Property(v => v.Telefono2).HasColumnName("telefono2").HasMaxLength(18).IsUnicode(false);
        builder.Property(v => v.DetalleId).HasColumnName("detalleID");
        builder.Property(v => v.DescripcionCorta).HasColumnName("descripcionCorta").HasMaxLength(60).IsUnicode(false);
        builder.Property(v => v.DescripcionAmpliada).HasColumnName("descripcionAmpliada").HasMaxLength(250).IsUnicode(false);
        builder.Property(v => v.FechaInicio).HasColumnName("fechaInicio");
        builder.Property(v => v.FechaFinal).HasColumnName("fechaFinal");
        builder.Property(v => v.MontoAcordado).HasColumnName("montoAcordado").HasColumnType("decimal(10, 2)");
        builder.Property(v => v.EsquemaPagos).HasColumnName("esquemaPagos").HasMaxLength(50).IsUnicode(false);
        builder.Property(v => v.Estatus).HasColumnName("estatus");
        builder.Property(v => v.ComposicionNombre).HasColumnName("composicionNombre").HasMaxLength(101).IsUnicode(false);
        builder.Property(v => v.ComposicionId).HasColumnName("composicionID").HasMaxLength(20).IsUnicode(false);
        builder.Property(v => v.Conocimientos).HasColumnName("conocimientos");
        builder.Property(v => v.Puntualidad).HasColumnName("puntualidad");
        builder.Property(v => v.Recomendacion).HasColumnName("recomendacion");
        builder.Property(v => v.Cumplimiento).HasColumnName("cumplimiento");
    }
}
