using Bogus;
using InventarioEscolar.Communication.Dtos;

namespace CommonTestUtilities.Dtos
{
    public static class UpdateCategoryDtoBuilder
    {
        public static UpdateCategoryDto Build()
        {
            var faker = new Faker<UpdateCategoryDto>("pt_BR")
                .RuleFor(x => x.Name, f => f.Commerce.Categories(1)[0])
                .RuleFor(x => x.Description, f => f.Commerce.ProductDescription());

            return faker.Generate();
        }
    }
}