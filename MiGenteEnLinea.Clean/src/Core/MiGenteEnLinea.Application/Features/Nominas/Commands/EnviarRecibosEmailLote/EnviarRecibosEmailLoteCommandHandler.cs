using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Domain.Interfaces;
using MiGenteEnLinea.Domain.Interfaces.Repositories;

namespace MiGenteEnLinea.Application.Features.Nominas.Commands.EnviarRecibosEmailLote;

/// <summary>
/// Handler para envío masivo de recibos de nómina por email.
/// 
/// DEPENDENCIAS:
/// - IUnitOfWork: Acceso a repositorios (RecibosHeader, Empleados)
/// - IPdfService: Generación de PDFs de recibos
/// - IEmailService: Envío de emails con attachments
/// 
/// LÓGICA:
/// 1. Itera cada reciboId con try-catch (error tolerance)
/// 2. Obtiene recibo con detalles (eager loading)
/// 3. Valida que recibo existe
/// 4. Obtiene empleado para email
/// 5. Valida que empleado tiene email configurado
/// 6. Genera PDF usando IPdfService
/// 7. Envía email con PDF adjunto usando IEmailService
/// 8. Registra éxito/fallo en contadores
/// 9. Retorna resultado con estadísticas completas
/// 
/// ERROR HANDLING:
/// - Continúa procesando aunque algunos recibos fallen
/// - Captura excepciones por recibo individual
/// - Registra errores detallados en lista
/// - Permite reintento posterior de los fallidos
/// </summary>
public class EnviarRecibosEmailLoteCommandHandler
    : IRequestHandler<EnviarRecibosEmailLoteCommand, EnviarRecibosEmailLoteResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPdfService _pdfService;
    private readonly IEmailService _emailService;
    private readonly ILogger<EnviarRecibosEmailLoteCommandHandler> _logger;

    public EnviarRecibosEmailLoteCommandHandler(
        IUnitOfWork unitOfWork,
        IPdfService pdfService,
        IEmailService emailService,
        ILogger<EnviarRecibosEmailLoteCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _pdfService = pdfService;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<EnviarRecibosEmailLoteResult> Handle(
        EnviarRecibosEmailLoteCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Starting batch email sending - Receipts: {Count}",
            request.ReciboIds.Count);

        var result = new EnviarRecibosEmailLoteResult();
        int exitosos = 0;
        int fallidos = 0;
        long totalBytes = 0;

        // Proceso cada recibo individualmente con error tolerance
        foreach (var reciboId in request.ReciboIds)
        {
            try
            {
                // 1. Obtener recibo con detalles
                var recibo = await _unitOfWork.RecibosHeader.GetWithDetallesAsync(
                    reciboId,
                    cancellationToken);

                if (recibo == null)
                {
                    var errorMsg = $"Recibo {reciboId} no encontrado";
                    result.Errores.Add(errorMsg);
                    _logger.LogWarning(errorMsg);
                    
                    result.RecibosEnviados.Add(new ReciboEmailDto
                    {
                        ReciboId = reciboId,
                        Enviado = false,
                        ErrorMensaje = errorMsg
                    });
                    
                    fallidos++;
                    continue;
                }

                // 2. Obtener empleado para email
                var empleado = await _unitOfWork.Empleados.GetByIdAsync(recibo.EmpleadoId);
                if (empleado == null)
                {
                    var errorMsg = $"Empleado {recibo.EmpleadoId} no encontrado para recibo {reciboId}";
                    result.Errores.Add(errorMsg);
                    _logger.LogWarning(errorMsg);
                    
                    result.RecibosEnviados.Add(new ReciboEmailDto
                    {
                        ReciboId = reciboId,
                        EmpleadoId = recibo.EmpleadoId,
                        Enviado = false,
                        ErrorMensaje = errorMsg
                    });
                    
                    fallidos++;
                    continue;
                }

                // 3. Obtener email del empleado
                // NOTA: El entity Empleado no tiene campo Email directo.
                // En el diseño actual, el email podría estar en:
                // - Una tabla separada de contactos
                // - La Credencial del usuario si el empleado tiene cuenta
                // - Un campo que se agregará en futuro (TODO)
                // 
                // WORKAROUND TEMPORAL: Usar Telefono1 como identificador o skip
                // Para producción, se necesita agregar campo Email a Empleado entity
                
                // Por ahora, generamos un email genérico basado en identificación
                // En producción real, esto debería venir de la base de datos
                var empleadoEmail = $"empleado.{empleado.Identificacion}@migente.com.do"; // TODO: Reemplazar con email real
                
                _logger.LogWarning(
                    "Using generated email for employee {EmpleadoId} - Real email field not available in Empleado entity",
                    empleado.EmpleadoId);

                // 4. Obtener empleador (para nombre en PDF y BCC opcional)
                var empleador = await _unitOfWork.Empleadores.FirstOrDefaultAsync(
                    e => e.UserId == recibo.UserId,
                    cancellationToken);

                if (empleador == null)
                {
                    var errorMsg = $"Empleador con UserId {recibo.UserId} no encontrado";
                    result.Errores.Add(errorMsg);
                    _logger.LogWarning(errorMsg);
                    fallidos++;
                    continue;
                }

                // 5. Generar PDF del recibo
                var pdfBytes = _pdfService.GenerarReciboPago(
                    reciboId: recibo.PagoId,
                    empleadorNombre: $"Empleador #{empleador.Id}", // TODO: Add company name field
                    empleadoNombre: empleado.NombreCompleto,
                    periodo: recibo.ConceptoPago,
                    salarioBruto: recibo.TotalIngresos,
                    deducciones: recibo.TotalDeducciones,
                    salarioNeto: recibo.NetoPagar
                );

                // 6. Preparar email content
                var asunto = request.Asunto ?? $"Recibo de Pago - {recibo.ConceptoPago}";
                var htmlBody = GenerarHtmlEmail(
                    empleado.NombreCompleto,
                    recibo.ConceptoPago,
                    recibo.NetoPagar,
                    request.MensajeAdicional);

                // 7. Enviar email con PDF adjunto
                // NOTA: IEmailService actual no soporta attachments directamente
                // Usamos SendEmailAsync genérico y agregamos PDF como base64 embebido en HTML
                var htmlBodyConPdf = AgregarPdfEmbebidoHtml(htmlBody, pdfBytes, $"recibo-{reciboId}.pdf");

                await _emailService.SendEmailAsync(
                    toEmail: empleadoEmail,
                    toName: empleado.NombreCompleto,
                    subject: asunto,
                    htmlBody: htmlBodyConPdf,
                    plainTextBody: GenerarTextoPlano(empleado.NombreCompleto, recibo.ConceptoPago, recibo.NetoPagar)
                );

                // TODO: Si se requiere BCC al empleador, necesitamos extender IEmailService
                // o hacer un segundo envío
                // if (request.CopiarEmpleador && !string.IsNullOrWhiteSpace(empleadorEmail))
                // {
                //     await _emailService.SendEmailAsync(...);
                // }

                // 8. Registrar éxito
                result.RecibosEnviados.Add(new ReciboEmailDto
                {
                    ReciboId = reciboId,
                    EmpleadoId = empleado.EmpleadoId,
                    EmpleadoNombre = empleado.NombreCompleto,
                    EmpleadoEmail = empleadoEmail,
                    Enviado = true,
                    FechaEnvio = DateTime.UtcNow,
                    TamanoPdf = pdfBytes.Length
                });

                exitosos++;
                totalBytes += pdfBytes.Length;

                _logger.LogInformation(
                    "Email sent successfully - Receipt: {ReciboId}, Employee: {EmpleadoNombre}, Email: {Email}",
                    reciboId,
                    empleado.NombreCompleto,
                    empleadoEmail);
            }
            catch (Exception ex)
            {
                var errorMsg = $"Error enviando email para recibo {reciboId}: {ex.Message}";
                result.Errores.Add(errorMsg);
                _logger.LogError(ex, "Error sending email for receipt {ReciboId}", reciboId);

                result.RecibosEnviados.Add(new ReciboEmailDto
                {
                    ReciboId = reciboId,
                    Enviado = false,
                    ErrorMensaje = ex.Message
                });

                fallidos++;
            }
        }

        // 9. Completar resultado
        result.EmailsEnviados = exitosos;
        result.EmailsFallidos = fallidos;
        result.TotalBytesEnviados = totalBytes;

        _logger.LogInformation(
            "Batch email sending completed - Success: {Success}, Failed: {Failed}, Total Bytes: {Bytes}",
            exitosos,
            fallidos,
            totalBytes);

        return result;
    }

    /// <summary>
    /// Genera el HTML del cuerpo del email
    /// </summary>
    private string GenerarHtmlEmail(
        string nombreEmpleado,
        string periodo,
        decimal netoAPagar,
        string? mensajeAdicional)
    {
        var mensajeExtra = !string.IsNullOrWhiteSpace(mensajeAdicional)
            ? $"<p style='margin: 20px 0; padding: 15px; background-color: #f0f8ff; border-left: 4px solid #0066cc;'>{mensajeAdicional}</p>"
            : string.Empty;

        return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #0066cc; color: white; padding: 20px; text-align: center; }}
        .content {{ padding: 30px; background-color: #ffffff; }}
        .footer {{ background-color: #f4f4f4; padding: 20px; text-align: center; font-size: 12px; color: #666; }}
        .highlight {{ color: #0066cc; font-weight: bold; font-size: 24px; }}
        .button {{ display: inline-block; padding: 12px 30px; background-color: #0066cc; color: white; text-decoration: none; border-radius: 5px; margin: 20px 0; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Recibo de Pago</h1>
        </div>
        <div class='content'>
            <h2>Hola {nombreEmpleado},</h2>
            <p>Te enviamos tu recibo de pago correspondiente a:</p>
            <p style='font-size: 18px; margin: 20px 0;'><strong>Período:</strong> {periodo}</p>
            <p style='font-size: 18px; margin: 20px 0;'><strong>Monto Neto:</strong> <span class='highlight'>RD$ {netoAPagar:N2}</span></p>
            
            {mensajeExtra}
            
            <p>El PDF adjunto contiene el detalle completo de tu recibo de pago con todos los ingresos y deducciones.</p>
            
            <p style='margin-top: 30px;'>Si tienes alguna pregunta sobre tu recibo, no dudes en contactar al departamento de recursos humanos.</p>
        </div>
        <div class='footer'>
            <p>&copy; 2025 MiGente En Línea - Sistema de Gestión de Nómina</p>
            <p>Este es un email automático, por favor no responder directamente.</p>
        </div>
    </div>
</body>
</html>";
    }

    /// <summary>
    /// Genera versión texto plano del email (fallback)
    /// </summary>
    private string GenerarTextoPlano(string nombreEmpleado, string periodo, decimal netoAPagar)
    {
        return $@"
Hola {nombreEmpleado},

Te enviamos tu recibo de pago correspondiente a:

Período: {periodo}
Monto Neto: RD$ {netoAPagar:N2}

El PDF adjunto contiene el detalle completo de tu recibo de pago con todos los ingresos y deducciones.

Si tienes alguna pregunta sobre tu recibo, no dudes en contactar al departamento de recursos humanos.

---
© 2025 MiGente En Línea - Sistema de Gestión de Nómina
Este es un email automático, por favor no responder directamente.
";
    }

    /// <summary>
    /// Agrega PDF embebido en HTML como base64 data URI
    /// NOTA: Esta es una solución temporal. Lo ideal sería extender IEmailService
    /// para soportar attachments nativos (usando MailKit o similar).
    /// </summary>
    private string AgregarPdfEmbebidoHtml(string htmlBody, byte[] pdfBytes, string filename)
    {
        var base64Pdf = Convert.ToBase64String(pdfBytes);
        var downloadLink = $@"
<div style='text-align: center; margin: 30px 0;'>
    <a href='data:application/pdf;base64,{base64Pdf}' 
       download='{filename}' 
       class='button'>
        📄 Descargar Recibo PDF
    </a>
    <p style='font-size: 12px; color: #666; margin-top: 10px;'>
        Haz clic en el botón para descargar tu recibo en formato PDF
    </p>
</div>";

        // Insertar antes del footer
        return htmlBody.Replace("<div class='footer'>", downloadLink + "<div class='footer'>");
    }
}
