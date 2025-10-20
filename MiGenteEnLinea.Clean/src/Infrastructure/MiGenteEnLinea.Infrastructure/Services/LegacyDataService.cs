using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Empleados.Commands.CreateRemuneraciones;
using MiGenteEnLinea.Application.Features.Empleados.Commands.CreateEmpleadoTemporal;
using MiGenteEnLinea.Application.Features.Empleados.DTOs;
using MiGenteEnLinea.Infrastructure.Persistence.Contexts;
using MiGenteEnLinea.Infrastructure.Persistence.Entities.Generated;
using System.Text;

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

    public async Task CreateRemuneracionesAsync(
        string userId,
        int empleadoId,
        List<RemuneracionItemDto> remuneraciones,
        CancellationToken cancellationToken = default)
    {
        // Legacy: db.Remuneraciones.AddRange(rem); db.SaveChanges();
        // Construir INSERT batch usando StringBuilder
        var sqlBuilder = new StringBuilder();
        var parameters = new List<object>();
        int paramIndex = 0;

        foreach (var rem in remuneraciones)
        {
            if (sqlBuilder.Length > 0)
                sqlBuilder.Append(";");

            sqlBuilder.Append($"INSERT INTO Remuneraciones (userID, empleadoID, descripcion, monto) " +
                            $"VALUES ({{{paramIndex}}}, {{{paramIndex + 1}}}, {{{paramIndex + 2}}}, {{{paramIndex + 3}}})");

            parameters.Add(userId);
            parameters.Add(empleadoId);
            parameters.Add(rem.Descripcion);
            parameters.Add(rem.Monto);

            paramIndex += 4;
        }

        if (sqlBuilder.Length > 0)
        {
            await _context.Database.ExecuteSqlRawAsync(
                sqlBuilder.ToString(),
                parameters.ToArray(),
                cancellationToken);
        }
    }

    public async Task UpdateRemuneracionesAsync(
        string userId,
        int empleadoId,
        List<RemuneracionItemDto> remuneraciones,
        CancellationToken cancellationToken = default)
    {
        // Legacy: DELETE existing, then INSERT new
        // Step 1: Delete existing remuneraciones for this empleadoId
        await _context.Database.ExecuteSqlRawAsync(
            "DELETE FROM Remuneraciones WHERE userID = {0} AND empleadoID = {1}",
            [userId, empleadoId],
            cancellationToken);

        // Step 2: Insert new remuneraciones
        await CreateRemuneracionesAsync(userId, empleadoId, remuneraciones, cancellationToken);
    }

    public async Task<List<DeduccionTssDto>> GetDeduccionesTssAsync(CancellationToken cancellationToken = default)
    {
        // Legacy: return db.Deducciones_TSS.ToList();
        return await _context.Database
            .SqlQueryRaw<DeduccionTssDto>(
                "SELECT id AS Id, descripcion AS Descripcion, porcentaje AS Porcentaje " +
                "FROM Deducciones_TSS")
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> DarDeBajaEmpleadoAsync(
        int empleadoId,
        string userId,
        DateTime fechaBaja,
        decimal prestaciones,
        string motivo,
        CancellationToken cancellationToken = default)
    {
        // Legacy: 
        // empleado.Activo = false;
        // empleado.fechaSalida = fechaBaja.Date;
        // empleado.motivoBaja = motivo;
        // empleado.prestaciones = prestaciones;
        // db.SaveChanges();

        await _context.Database.ExecuteSqlRawAsync(
            "UPDATE Empleados SET Activo = 0, fechaSalida = {0}, motivoBaja = {1}, prestaciones = {2} " +
            "WHERE empleadoID = {3} AND userID = {4}",
            [fechaBaja.Date, motivo, prestaciones, empleadoId, userId],
            cancellationToken);

        return true;
    }

    public async Task<bool> CancelarTrabajoAsync(
        int contratacionId,
        int detalleId,
        CancellationToken cancellationToken = default)
    {
        // Legacy:
        // detalle.estatus = 3;
        // db.SaveChanges();

        await _context.Database.ExecuteSqlRawAsync(
            "UPDATE DetalleContrataciones SET estatus = 3 " +
            "WHERE contratacionID = {0} AND detalleID = {1}",
            [contratacionId, detalleId],
            cancellationToken);

        return true;
    }

    public async Task<bool> EliminarReciboEmpleadoAsync(
        int pagoId,
        CancellationToken cancellationToken = default)
    {
        // Legacy uses 2 separate DbContexts - we'll use 2 separate SQL commands
        // Step 1: Delete details
        await _context.Database.ExecuteSqlRawAsync(
            "DELETE FROM Empleador_Recibos_Detalle WHERE pagoID = {0}",
            [pagoId],
            cancellationToken);

        // Step 2: Delete header
        await _context.Database.ExecuteSqlRawAsync(
            "DELETE FROM Empleador_Recibos_Header WHERE pagoID = {0}",
            [pagoId],
            cancellationToken);

        return true;
    }

    public async Task<bool> EliminarReciboContratacionAsync(
        int pagoId,
        CancellationToken cancellationToken = default)
    {
        // Legacy uses 2 separate DbContexts - same pattern for contrataciones
        // Step 1: Delete details
        await _context.Database.ExecuteSqlRawAsync(
            "DELETE FROM Empleador_Recibos_Detalle_Contrataciones WHERE pagoID = {0}",
            [pagoId],
            cancellationToken);

        // Step 2: Delete header
        await _context.Database.ExecuteSqlRawAsync(
            "DELETE FROM Empleador_Recibos_Header_Contrataciones WHERE pagoID = {0}",
            [pagoId],
            cancellationToken);

        return true;
    }

    public async Task<ReciboContratacionDto?> GetReciboContratacionAsync(
        int pagoId,
        CancellationToken cancellationToken = default)
    {
        // Query identical to Legacy:
        // db.Empleador_Recibos_Header_Contrataciones.Where(x => x.pagoID == pagoID)
        //   .Include(h => h.Empleador_Recibos_Detalle_Contrataciones)
        //   .Include(f => f.EmpleadosTemporales).FirstOrDefault();

        var headerEntity = await _context
            .Set<EmpleadorRecibosHeaderContratacione>()
            .Where(x => x.PagoId == pagoId)
            .Include(h => h.EmpleadorRecibosDetalleContrataciones)
            .Include(f => f.Contratacion) // EmpleadoTemporal
            .FirstOrDefaultAsync(cancellationToken);

        if (headerEntity == null)
        {
            return null;
        }

        // Map to DTO
        var dto = new ReciboContratacionDto
        {
            PagoId = headerEntity.PagoId,
            UserId = headerEntity.UserId,
            ContratacionId = headerEntity.ContratacionId,
            FechaRegistro = headerEntity.FechaRegistro,
            FechaPago = headerEntity.FechaPago,
            ConceptoPago = headerEntity.ConceptoPago,
            Tipo = headerEntity.Tipo,
            Detalles = headerEntity.EmpleadorRecibosDetalleContrataciones
                .Select(d => new ReciboContratacionDetalleDto
                {
                    DetalleId = d.DetalleId,
                    PagoId = d.PagoId,
                    Concepto = d.Concepto,
                    Monto = d.Monto
                })
                .ToList()
        };

        // Map EmpleadoTemporal if exists
        if (headerEntity.Contratacion != null)
        {
            var emp = headerEntity.Contratacion;
            dto.EmpleadoTemporal = new EmpleadoTemporalSimpleDto
            {
                ContratacionId = emp.ContratacionId,
                Nombre = emp.Nombre,
                Apellido = emp.Apellido,
                Cedula = emp.Identificacion // In Legacy, "identificacion" is the cedula field
            };
        }

        return dto;
    }

    public async Task<bool> EliminarEmpleadoTemporalAsync(
        int contratacionId,
        CancellationToken cancellationToken = default)
    {
        // Legacy: Complex cascade delete using multiple DbContexts
        // 1. Get EmpleadoTemporal with receipts
        // 2. For each receipt: delete Detalle → Header
        // 3. Delete EmpleadoTemporal

        // Step 1: Get all receipt IDs for this empleadoTemporal
        var reciboIds = await _context
            .Set<EmpleadorRecibosHeaderContratacione>()
            .Where(r => r.ContratacionId == contratacionId)
            .Select(r => r.PagoId)
            .ToListAsync(cancellationToken);

        // Step 2: For each receipt, delete Detalle → Header
        foreach (var pagoId in reciboIds)
        {
            // Delete details first
            await _context.Database.ExecuteSqlRawAsync(
                "DELETE FROM Empleador_Recibos_Detalle_Contrataciones WHERE pagoID = {0}",
                [pagoId],
                cancellationToken);

            // Then delete header
            await _context.Database.ExecuteSqlRawAsync(
                "DELETE FROM Empleador_Recibos_Header_Contrataciones WHERE pagoID = {0}",
                [pagoId],
                cancellationToken);
        }

        // Step 3: Delete EmpleadoTemporal
        await _context.Database.ExecuteSqlRawAsync(
            "DELETE FROM EmpleadosTemporales WHERE contratacionID = {0}",
            [contratacionId],
            cancellationToken);

        return true;
    }

    public async Task<List<PagoContratacionDto>> GetPagosContratacionesAsync(
        int contratacionId,
        int detalleId,
        CancellationToken cancellationToken = default)
    {
        // Legacy: SELECT from VPagosContrataciones view with filters
        var result = await _context
            .Set<VpagosContratacione>()
            .Where(x => x.ContratacionId == contratacionId && x.DetalleId == detalleId)
            .Select(x => new PagoContratacionDto
            {
                PagoId = x.PagoId,
                UserId = x.UserId,
                FechaRegistro = x.FechaRegistro,
                FechaPago = x.FechaPago,
                Expr1 = x.Expr1,
                Monto = x.Monto,
                ContratacionId = x.ContratacionId,
                DetalleId = x.DetalleId
            })
            .ToListAsync(cancellationToken);

        return result;
    }

    public async Task<int> CreateEmpleadoTemporalAsync(
        CreateEmpleadoTemporalCommand command,
        CancellationToken cancellationToken = default)
    {
        // Legacy: Uses 2 separate DbContexts (2 transactions)
        // Step 1: Create EmpleadoTemporal
        var empleadoTemporal = new EmpleadosTemporale
        {
            UserId = command.UserId,
            FechaRegistro = DateTime.Now,
            Tipo = command.Tipo,
            NombreComercial = command.NombreComercial,
            Rnc = command.Rnc,
            Nombre = command.Nombre,
            Apellido = command.Apellido,
            Identificacion = command.Identificacion,
            Telefono1 = command.Telefono,
            Direccion = command.Direccion
        };

        _context.Set<EmpleadosTemporale>().Add(empleadoTemporal);
        await _context.SaveChangesAsync(cancellationToken);

        int contratacionId = empleadoTemporal.ContratacionId;

        // Step 2: Create DetalleContrataciones (with the generated contratacionId)
        // Map Command properties to entity properties
        var detalle = new DetalleContratacione
        {
            ContratacionId = contratacionId,
            DescripcionCorta = command.Servicio, // "Servicio" maps to "DescripcionCorta"
            FechaInicio = command.FechaInicio.HasValue ? DateOnly.FromDateTime(command.FechaInicio.Value) : null,
            FechaFinal = command.FechaFin.HasValue ? DateOnly.FromDateTime(command.FechaFin.Value) : null,
            MontoAcordado = command.Pago,
            DescripcionAmpliada = command.LugarTrabajo, // Assuming LugarTrabajo maps to DescripcionAmpliada
            EsquemaPagos = command.HorarioTrabajo, // Assuming HorarioTrabajo maps to EsquemaPagos
            Estatus = command.Estatus ?? 1 // Default to 1 (active)
        };

        _context.Set<DetalleContratacione>().Add(detalle);
        await _context.SaveChangesAsync(cancellationToken);

        return contratacionId;
    }
}

