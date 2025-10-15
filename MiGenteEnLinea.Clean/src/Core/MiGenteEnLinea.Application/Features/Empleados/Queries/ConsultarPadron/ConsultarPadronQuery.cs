using MediatR;

namespace MiGenteEnLinea.Application.Features.Empleados.Queries.ConsultarPadron;

/// <summary>
/// Query para consultar una cédula en el Padrón Nacional Dominicano.
/// Integra con IPadronService para obtener datos del ciudadano.
/// </summary>
public record ConsultarPadronQuery : IRequest<PadronResultDto?>
{
    /// <summary>
    /// Cédula de identidad a consultar (11 dígitos, puede tener guiones).
    /// Ejemplo: "001-1234567-8" o "00112345678"
    /// </summary>
    public string Cedula { get; init; } = null!;
}
