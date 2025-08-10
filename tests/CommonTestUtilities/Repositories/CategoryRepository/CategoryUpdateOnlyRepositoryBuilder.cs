using InventarioEscolar.Domain.Interfaces.Repositories.Categories;
using NSubstitute;

namespace CommonTestUtilities.Repositories.CategoryRepository
{
    public class CategoryUpdateOnlyRepositoryBuilder
    {
        private readonly ICategoryUpdateOnlyRepository _repository;
        public CategoryUpdateOnlyRepositoryBuilder()
        {
            _repository = Substitute.For<ICategoryUpdateOnlyRepository>();
        }
        public ICategoryUpdateOnlyRepository Build() => _repository;
    }
}