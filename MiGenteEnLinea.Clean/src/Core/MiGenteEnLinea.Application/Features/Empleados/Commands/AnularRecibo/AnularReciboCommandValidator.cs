using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.AnularRecibo;

public class AnularReciboCommandValidator : AbstractValidator<AnularReciboCommand>
{
    public AnularReciboCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("El ID del usuario es requerido")
            .MaximumLength(450).WithMessage("El ID del usuario no puede exceder 450 caracteres");

        RuleFor(x => x.PagoId)
            .GreaterThan(0).WithMessage("El ID del pago debe ser mayor a 0");

        RuleFor(x => x.MotivoAnulacion)
            .MaximumLength(500).WithMessage("El motivo de anulación no puede exceder 500 caracteres")
            .When(x => !string.IsNullOrEmpty(x.MotivoAnulacion));
    }
}
