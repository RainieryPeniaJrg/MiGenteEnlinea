using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Suscripciones.Commands.UpdateSuscripcion;

/// <summary>
/// Validador para UpdateSuscripcionCommand.
/// </summary>
public class UpdateSuscripcionCommandValidator : AbstractValidator<UpdateSuscripcionCommand>
{
    public UpdateSuscripcionCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("El ID del usuario es requerido.");

        RuleFor(x => x.NuevoPlanId)
            .GreaterThan(0)
            .WithMessage("El ID del plan debe ser mayor a 0.");

        RuleFor(x => x.NuevoVencimiento)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("La nueva fecha de vencimiento debe ser futura.");
    }
}
