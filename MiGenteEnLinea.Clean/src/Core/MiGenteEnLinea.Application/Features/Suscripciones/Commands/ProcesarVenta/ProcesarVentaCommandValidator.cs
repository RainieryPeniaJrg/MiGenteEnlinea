using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Suscripciones.Commands.ProcesarVenta;

/// <summary>
/// Validador para ProcesarVentaCommand.
/// </summary>
public class ProcesarVentaCommandValidator : AbstractValidator<ProcesarVentaCommand>
{
    public ProcesarVentaCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId es requerido");

        RuleFor(x => x.PlanId)
            .GreaterThan(0).WithMessage("PlanId debe ser mayor a 0");

        RuleFor(x => x.CardNumber)
            .NotEmpty().WithMessage("Número de tarjeta es requerido")
            .Matches(@"^\d{13,19}$").WithMessage("Número de tarjeta debe tener entre 13 y 19 dígitos")
            .CreditCard().WithMessage("Número de tarjeta no es válido");

        RuleFor(x => x.Cvv)
            .NotEmpty().WithMessage("CVV es requerido")
            .Matches(@"^\d{3,4}$").WithMessage("CVV debe tener 3 o 4 dígitos");

        RuleFor(x => x.ExpirationDate)
            .NotEmpty().WithMessage("Fecha de expiración es requerida")
            .Matches(@"^(0[1-9]|1[0-2])\d{2}$").WithMessage("Fecha de expiración debe estar en formato MMYY")
            .Must(BeValidExpirationDate).WithMessage("Tarjeta expirada");

        RuleFor(x => x.ClientIp)
            .Matches(@"^(\d{1,3}\.){3}\d{1,3}$").WithMessage("Dirección IP no válida")
            .When(x => !string.IsNullOrEmpty(x.ClientIp));

        RuleFor(x => x.ReferenceNumber)
            .MaximumLength(50).WithMessage("Número de referencia no puede exceder 50 caracteres")
            .When(x => !string.IsNullOrEmpty(x.ReferenceNumber));

        RuleFor(x => x.InvoiceNumber)
            .MaximumLength(50).WithMessage("Número de factura no puede exceder 50 caracteres")
            .When(x => !string.IsNullOrEmpty(x.InvoiceNumber));
    }

    /// <summary>
    /// Valida que la tarjeta no esté expirada.
    /// </summary>
    private bool BeValidExpirationDate(string expirationDate)
    {
        if (string.IsNullOrWhiteSpace(expirationDate) || expirationDate.Length != 4)
            return false;

        if (!int.TryParse(expirationDate.Substring(0, 2), out int month))
            return false;

        if (!int.TryParse(expirationDate.Substring(2, 2), out int year))
            return false;

        // Año es 2 dígitos (ej: 25 = 2025)
        var fullYear = 2000 + year;
        var expirationDateObj = new DateTime(fullYear, month, 1).AddMonths(1).AddDays(-1);

        return expirationDateObj >= DateTime.UtcNow.Date;
    }
}
