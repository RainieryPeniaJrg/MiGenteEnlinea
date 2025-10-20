using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Dashboard.Queries.GetDashboardContratista;

/// <summary>
/// Validador para GetDashboardContratistaQuery.
/// Valida que el UserId esté presente y la FechaReferencia sea válida.
/// </summary>
public class GetDashboardContratistaQueryValidator : AbstractValidator<GetDashboardContratistaQuery>
{
    public GetDashboardContratistaQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("El UserId es requerido para obtener el dashboard del contratista");

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
