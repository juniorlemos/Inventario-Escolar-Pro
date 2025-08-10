using InventarioEscolar.Domain.Interfaces.Repositories.Categories;
using NSubstitute;

namespace CommonTestUtilities.Repositories.CategoryRepository
{
    public class CategoryDeleteOnlyRepositoryBuilder
    {
        private readonly ICategoryDeleteOnlyRepository _repository;
        public CategoryDeleteOnlyRepositoryBuilder()
        {
            _repository = Substitute.For<ICategoryDeleteOnlyRepository>();
        }

        public CategoryDeleteOnlyRepositoryBuilder WithDeleteReturningTrue(long categoryId)
        {
            _repository.Delete(categoryId).Returns(true);
            return this;
        }
        public ICategoryDeleteOnlyRepository Build() => _repository;
    }
}