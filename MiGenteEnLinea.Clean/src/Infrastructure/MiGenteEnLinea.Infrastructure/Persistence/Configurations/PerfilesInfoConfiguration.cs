using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Domain.Entities.Seguridad;

namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuración de Entity Framework para la entidad PerfilesInfo
/// </summary>
public class PerfilesInfoConfiguration : IEntityTypeConfiguration<PerfilesInfo>
{
    public void Configure(EntityTypeBuilder<PerfilesInfo> builder)
    {
        // Nombre de tabla
        builder.ToTable("perfilesInfo");

        // Primary Key
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        // PerfilId (FK nullable)
        builder.Property(p => p.PerfilId)
            .HasColumnName("perfilID");

        // UserId
        builder.Property(p => p.UserId)
            .IsRequired()
            .HasMaxLength(250) // Debe coincidir con Credenciales.userID
            .HasColumnName("userID")
            .IsUnicode(false);

        // TipoIdentificacion (1 = Cédula, 2 = Pasaporte, 3 = RNC)
        builder.Property(p => p.TipoIdentificacion)
            .HasColumnName("tipoIdentificacion");

        // NombreComercial
        builder.Property(p => p.NombreComercial)
            .HasMaxLength(50)
            .HasColumnName("nombreComercial")
            .IsUnicode(false);

        // Identificacion
        builder.Property(p => p.Identificacion)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnName("identificacion")
            .IsUnicode(false);

        // Direccion
        builder.Property(p => p.Direccion)
            .HasColumnName("direccion")
            .HasColumnType("text");

        // FotoPerfil (binary)
        builder.Property(p => p.FotoPerfil)
            .HasColumnName("fotoPerfil")
            .HasColumnType("varbinary(max)");

        // Presentacion
        builder.Property(p => p.Presentacion)
            .HasColumnName("presentacion")
            .HasColumnType("text");

        // CedulaGerente
        builder.Property(p => p.CedulaGerente)
            .HasMaxLength(20)
            .HasColumnName("cedulaGerente")
            .IsUnicode(false);

        // NombreGerente
        builder.Property(p => p.NombreGerente)
            .HasMaxLength(50)
            .HasColumnName("nombreGerente")
            .IsUnicode(false);

        // ApellidoGerente
        builder.Property(p => p.ApellidoGerente)
            .HasMaxLength(50)
            .HasColumnName("apellidoGerente")
            .IsUnicode(false);

        // DireccionGerente
        builder.Property(p => p.DireccionGerente)
            .HasMaxLength(250)
            .HasColumnName("direccionGerente")
            .IsUnicode(false);

        // Ignorar propiedades calculadas
        builder.Ignore(p => p.NombreCompletoGerente);
        builder.Ignore(p => p.TieneFotoPerfil);
        builder.Ignore(p => p.EsEmpresa);
        builder.Ignore(p => p.TieneInformacionGerente);

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
            .HasDatabaseName("IX_PerfilesInfo_UserId");

        builder.HasIndex(p => p.PerfilId)
            .HasDatabaseName("IX_PerfilesInfo_PerfilId");

        builder.HasIndex(p => p.Identificacion)
            .HasDatabaseName("IX_PerfilesInfo_Identificacion");

        builder.HasIndex(p => p.TipoIdentificacion)
            .HasDatabaseName("IX_PerfilesInfo_TipoIdentificacion");

        builder.HasIndex(p => new { p.UserId, p.Identificacion })
            .HasDatabaseName("IX_PerfilesInfo_UserId_Identificacion");

        // Índice único: una sola información extendida por usuario
        builder.HasIndex(p => p.UserId)
            .IsUnique()
            .HasDatabaseName("UX_PerfilesInfo_UserId");

        // ===========================
        // RELACIONES
        // ===========================
        
        // ✅ RELACIÓN: PerfilesInfo → Perfile (N:1) - OPCIONAL
        // Una información de perfil puede estar asociada a un tipo de perfil
        // Nota: PerfilId es NULLABLE porque esta relación es opcional
        builder.HasOne<Perfile>()
            .WithMany()
            .HasForeignKey(p => p.PerfilId)
            .HasConstraintName("FK_perfilesInfo_Perfiles")
            .OnDelete(DeleteBehavior.Restrict) // No borrar Perfile si tiene PerfilesInfos asociados
            .IsRequired(false); // Relación opcional (FK nullable)
    }
}
