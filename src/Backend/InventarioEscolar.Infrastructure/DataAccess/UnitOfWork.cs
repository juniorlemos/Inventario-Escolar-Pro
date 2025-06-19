using InventarioEscolar.Domain.Repositories;

namespace InventarioEscolar.Infrastructure.DataAccess
{

    public class UnitOfWork : IUnitOfWork
    {
        private readonly InventarioEscolarProDBContext _dbContext;

        public UnitOfWork(InventarioEscolarProDBContext dbContext) => _dbContext = dbContext;

        public async Task Commit() => await _dbContext.SaveChangesAsync();
    }
}
