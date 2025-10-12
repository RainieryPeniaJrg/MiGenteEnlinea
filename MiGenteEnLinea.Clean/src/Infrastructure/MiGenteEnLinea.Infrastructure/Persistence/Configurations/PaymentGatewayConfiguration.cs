using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Domain.Entities.Pagos;

namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuración de Entity Framework Core para la entidad PaymentGateway.
/// </summary>
public class PaymentGatewayConfiguration : IEntityTypeConfiguration<PaymentGateway>
{
    public void Configure(EntityTypeBuilder<PaymentGateway> builder)
    {
        builder.ToTable("PaymentGateway");

        // Primary Key
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        // Properties
        builder.Property(p => p.ModoTest)
            .IsRequired()
            .HasColumnName("test");

        builder.Property(p => p.UrlProduccion)
            .IsRequired()
            .HasMaxLength(150)
            .HasColumnName("productionURL")
            .IsUnicode(false);

        builder.Property(p => p.UrlTest)
            .IsRequired()
            .HasMaxLength(150)
            .HasColumnName("testURL")
            .IsUnicode(false);

        builder.Property(p => p.MerchantId)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnName("merchantID")
            .IsUnicode(false);

        builder.Property(p => p.TerminalId)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnName("terminalID")
            .IsUnicode(false);

        builder.Property(p => p.Activa)
            .IsRequired()
            .HasColumnName("activa")
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
        builder.HasIndex(p => p.MerchantId)
            .HasDatabaseName("IX_PaymentGateway_MerchantId");

        builder.HasIndex(p => p.ModoTest)
            .HasDatabaseName("IX_PaymentGateway_ModoTest");

        builder.HasIndex(p => p.Activa)
            .HasDatabaseName("IX_PaymentGateway_Activa");

        // Ignore domain events
        builder.Ignore(p => p.Events);
    }
}
