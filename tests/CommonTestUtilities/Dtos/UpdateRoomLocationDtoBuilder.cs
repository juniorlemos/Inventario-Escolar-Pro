using Bogus;
using InventarioEscolar.Communication.Dtos;

namespace CommonTestUtilities.Dtos
{
    public static class UpdateRoomLocationDtoBuilder
    {
        public static UpdateRoomLocationDto Build()
        {
            var faker = new Faker<UpdateRoomLocationDto>("pt_BR")
                .RuleFor(x => x.Name, f => f.Address.SecondaryAddress())
                .RuleFor(x => x.Description, f => f.Lorem.Sentence(5))
                .RuleFor(x => x.Building, f => f.Address.BuildingNumber());

            return faker.Generate();
        }
    }
}