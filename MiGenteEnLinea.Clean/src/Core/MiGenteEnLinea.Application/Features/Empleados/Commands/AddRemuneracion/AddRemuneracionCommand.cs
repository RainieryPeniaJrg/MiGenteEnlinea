using MediatR;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.AddRemuneracion;

/// <summary>
/// Command para agregar una remuneración extra (bono, comisión, incentivo) a un empleado.
/// Legacy: EmpleadosService.guardarOtrasRemuneraciones(rem)
/// </summary>
/// <remarks>
/// El empleado puede tener máximo 3 remuneraciones extras simultáneas (slots 1, 2, 3).
/// Cada remuneración tiene una descripción y un monto que se suma al salario base en el recibo.
/// </remarks>
public record AddRemuneracionCommand : IRequest<bool>
{
    /// <summary>
    /// Identificador del empleador (para validar propiedad del empleado).
    /// </summary>
    public string UserId { get; init; } = string.Empty;

    /// <summary>
    /// Identificador del empleado al que se agregará la remuneración.
    /// </summary>
    public int EmpleadoId { get; init; }

    /// <summary>
    /// Número del slot de remuneración (1, 2 o 3).
    /// Indica cuál de las 3 posiciones disponibles usar.
    /// </summary>
    public int Numero { get; init; }

    /// <summary>
    /// Descripción de la remuneración (ej: "Bono por productividad", "Comisión ventas Q1").
    /// </summary>
    public string Descripcion { get; init; } = string.Empty;

    /// <summary>
    /// Monto de la remuneración extra.
    /// Debe ser mayor o igual a cero.
    /// </summary>
    public decimal Monto { get; init; }
}
