using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Exceptions;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Domain.Entities.Authentication;

namespace MiGenteEnLinea.Application.Features.Seguridad.Credenciales.Commands.DeleteUser;

/// <summary>
/// Handler para DeleteUserCommand.
/// Implementa lógica EXACTA del Legacy: borrarUsuario() en LoginService.asmx.cs
/// </summary>
/// <remarks>
/// LÓGICA LEGACY (líneas 131-138 LoginService.asmx.cs):
/// <code>
/// public void borrarUsuario(string userID, int credencialID)
/// {
///     using (var db = new migenteEntities())
///     {
///         var result = db.Credenciales.Where(a => a.userID == userID && a.id==credencialID).FirstOrDefault();
///         db.Credenciales.Remove(result);
///         db.SaveChanges();
///     }
/// }
/// </code>
/// 
/// COMPORTAMIENTO:
/// - Hard delete (no soft delete)
/// - Busca por userID + credencialID (doble clave)
/// - Confía en FK constraints para cascada
/// - No manejo explícito de errores (NullReferenceException si no existe)
/// </remarks>
public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<DeleteUserCommandHandler> _logger;

    public DeleteUserCommandHandler(
        IApplicationDbContext context,
        ILogger<DeleteUserCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Iniciando eliminación de usuario. UserID: {UserID}, CredencialID: {CredencialID}",
            request.UserID, request.CredencialID);

        // LÓGICA LEGACY EXACTA: buscar por userID + credencialID
        var credencial = await _context.Credenciales
            .Where(c => c.UserId == request.UserID && c.Id == request.CredencialID)
            .FirstOrDefaultAsync(cancellationToken);

        if (credencial == null)
        {
            _logger.LogWarning(
                "Credencial no encontrada. UserID: {UserID}, CredencialID: {CredencialID}",
                request.UserID, request.CredencialID);

            throw new NotFoundException(nameof(Credencial), $"UserID: {request.UserID}, CredencialID: {request.CredencialID}");
        }

        // LÓGICA LEGACY EXACTA: db.Credenciales.Remove(result)
        _context.Credenciales.Remove(credencial);

        // LÓGICA LEGACY EXACTA: db.SaveChanges()
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Usuario eliminado exitosamente. UserID: {UserID}, CredencialID: {CredencialID}",
            request.UserID, request.CredencialID);

        return Unit.Value;
    }
}
