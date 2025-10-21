using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Nominas.Commands.ProcesarNominaLote;

public class ProcesarNominaLoteCommandValidator : AbstractValidator<ProcesarNominaLoteCommand>
{
    public ProcesarNominaLoteCommandValidator()
    {
        RuleFor(x => x.EmpleadorId)
            .GreaterThan(0)
            .WithMessage("El ID del empleador es requerido");

        RuleFor(x => x.Periodo)
            .NotEmpty()
            .WithMessage("El período es requerido")
            .MaximumLength(20)
            .WithMessage("El período no puede exceder 20 caracteres");

        RuleFor(x => x.FechaPago)
            .NotEmpty()
            .WithMessage("La fecha de pago es requerida")
            .GreaterThanOrEqualTo(new DateTime(2020, 1, 1))
            .WithMessage("La fecha de pago debe ser posterior a 2020");

        RuleFor(x => x.Empleados)
            .NotEmpty()
            .WithMessage("Debe incluir al menos un empleado")
            .Must(list => list.Count <= 500)
            .WithMessage("No se pueden procesar más de 500 empleados en un lote");

        RuleForEach(x => x.Empleados).ChildRules(empleado =>
        {
            empleado.RuleFor(e => e.EmpleadoId)
                .GreaterThan(0)
                .WithMessage("El ID del empleado es requerido");

            empleado.RuleFor(e => e.Salario)
                .GreaterThan(0)
                .WithMessage("El salario debe ser mayor a 0")
                .LessThanOrEqualTo(1_000_000)
                .WithMessage("El salario no puede exceder RD$1,000,000");

            empleado.RuleForEach(e => e.Conceptos).ChildRules(concepto =>
            {
                concepto.RuleFor(c => c.Concepto)
                    .NotEmpty()
                    .WithMessage("El concepto es requerido")
                    .MaximumLength(100)
                    .WithMessage("El concepto no puede exceder 100 caracteres");

                concepto.RuleFor(c => c.Monto)
                    .GreaterThan(0)
                    .WithMessage("El monto del concepto debe ser mayor a 0");

                concepto.When(c => !string.IsNullOrEmpty(c.Detalle), () =>
                {
                    concepto.RuleFor(c => c.Detalle)
                        .MaximumLength(250)
                        .WithMessage("El detalle no puede exceder 250 caracteres");
                });
            });
        });

        When(x => !string.IsNullOrEmpty(x.Notas), () =>
        {
            RuleFor(x => x.Notas)
                .MaximumLength(500)
                .WithMessage("Las notas no pueden exceder 500 caracteres");
        });
    }
}
