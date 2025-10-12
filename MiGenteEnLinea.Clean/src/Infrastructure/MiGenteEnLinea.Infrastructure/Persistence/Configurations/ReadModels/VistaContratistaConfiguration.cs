using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Domain.ReadModels;

namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations.ReadModels;

/// <summary>
/// Configuración de Entity Framework para VistaContratista
/// Mapea al read model con la vista legacy "VContratistas"
/// </summary>
public class VistaContratistaConfiguration : IEntityTypeConfiguration<VistaContratista>
{
    public void Configure(EntityTypeBuilder<VistaContratista> builder)
    {
        builder.ToView("VContratistas");
        builder.HasNoKey();

        builder.Property(v => v.ContratistaId).HasColumnName("contratistaID");
        builder.Property(v => v.FechaIngreso).HasColumnName("fechaIngreso").HasColumnType("datetime");
        builder.Property(v => v.UserId).HasColumnName("userID").HasMaxLength(250).IsUnicode(false);
        builder.Property(v => v.Titulo).HasColumnName("titulo").HasMaxLength(70).IsUnicode(false);
        builder.Property(v => v.Tipo).HasColumnName("tipo");
        builder.Property(v => v.Identificacion).HasColumnName("identificacion").HasMaxLength(20).IsUnicode(false);
        builder.Property(v => v.Nombre).HasMaxLength(20).IsUnicode(false);
        builder.Property(v => v.Apellido).HasMaxLength(50).IsUnicode(false);
        builder.Property(v => v.Sector).HasColumnName("sector").HasMaxLength(40).IsUnicode(false);
        builder.Property(v => v.Experiencia).HasColumnName("experiencia");
        builder.Property(v => v.Presentacion).HasColumnName("presentacion").HasMaxLength(250).IsUnicode(false);
        builder.Property(v => v.Telefono1).HasColumnName("telefono1").HasMaxLength(16).IsUnicode(false);
        builder.Property(v => v.Whatsapp1).HasColumnName("whatsapp1");
        builder.Property(v => v.Whatsapp2).HasColumnName("whatsapp2");
        builder.Property(v => v.Email).HasColumnName("email").HasMaxLength(50).IsUnicode(false);
        builder.Property(v => v.Activo).HasColumnName("activo");
        builder.Property(v => v.Provincia).HasColumnName("provincia").HasMaxLength(50).IsUnicode(false);
        builder.Property(v => v.NivelNacional).HasColumnName("nivelNacional");
        builder.Property(v => v.Calificacion).HasColumnName("calificacion").HasColumnType("decimal(10, 2)");
        builder.Property(v => v.TotalRegistros).HasColumnName("total_registros");
        builder.Property(v => v.Telefono2).HasColumnName("telefono2").HasMaxLength(20).IsUnicode(false);
        builder.Property(v => v.ImagenUrl).HasColumnName("imagenURL").HasMaxLength(150).IsUnicode(false);
    }
}
