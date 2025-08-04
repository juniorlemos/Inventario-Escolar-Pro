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
    public class SchoolWriteOnlyRepositoryBuilder
    {
        private readonly ISchoolWriteOnlyRepository _repository;
        public SchoolWriteOnlyRepositoryBuilder()
        {
            _repository = Substitute.For<ISchoolWriteOnlyRepository>();
        }
        public ISchoolWriteOnlyRepository Build() => _repository;
    }
}
