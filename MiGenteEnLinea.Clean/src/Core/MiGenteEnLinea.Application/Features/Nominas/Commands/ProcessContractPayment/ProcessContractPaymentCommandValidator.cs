using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Nominas.Commands.ProcessContractPayment;

/// <summary>
/// Validador para ProcessContractPaymentCommand.
/// </summary>
public class ProcessContractPaymentCommandValidator : AbstractValidator<ProcessContractPaymentCommand>
{
    public ProcessContractPaymentCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("El UserId es requerido")
            .MaximumLength(100);

        RuleFor(x => x.ContratacionId)
            .GreaterThan(0).WithMessage("El ContratacionId debe ser mayor a 0");

        RuleFor(x => x.DetalleId)
            .GreaterThan(0).WithMessage("El DetalleId debe ser mayor a 0");

        RuleFor(x => x.FechaRegistro)
            .NotEmpty().WithMessage("La fecha de registro es requerida");

        RuleFor(x => x.FechaPago)
            .NotEmpty().WithMessage("La fecha de pago es requerida");

        RuleFor(x => x.ConceptoPago)
            .NotEmpty().WithMessage("El concepto de pago es requerido")
            .MaximumLength(50);

        RuleFor(x => x.Tipo)
            .GreaterThan(0).WithMessage("El tipo debe ser mayor a 0");

        RuleFor(x => x.Detalles)
            .NotEmpty().WithMessage("Debe incluir al menos un detalle de pago");

        RuleForEach(x => x.Detalles).ChildRules(detalle =>
        {
            detalle.RuleFor(d => d.Concepto)
                .NotEmpty().WithMessage("El concepto del detalle es requerido")
                .MaximumLength(90);

            detalle.RuleFor(d => d.Monto)
                .GreaterThan(0).WithMessage("El monto debe ser mayor a 0");
        });
    }
}
