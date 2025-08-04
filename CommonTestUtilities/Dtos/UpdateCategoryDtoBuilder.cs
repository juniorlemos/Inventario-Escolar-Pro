using Bogus;
using InventarioEscolar.Communication.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTestUtilities.Dtos
{
    public static class UpdateCategoryDtoBuilder
    {
        public static UploadCategoryDto Build()
        {
            var faker = new Faker<UploadCategoryDto>("pt_BR")
                .RuleFor(x => x.Name, f => f.Commerce.Categories(1)[0])
                .RuleFor(x => x.Description, f => f.Commerce.ProductDescription());

            return faker.Generate();
        }
    }
}
