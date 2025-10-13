using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Domain.Entities.Empleadores;

namespace MiGenteEnLinea.Application.Features.Empleadores.Commands.CreateEmpleador;

/// <summary>
/// Handler: Procesa la creación de un nuevo Empleador
/// </summary>
public sealed class CreateEmpleadorCommandHandler : IRequestHandler<CreateEmpleadorCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<CreateEmpleadorCommandHandler> _logger;

    public CreateEmpleadorCommandHandler(
        IApplicationDbContext context,
        ILogger<CreateEmpleadorCommandHandler> logger)
    {
        _context = context;
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
        var existeEmpleador = await _context.Empleadores
            .AnyAsync(e => e.UserId == request.UserId, cancellationToken);

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
        // PASO 4: Agregar a DbContext
        // ============================================
        _context.Empleadores.Add(empleador);

        // ============================================
        // PASO 5: Guardar cambios
        // ============================================
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Empleador creado exitosamente. EmpleadorId: {EmpleadorId}, UserId: {UserId}",
            empleador.Id, request.UserId);

        return empleador.Id;
    }
}
