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
    public class SchoolUpdateOnlyRepositoryBuilder
    {
        private readonly ISchoolUpdateOnlyRepository _repository;
        public SchoolUpdateOnlyRepositoryBuilder()
        {
            _repository = Substitute.For<ISchoolUpdateOnlyRepository>();
        }

        public ISchoolUpdateOnlyRepository Build() => _repository;
    }
}
