using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Empleados.Queries.GetFichaTemporales;

public class GetFichaTemporalesQueryValidator : AbstractValidator<GetFichaTemporalesQuery>
{
    public GetFichaTemporalesQueryValidator()
    {
        RuleFor(x => x.ContratacionId)
            .GreaterThan(0).WithMessage("El ContratacionId debe ser mayor a 0");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("El UserId es requerido");
    }
}
