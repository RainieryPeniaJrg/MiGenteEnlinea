using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Domain.Entities.Contratistas;

namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuración de Entity Framework para la entidad Contratista
/// Mapea la entidad de dominio "Contratista" a la tabla legacy "Contratistas"
/// </summary>
public sealed class ContratistaConfiguration : IEntityTypeConfiguration<Contratista>
{
    public void Configure(EntityTypeBuilder<Contratista> builder)
    {
        // ===========================
        // MAPEO A TABLA LEGACY
        // ===========================
        builder.ToTable("Contratistas");

        // ===========================
        // PRIMARY KEY
        // ===========================
        builder.HasKey(c => c.Id);

        // ===========================
        // MAPEO DE COLUMNAS LEGACY
        // ===========================
        builder.Property(c => c.Id)
            .HasColumnName("contratistaID")
            .ValueGeneratedOnAdd(); // Identity en SQL Server

        builder.Property(c => c.FechaIngreso)
            .HasColumnName("fechaIngreso")
            .HasColumnType("datetime")
            .IsRequired(false); // Nullable en legacy

        builder.Property(c => c.UserId)
            .HasColumnName("userID")
            .HasMaxLength(250)
            .IsUnicode(false) // VARCHAR en legacy
            .IsRequired(); // FK obligatorio

        builder.Property(c => c.Titulo)
            .HasColumnName("titulo")
            .HasMaxLength(70)
            .IsUnicode(false) // VARCHAR en legacy
            .IsRequired(false); // Nullable

        builder.Property(c => c.Tipo)
            .HasColumnName("tipo")
            .IsRequired(); // NOT NULL en dominio

        builder.Property(c => c.Identificacion)
            .HasColumnName("identificacion")
            .HasMaxLength(20)
            .IsUnicode(false) // VARCHAR en legacy
            .IsRequired(false); // Nullable

        builder.Property(c => c.Nombre)
            .HasColumnName("Nombre") // Nota: Columna con mayúscula en legacy
            .HasMaxLength(20)
            .IsUnicode(false) // VARCHAR en legacy
            .IsRequired(false); // Nullable

        builder.Property(c => c.Apellido)
            .HasColumnName("Apellido") // Nota: Columna con mayúscula en legacy
            .HasMaxLength(50)
            .IsUnicode(false) // VARCHAR en legacy
            .IsRequired(false); // Nullable

        builder.Property(c => c.Sector)
            .HasColumnName("sector")
            .HasMaxLength(40)
            .IsUnicode(false) // VARCHAR en legacy
            .IsRequired(false); // Nullable

        builder.Property(c => c.Experiencia)
            .HasColumnName("experiencia")
            .IsRequired(false); // Nullable

        builder.Property(c => c.Presentacion)
            .HasColumnName("presentacion")
            .HasMaxLength(250)
            .IsUnicode(false) // VARCHAR en legacy
            .IsRequired(false); // Nullable

        builder.Property(c => c.Telefono1)
            .HasColumnName("telefono1")
            .HasMaxLength(16)
            .IsUnicode(false) // VARCHAR en legacy
            .IsRequired(false); // Nullable

        builder.Property(c => c.Whatsapp1)
            .HasColumnName("whatsapp1")
            .IsRequired(); // NOT NULL en dominio

        builder.Property(c => c.Telefono2)
            .HasColumnName("telefono2")
            .HasMaxLength(20)
            .IsUnicode(false) // VARCHAR en legacy
            .IsRequired(false); // Nullable

        builder.Property(c => c.Whatsapp2)
            .HasColumnName("whatsapp2")
            .IsRequired(); // NOT NULL en dominio

        // Email como Value Object - conversión manual
        builder.Property<string>("_email")
            .HasColumnName("email")
            .HasMaxLength(50)
            .IsUnicode(false) // VARCHAR en legacy
            .IsRequired(false); // Nullable

        builder.Ignore(c => c.Email); // La propiedad pública se ignora

        builder.Property(c => c.Activo)
            .HasColumnName("activo")
            .IsRequired(); // NOT NULL en dominio

        builder.Property(c => c.Provincia)
            .HasColumnName("provincia")
            .HasMaxLength(50)
            .IsUnicode(false) // VARCHAR en legacy
            .IsRequired(false); // Nullable

        builder.Property(c => c.NivelNacional)
            .HasColumnName("nivelNacional")
            .IsRequired(); // NOT NULL en dominio

        builder.Property(c => c.ImagenUrl)
            .HasColumnName("imagenURL")
            .HasMaxLength(150)
            .IsUnicode(false) // VARCHAR en legacy
            .IsRequired(false); // Nullable

        // ===========================
        // COLUMNAS DE AUDITORÍA (NUEVAS - Agregadas con Clean Architecture)
        // ===========================
        // Estas columnas NO existen en la tabla legacy actualmente
        // Se agregarán con una migración posterior
        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired(false); // Opcional hasta que ejecutemos la migración

        builder.Property(c => c.CreatedBy)
            .HasColumnName("created_by")
            .HasMaxLength(100)
            .IsRequired(false); // Opcional hasta que ejecutemos la migración

        builder.Property(c => c.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired(false); // Opcional hasta que ejecutemos la migración

        builder.Property(c => c.UpdatedBy)
            .HasColumnName("updated_by")
            .HasMaxLength(100)
            .IsRequired(false); // Opcional hasta que ejecutemos la migración

        // ===========================
        // ÍNDICES
        // ===========================
        // Índice único en UserId: Un usuario solo puede tener un perfil de contratista
        builder.HasIndex(c => c.UserId)
            .IsUnique()
            .HasDatabaseName("IX_Contratistas_UserID");

        // Índice en FechaIngreso para consultas ordenadas
        builder.HasIndex(c => c.FechaIngreso)
            .HasDatabaseName("IX_Contratistas_FechaIngreso");

        // Índice en Activo para filtrar contratistas activos
        builder.HasIndex(c => c.Activo)
            .HasDatabaseName("IX_Contratistas_Activo");

        // Índice en Provincia para búsquedas por ubicación
        builder.HasIndex(c => c.Provincia)
            .HasDatabaseName("IX_Contratistas_Provincia");

        // Índice compuesto en Sector + Provincia para búsquedas específicas
        builder.HasIndex(c => new { c.Sector, c.Provincia })
            .HasDatabaseName("IX_Contratistas_Sector_Provincia");

        // ===========================
        // RELACIONES
        // ===========================
        // Relación con Credencial (si está en el DbContext)
        // Un Contratista pertenece a una Credencial (relación 1:1)
        // Nota: Descomentar cuando Credencial esté en el DbContext
        /*
        builder.HasOne<Credencial>()
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .HasPrincipalKey(cred => cred.UserId)
            .OnDelete(DeleteBehavior.Restrict); // No eliminar en cascada
        */

        // Relación con ContratistasFotos (1:N)
        // Un contratista puede tener múltiples fotos de trabajos
        // Nota: La relación inversa ya está configurada en la entidad scaffolded
        // No necesitamos configurarla aquí si usamos la entidad legacy para fotos

        // Relación con ContratistasServicios (N:M)
        // Un contratista puede ofrecer múltiples servicios
        // Nota: La relación inversa ya está configurada en la entidad scaffolded
        // No necesitamos configurarla aquí si usamos la entidad legacy para servicios

        // ===========================
        // PROPIEDADES IGNORADAS
        // ===========================
        // Los eventos de dominio no se persisten en la base de datos
        builder.Ignore(c => c.Events);
    }
}
