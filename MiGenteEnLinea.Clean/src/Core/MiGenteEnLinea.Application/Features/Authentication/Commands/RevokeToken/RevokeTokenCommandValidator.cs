using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.RevokeToken;

/// <summary>
/// Validador para RevokeTokenCommand
/// </summary>
public class RevokeTokenCommandValidator : AbstractValidator<RevokeTokenCommand>
{
    public RevokeTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage("El refresh token es requerido")
            .MinimumLength(20)
            .WithMessage("El refresh token no tiene el formato correcto (demasiado corto)")
            .MaximumLength(200)
            .WithMessage("El refresh token no tiene el formato correcto (demasiado largo)");

        RuleFor(x => x.IpAddress)
            .NotEmpty()
            .WithMessage("La dirección IP es requerida")
            .Matches(@"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$|^([0-9a-fA-F]{1,4}:){7}[0-9a-fA-F]{1,4}$|^unknown$")
            .WithMessage("La dirección IP no es válida (debe ser IPv4, IPv6, o 'unknown')");

        RuleFor(x => x.Reason)
            .MaximumLength(200)
            .WithMessage("La razón de revocación no puede exceder 200 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Reason));
    }
}
