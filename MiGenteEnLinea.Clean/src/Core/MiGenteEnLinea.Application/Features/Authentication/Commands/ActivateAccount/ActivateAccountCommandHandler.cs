using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Domain.Interfaces.Repositories;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Authentication;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.ActivateAccount;

/// <summary>
/// Handler para ActivateAccountCommand
/// LOTE 1: Refactorizado para usar ICredencialRepository
/// Réplica EXACTA de Activar.aspx.cs + SuscripcionesService.guardarCredenciales()
/// </summary>
public sealed class ActivateAccountCommandHandler : IRequestHandler<ActivateAccountCommand, bool>
{
    private readonly ICredencialRepository _credencialRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ActivateAccountCommandHandler> _logger;

    public ActivateAccountCommandHandler(
        ICredencialRepository credencialRepository,
        IUnitOfWork unitOfWork,
        ILogger<ActivateAccountCommandHandler> logger)
    {
        _credencialRepository = credencialRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(ActivateAccountCommand request, CancellationToken cancellationToken)
    {
        // ================================================================================
        // PASO 1: BUSCAR CREDENCIAL POR USERID (LOTE 1: Usando repositorio)
        // ================================================================================
        // Legacy: guardarCredenciales(cr) donde cr.userID == userId && cr.email == email
        var credencial = await _credencialRepository.GetByUserIdAsync(request.UserId, cancellationToken);

        if (credencial == null)
        {
            _logger.LogWarning(
                "Intento de activación fallido: credencial no encontrada. UserId: {UserId}, Email: {Email}",
                request.UserId, request.Email);
            return false;
        }

        // Validación adicional de email (case-insensitive)
        if (!credencial.Email.Value.Equals(request.Email, StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogWarning(
                "Intento de activación con email incorrecto. UserId: {UserId}, EmailEsperado: {EmailEsperado}, EmailRecibido: {EmailRecibido}",
                request.UserId, credencial.Email.Value, request.Email);
            return false;
        }

        // ================================================================================
        // PASO 2: VERIFICAR SI YA ESTÁ ACTIVO
        // ================================================================================
        if (credencial.Activo)
        {
            _logger.LogInformation(
                "Intento de activar cuenta ya activa. UserId: {UserId}, Email: {Email}",
                request.UserId, request.Email);
            return true; // Ya está activo, retornar success
        }

        // ================================================================================
        // PASO 3: ACTIVAR CREDENCIAL
        // ================================================================================
        // Legacy: result.activo = true (en Activar.aspx.cs)
        credencial.Activar();

        // Repository maneja Update automáticamente por EF Core tracking
        _credencialRepository.Update(credencial);

        // ================================================================================
        // PASO 4: GUARDAR CAMBIOS (LOTE 1: Usando UnitOfWork)
        // ================================================================================
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Cuenta activada exitosamente. UserId: {UserId}, Email: {Email}",
            request.UserId, request.Email);

        return true;
    }
}
