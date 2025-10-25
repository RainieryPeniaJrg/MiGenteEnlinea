using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.ChangePasswordById;

/// <summary>
/// Validador para ChangePasswordByIdCommand
/// </summary>
public sealed class ChangePasswordByIdCommandValidator : AbstractValidator<ChangePasswordByIdCommand>
{
    public ChangePasswordByIdCommandValidator()
    {
        RuleFor(x => x.CredencialId)
            .GreaterThan(0).WithMessage("El CredencialId debe ser mayor a 0");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("La nueva contraseña es requerida")
            .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres")
            .MaximumLength(100).WithMessage("La contraseña no puede exceder 100 caracteres");
    }
}
