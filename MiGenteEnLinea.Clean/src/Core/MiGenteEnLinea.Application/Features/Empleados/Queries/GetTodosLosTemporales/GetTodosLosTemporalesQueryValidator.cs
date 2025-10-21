using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Empleados.Queries.GetTodosLosTemporales;

/// <summary>
/// Validador para GetTodosLosTemporalesQuery
/// </summary>
public class GetTodosLosTemporalesQueryValidator : AbstractValidator<GetTodosLosTemporalesQuery>
{
    public GetTodosLosTemporalesQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("El UserId es requerido");
    }
}
