using MiGenteEnLinea.Application.Features.Empleados.Commands.CreateRemuneraciones;
using MiGenteEnLinea.Application.Features.Empleados.DTOs;

namespace MiGenteEnLinea.Application.Common.Interfaces;

/// <summary>
/// Servicio para acceder a tablas Legacy que no están migradas a DDD
/// Evita dependencia circular entre Application e Infrastructure
/// </summary>
public interface ILegacyDataService
{
    /// <summary>
    /// Obtiene remuneraciones de la tabla Remuneraciones por userId y empleadoId
    /// </summary>
    Task<List<RemuneracionDto>> GetRemuneracionesAsync(string userId, int empleadoId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Elimina una remuneración de la tabla Remuneraciones
    /// </summary>
    Task DeleteRemuneracionAsync(string userId, int remuneracionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Crea múltiples remuneraciones en batch
    /// Migrado de: EmpleadosService.guardarOtrasRemuneraciones
    /// </summary>
    Task CreateRemuneracionesAsync(string userId, int empleadoId, List<RemuneracionItemDto> remuneraciones, CancellationToken cancellationToken = default);

    /// <summary>
    /// Actualiza remuneraciones (elimina existentes y crea nuevas)
    /// Migrado de: EmpleadosService.actualizarRemuneraciones
    /// </summary>
    Task UpdateRemuneracionesAsync(string userId, int empleadoId, List<RemuneracionItemDto> remuneraciones, CancellationToken cancellationToken = default);
}
