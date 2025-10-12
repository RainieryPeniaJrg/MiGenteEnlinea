using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Domain.Entities.Contrataciones;

namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuración de Entity Framework Core para la entidad DetalleContratacion.
/// Mapea la entidad de dominio a la tabla legacy "Detalle_Contrataciones".
/// </summary>
public sealed class DetalleContratacionConfiguration : IEntityTypeConfiguration<DetalleContratacion>
{
    public void Configure(EntityTypeBuilder<DetalleContratacion> builder)
    {
        // Nombre de tabla legacy
        builder.ToTable("Detalle_Contrataciones");

        // Clave primaria
        builder.HasKey(dc => dc.DetalleId);
        builder.Property(dc => dc.DetalleId)
            .HasColumnName("detalleID")
            .ValueGeneratedOnAdd();

        // Propiedades
        builder.Property(dc => dc.ContratacionId)
            .HasColumnName("contratacionID");

        builder.Property(dc => dc.DescripcionCorta)
            .IsRequired()
            .HasMaxLength(60)
            .HasColumnName("descripcionCorta")
            .IsUnicode(false);

        builder.Property(dc => dc.DescripcionAmpliada)
            .HasMaxLength(250)
            .HasColumnName("descripcionAmpliada")
            .IsUnicode(false);

        builder.Property(dc => dc.FechaInicio)
            .IsRequired()
            .HasColumnName("fechaInicio")
            .HasColumnType("date");

        builder.Property(dc => dc.FechaFinal)
            .IsRequired()
            .HasColumnName("fechaFinal")
            .HasColumnType("date");

        builder.Property(dc => dc.MontoAcordado)
            .IsRequired()
            .HasColumnName("montoAcordado")
            .HasColumnType("decimal(10, 2)");

        builder.Property(dc => dc.EsquemaPagos)
            .HasMaxLength(50)
            .HasColumnName("esquemaPagos")
            .IsUnicode(false);

        builder.Property(dc => dc.Estatus)
            .IsRequired()
            .HasColumnName("estatus")
            .HasDefaultValue(1); // Pendiente por defecto

        builder.Property(dc => dc.Calificado)
            .IsRequired()
            .HasColumnName("calificado")
            .HasDefaultValue(false);

        builder.Property(dc => dc.CalificacionId)
            .HasColumnName("calificacionID");

        builder.Property(dc => dc.Notas)
            .HasMaxLength(500)
            .HasColumnName("notas")
            .IsUnicode(false);

        builder.Property(dc => dc.MotivoCancelacion)
            .HasMaxLength(500)
            .HasColumnName("motivo_cancelacion")
            .IsUnicode(false);

        builder.Property(dc => dc.FechaInicioReal)
            .HasColumnName("fecha_inicio_real")
            .HasColumnType("datetime");

        builder.Property(dc => dc.FechaFinalizacionReal)
            .HasColumnName("fecha_finalizacion_real")
            .HasColumnType("datetime");

        builder.Property(dc => dc.PorcentajeAvance)
            .IsRequired()
            .HasColumnName("porcentaje_avance")
            .HasDefaultValue(0);

        // Campos de auditoría (heredados de AggregateRoot → AuditableEntity)
        builder.Property(dc => dc.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired(false);

        builder.Property(dc => dc.CreatedBy)
            .HasColumnName("created_by")
            .HasMaxLength(250)
            .IsRequired(false);

        builder.Property(dc => dc.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired(false);

        builder.Property(dc => dc.UpdatedBy)
            .HasColumnName("updated_by")
            .HasMaxLength(250)
            .IsRequired(false);

        // Ignorar la colección de eventos de dominio
        builder.Ignore(dc => dc.Events);

        // Relaciones
        // Nota: La relación con EmpleadosTemporale (contratacionID) se maneja desde esa entidad
        // Relación con Calificacion se maneja desde Calificacion

        // Índices para optimizar consultas
        builder.HasIndex(dc => dc.ContratacionId)
            .HasDatabaseName("IX_DetalleContrataciones_ContratacionId");

        builder.HasIndex(dc => dc.Estatus)
            .HasDatabaseName("IX_DetalleContrataciones_Estatus");

        builder.HasIndex(dc => dc.Calificado)
            .HasDatabaseName("IX_DetalleContrataciones_Calificado");

        builder.HasIndex(dc => dc.CalificacionId)
            .HasDatabaseName("IX_DetalleContrataciones_CalificacionId");

        builder.HasIndex(dc => new { dc.FechaInicio, dc.FechaFinal })
            .HasDatabaseName("IX_DetalleContrataciones_Fechas");

        builder.HasIndex(dc => new { dc.Estatus, dc.FechaInicio })
            .HasDatabaseName("IX_DetalleContrataciones_Estatus_FechaInicio");

        builder.HasIndex(dc => new { dc.Estatus, dc.Calificado })
            .HasDatabaseName("IX_DetalleContrataciones_Estatus_Calificado");
    }
}
