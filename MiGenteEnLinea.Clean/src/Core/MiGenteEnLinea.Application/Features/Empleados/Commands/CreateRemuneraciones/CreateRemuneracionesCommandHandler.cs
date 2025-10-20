using MediatR;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.CreateRemuneraciones;

/// <summary>
/// Handler para CreateRemuneracionesCommand.
/// Migrado de: EmpleadosService.guardarOtrasRemuneraciones(List<Remuneraciones> rem) - Line 649
/// </summary>
public class CreateRemuneracionesCommandHandler : IRequestHandler<CreateRemuneracionesCommand, bool>
{
    private readonly ILegacyDataService _legacyDataService;

    public CreateRemuneracionesCommandHandler(ILegacyDataService legacyDataService)
    {
        _legacyDataService = legacyDataService;
    }

    public async Task<bool> Handle(CreateRemuneracionesCommand request, CancellationToken cancellationToken)
    {
        // Legacy: db.Remuneraciones.AddRange(rem); db.SaveChanges(); return true;
        await _legacyDataService.CreateRemuneracionesAsync(
            request.UserId,
            request.EmpleadoId,
            request.Remuneraciones,
            cancellationToken);

        return true;
    }
}
