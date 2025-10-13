using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Contratistas.Commands.AddServicio;

/// <summary>
/// Validator: Validaciones de input para AddServicioCommand
/// </summary>
public class AddServicioCommandValidator : AbstractValidator<AddServicioCommand>
{
    public AddServicioCommandValidator()
    {
        RuleFor(x => x.ContratistaId)
            .GreaterThan(0).WithMessage("ContratistaId debe ser mayor a 0");

        RuleFor(x => x.DetalleServicio)
            .NotEmpty().WithMessage("DetalleServicio es requerido")
            .MaximumLength(250).WithMessage("DetalleServicio no puede exceder 250 caracteres");
    }
}
