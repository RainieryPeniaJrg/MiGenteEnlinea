using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Domain.Interfaces.Repositories;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Empleadores;

namespace MiGenteEnLinea.Application.Features.Empleadores.Commands.UpdateEmpleadorFoto;

/// <summary>
/// Handler: Procesa la actualización de la foto del Empleador
/// </summary>
public sealed class UpdateEmpleadorFotoCommandHandler : IRequestHandler<UpdateEmpleadorFotoCommand, bool>
{
    private readonly IEmpleadorRepository _empleadorRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateEmpleadorFotoCommandHandler> _logger;

    public UpdateEmpleadorFotoCommandHandler(
        IEmpleadorRepository empleadorRepository,
        IUnitOfWork unitOfWork,
        ILogger<UpdateEmpleadorFotoCommandHandler> logger)
    {
        _empleadorRepository = empleadorRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Maneja la actualización de la foto del empleador
    /// </summary>
    /// <exception cref="InvalidOperationException">Si empleador no existe</exception>
    /// <exception cref="ArgumentException">Si foto excede tamaño máximo</exception>
    public async Task<bool> Handle(UpdateEmpleadorFotoCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Actualizando foto de empleador. UserId: {UserId}, TamañoFoto: {TamañoBytes} bytes",
            request.UserId, request.Foto.Length);

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
        // PASO 2: Actualizar foto con método de dominio
        // ============================================
        // El método ActualizarFoto() de la entidad Empleador maneja:
        // - Validación de tamaño máximo (5MB)
        // - Validación de foto no vacía
        // - Levanta eventos de dominio (FotoActualizadaEvent)
        try
        {
            empleador.ActualizarFoto(request.Foto);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Error al actualizar foto: {Mensaje}", ex.Message);
            throw; // Re-throw para que API retorne 400 Bad Request
        }

        // ============================================
        // PASO 3: Guardar cambios
        // ============================================
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Foto de empleador actualizada exitosamente. EmpleadorId: {EmpleadorId}, UserId: {UserId}",
            empleador.Id, request.UserId);

        return true;
    }
}
