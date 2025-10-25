using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Utilitarios.Queries.ConvertirNumeroALetras;

/// <summary>
/// Validador para ConvertirNumeroALetrasQuery.
/// </summary>
public sealed class ConvertirNumeroALetrasQueryValidator 
    : AbstractValidator<ConvertirNumeroALetrasQuery>
{
    public ConvertirNumeroALetrasQueryValidator()
    {
        RuleFor(x => x.Numero)
            .GreaterThanOrEqualTo(0)
            .WithMessage("El número debe ser mayor o igual a 0")
            .LessThan(1_000_000_000_000_000m) // Un billón (límite del algoritmo Legacy)
            .WithMessage("El número excede el límite soportado (menos de 1 billón)");
    }
}
