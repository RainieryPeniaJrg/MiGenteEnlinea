using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Domain.Interfaces.Repositories;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.UpdateCredencial;

/// <summary>
/// Handler para UpdateCredencialCommand
/// Réplica EXACTA de SuscripcionesService.actualizarCredenciales() del Legacy
/// GAP-012: Actualiza password, email y estado activo en una credencial
/// </summary>
public sealed class UpdateCredencialCommandHandler : IRequestHandler<UpdateCredencialCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<UpdateCredencialCommandHandler> _logger;

    public UpdateCredencialCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        ILogger<UpdateCredencialCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    /// <summary>
    /// Actualiza credencial completa (password + email + activo)
    /// 
    /// Legacy behavior (SuscripcionesService.cs líneas 157-177):
    /// - Query: db.Credenciales.Where(x => x.email == c.email AND x.userID == c.userID).FirstOrDefault()
    /// - Si existe: result.password = c.password; result.activo = c.activo; result.email = c.email;
    /// - db.SaveChanges()
    /// - Retorna true
    /// 
    /// ⚠️ PROBLEMA LEGACY:
    /// - El WHERE usa email + userID, pero el password ya viene ENCRIPTADO desde el cliente
    /// - En MiPerfilEmpleador.aspx.cs línea 275: cr.password = crypt.Encrypt(txtPassword.Text);
    /// - Legacy NO valida si el email ya existe en otra credencial (puede causar duplicados)
    /// 
    /// Clean behavior:
    /// - Query por userID solamente (más seguro)
    /// - Hashea password con BCrypt (si se provee)
    /// - Valida que nuevo email no exista en otra credencial
    /// - Actualiza email, password (hasheado) y activo
    /// </summary>
    public async Task<bool> Handle(UpdateCredencialCommand request, CancellationToken cancellationToken)
    {
        // ================================================================================
        // PASO 1: OBTENER CREDENCIAL ACTUAL POR USERID
        // ================================================================================
        // Legacy línea 163: db.Credenciales.Where(x => x.email == c.email && x.userID == c.userID).FirstOrDefault()
        // Clean: Solo por userId (más seguro)
        var credencial = await _unitOfWork.Credenciales
            .GetByUserIdAsync(request.UserId, cancellationToken);

        if (credencial == null)
        {
            _logger.LogWarning(
                "No se encontró credencial para actualizar. UserId: {UserId}",
                request.UserId);
            return false;
        }

        // ================================================================================
        // PASO 2: VALIDAR QUE NUEVO EMAIL NO EXISTA EN OTRA CREDENCIAL
        // ================================================================================
        // ⚠️ Legacy NO hace esta validación, pero Clean sí debe hacerla para evitar duplicados
        if (credencial.Email.Value != request.Email)
        {
            var emailExiste = await _unitOfWork.Credenciales
                .ExistsByEmailAsync(request.Email, cancellationToken);

            if (emailExiste)
            {
                _logger.LogWarning(
                    "Email ya existe en otra credencial. Email: {Email}",
                    request.Email);
                return false;
            }
        }

        // ================================================================================
        // PASO 3: ACTUALIZAR CREDENCIAL
        // ================================================================================
        // Legacy líneas 166-168:
        // result.password = c.password;  // ⚠️ Ya viene encriptado desde cliente
        // result.activo = c.activo;
        // result.email = c.email;

        // Actualizar email
        var nuevoEmail = Domain.ValueObjects.Email.Create(request.Email);
        if (nuevoEmail == null)
        {
            _logger.LogWarning("Email inválido: {Email}", request.Email);
            return false;
        }
        credencial.ActualizarEmail(nuevoEmail);

        // Actualizar password (solo si se provee)
        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            var passwordHasheado = _passwordHasher.HashPassword(request.Password);
            credencial.ActualizarPasswordHash(passwordHasheado);
        }

        // Actualizar estado activo
        if (request.Activo && !credencial.Activo)
        {
            credencial.Activar();
        }
        else if (!request.Activo && credencial.Activo)
        {
            credencial.Desactivar();
        }

        // ================================================================================
        // PASO 4: GUARDAR CAMBIOS
        // ================================================================================
        // No necesitamos llamar UpdateAsync, el DbContext detecta los cambios automáticamente
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Credencial actualizada exitosamente. UserId: {UserId}, Email: {Email}, Activo: {Activo}",
            request.UserId,
            request.Email,
            request.Activo);

        return true;
    }
}
