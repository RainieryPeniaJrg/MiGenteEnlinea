using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Domain.Interfaces.Repositories;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.ChangePasswordById;

/// <summary>
/// Handler para ChangePasswordByIdCommand
/// Réplica EXACTA de SuscripcionesService.actualizarPassByID() del Legacy
/// GAP-014: Cambia password usando credential ID en lugar de userID
/// </summary>
public sealed class ChangePasswordByIdCommandHandler : IRequestHandler<ChangePasswordByIdCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<ChangePasswordByIdCommandHandler> _logger;

    public ChangePasswordByIdCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        ILogger<ChangePasswordByIdCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    /// <summary>
    /// Cambia la contraseña de una credencial usando su ID
    /// 
    /// Legacy behavior (SuscripcionesService.cs líneas 184-203):
    /// - Query: db.Credenciales.Where(x => x.id == c.id).FirstOrDefault()
    /// - Si existe: result.password = c.password (⚠️ ya viene encriptado desde cliente)
    /// - db.SaveChanges()
    /// - Retorna true
    /// 
    /// ⚠️ DIFERENCIA CON GAP-012 (actualizarCredenciales):
    /// - GAP-012: Query por userID + email, actualiza email + activo + password
    /// - GAP-014: Query solo por ID, actualiza SOLO password
    /// 
    /// Clean behavior:
    /// - Query por ID de credencial
    /// - Hashea password con BCrypt (no confía en cliente)
    /// - Actualiza solo password
    /// </summary>
    public async Task<bool> Handle(ChangePasswordByIdCommand request, CancellationToken cancellationToken)
    {
        // ================================================================================
        // PASO 1: OBTENER CREDENCIAL POR ID
        // ================================================================================
        // Legacy línea 189: db.Credenciales.Where(x => x.id == c.id).FirstOrDefault()
        var credencial = await _unitOfWork.Credenciales.GetByIdAsync(request.CredencialId, cancellationToken);

        if (credencial == null)
        {
            _logger.LogWarning(
                "No se encontró credencial para actualizar password. CredencialId: {CredencialId}",
                request.CredencialId);
            return false;
        }

        // ================================================================================
        // PASO 2: HASHEAR NUEVA CONTRASEÑA
        // ================================================================================
        // Legacy línea 191: result.password = c.password (⚠️ ya viene encriptado)
        // Clean: Hasheamos en servidor (más seguro)
        var passwordHasheado = _passwordHasher.HashPassword(request.NewPassword);

        // ================================================================================
        // PASO 3: ACTUALIZAR PASSWORD
        // ================================================================================
        credencial.ActualizarPasswordHash(passwordHasheado);

        // ================================================================================
        // PASO 4: GUARDAR CAMBIOS
        // ================================================================================
        // Legacy línea 193: db.SaveChanges()
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Contraseña actualizada exitosamente por ID. CredencialId: {CredencialId}, UserId: {UserId}",
            request.CredencialId,
            credencial.UserId);

        return true;
    }
}
