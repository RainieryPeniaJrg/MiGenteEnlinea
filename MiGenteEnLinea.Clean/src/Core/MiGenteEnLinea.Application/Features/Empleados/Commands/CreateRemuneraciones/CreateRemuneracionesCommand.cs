using MediatR;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.CreateRemuneraciones;

/// <summary>
/// Command para crear múltiples remuneraciones en batch.
/// Migrado de: EmpleadosService.guardarOtrasRemuneraciones(List<Remuneraciones> rem) - Line 649
/// </summary>
public record CreateRemuneracionesCommand(
    string UserId,
    int EmpleadoId,
    List<RemuneracionItemDto> Remuneraciones
) : IRequest<bool>;

public class RemuneracionItemDto
{
    public string Descripcion { get; set; } = string.Empty;
    public decimal Monto { get; set; }
}
