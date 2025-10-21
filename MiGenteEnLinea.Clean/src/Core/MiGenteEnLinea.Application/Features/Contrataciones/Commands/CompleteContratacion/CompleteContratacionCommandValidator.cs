using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Contrataciones.Commands.CompleteContratacion;

public class CompleteContratacionCommandValidator : AbstractValidator<CompleteContratacionCommand>
{
    public CompleteContratacionCommandValidator()
    {
        RuleFor(x => x.DetalleId)
            .GreaterThan(0)
            .WithMessage("El ID del detalle de contratación debe ser mayor a 0");
    }
}
