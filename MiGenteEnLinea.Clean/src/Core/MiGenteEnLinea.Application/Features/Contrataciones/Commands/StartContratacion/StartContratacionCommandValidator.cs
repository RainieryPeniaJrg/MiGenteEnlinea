using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Contrataciones.Commands.StartContratacion;

public class StartContratacionCommandValidator : AbstractValidator<StartContratacionCommand>
{
    public StartContratacionCommandValidator()
    {
        RuleFor(x => x.DetalleId)
            .GreaterThan(0)
            .WithMessage("El ID del detalle de contratación debe ser mayor a 0");
    }
}
