using MiGenteEnLinea.Domain.Entities.Pagos;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Pagos;
using MiGenteEnLinea.Infrastructure.Persistence.Contexts;

namespace MiGenteEnLinea.Infrastructure.Persistence.Repositories.Pagos;

public class VentaRepository : Repository<Venta>, IVentaRepository
{
    public VentaRepository(MiGenteDbContext context) : base(context) { }
}
