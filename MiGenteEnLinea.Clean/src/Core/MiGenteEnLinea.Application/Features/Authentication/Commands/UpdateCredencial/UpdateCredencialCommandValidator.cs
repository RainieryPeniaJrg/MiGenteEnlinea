using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.UpdateCredencial;

/// <summary>
/// Validador para UpdateCredencialCommand
/// </summary>
public sealed class UpdateCredencialCommandValidator : AbstractValidator<UpdateCredencialCommand>
{
    public UpdateCredencialCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("El UserId es requerido")
            .MaximumLength(128).WithMessage("El UserId no puede exceder 128 caracteres");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es requerido")
            .EmailAddress().WithMessage("El email debe ser válido")
            .MaximumLength(100).WithMessage("El email no puede exceder 100 caracteres");

        // Password es opcional (si se provee, debe cumplir requisitos)
        When(x => !string.IsNullOrWhiteSpace(x.Password), () =>
        {
            RuleFor(x => x.Password)
                .MinimumLength(6).WithMessage("El password debe tener al menos 6 caracteres")
                .MaximumLength(100).WithMessage("El password no puede exceder 100 caracteres");
        });
    }
}
