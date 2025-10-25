using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.ResendActivationEmail;

/// <summary>
/// Validador para ResendActivationEmailCommand
/// </summary>
public sealed class ResendActivationEmailCommandValidator : AbstractValidator<ResendActivationEmailCommand>
{
    public ResendActivationEmailCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es requerido")
            .EmailAddress().WithMessage("El email debe ser válido")
            .MaximumLength(100).WithMessage("El email no puede exceder 100 caracteres");

        RuleFor(x => x.Host)
            .NotEmpty().WithMessage("El host es requerido")
            .Must(host => Uri.TryCreate(host, UriKind.Absolute, out _))
            .WithMessage("El host debe ser una URL válida");

        // UserId es opcional
        When(x => !string.IsNullOrWhiteSpace(x.UserId), () =>
        {
            RuleFor(x => x.UserId)
                .MaximumLength(128).WithMessage("El UserId no puede exceder 128 caracteres");
        });
    }
}
