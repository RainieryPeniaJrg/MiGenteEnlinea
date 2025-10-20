using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Empleados.Queries.GetReciboContratacion;

public class GetReciboContratacionQueryValidator : AbstractValidator<GetReciboContratacionQuery>
{
    public GetReciboContratacionQueryValidator()
    {
        RuleFor(x => x.PagoId)
            .GreaterThan(0)
            .WithMessage("El ID del pago debe ser mayor a 0");
    }
}
