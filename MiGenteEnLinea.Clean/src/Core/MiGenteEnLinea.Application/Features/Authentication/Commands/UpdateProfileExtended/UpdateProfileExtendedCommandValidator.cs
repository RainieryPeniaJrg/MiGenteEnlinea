using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.UpdateProfileExtended;

/// <summary>
/// Validator para UpdateProfileExtendedCommand
/// </summary>
public class UpdateProfileExtendedCommandValidator : AbstractValidator<UpdateProfileExtendedCommand>
{
    public UpdateProfileExtendedCommandValidator()
    {
        // ====================================
        // VALIDACIONES DE PERFILE (REQUERIDAS)
        // ====================================
        
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId es requerido")
            .Must(ValidarGuid)
            .WithMessage("UserId debe ser un GUID válido");
        
        RuleFor(x => x.Nombre)
            .NotEmpty()
            .WithMessage("Nombre es requerido")
            .MaximumLength(100)
            .WithMessage("Nombre no puede exceder 100 caracteres");
        
        RuleFor(x => x.Apellido)
            .NotEmpty()
            .WithMessage("Apellido es requerido")
            .MaximumLength(100)
            .WithMessage("Apellido no puede exceder 100 caracteres");
        
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email es requerido")
            .EmailAddress()
            .WithMessage("Email debe ser válido")
            .MaximumLength(100)
            .WithMessage("Email no puede exceder 100 caracteres");
        
        RuleFor(x => x.Telefono1)
            .MaximumLength(20)
            .When(x => !string.IsNullOrEmpty(x.Telefono1))
            .WithMessage("Telefono1 no puede exceder 20 caracteres");
        
        RuleFor(x => x.Telefono2)
            .MaximumLength(20)
            .When(x => !string.IsNullOrEmpty(x.Telefono2))
            .WithMessage("Telefono2 no puede exceder 20 caracteres");
        
        RuleFor(x => x.Usuario)
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.Usuario))
            .WithMessage("Usuario no puede exceder 50 caracteres");

        // ====================================
        // VALIDACIONES DE PERFILESINFO (OPCIONALES)
        // ====================================
        
        RuleFor(x => x.Identificacion)
            .MaximumLength(20)
            .When(x => !string.IsNullOrEmpty(x.Identificacion))
            .WithMessage("Identificacion no puede exceder 20 caracteres");
        
        RuleFor(x => x.NombreComercial)
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.NombreComercial))
            .WithMessage("NombreComercial no puede exceder 50 caracteres");
        
        RuleFor(x => x.CedulaGerente)
            .MaximumLength(20)
            .When(x => !string.IsNullOrEmpty(x.CedulaGerente))
            .WithMessage("CedulaGerente no puede exceder 20 caracteres");
        
        RuleFor(x => x.NombreGerente)
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.NombreGerente))
            .WithMessage("NombreGerente no puede exceder 50 caracteres");
        
        RuleFor(x => x.ApellidoGerente)
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.ApellidoGerente))
            .WithMessage("ApellidoGerente no puede exceder 50 caracteres");
        
        RuleFor(x => x.DireccionGerente)
            .MaximumLength(250)
            .When(x => !string.IsNullOrEmpty(x.DireccionGerente))
            .WithMessage("DireccionGerente no puede exceder 250 caracteres");
    }

    private bool ValidarGuid(string? guid)
    {
        return !string.IsNullOrEmpty(guid) && Guid.TryParse(guid, out _);
    }
}
