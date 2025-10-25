using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Contrataciones.Commands.CancelarTrabajo;

/// <summary>
/// Validator para CancelarTrabajoCommand.
/// </summary>
public class CancelarTrabajoCommandValidator : AbstractValidator<CancelarTrabajoCommand>
{
    public CancelarTrabajoCommandValidator()
    {
        RuleFor(x => x.ContratacionId)
            .GreaterThan(0)
            .WithMessage("El ID de contratación debe ser mayor a 0");

        RuleFor(x => x.DetalleId)
            .GreaterThan(0)
            .WithMessage("El ID de detalle debe ser mayor a 0");
    }
}
