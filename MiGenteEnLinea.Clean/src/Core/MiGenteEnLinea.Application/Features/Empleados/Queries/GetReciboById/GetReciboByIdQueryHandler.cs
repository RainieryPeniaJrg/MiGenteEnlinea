using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Application.Common.Exceptions;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Empleados.DTOs;
using MiGenteEnLinea.Domain.Entities.Nominas;

namespace MiGenteEnLinea.Application.Features.Empleados.Queries.GetReciboById;

public class GetReciboByIdQueryHandler : IRequestHandler<GetReciboByIdQuery, ReciboDetalleDto>
{
    private readonly IApplicationDbContext _context;

    public GetReciboByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ReciboDetalleDto> Handle(GetReciboByIdQuery request, CancellationToken cancellationToken)
    {
        // PASO 1: Buscar recibo con join manual a Empleado (no hay relación de navegación configurada)
        // ReciboHeader no tiene propiedad Empleado, hacemos join explícito
        var recibo = await (
            from r in _context.RecibosHeader.AsNoTracking()
            join e in _context.Empleados.AsNoTracking() on r.EmpleadoId equals e.EmpleadoId
            where r.PagoId == request.PagoId && r.UserId == request.UserId
            select new
            {
                Header = r,
                EmpleadoNombre = e.Nombre + " " + e.Apellido
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (recibo == null)
            throw new NotFoundException(nameof(ReciboHeader), request.PagoId);

        // PASO 2: Obtener detalles del recibo
        var detalles = await _context.RecibosDetalle
            .AsNoTracking()
            .Where(d => d.PagoId == request.PagoId)
            .ToListAsync(cancellationToken);

        // PASO 3: Construir DTO
        return new ReciboDetalleDto
        {
            // Header
            PagoId = recibo.Header.PagoId,
            EmpleadoId = recibo.Header.EmpleadoId,
            EmpleadoNombre = recibo.EmpleadoNombre,
            UserId = recibo.Header.UserId,
            FechaPago = recibo.Header.FechaPago,
            FechaRegistro = recibo.Header.FechaRegistro,
            Comentarios = null, // ReciboHeader no tiene Comentarios en el dominio actual
            Estado = recibo.Header.Estado,
            MotivoAnulacion = null, // Verificar si existe en el dominio

            // Totales
            TotalPercepciones = recibo.Header.TotalIngresos,
            TotalDeducciones = recibo.Header.TotalDeducciones,
            NetoPagar = recibo.Header.NetoPagar,

            // Detalles - Percepciones (TipoConcepto = 1 o Monto > 0)
            Percepciones = detalles
                .Where(d => d.TipoConcepto == 1 || d.Monto > 0)
                .Select(d => new ReciboLineaDto
                {
                    DetalleId = d.DetalleId,
                    Descripcion = d.Concepto,
                    Monto = d.Monto
                })
                .ToList(),

            // Detalles - Deducciones (TipoConcepto = 2 o Monto < 0)
            Deducciones = detalles
                .Where(d => d.TipoConcepto == 2 || d.Monto < 0)
                .Select(d => new ReciboLineaDto
                {
                    DetalleId = d.DetalleId,
                    Descripcion = d.Concepto,
                    Monto = d.Monto
                })
                .ToList()
        };
    }
}
