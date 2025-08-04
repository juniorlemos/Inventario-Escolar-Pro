using CommonTestUtilities.Repositories.CategoryRepository;
using InventarioEscolar.Domain.Interfaces.Repositories.Categories;
using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTestUtilities.Repositories.SchoolRepository
{
    public class SchoolDeleteOnlyRepositoryBuilder
    {
        private readonly ISchoolDeleteOnlyRepository _repository;
        public SchoolDeleteOnlyRepositoryBuilder()
        {
            _repository = Substitute.For<ISchoolDeleteOnlyRepository>();
        }

        public SchoolDeleteOnlyRepositoryBuilder WithDeleteReturningTrue(long schoolId)
        {
            _repository.Delete(schoolId).Returns(true);
            return this;
        }
        public ISchoolDeleteOnlyRepository Build() => _repository;

    }
}

