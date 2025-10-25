using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Domain.Entities.Authentication;

namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuración de entidad PasswordResetToken
/// </summary>
public class PasswordResetTokenConfiguration : IEntityTypeConfiguration<PasswordResetToken>
{
    public void Configure(EntityTypeBuilder<PasswordResetToken> builder)
    {
        builder.ToTable("PasswordResetTokens");

        // Primary Key
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
            .ValueGeneratedOnAdd();

        // Properties
        builder.Property(t => t.UserId)
            .IsRequired()
            .HasMaxLength(450)
            .HasColumnName("userId");

        builder.Property(t => t.Email)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("email");

        builder.Property(t => t.Token)
            .IsRequired()
            .HasMaxLength(10)
            .HasColumnName("token");

        builder.Property(t => t.ExpiresAt)
            .IsRequired()
            .HasColumnName("expiresAt");

        builder.Property(t => t.UsedAt)
            .HasColumnName("usedAt");

        // Audit fields (from AuditableEntity)
        builder.Property(t => t.CreatedAt)
            .IsRequired()
            .HasColumnName("createdAt")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(t => t.CreatedBy)
            .HasMaxLength(100)
            .HasColumnName("createdBy");

        builder.Property(t => t.UpdatedAt)
            .HasColumnName("updatedAt");

        builder.Property(t => t.UpdatedBy)
            .HasMaxLength(100)
            .HasColumnName("updatedBy");

        // Indexes
        builder.HasIndex(t => t.Token)
            .HasDatabaseName("IX_PasswordResetTokens_Token");

        builder.HasIndex(t => t.UserId)
            .HasDatabaseName("IX_PasswordResetTokens_UserId");

        builder.HasIndex(t => t.Email)
            .HasDatabaseName("IX_PasswordResetTokens_Email");

        // Ignore computed properties
        builder.Ignore(t => t.IsUsed);
        builder.Ignore(t => t.IsExpired);
        builder.Ignore(t => t.IsValid);
    }
}
