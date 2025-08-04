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
