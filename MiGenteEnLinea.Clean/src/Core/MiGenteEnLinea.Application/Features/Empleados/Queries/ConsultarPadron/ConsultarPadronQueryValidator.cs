using System.Linq;
using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Empleados.Queries.ConsultarPadron;

/// <summary>
/// Validador para ConsultarPadronQuery.
/// Valida formato de cédula dominicana (11 dígitos).
/// </summary>
public class ConsultarPadronQueryValidator : AbstractValidator<ConsultarPadronQuery>
{
    public ConsultarPadronQueryValidator()
    {
        RuleFor(x => x.Cedula)
            .NotEmpty()
            .WithMessage("La cédula es requerida")
            .Must(BeValidCedula)
            .WithMessage("La cédula debe tener 11 dígitos (puede incluir guiones: XXX-XXXXXXX-X)");
    }

    /// <summary>
    /// Valida que la cédula tenga el formato correcto (11 dígitos).
    /// Acepta formato con guiones: 001-1234567-8
    /// </summary>
    private bool BeValidCedula(string cedula)
    {
        if (string.IsNullOrWhiteSpace(cedula))
            return false;

        // Remover guiones y espacios
        var cedulaLimpia = cedula.Replace("-", "").Replace(" ", "").Trim();

        // Validar que tenga 11 dígitos
        return cedulaLimpia.Length == 11 && cedulaLimpia.All(char.IsDigit);
    }
}
