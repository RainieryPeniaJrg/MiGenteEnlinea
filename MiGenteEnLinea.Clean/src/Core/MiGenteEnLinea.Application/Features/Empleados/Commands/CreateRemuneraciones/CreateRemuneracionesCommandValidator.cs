using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.CreateRemuneraciones;

/// <summary>
/// Validador para CreateRemuneracionesCommand.
/// </summary>
public class CreateRemuneracionesCommandValidator : AbstractValidator<CreateRemuneracionesCommand>
{
    public CreateRemuneracionesCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId es requerido");

        RuleFor(x => x.EmpleadoId)
            .GreaterThan(0)
            .WithMessage("EmpleadoId debe ser mayor a 0");

        RuleFor(x => x.Remuneraciones)
            .NotEmpty()
            .WithMessage("Debe proporcionar al menos una remuneración");

        RuleForEach(x => x.Remuneraciones).ChildRules(remuneracion =>
        {
            remuneracion.RuleFor(r => r.Descripcion)
                .NotEmpty()
                .WithMessage("Descripción es requerida")
                .MaximumLength(500)
                .WithMessage("Descripción no puede exceder 500 caracteres");

            remuneracion.RuleFor(r => r.Monto)
                .GreaterThan(0)
                .WithMessage("Monto debe ser mayor a 0");
        });
    }
}
