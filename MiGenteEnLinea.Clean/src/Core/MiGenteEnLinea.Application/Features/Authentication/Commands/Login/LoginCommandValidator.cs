using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.Login;

/// <summary>
/// Validador para LoginCommand
/// </summary>
public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El correo electrónico es requerido")
            .EmailAddress().WithMessage("El correo electrónico no es válido")
            .MaximumLength(100).WithMessage("El correo no puede exceder 100 caracteres");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es requerida")
            .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres")
            .MaximumLength(100).WithMessage("La contraseña no puede exceder 100 caracteres");

        RuleFor(x => x.IpAddress)
            .NotEmpty().WithMessage("La dirección IP es requerida")
            .MaximumLength(50).WithMessage("La dirección IP no puede exceder 50 caracteres");
    }
}
