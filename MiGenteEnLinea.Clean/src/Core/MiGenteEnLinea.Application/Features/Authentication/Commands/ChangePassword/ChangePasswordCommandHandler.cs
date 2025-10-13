using MediatR;
using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Authentication.DTOs;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.ChangePassword;

/// <summary>
/// Handler para cambiar contraseña
/// </summary>
/// <remarks>
/// LÓGICA COPIADA DE: SuscripcionesService.cs -> actualizarPass(Credenciales c)
/// Legacy:
/// 1. Buscar credencial por email y userID
/// 2. Actualizar password
/// 3. SaveChanges
/// </remarks>
public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ChangePasswordResult>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public ChangePasswordCommandHandler(
        IApplicationDbContext context,
        IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<ChangePasswordResult> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        // PASO 1: Buscar credencial (IGUAL AL LEGACY)
        // Legacy: db.Credenciales.Where(x => x.email == c.email && x.userID == c.userID).FirstOrDefault()
        var credencial = await _context.Credenciales
            .Where(x => x.Email == request.Email && x.UserId == request.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (credencial == null)
        {
            return new ChangePasswordResult
            {
                Success = false,
                Message = "Credencial no encontrada"
            };
        }

        // PASO 2: Actualizar password (LEGACY usaba Crypt, CLEAN usa BCrypt)
        // Legacy: result.password = c.password;
        var newPasswordHash = _passwordHasher.HashPassword(request.NewPassword);
        
        // Usar método de dominio (DDD) en lugar de setear propiedades directamente
        credencial.ActualizarPasswordHash(newPasswordHash);

        // PASO 3: Guardar cambios (IGUAL AL LEGACY)
        // Legacy: db.SaveChanges()
        await _context.SaveChangesAsync(cancellationToken);

        return new ChangePasswordResult
        {
            Success = true,
            Message = "Contraseña actualizada exitosamente"
        };
    }
}
