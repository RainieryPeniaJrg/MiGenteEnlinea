using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.AddProfileInfo;

/// <summary>
/// Validator para AddProfileInfoCommand
/// </summary>
public class AddProfileInfoCommandValidator : AbstractValidator<AddProfileInfoCommand>
{
    public AddProfileInfoCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("El UserId es requerido")
            .Must(BeValidGuid).WithMessage("El UserId debe ser un GUID válido");

        RuleFor(x => x.Identificacion)
            .NotEmpty().WithMessage("La identificación es requerida")
            .MaximumLength(20).WithMessage("La identificación no puede exceder 20 caracteres");

        RuleFor(x => x.NombreComercial)
            .MaximumLength(50).WithMessage("El nombre comercial no puede exceder 50 caracteres")
            .When(x => !string.IsNullOrWhiteSpace(x.NombreComercial));

        RuleFor(x => x.CedulaGerente)
            .MaximumLength(20).WithMessage("La cédula del gerente no puede exceder 20 caracteres")
            .When(x => !string.IsNullOrWhiteSpace(x.CedulaGerente));

        RuleFor(x => x.NombreGerente)
            .MaximumLength(50).WithMessage("El nombre del gerente no puede exceder 50 caracteres")
            .When(x => !string.IsNullOrWhiteSpace(x.NombreGerente));

        RuleFor(x => x.ApellidoGerente)
            .MaximumLength(50).WithMessage("El apellido del gerente no puede exceder 50 caracteres")
            .When(x => !string.IsNullOrWhiteSpace(x.ApellidoGerente));

        RuleFor(x => x.DireccionGerente)
            .MaximumLength(250).WithMessage("La dirección del gerente no puede exceder 250 caracteres")
            .When(x => !string.IsNullOrWhiteSpace(x.DireccionGerente));
    }

    private static bool BeValidGuid(string userId)
    {
        return Guid.TryParse(userId, out _);
    }
}
