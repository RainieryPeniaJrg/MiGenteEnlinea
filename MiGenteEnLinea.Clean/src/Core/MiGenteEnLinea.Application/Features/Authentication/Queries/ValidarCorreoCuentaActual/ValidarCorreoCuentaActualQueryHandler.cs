using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Authentication.Queries.ValidarCorreoCuentaActual;

/// <summary>
/// Handler para ValidarCorreoCuentaActualQuery.
/// Migrado desde: SuscripcionesService.validarCorreoCuentaActual(string correo, string userID) - línea 220
/// </summary>
public class ValidarCorreoCuentaActualQueryHandler : IRequestHandler<ValidarCorreoCuentaActualQuery, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<ValidarCorreoCuentaActualQueryHandler> _logger;

    public ValidarCorreoCuentaActualQueryHandler(
        IApplicationDbContext context,
        ILogger<ValidarCorreoCuentaActualQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> Handle(ValidarCorreoCuentaActualQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Validando si correo {Email} pertenece al usuario {UserId}",
            request.Email,
            request.UserId);

        // PASO 1: Buscar credencial con el email Y userId específico (paridad exacta con Legacy)
        // Legacy: db.Cuentas.Where(x => x.Email == correo && x.userID==userID)
        // Clean: Cuentas → Credenciales
        var credencial = await _context.Credenciales
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Email.Value == request.Email && x.UserId == request.UserId,
                cancellationToken);

        // PASO 2: Retornar true si existe, false si no existe
        var existe = credencial != null;

        _logger.LogInformation(
            "Validación de correo {Email} para usuario {UserId}: {Resultado}",
            request.Email,
            request.UserId,
            existe ? "VÁLIDO (correo pertenece al usuario)" : "INVÁLIDO (correo no pertenece al usuario o no existe)");

        return existe;
    }
}
