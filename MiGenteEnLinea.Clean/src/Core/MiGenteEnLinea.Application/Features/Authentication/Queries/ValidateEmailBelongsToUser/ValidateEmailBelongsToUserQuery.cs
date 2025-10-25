using MediatR;

namespace MiGenteEnLinea.Application.Features.Authentication.Queries.ValidateEmailBelongsToUser;

/// <summary>
/// Query para validar si un correo electrónico pertenece a un usuario específico.
/// Usado en contexto de suscripciones para verificar si el email ya está registrado para ese userID.
/// </summary>
/// <remarks>
/// Mapeo desde Legacy: SuscripcionesService.validarCorreoCuentaActual(correo, userID)
/// Lógica: WHERE Email == correo && userID == userID
/// Caso de uso: Validar si email ya existe en la suscripción del usuario antes de crear credencial adicional.
/// </remarks>
public sealed record ValidateEmailBelongsToUserQuery : IRequest<bool>
{
    /// <summary>
    /// Correo electrónico a validar.
    /// </summary>
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// ID del usuario (userID) propietario de la suscripción.
    /// </summary>
    public string UserID { get; init; } = string.Empty;
}
