using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Domain.Entities.Authentication;
using MiGenteEnLinea.Domain.ValueObjects;

namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuración de Fluent API para la entidad Credencial.
/// Mapea la entidad de dominio a la tabla legacy "Credenciales".
/// </summary>
public sealed class CredencialConfiguration : IEntityTypeConfiguration<Credencial>
{
    public void Configure(EntityTypeBuilder<Credencial> builder)
    {
        // Mapeo a tabla existente
        builder.ToTable("Credenciales");

        // Clave primaria
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        // UserId - Mapeo a columna legacy
        builder.Property(c => c.UserId)
            .IsRequired()
            .HasColumnName("userID")
            .HasMaxLength(250) // Debe coincidir con FKs (Calificaciones, Suscripciones, etc.)
            .IsUnicode(false);

        // Email - Mapeo con Value Object
        builder.Property(c => c.Email)
            .HasConversion(
                email => email.Value, // De Email a string
                value => Email.CreateUnsafe(value)) // De string a Email
            .IsRequired()
            .HasColumnName("email")
            .HasMaxLength(100)
            .IsUnicode(false);

        // PasswordHash - Mapeo a columna "password" (legacy name)
        builder.Property(c => c.PasswordHash)
            .IsRequired()
            .HasColumnName("password")
            .IsUnicode(false);

        // Activo
        builder.Property(c => c.Activo)
            .IsRequired()
            .HasColumnName("activo")
            .HasDefaultValue(false);

        // FechaActivacion - Nueva columna (agregada en migración)
        builder.Property(c => c.FechaActivacion)
            .HasColumnName("fecha_activacion")
            .HasColumnType("datetime2");

        // UltimoAcceso - Nueva columna (agregada en migración)
        builder.Property(c => c.UltimoAcceso)
            .HasColumnName("ultimo_acceso")
            .HasColumnType("datetime2");

        // UltimaIp - Nueva columna (agregada en migración)
        builder.Property(c => c.UltimaIp)
            .HasColumnName("ultima_ip")
            .HasMaxLength(45) // IPv6 max length
            .IsUnicode(false);

        // Campos de auditoría (base class AuditableEntity)
        builder.Property(c => c.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at")
            .HasColumnType("datetime2")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(c => c.CreatedBy)
            .HasColumnName("created_by")
            .HasMaxLength(50)
            .IsUnicode(false);

        builder.Property(c => c.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("datetime2");

        builder.Property(c => c.UpdatedBy)
            .HasColumnName("updated_by")
            .HasMaxLength(50)
            .IsUnicode(false);

        // Índices para performance
        builder.HasIndex(c => c.UserId)
            .IsUnique()
            .HasDatabaseName("IX_Credenciales_UserID");

        builder.HasIndex(c => c.Email)
            .IsUnique()
            .HasDatabaseName("IX_Credenciales_Email");

        builder.HasIndex(c => c.Activo)
            .HasDatabaseName("IX_Credenciales_Activo");

        // Ignorar propiedad de navegación de eventos (no se persiste)
        builder.Ignore(c => c.Events);
    }
}
