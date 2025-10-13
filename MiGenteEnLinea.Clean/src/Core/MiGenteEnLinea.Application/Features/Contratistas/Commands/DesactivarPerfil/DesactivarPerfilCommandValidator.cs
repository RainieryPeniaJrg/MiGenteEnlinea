using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Contratistas.Commands.DesactivarPerfil;

/// <summary>
/// Validator: Validaciones de input para DesactivarPerfilCommand
/// </summary>
public class DesactivarPerfilCommandValidator : AbstractValidator<DesactivarPerfilCommand>
{
    public DesactivarPerfilCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId es requerido")
            .Must(BeValidGuid).WithMessage("UserId debe ser un GUID válido");
    }

    private bool BeValidGuid(string value)
    {
        return Guid.TryParse(value, out _);
    }
}
