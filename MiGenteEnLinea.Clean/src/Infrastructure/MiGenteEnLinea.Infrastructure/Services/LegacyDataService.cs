using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Empleados.DTOs;
using MiGenteEnLinea.Infrastructure.Persistence.Contexts;

namespace MiGenteEnLinea.Infrastructure.Services;

/// <summary>
/// Implementación de ILegacyDataService usando raw SQL
/// Accede a tablas Legacy sin necesidad de entidades DDD completas
/// </summary>
public class LegacyDataService : ILegacyDataService
{
    private readonly MiGenteDbContext _context;

    public LegacyDataService(MiGenteDbContext context)
    {
        _context = context;
    }

    public async Task<List<RemuneracionDto>> GetRemuneracionesAsync(
        string userId,
        int empleadoId,
        CancellationToken cancellationToken = default)
    {
        // Legacy: return db.Remuneraciones.Where(x => x.userID == userID && x.empleadoID == empleadoID).ToList();
        return await _context.Database
            .SqlQueryRaw<RemuneracionDto>(
                "SELECT id AS Id, userID AS UserId, empleadoID AS EmpleadoId, " +
                "descripcion AS Descripcion, monto AS Monto " +
                "FROM Remuneraciones WHERE userID = {0} AND empleadoID = {1}",
                userId, empleadoId)
            .ToListAsync(cancellationToken);
    }

    public async Task DeleteRemuneracionAsync(
        string userId,
        int remuneracionId,
        CancellationToken cancellationToken = default)
    {
        // Legacy: db.Remuneraciones.Remove(toDelete); db.SaveChanges();
        await _context.Database.ExecuteSqlRawAsync(
            "DELETE FROM Remuneraciones WHERE userID = {0} AND id = {1}",
            [userId, remuneracionId],
            cancellationToken);
    }
}
