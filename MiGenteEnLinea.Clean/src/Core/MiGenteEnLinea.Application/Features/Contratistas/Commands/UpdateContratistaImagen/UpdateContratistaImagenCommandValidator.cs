using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Contratistas.Commands.UpdateContratistaImagen;

/// <summary>
/// Validator: Validaciones de input para UpdateContratistaImagenCommand
/// </summary>
public class UpdateContratistaImagenCommandValidator : AbstractValidator<UpdateContratistaImagenCommand>
{
    public UpdateContratistaImagenCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId es requerido")
            .Must(BeValidGuid).WithMessage("UserId debe ser un GUID válido");

        RuleFor(x => x.ImagenUrl)
            .NotEmpty().WithMessage("ImagenUrl es requerida")
            .MaximumLength(150).WithMessage("ImagenUrl no puede exceder 150 caracteres")
            .Must(BeValidUrl).WithMessage("ImagenUrl debe ser una URL válida");
    }

    private bool BeValidGuid(string value)
    {
        return Guid.TryParse(value, out _);
    }

    private bool BeValidUrl(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        // Validar que sea una URL relativa o absoluta válida
        return Uri.TryCreate(value, UriKind.RelativeOrAbsolute, out _);
    }
}
