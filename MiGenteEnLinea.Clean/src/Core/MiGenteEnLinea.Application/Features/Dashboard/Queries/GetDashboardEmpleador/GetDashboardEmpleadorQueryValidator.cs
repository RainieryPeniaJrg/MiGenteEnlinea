using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Dashboard.Queries.GetDashboardEmpleador;

/// <summary>
/// Validador para GetDashboardEmpleadorQuery.
/// </summary>
public class GetDashboardEmpleadorQueryValidator : AbstractValidator<GetDashboardEmpleadorQuery>
{
    public GetDashboardEmpleadorQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("El UserId es requerido para obtener el dashboard");

        // Validar fecha de referencia si se proporciona
        When(x => x.FechaReferencia.HasValue, () =>
        {
            RuleFor(x => x.FechaReferencia!.Value)
                .LessThanOrEqualTo(DateTime.UtcNow.AddDays(1))
                .WithMessage("La fecha de referencia no puede ser futura")
                .GreaterThanOrEqualTo(new DateTime(2020, 1, 1))
                .WithMessage("La fecha de referencia no puede ser anterior a 2020");
        });
    }
}
