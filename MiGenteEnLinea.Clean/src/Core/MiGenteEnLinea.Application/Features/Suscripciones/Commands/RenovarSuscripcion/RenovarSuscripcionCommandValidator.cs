using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Suscripciones.Commands.RenovarSuscripcion;

/// <summary>
/// Validador para RenovarSuscripcionCommand.
/// </summary>
public class RenovarSuscripcionCommandValidator : AbstractValidator<RenovarSuscripcionCommand>
{
    public RenovarSuscripcionCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("El ID del usuario es requerido.");

        RuleFor(x => x.MesesExtension)
            .GreaterThan(0)
            .WithMessage("Los meses de extensión deben ser mayores a 0.")
            .LessThanOrEqualTo(24)
            .WithMessage("La extensión máxima es de 24 meses.");

        RuleFor(x => x.Motivo)
            .MaximumLength(250)
            .WithMessage("El motivo no puede exceder 250 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.Motivo));
    }
}
