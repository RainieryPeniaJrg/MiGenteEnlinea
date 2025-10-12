using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Domain.Entities.Seguridad;

namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuración de Entity Framework para la entidad Perfile
/// </summary>
public class PerfileConfiguration : IEntityTypeConfiguration<Perfile>
{
    public void Configure(EntityTypeBuilder<Perfile> builder)
    {
        // Nombre de tabla
        builder.ToTable("Perfiles");

        // Primary Key
        builder.HasKey(p => p.PerfilId);
        builder.Property(p => p.PerfilId)
            .HasColumnName("perfilID")
            .ValueGeneratedOnAdd();

        // FechaCreacion
        builder.Property(p => p.FechaCreacion)
            .IsRequired()
            .HasColumnName("fechaCreacion")
            .HasColumnType("datetime");

        // UserId
        builder.Property(p => p.UserId)
            .IsRequired()
            .HasMaxLength(450)
            .HasColumnName("userID")
            .IsUnicode(false);

        // Tipo (1 = Empleador, 2 = Contratista)
        builder.Property(p => p.Tipo)
            .IsRequired()
            .HasColumnName("Tipo");

        // Nombre
        builder.Property(p => p.Nombre)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnName("Nombre")
            .IsUnicode(false);

        // Apellido
        builder.Property(p => p.Apellido)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("Apellido")
            .IsUnicode(false);

        // Email
        builder.Property(p => p.Email)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("Email")
            .IsUnicode(false);

        // Telefono1
        builder.Property(p => p.Telefono1)
            .HasMaxLength(20)
            .HasColumnName("telefono1")
            .IsUnicode(false);

        // Telefono2
        builder.Property(p => p.Telefono2)
            .HasMaxLength(20)
            .HasColumnName("telefono2")
            .IsUnicode(false);

        // Usuario
        builder.Property(p => p.Usuario)
            .HasMaxLength(20)
            .HasColumnName("usuario")
            .IsUnicode(false);

        // Ignorar propiedades calculadas
        builder.Ignore(p => p.NombreCompleto);
        builder.Ignore(p => p.EsEmpleador);
        builder.Ignore(p => p.EsContratista);

        // Mapeo de campos de auditoría heredados
        builder.Property(p => p.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("datetime");

        builder.Property(p => p.CreatedBy)
            .HasColumnName("created_by")
            .HasMaxLength(450)
            .IsUnicode(false);

        builder.Property(p => p.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("datetime");

        builder.Property(p => p.UpdatedBy)
            .HasColumnName("updated_by")
            .HasMaxLength(450)
            .IsUnicode(false);

        // Ignorar colección de eventos de dominio
        builder.Ignore(p => p.Events);

        // Índices para optimización de consultas
        builder.HasIndex(p => p.UserId)
            .HasDatabaseName("IX_Perfiles_UserId");

        builder.HasIndex(p => p.Tipo)
            .HasDatabaseName("IX_Perfiles_Tipo");

        builder.HasIndex(p => p.Email)
            .HasDatabaseName("IX_Perfiles_Email");

        builder.HasIndex(p => new { p.Tipo, p.FechaCreacion })
            .HasDatabaseName("IX_Perfiles_Tipo_FechaCreacion");

        // Índice único: un solo perfil por usuario
        builder.HasIndex(p => p.UserId)
            .IsUnique()
            .HasDatabaseName("UX_Perfiles_UserId");
    }
}
