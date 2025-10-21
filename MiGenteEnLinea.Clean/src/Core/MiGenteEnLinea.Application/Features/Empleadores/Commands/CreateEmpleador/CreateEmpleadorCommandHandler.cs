using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Domain.Entities.Empleadores;
using MiGenteEnLinea.Domain.Interfaces.Repositories;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Empleadores;

namespace MiGenteEnLinea.Application.Features.Empleadores.Commands.CreateEmpleador;

/// <summary>
/// Handler: Procesa la creación de un nuevo Empleador
/// </summary>
public sealed class CreateEmpleadorCommandHandler : IRequestHandler<CreateEmpleadorCommand, int>
{
    private readonly IApplicationDbContext _context; // Temporary for Credenciales validation
    private readonly IEmpleadorRepository _empleadorRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateEmpleadorCommandHandler> _logger;

    public CreateEmpleadorCommandHandler(
        IApplicationDbContext context,
        IEmpleadorRepository empleadorRepository,
        IUnitOfWork unitOfWork,
        ILogger<CreateEmpleadorCommandHandler> logger)
    {
        _context = context;
        _empleadorRepository = empleadorRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Maneja la creación del empleador
    /// </summary>
    /// <exception cref="InvalidOperationException">Si userId no existe o ya tiene empleador</exception>
    public async Task<int> Handle(CreateEmpleadorCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creando empleador para userId: {UserId}", request.UserId);

        // ============================================
        // PASO 1: Validar que userId existe en Credenciales
        // ============================================
        var credencialExists = await _context.Credenciales
            .AnyAsync(c => c.UserId == request.UserId, cancellationToken);

        if (!credencialExists)
        {
            _logger.LogWarning("UserId {UserId} no encontrado en Credenciales", request.UserId);
            throw new InvalidOperationException($"Usuario {request.UserId} no encontrado");
        }

        // ============================================
        // PASO 2: Validar que NO exista empleador para ese userId
        // ============================================
        // BUSINESS RULE: Un userId puede ser empleador O contratista (relación 1:1)
        var existeEmpleador = await _empleadorRepository.ExistsByUserIdAsync(request.UserId, cancellationToken);

        if (existeEmpleador)
        {
            _logger.LogWarning("Ya existe empleador para userId: {UserId}", request.UserId);
            throw new InvalidOperationException($"Ya existe un empleador para el usuario {request.UserId}");
        }

        // ============================================
        // PASO 3: Crear empleador con factory method de dominio
        // ============================================
        var empleador = Empleador.Create(
            userId: request.UserId,
            habilidades: request.Habilidades,
            experiencia: request.Experiencia,
            descripcion: request.Descripcion
        );

        // ============================================
        // PASO 4: Agregar a repositorio
        // ============================================
        await _empleadorRepository.AddAsync(empleador, cancellationToken);

        // ============================================
        // PASO 5: Guardar cambios
        // ============================================
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Empleador creado exitosamente. EmpleadorId: {EmpleadorId}, UserId: {UserId}",
            empleador.Id, request.UserId);

        return empleador.Id;
    }
}
