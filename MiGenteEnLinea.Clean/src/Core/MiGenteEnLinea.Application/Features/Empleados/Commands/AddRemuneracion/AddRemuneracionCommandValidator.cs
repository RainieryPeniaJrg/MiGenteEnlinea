using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.AddRemuneracion;

/// <summary>
/// Validador para AddRemuneracionCommand.
/// Valida que el número de remuneración sea válido (1-3) y que los datos sean coherentes.
/// </summary>
public class AddRemuneracionCommandValidator : AbstractValidator<AddRemuneracionCommand>
{
    public AddRemuneracionCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId es requerido")
            .MaximumLength(450);

        RuleFor(x => x.EmpleadoId)
            .GreaterThan(0).WithMessage("EmpleadoId debe ser mayor a 0");

        RuleFor(x => x.Numero)
            .InclusiveBetween(1, 3)
            .WithMessage("El número de remuneración debe ser 1, 2 o 3 (máximo 3 remuneraciones por empleado)");

        RuleFor(x => x.Descripcion)
            .NotEmpty().WithMessage("La descripción de la remuneración es requerida")
            .MaximumLength(200).WithMessage("La descripción no puede exceder 200 caracteres");

        RuleFor(x => x.Monto)
            .GreaterThanOrEqualTo(0).WithMessage("El monto debe ser mayor o igual a cero");
    }
}
