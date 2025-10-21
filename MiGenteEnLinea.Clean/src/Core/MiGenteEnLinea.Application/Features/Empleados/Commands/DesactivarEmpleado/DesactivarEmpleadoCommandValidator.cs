using FluentValidation;
using System;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.DesactivarEmpleado;

/// <summary>
/// Validador para DesactivarEmpleadoCommand.
/// </summary>
public class DesactivarEmpleadoCommandValidator : AbstractValidator<DesactivarEmpleadoCommand>
{
    public DesactivarEmpleadoCommandValidator()
    {
        RuleFor(x => x.EmpleadoId)
            .GreaterThan(0).WithMessage("EmpleadoId debe ser mayor a 0");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId es requerido")
            .MaximumLength(450);

        RuleFor(x => x.FechaBaja)
            .NotEmpty().WithMessage("Fecha de baja es requerida")
            .LessThanOrEqualTo(DateTime.Now)
            .WithMessage("Fecha de baja no puede ser futura");

        RuleFor(x => x.Prestaciones)
            .GreaterThanOrEqualTo(0).WithMessage("Prestaciones no puede ser negativo");

        RuleFor(x => x.MotivoBaja)
            .NotEmpty().WithMessage("Motivo de baja es requerido")
            .MaximumLength(500);
    }
}
