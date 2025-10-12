using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Domain.ReadModels;

namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations.ReadModels;

/// <summary>
/// Configuración de Entity Framework para VistaPerfil
/// Mapea al read model con la vista legacy "VPerfiles"
/// </summary>
public class VistaPerfilConfiguration : IEntityTypeConfiguration<VistaPerfil>
{
    public void Configure(EntityTypeBuilder<VistaPerfil> builder)
    {
        builder.ToView("VPerfiles");
        builder.HasNoKey();

        builder.Property(v => v.PerfilId).HasColumnName("perfilID");
        builder.Property(v => v.FechaCreacion).HasColumnName("fechaCreacion").HasColumnType("datetime");
        builder.Property(v => v.UserId).HasColumnName("userID").IsUnicode(false);
        builder.Property(v => v.Tipo);
        builder.Property(v => v.Nombre).HasMaxLength(20).IsUnicode(false);
        builder.Property(v => v.Apellido).HasMaxLength(50).IsUnicode(false);
        builder.Property(v => v.Email).HasMaxLength(100).IsUnicode(false);
        builder.Property(v => v.Telefono1).HasColumnName("telefono1").HasMaxLength(20).IsUnicode(false);
        builder.Property(v => v.Telefono2).HasColumnName("telefono2").HasMaxLength(20).IsUnicode(false);
        builder.Property(v => v.Usuario).HasColumnName("usuario").HasMaxLength(20).IsUnicode(false);
        builder.Property(v => v.Id).HasColumnName("id");
        builder.Property(v => v.TipoIdentificacion).HasColumnName("tipoIdentificacion");
        builder.Property(v => v.Identificacion).HasColumnName("identificacion").HasMaxLength(20).IsUnicode(false);
        builder.Property(v => v.Direccion).HasColumnName("direccion").HasColumnType("text");
        builder.Property(v => v.FotoPerfil).HasColumnName("fotoPerfil");
        builder.Property(v => v.Presentacion).HasColumnName("presentacion").HasColumnType("text");
        builder.Property(v => v.NombreComercial).HasColumnName("nombreComercial").HasMaxLength(50).IsUnicode(false);
        builder.Property(v => v.CedulaGerente).HasColumnName("cedulaGerente").HasMaxLength(20).IsUnicode(false);
        builder.Property(v => v.NombreGerente).HasColumnName("nombreGerente").HasMaxLength(50).IsUnicode(false);
        builder.Property(v => v.ApellidoGerente).HasColumnName("apellidoGerente").HasMaxLength(50).IsUnicode(false);
        builder.Property(v => v.DireccionGerente).HasColumnName("direccionGerente").HasMaxLength(250).IsUnicode(false);
    }
}
