using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.ModificarCalificacion;

public class ModificarCalificacionCommandValidator : AbstractValidator<ModificarCalificacionCommand>
{
    public ModificarCalificacionCommandValidator()
    {
        RuleFor(x => x.CalificacionId)
            .GreaterThan(0).WithMessage("El CalificacionId debe ser mayor a 0");

        RuleFor(x => x.Conocimientos)
            .InclusiveBetween(1, 5)
            .When(x => x.Conocimientos.HasValue)
            .WithMessage("Conocimientos debe estar entre 1 y 5");

        RuleFor(x => x.Cumplimiento)
            .InclusiveBetween(1, 5)
            .When(x => x.Cumplimiento.HasValue)
            .WithMessage("Cumplimiento debe estar entre 1 y 5");

        RuleFor(x => x.Puntualidad)
            .InclusiveBetween(1, 5)
            .When(x => x.Puntualidad.HasValue)
            .WithMessage("Puntualidad debe estar entre 1 y 5");
    }
}
