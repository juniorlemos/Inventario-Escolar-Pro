using InventarioEscolar.Domain.Repositories;

namespace InventarioEscolar.Infrastructure.DataAccess
{

    public class UnitOfWork(InventarioEscolarProDBContext dbContext) : IUnitOfWork
    {
        public async Task Commit() => await dbContext.SaveChangesAsync();
    }
}
