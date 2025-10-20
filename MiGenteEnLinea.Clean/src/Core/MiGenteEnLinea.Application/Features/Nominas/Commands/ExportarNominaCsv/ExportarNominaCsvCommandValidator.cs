using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Nominas.Commands.ExportarNominaCsv;

public class ExportarNominaCsvCommandValidator : AbstractValidator<ExportarNominaCsvCommand>
{
    public ExportarNominaCsvCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("El UserId es requerido");

        RuleFor(x => x.Periodo)
            .NotEmpty()
            .WithMessage("El período es requerido")
            .Matches(@"^\d{4}-\d{2}$")
            .WithMessage("El período debe estar en formato YYYY-MM (ej: 2025-10)");
    }
}
