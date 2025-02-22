using InventarioEscolar.Domain.Repositories;

namespace InventarioEscolar.Infrastructure.DataAccess
{

    public class UnitOfWork : IUnitOfWork
    {
        private readonly InventárioEscolarProDBContext _dbContext;

        public UnitOfWork(InventárioEscolarProDBContext dbContext) => _dbContext = dbContext;

        public async Task Commit() => await _dbContext.SaveChangesAsync();
    }
}
