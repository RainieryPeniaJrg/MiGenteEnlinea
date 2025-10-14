using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.RemoveRemuneracion;

/// <summary>
/// Validador para RemoveRemuneracionCommand.
/// Valida que el número de remuneración sea válido (1-3).
/// </summary>
public class RemoveRemuneracionCommandValidator : AbstractValidator<RemoveRemuneracionCommand>
{
    public RemoveRemuneracionCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId es requerido")
            .MaximumLength(450);

        RuleFor(x => x.EmpleadoId)
            .GreaterThan(0).WithMessage("EmpleadoId debe ser mayor a 0");

        RuleFor(x => x.Numero)
            .InclusiveBetween(1, 3)
            .WithMessage("El número de remuneración debe ser 1, 2 o 3");
    }
}
