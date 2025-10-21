using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Empleadores.Commands.DeleteEmpleador;

/// <summary>
/// Validador: DeleteEmpleadorCommand
/// </summary>
public sealed class DeleteEmpleadorCommandValidator : AbstractValidator<DeleteEmpleadorCommand>
{
    public DeleteEmpleadorCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId es requerido")
            .Must(BeValidGuid).WithMessage("UserId debe ser un GUID válido");
    }

    private bool BeValidGuid(string userId)
    {
        return Guid.TryParse(userId, out _);
    }
}
