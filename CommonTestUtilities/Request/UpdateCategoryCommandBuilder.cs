using Bogus;
using CommonTestUtilities.Dtos;
using InventarioEscolar.Application.UsesCases.AssetCase.Update;
using InventarioEscolar.Application.UsesCases.CategoryCase.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTestUtilities.Request
{
    public static class UpdateCategoryCommandBuilder
    {
        public static UpdateCategoryCommand Build()
        {
            var faker = new Faker();

            var id = faker.Random.Long(1, 1000);
            var dto = UpdateCategoryDtoBuilder.Build();

            return new UpdateCategoryCommand(id, dto);
        }
    }
}
