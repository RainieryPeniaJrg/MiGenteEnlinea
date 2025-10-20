using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.EliminarEmpleadoTemporal;

public class EliminarEmpleadoTemporalCommandValidator : AbstractValidator<EliminarEmpleadoTemporalCommand>
{
    public EliminarEmpleadoTemporalCommandValidator()
    {
        RuleFor(x => x.ContratacionId)
            .GreaterThan(0)
            .WithMessage("El ID de contratación debe ser mayor a 0");
    }
}
