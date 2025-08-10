using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.Categories;
using InventarioEscolar.Domain.Pagination;
using NSubstitute;

namespace CommonTestUtilities.Repositories.CategoryRepository
{
    public class CategoryReadOnlyRepositoryBuilder
    {
        private readonly ICategoryReadOnlyRepository _repository;
        public CategoryReadOnlyRepositoryBuilder()
        {
            _repository = Substitute.For<ICategoryReadOnlyRepository>();
        }
        public CategoryReadOnlyRepositoryBuilder WithCategoryExist(long id, Category category)
        {
            _repository.GetById(id).Returns(category);
            return this;
        }
        public CategoryReadOnlyRepositoryBuilder WithCategoryNotExist(long id)
        {
            _repository.GetById(id).Returns((Category)null!);
            return this;
        }
        public CategoryReadOnlyRepositoryBuilder WithCategoriesExist(IEnumerable<Category> categories, int page = 1, int pageSize = 10)
        {
            var categoryList = categories.ToList();
            var skip = (page - 1) * pageSize;
            var pagedItems = categoryList.Skip(skip).Take(pageSize).ToList();

            var pagedResult = new PagedResult<Category>(
                pagedItems,
                categoryList.Count,
                page,
                pageSize
            );

            _repository.GetAll(page, pageSize).Returns(pagedResult);

            return this;
        }
        public CategoryReadOnlyRepositoryBuilder WithGetAllReturningNull(int page, int pageSize)
        {
            _repository.GetAll(page, pageSize)!.Returns((PagedResult<Category>?)null);
            return this;
        }
        public CategoryReadOnlyRepositoryBuilder WithCategoryNameNotExists(string name, long schoolId)
        {
            _repository.ExistCategoryName(name, schoolId).Returns(false);
            return this;
        }
        public CategoryReadOnlyRepositoryBuilder WithCategoryNameExists(string name, long schoolId)
        {
            _repository.ExistCategoryName(name, schoolId).Returns(true);
            return this;
        }
        public ICategoryReadOnlyRepository Build() => _repository;
    }
}
