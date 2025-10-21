using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Domain.Entities.Contratistas;
using MiGenteEnLinea.Domain.Interfaces.Repositories;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Contratistas;

namespace MiGenteEnLinea.Application.Features.Contratistas.Commands.CreateContratista;

/// <summary>
/// Handler: Crea un nuevo perfil de contratista
/// </summary>
public class CreateContratistaCommandHandler : IRequestHandler<CreateContratistaCommand, int>
{
    private readonly IApplicationDbContext _context; // Temporary for Credenciales validation
    private readonly IContratistaRepository _contratistaRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateContratistaCommandHandler> _logger;

    public CreateContratistaCommandHandler(
        IApplicationDbContext context,
        IContratistaRepository contratistaRepository,
        IUnitOfWork unitOfWork,
        ILogger<CreateContratistaCommandHandler> logger)
    {
        _context = context;
        _contratistaRepository = contratistaRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<int> Handle(CreateContratistaCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creando perfil de contratista para userId: {UserId}", request.UserId);

        // 1. VALIDAR: UserId existe en Credenciales
        var credencialExiste = await _context.Credenciales
            .AnyAsync(c => c.UserId == request.UserId, cancellationToken);

        if (!credencialExiste)
        {
            _logger.LogWarning("UserId {UserId} no existe en Credenciales", request.UserId);
            throw new InvalidOperationException($"El usuario con ID {request.UserId} no existe");
        }

        // 2. VALIDAR: No existe otro contratista con el mismo userId (relación 1:1)
        var contratistaExistente = await _contratistaRepository
            .ExistsByUserIdAsync(request.UserId, cancellationToken);

        if (contratistaExistente)
        {
            _logger.LogWarning("Ya existe un contratista con userId: {UserId}", request.UserId);
            throw new InvalidOperationException($"Ya existe un perfil de contratista para el usuario {request.UserId}");
        }

        // 3. CREAR CONTRATISTA usando Factory Method de dominio
        var contratista = Contratista.Create(
            userId: request.UserId,
            nombre: request.Nombre,
            apellido: request.Apellido,
            tipo: request.Tipo,
            titulo: request.Titulo,
            identificacion: request.Identificacion,
            sector: request.Sector,
            experiencia: request.Experiencia,
            presentacion: request.Presentacion,
            telefono1: request.Telefono1,
            whatsapp1: request.Whatsapp1,
            provincia: request.Provincia
        );

        // 4. AGREGAR usando Repository
        await _contratistaRepository.AddAsync(contratista, cancellationToken);

        // 5. GUARDAR CAMBIOS usando UnitOfWork
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Contratista creado exitosamente. ContratistaId: {ContratistaId}, UserId: {UserId}",
            contratista.Id, request.UserId);

        return contratista.Id;
    }
}
