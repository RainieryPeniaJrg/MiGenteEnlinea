using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.EliminarRecibo;

public class EliminarReciboEmpleadoCommandValidator : AbstractValidator<EliminarReciboEmpleadoCommand>
{
    public EliminarReciboEmpleadoCommandValidator()
    {
        RuleFor(x => x.PagoId)
            .GreaterThan(0)
            .WithMessage("El ID de pago debe ser mayor que 0");
    }
}
