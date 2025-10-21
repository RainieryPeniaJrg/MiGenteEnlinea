using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Domain.Entities.Contrataciones;
using MiGenteEnLinea.Domain.Interfaces;
using MiGenteEnLinea.Domain.Interfaces.Repositories;

namespace MiGenteEnLinea.Application.Features.Contrataciones.Commands.CreateContratacion;

/// <summary>
/// Handler para crear una nueva propuesta de contratación.
/// 
/// LÓGICA DE NEGOCIO:
/// 1. Validar datos de entrada (FluentValidation se ejecuta antes)
/// 2. Crear entidad DetalleContratacion usando factory method
/// 3. Guardar en base de datos vía UnitOfWork
/// 4. Domain Event ContratacionCreadaEvent se dispara automáticamente
/// 5. Retornar ID del detalle creado
/// </summary>
public class CreateContratacionCommandHandler : IRequestHandler<CreateContratacionCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateContratacionCommandHandler> _logger;

    public CreateContratacionCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<CreateContratacionCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<int> Handle(CreateContratacionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Creating new contratacion: {DescripcionCorta}, Amount: {MontoAcordado}, Start: {FechaInicio}",
            request.DescripcionCorta,
            request.MontoAcordado,
            request.FechaInicio);

        try
        {
            // Crear entidad usando factory method del Domain
            var contratacion = DetalleContratacion.Crear(
                descripcionCorta: request.DescripcionCorta,
                fechaInicio: request.FechaInicio,
                fechaFinal: request.FechaFinal,
                montoAcordado: request.MontoAcordado,
                descripcionAmpliada: request.DescripcionAmpliada,
                esquemaPagos: request.EsquemaPagos,
                contratacionId: request.ContratacionId
            );

            // Agregar notas si se especificaron
            if (!string.IsNullOrWhiteSpace(request.Notas))
            {
                contratacion.ActualizarNotas(request.Notas);
            }

            // Guardar en base de datos
            await _unitOfWork.DetallesContrataciones.AddAsync(contratacion, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Contratacion created successfully with ID: {DetalleId}",
                contratacion.DetalleId);

            return contratacion.DetalleId;
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validation error creating contratacion: {Message}", ex.Message);
            throw; // FluentValidation debería haber capturado esto, pero por si acaso
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating contratacion");
            throw;
        }
    }
}
