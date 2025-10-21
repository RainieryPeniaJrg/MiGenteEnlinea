using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Empleados.Queries.GetVistaContratacionTemporal;

/// <summary>
/// Validador para GetVistaContratacionTemporalQuery
/// </summary>
public class GetVistaContratacionTemporalQueryValidator : AbstractValidator<GetVistaContratacionTemporalQuery>
{
    public GetVistaContratacionTemporalQueryValidator()
    {
        RuleFor(x => x.ContratacionId)
            .GreaterThan(0).WithMessage("El ContratacionId debe ser mayor a 0");
        
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("El UserId es requerido");
    }
}
