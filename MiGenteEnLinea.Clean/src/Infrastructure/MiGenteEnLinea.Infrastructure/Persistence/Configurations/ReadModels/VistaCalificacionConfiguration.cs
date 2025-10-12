using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Domain.ReadModels;

namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations.ReadModels;

/// <summary>
/// Configuración de Entity Framework para VistaCalificacion
/// Mapea al read model con la vista legacy "VCalificaciones"
/// </summary>
public class VistaCalificacionConfiguration : IEntityTypeConfiguration<VistaCalificacion>
{
    public void Configure(EntityTypeBuilder<VistaCalificacion> builder)
    {
        // Mapea a vista de base de datos (solo lectura)
        builder.ToView("VCalificaciones");

        // Las vistas no tienen clave primaria
        builder.HasNoKey();

        // Configuración de propiedades
        builder.Property(v => v.CalificacionId).HasColumnName("calificacionID");
        builder.Property(v => v.Fecha).HasColumnName("fecha").HasColumnType("datetime");
        builder.Property(v => v.UserId).HasColumnName("userID").HasMaxLength(250).IsUnicode(false);
        builder.Property(v => v.Tipo).HasColumnName("tipo").HasMaxLength(20).IsUnicode(false);
        builder.Property(v => v.Identificacion).HasColumnName("identificacion").HasMaxLength(20).IsUnicode(false);
        builder.Property(v => v.Nombre).HasColumnName("nombre").HasMaxLength(100).IsUnicode(false);
        builder.Property(v => v.Puntualidad).HasColumnName("puntualidad");
        builder.Property(v => v.Cumplimiento).HasColumnName("cumplimiento");
        builder.Property(v => v.Conocimientos).HasColumnName("conocimientos");
        builder.Property(v => v.Recomendacion).HasColumnName("recomendacion");
        builder.Property(v => v.PerfilId).HasColumnName("perfilID");
        builder.Property(v => v.FechaCreacion).HasColumnName("fechaCreacion").HasColumnType("datetime");
        builder.Property(v => v.Expr1).IsUnicode(false);
        builder.Property(v => v.Expr2);
        builder.Property(v => v.Expr3).HasMaxLength(20).IsUnicode(false);
        builder.Property(v => v.Apellido).HasMaxLength(50).IsUnicode(false);
        builder.Property(v => v.Email).HasMaxLength(100).IsUnicode(false);
        builder.Property(v => v.Telefono1).HasColumnName("telefono1").HasMaxLength(20).IsUnicode(false);
        builder.Property(v => v.Telefono2).HasColumnName("telefono2").HasMaxLength(20).IsUnicode(false);
        builder.Property(v => v.Usuario).HasColumnName("usuario").HasMaxLength(20).IsUnicode(false);
    }
}
