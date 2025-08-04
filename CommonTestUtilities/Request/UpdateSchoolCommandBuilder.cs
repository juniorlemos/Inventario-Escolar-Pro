using Bogus;
using CommonTestUtilities.Dtos;
using InventarioEscolar.Application.UsesCases.CategoryCase.Update;
using InventarioEscolar.Application.UsesCases.SchoolCase.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTestUtilities.Request
{
    public static class UpdateSchoolCommandBuilder
    {
        public static UpdateSchoolCommand Build()
        {
            var faker = new Faker();

            var id = faker.Random.Long(1, 1000);
            var dto = UpdateSchoolDtoBuilder.Build();

            return new UpdateSchoolCommand(id, dto);
        }
    }
}
