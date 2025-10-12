using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Domain.Entities.Suscripciones;

namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuración de Entity Framework Core para la entidad PlanContratista.
/// </summary>
public class PlanContratistaConfiguration : IEntityTypeConfiguration<PlanContratista>
{
    public void Configure(EntityTypeBuilder<PlanContratista> builder)
    {
        builder.ToTable("Planes_Contratistas");

        // Primary Key
        builder.HasKey(p => p.PlanId);
        builder.Property(p => p.PlanId)
            .HasColumnName("planID")
            .ValueGeneratedOnAdd();

        // Properties
        builder.Property(p => p.NombrePlan)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("nombrePlan")
            .IsUnicode(false);

        builder.Property(p => p.Precio)
            .IsRequired()
            .HasColumnType("decimal(10, 2)")
            .HasColumnName("precio");

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
        builder.HasIndex(p => p.NombrePlan)
            .HasDatabaseName("IX_PlanesContratistas_NombrePlan");

        builder.HasIndex(p => p.Activo)
            .HasDatabaseName("IX_PlanesContratistas_Activo");

        builder.HasIndex(p => p.Precio)
            .HasDatabaseName("IX_PlanesContratistas_Precio");

        // Ignore domain events
        builder.Ignore(p => p.Events);
    }
}
