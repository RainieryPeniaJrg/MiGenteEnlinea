using MiGenteEnLinea.Domain.Entities.Empleados;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Empleados;
using MiGenteEnLinea.Infrastructure.Persistence.Contexts;

namespace MiGenteEnLinea.Infrastructure.Persistence.Repositories.Empleados;

public class EmpleadoRepository : Repository<Empleado>, IEmpleadoRepository
{
    public EmpleadoRepository(MiGenteDbContext context) : base(context) { }
}
