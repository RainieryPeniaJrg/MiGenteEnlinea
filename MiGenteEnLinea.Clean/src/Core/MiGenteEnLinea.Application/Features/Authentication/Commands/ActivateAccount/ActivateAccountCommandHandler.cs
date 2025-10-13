using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.ActivateAccount;

/// <summary>
/// Handler para ActivateAccountCommand
/// Réplica EXACTA de Activar.aspx.cs + SuscripcionesService.guardarCredenciales()
/// </summary>
public sealed class ActivateAccountCommandHandler : IRequestHandler<ActivateAccountCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<ActivateAccountCommandHandler> _logger;

    public ActivateAccountCommandHandler(
        IApplicationDbContext context,
        ILogger<ActivateAccountCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> Handle(ActivateAccountCommand request, CancellationToken cancellationToken)
    {
        // ================================================================================
        // PASO 1: BUSCAR CREDENCIAL POR USERID + EMAIL
        // ================================================================================
        // Legacy: guardarCredenciales(cr) donde cr.userID == userId && cr.email == email
        var emailLower = request.Email.ToLowerInvariant();
        var credencial = await _context.Credenciales
            .Where(c => c.UserId == request.UserId && 
                        c.Email.Value.ToLowerInvariant() == emailLower)
            .FirstOrDefaultAsync(cancellationToken);

        if (credencial == null)
        {
            _logger.LogWarning(
                "Intento de activación fallido: credencial no encontrada. UserId: {UserId}, Email: {Email}",
                request.UserId, request.Email);
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

        // ================================================================================
        // PASO 4: GUARDAR CAMBIOS
        // ================================================================================
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Cuenta activada exitosamente. UserId: {UserId}, Email: {Email}",
            request.UserId, request.Email);

        return true;
    }
}
