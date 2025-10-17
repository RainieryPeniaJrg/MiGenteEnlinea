using MiGenteEnLinea.Domain.Entities.Suscripciones;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Suscripciones;
using MiGenteEnLinea.Infrastructure.Persistence.Contexts;

namespace MiGenteEnLinea.Infrastructure.Persistence.Repositories.Suscripciones;

public class SuscripcionRepository : Repository<Suscripcion>, ISuscripcionRepository
{
    public SuscripcionRepository(MiGenteDbContext context) : base(context) { }
}
