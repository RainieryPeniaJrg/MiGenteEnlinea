using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.UpdateProfile;

/// <summary>
/// Validador para UpdateProfileCommand
/// </summary>
public sealed class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("El UserId es requerido")
            .Must(BeAValidGuid).WithMessage("El UserId debe ser un GUID válido");

        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(x => x.Apellido)
            .NotEmpty().WithMessage("El apellido es requerido")
            .MaximumLength(100).WithMessage("El apellido no puede exceder 100 caracteres");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El correo electrónico es requerido")
            .EmailAddress().WithMessage("El correo electrónico no es válido")
            .MaximumLength(100).WithMessage("El correo no puede exceder 100 caracteres");

        RuleFor(x => x.Telefono1)
            .MaximumLength(20).WithMessage("El teléfono 1 no puede exceder 20 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Telefono1));

        RuleFor(x => x.Telefono2)
            .MaximumLength(20).WithMessage("El teléfono 2 no puede exceder 20 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Telefono2));

        RuleFor(x => x.Usuario)
            .MaximumLength(50).WithMessage("El usuario no puede exceder 50 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Usuario));
    }

    private bool BeAValidGuid(string userId)
    {
        return Guid.TryParse(userId, out _);
    }
}
