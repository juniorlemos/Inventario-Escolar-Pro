using Bogus;
using InventarioEscolar.Communication.Dtos;

namespace CommonTestUtilities.Dtos
{
    public static class UpdateSchoolDtoBuilder
    {
        public static UpdateSchoolDto Build()
        {
            var faker = new Faker<UpdateSchoolDto>("pt_BR")
                .RuleFor(x => x.Name, f => f.Commerce.ProductName())
                .RuleFor(x => x.Inep, f => f.Random.String2(0, 20))
                .RuleFor(x => x.Address, f => f.Address.FullAddress())
                .RuleFor(x => x.City, f => f.Address.City());

            return faker.Generate();
        }
    }
}