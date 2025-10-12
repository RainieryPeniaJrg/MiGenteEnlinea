using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Domain.Entities.Suscripciones;

namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuración de Entity Framework Core para la entidad PlanEmpleador.
/// </summary>
public class PlanEmpleadorConfiguration : IEntityTypeConfiguration<PlanEmpleador>
{
    public void Configure(EntityTypeBuilder<PlanEmpleador> builder)
    {
        builder.ToTable("Planes_empleadores");

        // Primary Key
        builder.HasKey(p => p.PlanId);
        builder.Property(p => p.PlanId)
            .HasColumnName("planID")
            .ValueGeneratedOnAdd();

        // Properties
        builder.Property(p => p.Nombre)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnName("nombre")
            .IsUnicode(false);

        builder.Property(p => p.Precio)
            .IsRequired()
            .HasColumnType("decimal(18, 2)")
            .HasColumnName("precio");

        builder.Property(p => p.LimiteEmpleados)
            .IsRequired()
            .HasColumnName("empleados");

        builder.Property(p => p.MesesHistorico)
            .IsRequired()
            .HasColumnName("historico");

        builder.Property(p => p.IncluyeNomina)
            .IsRequired()
            .HasColumnName("nomina");

        builder.Property(p => p.Activo)
            .IsRequired()
            .HasColumnName("activo")
            .HasDefaultValue(true);

        // Audit Fields (nuevos campos, nullable para compatibilidad con datos existentes)
        builder.Property(p => p.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired(false);

        builder.Property(p => p.CreatedBy)
            .HasColumnName("created_by")
            .HasMaxLength(450)
            .IsRequired(false);

        builder.Property(p => p.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired(false);

        builder.Property(p => p.UpdatedBy)
            .HasColumnName("updated_by")
            .HasMaxLength(450)
            .IsRequired(false);

        // Indexes
        builder.HasIndex(p => p.Nombre)
            .HasDatabaseName("IX_PlanesEmpleadores_Nombre");

        builder.HasIndex(p => p.Activo)
            .HasDatabaseName("IX_PlanesEmpleadores_Activo");

        builder.HasIndex(p => p.Precio)
            .HasDatabaseName("IX_PlanesEmpleadores_Precio");

        // Ignore domain events
        builder.Ignore(p => p.Events);
    }
}
