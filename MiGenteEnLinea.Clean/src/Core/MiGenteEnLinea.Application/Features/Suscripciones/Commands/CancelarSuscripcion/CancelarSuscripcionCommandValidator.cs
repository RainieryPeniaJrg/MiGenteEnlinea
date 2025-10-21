using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Suscripciones.Commands.CancelarSuscripcion;

/// <summary>
/// Validador para CancelarSuscripcionCommand.
/// </summary>
public class CancelarSuscripcionCommandValidator : AbstractValidator<CancelarSuscripcionCommand>
{
    public CancelarSuscripcionCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("El ID del usuario es requerido.");

        RuleFor(x => x.MotivoCancelacion)
            .MaximumLength(250)
            .WithMessage("El motivo de cancelación no puede exceder 250 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.MotivoCancelacion));
    }
}
