using InventarioEscolar.Domain.Interfaces.Repositories.Categories;
using NSubstitute;

namespace CommonTestUtilities.Repositories.CategoryRepository
{
    public class CategoryWriteOnlyRepositoryBuilder
    {
        private readonly ICategoryWriteOnlyRepository _repository;
        public CategoryWriteOnlyRepositoryBuilder()
        {
            _repository = Substitute.For<ICategoryWriteOnlyRepository>();
        }
        public ICategoryWriteOnlyRepository Build() => _repository;
    }
}