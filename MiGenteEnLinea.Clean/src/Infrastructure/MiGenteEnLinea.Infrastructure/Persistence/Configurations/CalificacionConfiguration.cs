using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Domain.Entities.Calificaciones;

namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuración de Entity Framework Core para la entidad Calificacion
/// 
/// MAPEO CON LEGACY:
/// - Tabla destino: "Calificaciones" (nombre original de la base de datos)
/// - Columnas mapeadas a nombres legacy españoles
/// - PK: calificacionID
/// - FK: userID (a Credenciales) - representa al empleador que califica
/// 
/// CONSIDERACIONES:
/// - Las calificaciones son INMUTABLES (no se editan)
/// - El campo "identificacion" almacena la cédula del contratista calificado
/// - El campo "nombre" es desnormalizado para performance en queries
/// - No hay FK explícita a Contratista porque usa cédula (no PK)
/// 
/// ÍNDICES ESTRATÉGICOS:
/// - IX_Calificaciones_EmpleadorUserId: Calificaciones dadas por un empleador
/// - IX_Calificaciones_ContratistaIdentificacion: Calificaciones recibidas por un contratista
/// - IX_Calificaciones_Fecha: Ordenamiento temporal
/// - IX_Calificaciones_Promedio: Filtrado por calidad (computed column)
/// </summary>
public class CalificacionConfiguration : IEntityTypeConfiguration<Calificacion>
{
    public void Configure(EntityTypeBuilder<Calificacion> builder)
    {
        // ========================================
        // CONFIGURACIÓN DE TABLA
        // ========================================
        builder.ToTable("Calificaciones");

        // ========================================
        // CLAVE PRIMARIA
        // ========================================
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("calificacionID")
            .ValueGeneratedOnAdd()
            .IsRequired();

        // ========================================
        // PROPIEDADES BÁSICAS
        // ========================================

        // Fecha - Fecha de la calificación
        builder.Property(c => c.Fecha)
            .HasColumnName("fecha")
            .HasColumnType("datetime")
            .IsRequired();

        // EmpleadorUserId - FK a Credenciales (quien califica)
        builder.Property(c => c.EmpleadorUserId)
            .HasColumnName("userID")
            .HasMaxLength(250) // Coincide con legacy
            .IsRequired();

        // Tipo - Tipo de calificación (legacy: "Contratista")
        builder.Property(c => c.Tipo)
            .HasColumnName("tipo")
            .HasMaxLength(20)
            .IsRequired();

        // ContratistaIdentificacion - Cédula del contratista calificado
        builder.Property(c => c.ContratistaIdentificacion)
            .HasColumnName("identificacion")
            .HasMaxLength(20)
            .IsRequired();

        // ContratistaNombre - Nombre completo (desnormalizado)
        builder.Property(c => c.ContratistaNombre)
            .HasColumnName("nombre")
            .HasMaxLength(100)
            .IsRequired();

        // Puntualidad - Calificación de puntualidad (1-5)
        builder.Property(c => c.Puntualidad)
            .HasColumnName("puntualidad")
            .IsRequired();

        // Cumplimiento - Calificación de cumplimiento (1-5)
        builder.Property(c => c.Cumplimiento)
            .HasColumnName("cumplimiento")
            .IsRequired();

        // Conocimientos - Calificación de conocimientos (1-5)
        builder.Property(c => c.Conocimientos)
            .HasColumnName("conocimientos")
            .IsRequired();

        // Recomendacion - Calificación de recomendación (1-5)
        builder.Property(c => c.Recomendacion)
            .HasColumnName("recomendacion")
            .IsRequired();

        // ========================================
        // RELACIONES (FKs)
        // ========================================

        // FK: Calificacion -> Credencial (EmpleadorUserId)
        // Representa al empleador que dio la calificación
        builder.HasOne<Domain.Entities.Authentication.Credencial>()
            .WithMany()
            .HasForeignKey(c => c.EmpleadorUserId)
            .HasConstraintName("FK_Calificaciones_Credenciales_Empleador")
            .OnDelete(DeleteBehavior.Cascade); // Si se elimina el empleador, se eliminan sus calificaciones

        // NOTA IMPORTANTE SOBRE CONTRATISTA:
        // No configuramos FK explícita a Contratista porque:
        // 1. El campo "identificacion" es la cédula (no el PK de Contratista)
        // 2. Mantiene compatibilidad con legacy donde no existía FK
        // 3. Permite calificar contratistas que ya no existen en el sistema
        // 4. Es un caso de desnormalización intencional para performance
        // Si se requiere integridad referencial estricta, considerar:
        //   - Agregar campo ContratistaId (FK real)
        //   - Crear trigger SQL para validar existencia

        // ========================================
        // ÍNDICES PARA OPTIMIZACIÓN
        // ========================================

        // Índice: Búsqueda por empleador (calificaciones dadas)
        builder.HasIndex(c => c.EmpleadorUserId)
            .HasDatabaseName("IX_Calificaciones_EmpleadorUserId")
            .IsUnique(false);

        // Índice: Búsqueda por contratista (calificaciones recibidas)
        // Este es el query MÁS FRECUENTE: ver calificaciones de un contratista
        builder.HasIndex(c => c.ContratistaIdentificacion)
            .HasDatabaseName("IX_Calificaciones_ContratistaIdentificacion")
            .IsUnique(false);

        // Índice: Ordenamiento por fecha
        builder.HasIndex(c => c.Fecha)
            .HasDatabaseName("IX_Calificaciones_Fecha")
            .IsUnique(false);

        // Índice compuesto: Contratista + Fecha (query común)
        builder.HasIndex(c => new { c.ContratistaIdentificacion, c.Fecha })
            .HasDatabaseName("IX_Calificaciones_Contratista_Fecha")
            .IsUnique(false);

        // Índice compuesto: Empleador + Fecha (historial del empleador)
        builder.HasIndex(c => new { c.EmpleadorUserId, c.Fecha })
            .HasDatabaseName("IX_Calificaciones_Empleador_Fecha")
            .IsUnique(false);

        // Índice: Por tipo (aunque siempre sea "Contratista")
        builder.HasIndex(c => c.Tipo)
            .HasDatabaseName("IX_Calificaciones_Tipo")
            .IsUnique(false);

        // ========================================
        // VALIDACIONES A NIVEL DE BD
        // ========================================

        // Check constraints para validar rango de calificaciones (1-5)
        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_Calificaciones_Puntualidad_Rango",
                "puntualidad >= 1 AND puntualidad <= 5");
            t.HasCheckConstraint("CK_Calificaciones_Cumplimiento_Rango",
                "cumplimiento >= 1 AND cumplimiento <= 5");
            t.HasCheckConstraint("CK_Calificaciones_Conocimientos_Rango",
                "conocimientos >= 1 AND conocimientos <= 5");
            t.HasCheckConstraint("CK_Calificaciones_Recomendacion_Rango",
                "recomendacion >= 1 AND recomendacion <= 5");
        });
    }
}
