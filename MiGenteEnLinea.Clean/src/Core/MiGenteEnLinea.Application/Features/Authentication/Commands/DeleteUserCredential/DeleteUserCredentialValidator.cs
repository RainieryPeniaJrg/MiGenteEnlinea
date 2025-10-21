using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.DeleteUserCredential;

/// <summary>
/// Validador para DeleteUserCredentialCommand
/// </summary>
public class DeleteUserCredentialValidator : AbstractValidator<DeleteUserCredentialCommand>
{
    public DeleteUserCredentialValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId es requerido")
            .Must(BeValidGuid).WithMessage("UserId debe ser un GUID válido");

        RuleFor(x => x.CredentialId)
            .GreaterThan(0).WithMessage("CredentialId debe ser mayor a 0");
    }

    private bool BeValidGuid(string userId)
    {
        return Guid.TryParse(userId, out _);
    }
}
