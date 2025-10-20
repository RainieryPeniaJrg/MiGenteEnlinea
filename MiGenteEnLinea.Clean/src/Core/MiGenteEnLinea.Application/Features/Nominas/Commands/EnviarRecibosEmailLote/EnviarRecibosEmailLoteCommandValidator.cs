using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Nominas.Commands.EnviarRecibosEmailLote;

/// <summary>
/// Validador para EnviarRecibosEmailLoteCommand
/// 
/// REGLAS DE VALIDACIÓN:
/// 1. ReciboIds no puede estar vacío
/// 2. ReciboIds no puede contener duplicados
/// 3. Cada ReciboId debe ser mayor que 0
/// 4. Asunto (si se provee) debe tener longitud razonable
/// 5. MensajeAdicional (si se provee) no debe exceder límite
/// </summary>
public class EnviarRecibosEmailLoteCommandValidator
    : AbstractValidator<EnviarRecibosEmailLoteCommand>
{
    public EnviarRecibosEmailLoteCommandValidator()
    {
        // Validar ReciboIds
        RuleFor(x => x.ReciboIds)
            .NotEmpty()
            .WithMessage("Debe proporcionar al menos un recibo para enviar");

        RuleFor(x => x.ReciboIds)
            .Must(recibos => recibos.Count > 0)
            .WithMessage("La lista de recibos no puede estar vacía");

        RuleFor(x => x.ReciboIds)
            .Must(recibos => recibos.All(id => id > 0))
            .WithMessage("Todos los IDs de recibos deben ser mayores que 0");

        RuleFor(x => x.ReciboIds)
            .Must(recibos => recibos.Distinct().Count() == recibos.Count)
            .WithMessage("La lista de recibos contiene IDs duplicados");

        // Limitar cantidad máxima para evitar sobrecarga del servidor de email
        RuleFor(x => x.ReciboIds)
            .Must(recibos => recibos.Count <= 100)
            .WithMessage("No se pueden enviar más de 100 recibos por lote. Use múltiples lotes para cantidades mayores.");

        // Validar Asunto (opcional)
        When(x => !string.IsNullOrWhiteSpace(x.Asunto), () =>
        {
            RuleFor(x => x.Asunto)
                .MaximumLength(200)
                .WithMessage("El asunto del email no puede exceder 200 caracteres");

            RuleFor(x => x.Asunto)
                .MinimumLength(5)
                .WithMessage("El asunto del email debe tener al menos 5 caracteres");
        });

        // Validar MensajeAdicional (opcional)
        When(x => !string.IsNullOrWhiteSpace(x.MensajeAdicional), () =>
        {
            RuleFor(x => x.MensajeAdicional)
                .MaximumLength(1000)
                .WithMessage("El mensaje adicional no puede exceder 1000 caracteres");
        });
    }
}
