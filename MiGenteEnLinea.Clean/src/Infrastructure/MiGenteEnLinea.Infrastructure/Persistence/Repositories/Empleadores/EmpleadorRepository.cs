using MiGenteEnLinea.Domain.Entities.Empleadores;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Empleadores;
using MiGenteEnLinea.Infrastructure.Persistence.Contexts;

namespace MiGenteEnLinea.Infrastructure.Persistence.Repositories.Empleadores;

public class EmpleadorRepository : Repository<Empleador>, IEmpleadorRepository
{
    public EmpleadorRepository(MiGenteDbContext context) : base(context) { }
}
