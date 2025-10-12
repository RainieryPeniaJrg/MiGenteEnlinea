namespace MiGenteEnLinea.Domain.ReadModels;

/// <summary>
/// Vista de solo lectura optimizada para listado y consulta de empleados
/// </summary>
/// <remarks>
/// Esta vista combina información completa de empleados incluyendo datos personales,
/// laborales, de contacto y remuneraciones extras. Usada para reportes y consultas
/// sin necesidad de múltiples JOINs en tiempo real.
/// </remarks>
public sealed class VistaEmpleado
{
    /// <summary>
    /// ID del empleado
    /// </summary>
    public int EmpleadoId { get; init; }

    /// <summary>
    /// ID del usuario empleador
    /// </summary>
    public string? UserId { get; init; }

    /// <summary>
    /// Fecha de registro del empleado
    /// </summary>
    public DateTime? FechaRegistro { get; init; }

    /// <summary>
    /// Fecha de inicio laboral
    /// </summary>
    public DateOnly? FechaInicio { get; init; }

    /// <summary>
    /// Identificación (cédula) del empleado
    /// </summary>
    public string? Identificacion { get; init; }

    /// <summary>
    /// Nombre completo del empleado
    /// </summary>
    public string? Nombre { get; init; }

    /// <summary>
    /// Fecha de nacimiento
    /// </summary>
    public DateOnly? Nacimiento { get; init; }

    /// <summary>
    /// Dirección del empleado
    /// </summary>
    public string? Direccion { get; init; }

    /// <summary>
    /// Teléfono principal
    /// </summary>
    public string? Telefono1 { get; init; }

    /// <summary>
    /// Teléfono secundario
    /// </summary>
    public string? Telefono2 { get; init; }

    /// <summary>
    /// Salario base del empleado
    /// </summary>
    public decimal? Salario { get; init; }

    /// <summary>
    /// Periodo de pago (1=Semanal, 2=Quincenal, 3=Mensual)
    /// </summary>
    public int? PeriodoPago { get; init; }

    /// <summary>
    /// Indica si tiene contrato formal
    /// </summary>
    public bool? Contrato { get; init; }

    /// <summary>
    /// Indica si el empleado está activo
    /// </summary>
    public bool? Activo { get; init; }

    /// <summary>
    /// Alias o apodo del empleado
    /// </summary>
    public string? Alias { get; init; }

    /// <summary>
    /// Estado civil (1=Soltero, 2=Casado, 3=Divorciado, 4=Viudo)
    /// </summary>
    public int? EstadoCivil { get; init; }

    /// <summary>
    /// Provincia donde reside
    /// </summary>
    public string? Provincia { get; init; }

    /// <summary>
    /// Municipio donde reside
    /// </summary>
    public string? Municipio { get; init; }

    /// <summary>
    /// Posición o cargo del empleado
    /// </summary>
    public string? Posicion { get; init; }

    /// <summary>
    /// Nombre del contacto de emergencia
    /// </summary>
    public string? ContactoEmergencia { get; init; }

    /// <summary>
    /// Teléfono del contacto de emergencia
    /// </summary>
    public string? TelefonoEmergencia { get; init; }

    /// <summary>
    /// Descripción de remuneración extra 1 (bonos, comisiones, etc.)
    /// </summary>
    public string? RemuneracionExtra1 { get; init; }

    /// <summary>
    /// Monto de remuneración extra 1
    /// </summary>
    public decimal? MontoExtra1 { get; init; }

    /// <summary>
    /// Descripción de remuneración extra 2
    /// </summary>
    public string? RemuneracionExtra2 { get; init; }

    /// <summary>
    /// Monto de remuneración extra 2
    /// </summary>
    public decimal? MontoExtra2 { get; init; }

    /// <summary>
    /// Descripción de remuneración extra 3
    /// </summary>
    public string? RemuneracionExtra3 { get; init; }

    /// <summary>
    /// Monto de remuneración extra 3
    /// </summary>
    public decimal? MontoExtra3 { get; init; }

    /// <summary>
    /// Indica si está inscrito en TSS (Tesorería de la Seguridad Social)
    /// </summary>
    public bool? Tss { get; init; }
}
