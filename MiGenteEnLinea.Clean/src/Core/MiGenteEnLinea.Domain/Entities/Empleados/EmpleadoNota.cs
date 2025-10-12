using MiGenteEnLinea.Domain.Common;
using MiGenteEnLinea.Domain.Events.Empleados;

namespace MiGenteEnLinea.Domain.Entities.Empleados;

/// <summary>
/// Entidad que representa una nota o comentario sobre un empleado.
/// Permite a los empleadores llevar un registro de observaciones, incidentes o recordatorios.
/// </summary>
public sealed class EmpleadoNota : AuditableEntity
{
    /// <summary>
    /// Identificador único de la nota.
    /// </summary>
    public int NotaId { get; private set; }

    /// <summary>
    /// Identificador del empleador que creó la nota.
    /// </summary>
    public string UserId { get; private set; } = null!;

    /// <summary>
    /// Identificador del empleado al que pertenece la nota.
    /// </summary>
    public int EmpleadoId { get; private set; }

    /// <summary>
    /// Fecha de creación de la nota.
    /// </summary>
    public DateTime Fecha { get; private set; }

    /// <summary>
    /// Contenido de la nota.
    /// </summary>
    public string Nota { get; private set; } = null!;

    /// <summary>
    /// Indica si la nota ha sido eliminada lógicamente.
    /// </summary>
    public bool Eliminada { get; private set; }

    // Constructor privado para EF Core
    private EmpleadoNota()
    {
    }

    /// <summary>
    /// Crea una nueva nota para un empleado.
    /// </summary>
    /// <param name="userId">Identificador del empleador.</param>
    /// <param name="empleadoId">Identificador del empleado.</param>
    /// <param name="nota">Contenido de la nota.</param>
    /// <returns>Nueva instancia de EmpleadoNota.</returns>
    /// <exception cref="ArgumentException">Si los parámetros son inválidos.</exception>
    public static EmpleadoNota Create(string userId, int empleadoId, string nota)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("El ID del usuario es requerido", nameof(userId));

        if (empleadoId <= 0)
            throw new ArgumentException("El ID del empleado debe ser mayor a cero", nameof(empleadoId));

        if (string.IsNullOrWhiteSpace(nota))
            throw new ArgumentException("El contenido de la nota es requerido", nameof(nota));

        if (nota.Length > 250)
            throw new ArgumentException("La nota no puede exceder 250 caracteres", nameof(nota));

        var empleadoNota = new EmpleadoNota
        {
            UserId = userId,
            EmpleadoId = empleadoId,
            Nota = nota.Trim(),
            Fecha = DateTime.UtcNow,
            Eliminada = false
        };

        return empleadoNota;
    }

    /// <summary>
    /// Actualiza el contenido de la nota.
    /// </summary>
    /// <param name="nuevaNota">Nuevo contenido de la nota.</param>
    /// <exception cref="ArgumentException">Si la nota es inválida.</exception>
    /// <exception cref="InvalidOperationException">Si la nota está eliminada.</exception>
    public void ActualizarNota(string nuevaNota)
    {
        if (Eliminada)
            throw new InvalidOperationException("No se puede actualizar una nota eliminada");

        if (string.IsNullOrWhiteSpace(nuevaNota))
            throw new ArgumentException("El contenido de la nota es requerido", nameof(nuevaNota));

        if (nuevaNota.Length > 250)
            throw new ArgumentException("La nota no puede exceder 250 caracteres", nameof(nuevaNota));

        Nota = nuevaNota.Trim();
    }

    /// <summary>
    /// Marca la nota como eliminada (soft delete).
    /// </summary>
    /// <exception cref="InvalidOperationException">Si la nota ya está eliminada.</exception>
    public void Eliminar()
    {
        if (Eliminada)
            throw new InvalidOperationException("La nota ya está eliminada");

        Eliminada = true;
    }

    /// <summary>
    /// Restaura una nota eliminada.
    /// </summary>
    /// <exception cref="InvalidOperationException">Si la nota no está eliminada.</exception>
    public void Restaurar()
    {
        if (!Eliminada)
            throw new InvalidOperationException("La nota no está eliminada");

        Eliminada = false;
    }

    /// <summary>
    /// Verifica si la nota fue creada recientemente (últimas 24 horas).
    /// </summary>
    /// <returns>True si la nota es reciente, False en caso contrario.</returns>
    public bool EsReciente()
    {
        return (DateTime.UtcNow - Fecha).TotalHours <= 24;
    }

    /// <summary>
    /// Obtiene un resumen corto de la nota (primeros 50 caracteres).
    /// </summary>
    /// <returns>Resumen de la nota.</returns>
    public string ObtenerResumen()
    {
        if (Nota.Length <= 50)
            return Nota;

        return Nota.Substring(0, 47) + "...";
    }
}
