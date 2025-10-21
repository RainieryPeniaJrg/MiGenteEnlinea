using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Infrastructure.Persistence.Entities.Generated;

namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuración EF Core para entidad Remuneracione
/// Tabla Legacy: Remuneraciones
/// </summary>
public class RemuneracioneConfiguration : IEntityTypeConfiguration<Remuneracione>
{
    public void Configure(EntityTypeBuilder<Remuneracione> builder)
    {
        // Tabla
        builder.ToTable("Remuneraciones");

        // Primary Key
        builder.HasKey(e => e.Id)
            .HasName("PK_Remuneraciones");

        // Propiedades
        builder.Property(e => e.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.UserId)
            .IsRequired()
            .HasMaxLength(450)
            .HasColumnName("userID");

        builder.Property(e => e.Descripcion)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnName("descripcion");

        builder.Property(e => e.Monto)
            .HasColumnType("decimal(18, 2)")
            .HasColumnName("monto");

        builder.Property(e => e.EmpleadoId)
            .HasColumnName("empleadoID");

        // Foreign Keys
        builder.HasOne(d => d.Empleado)
            .WithMany(p => p.Remuneraciones)
            .HasForeignKey(d => d.EmpleadoId)
            .HasConstraintName("FK_Remuneraciones_Empleados")
            .OnDelete(DeleteBehavior.Cascade);

        // Indices
        builder.HasIndex(e => e.EmpleadoId, "IX_Remuneraciones_EmpleadoId");
        builder.HasIndex(e => e.UserId, "IX_Remuneraciones_UserId");
    }
}
