using MiGenteEnLinea.Domain.ReadModels;
using Microsoft.EntityFrameworkCore;

namespace MiGenteEnLinea.Application.Common.Interfaces;

/// <summary>
/// Interfaz para el DbContext de la aplicación
/// </summary>
/// <remarks>
/// Permite a Application Layer acceder a entidades sin depender de Infrastructure.
/// NOTA: RefreshToken NO está expuesto aquí porque es una entidad de infraestructura
/// (no es parte del dominio de negocio). Se maneja directamente en Infrastructure layer.
/// </remarks>
public interface IApplicationDbContext
{
    // Entidades de dominio (Write Models)
    DbSet<Domain.Entities.Authentication.Credencial> Credenciales { get; }
    DbSet<Domain.Entities.Suscripciones.Suscripcion> Suscripciones { get; }
    DbSet<Domain.Entities.Suscripciones.PlanEmpleador> PlanesEmpleadores { get; }
    DbSet<Domain.Entities.Suscripciones.PlanContratista> PlanesContratistas { get; }
    DbSet<Domain.Entities.Pagos.Venta> Ventas { get; }
    DbSet<Domain.Entities.Seguridad.Perfile> Perfiles { get; }
    DbSet<Domain.Entities.Contratistas.Contratista> Contratistas { get; }
    DbSet<Domain.Entities.Contratistas.ContratistaServicio> ContratistasServicios { get; }
    DbSet<Domain.Entities.Empleadores.Empleador> Empleadores { get; }
    DbSet<Domain.Entities.Empleados.Empleado> Empleados { get; }
    DbSet<Domain.Entities.Nominas.ReciboHeader> RecibosHeader { get; }
    DbSet<Domain.Entities.Nominas.ReciboDetalle> RecibosDetalle { get; }
    DbSet<Domain.Entities.Nominas.DeduccionTss> DeduccionesTss { get; }
    DbSet<Domain.Entities.Calificaciones.Calificacion> Calificaciones { get; }
    DbSet<Domain.Entities.Contrataciones.DetalleContratacion> DetalleContrataciones { get; }
    
    // Read Models (Views)
    DbSet<VistaPerfil> VPerfiles { get; }
    
    // EF Core methods
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}