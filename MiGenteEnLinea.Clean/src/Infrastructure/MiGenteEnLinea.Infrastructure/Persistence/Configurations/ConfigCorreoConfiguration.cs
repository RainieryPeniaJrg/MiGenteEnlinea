using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Domain.Entities.Configuracion;

namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuración de Entity Framework para ConfigCorreo
/// Mapea al modelo de dominio con la tabla legacy "Config_Correo"
/// </summary>
public class ConfigCorreoConfiguration : IEntityTypeConfiguration<ConfigCorreo>
{
    public void Configure(EntityTypeBuilder<ConfigCorreo> builder)
    {
        // Tabla
        builder.ToTable("Config_Correo");

        // Clave primaria
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        // Propiedades
        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(70)
            .HasColumnName("email");

        builder.Property(c => c.Pass)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("pass");

        builder.Property(c => c.Servidor)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("servidor");

        builder.Property(c => c.Puerto)
            .IsRequired()
            .HasColumnName("puerto");

        // Índices
        builder.HasIndex(c => c.Email)
            .HasDatabaseName("IX_ConfigCorreo_Email");

        builder.HasIndex(c => c.Servidor)
            .HasDatabaseName("IX_ConfigCorreo_Servidor");

        // Ignorar propiedades computadas
        builder.Ignore(c => c.EstaConfigurada);
    }
}
