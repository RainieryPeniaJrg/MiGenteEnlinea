using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Contrataciones.Commands.AcceptContratacion;

/// <summary>
/// Validador para AcceptContratacionCommand.
/// 
/// REGLAS:
/// 1. DetalleId: Debe ser mayor a 0
/// </summary>
public class AcceptContratacionCommandValidator : AbstractValidator<AcceptContratacionCommand>
{
    public AcceptContratacionCommandValidator()
    {
        RuleFor(x => x.DetalleId)
            .GreaterThan(0)
            .WithMessage("El ID del detalle de contratación debe ser mayor a 0");
    }
}
