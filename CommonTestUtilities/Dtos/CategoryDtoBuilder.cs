using Bogus;
using InventarioEscolar.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTestUtilities.Dtos
{
    public static class CategoryDtoBuilder
    {
        public static CategoryDto Build()
        {
            var faker = new Faker("pt_BR");

            return new CategoryDto
            {
                Id = faker.Random.Long(1, 1000),
                Name = faker.Commerce.Categories(1)[0],
                Description = faker.Lorem.Sentence(5),
                SchoolId = faker.Random.Long(1, 1000)
            };
        }
    }
}
