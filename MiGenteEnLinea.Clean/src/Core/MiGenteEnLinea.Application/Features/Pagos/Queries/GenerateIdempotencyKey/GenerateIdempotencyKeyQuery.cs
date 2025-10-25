using MediatR;

namespace MiGenteEnLinea.Application.Features.Pagos.Queries.GenerateIdempotencyKey;

/// <summary>
/// Query para generar una clave de idempotencia de Cardnet.
/// </summary>
/// <remarks>
/// Mapeo desde Legacy: PaymentService.consultarIdempotency(url)
/// Caso de uso: Prevenir transacciones duplicadas en pagos con tarjeta.
/// La clave de idempotencia se usa para garantizar que una transacción
/// no se procese más de una vez, incluso si el cliente reintenta la petición.
/// </remarks>
public sealed record GenerateIdempotencyKeyQuery : IRequest<string>
{
    // No requiere parámetros - la URL de Cardnet está en configuración
}
