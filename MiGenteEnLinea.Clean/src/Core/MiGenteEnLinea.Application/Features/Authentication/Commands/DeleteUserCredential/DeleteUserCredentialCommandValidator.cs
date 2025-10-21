using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.DeleteUserCredential;

/// <summary>
/// Validator para DeleteUserCredentialCommand
/// </summary>
public class DeleteUserCredentialCommandValidator : AbstractValidator<DeleteUserCredentialCommand>
{
    public DeleteUserCredentialCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId es requerido");

        RuleFor(x => x.CredentialId)
            .GreaterThan(0)
            .WithMessage("CredentialId debe ser mayor que 0");
    }
}
