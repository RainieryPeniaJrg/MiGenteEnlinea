using MediatR;
using MiGenteEnLinea.Domain.Entities.Suscripciones;

namespace MiGenteEnLinea.Application.Features.Suscripciones.Queries.GetSuscripcionActiva;

/// <summary>
/// Query para obtener la suscripción activa de un usuario.
/// </summary>
/// <remarks>
/// Legacy: Lógica repetida en múltiples *.aspx.cs (Comunity1.Master.cs, ContratistaM.Master.cs, etc.)
/// Uso: Validar acceso, mostrar información de plan, verificar vencimiento.
/// </remarks>
public record GetSuscripcionActivaQuery : IRequest<Suscripcion?>
{
    /// <summary>
    /// ID del usuario para buscar su suscripción activa.
    /// </summary>
    public string UserId { get; init; } = string.Empty;
}
