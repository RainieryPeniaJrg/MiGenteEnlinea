using MediatR;
using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Authentication.DTOs;
using MiGenteEnLinea.Domain.Interfaces.Repositories;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Authentication;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.ChangePassword;

/// <summary>
/// Handler para cambiar contraseña
/// LOTE 1: Refactorizado para usar ICredencialRepository
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
    private readonly ICredencialRepository _credencialRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public ChangePasswordCommandHandler(
        ICredencialRepository credencialRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher)
    {
        _credencialRepository = credencialRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<ChangePasswordResult> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        // PASO 1: Buscar credencial (LOTE 1: Usando repositorio)
        // Legacy: db.Credenciales.Where(x => x.email == c.email && x.userID == c.userID).FirstOrDefault()
        var credencial = await _credencialRepository.GetByUserIdAsync(request.UserId, cancellationToken);

        if (credencial == null)
        {
            return new ChangePasswordResult
            {
                Success = false,
                Message = "Credencial no encontrada"
            };
        }

        // Validación adicional: verificar email (seguridad adicional)
        if (credencial.Email.Value != request.Email)
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

        // Repository maneja el Update automáticamente por tracking de EF Core
        _credencialRepository.Update(credencial);

        // PASO 3: Guardar cambios (LOTE 1: Usando UnitOfWork)
        // Legacy: db.SaveChanges()
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ChangePasswordResult
        {
            Success = true,
            Message = "Contraseña actualizada exitosamente"
        };
    }
}
