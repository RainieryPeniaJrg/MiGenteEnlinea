using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Suscripciones.Commands.CreateSuscripcion;

/// <summary>
/// Validador para CreateSuscripcionCommand.
/// </summary>
public class CreateSuscripcionCommandValidator : AbstractValidator<CreateSuscripcionCommand>
{
    public CreateSuscripcionCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("El ID del usuario es requerido.");

        RuleFor(x => x.PlanId)
            .GreaterThan(0)
            .WithMessage("El ID del plan debe ser mayor a 0.");

        RuleFor(x => x.FechaInicio)
            .Must(fecha => !fecha.HasValue || fecha.Value <= DateTime.UtcNow.AddDays(1))
            .WithMessage("La fecha de inicio no puede ser mayor a mañana.")
            .When(x => x.FechaInicio.HasValue);
    }
}
