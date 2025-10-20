using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Calificaciones.Commands.CalificarPerfil;

public class CalificarPerfilCommandValidator : AbstractValidator<CalificarPerfilCommand>
{
    public CalificarPerfilCommandValidator()
    {
        RuleFor(x => x.EmpleadorUserId)
            .NotEmpty()
            .WithMessage("EmpleadorUserId es requerido")
            .MaximumLength(450)
            .WithMessage("EmpleadorUserId no puede exceder 450 caracteres");

        RuleFor(x => x.ContratistaIdentificacion)
            .NotEmpty()
            .WithMessage("ContratistaIdentificacion es requerida")
            .MaximumLength(20)
            .WithMessage("ContratistaIdentificacion no puede exceder 20 caracteres");

        RuleFor(x => x.ContratistaNombre)
            .NotEmpty()
            .WithMessage("ContratistaNombre es requerido")
            .MaximumLength(100)
            .WithMessage("ContratistaNombre no puede exceder 100 caracteres");

        RuleFor(x => x.Puntualidad)
            .InclusiveBetween(1, 5)
            .WithMessage("Puntualidad debe estar entre 1 y 5");

        RuleFor(x => x.Cumplimiento)
            .InclusiveBetween(1, 5)
            .WithMessage("Cumplimiento debe estar entre 1 y 5");

        RuleFor(x => x.Conocimientos)
            .InclusiveBetween(1, 5)
            .WithMessage("Conocimientos debe estar entre 1 y 5");

        RuleFor(x => x.Recomendacion)
            .InclusiveBetween(1, 5)
            .WithMessage("Recomendacion debe estar entre 1 y 5");
    }
}
