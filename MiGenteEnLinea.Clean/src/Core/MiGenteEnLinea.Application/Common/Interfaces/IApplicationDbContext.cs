using MiGenteEnLinea.Domain.ReadModels;
using Microsoft.EntityFrameworkCore;

namespace MiGenteEnLinea.Application.Common.Interfaces;

/// <summary>
/// Interfaz para el DbContext de la aplicación
/// </summary>
/// <remarks>
/// Permite a Application Layer acceder a entidades sin depender de Infrastructure
/// </remarks>
public interface IApplicationDbContext
{
    // Entidades de dominio (Write Models)
    DbSet<Domain.Entities.Authentication.Credencial> Credenciales { get; }
    DbSet<Domain.Entities.Suscripciones.Suscripcion> Suscripciones { get; }
    DbSet<Domain.Entities.Suscripciones.PlanEmpleador> PlanesEmpleadores { get; }
    DbSet<Domain.Entities.Seguridad.Perfile> Perfiles { get; }
    DbSet<Domain.Entities.Contratistas.Contratista> Contratistas { get; }
    DbSet<Domain.Entities.Contratistas.ContratistaServicio> ContratistasServicios { get; }
    DbSet<Domain.Entities.Empleadores.Empleador> Empleadores { get; }
    
    // Read Models (Views)
    DbSet<VistaPerfil> VPerfiles { get; }
    
    // EF Core methods
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
