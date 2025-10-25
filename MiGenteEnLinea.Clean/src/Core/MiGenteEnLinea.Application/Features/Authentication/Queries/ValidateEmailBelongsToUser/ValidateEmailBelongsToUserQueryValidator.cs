using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Authentication.Queries.ValidateEmailBelongsToUser;

/// <summary>
/// Validador para ValidateEmailBelongsToUserQuery.
/// </summary>
public sealed class ValidateEmailBelongsToUserQueryValidator 
    : AbstractValidator<ValidateEmailBelongsToUserQuery>
{
    public ValidateEmailBelongsToUserQueryValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El correo electrónico es requerido")
            .EmailAddress().WithMessage("Formato de correo electrónico inválido")
            .MaximumLength(100);

        RuleFor(x => x.UserID)
            .NotEmpty().WithMessage("El userID es requerido");
    }
}
