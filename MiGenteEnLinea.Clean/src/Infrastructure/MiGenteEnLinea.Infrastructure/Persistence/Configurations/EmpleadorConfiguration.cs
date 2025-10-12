using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Domain.Entities.Empleadores;

namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuración de Entity Framework para la entidad Empleador
/// Mapea la entidad de dominio "Empleador" a la tabla legacy "Ofertantes"
/// </summary>
public sealed class EmpleadorConfiguration : IEntityTypeConfiguration<Empleador>
{
    public void Configure(EntityTypeBuilder<Empleador> builder)
    {
        // ===========================
        // MAPEO A TABLA LEGACY
        // ===========================
        // Nota: La tabla se llama "Ofertantes" (plural) por compatibilidad con el sistema legacy
        // pero en el dominio usamos "Empleador" (singular) por claridad
        builder.ToTable("Ofertantes");

        // ===========================
        // PRIMARY KEY
        // ===========================
        builder.HasKey(e => e.Id);

        // ===========================
        // MAPEO DE COLUMNAS LEGACY
        // ===========================
        builder.Property(e => e.Id)
            .HasColumnName("ofertanteID")
            .ValueGeneratedOnAdd(); // Identity en SQL Server

        builder.Property(e => e.FechaPublicacion)
            .HasColumnName("fechaPublicacion")
            .HasColumnType("datetime")
            .IsRequired(false); // Nullable en legacy

        builder.Property(e => e.UserId)
            .HasColumnName("userID")
            .IsUnicode(false) // VARCHAR en legacy
            .IsRequired(); // FK obligatorio

        builder.Property(e => e.Habilidades)
            .HasColumnName("habilidades")
            .HasMaxLength(200)
            .IsUnicode(false) // VARCHAR en legacy
            .IsRequired(false); // Nullable

        builder.Property(e => e.Experiencia)
            .HasColumnName("experiencia")
            .HasMaxLength(200)
            .IsUnicode(false) // VARCHAR en legacy
            .IsRequired(false); // Nullable

        builder.Property(e => e.Descripcion)
            .HasColumnName("descripcion")
            .HasMaxLength(500)
            .IsUnicode(false) // VARCHAR en legacy
            .IsRequired(false); // Nullable

        builder.Property(e => e.Foto)
            .HasColumnName("foto")
            .HasColumnType("varbinary(max)") // VARBINARY(MAX) en legacy
            .IsRequired(false); // Nullable

        // ===========================
        // COLUMNAS DE AUDITORÍA (NUEVAS - Agregadas con Clean Architecture)
        // ===========================
        // Estas columnas NO existen en la tabla legacy actualmente
        // Se agregarán con una migración posterior
        builder.Property(e => e.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired(false); // Opcional hasta que ejecutemos la migración

        builder.Property(e => e.CreatedBy)
            .HasColumnName("created_by")
            .HasMaxLength(100)
            .IsRequired(false); // Opcional hasta que ejecutemos la migración

        builder.Property(e => e.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired(false); // Opcional hasta que ejecutemos la migración

        builder.Property(e => e.UpdatedBy)
            .HasColumnName("updated_by")
            .HasMaxLength(100)
            .IsRequired(false); // Opcional hasta que ejecutemos la migración

        // ===========================
        // ÍNDICES
        // ===========================
        // Índice único en UserId: Un usuario solo puede tener un perfil de empleador
        builder.HasIndex(e => e.UserId)
            .IsUnique()
            .HasDatabaseName("IX_Ofertantes_UserID");

        // Índice en FechaPublicacion para consultas ordenadas
        builder.HasIndex(e => e.FechaPublicacion)
            .HasDatabaseName("IX_Ofertantes_FechaPublicacion");

        // ===========================
        // RELACIONES
        // ===========================
        // Relación con Credencial (si está en el DbContext)
        // Un Empleador pertenece a una Credencial (relación 1:1)
        // Nota: Descomentar cuando Credencial esté en el DbContext
        /*
        builder.HasOne<Credencial>()
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .HasPrincipalKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict); // No eliminar en cascada
        */

        // ===========================
        // PROPIEDADES IGNORADAS
        // ===========================
        // Los eventos de dominio no se persisten en la base de datos
        builder.Ignore(e => e.Events);

        // ===========================
        // CONFIGURACIONES ADICIONALES
        // ===========================
        // Configurar comportamiento de queries
        // TODO: Descomentar después de ejecutar migración de auditoría
        // builder.HasQueryFilter(e => e.CreatedAt != null); // Filtrar registros sin auditoría (legacy)
    }
}
