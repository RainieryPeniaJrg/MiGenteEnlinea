using MediatR;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.DeleteRemuneracion;

/// <summary>
/// Handler para eliminar remuneración
/// Migrado desde: EmpleadosService.quitarRemuneracion(string userID, int id)
/// 
/// Legacy: 
/// var toDelete = db.Remuneraciones.Where(x => x.userID == userID && x.id == id).FirstOrDefault();
/// if (toDelete!=null) {
///     db.Remuneraciones.Remove(toDelete);
///     db.SaveChanges();
/// }
/// </summary>
public class DeleteRemuneracionCommandHandler : IRequestHandler<DeleteRemuneracionCommand, Unit>
{
    private readonly ILegacyDataService _legacyDataService;

    public DeleteRemuneracionCommandHandler(ILegacyDataService legacyDataService)
    {
        _legacyDataService = legacyDataService;
    }

    public async Task<Unit> Handle(DeleteRemuneracionCommand request, CancellationToken cancellationToken)
    {
        // Buscar y eliminar remuneración (mismo where del Legacy)
        await _legacyDataService.DeleteRemuneracionAsync(
            request.UserId,
            request.RemuneracionId,
            cancellationToken);

        // Legacy no lanza error si no encuentra (sólo valida != null)
        // Mantenemos comportamiento idéntico

        return Unit.Value;
    }
}
