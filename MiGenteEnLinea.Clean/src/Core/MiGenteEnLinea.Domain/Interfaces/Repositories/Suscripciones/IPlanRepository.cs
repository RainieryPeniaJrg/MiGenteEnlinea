using MiGenteEnLinea.Domain.Entities.Suscripciones;

namespace MiGenteEnLinea.Domain.Interfaces.Repositories.Suscripciones;

/// <summary>
/// Repositorio para entidades de Plan (PlanEmpleador, PlanContratista)
/// NOTA LOTE 0: Plan es clase base abstracta, los repositorios específicos
/// IPlanEmpleadorRepository e IPlanContratistaRepository se crearán en LOTE 2
/// </summary>
public interface IPlanEmpleadorRepository : IRepository<PlanEmpleador>
{
}

public interface IPlanContratistaRepository : IRepository<PlanContratista>
{
}
