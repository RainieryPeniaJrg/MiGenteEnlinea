using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.ActivateAccount;

/// <summary>
/// Validador para ActivateAccountCommand
/// </summary>
public sealed class ActivateAccountCommandValidator : AbstractValidator<ActivateAccountCommand>
{
    public ActivateAccountCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("El UserId es requerido")
            .Must(BeAValidGuid).WithMessage("El UserId debe ser un GUID válido");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El correo electrónico es requerido")
            .EmailAddress().WithMessage("El correo electrónico no es válido");
    }

    private bool BeAValidGuid(string userId)
    {
        return Guid.TryParse(userId, out _);
    }
}
