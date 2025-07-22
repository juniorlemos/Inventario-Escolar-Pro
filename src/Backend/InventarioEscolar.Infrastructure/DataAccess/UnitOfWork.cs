using InventarioEscolar.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventarioEscolar.Infrastructure.DataAccess
{

    public class UnitOfWork(InventarioEscolarProDBContext dbContext) : IUnitOfWork
    {
        public async Task Commit() => await dbContext.SaveChangesAsync();

        public async Task ExecuteInTransaction(Func<Task> operation)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                await operation();

                await dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw; 
            }
        }
    }
}
