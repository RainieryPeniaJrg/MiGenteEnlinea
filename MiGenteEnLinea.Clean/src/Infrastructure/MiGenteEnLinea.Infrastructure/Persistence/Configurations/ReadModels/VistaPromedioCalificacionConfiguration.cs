using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Domain.ReadModels;

namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations.ReadModels;

/// <summary>
/// Configuración de Entity Framework para VistaPromedioCalificacion
/// Mapea al read model con la vista legacy "VPromedioCalificacion"
/// </summary>
public class VistaPromedioCalificacionConfiguration : IEntityTypeConfiguration<VistaPromedioCalificacion>
{
    public void Configure(EntityTypeBuilder<VistaPromedioCalificacion> builder)
    {
        builder.ToView("VPromedioCalificacion");
        builder.HasNoKey();

        builder.Property(v => v.Identificacion).HasColumnName("identificacion").HasMaxLength(20).IsUnicode(false);
        builder.Property(v => v.CalificacionPromedio).HasColumnName("calificacion_promedio").HasColumnType("decimal(10, 2)");
        builder.Property(v => v.TotalRegistros).HasColumnName("total_registros");
    }
}
