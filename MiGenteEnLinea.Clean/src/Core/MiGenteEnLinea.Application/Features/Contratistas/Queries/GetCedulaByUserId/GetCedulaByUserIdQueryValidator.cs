using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Contratistas.Queries.GetCedulaByUserId;

/// <summary>
/// Validador para GetCedulaByUserIdQuery
/// </summary>
public sealed class GetCedulaByUserIdQueryValidator : AbstractValidator<GetCedulaByUserIdQuery>
{
    public GetCedulaByUserIdQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("El UserId es requerido")
            .MaximumLength(128).WithMessage("El UserId no puede exceder 128 caracteres");
    }
}
