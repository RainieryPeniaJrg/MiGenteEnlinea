using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Domain.Interfaces.Repositories;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Empleadores;

namespace MiGenteEnLinea.Application.Features.Empleadores.Commands.DeleteEmpleador;

/// <summary>
/// Handler: Procesa la eliminación del Empleador
/// </summary>
/// <remarks>
/// ⚠️ ADVERTENCIA: Este handler realiza eliminación FÍSICA (hard delete)
/// 
/// TODO: Modificar Empleador entity para heredar de SoftDeletableEntity
/// y cambiar este handler a soft delete (IsDeleted=true)
/// 
/// Por ahora, eliminación física debido a que Empleador no tiene soft delete implementado.
/// </remarks>
public sealed class DeleteEmpleadorCommandHandler : IRequestHandler<DeleteEmpleadorCommand, bool>
{
    private readonly IEmpleadorRepository _empleadorRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteEmpleadorCommandHandler> _logger;

    public DeleteEmpleadorCommandHandler(
        IEmpleadorRepository empleadorRepository,
        IUnitOfWork unitOfWork,
        ILogger<DeleteEmpleadorCommandHandler> logger)
    {
        _empleadorRepository = empleadorRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Maneja la eliminación del empleador
    /// </summary>
    /// <exception cref="InvalidOperationException">Si empleador no existe</exception>
    public async Task<bool> Handle(DeleteEmpleadorCommand request, CancellationToken cancellationToken)
    {
        _logger.LogWarning(
            "⚠️ Eliminación FÍSICA de empleador. UserId: {UserId}. Considerar cambiar a soft delete.",
            request.UserId);

        // ============================================
        // PASO 1: Buscar empleador por userId
        // ============================================
        var empleador = await _empleadorRepository.GetByUserIdAsync(request.UserId, cancellationToken);

        if (empleador == null)
        {
            _logger.LogWarning("Empleador no encontrado para userId: {UserId}", request.UserId);
            throw new InvalidOperationException($"Empleador no encontrado para usuario {request.UserId}");
        }

        // ============================================
        // PASO 2: Eliminar físicamente
        // ============================================
        _empleadorRepository.Remove(empleador);

        // ============================================
        // PASO 3: Guardar cambios
        // ============================================
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Empleador eliminado FÍSICAMENTE. EmpleadorId: {EmpleadorId}, UserId: {UserId}",
            empleador.Id, request.UserId);

        return true;
    }
}
