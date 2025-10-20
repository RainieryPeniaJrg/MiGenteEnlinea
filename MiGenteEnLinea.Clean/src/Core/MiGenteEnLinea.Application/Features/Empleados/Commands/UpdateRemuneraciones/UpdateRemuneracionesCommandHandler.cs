using MediatR;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.UpdateRemuneraciones;

/// <summary>
/// Handler para UpdateRemuneracionesCommand.
/// Migrado de: EmpleadosService.actualizarRemuneraciones(List<Remuneraciones> rem, int empleadoID) - Line 659
/// Legacy: Elimina remuneraciones existentes y luego inserta las nuevas
/// </summary>
public class UpdateRemuneracionesCommandHandler : IRequestHandler<UpdateRemuneracionesCommand, bool>
{
    private readonly ILegacyDataService _legacyDataService;

    public UpdateRemuneracionesCommandHandler(ILegacyDataService legacyDataService)
    {
        _legacyDataService = legacyDataService;
    }

    public async Task<bool> Handle(UpdateRemuneracionesCommand request, CancellationToken cancellationToken)
    {
        // Legacy: DELETE existing WHERE empleadoID, then AddRange new, SaveChanges
        await _legacyDataService.UpdateRemuneracionesAsync(
            request.UserId,
            request.EmpleadoId,
            request.Remuneraciones,
            cancellationToken);

        return true;
    }
}
