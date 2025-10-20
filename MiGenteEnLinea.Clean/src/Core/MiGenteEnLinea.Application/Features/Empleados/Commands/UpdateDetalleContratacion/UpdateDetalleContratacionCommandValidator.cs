using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.UpdateDetalleContratacion;

public class UpdateDetalleContratacionCommandValidator : AbstractValidator<UpdateDetalleContratacionCommand>
{
    public UpdateDetalleContratacionCommandValidator()
    {
        RuleFor(x => x.ContratacionId)
            .GreaterThan(0)
            .WithMessage("El ID de contratación debe ser mayor que 0");

        RuleFor(x => x.FechaInicio)
            .NotNull()
            .When(x => x.FechaFin.HasValue)
            .WithMessage("La fecha de inicio es requerida cuando se especifica fecha de fin");

        RuleFor(x => x.FechaFin)
            .GreaterThan(x => x.FechaInicio)
            .When(x => x.FechaInicio.HasValue && x.FechaFin.HasValue)
            .WithMessage("La fecha de fin debe ser posterior a la fecha de inicio");

        RuleFor(x => x.MontoAcordado)
            .GreaterThan(0)
            .When(x => x.MontoAcordado.HasValue)
            .WithMessage("El monto acordado debe ser mayor que 0");
    }
}
