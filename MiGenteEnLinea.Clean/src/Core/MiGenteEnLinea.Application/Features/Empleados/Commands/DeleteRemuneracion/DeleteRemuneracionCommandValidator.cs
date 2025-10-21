using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.DeleteRemuneracion;

public class DeleteRemuneracionCommandValidator : AbstractValidator<DeleteRemuneracionCommand>
{
    public DeleteRemuneracionCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId es requerido");

        RuleFor(x => x.RemuneracionId)
            .GreaterThan(0)
            .WithMessage("RemuneracionId debe ser mayor a 0");
    }
}
