using MiGenteEnLinea.Domain.Entities.Calificaciones;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Calificaciones;
using MiGenteEnLinea.Infrastructure.Persistence.Contexts;

namespace MiGenteEnLinea.Infrastructure.Persistence.Repositories.Calificaciones;

public class CalificacionRepository : Repository<Calificacion>, ICalificacionRepository
{
    public CalificacionRepository(MiGenteDbContext context) : base(context) { }
}
