using FluentValidation;
using System;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.UpdateEmpleado;

/// <summary>
/// Validador para UpdateEmpleadoCommand.
/// Solo valida campos que se están actualizando (no nulos).
/// </summary>
public class UpdateEmpleadoCommandValidator : AbstractValidator<UpdateEmpleadoCommand>
{
    public UpdateEmpleadoCommandValidator()
    {
        RuleFor(x => x.EmpleadoId)
            .GreaterThan(0).WithMessage("EmpleadoId debe ser mayor a 0");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId es requerido")
            .MaximumLength(450);

        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("Nombre no puede estar vacío")
            .MaximumLength(100)
            .When(x => x.Nombre != null);

        RuleFor(x => x.Apellido)
            .NotEmpty().WithMessage("Apellido no puede estar vacío")
            .MaximumLength(100)
            .When(x => x.Apellido != null);

        RuleFor(x => x.Alias)
            .MaximumLength(100)
            .When(x => x.Alias != null);

        RuleFor(x => x.EstadoCivil)
            .InclusiveBetween(1, 4)
            .WithMessage("Estado civil debe ser: 1=Soltero, 2=Casado, 3=Divorciado, 4=Viudo")
            .When(x => x.EstadoCivil.HasValue);

        RuleFor(x => x.Nacimiento)
            .LessThan(DateTime.Now.AddYears(-16))
            .WithMessage("Empleado debe tener al menos 16 años")
            .When(x => x.Nacimiento.HasValue);

        RuleFor(x => x.Telefono1)
            .Matches(@"^[0-9]{10}$")
            .WithMessage("Teléfono debe tener 10 dígitos")
            .When(x => !string.IsNullOrEmpty(x.Telefono1));

        RuleFor(x => x.Telefono2)
            .Matches(@"^[0-9]{10}$")
            .WithMessage("Teléfono debe tener 10 dígitos")
            .When(x => !string.IsNullOrEmpty(x.Telefono2));

        RuleFor(x => x.FechaInicio)
            .LessThanOrEqualTo(DateTime.Now)
            .WithMessage("Fecha de inicio no puede ser futura")
            .When(x => x.FechaInicio.HasValue);

        RuleFor(x => x.Salario)
            .GreaterThan(0).WithMessage("Salario debe ser mayor a 0")
            .LessThanOrEqualTo(10000000).WithMessage("Salario excede el límite permitido")
            .When(x => x.Salario.HasValue);

        RuleFor(x => x.PeriodoPago)
            .InclusiveBetween(1, 3)
            .WithMessage("Período de pago debe ser: 1=Semanal, 2=Quincenal, 3=Mensual")
            .When(x => x.PeriodoPago.HasValue);

        RuleFor(x => x.DiasPago)
            .InclusiveBetween(1, 31)
            .WithMessage("Días de pago debe estar entre 1 y 31")
            .When(x => x.DiasPago.HasValue);
    }
}
