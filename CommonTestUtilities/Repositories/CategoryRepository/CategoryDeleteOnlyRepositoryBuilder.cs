using CommonTestUtilities.Repositories.AssetRepository;
using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using InventarioEscolar.Domain.Interfaces.Repositories.Categories;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
