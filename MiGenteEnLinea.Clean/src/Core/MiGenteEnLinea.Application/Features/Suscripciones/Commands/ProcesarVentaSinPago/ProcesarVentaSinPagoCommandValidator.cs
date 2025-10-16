using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Suscripciones.Commands.ProcesarVentaSinPago;

/// <summary>
/// Validador para ProcesarVentaSinPagoCommand.
/// </summary>
public class ProcesarVentaSinPagoCommandValidator : AbstractValidator<ProcesarVentaSinPagoCommand>
{
    public ProcesarVentaSinPagoCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("El ID del usuario es requerido.");

        RuleFor(x => x.PlanId)
            .GreaterThan(0)
            .WithMessage("El ID del plan debe ser mayor a 0.");

        RuleFor(x => x.Motivo)
            .MaximumLength(250)
            .WithMessage("El motivo no puede exceder 250 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.Motivo));
    }
}
