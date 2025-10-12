using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Domain.ReadModels;

namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations.ReadModels;

/// <summary>
/// Configuración de Entity Framework para VistaPago
/// Mapea al read model con la vista legacy "VPagos"
/// </summary>
public class VistaPagoConfiguration : IEntityTypeConfiguration<VistaPago>
{
    public void Configure(EntityTypeBuilder<VistaPago> builder)
    {
        builder.ToView("VPagos");
        builder.HasNoKey();

        builder.Property(v => v.PagoId).HasColumnName("pagoID");
        builder.Property(v => v.UserId).HasColumnName("userID").IsUnicode(false);
        builder.Property(v => v.EmpleadoId).HasColumnName("empleadoID");
        builder.Property(v => v.FechaRegistro).HasColumnName("fechaRegistro").HasColumnType("datetime");
        builder.Property(v => v.FechaPago).HasColumnName("fechaPago").HasColumnType("datetime");
        builder.Property(v => v.Expr1).HasMaxLength(18).IsUnicode(false);
        builder.Property(v => v.Monto).HasColumnType("decimal(38, 2)");
    }
}
