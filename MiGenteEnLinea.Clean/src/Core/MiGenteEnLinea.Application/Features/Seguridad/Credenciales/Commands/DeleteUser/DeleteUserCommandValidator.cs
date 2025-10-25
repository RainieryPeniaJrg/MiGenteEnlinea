using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Seguridad.Credenciales.Commands.DeleteUser;

/// <summary>
/// Validador para DeleteUserCommand.
/// Validación mínima: campos requeridos.
/// </summary>
public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.UserID)
            .NotEmpty().WithMessage("El UserID es requerido")
            .MaximumLength(100).WithMessage("El UserID no puede exceder 100 caracteres");

        RuleFor(x => x.CredencialID)
            .GreaterThan(0).WithMessage("El CredencialID debe ser mayor a 0");
    }
}
