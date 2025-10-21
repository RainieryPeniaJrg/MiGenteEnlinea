using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Empleadores.Commands.UpdateEmpleadorFoto;

/// <summary>
/// Validador: UpdateEmpleadorFotoCommand
/// </summary>
public sealed class UpdateEmpleadorFotoCommandValidator : AbstractValidator<UpdateEmpleadorFotoCommand>
{
    private const int MaxSizeBytes = 5 * 1024 * 1024; // 5MB

    public UpdateEmpleadorFotoCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId es requerido")
            .Must(BeValidGuid).WithMessage("UserId debe ser un GUID válido");

        RuleFor(x => x.Foto)
            .NotNull().WithMessage("Foto es requerida")
            .NotEmpty().WithMessage("Foto no puede estar vacía")
            .Must(foto => foto.Length <= MaxSizeBytes)
            .WithMessage($"La foto no puede exceder {MaxSizeBytes / (1024 * 1024)}MB");
    }

    private bool BeValidGuid(string userId)
    {
        return Guid.TryParse(userId, out _);
    }
}
