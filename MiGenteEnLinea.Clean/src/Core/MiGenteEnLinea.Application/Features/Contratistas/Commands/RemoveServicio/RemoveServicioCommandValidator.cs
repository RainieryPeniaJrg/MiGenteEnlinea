using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Contratistas.Commands.RemoveServicio;

/// <summary>
/// Validator: Validaciones de input para RemoveServicioCommand
/// </summary>
public class RemoveServicioCommandValidator : AbstractValidator<RemoveServicioCommand>
{
    public RemoveServicioCommandValidator()
    {
        RuleFor(x => x.ServicioId)
            .GreaterThan(0).WithMessage("ServicioId debe ser mayor a 0");

        RuleFor(x => x.ContratistaId)
            .GreaterThan(0).WithMessage("ContratistaId debe ser mayor a 0");
    }
}
