using MiGenteEnLinea.Domain.Entities.Contratistas;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Contratistas;
using MiGenteEnLinea.Infrastructure.Persistence.Contexts;

namespace MiGenteEnLinea.Infrastructure.Persistence.Repositories.Contratistas;

public class ContratistaRepository : Repository<Contratista>, IContratistaRepository
{
    public ContratistaRepository(MiGenteDbContext context) : base(context) { }
}
