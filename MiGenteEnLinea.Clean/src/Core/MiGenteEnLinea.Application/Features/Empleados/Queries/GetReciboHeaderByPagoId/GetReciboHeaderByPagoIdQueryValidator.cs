using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Empleados.Queries.GetReciboHeaderByPagoId;

/// <summary>
/// Validador para GetReciboHeaderByPagoIdQuery
/// </summary>
public class GetReciboHeaderByPagoIdQueryValidator : AbstractValidator<GetReciboHeaderByPagoIdQuery>
{
    public GetReciboHeaderByPagoIdQueryValidator()
    {
        RuleFor(x => x.PagoId)
            .GreaterThan(0).WithMessage("El PagoId debe ser mayor a 0");
    }
}
