using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Contrataciones.Commands.RejectContratacion;

/// <summary>
/// Validador para RejectContratacionCommand.
/// </summary>
public class RejectContratacionCommandValidator : AbstractValidator<RejectContratacionCommand>
{
    public RejectContratacionCommandValidator()
    {
        RuleFor(x => x.DetalleId)
            .GreaterThan(0)
            .WithMessage("El ID del detalle de contratación debe ser mayor a 0");

        RuleFor(x => x.Motivo)
            .NotEmpty()
            .WithMessage("El motivo del rechazo es requerido")
            .MinimumLength(10)
            .WithMessage("El motivo debe tener al menos 10 caracteres")
            .MaximumLength(500)
            .WithMessage("El motivo no puede exceder 500 caracteres");
    }
}
