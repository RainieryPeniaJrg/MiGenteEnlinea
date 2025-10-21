using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Configuracion.Queries.GetOpenAiConfig;

/// <summary>
/// Handler para GetOpenAiConfigQuery.
/// Migrado desde: BotServices.getOpenAI() - línea 11
/// </summary>
public class GetOpenAiConfigQueryHandler : IRequestHandler<GetOpenAiConfigQuery, OpenAiConfigDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<GetOpenAiConfigQueryHandler> _logger;

    public GetOpenAiConfigQueryHandler(
        IApplicationDbContext context,
        ILogger<GetOpenAiConfigQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<OpenAiConfigDto?> Handle(GetOpenAiConfigQuery request, CancellationToken cancellationToken)
    {
        _logger.LogWarning(
            "⚠️ SECURITY WARNING: GetOpenAiConfig endpoint called. " +
            "Este endpoint expone API keys y debe ser reemplazado por configuración desde Backend.");

        try
        {
            // PASO 1: Buscar configuración en tabla OpenAi_Config
            var config = await _context.OpenAiConfigs
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (config == null)
            {
                _logger.LogWarning("No se encontró configuración de OpenAI en la base de datos");
                return null;
            }

            // PASO 2: Mapear a DTO (paridad con Legacy)
            var dto = new OpenAiConfigDto
            {
                ConfigId = config.Id,
                ApiKey = config.OpenAIApiKey,
                ApiUrl = config.OpenAIApiUrl
            };

            _logger.LogInformation("Configuración OpenAI obtenida: ApiUrl={ApiUrl}", config.OpenAIApiUrl);

            return dto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener configuración de OpenAI");
            throw;
        }
    }
}
