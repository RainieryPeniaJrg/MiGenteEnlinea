using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Contratistas.Commands.ActivarPerfil;

/// <summary>
/// Validator: Validaciones de input para ActivarPerfilCommand
/// </summary>
public class ActivarPerfilCommandValidator : AbstractValidator<ActivarPerfilCommand>
{
    public ActivarPerfilCommandValidator()
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
