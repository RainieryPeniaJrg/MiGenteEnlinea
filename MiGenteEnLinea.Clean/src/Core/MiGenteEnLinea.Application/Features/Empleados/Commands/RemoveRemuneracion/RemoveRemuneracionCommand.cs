using MediatR;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.RemoveRemuneracion;

/// <summary>
/// Command para eliminar una remuneración extra de un empleado.
/// Legacy: EmpleadosService.quitarRemuneracion(userID, id)
/// </summary>
/// <remarks>
/// Elimina el contenido de uno de los 3 slots de remuneraciones extras.
/// El slot quedará disponible para agregar una nueva remuneración posteriormente.
/// </remarks>
public record RemoveRemuneracionCommand : IRequest<bool>
{
    /// <summary>
    /// Identificador del empleador (para validar propiedad del empleado).
    /// </summary>
    public string UserId { get; init; } = string.Empty;

    /// <summary>
    /// Identificador del empleado del cual se eliminará la remuneración.
    /// </summary>
    public int EmpleadoId { get; init; }

    /// <summary>
    /// Número del slot de remuneración a eliminar (1, 2 o 3).
    /// </summary>
    public int Numero { get; init; }
}
