using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Domain.Interfaces.Repositories;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.UpdateProfile;

/// <summary>
/// Handler para UpdateProfileCommand
/// Réplica EXACTA de LoginService.actualizarPerfil() del Legacy
/// </summary>
public sealed class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateProfileCommandHandler> _logger;

    public UpdateProfileCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<UpdateProfileCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        // ================================================================================
        // PASO 1: BUSCAR PERFIL POR USERID
        // ================================================================================
        // Legacy: actualizarPerfil(info, cuenta) - actualiza dos tablas separadas
        // Clean: Perfil es una sola entidad que contiene toda la info
        var perfil = await _unitOfWork.Perfiles
            .GetByUserIdAsync(request.UserId, cancellationToken);

        if (perfil == null)
        {
            _logger.LogWarning("Intento de actualizar perfil inexistente. UserId: {UserId}", request.UserId);
            return false;
        }

        // ================================================================================
        // PASO 2: ACTUALIZAR CAMPOS DEL PERFIL
        // ================================================================================
        // Legacy: db.Entry(cuenta).State = EntityState.Modified
        perfil.ActualizarInformacionBasica(
            nombre: request.Nombre,
            apellido: request.Apellido,
            email: request.Email,
            telefono1: request.Telefono1,
            telefono2: request.Telefono2,
            usuario: request.Usuario
        );

        // ================================================================================
        // PASO 3: MARCAR COMO MODIFICADO Y GUARDAR CAMBIOS
        // ================================================================================
        _unitOfWork.Perfiles.Update(perfil);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Perfil actualizado exitosamente. UserId: {UserId}, Email: {Email}",
            request.UserId, request.Email);

        return true;
    }
}
