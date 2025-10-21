using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.CalificarContratacion;

public class CalificarContratacionCommandValidator : AbstractValidator<CalificarContratacionCommand>
{
    public CalificarContratacionCommandValidator()
    {
        RuleFor(x => x.ContratacionId)
            .GreaterThan(0).WithMessage("El ContratacionId debe ser mayor a 0");

        RuleFor(x => x.CalificacionId)
            .GreaterThan(0).WithMessage("El CalificacionId debe ser mayor a 0");
    }
}
