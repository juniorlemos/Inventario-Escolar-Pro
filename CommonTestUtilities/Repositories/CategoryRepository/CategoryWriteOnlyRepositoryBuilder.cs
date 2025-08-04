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
