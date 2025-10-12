using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Domain.ReadModels;

namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations.ReadModels;

/// <summary>
/// Configuración de Entity Framework para VistaSuscripcion
/// Mapea al read model con la vista legacy "VSuscripciones"
/// </summary>
public class VistaSuscripcionConfiguration : IEntityTypeConfiguration<VistaSuscripcion>
{
    public void Configure(EntityTypeBuilder<VistaSuscripcion> builder)
    {
        builder.ToView("VSuscripciones");
        builder.HasNoKey();

        builder.Property(v => v.SuscripcionId).HasColumnName("suscripcionID");
        builder.Property(v => v.UserId).HasColumnName("userID").IsUnicode(false);
        builder.Property(v => v.PlanId).HasColumnName("planID");
        builder.Property(v => v.Vencimiento).HasColumnName("vencimiento");
        builder.Property(v => v.Nombre).HasColumnName("nombre").HasMaxLength(50).IsUnicode(false);
        builder.Property(v => v.ProximoPago).HasColumnType("datetime");
        builder.Property(v => v.FechaInicio).HasColumnName("fechaInicio");
    }
}
