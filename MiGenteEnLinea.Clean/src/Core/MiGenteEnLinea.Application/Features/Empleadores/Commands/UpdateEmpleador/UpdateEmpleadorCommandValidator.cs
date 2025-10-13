using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Empleadores.Commands.UpdateEmpleador;

/// <summary>
/// Validador: UpdateEmpleadorCommand
/// </summary>
public sealed class UpdateEmpleadorCommandValidator : AbstractValidator<UpdateEmpleadorCommand>
{
    public UpdateEmpleadorCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId es requerido")
            .Must(BeValidGuid).WithMessage("UserId debe ser un GUID válido");

        RuleFor(x => x.Habilidades)
            .MaximumLength(200).WithMessage("Habilidades no puede exceder 200 caracteres")
            .When(x => !string.IsNullOrWhiteSpace(x.Habilidades));

        RuleFor(x => x.Experiencia)
            .MaximumLength(200).WithMessage("Experiencia no puede exceder 200 caracteres")
            .When(x => !string.IsNullOrWhiteSpace(x.Experiencia));

        RuleFor(x => x.Descripcion)
            .MaximumLength(500).WithMessage("Descripcion no puede exceder 500 caracteres")
            .When(x => !string.IsNullOrWhiteSpace(x.Descripcion));

        // Al menos un campo debe ser proporcionado
        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.Habilidades) ||
                      !string.IsNullOrWhiteSpace(x.Experiencia) ||
                      !string.IsNullOrWhiteSpace(x.Descripcion))
            .WithMessage("Al menos un campo debe ser proporcionado para actualizar");
    }

    private bool BeValidGuid(string userId)
    {
        return Guid.TryParse(userId, out _);
    }
}
