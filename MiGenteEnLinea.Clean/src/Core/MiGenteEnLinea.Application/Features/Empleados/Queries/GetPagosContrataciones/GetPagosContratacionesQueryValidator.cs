using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Empleados.Queries.GetPagosContrataciones;

public class GetPagosContratacionesQueryValidator : AbstractValidator<GetPagosContratacionesQuery>
{
    public GetPagosContratacionesQueryValidator()
    {
        RuleFor(x => x.ContratacionId)
            .GreaterThan(0)
            .WithMessage("El ID de contratación debe ser mayor que 0");

        RuleFor(x => x.DetalleId)
            .GreaterThan(0)
            .WithMessage("El ID de detalle debe ser mayor que 0");
    }
}
