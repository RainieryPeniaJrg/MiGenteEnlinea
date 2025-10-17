using MiGenteEnLinea.Domain.Entities.Suscripciones;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Suscripciones;
using MiGenteEnLinea.Infrastructure.Persistence.Contexts;

namespace MiGenteEnLinea.Infrastructure.Persistence.Repositories.Suscripciones;

/// <summary>
/// LOTE 0: Repositorios para Planes (PlanEmpleador, PlanContratista)
/// </summary>
public class PlanEmpleadorRepository : Repository<PlanEmpleador>, IPlanEmpleadorRepository
{
    public PlanEmpleadorRepository(MiGenteDbContext context) : base(context) { }
}

public class PlanContratistaRepository : Repository<PlanContratista>, IPlanContratistaRepository
{
    public PlanContratistaRepository(MiGenteDbContext context) : base(context) { }
}
