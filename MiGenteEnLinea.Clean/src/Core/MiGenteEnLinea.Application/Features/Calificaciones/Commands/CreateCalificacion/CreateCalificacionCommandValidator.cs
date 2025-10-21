using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Calificaciones.Commands.CreateCalificacion;

/// <summary>
/// Validador para CreateCalificacionCommand (4 dimensiones)
/// </summary>
public class CreateCalificacionCommandValidator : AbstractValidator<CreateCalificacionCommand>
{
    public CreateCalificacionCommandValidator()
    {
        RuleFor(x => x.EmpleadorUserId)
            .NotEmpty()
            .WithMessage("El ID del empleador es requerido")
            .MaximumLength(50)
            .WithMessage("El ID del empleador no puede exceder 50 caracteres");

        RuleFor(x => x.ContratistaIdentificacion)
            .NotEmpty()
            .WithMessage("La identificación del contratista es requerida")
            .MaximumLength(20)
            .WithMessage("La identificación no puede exceder 20 caracteres")
            .Matches(@"^[0-9-]+$")
            .WithMessage("La identificación solo puede contener números y guiones");

        RuleFor(x => x.ContratistaNombre)
            .NotEmpty()
            .WithMessage("El nombre del contratista es requerido")
            .MaximumLength(100)
            .WithMessage("El nombre no puede exceder 100 caracteres");

        // Validar las 4 dimensiones (1-5 estrellas cada una)
        RuleFor(x => x.Puntualidad)
            .InclusiveBetween(1, 5)
            .WithMessage("La calificación de puntualidad debe estar entre 1 y 5 estrellas");

        RuleFor(x => x.Cumplimiento)
            .InclusiveBetween(1, 5)
            .WithMessage("La calificación de cumplimiento debe estar entre 1 y 5 estrellas");

        RuleFor(x => x.Conocimientos)
            .InclusiveBetween(1, 5)
            .WithMessage("La calificación de conocimientos debe estar entre 1 y 5 estrellas");

        RuleFor(x => x.Recomendacion)
            .InclusiveBetween(1, 5)
            .WithMessage("La calificación de recomendación debe estar entre 1 y 5 estrellas");

        // Regla de negocio: No puede calificarse a sí mismo
        RuleFor(x => x)
            .Must(x => x.EmpleadorUserId != x.ContratistaIdentificacion)
            .WithMessage("No puedes calificarte a ti mismo");
    }
}
