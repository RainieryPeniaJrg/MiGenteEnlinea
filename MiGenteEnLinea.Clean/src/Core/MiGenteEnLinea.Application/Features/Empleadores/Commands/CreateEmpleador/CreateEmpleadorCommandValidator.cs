using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Empleadores.Commands.CreateEmpleador;

/// <summary>
/// Validador: CreateEmpleadorCommand
/// </summary>
public sealed class CreateEmpleadorCommandValidator : AbstractValidator<CreateEmpleadorCommand>
{
    public CreateEmpleadorCommandValidator()
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
    }

    private bool BeValidGuid(string userId)
    {
        return Guid.TryParse(userId, out _);
    }
}
